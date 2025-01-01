using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Rems_Auth.Data;
using Rems_Auth.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rems_Auth.Utilities
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> _userConnections = new(); // Store user connections
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        // Join chat room using chatId
        public async Task JoinChat(Guid chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            Console.WriteLine($"Connection {Context.ConnectionId} joined chat group {chatId}");
        }

        // Send message to both the sender and receiver
        public async Task SendMessage(ChatMessage message)
        {
            // Fetch the chat to ensure it's valid
            var chat = await _context.Chats
                .FirstOrDefaultAsync(c => c.Id == message.ChatId);

            if (chat == null)
            {
                // Handle the case where the chat doesn't exist (can be a 400 Bad Request)
                Console.WriteLine("Chat not found.");
                return;
            }

            // Ensure SenderId is valid and determine ReceiverId
            Guid senderId = message.SenderId;
            Guid receiverId = Guid.Empty;

            // Determine the receiverId based on the chat participants
            if (chat.OwnerId == senderId)
            {
                receiverId = chat.ViewerId; // If owner is sending the message, the receiver is the viewer
            }
            else if (chat.ViewerId == senderId)
            {
                receiverId = chat.OwnerId; // If viewer is sending the message, the receiver is the owner
            }
            else
            {
                Console.WriteLine($"Invalid SenderId: {senderId} for this chat.");
                return;
            }

            // Debugging: Log the senderId and receiverId
            Console.WriteLine($"SenderId: {senderId}, ReceiverId: {receiverId}");

            // Create the message object and save it to the database
            var newMessage = new Message
            {
                ChatId = message.ChatId,
                SenderId = senderId,
                ReceiverId = receiverId, // Ensure the receiverId is properly set
                Content = message.Content,
                Timestamp = message.Timestamp
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Send message to the group associated with chatId
            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);
        }


        // Track connections
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Set this to the user's ID on login
            if (userId != null)
            {
                _userConnections[userId] = Context.ConnectionId;
                Console.WriteLine($"User {userId} connected with connectionId: {Context.ConnectionId}");
            }
            return base.OnConnectedAsync();
        }

        // Handle disconnections
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                _userConnections.Remove(userId);
                Console.WriteLine($"User {userId} disconnected");
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rems_Auth.Data;
using Rems_Auth.Dtos;
using Rems_Auth.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ChatController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartChat([FromBody] StartChatRequest request)
    {
        // Existing chat lookup logic
        var existingChat = _context.Chats
            .FirstOrDefault(c => c.ListingId == request.ListingId &&
                                 c.OwnerId == request.OwnerId &&
                                 c.ViewerId == request.ViewerId);
        Console.WriteLine($"ListingId: {request.ListingId}, OwnerId: {request.OwnerId}, ViewerId: {request.ViewerId}");

        // If a chat already exists, return the chatId
        if (existingChat != null)
        {
            return Ok(new
            {
                chatId = existingChat.Id,
                ownerId = existingChat.OwnerId,
                viewerId = existingChat.ViewerId
            });
        }

        // If no chat exists, create a new one
        var newChat = new Chat
        {
            ListingId = request.ListingId,
            OwnerId = request.OwnerId,
            ViewerId = request.ViewerId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Chats.Add(newChat);
        await _context.SaveChangesAsync();

        // Return chatId, ownerId, and viewerId
        return Ok(new
        {
            chatId = newChat.Id,
            ownerId = newChat.OwnerId,
            viewerId = newChat.ViewerId
        });
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        // Ensure the chat exists
        var chat = await _context.Chats
            .FirstOrDefaultAsync(c => c.Id == request.ChatId);

        if (chat == null)
        {
            return BadRequest("Chat not found.");
        }

        // Determine the sender and receiver
        Guid senderId, receiverId;

        if (chat.OwnerId == request.SenderId)
        {
            // If the owner is sending the message, the receiver is the viewer
            senderId = chat.OwnerId;
            receiverId = chat.ViewerId;
        }
        else if (chat.ViewerId == request.SenderId)
        {
            // If the viewer is sending the message, the receiver is the owner
            senderId = chat.ViewerId;
            receiverId = chat.OwnerId;
        }
        else
        {
            return BadRequest("SenderId does not belong to this chat.");
        }

        // Create the message object
        var message = new Message
        {
            ChatId = request.ChatId,
            SenderId = senderId,
            ReceiverId = receiverId,  // Store receiverId as well
            Content = request.Content,
            Timestamp = DateTime.UtcNow
        };

        // Save the message
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return Ok(new { messageId = message.Id, senderId = senderId, receiverId = receiverId, content = message.Content, timestamp = message.Timestamp });
    }

    [HttpGet("{chatId}/messages")]
    public async Task<IActionResult> GetMessages(Guid chatId)
    {
        // Fetch messages from the database for the specified chatId
        var messages = await _context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.Timestamp) // Ensure messages are ordered by timestamp
            .ToListAsync();

        // Map the messages to a DTO (MessageDto)
        var messageDtos = messages.Select(m => new MessageDto
        {
            SenderId = m.SenderId,
            Content = m.Content,
            Timestamp = m.Timestamp
        }).ToList();

        return Ok(messageDtos); // Return the list of messages
    }

    [HttpGet("user/{userId}/chats")]
    public async Task<IActionResult> GetUserChats(Guid userId)
    {
        // Fetch chats where the user is either the owner or viewer
        var chats = await _context.Chats
            .Where(c => c.OwnerId == userId || c.ViewerId == userId)
            .Select(c => new
            {
                ChatId = c.Id, // Add ChatId explicitly for frontend compatibility
                ListingId = c.ListingId,
                OwnerId = c.OwnerId,
                ViewerId = c.ViewerId,
                LastMessage = _context.Messages
                    .Where(m => m.ChatId == c.Id)
                    .OrderByDescending(m => m.Timestamp)
                    .Select(m => m.Content) // Select Content directly
                    .FirstOrDefault() // Return null if no messages exist
            })
            .ToListAsync();

        return Ok(chats);
    }

    [HttpDelete("{chatId}")]
    public async Task<IActionResult> DeleteChat(Guid chatId)
    {
        // Find the chat by the provided chatId
        var chat = await _context.Chats.FindAsync(chatId);

        // If the chat does not exist, return a 404 Not Found response
        if (chat == null)
        {
            return NotFound("Chat not found.");
        }

        // Remove the chat from the database
        _context.Chats.Remove(chat);

        // Optionally, delete all associated messages
        var messages = await _context.Messages.Where(m => m.ChatId == chatId).ToListAsync();
        _context.Messages.RemoveRange(messages);

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Ok("Chat deleted successfully.");
    }


}

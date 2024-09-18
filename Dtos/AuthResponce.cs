﻿namespace Rems_Auth.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public DateTime Expires { get; set; }  // New property to include expiration date
        public string RefreshToken { get; set; }  // Add the refresh token property
        public DateTime RefreshTokenExpires { get; set; }  // Expiration time
    }
}
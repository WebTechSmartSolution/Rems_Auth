﻿namespace Rems_Auth.Utilities
{
    public class EmailSettings
    {
        public required string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public required string SenderEmail { get; set; }
        public required string SenderPassword { get; set; }
    }
}

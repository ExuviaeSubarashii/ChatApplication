using System;
using System.Collections.Generic;

namespace Chat.Domain.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Username { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? HasPassword { get; set; } = null!;
        public byte[]? Image { get; set; } = null!;
        public string? Server { get; set; } = null!;
    }
}

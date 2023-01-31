using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Models
{
    public partial class Servers
    {
        public int Id { get; set; }
       public string? UserNames { get; set; }
        public string? Channels { get; set; }
        public string? ServerName { get; set; } = null!;
    }
}

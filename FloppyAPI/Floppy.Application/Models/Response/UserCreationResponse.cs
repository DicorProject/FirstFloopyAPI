using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floppy.Application.Models.Response
{
    public class UserCreationResponse
    {
        public int UserId { get; set; }
        public string? Token { get; set; }
    }
}

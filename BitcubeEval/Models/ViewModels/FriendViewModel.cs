using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcubeEval.Models.ViewModels
{
    public class FriendViewModel
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string UserName { get; set; }
        public bool Confirmed { get; set; }
    }
}

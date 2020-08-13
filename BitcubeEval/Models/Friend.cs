using BitcubeEval.Areas.Identity.Data;
using System.Collections.Generic;

namespace BitcubeEval.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public string FriendConnectionId { get; set; }

        public BitcubeUser FriendConnection { get; set; }
    }
}

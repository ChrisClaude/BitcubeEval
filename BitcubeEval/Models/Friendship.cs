using BitcubeEval.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BitcubeEval.Models
{
    public class Friendship
    {
        public string UserId { get; set; }
        public string FriendUserId { get; set; } // this field will allow us to have unique UserId and Friend.UserId combination
        public int FriendId { get; set; }
        [DefaultValue(false)]
        public bool Confirmed { get; set; }
        public BitcubeUser User { get; set; }
        public Friend Friend { get; set; }
        // TODO Friendship must reflect for both users
        // TODO Friendship needs to be confirmed by receiver after the request has been made
        // TODO Use a Confirmed boolean to implement friendship confirmation
    }
}

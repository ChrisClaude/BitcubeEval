using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Identity;

namespace BitcubeEval.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the BitcubeUser class
    public class BitcubeUser : IdentityUser
    {
        [PersonalData]
        public IEnumerable<Friendship> Friendships { get; set; }
        [PersonalData]
        public DateTime? DOB { get; set; }
    }
}

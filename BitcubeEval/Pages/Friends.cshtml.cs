using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcubeEval.Areas.Identity.Data;
using BitcubeEval.Data;
using BitcubeEval.Models;
using BitcubeEval.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BitcubeEval.Pages
{
    [Authorize]
    public class FriendsModel : PageModel
    {

        public FriendsModel(BitcubeContext context, UserManager<BitcubeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public readonly BitcubeContext _context;
        private readonly UserManager<BitcubeUser> _userManager;
        [BindProperty]
        public List<FriendViewModel> Friends { get; set; }
        [BindProperty]
        public List<FriendViewModel> FriendsSuggestions { get; set; }

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            Friends = new List<FriendViewModel>();
            var user = _context.Users.Include(user => user.Friendships)
                .ThenInclude(frienship => frienship.Friend)
                .ThenInclude(friend => friend.FriendConnection)
                .Where(user => user.Id == currentUser.Id)
                .FirstOrDefault();

            FriendsSuggestions = _context.Users.Include(user => user.Friendships)
                .ThenInclude(frienship => frienship.Friend)
                .ThenInclude(friend => friend.FriendConnection)
                .Where(user => user.Id != currentUser.Id)
                .Select(user => new FriendViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                })
                .ToList(); // friends suggestion list must not include the current logged in user

            var receivedFriends = _context.Friendships
                .Include(f => f.User)
                .Where(f => f.FriendUserId == currentUser.Id).ToList();

            if (user != null)
            {
                var friendships = user.Friendships;

                foreach (var friendship in friendships)
                {
                    var friendVM = new FriendViewModel
                    {
                        UserId = friendship.UserId,
                        FriendId = friendship.FriendUserId,
                        UserName = friendship.Friend.FriendConnection.UserName,
                        Confirmed = friendship.Confirmed
                    };

                    Friends.Add(friendVM);
                }
            }

            if (receivedFriends.Count() > 0)
            {
                foreach (var friend in receivedFriends)
                {
                    var friendVM = new FriendViewModel
                    {
                        UserId = friend.UserId, 
                        FriendId = friend.FriendUserId,
                        UserName = friend.User.UserName,
                        Confirmed = friend.Confirmed
                    };
                    Friends.Add(friendVM);
                }
            }


            // this process aims at filtering the FriendsSuggestions list from the friends list of the currently authenticated user
            if (Friends.Count() > 0)
            {
                foreach (var friend in Friends)
                {
                    var myFriend = FriendsSuggestions.Where(f => f.UserId == friend.UserId).FirstOrDefault();
                    
                    if (myFriend != null)
                    {
                        FriendsSuggestions.Remove(friend);
                    }
                }
            }

        }

        public async Task<IActionResult> OnPostAsync(string bitcubeUserId, string currentUserId)
        {
            var bitcubeUser = await _userManager.FindByIdAsync(bitcubeUserId);

            if (bitcubeUser == null)
            {
                return Page();
            }

            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            // a friend user must be different to the current user
            if (currentUser == bitcubeUser)
            {
                return Page();
            }

            // all the bad scenarios are avoided then we add user in friends' list
            var friend = new Friend { FriendConnection = bitcubeUser };
            var friendship = new Friendship
            {
                User = currentUser,
                Friend = friend,
                FriendUserId = friend.FriendConnection.Id,
                Confirmed = false
            };

            await _context.Friends.AddAsync(friend);
            await _context.Friendships.AddAsync(friendship);
            await _context.SaveChangesAsync();

            return RedirectToPage("IndexModel");
        }

        public async Task<IActionResult> OnPostUpdateAsync(string confirmFriendUserId, string currentUserId)
        {
            var friendship = await _context.Friendships.Where(f => f.UserId == confirmFriendUserId && f.FriendUserId == currentUserId).FirstOrDefaultAsync();

            if (friendship == null) 
            {
                // TODO: This should be accompagny with an error message
                return Page();
            }

            friendship.Confirmed = true;

            await _context.SaveChangesAsync();

            // Should pass in data here
            return Page();
        }
    }

    
}
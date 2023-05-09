using ChatManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class FriendshipsController : Controller
    {
        // GET: Friendship
        [OnlineUsers.UserAccess]
        public ActionResult Index()
        {
            return View();
        }
        [OnlineUsers.UserAccess]
        public PartialViewResult Friends(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Friendships.HasChanged || DB.Logins.HasChanged || DB.Users.HasChanged) //Friends has changed methode a ajouter
            {
                return PartialView(DB.Friendships.ToList((int)Session["currentUserId"])); //retourner chaque ami sous forme de liste IENUMERABLE
            }
            return null;
        }
        [OnlineUsers.UserAccess]
        public void FriendRequest(int id)
        {
            var user = new Friendship();
            var targetFriend = new Friendship();
            user.Id = (int)Session["currentUserId"];
            user.targetUserId = id;
            user.friendshipStatus = 2;
            targetFriend.Id = id;
            targetFriend.targetUserId = (int)Session["currentUserId"];
            targetFriend.friendshipStatus = 1;
            DB.Friendships.Add(user);
            DB.Friendships.Add(targetFriend);
        }
    }
}
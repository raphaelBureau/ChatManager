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
            var friendships= DB.Friendships.ToList();
            var idCurrentUser = (int)Session["currentUserId"];
            if (friendships.Where(x => x.IdUser == idCurrentUser && x.targetUserId == id).Count() == 0)
            {
                var user = new Friendship();
                var targetFriend = new Friendship();
                user.IdUser = idCurrentUser;
                user.targetUserId = id;
                user.friendshipStatus = 2;
                targetFriend.IdUser = id;
                targetFriend.targetUserId = idCurrentUser;
                targetFriend.friendshipStatus = 1;
                DB.Friendships.Add(user);
                DB.Friendships.Add(targetFriend);
            }
            
        }
        [OnlineUsers.UserAccess]
        public void AcceptFriendRequest(int id)
        {
            var friendships = DB.Friendships.ToList();
            var idCurrentUser = (int)Session["currentUserId"];
            if (friendships.Where(x => x.IdUser == idCurrentUser && x.targetUserId == id) != null )
            {
                var user = friendships.Where(x => x.IdUser == idCurrentUser && x.targetUserId == id).First();
                var targetFriend = friendships.Where(x => x.targetUserId == idCurrentUser && x.IdUser == id).First();
                user.friendshipStatus = 3;
                targetFriend.friendshipStatus = 3;
                DB.Friendships.Update(user);
                DB.Friendships.Update(targetFriend);
            }
        }
        [OnlineUsers.UserAccess]
        public void DenyFriendRequest(int id)
        {
            var friendships = DB.Friendships.ToList();
            var idCurrentUser = (int)Session["currentUserId"];
            if (friendships.Where(x => x.IdUser == idCurrentUser && x.targetUserId == id) != null)
            {
                var user = friendships.Where(x => x.IdUser == idCurrentUser && x.targetUserId == id).First();
                var targetFriend = friendships.Where(x => x.targetUserId == idCurrentUser && x.IdUser == id).First();
                user.friendshipStatus = 4;
                targetFriend.friendshipStatus = 5;
                DB.Friendships.Update(user);
                DB.Friendships.Update(targetFriend);
            }
        }
        [OnlineUsers.UserAccess]
        public void DeleteFriend(int id)
        {
            var friendships = DB.Friendships.ToList();
            var idCurrentUser = (int)Session["currentUserId"];
            if (friendships.Where(x => x.IdUser == idCurrentUser && x.targetUserId == id) != null)
            {
                var user = friendships.Where(x => x.IdUser == idCurrentUser && x.targetUserId == id).First();
                var targetFriend = friendships.Where(x => x.targetUserId == idCurrentUser && x.IdUser == id).First();
                DB.Friendships.Delete(user.Id);
                DB.Friendships.Delete(targetFriend.Id);
            }
        }
    }
}
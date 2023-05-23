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
            if (forceRefresh || DB.Friendships.HasChanged || DB.Logins.HasChanged || DB.Users.HasChanged)
            {
                List<Friendship> friends = DB.Friendships.ToList((int)Session["currentUserId"]);
                    Func<Friendship, bool> x = (f) =>
                    {

                        string partialName = "";
                        if (Session["text"] != null)
                        {
                            partialName = Session["text"].ToString();
                        }
                        if (f.Blocked)
                        {
                            return (bool)Session["b"] && f.fullName.Contains(partialName);
                        }
                        switch (f.friendshipStatus)
                        {
                            case 0:
                                return (bool)Session["nF"] && f.fullName.Contains(partialName);
                            case 1:
                                return (bool)Session["r"] && f.fullName.Contains(partialName);
                            case 2:
                                return (bool)Session["p"] && f.fullName.Contains(partialName);
                            case 3:
                                return (bool)Session["f"] && f.fullName.Contains(partialName);
                            case 4:
                                return ((bool)Session["refused"] || (bool)Session["nF"]) && f.fullName.Contains(partialName);
                            case 5:
                                return (bool)Session["refused"] && f.fullName.Contains(partialName);
                        }
                        return false;
                    };
                    return PartialView(friends.Where(x));
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
        [HttpGet]
        [OnlineUsers.UserAccess]
        public void Filter(bool nF, bool r, bool p, bool f,bool refused, bool b)
        {
                Session["nF"] = nF;
                Session["r"] = r;
                Session["p"] = p;
                Session["f"] = f;
                Session["refused"] = refused;
                Session["b"] = b;
        }
        [HttpGet]
        [OnlineUsers.UserAccess]
        public void Search(string text = null)
        {
            Session["text"] = text;
        }
    }
}
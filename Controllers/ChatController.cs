using ChatManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class ChatController : Controller
    {
        [OnlineUsers.UserAccess]
        public ActionResult Index()
        {
            return View();
        }
        [OnlineUsers.UserAccess]
        public PartialViewResult Friends(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Friendships.HasChanged)
            {
                List<Friendship> friends = DB.Friendships.ToList((int)Session["currentUserId"]);
                
                return PartialView(friends.Where((f) => f.friendshipStatus == 3));
            }
            return null;
        }
        [OnlineUsers.UserAccess]
        public PartialViewResult Chat(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Chats.HasChanged)
            {
                if (Session["idFriendChatting"] != null)
                {
                    var u = DB.Users.Get((int)Session["idFriendChatting"]);
                    ViewBag.Avatar = u.GetAvatarURL();
                    ViewBag.FullName = u.GetFullName();
                    var c = DB.Chats.GetChatByUsers((int)Session["currentUserId"], u.Id);
                    return PartialView(c);
                }
                ViewBag.Avatar = null;
                ViewBag.FullName = null;
                return PartialView();
            }
            return null;
        }
        [OnlineUsers.UserAccess]
        public void FriendChat(int id)
        {
            Session["idFriendChatting"] = id;
        }
        [OnlineUsers.UserAccess]
        public void SendMessage(int idtargetUser, string message)
        {
            int id = (int)Session["currentUserId"];
            var Chat = DB.Chats.GetChatByUsers(id, idtargetUser);
            if (Chat == null)
            {
                Chat = new Chats() { UserId1 = id, UserId2 = idtargetUser };
                Chat.UserId1Messages.Add((DateTime.UtcNow, message));
                DB.Chats.Add(Chat);
            }
            else
            {
                if (Chat.UserId1 == id)
                {
                    Chat.UserId1Messages.Add((DateTime.UtcNow, message));
                }
                else
                {
                    Chat.UserId2Messages.Add((DateTime.UtcNow, message));
                }
                DB.Chats.Update(Chat);
            }
        }
    }
}
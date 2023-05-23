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
        public PartialViewResult Chat(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Chats.HasChanged)
            {
                // Afficher les messages

            }
            return null;
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
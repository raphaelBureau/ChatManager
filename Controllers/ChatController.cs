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
            if (OnlineUsers.GetSessionUser().IsAdmin)
            {
                return RedirectToAction("Moderation");
            }
            return View();
        }
        [OnlineUsers.AdminAccess]
        public ActionResult Moderation()
        {
            return View();
        }
        [OnlineUsers.AdminAccess]
        public PartialViewResult Conversation(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Chats.HasChanged)
            {
                List<Chats> chats = DB.Chats.ToList();

                ViewBag.AllUser = DB.Users.ToList();

                return PartialView(chats);
            }
            return null;
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
                    ViewBag.Id = Session["currentUserId"];
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
        [OnlineUsers.UserAccess]
        public void DeleteMessage(int idtargetUser, int index)
        {
            var chat = DB.Chats.GetChatByUsers((int)Session["currentUserId"], idtargetUser);
            List<(DateTime date, string message)> message = null;
            if ((int)Session["currentUserId"] == chat.UserId1)
            {
                message = chat.UserId1Messages;
            }
            else
            {
                message = chat.UserId2Messages;
            }
            message.RemoveAt(index);
            DB.Chats.Update(chat);
        }
        [OnlineUsers.UserAccess]
        public void EditMessage(int idtargetUser, string message, int index)
        {
            var chat = DB.Chats.GetChatByUsers((int)Session["currentUserId"], idtargetUser);
            List<(DateTime date, string message)> m = null;
            if ((int)Session["currentUserId"] == chat.UserId1)
            {
                m = chat.UserId1Messages;
            }
            else
            {
                m = chat.UserId2Messages;
            }
            m[index] = (m[index].date, message);
            DB.Chats.Update(chat);
        }
    }
}
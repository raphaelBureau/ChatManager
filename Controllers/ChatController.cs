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
    }
}
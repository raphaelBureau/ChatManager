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
            if (forceRefresh || DB.Friendships.HasChanged) //Friends has changed methode a ajouter
            {
                return PartialView(DB.Friendships.ToList((int)Session["currentUserId"])); //retourner chaque ami sous forme de liste IENUMERABLE
            }
            return null;
        }
        public void AddFriends()
        {

        }
    }
}
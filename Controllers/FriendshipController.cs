using ChatManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class FriendshipController : Controller
    {
        // GET: Friendship
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Friends(bool forceRefresh = false)
        {
            if (forceRefresh) //Friends has changed methode a ajouter
            {
                return PartialView(); //retourner chaque ami sous forme de liste IENUMERABLE
            }
            return null;
        }
    }
}
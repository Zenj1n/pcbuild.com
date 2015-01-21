using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace pcbuild.Controllers
{
    public class PrintLijstController : Controller
    {
        public ActionResult Reload(string voeding, string prijs, string webshop)
        {
            //
            HttpCookie voeding_cookie = new HttpCookie("voeding_cookie");
            HttpCookie voedingprijs_cookie = new HttpCookie("voedingprijs_cookie");
            HttpCookie voedingwebshop_cookie = new HttpCookie("voedingwebshop_cookie");
            
            //voeg data toe aan cookies
            voeding_cookie.Value = voeding;
            voedingprijs_cookie.Value = prijs;
            voedingwebshop_cookie.Value = webshop;            

            //save the cookies!!!
            Response.Cookies.Add(voeding_cookie);
            Response.Cookies.Add(voedingprijs_cookie);
            Response.Cookies.Add(voedingwebshop_cookie);

            return RedirectToAction("Index");
        }
        
        // GET: PrintLijst
        public ActionResult Index()
        {
            HttpCookie voeding_cookie = new HttpCookie("voeding_cookie");
            HttpCookie voedingprijs_cookie = new HttpCookie("voedingprijs_cookie");
            HttpCookie voedingwebshop_cookie = new HttpCookie("voedingwebshop_cookie");

            string prijs = voedingprijs_cookie.Value;
            decimal prijs_opslag = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));

            return View();
        }
    }
}
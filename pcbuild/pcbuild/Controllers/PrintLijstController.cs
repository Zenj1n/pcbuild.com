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
        /// <summary>
        ///  Deze methode maakt, voegt data en slaat de cookies op
        /// </summary>
        /// <param name="voeding"></param>
        /// <param name="prijs"></param>
        /// <param name="webshop"></param>
        /// <returns></returns>
        public ActionResult Reload(string voeding, string prijs, string webshop)
        {
            //Maak cookies aan
            HttpCookie voeding_cookie = new HttpCookie("voeding_cookie");
            HttpCookie voedingprijs_cookie = new HttpCookie("voedingprijs_cookie");
            HttpCookie voedingwebshop_cookie = new HttpCookie("voedingwebshop_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");

            //Roep prijs cookies aan
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];

            //Vereken totale prijs
            decimal prijs_voeding = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value, new CultureInfo("is-IS"));
            decimal prijs_totaal = prijs_totaal_vorige + prijs_voeding;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            Response.Cookies.Add(totale_prijs_cookie);
            
            //voeg data toe aan cookies
            voeding_cookie.Value = voeding;
            voedingprijs_cookie.Value = prijs;
            voedingwebshop_cookie.Value = webshop;            

            //save the cookies!!!
            Response.Cookies.Add(voeding_cookie);
            Response.Cookies.Add(voedingprijs_cookie);
            Response.Cookies.Add(voedingwebshop_cookie);

            //Roep methode Index aan
            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Roep de cookies aan en gebruik het voor de view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Roep cookies aan voor View
            HttpCookie voeding_cookie = new HttpCookie("voeding_cookie");
            HttpCookie voedingprijs_cookie = new HttpCookie("voedingprijs_cookie");
            HttpCookie voedingwebshop_cookie = new HttpCookie("voedingwebshop_cookie");

            // roep de view aan
            return View();
        }
    }
}
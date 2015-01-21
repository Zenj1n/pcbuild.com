using Neo4jClient;
using pcbuild.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections;
using Neo4jClient.Cypher;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using pcbuild.Models.VideokaartModels;
using System.Globalization;


namespace pcbuild.Controllers
{
    public class VideokaartController : Controller
    {
        public ActionResult Reload(string moederbord, string prijs, string webshop, string vormfactor, string ddr)
        {
            //Maak cookie arrays
            HttpCookie moederbord_cookie = new HttpCookie("moederbord_cookie");
            HttpCookie moederbordprijs_cookie = new HttpCookie("moederbordprijs_cookie");
            HttpCookie moederbordwebshop_cookie = new HttpCookie("moederbordwebshop_cookie");

            HttpCookie moederbordvormfactor_cookie = new HttpCookie("moederbordvormfactor_cookie");
            HttpCookie moederbordddr_cookie = new HttpCookie("moederbordddr_cookie");

            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            HttpCookie videokaartprijs_cookie = new HttpCookie("videokaartprijs_cookie");
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];
            videokaartprijs_cookie = Request.Cookies["videokaartprijs_cookie"];

            decimal prijs_moederbord = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));
            decimal prijs_videokaart = Convert.ToDecimal(videokaartprijs_cookie.Value, new CultureInfo("is-IS"));
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value, new CultureInfo("is-IS")) - prijs_videokaart;
            decimal prijs_totaal = prijs_totaal_vorige + prijs_moederbord;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            videokaartprijs_cookie.Value = "0,00";
            Response.Cookies.Add(videokaartprijs_cookie);
            Response.Cookies.Add(totale_prijs_cookie);

            //voeg data toe aan cookies
            moederbord_cookie.Value = moederbord;
            moederbordprijs_cookie.Value = prijs;
            moederbordwebshop_cookie.Value = webshop;
            moederbordvormfactor_cookie.Value = vormfactor;
            moederbordddr_cookie.Value = ddr;

            //save the cookies!!!
            Response.Cookies.Add(moederbord_cookie);
            Response.Cookies.Add(moederbordprijs_cookie);
            Response.Cookies.Add(moederbordwebshop_cookie);
            Response.Cookies.Add(moederbordvormfactor_cookie);
            Response.Cookies.Add(moederbordddr_cookie);

            return RedirectToAction("Index");

        }

        // GET: Videokaart
        public ActionResult Index()
        {
            //Maak cookie arrays
            HttpCookie moederbord_cookie = new HttpCookie("moederbord_cookie");
            HttpCookie moederbordprijs_cookie = new HttpCookie("moederbordprijs_cookie");
            HttpCookie moederbordwebshop_cookie = new HttpCookie("moederbordwebshop_cookie");

            HttpCookie moederbordvormfactor_cookie = new HttpCookie("moederbordvormfactor_cookie");
            HttpCookie moederbordddr_cookie = new HttpCookie("moederbordddr_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");

            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];
            moederbord_cookie = Request.Cookies["moederbord_cookie"];
            moederbordprijs_cookie = Request.Cookies["moederbordprijs_cookie"];
            moederbordwebshop_cookie = Request.Cookies["moederbordwebshop_cookie"];
            moederbordvormfactor_cookie = Request.Cookies["moederbordvormfactor_cookie"];
            moederbordddr_cookie = Request.Cookies["moederbordddr_cookie"];

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle Videokaarten op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:videokaart)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelVideokaart
              {
                  Videokaart_all = n.As<Videokaart_Model>(),
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
              .Results;

            return View(componenten_query);
        }
    }
}
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
using pcbuild.Models.WerkgeheugenModels;
using System.Globalization;

namespace pcbuild.Controllers
{
    public class WerkgeheugenController : Controller
    {
        public ActionResult Reload(string videokaart, string prijs, string webshop)
        {
            //Maak cookie arrays
            HttpCookie videokaart_cookie = new HttpCookie("videokaart_cookie");
            HttpCookie videokaartprijs_cookie = new HttpCookie("videokaartprijs_cookie");
            HttpCookie videokaartwebshop_cookie = new HttpCookie("videokaartwebshop_cookie");

            //voeg data toe aan cookies
            videokaart_cookie.Value = videokaart;
            videokaartprijs_cookie.Value = prijs;
            videokaartwebshop_cookie.Value = webshop;

            //save the cookies!!!
            Response.Cookies.Add(videokaart_cookie);
            Response.Cookies.Add(videokaartprijs_cookie);
            Response.Cookies.Add(videokaartwebshop_cookie);

            return RedirectToAction("Index");
        }
        // GET: Werkgeheugen
        public ActionResult Index()
        {
            HttpCookie videokaart_cookie = new HttpCookie("videokaart_cookie");
            HttpCookie videokaartprijs_cookie = new HttpCookie("videokaartprijs_cookie");
            HttpCookie videokaartwebshop_cookie = new HttpCookie("videokaartwebshop_cookie");

            HttpCookie moederbordddr_cookie = new HttpCookie("moederbordddr_cookie");            
            moederbordddr_cookie = Request.Cookies["moederbordddr_cookie"];

            string prijs = videokaartprijs_cookie.Value;
            decimal prijs_videokaart = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));

            string ddr = moederbordddr_cookie.Value;    //  moederbord ddr voor matchen
            string ddr_search = "(?i).*"+ddr+".*";            

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle behuizingen op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:werkgeheugen)-[r:verkrijgbaar]-(p:Webshop)")
              .Where("n.naam =~ {ddr_query}")
              .WithParam("ddr_query", ddr_search)
              .Return((n, r, p) => new ViewModelWerkgeheugen
              {
                  Werkgeheugen_all = n.As<Werkgeheugen_Model>(),
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
               .Limit(100)
              .Results;

            return View(componenten_query);
        }
    }
}
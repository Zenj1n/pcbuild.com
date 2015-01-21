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
using pcbuild.Models.VoedingModels;
using System.Globalization;

namespace pcbuild.Controllers
{
    public class VoedingController : Controller
    {
        public ActionResult Reload(string opslag, string prijs, string webshop)
        {
            //Maak cookie arrays
            HttpCookie opslag_cookie = new HttpCookie("opslag_cookie");
            HttpCookie opslagprijs_cookie = new HttpCookie("opslagprijs_cookie");
            HttpCookie opslagwebshop_cookie = new HttpCookie("opslagwebshop_cookie");

            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];

            string prijs_2 = totale_prijs_cookie.Value;
            decimal prijs_opslag = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));
            decimal prijs_totaal_vorige = Convert.ToDecimal(prijs_2, new CultureInfo("is-IS"));
            decimal prijs_totaal = prijs_totaal_vorige + prijs_opslag;
            string prijs_totaal_string = prijs_totaal.ToString();

            totale_prijs_cookie.Value = prijs_totaal_string;
            Response.Cookies.Add(totale_prijs_cookie);

            //voeg data toe aan cookies
            opslag_cookie.Value = opslag;
            opslagprijs_cookie.Value = prijs;
            opslagwebshop_cookie.Value = webshop;

            //save the cookies!!!
            Response.Cookies.Add(opslag_cookie);
            Response.Cookies.Add(opslagprijs_cookie);
            Response.Cookies.Add(opslagwebshop_cookie);

            return RedirectToAction("Index");
        }
        // GET: Voeding
        public ActionResult Index()
        {
            HttpCookie opslag_cookie = new HttpCookie("opslag_cookie");
            HttpCookie opslagprijs_cookie = new HttpCookie("opslagprijs_cookie");
            HttpCookie opslagwebshop_cookie = new HttpCookie("opslagwebshop_cookie");

            string prijs = opslagprijs_cookie.Value;
            decimal prijs_opslag = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle behuizingen op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:voeding)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelVoeding
              {
                  Voeding_all = n.As<Voeding_Model>(),
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
              .Results;

            return View(componenten_query);
        }
    }
}
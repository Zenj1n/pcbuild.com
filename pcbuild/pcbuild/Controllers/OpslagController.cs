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
using pcbuild.Models.OpslagModels;
using System.Globalization;

namespace pcbuild.Controllers
{
    public class OpslagController : Controller
    {
        public ActionResult Reload(string behuizing, string prijs, string webshop)
        {
            //Maak cookie arrays
            HttpCookie behuizing_cookie = new HttpCookie("behuizing_cookie");
            HttpCookie behuizingprijs_cookie = new HttpCookie("behuizingprijs_cookie");
            HttpCookie behuizingwebshop_cookie = new HttpCookie("behuizingwebshop_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            HttpCookie opslagprijs_cookie = new HttpCookie("opslagprijs_cookie");

            //Roep cookies aan
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];
            opslagprijs_cookie = Request.Cookies["opslagprijs_cookie"];

            //Verreken totale prijs
            decimal prijs_behuizing = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));
            decimal prijs_opslag = Convert.ToDecimal(opslagprijs_cookie.Value, new CultureInfo("is-IS"));
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value, new CultureInfo("is-IS")) - prijs_opslag;
            decimal prijs_totaal = prijs_totaal_vorige + prijs_behuizing;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            opslagprijs_cookie.Value = "0,00";
            Response.Cookies.Add(opslagprijs_cookie);
            Response.Cookies.Add(totale_prijs_cookie);

            //voeg data toe aan cookies
            behuizing_cookie.Value = behuizing;
            behuizingprijs_cookie.Value = prijs;
            behuizingwebshop_cookie.Value = webshop;

            //save the cookies!!!
            Response.Cookies.Add(behuizing_cookie);
            Response.Cookies.Add(behuizingprijs_cookie);
            Response.Cookies.Add(behuizingwebshop_cookie);

            return RedirectToAction("Index");
        }
        // GET: Opslag
        public ActionResult Index()
        {
            //Maak cookie arrays
            HttpCookie behuizing_cookie = new HttpCookie("behuizing_cookie");
            HttpCookie behuizingprijs_cookie = new HttpCookie("behuizingprijs_cookie");
            HttpCookie behuizingwebshop_cookie = new HttpCookie("behuizingwebshop_cookie");

            string prijs = behuizingprijs_cookie.Value;
            decimal prijs_behuizing = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));

            //Connectie met database
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();

            // Query om opslag op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:opslag)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p, z) => new ViewModelOpslag
              {
                  Opslag_m = n.As<Opslag_Model>(),                  
                  Verkrijgbaar_m = r.As<Verkrijgbaar_Model>(),
                  Webshop_m = p.As<Webshop_Model>(),
              })
              .Results;

            return View(componenten_query);
        }
    }
}
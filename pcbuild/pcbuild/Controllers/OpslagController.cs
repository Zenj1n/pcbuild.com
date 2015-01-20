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

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle behuizingen op te halen
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
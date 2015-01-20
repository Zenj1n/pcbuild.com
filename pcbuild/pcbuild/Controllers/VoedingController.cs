﻿using Neo4jClient;
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
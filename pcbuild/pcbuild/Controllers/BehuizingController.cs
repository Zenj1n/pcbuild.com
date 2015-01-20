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

namespace pcbuild.Controllers
{
    public class BehuizingController : Controller
    {
        // GET: Behuizing
        public ActionResult Index(string werkgeheugen, string prijs, string webshop)
        {
            //Maak cookie arrays
            HttpCookie werkgeheugen_cookie = new HttpCookie("werkgeheugen_cookie");
            HttpCookie werkgeheugenprijs_cookie = new HttpCookie("werkgeheugenprijs_cookie");
            HttpCookie werkgeheugenwebshop_cookie = new HttpCookie("werkgeheugenwebshop_cookie");

            HttpCookie moederbordvormfactor_cookie = new HttpCookie("moederbordvormfactor_cookie");
            moederbordvormfactor_cookie = Request.Cookies["moederbordvormfactor_cookie"];

            string vormfactor = moederbordvormfactor_cookie.Value;  // moederbord vormfactor voor matchen
            string vormfactor_search = "(?i).*" + vormfactor[0] + ".*";

            //voeg data toe aan cookies
            werkgeheugen_cookie.Value = werkgeheugen;
            werkgeheugenprijs_cookie.Value = prijs;
            werkgeheugenwebshop_cookie.Value = webshop;

            //save the cookies!!!
            Response.Cookies.Add(werkgeheugen_cookie);
            Response.Cookies.Add(werkgeheugenprijs_cookie);
            Response.Cookies.Add(werkgeheugenwebshop_cookie);

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle behuizingen op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:behuizing)-[r:verkrijgbaar]-(p:Webshop)")
              .Where("n.naam =~ {vormfactor_c}")
              .WithParam("vormfactor_c", vormfactor_search)
              .Return((n, r, p) => new ViewModelBehuizing
              {
                  Behuizing_test = n.As<Behuizing_Model>(),
                  Verkrijgbaar_test = r.As<Verkrijgbaar_Model>(),
                  Webshop_test = p.As<Webshop_Model>(),
              })
              .Results;

                return View(componenten_query);
            }

        }
}

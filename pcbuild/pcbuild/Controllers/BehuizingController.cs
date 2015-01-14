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
        public ActionResult Index()
        {        
            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle behuizingen op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:behuizing)-[r:verkrijgbaar]-(p:Webshop)")
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

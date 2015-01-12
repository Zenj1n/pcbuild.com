using Neo4jClient;
using pcbuild.Models;
using pcbuild.Models.ProcessorModels;
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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ProcessorStart()
        {

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle behuizingen op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:processor)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelProcessor
              {
                  Proccesor_m = n.As<Processor_Model>(),
                  Verkrijgbaar_m = r.As<Verkrijgbaar_Model>(),
                  Webshop_m = p.As<Webshop_Model>(),
              })
              .Results;

            return View(componenten_query);
        }
    }
}
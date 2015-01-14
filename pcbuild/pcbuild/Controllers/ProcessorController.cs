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
    public class ProcessorController : Controller
    {
        // GET: Processor
        public ActionResult Index()
        {
            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle processoren op te halen
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

            Debug.WriteLine(componenten_query);

            return View(componenten_query);
        }
    }
}
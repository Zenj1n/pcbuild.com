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
using pcbuild.Models.HardDiskModels;
using pcbuild.Models.SSDModels;

namespace pcbuild.Controllers
{
    public class HardDiskController : Controller
    {
        // GET: HardDisk
        public ActionResult Index()
        {
            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle behuizingen op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:hd)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelHardDisk
              {
                  Harddisk_all = n.As<Harddisk_Model>(),                  
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
              .Results;

            var componenten_query2 = client
               .Cypher
              .Match("(n:ssd)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelSSD
              {
                  SSD_all = n.As<SSD_Model>(),
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
              .Results;

           // var AllOpslagModel =  new Opslag_Model(componenten_query,componenten_query2)

            Opslag_Model test = new Opslag_Model
            {
            };

            return View(test);
        }
    }
}
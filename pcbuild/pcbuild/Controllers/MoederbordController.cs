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
using pcbuild.Models.MoederbordModels;

namespace pcbuild.Controllers
{
    public class MoederbordController : Controller
    {
        // GET: Moederbord
        public ActionResult Index(string processor, string socket, string prijs)
        {

            Debug.WriteLine(processor);
            Debug.WriteLine(socket);
            Debug.WriteLine(prijs);



            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle Moederborden op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:moederbord)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelMoederbord
              {
                  Moederbord_all = n.As<Moederbord_Model>(),
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
              .Results;
            
            return View(componenten_query);
        }
    }
}
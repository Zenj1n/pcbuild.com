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

            //Query om alle componenten opte halen
            var componenten_query = client
                .Cypher
                .Match("(n:behuizing)")
                .Return(n => n.As<Behuizing_Model>())
                .Results;

            //Test om te kijken of er data in de model zit
            foreach (var item in componenten_query)
            {
                if (item.naam == null)
                {
                    Debug.WriteLine("Geen data");
                }
                else
                {

                    Debug.WriteLine("1", item.naam);
                    Debug.WriteLine("2", item.interfaces);
                    Debug.WriteLine("3", item.vormfactor);
                    Debug.WriteLine("4", item.vormvoeding);
                }

            }
            return View(componenten_query);
        }
    }
}
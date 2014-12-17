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
    public class ComponentenController : Controller
    {
      
        public ActionResult Index()
        {
            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            //Query om alle componenten opte halen
            var componenten_query = client
                .Cypher
                .Match("(n)")
                .Return(n => n.As<All_Components>())
                .Results;

            //Test om te kijken of er data in de model zit
            foreach (var item in componenten_query)
            {
                if (item.name == null)
                {
                    Debug.WriteLine("Geen data");
                }
                else
                {

                    Debug.WriteLine("1", item.name);
                    Debug.WriteLine("2", item.desc);
                    Debug.WriteLine("3", item.url);
                    Debug.WriteLine("4", item.price);
                    Debug.WriteLine("5", item.webshop);
                    Debug.WriteLine("6", item.component);
                }
               
            }
                return View(componenten_query);
        }
    }
}
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
              .Return((n, r, p) => new
              {
                 n = n.As<Behuizing_Model>(),
                 r = r.As<Verkrijgbaar_Model>(),
                 p = p.As<Webshop_Model>(),
              })
              .Results;

            var viewModel = new ViewModel()
            {
                
            };
            
            List<ViewModel> viewModelList = new List<ViewModel>();
            viewModelList.Add(viewModel);
                

            //var componenten_query = client
            //    .Cypher
            //    .Match("(n:behuizing)")
            //    .Return(n => n.As<Behuizing_Model>())
            //    .Results;

            //Test om te kijken of er data in de model zit
            foreach (var item in componenten_query)
            {
                if (item.n.naam == null)
                {
                    Debug.WriteLine("Geen data");
                }
                else
                {

                    Debug.WriteLine("1", item.n.naam);
                    Debug.WriteLine("2", item.n.interfaces);
                    Debug.WriteLine("3", item.n.vormfactor);
                    Debug.WriteLine("4", item.n.vormvoeding);
                    Debug.WriteLine("6", item.r.prijs);
                }

            }
            return View(componenten_query);
        }
    }
}
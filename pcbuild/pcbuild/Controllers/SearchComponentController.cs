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
using pcbuild.Models.SearchModels;

namespace pcbuild.Controllers
{
    public class SearchComponentController : Controller
    {

        // GET: SearchComponent
        public ActionResult Index()
        {               
            return View();
        }

        public ActionResult Search(string name)
        //Deze methode zorgt ervoor dat cookies worden gemaakt
        //en strings van de vorige stap worden dan opgeslagen in de cookies
        //en in de volgende methode de cookies worden aangeroepen voor de view
        {
            // Query eis
            string search_query = "(?i).*" + name + ".*";

            //Maak verbinding met database
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();

            //Voer query uit m.b.v neo4jclient en zet het in een variable
            var componenten_query = client
             .Cypher
             .Match("(n)-[r:verkrijgbaar]-(p:Webshop)")
             .Where((Webshop_Model p) => p.naam == "alternate.nl" || p.naam == "Informatique")
             .AndWhere("n.naam =~ {search_var}")
             .WithParam("search_var", search_query)
             .Return((n, r, p) => new ViewModelSearch
             {
                 Search_m = n.As<Search_Model>(),
                 Verkrijgbaar_m = r.As<Verkrijgbaar_Model>(),
                 Webshop_m = p.As<Webshop_Model>(),
             })
             .Results;

            //Stuur data naar de view
            return View(componenten_query);
        }
    }
}
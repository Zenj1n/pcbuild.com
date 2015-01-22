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
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();

            //Query om alle componenten opte halen
            var componenten_query = client
                .Cypher
                .Match("(n)")
                .Return(n => n.As<All_Components>())
                .Results;

                return View(componenten_query);
        }
    }
}
using Neo4jClient;
using pcbuild.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections;

namespace pcbuild.Controllers
{
    public class ComponentenController : Controller
    {

        public ActionResult Index()
        {
            var componenten = new List<All_Components>();
            ArrayList componenten2 = new ArrayList();
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
                client.Connect();

                //var test = client.Cypher
                //    .Match("(Movie:Movie)")
                //    .Return(Movie => Movie.As<Movie>());
                //var test2 = test.Results;
                //Console.WriteLine(test2);
            
                //var Test = client
                //    .Cypher
                //    .Match("(Movie:Movie")
                //    .Return<Node<Movie>>("Movie")
                //    .Results;
                //Console.WriteLine(Movie.title);
                //Console.ReadLine();
                for (int i = 0; i < 100; i++){
                    componenten2.Add("yo");
                }

                // Code als connectie met database werkt moet nog met Rinesh bespreken hoe.
                //foreach (var all_component in componenten)
                //{
                //    var component = new All_Components();

                //    component.All_Components_Name = ;
                //    component.All_Components_Price = ;

                //    componenten.Add(component);
                //}

                foreach (var all_component in componenten2)
                {
                    var component = new All_Components();

                    component.All_Components_Name = "WDD GREEN 2TB";
                    component.All_Components_Price = 90;

                    componenten.Add(component);
                }
                     
            
            return View(componenten);
        }
    }
}
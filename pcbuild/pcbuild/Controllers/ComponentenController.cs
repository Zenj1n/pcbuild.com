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
            var componenten = new List<All_Components>();
            ArrayList componenten2 = new ArrayList();
            //All_Components x = new All_Components();

            WebClient c = new WebClient();
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
                client.Connect();


                try
                {
                    var componenten_query = client
                        .Cypher
                        .Match("(n)")
                        .Return(n => n.As<All_Components>())
                        ;

                    var test = componenten_query.Results;
                   // Debug.WriteLine(test.ToList);

                    foreach (var item in test.ToList())
                    {
                        Debug.WriteLine("1", item.All_Components_Name);
                    }

                    //ToString(x.All_Components_Name);                    
                    //Debug.WriteLine(x.All_Components_Name);
                    Debug.WriteLine("test_string");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                for (int i = 0; i < 100; i++){
                    componenten2.Add("yo");
                }

                //foreach (var all_component in componenten2)
                //{
                //    var component = new All_Components();

                //    component.All_Components_Name = "WDD GREEN 2TB";
                //    component.All_Components_Price = 90;

                //    componenten.Add(component);
                //}
                     
            
            return View(componenten);
        }
    }
}
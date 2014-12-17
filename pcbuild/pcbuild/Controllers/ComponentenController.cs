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

            var componenten_query = client
                .Cypher
                .Match("n")
                .Return<All_Components>("n");
                ;

            var test = componenten_query.Results.ToList();

                //var test = componenten_query.Results;
                   // Debug.WriteLine(test.ToList);

            foreach (var item in test)
            {
                Debug.WriteLine("1", item.All_Components_Name);
                Debug.WriteLine("2", item.All_Components_Desc);
                Debug.WriteLine("3", item.All_Components_URL);
                Debug.WriteLine("4", item.All_Components_Price);
                Debug.WriteLine("5", item.All_Components_Webshop);
                Debug.WriteLine("6", item.All_Components_Component);
            }

                //foreach (var item in test)
                //{
                //Debug.WriteLine("1",item.All_Components_Name.ToString());
                //Debug.WriteLine("2", item.All_Components_Desc.ToString());
                //Debug.WriteLine("3", item.All_Components_URL.ToString());
                //Debug.WriteLine("4", item.All_Components_Price.ToString());
                //Debug.WriteLine("5", item.All_Components_Webshop.ToString());
                //Debug.WriteLine("6", item.All_Components_Component.ToString());
                //}
                Debug.WriteLine("test_string");

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
                return View();
        }
    }
}
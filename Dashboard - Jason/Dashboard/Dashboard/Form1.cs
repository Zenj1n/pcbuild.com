using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neo4jClient.Cypher;
using Neo4jClient;
using System;
using System.IO;
using System.Globalization;

namespace Dashboard
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            int aantalCPU = query_aantal("processor");
            int aantalGPU = query_aantal("videokaart");
            int aantalPSU = query_aantal("voeding");
            int aantalRAM = query_aantal("werkgeheugen");
            int aantalCase = query_aantal("behuizing");
            int aantalOpslag = query_aantal("opslag");
            int aantalMoederbord = query_aantal("moederbord");
            int Totaal = aantalCPU + aantalGPU + aantalPSU + aantalRAM + aantalCase + aantalOpslag + aantalMoederbord;

            decimal totaalprijsCPU = query_prijzen("processor").Item1;
            decimal totaalprijsGPU = query_prijzen("videokaart").Item1;
            decimal totaalprijsPSU = query_prijzen("voeding").Item1;
            decimal totaalprijsRAM = query_prijzen("werkgeheugen").Item1;
            decimal totaalprijsCase = query_prijzen("behuizing").Item1;
            decimal totaalprijsOpslag = query_prijzen("opslag").Item1;
            decimal totaalprijsMoederbord = query_prijzen("moederbord").Item1;

            decimal avgprijsCPU = query_prijzen("processor").Item2;
            decimal avgprijsGPU = query_prijzen("videokaart").Item2;
            decimal avgprijsPSU = query_prijzen("voeding").Item2;
            decimal avgprijsRAM = query_prijzen("werkgeheugen").Item2;
            decimal avgprijsCase = query_prijzen("behuizing").Item2;
            decimal avgprijsOpslag = query_prijzen("opslag").Item2;
            decimal avgprijsMoederbord = query_prijzen("moederbord").Item2;

            InitializeComponent();
            Aantal_onderdelen_chart.Series["CPU"].Points.AddY(aantalCPU);
            Aantal_onderdelen_chart.Series["GPU"].Points.AddY(aantalGPU);
            Aantal_onderdelen_chart.Series["PSU"].Points.AddY(aantalPSU);
            Aantal_onderdelen_chart.Series["RAM"].Points.AddY(aantalRAM);
            Aantal_onderdelen_chart.Series["Case"].Points.AddY(aantalCase);
            Aantal_onderdelen_chart.Series["Opslag"].Points.AddY(aantalOpslag);
            Aantal_onderdelen_chart.Series["Moederbord"].Points.AddY(aantalMoederbord);
            Aantal_onderdelen_chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;

            Avg_Prijzen_chart.Series["CPU"].Points.AddY(avgprijsCPU);
            Avg_Prijzen_chart.Series["GPU"].Points.AddY(avgprijsGPU);
            Avg_Prijzen_chart.Series["PSU"].Points.AddY(avgprijsPSU);
            Avg_Prijzen_chart.Series["RAM"].Points.AddY(avgprijsRAM);
            Avg_Prijzen_chart.Series["Case"].Points.AddY(avgprijsCase);
            Avg_Prijzen_chart.Series["Opslag"].Points.AddY(avgprijsOpslag);
            Avg_Prijzen_chart.Series["Moederbord"].Points.AddY(avgprijsMoederbord);
            Avg_Prijzen_chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;


            Totale_Prijzen_Chart.Series["CPU"].Points.AddY(totaalprijsCPU);
            Totale_Prijzen_Chart.Series["GPU"].Points.AddY(totaalprijsGPU);
            Totale_Prijzen_Chart.Series["PSU"].Points.AddY(totaalprijsPSU);
            Totale_Prijzen_Chart.Series["RAM"].Points.AddY(totaalprijsRAM);
            Totale_Prijzen_Chart.Series["Case"].Points.AddY(totaalprijsCase);
            Totale_Prijzen_Chart.Series["Opslag"].Points.AddY(totaalprijsOpslag);
            Totale_Prijzen_Chart.Series["Moederbord"].Points.AddY(totaalprijsMoederbord);
            Totale_Prijzen_Chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;


            textBox3.Text = "Totaal: " + Totaal.ToString();
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
                       

        }

        public int query_aantal(string component)
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();
            var aantal_query = client
            .Cypher
            .Match("(n:" + component + ")")
            .Return(n => new
                {
                    Aantal = (int)n.Count()
                })
           .Results
           .ToArray();
            int Aantal = aantal_query[0].Aantal;
            return Aantal;
        }

        public Tuple<decimal, decimal> query_prijzen(string component)
        {
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
            client.Connect();
            var prijzen_query = client
            .Cypher
            .Match("(n:" + component + ")-[r:verkrijgbaar]-(m:Webshop)")
            .Return(r => new RelationshipViewModel
            {
                relationshipObj = r.As<RelationshipObj>()
            })
           .Results
           .ToArray();
            decimal total_prijs = 0;
            decimal avg_prijs = 0;

            for (int i = 0; i < prijzen_query.Length; i++)
            {
                total_prijs = total_prijs + Convert.ToDecimal(prijzen_query[0].relationshipObj.prijs, new CultureInfo("is-IS"));
            }

            avg_prijs = total_prijs / prijzen_query.Length;
            return Tuple.Create(total_prijs, avg_prijs);
        }
    }
    
}

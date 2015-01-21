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

namespace Dashboard
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            int aantalCPU = query_aantal("processor");
            int aantalGPU = query_aantal("videokaart");
            int aantalPSU = query_aantal("voeding");
            int aantalRAM = query_aantal("geheugen");
            int aantalCase = query_aantal("behuizing");
            int aantalOpslag = query_aantal("opslag");
            int aantalMoederbord = query_aantal("moederbord");
            int Totaal = aantalCPU + aantalGPU + aantalPSU + aantalRAM + aantalCase + aantalOpslag + aantalMoederbord;

            InitializeComponent();
            Aantal_onderdelen_chart.Series["CPU"].Points.AddY(aantalCPU);
            Aantal_onderdelen_chart.Series["GPU"].Points.AddY(aantalGPU);
            Aantal_onderdelen_chart.Series["PSU"].Points.AddY(aantalPSU);
            Aantal_onderdelen_chart.Series["RAM"].Points.AddY(aantalRAM);
            Aantal_onderdelen_chart.Series["Case"].Points.AddY(aantalCase);
            Aantal_onderdelen_chart.Series["Opslag"].Points.AddY(aantalOpslag);
            Aantal_onderdelen_chart.Series["Moederbord"].Points.AddY(aantalMoederbord);
            Aantal_onderdelen_chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
            textBox3.Text = Totaal.ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
                       

        }

        public int query_aantal(string component)
        {
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
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
    }
    
}

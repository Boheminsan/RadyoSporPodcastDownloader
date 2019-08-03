using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RdSpPodcast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link = "http://www.radyospor.com/Podcast";
            indir(link);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void indir(string link)
        {
            try
            {
                Uri url = new Uri(link);
                WebClient client = new WebClient();
                client.Encoding = Encoding.Default;
                string html = client.DownloadString(url); // html kodları indiriyoruz
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                listBox1.Items.Clear();
                document.LoadHtml(html); // html kodlarını bir HtmlDocment nesnesine yüklüyoruz.
                //HtmlNodeCollection basliklar = document.DocumentNode.SelectNodes("//p");           
                //div[contains(@id, 'haberTextDiv')]
                HtmlNodeCollection basliklar = document.DocumentNode.SelectNodes("//div[contains(@id, 'podcast_icerik')]//div[contains(@class, 'podcast_programlar')]//div[contains(@class, 'programlar_container')]//div[contains(@class, 'podcast_programi')]//span[contains(@class,'podcast_indir')]//a");
                var sb = new StringBuilder();
                var liste = new List<string>();
                foreach (HtmlNode baslik in basliklar)
                {
                    string hmetin = baslik.GetAttributeValue("href", string.Empty);
                    liste.Add("http://www.radyospor.com" + hmetin.Substring(2, hmetin.Length - 2));
                }
                liste.Reverse();
                foreach (var item in liste)
                {
                    listBox1.Items.Add(item);
                }
            }
            catch (WebException f)
            {
                MessageBox.Show(f.ToString());
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thr = new Thread(Download);
            thr.Start();
        }

        private void Download()
        {
            string root = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + "\\RS";
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            else
            {
                WebClient webClient = new WebClient();
                foreach (var podc in listBox1.Items)
                {
                    string[] boluk = podc.ToString().Split('/');
                    //MessageBox.Show(root + "\\" + boluk[6]);
                    //MessageBox.Show(podc.ToString());
                    if (File.Exists(root + "\\" + boluk[6]))
                    {
                        continue;
                    }
                    else
                    {
                        if (boluk[6].StartsWith("Kriminal-Spor"))
                        {
                            continue;
                        }
                        else
                        {
                            webClient.DownloadFile(podc.ToString(), root + "\\" + boluk[6]);
                        }
                    }
                }
            }
        }
    }
}

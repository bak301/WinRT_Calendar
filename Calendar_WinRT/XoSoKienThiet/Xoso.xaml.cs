using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;
using Newtonsoft.Json;
using System.Net.Http;
using Windows.Data.Json;
using System.Globalization;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Calendar_WinRT.XoSoKienThiet
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Xoso : Page
    {
        List<SyndicationItem> listRSSItem;
        Dictionary<string, string> downloadRegions;
        Dictionary<string, string> listDate;
        public Xoso()
        {
            this.InitializeComponent();
            //SyndicationFeed sf = new SyndicationFeed();
            //HttpWebResponse rs = (HttpWebResponse)await WebRequest.Create("http://vnexpress.net/rss/tin-moi-nhat.rss").GetResponseAsync();
            //sf.Load(new StreamReader(rs.GetResponseStream()).ReadToEnd());
            
            listDate = new Dictionary<string, string>();
            lblOpenDate.Text = "Hôm nay ngày " + DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            getJsonData();
        }
        
        private async void getJsonData()
        {
            
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri("http://210.245.85.214/Regions.json"));
            var jsonString = await response.Content.ReadAsStringAsync();
            JsonArray arrayRegion = JsonValue.Parse(jsonString).GetArray();
            downloadRegions = new Dictionary<string, string>();
            for (uint i = 0; i < arrayRegion.Count; i++)
            {
                string name1 = arrayRegion.GetObjectAt(i).GetNamedString("name");
                string link1 = arrayRegion.GetObjectAt(i).GetNamedString("link");
                Regions r = new Regions();
                r.name = name1;
                r.link = link1;
                downloadRegions.Add(r.name, r.link);
            }
            boxRegion.ItemsSource=downloadRegions.Keys;
        }
        private void boxRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridView.Items.Clear();
            int id = boxRegion.SelectedIndex;
            getDataFromURL(downloadRegions.Values.ElementAt(id));
        }
        private async void getDataFromURL(string url)
        {
            //SyndicationFeed feed = new SyndicationFeed();
            SyndicationClient client = new SyndicationClient();
            SyndicationFeed feed = await client.RetrieveFeedAsync(new Uri(url));
            listRSSItem = new List<SyndicationItem>();

            foreach (SyndicationItem item in feed.Items)
            {
                gridView.Items.Add(item.Title.Text+"\n"+item.Summary.Text);
                
                string tmp = item.Title.Text;
                string[] date = tmp.Split(' ');
                string currentDate = date[7];
                string[] processDate = currentDate.Split('/');
                string addDate = processDate[0] + '/' + processDate[1] + '/' + "2016";
                
                //listDate.Add(addDate, item.Summary.Text);
                listRSSItem.Add(item);
                
                if (listDate.ContainsKey(addDate))
                {
                    listDate.Clear();
                } else
                {
                    listDate.Add(addDate, item.Summary.Text);
                }

            }
            lastest.Items.Clear();
            lastest.Items.Add(listRSSItem.First().Title.Text+'\n'+listRSSItem.First().Summary.Text);
        }
        
        
        private async void calendarView_CurrentDateChanged(object sender, EventArgs e)
        {
            DateTime dt = Convert.ToDateTime(calendarView.CurrentDate);
            string gotDate = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (listDate.ContainsKey(gotDate))
            {

                gridView.Items.Clear();
                string val = "";
                if (listDate.TryGetValue(gotDate, out val))
                {
                    lblOpenDate.Text = "Kết quả ngày " + gotDate;
                    gridView.Items.Add(val);
                }

            }
            else
            {
                MessageDialog ms = new MessageDialog("Expired Document");
                await ms.ShowAsync();
            }
        }

        private void back_Click(object sender, RoutedEventArgs e) {
            if (this.Frame != null) {
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}

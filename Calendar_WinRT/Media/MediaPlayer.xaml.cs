using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Windows.UI.Popups;
using Newtonsoft.Json;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MediaPlayer : Page
    {
        public MediaPlayer()
        {

            this.InitializeComponent();
            ImageBrush IBbackground = new ImageBrush();
            BitmapImage bmpBack = new BitmapImage(new Uri("ms-appx:///Assets/backG.jpg"));
            IBbackground.ImageSource = bmpBack;
            grBackGround.Background = IBbackground;

            ImageBrush IBback01 = new ImageBrush();
            BitmapImage bmpBack01 = new BitmapImage(new Uri("ms-appx:///Assets/back01.jpg"));
            IBback01.ImageSource = bmpBack01;
            grBack01.Background = IBback01;

            ImageBrush IBback02 = new ImageBrush();
            BitmapImage bmpBack02 = new BitmapImage(new Uri("ms-appx:///Assets/back2.jpg"));
            IBback02.ImageSource = bmpBack02;
            grBack02.Background = IBback02;

            ImageBrush IBback03 = new ImageBrush();
            BitmapImage bmpBack03 = new BitmapImage(new Uri("ms-appx:///Assets/back3.jpg"));
            IBback03.ImageSource = bmpBack03;
            grBack03.Background = IBback03;

            cbSite.Items.Add("mp3.zing.vn");
            cbSite.Items.Add("nhaccuatui.com");
        }

        private List<Musics> listMusics;

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            using (var httpClient = new HttpClient())
            {

                string nameMusic = txtSearch.Text;
                nameMusic = nameMusic.Replace(" ", "%");
                string url = "";                

                if (cbSite.SelectedIndex == -1)
                {
                    url = "http://j.ginggong.com/jOut.ashx?k=" + nameMusic + "&code=ab16afcf-2e81-4265-8536-d2e856fc57b8";
                }
                else
                {
                    url = "http://j.ginggong.com/jOut.ashx?k=" + nameMusic +"&h="+cbSite.SelectedItem.ToString()+"&code=ab16afcf-2e81-4265-8536-d2e856fc57b8";
                }

                string jsonString = await httpClient.GetStringAsync(url);
                listMusics = JsonConvert.DeserializeObject<List<Musics>>(jsonString);
                lstItems.ItemsSource = listMusics;
            }
        }

        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Musics ms = lstItems.SelectedItem as Musics;

            if (ms != null)
            {
                ImageBrush IBback03 = new ImageBrush();
                BitmapImage bmpBack03 = new BitmapImage(new Uri(ms.Avatar));
                IBback03.ImageSource = bmpBack03;
                grBack03.Background = IBback03;

                string[] nameAndAtist = ms.Title.Split(new[] {" - "},StringSplitOptions.None);
                txtNameMusic.Text = nameAndAtist[0];
                txtAtist.Text = nameAndAtist[1];
                


                string url = ms.UrlJunDownload;
                myMedia.Source = new Uri(url, UriKind.Absolute);

                //MessageDialog msg = new MessageDialog(url);
                //msg.ShowAsync();

            }
            
        }




    }
}

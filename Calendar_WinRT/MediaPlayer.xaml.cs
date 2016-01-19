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
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Calendar_WinRT.MediaPlay;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Calendar_WinRT.MediaPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MediaPlayer : Page
    {
        private Boolean online = true;
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

            myMedia.MediaEnded += media_MediaEnded;
        }

        private List<Musics> listMusics;

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            online = true;
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
                foreach (Musics ms in listMusics)
                {
                    ms.Name = ms.Title;
                }
                lstItems.ItemsSource = listMusics;

            }
        }

        private async void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (online == true)
            {
                Musics ms = lstItems.SelectedItem as Musics;

                if (ms != null)
                {
                    ImageBrush IBback03 = new ImageBrush();
                    BitmapImage bmpBack03 = new BitmapImage(new Uri(ms.Avatar));
                    IBback03.ImageSource = bmpBack03;
                    grBack03.Background = IBback03;

                    string[] nameAndAtist = ms.Title.Split(new[] { " - " }, StringSplitOptions.None);
                    txtNameMusic.Text = nameAndAtist[0];
                    if (nameAndAtist.Length > 1)
                    {
                        txtAtist.Text = nameAndAtist[1];
                    }

                    string url = ms.UrlJunDownload;
                    myMedia.Source = new Uri(url, UriKind.Absolute);

                    //MessageDialog msg = new MessageDialog(url);
                    //msg.ShowAsync();

                }
            }
            else
            {
                StorageFile file = lstItems.SelectedItem as StorageFile;
                if (file != null)
                {
                    IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                    myMedia.SetSource(stream, file.ContentType);
                    myMedia.Play();
                    index = lstItems.SelectedIndex;


                    txtNameMusic.Text = file.Name;

                    ImageBrush IBback03 = new ImageBrush();
                    BitmapImage bmpBack03 = new BitmapImage(new Uri("ms-appx:///Assets/back3.jpg"));
                    IBback03.ImageSource = bmpBack03;
                    grBack03.Background = IBback03;

                    
                }
            }
            
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            myMedia.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            myMedia.Pause();
        }

        private int index = 0;
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (index < this.lstItems.Items.Count - 1)
            {
                index++;
                lstItems.SelectedIndex = index;
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (index > 0)
            {
                index--;
                lstItems.SelectedIndex = index;
            }
        }

        private async void btnPrevew_Click(object sender, RoutedEventArgs e)
        {
            online = false;
            lstItems.ItemsSource = null;
            lstItems.Items.Clear();

            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".avi");
            picker.FileTypeFilter.Add(".mp3");
            IReadOnlyList<StorageFile> list = await picker.PickMultipleFilesAsync();
            foreach (StorageFile item in list)
            {                

                if (CheckExist(item))
                {
                    lstItems.Items.Add(item);
                }
            }
        }



        private bool CheckExist(StorageFile item)
        {
            foreach (var i in lstItems.Items)
            {
                StorageFile f = (StorageFile)i;
                if (f.Name.Equals(item.Name))
                    return false;
            }
            return true;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void turnDownVol(object sender, RoutedEventArgs e)
        {
            if (myMedia.Volume > 0.1)
            {
                myMedia.Volume = myMedia.Volume - 0.1;
            }
        }


        private void turnUpVol(object sender, RoutedEventArgs e)
        {     
            
            if (myMedia.Volume < 1)
            {
                myMedia.Volume = myMedia.Volume + 0.1;
            }
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (index < this.lstItems.Items.Count - 1)
            {
                index++;
                lstItems.SelectedIndex = index-1;
            }
        }

        private DispatcherTimer timer = null;
        private TimeSpan TotalTime;
        private void myMedia_MediaOpened(object sender, RoutedEventArgs e)
        {
            TotalTime = myMedia.NaturalDuration.TimeSpan;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_tick;
            timer.Start();
        }

        private void timer_tick(object sender, object e)
        {
            if (myMedia.NaturalDuration.TimeSpan.TotalSeconds > 0)
            {
                if (TotalTime.TotalSeconds > 0)
                {
                    // Updating time slider
                    slTime.Value = (myMedia.Position.TotalSeconds/TotalTime.TotalSeconds *100);
                }
            }
        }

        private void closePane(object sender, RoutedEventArgs e) {
            myMedia.Position = myMedia.Position - TimeSpan.FromSeconds(3);
        }

        private void openPanel(object sender, RoutedEventArgs e) {
            myMedia.Position = myMedia.Position + TimeSpan.FromSeconds(3);
        }

    }
}

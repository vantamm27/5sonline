using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Model.YoutubeVideoSampleWP80;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using MyToolkit.Multimedia;
using System.IO;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;

namespace TestYouTubeEx
{
    public partial class MainPage : PhoneApplicationPage
    {
        int index;
        int nResult;
        int indexMax;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
          
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            index = 0;
            DataContext = App.ViewModel;
            nResult = 10;
            
        }

       
       

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            indexMax = App.ViewModel.Count() / nResult;
            LoadPlayList(index* nResult, nResult);
            base.OnNavigatedTo(e);
        }

        //Method to get the vdieos from the Youtube Playlist
        private async Task<List<YoutubeVideo>> GetYoutubePlaylist(string url)
        {
            try
            {
                //HttpClient client = new HttpClient();
                //client.Timeout = new TimeSpan(0, 0, 10);
                //var feedXML = await client.GetStringAsync(new Uri(url));
                StreamReader stream = new StreamReader(url);
                string xml = stream.ReadToEnd();
                StringReader stringReader = new StringReader(xml);
                //StringReader stringReader = new StringReader(feedXML);

                XmlReader xmlReader = XmlReader.Create(stringReader);
                SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
                List<YoutubeVideo> videosList = new List<YoutubeVideo>();
                YoutubeVideo video;
               
             
                foreach (SyndicationItem item in feed.Items)
                {
                    video = new YoutubeVideo();                 
                    video.YoutubeLink = item.Links[0].Uri.ToString();
                    string a = video.YoutubeLink.ToString().Remove(0, 31);                    
                    string id = a.Substring(0, 11);
                    if (id == null)
                        continue;
                    video.Id = id;
                    video.Title = item.Title.Text;

                    char[] strChars = { ':', ' ' };
                    string[]  title = video.Title.Substring(15).Split(strChars);
                                        
                    foreach (string str in title)
                    {
                        int i;
                        string temp = str.Trim(strChars);
                        
                            if (Int32.TryParse(temp, out i))
                            {
                                video.Episodes = i;
                                break;
                            }
                        
                    }


                    video.PubDate = item.PublishDate.DateTime;

                    video.Thumbnail = YouTube.GetThumbnailUri(video.Id, YouTubeThumbnailSize.Small).ToString();
                    video.VideoLink = video.YoutubeLink;
                    App.ViewModel.Save(video);
                    videosList.Add(video);
                }

                return videosList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        //After selecting a video, navigate to the VideoPage
        private void VideosList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            YoutubeVideo video = e.AddedItems[0] as YoutubeVideo;
            
            if(video != null)
                PlayVideo(video.Id);

            //if(video != null)
            //{
            //    NavigationService.Navigate(new Uri("/VideoPager.xaml?videoid=" + video.Id, UriKind.Relative));
            //}
          

        }
        private async void GetLink(string YoutubeID)
        {
            var url = await YouTube.GetVideoUriAsync(YoutubeID, YouTubeQuality.QualityMedium);
            if (url == null)
            {
                MessageBox.Show("Return null");
                return;
            }
            MessageBox.Show("Return OK");

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Terminate();

            base.OnBackKeyPress(e);
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           

        }
        private async void LoadPlayList(int ifrom, int nResult)
        {
            try
            {
                if (App.ViewModel.Count() <= 0)
                {
                    GetYoutubePlaylist("DataBase/1.xml");
                    GetYoutubePlaylist("DataBase/2.xml");
                    GetYoutubePlaylist("DataBase/3.xml");
                    GetYoutubePlaylist("DataBase/4.xml");
                    GetYoutubePlaylist("DataBase/5.xml");
                    GetYoutubePlaylist("DataBase/6.xml");
                    GetYoutubePlaylist("DataBase/7.xml");
                    GetYoutubePlaylist("DataBase/8.xml");
                    GetYoutubePlaylist("DataBase/9.xml");
                    indexMax = App.ViewModel.Count() / nResult;
                   
                }
                var playlistVideos = App.ViewModel.Load(ifrom, nResult);
                    PlaylistVideos.ItemsSource = playlistVideos;
                    PlaylistVideos.Visibility = Visibility.Visible;
                    PlaylistProgress.Visibility = Visibility.Collapsed;
              
                //if (NetworkInterface.GetIsNetworkAvailable())
                //{                    
                //    int max_results = 10;
                //    int index_result = index * 10 +1;
                    
                //    //Playlist Videos
                //    PlaylistVideos.Visibility = Visibility.Collapsed;
                //    PlaylistProgress.Visibility = Visibility.Visible;

                //    //Here is the ID of the Playlist
                //    //string YoutubePlaylist = "PLFPUGjQjckXET1TSWuDoPjYljfedxrA04";
                //    string YoutubePlaylist = "http://gdata.youtube.com/feeds/api/playlists/PLCu0gLeStK1prN2kCqbsMXsa2Ki4btCj2";
                //    var playlistVideos = await GetYoutubePlaylist(YoutubePlaylist + "?orderby=published&max-results=" + max_results);//+"&start-index="+index_result.ToString());
                //    PlaylistVideos.ItemsSource = playlistVideos;


                //    PlaylistVideos.Visibility = Visibility.Visible;
                //    PlaylistProgress.Visibility = Visibility.Collapsed;
                //    /////////
                //}
                //else
                //{
                //    MessageBox.Show("You're not connected to Internet!");
                //}
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Image_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void Rewind_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if ((index - 1) >= 0)
            {
                index--;
                LoadPlayList(index * nResult, nResult);
            }
        }

        private void Fastforward_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if ((index + 1) <= indexMax)
            {
                index++;
                LoadPlayList(index * nResult, nResult);
            }

        }

        private void ToHead_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            index = 0;
            LoadPlayList(index * nResult, nResult);

        }

        private void Search_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            

        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //if (!System.Text.RegularExpressions.Regex.IsMatch(InputIndex.Text, "(.*[0-9])") )
            //{
            //    if( InputIndex.Text.Length >0)
            //    InputIndex.Text= InputIndex.Text.Remove(InputIndex.Text.Length - 1);
                
            //}
        }
        private async void PlayVideo(string strYouTubeId)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                   
                    //if (NavigationContext.QueryString.TryGetValue("videoid", out strYouTubeId))
                    {
                        //Get The Video Uri and set it as a player source
                        YouTubeUri url = await YouTube.GetVideoUriAsync(strYouTubeId, YouTubeQuality.Quality360P);
                        if (url == null)
                        {
                            MessageBox.Show("Return null");

                        }

                        // player.Source = new Uri("https://www.youtube.com/watch?v=Zln9I9IttLA&feature=youtube_gdata");
                        //player.Source = url.Uri;
                        MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();

                        mediaPlayerLauncher.Media = url.Uri;// new Uri("MyVideo.wmv", UriKind.Relative);
                        mediaPlayerLauncher.Location = MediaLocationType.Data;
                        mediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop | MediaPlaybackControls.Skip;
                        mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;

                        mediaPlayerLauncher.Show();
                        //player.Source = new Uri("http://chz.240.mp3.zdn.vn/zv/28aab30bac5e95d73437e13fe319b1f4/5529c390/2015/04/10/9/d/9d537ae59be8aa49e720a41628ad0dc2.3gp?start=0");

                    }
                }
                else
                {
                    MessageBox.Show("You're not connected to Internet!");
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        
    }
}
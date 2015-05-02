using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.NetworkInformation;
using MyToolkit.Multimedia;
using MyToolkit;
using Microsoft.Phone.Tasks;

namespace TestYouTubeEx
{
    public partial class VideoPager : PhoneApplicationPage
    {

        private System.Windows.Threading.DispatcherTimer timer;
        private bool bMouseMove = false;
        public VideoPager()
        {
            InitializeComponent();
            //timer = new System.Windows.Threading.DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            //timer.Tick += new EventHandler(UpdateTimeLine);
        }

        //public void UpdateTimeLine(object sender, EventArgs e)
        //{
        //    if(!bMouseMove)
        //    timelineSlider.Value = player.Position.TotalSeconds;
        //}


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    string strYouTubeId = String.Empty;
                    if (NavigationContext.QueryString.TryGetValue("videoid", out strYouTubeId))
                    {
                         //Get The Video Uri and set it as a player source
                        YouTubeUri url = await YouTube.GetVideoUriAsync(strYouTubeId, YouTubeQuality.Quality360P);  
                         if(url == null)
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

            base.OnNavigatedTo(e);
        }

        private void player_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //if (Navigation.Visibility == Visibility.Visible)
            //{
            //    Navigation.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    Navigation.Visibility = Visibility.Visible;

            //}

        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            //player.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            //player.Pause();
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            //TimeSpan newPlayTime = new TimeSpan(0,0,5);

            //player.Position -= newPlayTime;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            //TimeSpan newPlayTime = new TimeSpan(0, 0, 5);

            //player.Position += newPlayTime;
            
        }

        private void player_MediaOpened(object sender, RoutedEventArgs e)
        {
            //timelineSlider.Maximum = (int)player.NaturalDuration.TimeSpan.TotalSeconds;
            //timer.Start();
            
        }

        private void timelineSlider_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //if(bMouseMove)
            //    player.Position = TimeSpan.FromSeconds((double)timelineSlider.Value);
            //bMouseMove = false;
        }

        private void timelineSlider_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bMouseMove = true;
        }

        private void player_MediaEnded(object sender, RoutedEventArgs e)
        {
            //int a = 10;
            //player.Position = new TimeSpan(0, 0, 1);
            //player.Play();
            
        }

        private void player_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            //int a = 10;
        }

        private void player_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            //int a = 10;
        }

    
       
         
    }

    
}
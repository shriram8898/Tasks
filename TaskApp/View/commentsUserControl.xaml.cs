using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using TaskApp.Models;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskApp.View
{
    public sealed partial class commentsUserControl : UserControl
    {
        public MediaPlayer media_player;
        MediaSource media_source;
        int secondscount = 0;
        bool Isplaying = false;
        DispatcherTimer timer2;
        public comments comments { get { return this.DataContext as comments; } }
        public ObservableCollection<comments> com = new ObservableCollection<comments>();
        public commentsUserControl()
        {
            this.InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer2 = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            media_player = new MediaPlayer();
            media_player.MediaEnded += Media_player_MediaEnded;
            secondscount = 0;
            HoursTextBlock.Text = "00";
            SecondsTextBlock.Text = "00";
            MinutesTextBlock.Text = "00";
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string msgtime;
            DateTime dt = DateTime.Now;
            string final = "";
            var date = DateTime.Today.AddDays(-1).ToString("MM-dd-yyyy");
            var date1 = comments.dt.ToString("MM-dd-yyyy");
            DateTime dt1 = comments.dt;
            var times = new TimeSpan(dt1.Ticks - dt.Ticks);
            double delta = Math.Abs(times.TotalSeconds);
            if (delta < 60)
            {
                final = "few seconds ago";
            }
            else if (delta < 60 * 2)
            {
                final = "a minute ago";
            }
            else if (delta < 45 * 60)
            {
                final = Math.Abs(times.Seconds).ToString() + " minutes ago";
            }
            else if (delta < 90 * 60)
            {
                final = "an hour ago";
            }
            else if (delta < 24 * 60 * 60)
            {
                final = Math.Abs(times.Hours).ToString() + " hours ago";
            }
            else if (delta < 48 * 60 * 60)
            {
                final = "yesterday";

            }
            else
            {
                if (date != date)
                    final = comments.dt.ToString();
                else
                    final = "yesterday";
            }
            string picture = "Assets/" + comments.empid + ".jpg";
            var bitmapImage = new BitmapImage(new Uri(this.BaseUri, picture));
            pic.ProfilePicture = bitmapImage;
            time.Text = final;
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            if(comments.IsFile==true)
            {
                message.Visibility = Visibility.Collapsed;
                media_source = MediaSource.CreateFromStorageFile(comments.storagefile);
                if (comments.storagefile.Name.Contains(".mp3"))
                {
                    audio.Visibility = Visibility.Visible;
                    media_player.Source = media_source;
                    sliProgress.Minimum = 0;
                    sliProgress.Maximum = media_player.PlaybackSession.NaturalDuration.TotalSeconds - 1;
                    timer2.Interval = TimeSpan.FromSeconds(1);
                    timer2.Tick += Times;
                }
                else
                {
                    video.Visibility = Visibility.Visible;
                    VideoPlayer.Source = media_source;
                }
                
            }
        }

        private void Times(object sender, object e)
        {
            secondscount++;
            HoursTextBlock.Text = String.Format("{0:00}", (secondscount / 3600));
            MinutesTextBlock.Text = String.Format("{0:00}", (secondscount % 3600) / 60);
            SecondsTextBlock.Text = String.Format("{0:00}", (secondscount % 60));
        }

        private void Media_player_MediaEnded(MediaPlayer sender, object args)
        {
            Isplaying = false;
            media_player.Source = media_source;
        }
        private void timer_Tick(object sender, object e)
        {
            if (Isplaying)
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = media_player.PlaybackSession.NaturalDuration.TotalSeconds - 1;
                sliProgress.Value = media_player.PlaybackSession.Position.TotalSeconds;

            }
            else
            {
                timer2.Stop();
                secondscount = 0;
                Play.Content = "\uF5B0";
            }
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (Isplaying == false)
            {
                Isplaying = true;
                timer2.Start();
                media_player.Play();
                Play.Content = "\uE103";
                timers.Visibility = Visibility.Visible;
            }
            else
            {
                Isplaying = false;
                timer2.Stop();
                media_player.Pause();
                Play.Content = "\uF5B0";
            }

        }
        private void sliProgress_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            media_player.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

    }
}

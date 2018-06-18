using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media;

namespace EindopdrachtUWP.Classes
{


    class SoundController
    {
        private Dictionary<string, Sound> sounds;

        MediaElement soundTrack = new MediaElement();
//        private Dictionary<string, MediaElement> sounds;


        //ElementSoundPlayer player = new ElementSoundPlayer();
        public bool mutedSFX = false;
        public bool mutedMusic = false;


        public SoundController()
        {
            sounds = new Dictionary<string, Sound>();
            mutedSFX = false;
            mutedMusic = false;
            soundTrack.Volume = 0.3;
            LoadSound("Soundtrack\\Soundtrack.wav", soundTrack);
            soundTrack.MediaEnded += SoundTrackEnded;
            
        }


        public void AddSound(string sound, double volume = 0.8)
        {
            if (sound == null || sound == "") return;
            if (sounds.ContainsKey(sound)) return;

            Sound s = new Sound()
            {
                firstSound = new MediaElement(),
                secondSound = new MediaElement(),
                playFirst = true
            };
            s.firstSound.AutoPlay = false;
            s.secondSound.AutoPlay = false;

            s.firstSound.Volume = volume;
            s.secondSound.Volume = volume;

            sounds.Add(sound, s);
            LoadSound(sound, s.firstSound);
            LoadSound(sound, s.secondSound);
            s.firstSound.MediaFailed += FailedLoadingMedia;
            s.secondSound.MediaFailed += FailedLoadingMedia;

        }

        private void FailedLoadingMedia(object sender, ExceptionRoutedEventArgs e)
        {
            MediaElement me = (MediaElement)sender;
            me.Source = null;
        }

        public void PlaySound(string sound)
        {
            if (mutedSFX) return;
            if(sounds.ContainsKey(sound))
            {
                Sound toPlay = sounds[sound];
                if (toPlay.playFirst)
                {
                    Play(toPlay.firstSound);
//                    toPlay.firstSound.Volume = 0.5;
//                    Debug.WriteLine(""+);
                }
                else
                {
                    Play(toPlay.secondSound);
                }
                toPlay.playFirst = !toPlay.playFirst;
            }

        }

        //public void LoadAllSounds()
        //{
        //    foreach (KeyValuePair<string, MediaElement> sound in sounds)
        //    {
        //    }
        //}


        // Must be in another method (because of async)
        private async void LoadSound(String filename, MediaElement sound)
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("Sounds\\" + filename);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            sound.SetSource(stream, file.ContentType);
        }

        private void Play(MediaElement sound)
        {
            try
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    Debug.WriteLine(sound.Volume);
                    sound.Position = new TimeSpan(0);
                    sound.Play();
                });

            }
            catch (Exception e)
            {
                //e.Message
                Debug.Write(e.Message);
            }
        }

        public void muteSFX()
        {
            mutedSFX = true;
        }
        public void unMuteSFX()
        {
            mutedSFX = false;
        }

        public void muteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    mutedMusic = true;
                    soundTrack.Pause();
                }
            );
        }

        public void unMuteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    mutedMusic = false;
                    soundTrack.Play();
                }
            );
        }
        private void SoundTrackEnded(object sender, RoutedEventArgs e)
        {
            Play(soundTrack);
        }
    }
}

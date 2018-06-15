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
//        private Dictionary<string, MediaElement> sounds;


        //ElementSoundPlayer player = new ElementSoundPlayer();
        

        public SoundController()
        {
            sounds = new Dictionary<string, Sound>();
        }

        public void AddSound(string sound)
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
            if(sounds.ContainsKey(sound))
            {
                Sound toPlay = sounds[sound];
                if (toPlay.playFirst)
                {
                    Play(toPlay.firstSound);
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
                   
                   // sound.Stop();
                    sound.Position = new TimeSpan(0);
//                    Task.Delay(10);
                    sound.Play();
                });

            }
            catch (Exception e)
            {
                //e.Message
                Debug.Write(e.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EindopdrachtUWP.Classes
{


    class SoundController
    {
        private Dictionary<string, MediaElement> sounds;

        public SoundController()
        {
            sounds = new Dictionary<string, MediaElement>();
        }

        public void AddSound(string sound)
        {
            MediaElement me = new MediaElement();
            me.AutoPlay = false;
            sounds.Add(sound, me);
        }

        public void PlaySound(string sound)
        {
            if(sounds.ContainsKey(sound))
            {
                Play(sounds[sound]);
            }

        }

        public void LoadAllSounds()
        {
            foreach (KeyValuePair<string, MediaElement> sound in sounds)
            {
                LoadSound(sound.Key, sound.Value);
            }
        }


        // Must be in another method (because of async)
        public async void LoadSound(String filename, MediaElement sound)
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("Sounds\\" + filename);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            sound.SetSource(stream, file.ContentType);
        }

        public void Play(MediaElement sound)
        {
            try
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    sound.Stop();
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
    }
}

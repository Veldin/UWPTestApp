using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPTestApp
{


    class SoundController
    {

        // Must be in another method (because of async)
        public static async void LoadSound(MediaElement sound, String filename)
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync(filename);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            sound.SetSource(stream, file.ContentType);
        }

        public static void Play(MediaElement sound)
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

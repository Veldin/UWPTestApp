using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPTestApp
{


    class SoundExampleOne
    {
        private MediaElement shotSound = new MediaElement();

        public SoundExampleOne()
        {
            shotSound.AutoPlay = false;
            LoadSound(shotSound, @"\Sounds\Weapons\shot_test.mp3");
        }



        public void playShotSound()
        {
            shotSound.Stop();
            shotSound.Play();

        }

        // Must be in another method (because of async)
        public async void LoadSound(MediaElement sound, String filename)
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync(filename);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            sound.SetSource(stream, file.ContentType);
        }
    }
}

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
    class SoundExample
    {

        private MediaElement shotSound = new MediaElement();

        public SoundExample()
        {
            //shotSound.AutoPlay = false;


            SoundController.LoadSound(shotSound, @"\Sounds\Weapons\shot_test.mp3");
        }



        public void PlayShotSound()
        {
            SoundController.Play(shotSound);

        }
    }
}

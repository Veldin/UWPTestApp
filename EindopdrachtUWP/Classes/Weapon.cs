using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace EindopdrachtUWP.Classes
{
    public interface Weapon
    {
        List<String> tags {get; set;}
        String name { get; set; }
        String description { get; set; }
        int currentClip { get; set; }
        int clipAmount { get; set; }
        int clipMax { get; set; }
        int damage { get; set; }
        float fireTime { get; set; }
        float fireTimer { get; set; }
        double critChance { get; set; }
        double critMultiplier { get; set; }
        int weaponLevel { get; set; }
        MediaElement shotsound { get; set; }
        MediaElement reloadSound { get; set; }
        float reloadTime { get; set; }
        float reloadTimer { get; set; }

        void Fire(float fromTop, float fromLeft);
        Boolean RemoveTag(String tag);
        Boolean HasTag(String tag);
        void AddTag(String tag);
        void Reload();
        void Display();
        void PlayShotSound();
        void PlayReloadSound();
        void Upgrade();
    }
}

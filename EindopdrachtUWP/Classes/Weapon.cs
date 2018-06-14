using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes
{
    public interface Weapon
    {
        List<String> tags {get;}
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
        String shotSound { get; set; }
        String reloadSound { get; set; }
        float reloadTime { get; set; }
        float reloadTimer { get; set; }

        void Fire(float fromTop, float fromLeft, List<GameObject> gameObjects);
        Boolean RemoveTag(String tag);
        Boolean HasTag(String tag);
        void AddTag(String tag);
        void Reload();
        void Display();
        void Upgrade();
        Boolean OnTick(float cooldownDelta, float delta);
    }
}
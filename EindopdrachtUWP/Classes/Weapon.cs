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
        float damage { get; set; }
        float accuracy { get; set; }
        float fireTime { get; set; }
        double critChance { get; set; }
        double critMultiplier { get; set; }
        int weaponLevel { get; set; }
        String shotSound { get; set; }
        String reloadSound { get; set; }
        float reloadTime { get; set; }

        bool Fire(float fromLeft, float fromTop, float width, float height, List<GameObject> gameObjects, String direction);
        void AddTag(String tag);
        void Reload();
        void Upgrade();
        int GetAmmo();
        Boolean OnTick(float delta);
    }
}
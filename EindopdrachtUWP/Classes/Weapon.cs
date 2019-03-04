using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes
{
    public interface Weapon
    {
        List<string> tags {get;}
        string name { get; set; }
        string description { get; set; }
        int currentClip { get; set; }
        int clipAmount { get; set; }
        int clipMax { get; set; }
        float damage { get; set; }
        float accuracy { get; set; }
        float fireTime { get; set; }
        double critChance { get; set; }
        double critMultiplier { get; set; }
        int weaponLevel { get; set; }
        string shotSound { get; set; }
        string reloadSound { get; set; }
        float reloadTime { get; set; }
        float range { get; set; }                    // The range of the gun (this is the distanceTillDestroyed value of all projectiles from this gun)

        /* Fire */
        /*
         * Called when this weapon should fire, returns true if it fired, returns false otherwise.
         * From Left and fromTop are the position where the fire happended.
         * width and height are the size of the bullet to spawn.
         * 
         * The list gameObjects is where the projectile gets added to if the fire happens.
         * The direction is used to know what sprite to load.
        */
        bool Fire(float fromLeft, float fromTop, float width, float height, List<GameObject> gameObjects, string direction);

        /* Reload */
        /*
         * Orderes the weapon to reaload.
        */
        void Reload();

        /* Reload */
        /*
         * Orderes the weapon to upgrade.
        */
        void Upgrade();

        /* GetAmmo */
        /*
         * Returns the total ammunution remaining in this weapon
        */
        int GetAmmo();

        /* OnTick */
        /*
         * Called every frame,
         * The argument delta is how much time is passed since the last call.
        */
        bool OnTick(float delta);
    }
}
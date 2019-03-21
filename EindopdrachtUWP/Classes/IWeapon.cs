using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes
{
    public interface IWeapon
    {
        List<string> Tags { get; }
        string Name { get; set; }
        string Description { get; set; }
        int CurrentClip { get; set; }
        int ClipAmount { get; set; }
        int ClipMax { get; set; }
        float Damage { get; set; }
        float Accuracy { get; set; }
        float FireTime { get; set; }
        double CritChance { get; set; }
        double CritMultiplier { get; set; }
        int WeaponLevel { get; set; }
        string ShotSound { get; set; }
        string ReloadSound { get; set; }
        float ReloadTime { get; set; }
        float Range { get; set; }        // The range of the gun (this is the distanceTillDestroyed value of all projectiles from this gun)

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

        /* Upgrade*/
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
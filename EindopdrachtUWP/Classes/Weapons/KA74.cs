using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class KA74 : Weapon
    {
        public List<string> tags { get; }
        public string name { get; set; }
        public string description { get; set; }
        public int currentClip { get; set; }
        public int clipAmount { get; set; }
        public int clipMax { get; set; }
        public int damage { get; set; }
        public float fireTime { get; set; }
        public float fireTimer { get; set; }
        public double critChance { get; set; }
        public double critMultiplier { get; set; }
        public int weaponLevel { get; set; }
        public string shotSound { get; set; }
        public string reloadSound { get; set; }
        public float reloadTime { get; set; }
        public float reloadTimer { get; set; }
        protected float fireCooldownDelta;      //The remaining delta for shooting
        protected float reloadCooldownDelta;    //The remaining delta for reloading
        protected bool ableToFire;              //Boolean to check is you're able to fire again
        protected bool ableToReload;            //Boolean to check is you're able to reload again

        public KA74()
        {
            // constructor for the KA74 class
            name = "KA74";
            description = "The KA74 is a strong short ranged weapon, also known as a shotgun";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 30;
            damage = 30;
            fireTime = 0.1f;
            critChance = 0.1;
            critMultiplier = 1.5;
            weaponLevel = 1;
            reloadTime = 1.5f;
            shotSound = "Weapon_Sounds\\KA74_Shot1.wav";
        }

        public void AddTag(string tag)
        {
            // add a tag to the tags list
            tags.Add(tag);
        }

        public void Display()
        {
            // show sprite
        }


        public void Fire(float fromTop, float fromLeft, List<GameObject> gameObjects)
        {
            // fire one bullet
            if (ableToFire && currentClip > 0)
            {
                gameObjects.Add(new Projectile(4, 4, fromLeft, fromTop, 0, 0, 0, 0, damage));
                currentClip--;
                ableToFire = false;


            }
            if (currentClip == 0)
            {
                Reload();
            }
        }

        public Boolean HasTag(string searchTag)
        {
            // check if the weapon has a certain tag
            foreach (string tag in tags)
            {
                if (tag.Equals(searchTag))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reload()
        {
            // reload this weapon, but only if you have enough clips
            if (ableToReload && clipAmount > 0)
            {
                clipAmount--;
                currentClip = clipMax;
                ableToReload = false;
            }
        }

        public Boolean RemoveTag(String searchTag)
        {
            // searches for a specific tag and removes it if it's found
            foreach (string tag in tags)
            {
                if (tag.Equals(searchTag))
                {
                    tags.Remove(searchTag);
                    return true;
                }
            }
            return false;
        }

        public void Upgrade()
        {
            // upgrade weapon level for a stronger weapon
            weaponLevel++;
            damage += 2;
        }

        public Boolean OnTick(float delta)
        {
            if (fireCooldownDelta - delta < 0)
            {
                fireCooldownDelta = fireTime;
                ableToFire = true;
            }
            else
            {
                fireCooldownDelta -= delta;
            }

            if (reloadCooldownDelta - delta < 0)
            {
                reloadCooldownDelta = reloadTime;
                ableToReload = true;
            }
            else
            {
                reloadCooldownDelta -= delta;
            }

            return true;
        }
    }
}

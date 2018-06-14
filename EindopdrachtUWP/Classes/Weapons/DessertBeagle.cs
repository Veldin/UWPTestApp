using System;
using System.Collections.Generic;
using System.Diagnostics;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class DessertBeagle : Weapon
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

        public DessertBeagle()
        {
            // constructor for the DessertBeagle class
            name = "DessertBeagle";
            description = "The DessertBeagle is a strong handgun, also known as a Desert Eagle";
            currentClip = 7;
            clipAmount = 10;
            clipMax = 7;
            damage = 35;
            fireTime = 1000;
            critChance = 0.3;
            critMultiplier = 1.5;
            weaponLevel = 1;
            reloadTime = 6000;
            shotSound = "Weapon_Sounds\\Dessert_Beagle_Shot1.wav";

            ableToReload = true;
            ableToFire = true;
            fireCooldownDelta = 0;
            reloadCooldownDelta = 0;
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

        public void Fire(float fromLeft, float fromTop, float width, float height, List<GameObject> gameObjects , String direction)
        {

            // fire one bullet
            if (ableToFire && currentClip > 0)
            {
                Debug.WriteLine("Able to fire");

                if (direction == "Top")
                {
                    gameObjects.Add(new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, damage, fromLeft, fromTop - height));
                }
                else if (direction == "Bottom")
                {
                    gameObjects.Add(new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, damage, fromLeft, fromTop + height));
                }
                else if (direction == "Left")
                {
                    gameObjects.Add(new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, damage, fromLeft - height, fromTop));
                }
                else //Right
                {
                    gameObjects.Add(new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, damage, fromLeft + height, fromTop));
                }
                
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
            damage += 1;
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
            else if(currentClip <= 0)
            {
                reloadCooldownDelta -= delta;
            }

            return true;
        }
    }
}

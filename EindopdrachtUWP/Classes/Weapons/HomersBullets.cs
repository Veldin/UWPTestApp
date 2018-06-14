using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class HomersBullets : Weapon
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

        public HomersBullets()
        {
            // constructor for the HomersBullets class
            name = "HomersBullets";
            description = "The HomersBullet is a bullet that follows it's target";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 1;
            damage = 120;
            fireTime = 1.5f;
            critChance = 0.1;
            critMultiplier = 2;
            weaponLevel = 1;
            reloadTime = 5;
            AddTag("homing");
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
            if (fireTimer == 0 && currentClip > 0)
            {
                GameObject project = new Projectile(6, 6, fromLeft, fromTop, 0, 0, 0, 0, damage);
                project.AddTag("homing");
                gameObjects.Add(project);
                currentClip--;
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
            if (reloadTimer == 0 && clipAmount > 0)
            {
                clipAmount--;
                currentClip = clipMax;
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
            damage += 5;
        }
    }
}

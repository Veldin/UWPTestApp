using System;
using System.Collections.Generic;
using UWPTestApp;
using Windows.UI.Xaml.Controls;

namespace EindopdrachtUWP.Classes.Weapons
{
    class BulletBill : Weapon
    {
        public List<string> tags { get => tags; }
        public string name { get { return name; } set { name = value; } }
        public string description { get { return description; } set { description = value; } }
        public int currentClip { get { return currentClip; } set { currentClip = value; } }
        public int clipAmount { get { return clipAmount; } set { clipAmount = value; } }
        public int clipMax { get { return clipMax; } set { clipMax = value; } }
        public int damage { get { return damage; } set { damage = value; } }
        public float fireTime { get { return fireTime; } set { fireTime = value; } }
        public float fireTimer { get { return fireTimer; } set { fireTimer = value; } }
        public double critChance { get { return critChance; } set { critChance = value; } }
        public double critMultiplier { get { return critMultiplier; } set { critMultiplier = value; } }
        public int weaponLevel { get { return weaponLevel; } set { weaponLevel = value; } }
        public String shotSound { get { return shotSound; } set { shotSound = value; } }
        public String reloadSound { get { return reloadSound; } set { reloadSound = value; } }
        public float reloadTime { get { return reloadTime; } set { reloadTime = value; } }
        public float reloadTimer { get { return reloadTimer; } set { reloadTimer = value; } }

        public BulletBill()
        {
            // constructor for the BulletBill class
            name = "BulletBill";
            description = "The BulletBill is a big bullet that goes in a straight line until it hits a wall";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 1;
            damage = 130;
            fireTime = 2;
            critChance = 0.1;
            critMultiplier = 2;
            weaponLevel = 1;
            reloadTime = 5;
            shotSound = "Weapon_Sounds\\Bullet_Bill_Shot1.wav";
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
            if (currentClip > 0)
            {
                fireTimer = 0;
                gameObjects.Add(new Projectile(7, 7, fromLeft, fromTop, 0, 0, 0, 0, damage));
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
            if (clipAmount > 0)
            {
                reloadTimer = 0;
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

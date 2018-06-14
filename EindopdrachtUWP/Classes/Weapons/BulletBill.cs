using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class BulletBill : Weapon
    {
        public List<string> tags { get; }
        public string name { get; set; }
        public string description { get; set; }
        public int currentClip { get; set; }
        public int clipAmount { get; set; }
        public int clipMax { get; set; }
        public int damage { get; set; }
        public float fireTime { get; set; }
        public double critChance { get; set; }
        public double critMultiplier { get; set; }
        public int weaponLevel { get; set; }
        public string shotSound { get; set; }
        public string reloadSound { get; set; }
        public float reloadTime { get; set; }
        protected float cooldownDelta;              //The max delta it takes to do the next action
        protected float remainingCooldownDelta;     //The remaining delta for the next action

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
            if (remainingCooldownDelta <= 0 && currentClip > 0)
            {
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
            if (remainingCooldownDelta <= 0 && clipAmount > 0)
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

        public Boolean OnTick(float cooldownDelta, float delta)
        {
            if (remainingCooldownDelta - delta < 0)
            {
                remainingCooldownDelta = cooldownDelta;
            }
            else
            {
                remainingCooldownDelta -= delta;
            }
            return true;
        }
    }
}

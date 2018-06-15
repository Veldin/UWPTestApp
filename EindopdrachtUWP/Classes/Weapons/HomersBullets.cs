﻿using System;
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
        public float damage { get; set; }
        public float accuracy { get; set; }
        public float fireTime { get; set; }
        public double critChance { get; set; }
        public double critMultiplier { get; set; }
        public int weaponLevel { get; set; }
        public string shotSound { get; set; }
        public string reloadSound { get; set; }
        public float reloadTime { get; set; }
        protected float fireCooldownDelta;      //The remaining delta for shooting
        protected float reloadCooldownDelta;    //The remaining delta for reloading
        protected bool ableToFire;              //Boolean to check is you're able to fire again
        protected bool ableToReload;            //Boolean to check is you're able to reload again

        public HomersBullets()
        {
            // constructor for the HomersBullets class
            name = "Homers Bullets";
            description = "The Homers Bullet is a bullet that follows it's target. D'OH!";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 10;
            damage = 150;
            accuracy = 0;
            fireTime = 1000;
            critChance = 0.2;
            critMultiplier = 1.5;
            weaponLevel = 1;
            reloadTime = 2000;
            tags = new List<string>();
            AddTag("homing");
            shotSound = "Weapon_Sounds\\Homers_Bullets_Shot1.wav";

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

        public void Fire(float fromLeft, float fromTop, float width, float height, List<GameObject> gameObjects, String direction)
        {
            // fire one bullet
            if (ableToFire && currentClip > 0)
            {
                GameObject project = new Projectile(6, 6, fromLeft, fromTop, 0, 0, 0, 0, damage);
                project.AddTag("homing");
                gameObjects.Add(project);
                currentClip--;
                ableToFire = false;
            }
            if (ableToReload && currentClip == 0)
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
            damage *= 1.1f;
            fireTime *= 0.95f;
            clipMax += 1;
            reloadTime *= 0.95f;
            critChance *= 1.2;
            critMultiplier += 0.1;
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

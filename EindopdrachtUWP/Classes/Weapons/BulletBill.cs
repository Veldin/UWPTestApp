﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public BulletBill()
        {
            // constructor for the BulletBill class
            name = "Bullet Bill";
            description = "The Bullet Bill is a big bullet that goes in a straight line until it hits a wall";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 5;
            damage = 500;
            accuracy = 0;
            fireTime = 2000;
            critChance = 0.15;
            critMultiplier = 2;
            weaponLevel = 1;
            reloadTime = 5000;
            shotSound = "Weapon_Sounds\\Bullet_Bill_Shot1.wav";

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

                Debug.WriteLine("Fire!");

                gameObjects.Add(new Projectile(7, 7, fromLeft, fromTop, 0, 0, 0, 0, damage));
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

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public float damage { get; set; }
        public float accuracy { get; set; }
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
        private string location;

        public KA74()
        {
            // constructor for the KA74 class
            name = "KA74";
            description = "The KA74 is a strong ranged machine gun, also known as the Kalasjnikov";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 30;
            damage = 50;
            accuracy = 1.5f;
            fireTime = 300;
            critChance = 0.05;
            critMultiplier = 1.25;
            weaponLevel = 1;
            reloadTime = 2000;
            shotSound = "Weapon_Sounds\\KA74_Shot1.wav";
            location = "Assets\\Sprites\\Bullet_Sprites\\Projectile_Sprite.png";

            ableToReload = false;
            ableToFire = true;
            fireCooldownDelta = 0;
            reloadCooldownDelta = 2000;
        }

        public void AddTag(string tag)
        {
            // add a tag to the tags list
            tags.Add(tag);
        }

        private float getProjectileDamage(float damage, float change, float multiplier, Random random)
        {
            //Determine if its a critical hit if the generated number is lower then the crid change times 100
            if (random.Next(0, 101) < (change * 100))
            {
                damage = damage * multiplier;

                //If it was a crit call this function again to be able to have double, tripple, quad, ect crits.
//                return getProjectileDamage(damage, change, multiplier, random);
            }

            return damage;
        }

        public bool Fire(float fromLeft, float fromTop, float width, float height, List<GameObject> gameObjects, String direction)
        {
            if (ableToReload && currentClip == 0)
            {
                Reload();
            }

            //Random random = new Random(Guid.NewGuid().GetHashCode());
            //float randomPositionOffset = (random.Next(0, (int)accuracy * (int)accuracy)) ;
            //randomPositionOffset = randomPositionOffset - accuracy * (accuracy) / 2;

            Random random = new Random();
            //Random.next first int is inclusive the second is excusive, due to this the half of the accuracy devided by 2 is added.
            //Get a number between the accuracy and the accuracy * -1.
            //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
            float randomPositionOffset = (random.Next((int)(accuracy * -1) * 100, (int)accuracy * 100) + accuracy / 2) / 100;

            float projectileDamage = getProjectileDamage((float)damage, (float)critChance, (float)critMultiplier, random);

            // fire one bullet
            if (ableToFire && currentClip > 0)
            {
                Projectile projectile;

                if (direction == "Top")
                {
                    projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop - height);
                    projectile.SetLocation(location);
                }
                else if (direction == "Bottom")
                {
                    projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop + height);
                    projectile.SetLocation(location);
                }
                else if (direction == "Left")
                {
                    projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft - height, fromTop + randomPositionOffset);
                    projectile.SetLocation(location);
                }
                else //Right
                {
                    projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + height, fromTop + randomPositionOffset);
                    projectile.SetLocation(location);
                }

                if (projectileDamage > damage)
                {
                    projectile.AddTag("crit");
                }

                gameObjects.Add(projectile);

                currentClip--;
                ableToFire = false;
                return true;
            }
            if (currentClip == 0 && clipAmount == 0)
            {
                MainPage.Current.weaponEmpty(name);
            }
            return false;
        }

        public void Reload()
        {
            // reload this weapon, but only if you have enough clips
            if (ableToReload && clipAmount > 0)
            {
                ableToFire = false;
                clipAmount--;
                currentClip = clipMax;
                MainPage.Current.getWeaponStats();
                MainPage.Current.UpdateCurrentClip();
                ableToReload = false;
            }
        }

        public void Upgrade()
        {
            // upgrade weapon level for a stronger weapon
            weaponLevel++;
            damage += 5;
            fireTime *= 0.99f;
            clipMax += 5;
            reloadTime *= 0.99f;
            critChance += 0.01;
            if (critChance > 0.75)
            {
                critChance = 0.75;
            }
            critMultiplier += 0.1;
        }

        public int GetAmmo()
        {
            return clipAmount * clipMax + currentClip;
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
                ableToFire = true;
            }
            else if (currentClip <= 0)
            {
                reloadCooldownDelta -= delta;
            }

            return true;
        }
    }
}

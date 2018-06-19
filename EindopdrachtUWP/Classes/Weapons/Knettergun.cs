﻿using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class Knettergun : Weapon
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
        private string location;

        public Knettergun()
        {
            // constructor for the Knettergun class
            name = "Knettergun";
            description = "The Knettergun is a strong short ranged weapon, also known as a shotgun";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 6;
            damage = 400;
            accuracy = 3.3f;
            fireTime = 3000;
            critChance = 0.03;
            critMultiplier = 1.2;
            weaponLevel = 1;
            reloadTime = 4000;
            shotSound = "Weapon_Sounds\\Knetter_Gun_Shot1.wav";
            location = "Assets\\Sprites\\Bullet_Sprites\\Projectile_Sprite.png";

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

            //Random random = new Random(Guid.NewGuid().GetHashCode());
            //float randomPositionOffset = (random.Next(0, (int)accuracy * (int)accuracy)) ;
            //randomPositionOffset = randomPositionOffset - accuracy * (accuracy) / 2;

            Random random = new Random();
            //Random.next first int is inclusive the second is excusive, due to this the half of the accuracy devided by 2 is added.
            //Get a number between the accuracy and the accuracy * -1.

            float projectileDamage = getProjectileDamage((float)damage, (float)critChance, (float)critMultiplier, random);

            // fire one bullet
            if (ableToFire && currentClip > 0)
            {

                if (direction == "Top")
                {
                    for (int i = 0; i < 25; i++)
                    {
                        float randomPositionOffset = random.Next((int)(accuracy * -1), (int)accuracy) + accuracy / 2;
                        var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage/25, fromLeft + randomPositionOffset, fromTop - height);
                        projectile.SetLocation(location);
                        gameObjects.Add(projectile);
                    }
                }
                else if (direction == "Bottom")
                {
                    for (int i = 0; i < 25; i++)
                    {
                        float randomPositionOffset = random.Next((int)(accuracy * -1), (int)accuracy) + accuracy / 2;
                        var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage/25, fromLeft + randomPositionOffset, fromTop + height);
                        projectile.SetLocation(location);
                        gameObjects.Add(projectile);
                    }
                }
                else if (direction == "Left")
                {
                    for (int i = 0; i < 25; i++)
                    {
                        float randomPositionOffset = random.Next((int)(accuracy * -1), (int)accuracy) + accuracy / 2;
                        var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage/25, fromLeft - height, fromTop + randomPositionOffset);
                        projectile.SetLocation(location);
                        gameObjects.Add(projectile);
                    }
                }
                else //Right
                {
                    for (int i = 0; i < 25; i++)
                    {
                        float randomPositionOffset = random.Next((int)(accuracy * -1), (int)accuracy) + accuracy / 2;
                        var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage/25, fromLeft + height, fromTop + randomPositionOffset);
                        projectile.SetLocation(location);
                        gameObjects.Add(projectile);
                    }
                }

                currentClip--;
                ableToFire = false;
                return true;
            }
            if (ableToReload && currentClip == 0)
            {
                Reload();
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

﻿using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class ArrivaGun : Weapon
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
        private string locationBottom;
        private string locationLeft;
        private string locationRight;
        private string locationTop;

        public ArrivaGun()
        {
            // constructor for the ArrivaGun class
            name = "Arriva Gun";
            description = "The Arriva Gun is a long range weapon which hits all enemies in its path (like a railgun)";
            currentClip = 10;
            clipAmount = 10;
            clipMax = 3;
            damage = 400;
            accuracy = 0;
            fireTime = 5000;
            critChance = 0.25;
            critMultiplier = 2;
            weaponLevel = 1;
            reloadTime = 3000;
            shotSound = "Weapon_Sounds\\Arriva_Gun_Shot1.wav";
            locationBottom = "Assets\\Sprites\\Bullet_Sprites\\Arriva_Gun_Bottom.png";
            locationLeft = "Assets\\Sprites\\Bullet_Sprites\\Arriva_Gun_Left.png";
            locationRight = "Assets\\Sprites\\Bullet_Sprites\\Arriva_Gun_Right.png";
            locationTop = "Assets\\Sprites\\Bullet_Sprites\\Arriva_Gun_Top.png";

            ableToReload = false;
            ableToFire = true;
            fireCooldownDelta = 0;
            reloadCooldownDelta = 3000;
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
            float randomPositionOffset = random.Next((int)(accuracy * -1), (int)accuracy) + accuracy / 2;

            float projectileDamage = getProjectileDamage((float)damage, (float)critChance, (float)critMultiplier, random);

            // fire one bullet
            if (ableToFire && currentClip > 0)
            {

                if (direction == "Top")
                {
                    var projectileTop = new Projectile(3, 15, fromLeft, fromTop-7, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop - height);
                    projectileTop.SetLocation(locationTop);
                    projectileTop.AddTag("ghost");
                    gameObjects.Add(projectileTop);
                    
                }
                else if (direction == "Bottom")
                {
                    var projectileBottom = new Projectile(3, 15, fromLeft, fromTop-7, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop + height);
                    projectileBottom.SetLocation(locationBottom);
                    projectileBottom.AddTag("ghost");
                    gameObjects.Add(projectileBottom);
                }
                else if (direction == "Left")
                {
                    var projectileLeft = new Projectile(15, 3, fromLeft - 7, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft - height, fromTop + randomPositionOffset);
                    projectileLeft.SetLocation(locationLeft);
                    projectileLeft.AddTag("ghost");
                    gameObjects.Add(projectileLeft);
                }
                else //Right
                {
                    var projectileRight = new Projectile(15, 3, fromLeft-7, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + height, fromTop + randomPositionOffset);
                    projectileRight.SetLocation(locationRight);
                    projectileRight.AddTag("ghost");
                    gameObjects.Add(projectileRight);
                }

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
            damage += 20;
            fireTime *= 0.99f;
            clipMax += 1;
            reloadTime *= 0.99f;
            critChance += 0.0625;
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

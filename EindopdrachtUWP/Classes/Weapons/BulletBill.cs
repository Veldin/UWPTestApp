﻿using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class BulletBill : IWeapon
    {
        public List<string> Tags { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CurrentClip { get; set; }
        public int ClipAmount { get; set; }
        public int ClipMax { get; set; }
        public float Damage { get; set; }
        public float Accuracy { get; set; }
        public float FireTime { get; set; }
        public double CritChance { get; set; }
        public double CritMultiplier { get; set; }
        public int WeaponLevel { get; set; }
        public string ShotSound { get; set; }
        public float Range { get; set; }        // The range of the gun (this is the distanceTillDestroyed value of all projectiles from this gun)
        public string ReloadSound { get; set; }
        public float ReloadTime { get; set; }
        protected float fireCooldownDelta;                  //The remaining delta for shooting
        protected float reloadCooldownDelta;                //The remaining delta for reloading
        protected bool ableToFire;                          //bool to check is you're able to fire again
        protected bool ableToReload;                        //bool to check is you're able to reload again
        private readonly string locationBottom;
        private readonly string locationLeft;
        private readonly string locationRight;
        private readonly string locationTop;

        public BulletBill()
        {
            // constructor for the BulletBill class
            Name = "Bullet Bill";
            Description = "The Bullet Bill is a big bullet that goes in a straight line until it hits a wall";
            CurrentClip = 0;
            ClipAmount = 0;
            ClipMax = 1;
            Damage = 350;
            Accuracy = 0;
            FireTime = 2000;
            CritChance = 0.15;
            CritMultiplier = 2;
            WeaponLevel = 1;
            Range = 1000;
            ReloadTime = 5000;
            ShotSound = "Weapon_Sounds\\Bullet_Bill_Shot1.wav";
            locationBottom = "Assets\\Sprites\\Bullet_Sprites\\Bullet_Bill_Bottom.gif";
            locationLeft = "Assets\\Sprites\\Bullet_Sprites\\Bullet_Bill_Left.gif";
            locationRight = "Assets\\Sprites\\Bullet_Sprites\\Bullet_Bill_Right.gif";
            locationTop = "Assets\\Sprites\\Bullet_Sprites\\Bullet_Bill_Top.gif";

            ableToReload = false;
            ableToFire = true;
            fireCooldownDelta = 0;
            reloadCooldownDelta = 5000;
        }

        private float getProjectileDamage(float damage, float change, float multiplier, Random random)
        {
            //Determine if its a critical hit if the generated number is lower then the crid change times 100
            if (random.Next(0, 101) < (change * 100))
            {
                damage = damage * multiplier;
            }
            return damage;
        }

        public void AddTag(string tag)
        {
            // add a tag to the tags list
            Tags.Add(tag);
        }

        public bool Fire(float fromLeft, float fromTop, float width, float height, List<GameObject> gameObjects, string direction)
        {
            if (ableToReload && CurrentClip == 0)
            {
                Reload();
            }

            Random random = new Random();
            //Random.next first int is inclusive the second is excusive, due to this the half of the accuracy devided by 2 is added.
            //Get a number between the accuracy and the accuracy * -1.
            //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
            float randomPositionOffset = (random.Next((int)(Accuracy * -1) * 100, (int)Accuracy * 100) + Accuracy / 2) / 100;

            float projectileDamage = getProjectileDamage((float)Damage, (float)CritChance, (float)CritMultiplier, random);

            // fire one bullet
            if (ableToFire && CurrentClip > 0)
            {
                Projectile projectile;
                if (direction == "Top")
                {
                    projectile = new Projectile(23, 27, fromLeft - 11.5f, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset - 11.5f, fromTop - height, Range);
                    projectile.SetLocation(locationTop);
                }
                else if (direction == "Bottom")
                {
                    projectile = new Projectile(23, 27, fromLeft - 11.5f, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset - 11.5f, fromTop + height, Range);
                    projectile.SetLocation(locationBottom);
                }
                else if (direction == "Left")
                {
                    projectile = new Projectile(23, 27, fromLeft, fromTop - 11.5f, 0, 0, 0, 0, projectileDamage, fromLeft - height, fromTop + randomPositionOffset - 11.5f, Range);
                    projectile.SetLocation(locationLeft);
                }
                else //Right
                {
                    projectile = new Projectile(23, 27, fromLeft, fromTop - 11.5f, 0, 0, 0, 0, projectileDamage, fromLeft + height, fromTop + randomPositionOffset - 11.5f, Range);
                    projectile.SetLocation(locationRight);
                }

                if (projectileDamage > Damage)
                {
                    projectile.AddTag("crit");
                }

                projectile.AddTag("ghost");
                gameObjects.Add(projectile);

                CurrentClip--;
                ableToFire = false;
                return true;
            }

            if (CurrentClip == 0 && ClipAmount == 0)
            {
                MainPage.Current.WeaponEmpty(Name);
            }
            return false;
        }

        public void Reload()
        {
            // reload this weapon, but only if you have enough clips
            if (ableToReload && ClipAmount > 0)
            {
                ableToFire = false;
                ClipAmount--;
                CurrentClip = ClipMax;
                MainPage.Current.GetWeaponStats();
                MainPage.Current.UpdateCurrentClip();
                ableToReload = false;
            }
        }

        public void Upgrade()
        {
            // upgrade weapon level for a stronger weapon
            WeaponLevel++;
            Damage += 25;
            FireTime *= 0.99f;
            ClipMax += 1;
            ReloadTime *= 0.99f;
            CritChance += 0.015;
            if (CritChance > 0.75)
            {
                CritChance = 0.75;
            }
            CritMultiplier += 0.1;
        }

        public int GetAmmo()
        {
            return ClipAmount * ClipMax + CurrentClip;
        }

        public bool OnTick(float delta)
        {
            if (fireCooldownDelta - delta < 0)
            {
                fireCooldownDelta = FireTime;
                ableToFire = true;
            }
            else
            {
                fireCooldownDelta -= delta;
            }

            if (reloadCooldownDelta - delta < 0)
            {
                reloadCooldownDelta = ReloadTime;
                ableToReload = true;
                ableToFire = true;
            }
            else if (CurrentClip <= 0)
            {
                reloadCooldownDelta -= delta;
            }
            return true;
        }
    }
}

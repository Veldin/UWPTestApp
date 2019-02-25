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

        public BulletBill()
        {
            // constructor for the BulletBill class
            name = "Bullet Bill";
            description = "The Bullet Bill is a big bullet that goes in a straight line until it hits a wall";
            currentClip = 0;
            clipAmount = 0;
            clipMax = 1;
            damage = 350;
            accuracy = 0;
            fireTime = 2000;
            critChance = 0.15;
            critMultiplier = 2;
            weaponLevel = 1;
            reloadTime = 5000;
            shotSound = "Weapon_Sounds\\Bullet_Bill_Shot1.wav";
            locationBottom = "Assets\\Sprites\\Bullet_Sprites\\Bullet_Bill_Bottom.png";
            locationLeft = "Assets\\Sprites\\Bullet_Sprites\\Bullet_Bill_Left.png";
            locationRight = "Assets\\Sprites\\Bullet_Sprites\\Bullet_Bill_Right.png";
            locationTop = "Assets\\Sprites\\Bullet_Sprites\\Bullet_Bill_Top.png";

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

                //If it was a crit call this function again to be able to have double, tripple, quad, ect crits.
//                return getProjectileDamage(damage, change, multiplier, random);
            }

            return damage;
        }

        public void AddTag(string tag)
        {
            // add a tag to the tags list
            tags.Add(tag);
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
                    projectile = new Projectile(23, 27, fromLeft - 11.5f, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset - 11.5f, fromTop - height);
                    projectile.SetLocation(locationTop);

                }
                else if (direction == "Bottom")
                {
                    projectile = new Projectile(23, 27, fromLeft - 11.5f, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset - 11.5f, fromTop + height);
                    projectile.SetLocation(locationBottom);
                }
                else if (direction == "Left")
                {
                    projectile = new Projectile(23, 27, fromLeft, fromTop - 11.5f, 0, 0, 0, 0, projectileDamage, fromLeft - height, fromTop + randomPositionOffset - 11.5f);
                    projectile.SetLocation(locationLeft);
                }
                else //Right
                {
                    projectile = new Projectile(23, 27, fromLeft, fromTop - 11.5f, 0, 0, 0, 0, projectileDamage, fromLeft + height, fromTop + randomPositionOffset - 11.5f);
                    projectile.SetLocation(locationRight);
                }

                if (projectileDamage > damage)
                {
                    projectile.AddTag("crit");
                }

                projectile.AddTag("ghost");
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
            damage += 25;
            fireTime *= 0.99f;
            clipMax += 1;
            reloadTime *= 0.99f;
            critChance += 0.015;
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

using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class DessertBeagle : Weapon
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

        public DessertBeagle()
        {
            // constructor for the DessertBeagle class
            name = "Dessert Beagle";
            description = "The Dessert Beagle is a strong handgun, also known as the Desert Eagle";
            currentClip = 0;
            clipAmount = 5;
            clipMax = 12;
            damage = 50;
            accuracy = 1;
            fireTime = 1000;
            critChance = 0.05;
            critMultiplier = 1.5;
            weaponLevel = 1;
            reloadTime = 3000;
            shotSound = "Weapon_Sounds\\Dessert_Beagle_Shot1.wav";
            location = "Assets\\Sprites\\Bullet_Sprites\\Projectile_Sprite.png";

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
                damage = damage* multiplier;
            }

            return damage;
        }

        public bool Fire(float fromLeft, float fromTop, float width, float height, List<GameObject> gameObjects , String direction)
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
                    var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop - height);
                    projectile.SetLocation(location);
                    gameObjects.Add(projectile);
                }
                else if (direction == "Bottom")
                {
                    var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop + height);
                    projectile.SetLocation(location);
                    gameObjects.Add(projectile);
                }
                else if (direction == "Left")
                {
                    var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft - height, fromTop + randomPositionOffset);
                    projectile.SetLocation(location);
                    gameObjects.Add(projectile);
                }
                else //Right
                {
                    var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + height, fromTop + randomPositionOffset);
                    projectile.SetLocation(location);
                    gameObjects.Add(projectile);
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
            clipMax += 1;
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
            else if(currentClip <= 0)
            {
                reloadCooldownDelta -= delta;
            }

            return true;
        }
    }
}

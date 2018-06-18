using System;
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
        private string locationBottom;
        private string locationLeft;
        private string locationRight;
        private string locationTop;

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
            locationBottom = "Assets\\Sprites\\Bullet_Sprites\\Homers_Bullets_Sprite.png";
            locationLeft = "Assets\\Sprites\\Bullet_Sprites\\Homers_Bullets_Sprite.png";
            locationRight = "Assets\\Sprites\\Bullet_Sprites\\Homers_Bullets_Sprite.png";
            locationTop = "Assets\\Sprites\\Bullet_Sprites\\Homers_Bullets_Sprite.png";
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

        private float getProjectileDamage(float damage, float change, float multiplier, Random random)
        {
            //Determine if its a critical hit if the generated number is lower then the crid change times 100
            if (random.Next(0, 101) < (change * 100))
            {
                damage = damage * multiplier;

                //If it was a crit call this function again to be able to have double, tripple, quad, ect crits.
                return getProjectileDamage(damage, change, multiplier, random);
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
            float randomPositionOffset = random.Next((int)(accuracy * -1), (int)accuracy) + accuracy / 2;

            float projectileDamage = getProjectileDamage((float)damage, (float)critChance, (float)critMultiplier, random);

            // fire one bullet
            if (ableToFire && currentClip > 0)
            {

                if (direction == "Top")
                {
                    var projectileTop = new Projectile(10, 14, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop - height);
                    projectileTop.SetLocation(locationTop);
                    gameObjects.Add(projectileTop);

                }
                else if (direction == "Bottom")
                {
                    var projectileBottom = new Projectile(10, 14, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop + height);
                    projectileBottom.SetLocation(locationBottom);
                    gameObjects.Add(projectileBottom);
                }
                else if (direction == "Left")
                {
                    var projectileLeft = new Projectile(10, 14, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft - height, fromTop + randomPositionOffset);
                    projectileLeft.SetLocation(locationLeft);
                    gameObjects.Add(projectileLeft);
                }
                else //Right
                {
                    var projectileRight = new Projectile(10, 14, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + height, fromTop + randomPositionOffset);
                    projectileRight.SetLocation(locationRight);
                    gameObjects.Add(projectileRight);
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

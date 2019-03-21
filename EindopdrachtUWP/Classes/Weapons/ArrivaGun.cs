using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class ArrivaGun : IWeapon
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

        public ArrivaGun()
        {
            // constructor for the ArrivaGun class
            Name = "Arriva Gun";
            Description = "The Arriva Gun is a long range weapon which hits all enemies in its path (like a railgun)";
            CurrentClip = 0;
            ClipAmount = 0;
            ClipMax = 2;
            Damage = 250;
            Accuracy = 0;
            FireTime = 6000;
            CritChance = 0.25;
            CritMultiplier = 2;
            WeaponLevel = 1;
            Range = 1000;
            ReloadTime = 8000;
            ShotSound = "Weapon_Sounds\\Arriva_Gun_Shot1.wav";
            locationBottom = "Assets\\Sprites\\Bullet_Sprites\\Arriva_Gun_Bottom.gif";
            locationLeft = "Assets\\Sprites\\Bullet_Sprites\\Arriva_Gun_Left.gif";
            locationRight = "Assets\\Sprites\\Bullet_Sprites\\Arriva_Gun_Right.gif";
            locationTop = "Assets\\Sprites\\Bullet_Sprites\\Arriva_Gun_Top.gif";

            ableToReload = false;
            ableToFire = true;
            fireCooldownDelta = 0;
            reloadCooldownDelta = 3000;
        }

        public void AddTag(string tag)
        {
            // add a tag to the tags list
            Tags.Add(tag);
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
                    projectile = new Projectile(8, 15, fromLeft, fromTop - 7, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop - height, Range);
                    projectile.SetLocation(locationTop);
                }
                else if (direction == "Bottom")
                {
                    projectile = new Projectile(8, 15, fromLeft, fromTop - 7, 0, 0, 0, 0, projectileDamage, fromLeft + randomPositionOffset, fromTop + height, Range);
                    projectile.SetLocation(locationBottom);
                }
                else if (direction == "Left")
                {
                    projectile = new Projectile(15, 8, fromLeft - 7, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft - height, fromTop + randomPositionOffset, Range);
                    projectile.SetLocation(locationLeft);
                }
                else //Right
                {
                    projectile = new Projectile(15, 8, fromLeft - 7, fromTop, 0, 0, 0, 0, projectileDamage, fromLeft + height, fromTop + randomPositionOffset, Range);
                    projectile.SetLocation(locationRight);
                }

                if (projectileDamage > Damage)
                {
                    projectile.AddTag("crit");
                }

                projectile.AddTag("ghost");
                projectile.AddTag("laser");
                projectile.SetMovementSpeed(2200);
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
            Damage += 10;
            FireTime *= 0.99f;
            ClipMax += 1;
            ReloadTime *= 0.99f;
            CritChance += 0.0625;
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

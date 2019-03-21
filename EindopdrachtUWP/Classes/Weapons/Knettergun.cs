using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class Knettergun : IWeapon
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
        public float Range { get; set; }        // The range of the gun (this is the distanceTillDestroyed value of all projectiles from this gun)
        public string ShotSound { get; set; }
        public string ReloadSound { get; set; }
        public float ReloadTime { get; set; }
        protected float fireCooldownDelta;                  //The remaining delta for shooting
        protected float reloadCooldownDelta;                //The remaining delta for reloading
        protected bool ableToFire;                          //bool to check is you're able to fire again
        protected bool ableToReload;                        //bool to check is you're able to reload again
        private readonly string location;

        public Knettergun()
        {
            // constructor for the Knettergun class
            Name = "Knettergun";
            Description = "The Knettergun is a strong short ranged weapon, also known as a shotgun";
            CurrentClip = 0;
            ClipAmount = 0;
            ClipMax = 6;
            Damage = 400;
            Accuracy = 3.3f;
            FireTime = 3000;
            CritChance = 0.03;
            CritMultiplier = 1.2;
            WeaponLevel = 1;
            Range = 125;
            ReloadTime = 4000;
            ShotSound = "Weapon_Sounds\\Knetter_Gun_Shot1.wav";
            location = "Assets\\Sprites\\Bullet_Sprites\\Projectile_Sprite.gif";

            ableToReload = false;
            ableToFire = true;
            fireCooldownDelta = 0;
            reloadCooldownDelta = 4000;
        }

        public void AddTag(string tag)
        {
            // add a tag to the tags list
            Tags.Add(tag);
        }

        private float GetProjectileDamage(float damage, float change, float multiplier, Random random)
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

            float projectileDamage = GetProjectileDamage((float)Damage, (float)CritChance, (float)CritMultiplier, random);

            // fire one bullet
            if (ableToFire && CurrentClip > 0)
            {
                if (direction == "Top")
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
                        float randomPositionOffset = (random.Next((int)(Accuracy * -1) * 100, (int)Accuracy * 100) + Accuracy / 2) / 100;
                        Projectile projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage / 12, fromLeft + randomPositionOffset, fromTop - height, Range + random.Next(50));
                        projectile.SetLocation(location);
                        if (projectileDamage > Damage)
                        {
                            projectile.AddTag("crit");
                        }

                        // Randomizes the movementspeed of the shot Projectile 
                        projectile.SetMovementSpeed(650 + random.Next(100));

                        gameObjects.Add(projectile);
                    }
                }
                else if (direction == "Bottom")
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
                        float randomPositionOffset = (random.Next((int)(Accuracy * -1) * 100, (int)Accuracy * 100) + Accuracy / 2) / 100;
                        Projectile projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage / 12, fromLeft + randomPositionOffset, fromTop + height, Range + random.Next(50));
                        projectile.SetLocation(location);
                        if (projectileDamage > Damage)
                        {
                            projectile.AddTag("crit");
                        }

                        // Randomizes the movementspeed of the shot Projectile 
                        projectile.SetMovementSpeed(650 + random.Next(100));

                        gameObjects.Add(projectile);
                    }
                }
                else if (direction == "Left")
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
                        float randomPositionOffset = (random.Next((int)(Accuracy * -1) * 100, (int)Accuracy * 100) + Accuracy / 2) / 100;
                        Projectile projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage / 12, fromLeft - height, fromTop + randomPositionOffset, Range + random.Next(50));
                        projectile.SetLocation(location);
                        if (projectileDamage > Damage)
                        {
                            projectile.AddTag("crit");
                        }

                        // Randomizes the movementspeed of the shot Projectile 
                        projectile.SetMovementSpeed(650 + random.Next(100));

                        gameObjects.Add(projectile);
                    }
                }
                else //Right
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
                        float randomPositionOffset = (random.Next((int)(Accuracy * -1) * 100, (int)Accuracy * 100) + Accuracy / 2) / 100;
                        Projectile projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage / 12, fromLeft + height, fromTop + randomPositionOffset, Range + random.Next(50));
                        projectile.SetLocation(location);
                        if (projectileDamage > Damage)
                        {
                            projectile.AddTag("crit");
                        }

                        // Randomizes the movementspeed of the shot Projectile 
                        projectile.SetMovementSpeed(650 + random.Next(100));

                        gameObjects.Add(projectile);
                    }
                }

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
            Damage += 20;
            FireTime *= 0.99f;
            ClipMax += 1;
            ReloadTime *= 0.99f;
            CritChance += 0.01;
            if (CritChance > 0.75)
            {
                CritChance = 0.75;
            }
            CritMultiplier += 0.05;
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

using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes.Weapons
{
    class Knettergun : Weapon
    {
        public List<string> tags        { get; }
        public string name              { get; set; }
        public string description       { get; set; }
        public int currentClip          { get; set; }
        public int clipAmount           { get; set; }
        public int clipMax              { get; set; }
        public float damage             { get; set; }
        public float accuracy           { get; set; }
        public float fireTime           { get; set; }
        public double critChance        { get; set; }
        public double critMultiplier    { get; set; }
        public int weaponLevel          { get; set; }
        public float range              { get; set; }        // The range of the gun (this is the distanceTillDestroyed value of all projectiles from this gun)
        public string shotSound         { get; set; }
        public string reloadSound       { get; set; }
        public float reloadTime         { get; set; }
        protected float fireCooldownDelta;                  //The remaining delta for shooting
        protected float reloadCooldownDelta;                //The remaining delta for reloading
        protected bool ableToFire;                          //bool to check is you're able to fire again
        protected bool ableToReload;                        //bool to check is you're able to reload again
        private string location;

        public Knettergun()
        {
            // constructor for the Knettergun class
            name                = "Knettergun";
            description         = "The Knettergun is a strong short ranged weapon, also known as a shotgun";
            currentClip         = 0;
            clipAmount          = 0;
            clipMax             = 6;
            damage              = 400;
            accuracy            = 3.3f;
            fireTime            = 3000;
            critChance          = 0.03;
            critMultiplier      = 1.2;
            weaponLevel         = 1;
            range               = 1000;
            reloadTime          = 4000;
            shotSound           = "Weapon_Sounds\\Knetter_Gun_Shot1.wav";
            location            = "Assets\\Sprites\\Bullet_Sprites\\Projectile_Sprite.png";

            ableToReload        = false;
            ableToFire          = true;
            fireCooldownDelta   = 0;
            reloadCooldownDelta = 4000;
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

        public bool Fire(float fromLeft, float fromTop, float width, float height, List<GameObject> gameObjects, string direction)
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

            float projectileDamage = getProjectileDamage((float)damage, (float)critChance, (float)critMultiplier, random);

            // fire one bullet
            if (ableToFire && currentClip > 0)
            {

                if (direction == "Top")
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
                        float randomPositionOffset = (random.Next((int)(accuracy * -1) * 100, (int)accuracy * 100) + accuracy / 2) / 100;
                        var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage/12, fromLeft + randomPositionOffset, fromTop - height, range);
                        projectile.SetLocation(location);
                        if (projectileDamage > damage)
                        {
                            projectile.AddTag("crit");
                        }
                        gameObjects.Add(projectile);
                    }
                }
                else if (direction == "Bottom")
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
                        float randomPositionOffset = (random.Next((int)(accuracy * -1) * 100, (int)accuracy * 100) + accuracy / 2) / 100;
                        var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage/12, fromLeft + randomPositionOffset, fromTop + height, range);
                        projectile.SetLocation(location);
                        if (projectileDamage > damage)
                        {
                            projectile.AddTag("crit");
                        }
                        gameObjects.Add(projectile);
                    }
                }
                else if (direction == "Left")
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
                        float randomPositionOffset = (random.Next((int)(accuracy * -1) * 100, (int)accuracy * 100) + accuracy / 2) / 100;
                        var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage/12, fromLeft - height, fromTop + randomPositionOffset, range);
                        projectile.SetLocation(location);
                        if (projectileDamage > damage)
                        {
                            projectile.AddTag("crit");
                        }
                        gameObjects.Add(projectile);
                    }
                }
                else //Right
                {
                    for (int i = 0; i < 12; i++)
                    {
                        //The random.next can only give ints back, this means its always rounded. To counter this the ints given are multiplied by 100, and the results devided by 100
                        float randomPositionOffset = (random.Next((int)(accuracy * -1) * 100, (int)accuracy * 100) + accuracy / 2) / 100;
                        var projectile = new Projectile(3, 3, fromLeft, fromTop, 0, 0, 0, 0, projectileDamage/12, fromLeft + height, fromTop + randomPositionOffset, range);
                        projectile.SetLocation(location);
                        if (projectileDamage > damage)
                        {
                            projectile.AddTag("crit");
                        }
                        gameObjects.Add(projectile);
                    }
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
            critChance += 0.01;
            if (critChance > 0.75)
            {
                critChance = 0.75;
            }
            critMultiplier += 0.05;
        }

        public int GetAmmo()
        {
            return clipAmount * clipMax + currentClip;
        }

        public bool OnTick(float delta)
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

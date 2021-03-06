﻿using EindopdrachtUWP;
using EindopdrachtUWP.Classes;
using EindopdrachtUWP.Classes.Weapons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UWPTestApp
{
    public class Player : GameObject, MovableObject, Targetable
    {
        private float walkSpeed;
        private float maxHealth;
        private float health;
        private float maxArmour;
        private float armour;
        private int level;

        public int Kills { get; set; }

        public string DeathSound { get; set; }
        public string MoveSound { get; set; }
        public string HitSound { get; set; }
        public string HealthLowSound { get; set; }

        public bool IsWalking { get; set; }

        public float selectNextWeaponDelay { get; set; }
        private float selectNextWeaponDelayMax;

        //Gun stuff
        private List<Weapon> weapons;
        public Weapon activeWeapon;

        private String direction;
        
        public float deltaForWalkingSound { get; set; }
        public float deltaForHealthLowSound { get; set; }


        public Player(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
            : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {

            AddTag("solid");

            DeathSound = "Generic_Sounds\\Player_Death_Sound.wav";
            MoveSound = "Generic_Sounds\\Player_Movement_Sound.wav";
            HitSound = "Generic_Sounds\\Player_Hit_Sound.wav";
            HealthLowSound = "Generic_Sounds\\Health_Low_Sound.wav";

            walkSpeed = 300;
            health = 300;
            maxHealth = 300;
            armour = 0;
            maxArmour = 150;
            level = 1;
            Kills = 0;
            direction = "Bottom";

            selectNextWeaponDelayMax = 1000;

            weapons = new List<Weapon>();

            weapons.Add(new DessertBeagle());
            weapons.Add(new KA74());
            weapons.Add(new Knettergun());
            weapons.Add(new UWP());
            weapons.Add(new FlameThrower());
            weapons.Add(new VLEKKannon());
            weapons.Add(new BulletBill());
            weapons.Add(new ArrivaGun());
            weapons.Add(new Batarang());
            weapons.Add(new HomersBullets());

            activeWeapon = weapons[0];

            Target = new Target(FromLeft, FromTop);
        }

        public Boolean selectNextWeapon()
        {
            //If you recently switched weapons this is higher then 0
            if (selectNextWeaponDelay > 0)
            {
                return false;
            }

            Boolean found = false;
            foreach (Weapon selected in weapons)
            {
                if (activeWeapon == selected)
                {
                    found = true;
                }else if (found)
                {
                    activeWeapon = selected;
                    selectNextWeaponDelay = selectNextWeaponDelayMax;
                    return true;
                }
            }
            activeWeapon = weapons[0];

            selectNextWeaponDelay = selectNextWeaponDelayMax;
            return true;
        }

        public Boolean selectPreviousWeapon()
        {
            if (selectNextWeaponDelay > 0)
            {
                return false;
            }
            
            for (int i = 0; i < weapons.Count(); i++)
            {
                if (activeWeapon == weapons[i])
                {
                    if (i - 1 < 0)
                    {
                        activeWeapon = weapons[9];
                    }
                    else
                    {
                        activeWeapon = weapons[i - 1];
                    }
                    selectNextWeaponDelay = selectNextWeaponDelayMax;
                    return true;
                }
            }
            activeWeapon = weapons[0];

            selectNextWeaponDelay = selectNextWeaponDelayMax;
            return true;
        }

        public void IncreaseHealth(float amount)
        {
            health += amount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            if (HasTag("health_low") && health >= 100)
            {
                RemoveTag("health_low");
            }
            else if (!HasTag("health_low") && health < 100)
            {
                AddTag("health_low");
            }
            MainPage.Current.updateHealth();
        }

        public void IncreaseArmour(float amount)
        {
            armour += amount;
            if (armour > maxArmour)
            {
                armour = maxArmour;
            }
            MainPage.Current.updateArmour();
        }

        public void setArmour(float amount)
        {
            armour = amount;
        }

        public float getArmour()
        {
            return armour;
        }

        public float getMaxArmour()
        {
            return maxArmour;
        }

        public float getHealth()
        {
            return health;
        }

        public float getMaxHealth()
        {
            return maxHealth;
        }

        //Every x amount of kills the player is leveled. Their max HP gets increased by 25, HP is filled to max and visuals get updated.
        public void IncreaseLevel()
        {
            level++;
            increaseMaxHealth(25);
            increaseMaxArmour(25);
            IncreaseHealth(maxHealth);
            AddTag("levelup");
        }

        public void increaseMaxHealth(float amount)
        {
            maxHealth += amount;
            MainPage.Current.updateHealth();
        }

        public void increaseMaxArmour(float amount)
        {
            maxArmour += amount;
            MainPage.Current.updateArmour();
        }

        public int GetLevel() => level;

        float MovableObject.GetMovementSpeed()
        {
            return walkSpeed;
        }
        
        void MovableObject.SetMovementSpeed(float speed)
        {
            walkSpeed = speed;
        }

        public Boolean Fire(String direction, List<GameObject> gameObjects)
        {
            this.direction = direction;
            MainPage.Current.UpdateCurrentClip();
            return activeWeapon.Fire(fromLeft + (width / 2), fromTop + (height / 2), width, height, gameObjects, direction);
        }

        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {

            if (HasTag("levelup"))
            {
                gameObjects.Add (new TextBox(75, 50, fromLeft, fromTop - 20, 0, 0, 0, 0, "Level Up!", 1000));
                RemoveTag("levelup");
            }

            if (health <= 0)
            {
                AddTag("destroyed");
                MainPage.Current.muteMusic();
                MainPage.Current.gameover();
            }

            selectNextWeaponDelay -= delta;

            String locationString = "Assets/Sprites/Player_Sprites/";

            if (activeWeapon is ArrivaGun)
            {
                locationString += "Arriva_Gun";
            }
            else if (activeWeapon is Batarang)
            {
                locationString += "Batarang";
            }
            else if (activeWeapon is BulletBill)
            {
                locationString += "Bullet_Bill";
            }
            else if (activeWeapon is DessertBeagle)
            {
                locationString += "Dessert_Beagle";
            }
            else if (activeWeapon is FlameThrower)
            {
                locationString += "Flamethrower";
            }
            else if (activeWeapon is HomersBullets)
            {
                locationString += "Homers_Bullets";
            }
            else if (activeWeapon is KA74)
            {
                locationString += "KA74";
            }
            else if (activeWeapon is Knettergun)
            {
                locationString += "Knetter_Gun";
            }
            else if (activeWeapon is UWP)
            {
                locationString += "UWP";
            }
            else if (activeWeapon is VLEKKannon)
            {
                locationString += "VLEKKannon";
            }

            locationString += "_" + direction + ".png";

            if (locationString != Location)
            {
                Location = locationString;
                Sprite = null;
            }
			
            activeWeapon.OnTick(delta);

            float differenceLeftAbs = Math.Abs(Target.FromLeft() - FromLeft);
            float differenceTopAbs = Math.Abs(Target.FromTop() - FromTop);

            float totalDifferenceAbs = differenceLeftAbs + differenceTopAbs;

            float differenceTopPercent = differenceTopAbs / (totalDifferenceAbs / 100);
            float differenceLeftPercent = differenceLeftAbs / (totalDifferenceAbs / 100);

            float moveTopDistance = walkSpeed * (differenceTopPercent / 100);
            float moveLeftDistance = walkSpeed * (differenceLeftPercent / 100);

            //Due to players being able to stand in himself only greater then or smaller then need to be checked.
            if (Target.FromLeft() > FromLeft)
            {
                AddFromLeft((moveLeftDistance * delta) / 10000);
            }
            else if(Target.FromLeft() < FromLeft) 
            {
                AddFromLeft(((moveLeftDistance * delta) / 10000) * -1);
            }

            if (Target.FromTop() > FromTop)
            {
                AddFromTop((moveTopDistance * delta) / 10000);
            }
            else if(Target.FromTop() < FromTop)
            {
                AddFromTop(((moveTopDistance * delta) / 10000) * -1);
            }

            //Reset the target to the current location, So if no buttons are pressed no movement is done
            Target.SetTarget(fromLeft,fromTop);
            return true;
        }

        public override Boolean CollitionEffect(GameObject gameObject)
        {
            if (gameObject.HasTag("solid"))
            {
                //Check collition from the left or right.
                if ((gameObject.FromLeft + gameObject.Width) > (FromLeft + Width))
                {
                    AddFromLeft(-1);
                }
                else if ((gameObject.FromLeft + gameObject.Width) < (FromLeft + Width))
                {
                    AddFromLeft(1);
                }

                //Check collition from top or bottom.
                if ((gameObject.FromTop + gameObject.Height) > (FromTop + Height))
                {
                    AddFromTop(-1);
                }
                else if ((gameObject.FromTop + gameObject.Height) < (FromTop + Height))
                {
                    AddFromTop(1);
                }
            }

            //If a player is coliding with an object their collitionEffect is triggered instantly and not after this resolves.
            //This is so the collition of the enemy still goes even thought they are not colliding anymore.
            gameObject.CollitionEffect(this);
            return true;
        }
        


        float Targetable.FromTop()
        {
            return FromTop;
        }

        float Targetable.FromLeft()
        {
            return FromLeft;
        }

        public List<Weapon> GetWeapons()
        {
            return weapons;
        }

        public Weapon GetActiveWeapon()
        {
            return activeWeapon;
        }
    }
}
using EindopdrachtUWP;
using EindopdrachtUWP.Classes;
using EindopdrachtUWP.Classes.Weapons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UWPTestApp;
using Windows.UI.Xaml.Controls;

namespace UWPTestApp
{
    public class Player : GameObject, MovableObject, Targetable
    {
        private float walkSpeed;
        private int health;
        private int armor;
        private int level;
        private MediaElement deathSound;
        private MediaElement moveSound;

        private float selectNextWeaponDelay;
        private float selectNextWeaponDelayMax;

        private String direction;

        public Player(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
            : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {

            AddTag("solid");

            walkSpeed = 300;
            health = 300;
            armor = 0;
            armor = 0;
            level = 1;

            direction = "Top";

            selectNextWeaponDelayMax = 1000;

            //this.Location = "Assets/Sprites/Player_Sprites/Arriva_Gun_Bottom.png";

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

        //Gun stuff
        private List<Weapon> weapons;
        private Weapon activeWeapon;

        public string getActiveWeaponName()
        {
            return activeWeapon.name;
        }

        public string selectNextWeapon()
        {
            //If you recently switched weapons this is higher then 0
            if (selectNextWeaponDelay > 0)
            {
                return activeWeapon.name;
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
                    return activeWeapon.name;
                }
            }
            activeWeapon = weapons[0];

            selectNextWeaponDelay = selectNextWeaponDelayMax;
            return activeWeapon.name;
        }
        //

        public void IncreaseHealth(int amount)
        {
            health += amount;
        }

        public void IncreaseArmor(int amount)
        {
            armor += amount;
        }

        public void IncreaseLevel()
        {
            level++;
        }

        public int GetLevel() => level;

        float MovableObject.GetMovementSpeed()
        {
            return walkSpeed;
        }

        void MovableObject.SetMoveSound(MediaElement moveSound)
        {
            this.moveSound = moveSound;
        }

        void MovableObject.PlayMoveSound()
        {
            // to be implemented
        }

        void MovableObject.SetDeathSound(MediaElement deathSound)
        {
            this.deathSound = deathSound;
        }

        void MovableObject.PlayDeathSound()
        {
            // to be implemented
        }

        void MovableObject.SetMovementSpeed(float speed)
        {
            walkSpeed = speed;
        }

        public Boolean Fire(String direction, List<GameObject> gameObjects)
        {
            this.direction = direction;

            
            return this.activeWeapon.Fire(fromLeft + (width / 2), fromTop + (height / 2), width, height, gameObjects, direction); 
        }

        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {
            selectNextWeaponDelay -= delta;
            //activeWeapon.OnTick(delta, delta);

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
            //totalDifference = differenceTop + differenceLeft;
            //totalDifferenceAbs = differenceTopAbs + differenceLeftAbs;

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

            if (gameObject.HasTag("hostile"))
            {
                // PlayHitSound();
            }


            //If a player is coliding with an object their collitionEffect is triggered instantly and not after this resolves.
            //This is so the collition of the enemy still goes even thought they are not colliding anymore.
            //This also lets you "push" away your enemies. (Because they act like a solid just apeared in them)
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
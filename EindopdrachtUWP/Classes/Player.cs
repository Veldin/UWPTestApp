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

        public Player(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
            : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {

            AddTag("solid");

            walkSpeed = 200;
            health = 300;
            armor = 0;
            armor = 0;
            level = 1;

            this.Location = "Assets/Sprites/Player_Sprites/Arriva_Gun_Bottom.png";

            weapons = new List<Weapon>();

            weapons.Add(new ArrivaGun());
            weapons.Add(new Batarang());
            weapons.Add(new BulletBill());
            weapons.Add(new DessertBeagle());
            weapons.Add(new FlameThrower());
            weapons.Add(new HomersBullets());
            weapons.Add(new KA74());
            weapons.Add(new Knettergun());
            weapons.Add(new UWP());
            weapons.Add(new VLEKKannon());

            activeWeapon = weapons[0];
        }

        //Gun stuff
        private List<Weapon> weapons;
        private Weapon activeWeapon;
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

        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {
            //throw new NotImplementedException();

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
    }
}
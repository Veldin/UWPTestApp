using EindopdrachtUWP;
using EindopdrachtUWP.Classes;
using EindopdrachtUWP.Classes.Weapons;
using System;
using System.Collections.Generic;
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

        public float SelectNextWeaponDelay { get; set; }

        //Gun stuff
        private List<Weapon> weapons;
        public Weapon activeWeapon;
        
        public float DeltaForWalkingSound { get; set; }
        public float DeltaForHealthLowSound { get; set; }
        public float SelectNextWeaponDelayMax { get; set; }

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
            Direction = "Bottom";

            SelectNextWeaponDelayMax = 1000;

            weapons = new List<Weapon>
            {
                new DessertBeagle(),
                new KA74(),
                new Knettergun(),
                new UWP(),
                new FlameThrower(),
                new VLEKKannon(),
                new BulletBill(),
                new ArrivaGun(),
                new Batarang(),
                new HomersBullets()
            };

            activeWeapon = weapons[0];

            Target = new Target(FromLeft, FromTop);
            //Target = new Target(this);
        }

        public bool SelectNextWeapon()
        {
            //If you recently switched weapons this is higher then 0
            if (SelectNextWeaponDelay > 0)
            {
                return false;
            }

            bool found = false;
            foreach (Weapon selected in weapons)
            {
                if (activeWeapon == selected)
                {
                    found = true;
                }else if (found)
                {
                    activeWeapon = selected;
                    SelectNextWeaponDelay = SelectNextWeaponDelayMax;
                    return true;
                }
            }
            activeWeapon = weapons[0];

            SelectNextWeaponDelay = SelectNextWeaponDelayMax;
            return true;
        }

        public bool SelectPreviousWeapon()
        {
            if (SelectNextWeaponDelay > 0)
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
                    SelectNextWeaponDelay = SelectNextWeaponDelayMax;
                    return true;
                }
            }
            activeWeapon = weapons[0];

            SelectNextWeaponDelay = SelectNextWeaponDelayMax;
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
            MainPage.Current.UpdateHealth();
        }

        public void IncreaseArmour(float amount)
        {
            armour += amount;
            if (armour > maxArmour)
            {
                armour = maxArmour;
            }
            MainPage.Current.UpdateArmour();
        }

        public void SetArmour(float amount)
        {
            armour = amount;
        }

        public float GetArmour()
        {
            return armour;
        }

        public float GetMaxArmour()
        {
            return maxArmour;
        }

        public float GetHealth()
        {
            return health;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        //Every x amount of kills the player is leveled. Their max HP gets increased by 25, HP is filled to max and visuals get updated.
        public void IncreaseLevel()
        {
            level++;
            IncreaseMaxHealth(25);
            IncreaseMaxArmour(25);
            IncreaseHealth(maxHealth);
            AddTag("levelup");
        }

        public void IncreaseMaxHealth(float amount)
        {
            maxHealth += amount;
            MainPage.Current.UpdateHealth();
        }

        public void IncreaseMaxArmour(float amount)
        {
            maxArmour += amount;
            MainPage.Current.UpdateArmour();
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

        public bool Fire(string direction, List<GameObject> gameObjects)
        {
            this.Direction = direction;
            MainPage.Current.UpdateCurrentClip();
            return activeWeapon.Fire(fromLeft + (width / 2), fromTop + (height / 2), width, height, gameObjects, direction);
        }

        public override bool IsActive(GameObject gameObject)
        {
            return true;
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
                MainPage.Current.MuteMusic();
                MainPage.Current.Gameover();
            }

            SelectNextWeaponDelay -= delta;

            string locationstring = "Assets/Sprites/Player_Sprites/";

            if (activeWeapon is ArrivaGun)
            {
                locationstring += "Arriva_Gun";
            }
            else if (activeWeapon is Batarang)
            {
                locationstring += "Batarang";
            }
            else if (activeWeapon is BulletBill)
            {
                locationstring += "Bullet_Bill";
            }
            else if (activeWeapon is DessertBeagle)
            {
                locationstring += "Dessert_Beagle";
            }
            else if (activeWeapon is FlameThrower)
            {
                locationstring += "Flamethrower";
            }
            else if (activeWeapon is HomersBullets)
            {
                locationstring += "Homers_Bullets";
            }
            else if (activeWeapon is KA74)
            {
                locationstring += "KA74";
            }
            else if (activeWeapon is Knettergun)
            {
                locationstring += "Knetter_Gun";
            }
            else if (activeWeapon is UWP)
            {
                locationstring += "UWP";
            }
            else if (activeWeapon is VLEKKannon)
            {
                locationstring += "VLEKKannon";
            }

            locationstring += "_" + Direction + ".png";

            if (locationstring != Location)
            {
                Location = locationstring;
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

        public override bool CollisionEffect(GameObject gameObject)
        {
            if (gameObject.HasTag("solid"))
            {
                //Check collision from the left or right.
                if ((gameObject.FromLeft + gameObject.Width) > (FromLeft + Width))
                {
                    AddFromLeft(-1);
                }
                else if ((gameObject.FromLeft + gameObject.Width) < (FromLeft + Width))
                {
                    AddFromLeft(1);
                }

                //Check collision from top or bottom.
                if ((gameObject.FromTop + gameObject.Height) > (FromTop + Height))
                {
                    AddFromTop(-1);
                }
                else if ((gameObject.FromTop + gameObject.Height) < (FromTop + Height))
                {
                    AddFromTop(1);
                }
            }

            //If a player is coliding with an object their CollisionEffect is triggered instantly and not after this resolves.
            //This is so the collision of the enemy still goes even thought they are not colliding anymore.
            gameObject.CollisionEffect(this);
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
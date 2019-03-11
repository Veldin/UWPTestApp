using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes
{
    class Pickup : GameObject
    {
        // Constants for ammunition for guns
        private const string AmmunitionArrivaGun = "AmmunitionArriva Gun";
        private const string AmmunitionBatarang = "AmmunitionBatarang";
        private const string AmmunitionBulletBill = "AmmunitionBullet Bill";
        private const string AmmunitionDessertBeagle = "AmmunitionDessert Beagle";
        private const string AmmunitionFlameThrower = "AmmunitionFlame Thrower";
        private const string AmmunitionHomersBullets = "AmmunitionHomers Bullets";
        private const string AmmunitionKa74 = "AmmunitionKA74";
        private const string AmmunitionKnettergun = "AmmunitionKnettergun";
        private const string AmmunitionUwp = "AmmunitionUWP";
        private const string AmmunitionVlekKannon = "AmmunitionVLEKKannon";

        // Constants for upgrading guns
        private const string UpgradeArrivaGun = "UpgradeArriva Gun";
        private const string UpgradeBatarang = "UpgradeBatarang";
        private const string UpgradeBulletBill = "UpgradeBullet Bill";
        private const string UpgradeDessertBeagle = "UpgradeDessert Beagle";
        private const string UpgradeFlameThrower = "UpgradeFlame Thrower";
        private const string UpgradeHomersBullets = "UpgradeHomers Bullets";
        private const string UpgradeKa74 = "UpgradeKA74";
        private const string UpgradeKnettergun = "UpgradeKnettergun";
        private const string UpgradeUwp = "UpgradeUWP";
        private const string UpgradeVlekKannon = "UpgradeVLEKKannon";

        // Constraints for armour up and health up.
        private const string ArmourUp = "ArmourUp";
        private const string HealthUp = "HealthUp";

        //Dictionary containing the sprites.
        private Dictionary<string, string[]> sprites = new Dictionary<string, string[]>()
        {
            { AmmunitionArrivaGun, new string[]
            {
                "Ammo_Pickups\\Arriva_Gun_Ammo_Right.png",
                "Ammo_Pickups\\Arriva_Gun_Ammo_Left.png"
            }},
            { AmmunitionBatarang, new string[]
            {
                "Ammo_Pickups\\Batarang_Ammo_Right.png",
                "Ammo_Pickups\\Batarang_Ammo_Left.png"
            }},
            { AmmunitionBulletBill, new string[]
            {
                "Ammo_Pickups\\Bullet_Bill_Ammo_Right.png",
                "Ammo_Pickups\\Bullet_Bill_Ammo_Left.png"
            }},
            { AmmunitionDessertBeagle, new string[]
            {
                "Ammo_Pickups\\Dessert_Beagle_Ammo_Right.png",
                "Ammo_Pickups\\Dessert_Beagle_Ammo_Left.png"
            }},
            { AmmunitionFlameThrower, new string[]
            {
                "Ammo_Pickups\\Flame_Thrower_Ammo_Right.png",
                "Ammo_Pickups\\Flame_Thrower_Ammo_Left.png"
            }},
            { AmmunitionHomersBullets, new string[]
            {
                "Ammo_Pickups\\Homers_Bullets_Ammo_Left.png",
                "Ammo_Pickups\\Homers_Bullets_Ammo_Right.png"
            }},
            { AmmunitionKa74, new string[]
            {
                "Ammo_Pickups\\KA74_Ammo_Right.png",
                "Ammo_Pickups\\KA74_Ammo_Left.png"
            }},
            { AmmunitionKnettergun, new string[]
            {
                "Ammo_Pickups\\Knetter_Gun_Ammo_Left.png",
                "Ammo_Pickups\\Knetter_Gun_Ammo_Right.png"
            }},
            { AmmunitionUwp, new string[]
            {
                "Ammo_Pickups\\UWP_Ammo_Right.png",
                "Ammo_Pickups\\UWP_Ammo_Left.png"
            }},
            { AmmunitionVlekKannon, new string[]
            {
                "Ammo_Pickups\\VLEKKannon_Ammo_Right.png",
                "Ammo_Pickups\\VLEKKannon_Ammo_Left.png"
            }},
            { UpgradeArrivaGun, new string[]
            {
                "Weapon_Upgrades\\Arriva_Gun_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\Arriva_Gun_Upgrade_Pickup_Left.png"
            }},
            { UpgradeBatarang, new string[]
            {
                "Weapon_Upgrades\\Batarang_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\Batarang_Upgrade_Pickup_Left.png"
            }},
            { UpgradeBulletBill, new string[]
            {
                "Weapon_Upgrades\\Bullet_Bill_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\Bullet_Bill_Upgrade_Pickup_Left.png"
            }},
            { UpgradeDessertBeagle, new string[]
            {
                "Weapon_Upgrades\\Dessert_Beagle_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\Dessert_Beagle_Upgrade_Pickup_Left.png"
            }},
            { UpgradeFlameThrower, new string[]
            {
                "Weapon_Upgrades\\Flame_Thrower_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\Flame_Thrower_Upgrade_Pickup_Left.png"
            }},
            { UpgradeHomersBullets, new string[]
            {
                "Weapon_Upgrades\\Homers_Bullets_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\Homers_Bullets_Upgrade_Pickup_Left.png"
            }},
            { UpgradeKa74, new string[]
            {
                "Weapon_Upgrades\\KA74_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\KA74_Upgrade_Pickup_Left.png"
            }},
            { UpgradeKnettergun, new string[]
            {
                "Weapon_Upgrades\\Knetter_Gun_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\Knetter_Gun_Upgrade_Pickup_Left.png"
            }},
            { UpgradeUwp, new string[]
            {
                "Weapon_Upgrades\\UWP_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\UWP_Upgrade_Pickup_Left.png"
            }},
            { UpgradeVlekKannon, new string[]
            {
                "Weapon_Upgrades\\VLEKKannon_Upgrade_Pickup_Right.png",
                "Weapon_Upgrades\\VLEKKannon_Upgrade_Pickup_Left.png"
            }},
            { ArmourUp, new string[]
            {
                "Armour_Pickup_Right.png",
                "Armour_Pickup_Left.png"
            }},
            { HealthUp, new string[]
            {
                "Health_Pickup_Right.png",
                "Health_Pickup_Left.png"
            }}
        };

        //The sprite can changes from orientation.
        private bool leftSpriteSelected = false;

        //Basic information bout the pickup.
        private int amount;
        private string type;
        private string pickUpSound;
        private string popupDisplayText;

        //The different sounds that this pickup can make.
        private const string pickUpAmmoSound = "Generic_Sounds\\Ammo_Pickup_Sound.wav";
        private const string pickUpUpgradeSound = "Generic_Sounds\\Weapon_Upgrade_Sound.wav";
        private const string pickUpArmourSound = "Generic_Sounds\\Armour_Pickup_Sound.wav";
        private const string pickUpHealthSound = "Generic_Sounds\\Health_Pickup_Sound.wav";
        
        //The delta this pickup exists.
        private float totalDelta = 0;

        //Both bitmaps are saved so switching between doesnt cause the other to unload.
        private CanvasBitmap rightSprite;
        private CanvasBitmap leftSprite;

        public Pickup(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0) : 
            base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {

            Random rand = new Random();
            int r = rand.Next(21); //Get a number from 0 to 21

            //THe random number dictates the type
            SetType(r);

            //Set the pickup sound
            SetSound();

            //Load the default sprite
            this.Location = "Assets\\Sprites\\Pickup_Sprites\\" + sprites[type][0];
        }

        /* SetSound */
        /*
         * Sets the sound of pickup. The type is used to deside what sound to play.
        */
        private void SetSound()
        {
            if (type.Contains("Ammunition"))
            {
                // ammunition pick up sound.
                pickUpSound = pickUpAmmoSound;
            }
            else if (type.Contains("Upgrade"))
            {
                // Upgrade pick up sound.
                pickUpSound = pickUpUpgradeSound;
            }
            else if (type.Equals(ArmourUp))
            {
                // Armour pick up sound.
                pickUpSound = pickUpArmourSound;
            }
            else //If no other sound is found the healthpickup sound it used.
            {
                // Health pick up sound
                pickUpSound = pickUpHealthSound;
            }
        }

        /* SetType */
        /*
         * Sets the type of pickup. (What it's supposed to do on pickup)
         * A number between 0 and 21 is expected. Any other number will be treated the same as 21
        */
        private int SetType(int r)
        {

            switch (r)
            {
                case 0:
                    type = AmmunitionArrivaGun;
                    popupDisplayText = "ArrivaGun Rails";
                    break;
                case 1:
                    type = AmmunitionBatarang;
                    popupDisplayText = "Batarang Rangs";
                    break;
                case 2:
                    type = AmmunitionBulletBill;
                    popupDisplayText = "BulletBill Bullet Bills";
                    break;
                // DessertBeagle has infinite bullets, this goes to default (health pickup)
                case 4:
                    type = AmmunitionFlameThrower;
                    popupDisplayText = "FlameThrower Canister";
                    break;
                case 5:
                    type = AmmunitionHomersBullets;
                    popupDisplayText = "HomersBullets Bullets";
                    break;
                case 6:
                    type = AmmunitionKa74;
                    popupDisplayText = "Ka74 Mags";
                    break;
                case 7:
                    type = AmmunitionKnettergun;
                    popupDisplayText = "Knettergun Slugs";
                    break;
                case 8:
                    type = AmmunitionUwp;
                    popupDisplayText = "Uwp Mags";
                    break;
                case 9:
                    type = AmmunitionVlekKannon;
                    popupDisplayText = "VlekKannon Chains";
                    break;
                case 10:
                    type = UpgradeArrivaGun;
                    popupDisplayText = "ArrivaGun Upgrade";
                    break;
                case 11:
                    type = UpgradeBatarang;
                    popupDisplayText = "Batarang Upgrade";
                    break;
                case 12:
                    type = UpgradeBulletBill;
                    popupDisplayText = "BulletBill Upgrade";
                    break;
                case 13:
                    type = UpgradeDessertBeagle;
                    popupDisplayText = "DessertBeagle Upgrade";
                    break;
                case 14:
                    type = UpgradeFlameThrower;
                    popupDisplayText = "FlameThrower Upgrade";
                    break;
                case 15:
                    type = UpgradeHomersBullets;
                    popupDisplayText = "HomersBullets Upgrade";
                    break;
                case 16:
                    type = UpgradeKa74;
                    popupDisplayText = "Ka74 Upgrade";
                    break;
                case 17:
                    type = UpgradeKnettergun;
                    popupDisplayText = "Knettergun Upgrade";
                    break;
                case 18:
                    type = UpgradeUwp;
                    popupDisplayText = "Uwp Upgrade";
                    break;
                case 19:
                    type = UpgradeVlekKannon;
                    popupDisplayText = "VlekKannon Upgrade";
                    break;
                case 20:
                    type = ArmourUp;
                    popupDisplayText = "ArmourUp";
                    amount = 100;
                    break;
                default:
                    type = HealthUp;
                    popupDisplayText = "HealthUp";
                    amount = 100;
                    break;
            }

            return r;
        }

        public override bool IsActive(GameObject gameObject)
        {
            return true;
        }

        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {
            if (HasTag("popup"))
            {
                gameObjects.Add(new TextBox(75, 50, fromLeft, fromTop - 20, 0, 0, 0, 0, popupDisplayText, 1000));
                RemoveTag("popup");
            }

            //Count the total delta up to know how long this item existed
            totalDelta += delta;

            //If its older then 1800 units
            if (totalDelta > 1800)
            {
                if (rightSprite == null && sprite != null)
                {
                    //CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Sprites/Player_Sprites/Arriva_Gun_Bottom.png"))
                    rightSprite = sprite;
                    sprite = null;
                    Location = "Assets\\Sprites\\Pickup_Sprites\\" + sprites[type][1];
                }
                else if (leftSprite == null && sprite != null)
                {
                    //CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Sprites/Player_Sprites/Arriva_Gun_Bottom.png"))
                    leftSprite = sprite;
                    Location = "Assets\\Sprites\\Pickup_Sprites\\" + sprites[type][1];
                }
                else
                {
                    if (leftSpriteSelected)
                    {
                        sprite = leftSprite;
                    }
                    else
                    {
                        sprite = rightSprite;
                    }
                    leftSpriteSelected = !leftSpriteSelected;
                }
                totalDelta = 0;
            }
            return true;
        }

        /* CollisionEffect */
        /*
         * If a player colides with an pickup the pickup should be picked up
        */
        public override bool CollisionEffect(GameObject gameObject)
        {
            //If the pickup is taged to destroy do nothing
            if (HasTag("destroyed")) return true;

            //If the coliding target is a player it should be pickd up
            if (gameObject is Player player)
            {
                //If this pickup contains Ammunition
                if (type.Contains("Ammunition"))
                {
                    string weaponName = type.Remove(0, 10);

                    foreach (var w in player.GetWeapons())
                    {
                        if (w.name.Equals(weaponName))
                        {
                            if (w.clipAmount >= 5)
                            {
                                w.currentClip = w.clipMax;
                            }
                            else
                            {
                                w.clipAmount+= 20;
                                MainPage.Current.getWeaponStats();
                                MainPage.Current.UpdateCurrentClip();
                            }
                            MainPage.Current.weaponAmmo(weaponName);
                            break;
                        }
                    }
                }
                else if (type.Contains("Upgrade")) //If this pickup contains an Upgrade
                {
                    string weaponName = type.Remove(0, 7);

                    foreach (var w in player.GetWeapons())
                    {
                        if (w.name.Equals(weaponName))
                        {
                            w.Upgrade();
                            MainPage.Current.getWeaponStats();
                            MainPage.Current.UpdateCurrentClip();
                            break;
                        }
                    }
                }
                else if (type.Equals(ArmourUp)) //If this pickup contains Armour
                {
                    player.IncreaseArmour(amount);
                    MainPage.Current.updateArmour();
                }
                else if (type.Equals(HealthUp)) //If this pickup contains Health
                {
                    player.IncreaseHealth(amount);
                    MainPage.Current.updateHealth();
                }
                AddTag("destroyed");
                AddTag("popup");

            }
            return true;
        }

        public string getPickUpSound()
        {
            return pickUpSound;
        }

        public static string[] getSounds()
        {
            return new string[]
            {
                pickUpAmmoSound,
                pickUpArmourSound,
                pickUpUpgradeSound,
                pickUpHealthSound
            };
        }
    }
}

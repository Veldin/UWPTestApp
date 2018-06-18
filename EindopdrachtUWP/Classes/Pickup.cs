using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using UWPTestApp;

namespace EindopdrachtUWP.Classes
{
    class Pickup : GameObject
    {

        // Constraints for ammunition for guns
        public const string AmmunitionArrivaGun = "AmmunitionArriva Gun";
        public const string AmmunitionBatarang = "AmmunitionBatarang";
        public const string AmmunitionBulletBill = "AmmunitionBullet Bill";
        public const string AmmunitionDessertBeagle = "AmmunitionDessert Beagle";
        public const string AmmunitionFlameThrower = "AmmunitionFlame Thrower";
        public const string AmmunitionHomersBullets = "AmmunitionHomers Bullets";
        public const string AmmunitionKa74 = "AmmunitionKA74";
        public const string AmmunitionKnettergun = "AmmunitionKnettergun";
        public const string AmmunitionUwp = "AmmunitionUWP";
        public const string AmmunitionVlekKannon = "AmmunitionVLEKKannon";

        // Constraints for upgrading guns
        public const string UpgradeArrivaGun = "UpgradeArriva Gun";
        public const string UpgradeBatarang = "UpgradeBatarang";
        public const string UpgradeBulletBill = "UpgradeBullet Bill";
        public const string UpgradeDessertBeagle = "UpgradeDessert Beagle";
        public const string UpgradeFlameThrower = "UpgradeFlame Thrower";
        public const string UpgradeHomersBullets = "UpgradeHomers Bullets";
        public const string UpgradeKa74 = "UpgradeKA74";
        public const string UpgradeKnettergun = "UpgradeKnettergun";
        public const string UpgradeUwp = "UpgradeUWP";
        public const string UpgradeVlekKannon = "UpgradeVLEKKannon";

        // Constraints for armor up and health up.
        public const string ArmorUp = "ArmorUp";
        public const string HealthUp = "HealthUp";





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
            { ArmorUp, new string[]
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

        private bool leftSpriteSelected = false;
        
        

        private int amount;
        private string type;
        private string pickUpSound;

        private const string pickUpAmmoSound = "Generic_Sounds\\Ammo_Pickup_Sound.wav";
        private const string pickUpUpgradeSound = "Generic_Sounds\\Weapon_Upgrade_Sound.wav";
        private const string pickUpArmorSound = "Generic_Sounds\\Armour_Pickup_Sound.wav";
        private const string pickUpHealthSound = "Generic_Sounds\\Health_Pickup_Sound.wav";


        private float totalDelta = 0;

        private CanvasBitmap rightSprite;
        private CanvasBitmap leftSprite;

        public Pickup(float width, float height, float fromLeft, float fromTop, int amount = 0, string type = "rand", float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0) : 
            base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            if (type == "rand")
            {
                Random rand = new Random();
                int r = rand.Next(21);
                switch (r)
                {
                    case 0:
                        type = AmmunitionArrivaGun;
                        break;
                    case 1:
                        type = AmmunitionBatarang;
                        break;
                    case 2:
                        type = AmmunitionBulletBill;
                        break;
                    // DessertBeagle has infinite bullets, this goes to default (health pickup)
                    case 4:
                        type = AmmunitionFlameThrower;
                        break;
                    case 5:
                        type = AmmunitionHomersBullets;
                        break;
                    case 6:
                        type = AmmunitionKa74;
                        break;
                    case 7:
                        type = AmmunitionKnettergun;
                        break;
                    case 8:
                        type = AmmunitionUwp;
                        break;
                    case 9:
                        type = AmmunitionVlekKannon;
                        break;
                    case 10:
                        type = UpgradeArrivaGun;
                        break;
                    case 11:
                        type = UpgradeBatarang;
                        break;
                    case 12:
                        type = UpgradeBulletBill;
                        break;
                    case 13:
                        type = UpgradeDessertBeagle;
                        break;
                    case 14:
                        type = UpgradeFlameThrower;
                        break;
                    case 15:
                        type = UpgradeHomersBullets;
                        break;
                    case 16:
                        type = UpgradeKa74;
                        break;
                    case 17:
                        type = UpgradeKnettergun;
                        break;
                    case 18:
                        type = UpgradeUwp;
                        break;
                    case 19:
                        type = UpgradeVlekKannon;
                        break;
                    case 20:
                        type = ArmorUp;
                        amount = 100;
                        break;
                    default:
                        type = HealthUp;
                        amount = 100;
                        break;
                }
                
            }
            this.amount = amount;
            this.type = type;

            if (type.Contains("Ammunition"))
            {
                // ammunition pick up sound
                pickUpSound = pickUpAmmoSound;
            }
            else if (type.Contains("Upgrade"))
            {
                // Upgrade pick up sound
                pickUpSound = pickUpUpgradeSound;
            }
            else if (type.Equals(ArmorUp))
            {
                // Armor pick up sound
                pickUpSound = pickUpArmorSound;
            }
            else if (type.Equals(HealthUp))
            {
                // Health pick up sound
                pickUpSound = pickUpHealthSound;
            }

            this.Location = "Assets\\Sprites\\Pickup_Sprites\\" + sprites[type][0];

            


        }

        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {
            totalDelta += delta;
            if (totalDelta > 1800)
            {
                if (rightSprite == null && sprite != null)
                {
                    //CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Sprites/Player_Sprites/Arriva_Gun_Bottom.png"))
                    rightSprite = sprite;
                    sprite = null;
                    this.Location = "Assets\\Sprites\\Pickup_Sprites\\" + sprites[type][1];
                }else if (leftSprite == null && sprite != null)
                {
                    //CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Sprites/Player_Sprites/Arriva_Gun_Bottom.png"))
                    leftSprite = sprite;
                    this.Location = "Assets\\Sprites\\Pickup_Sprites\\" + sprites[type][1];
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

        public override bool CollitionEffect(GameObject gameObject)
        {
            if (gameObject is Player player)
            {
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
                                w.clipAmount++;
                            }
                            MainPage.Current.weaponAmmo(weaponName);
                            break;
                        }
                    }
                }else if (type.Contains("Upgrade"))
                {
                    string weaponName = type.Remove(0, 7);

                    foreach (var w in player.GetWeapons())
                    {
                        if (w.name.Equals(weaponName))
                        {
                            w.Upgrade();
                            break;
                        }
                    }
                }
                else if (type.Equals(ArmorUp))
                {
                    player.IncreaseArmor(amount);
                }
                else if (type.Equals(HealthUp))
                {
                    player.IncreaseHealth(amount);
                }
                AddTag("destroyed");
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
                pickUpArmorSound,
                pickUpUpgradeSound,
                pickUpHealthSound
            };
    }

    }
}

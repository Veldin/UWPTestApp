using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EindopdrachtUWP.Classes.Weapons;
using UWPTestApp;

namespace EindopdrachtUWP.Classes
{
    class Pickup : GameObject
    {

        // Constraints for ammunition for guns
        public const string AmmunitionArrivaGun = "AmmunitionArrivaGun";
        public const string AmmunitionBatarang = "AmmunitionBatarang";
        public const string AmmunitionBulletBill = "AmmunitionBulletBill";
        public const string AmmunitionDessertBeagle = "AmmunitionDessertBeagle";
        public const string AmmunitionFlameThrower = "AmmunitionFlameThrower";
        public const string AmmunitionHomersBullets = "AmmunitionHomersBullets";
        public const string AmmunitionKa74 = "AmmunitionKa74";
        public const string AmmunitionKnettergun = "AmmunitionKnettergun";
        public const string AmmunitionUwp = "AmmunitionUwp";
        public const string AmmunitionVlekKannon = "AmmunitionVlekKannon";

        // Constraints for upgrading guns
        public const string UpgradeArrivaGun = "UpgradeArrivaGun";
        public const string UpgradeBatarang = "UpgradeBatarang";
        public const string UpgradeBulletBill = "UpgradeBulletBill";
        public const string UpgradeDessertBeagle = "UpgradeDessertBeagle";
        public const string UpgradeFlameThrower = "UpgradeFlameThrower";
        public const string UpgradeHomersBullets = "UpgradeHomersBullets";
        public const string UpgradeKa74 = "UpgradeKa74";
        public const string UpgradeKnettergun = "UpgradeKnettergun";
        public const string UpgradeUwp = "UpgradeUwp";
        public const string UpgradeVlekKannon = "UpgradeVlekKannon";

        // Constraints for armor up and health up.
        public const string ArmorUp = "armorup";
        public const string HealthUp = "healthup";


        
        private int amount;
        private string type;
        private string pickUpSound;

        private bool deletable = false;
        

        public Pickup(float width, float height, float fromLeft, float fromTop, int amount, string type, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0) : 
            base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            this.amount = amount;
            this.type = type;

            if (type.Contains("Ammunition"))
            {
                // ammunition pick up sound
                pickUpSound = "";
                
            }
            else if (type.Contains("Upgrade"))
            {
                // Upgrade pick up sound
                pickUpSound = "";
            }
            else if (type.Equals(ArmorUp))
            {
                // Armor pick up sound
                pickUpSound = "";
            }
            else if (type.Equals(HealthUp))
            {
                // Health pick up sound
                pickUpSound = "";
            }
        }

        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {
            if (deletable)
            {
                gameObjects.Remove(this);
            }
            return true;
        }

        public override bool CollitionEffect(GameObject gameObject)
        {
            if (gameObject is Player player)
            {
                if (deletable) return true;
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
                deletable = true;
            }
            return true;
        }

        public string getPickUpSound()
        {
            return pickUpSound;
        }
        

    }
}

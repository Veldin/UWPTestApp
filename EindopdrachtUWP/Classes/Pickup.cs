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
        
        public const string ArmorUp = "armorup";
        public const string HealthUp = "healthup";

        private int amount;
        private string type;
        private string pickupSound;

        private bool deletable = false;
        

        public Pickup(float width, float height, float fromLeft, float fromTop, int amount, string type, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0) : 
            base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            this.amount = amount;
            this.type = type;
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
        

    }
}

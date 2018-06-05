using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    class Pickup : GameObject
    {
        public Pickup(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0, string pickupTpe = "Ammunition")
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {

        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public override Boolean OnTick(List<GameObject> gameObjects, float delta)
        {
            return true;
        }

        public override Boolean CollitionEffect(GameObject gameObject)
        {
            if(gameObject.HasTag("allowPickup")){
                PlayPickupSound();
            }

            return true;
        }

        public void PlayPickupSound()
        {


        }
    }
}

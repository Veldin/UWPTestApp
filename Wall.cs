using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    public class Wall : GameObject
    {
        public Wall(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            
        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public override Boolean OnTick(ArrayList gameObjects, float delta)
        {
            return true;
        }

        public override Boolean CollitionEffect(GameObject gameObject)
        {
            gameObject.AddFromTop(1);

            //Check if its still coliding and repeat the effect
            if (IsColliding(gameObject))
            {
                CollitionEffect(gameObject);
            }

            return true;
        }
    }
}

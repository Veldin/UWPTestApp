using System;
using System.Collections.Generic;

namespace UWPTestApp
{
    public class Wall : GameObject
    {
        public Wall(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            AddTag("solid"); //Walls are solid, all of the time! (But don't have to be if you remove the tag!)

            Location = "Assets/Sprites/Pixel.png";
        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public override Boolean OnTick(List<GameObject> gameObjects, float delta)
        {
            return true;
        }

        public override Boolean CollitionEffect(GameObject gameObject)
        {
            return true;
        }
    }
}

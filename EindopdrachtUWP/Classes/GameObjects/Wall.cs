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

        public override bool IsActive(GameObject gameObject)
        {
            //Only activate nearby walls need to have collition so are active
            if (Math.Abs(gameObject.FromLeft - this.FromLeft) < 600)
            {
                return true;
            }

            if (Math.Abs(gameObject.FromTop - this.FromTop) < 600)
            {
                return true;
            }
            return false;
        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {
            return true;
        }

        public override bool CollisionEffect(GameObject gameObject)
        {
            return true;
        }
    }
}

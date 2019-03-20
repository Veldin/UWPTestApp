using EindopdrachtUWP;
using EindopdrachtUWP.Classes;
using System;
using System.Collections.Generic;
using UWPTestApp;

namespace UWPTestApp
{
    public class Background : GameObject
    {
        public Background(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            
            if (width != 800 && height != 600)
            {
                Location = string.Format("Assets/Sprites/Maps/Sidewalks/{0}x{1}.gif", width / 32, height / 32);
                Location = string.Format("Assets/Sprites/pixel.png");

            }
            else
            {
                Location = "Assets/Sprites/Maps/BG.png";
            }
            
        }

        public override bool IsActive(GameObject gameObject)
        {
            //Background is never active
            return false;
        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public override bool OnTick(List<GameObject> gameObjects, float delta)
        {
            return false;
        }

        public override bool CollisionEffect(GameObject gameObject)
        {
            return false;
        }
    }
}
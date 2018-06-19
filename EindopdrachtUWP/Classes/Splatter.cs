using System;
using System.Collections.Generic;

namespace UWPTestApp
{
    public class Splatter : GameObject
    {
        private float duration;
        private float minDuration;
        private float maxDuration;

        private float startHeight;
        private float startWidth;

        public Splatter(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {
            maxDuration = 40000;
            minDuration = 1000;

            startHeight = height;
            startWidth = width;

            //Generate a new random with the hash of the GUID as seed. This way splaters made on the same frame aren't always the same.
            Random random = new Random(Guid.NewGuid().GetHashCode());
            duration = random.Next((int)minDuration, (int)maxDuration);

            int randomPositionOffset = random.Next(1,19);
            location = "Assets/Sprites/Enemy_Sprites/Bloodsplatter"+ randomPositionOffset + ".png";
        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public override Boolean OnTick(List<GameObject> gameObjects, float delta)
        {
            duration -= delta;
            if (duration < 0)
            {
                AddTag("destroyed");
            }
            
            //Animation for the despawning.
            AddHeight((height / duration) * -1);
            AddWidth((width / duration) * -1);

            //if the hight or with are negative put them back on 0
            if (Height < 0)
            {
                Height = 0;
            }
            if (Width < 0)
            {
                Width = 0;
            }
            return true;
        }

        public override Boolean CollitionEffect(GameObject gameObject)
        {
            //IF the blood splatter hits a wall reduce its size so it doesnt show on top of the wall.
            if(gameObject is Wall)
            {
                AddHeight(-1);

                if (Height < 2)
                {
                    //If the height is below 1, remove the blood splatter.
                    AddTag("destroyed");
                }
            }
            return true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    public class Splatter : GameObject
    {
        public Splatter(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        : base(width, height, fromLeft, fromTop, widthDrawOffset, heightDrawOffset, fromLeftDrawOffset, fromTopDrawOffset)
        {

            //Generate a new random with the hash of the GUID as seed. This way splaters made on the same frame aren't always the same.
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int randomPositionOffset = random.Next(1,19);

            location = "Assets/Sprites/Enemy_Sprites/Bloodsplatter"+ randomPositionOffset + ".png";

        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public override Boolean OnTick(List<GameObject> gameObjects, float delta)
        {
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

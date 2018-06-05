<<<<<<< HEAD
﻿using System;
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
        public override Boolean OnTick(List<GameObject> gameObjects, float delta)
        {
            return true;
        }

        public override Boolean CollitionEffect(GameObject gameObject)
        {
            //AddFromTop(1);

            //Check if its still coliding and repeat the effect
            if (IsColliding(gameObject))
            {
               // CollitionEffect(gameObject);
            }

            Debug.WriteLine("is colidgin");

            return true;
        }
    }
}
=======
﻿using System;
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
>>>>>>> 965930e6d68c689e3f4bef2cdb55f37ce6400f88

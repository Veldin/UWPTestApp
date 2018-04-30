using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    class GameObject
    {
        String tag;
        float width;
        float height;

        //Location of the object (where to draw it)
        float fromTop;
        float fromLeft;

        public GameObject(String tag, float width, float height, float fromLeft, float fromTop)
        {
            this.tag = tag;
            this.width = width;
            this.height = height;
            this.fromLeft = fromLeft;
            this.fromTop = fromTop;
        }
    }
}

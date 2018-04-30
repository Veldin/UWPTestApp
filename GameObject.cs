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

        //Getters for the different properties that have to do with positioning in the canvas.
        public float getWith(){return width;}
        public float getHeight() { return height;}
        public float getFromTop() { return fromTop;}
        public float getFromLeft() { return fromLeft;}

        //Setters for the different properties that have to do with positioning in the canvas.
        public void setWith(float width) { this.width = width; }
        public void setHeight(float height) { this.height = height; }
        public void setFromTop(float fromTop) { this.fromTop = fromTop; }
        public void setFromLeft(float fromLeft) { this.fromLeft = fromLeft; }

        //Methods to add ammounts to properties that have to do with positioning in the canvas.
        //They also return the new number so we can use them to calculate with instandly.
        public float addWith(float width) { this.width += width; return this.width; }
        public float addHeight(float height) { this.height += height; return this.height; }
        public float addFromTop(float fromTop) { this.fromTop += fromTop; return this.fromTop; }
        public float addFromLeft(float fromLeft) { this.fromLeft += fromLeft; return this.fromLeft; }
    }
}

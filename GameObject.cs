using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    public abstract class GameObject
    {
        //Tags given to this object by the engine to do sertain tasks.
        protected ArrayList tags;

        //Location where this gameObject is within the game.
        //This is also used for the hitbox 
        protected float width;
        protected float height;
        protected float fromLeft;
        protected float fromTop;

        //Objects can be facing left or right
        protected Boolean isFacingLeft;

        //Offset where to draw the gameObject in the game.
        //The Sprite can be bigger or smaller then the hitbox.
        //The sprite can be more to the left or right then the hitbox.

        protected float widthDrawOffset;
        protected float heightDrawOffset;
        protected float fromLeftDrawOffset;
        protected float fromTopDrawOffset;


        public GameObject(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        {

            tags = new ArrayList();

            this.width = width;
            this.height = height;
            this.fromLeft = fromLeft;
            this.fromTop = fromTop;

            this.isFacingLeft = true;

            this.widthDrawOffset = widthDrawOffset;
            this.heightDrawOffset = heightDrawOffset;
            this.fromLeftDrawOffset = fromLeftDrawOffset;
            this.fromTopDrawOffset = fromTopDrawOffset;
        }

        //Getters and setters for the tag
        public void AddTag(string tag)
        {
            tags.Add(tag);
        }


        public Boolean HasTag(string tag)
        {

            if (tags.IndexOf(tag) > -1)
                return true;
            return false;
        }

        public Boolean RemoveTag(string tag)
        {
            if (HasTag(tag))
            {
                tags.Remove(tag);
                return true;
            }
            return false;
        }

        //Getters and setters for the fields that have to do with positioning of the GameOgject.
        //public float Width { get; set; }
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float FromLeft
        {
            get { return fromLeft; }
            set { fromLeft = value; }
        }

        public float FromTop
        {
            get { return fromTop; }
            set { fromTop = value; }
        }

        //Methods to add ammounts to fields that have to do with positioning of the GameOgject.
        //They also return the new number so we can use them to calculate with instandly.
        public float AddWidth(float width) { this.width += width; return this.width; }
        public float AddHeight(float height) { this.height += height; return this.height; }
        public float AddFromTop(float fromTop) { this.fromTop += fromTop; return this.fromTop; }
        public float AddFromLeft(float fromLeft) { this.fromLeft += fromLeft; return this.fromLeft; }

        //Getters and setters for the fields that have to do with positioning in the canvas.
        public float WidthDrawOffset
        {
            get { return widthDrawOffset; }
            set { widthDrawOffset = value; }
        }

        public float HeightDrawOffset
        {
            get { return heightDrawOffset; }
            set { heightDrawOffset = value; }
        }

        public float FromTopDrawOffset
        {
            get { return fromTopDrawOffset; }
            set { fromTopDrawOffset = value; }
        }

        public float FromLeftDrawOffset
        {
            get { return fromLeftDrawOffset; }
            set { fromLeftDrawOffset = value; }
        }

        //functions to check if an object is facing left or right.
        public Boolean IsFacingLeft()
        {
            return isFacingLeft;
        }

        public void FaceLeft()
        {
            isFacingLeft = true;
        }

        public void FaceRight()
        {
            isFacingLeft = false;
        }

        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public abstract Boolean OnTick(List<GameObject> gameObjects, float delta);

        public Boolean IsColliding(GameObject gameObject)
        {
            //Check if you are comparing to youself.
            if (this == gameObject)
            {
                return false;
            }

            //if (
                //Left and Right
            //    FromLeft > gameObject.FromLeft || FromLeft + Width < gameObject.FromLeft

                
                //gameObject.fromLeft + gameObject.Width > fromLeft && gameObject.fromLeft + gameObject.width < fromLeft + width &&
                //gameObject.fromTop + gameObject.Height > fromTop && gameObject.fromTop + gameObject.Height < fromTop + width
            //){

            //    return true;
            //}

            Debug.WriteLine(FromLeft > gameObject.FromLeft);
            Debug.WriteLine(FromLeft < gameObject.FromLeft);
            Debug.WriteLine("__");

            //gameObjects.Add(new GameObject("CREATED GUY", 50, 50, 10, 10));
            return false;
        }

        public abstract Boolean CollitionEffect(GameObject gameObject);

    }
}

﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UWPTestApp
{
    public abstract class GameObject
    {
        //Tags given to this object by the engine to do sertain tasks.
        protected ArrayList tags;

        //Target!
        private Target target;

        //Location where this gameObject is within the game.
        //This is also used for the hitbox 
        protected float width;
        protected float height;
        protected float fromLeft;
        protected float fromTop;

        //Objects can be facing top, right, bottom and left
        public String direction { get; set; }

        //Offset where to draw the gameObject in the game.
        //The Sprite can be bigger or smaller then the hitbox.
        //The sprite can be more to the left or right then the hitbox.
        protected float widthDrawOffset;
        protected float heightDrawOffset;
        protected float fromLeftDrawOffset;
        protected float fromTopDrawOffset;

        //The sprite location and the CanvasBitmap are stored seperatly
        //This is so the location gets changed more times in a frame the canvasBitmap doesn't have to get loaded more then once a frame.
        protected CanvasBitmap sprite;
        protected String location;

        public GameObject(float width, float height, float fromLeft, float fromTop, float widthDrawOffset = 0, float heightDrawOffset = 0, float fromLeftDrawOffset = 0, float fromTopDrawOffset = 0)
        {
            tags = new ArrayList();

            this.width = width;
            this.height = height;
            this.fromLeft = fromLeft;
            this.fromTop = fromTop;

            this.widthDrawOffset = widthDrawOffset;
            this.heightDrawOffset = heightDrawOffset;
            this.fromLeftDrawOffset = fromLeftDrawOffset;
            this.fromTopDrawOffset = fromTopDrawOffset;

            //Default location of the sprite.
            location = "Assets/blank.gif";
        }

        /* CreateResourcesAsync */
        /*
         * To be able to use the sprites on a Canvas the sprites needs to be loaded as CanvasBitmaps.
         * The first argument is the CanvasControl.
        */
        public async Task CreateResourcesAsync(CanvasControl sender)
        {
            try
            {
                //Load the recource.
                sprite = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///" + location));
            }
            catch (Exception e)
            {
                Debug.WriteLine(location);
                Debug.WriteLine(e.StackTrace);
            }
        }

        public CanvasBitmap Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public String Location
        {
            get { return location; }
            set { location = value; }
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

        //Getters and setters for the fields that have to do with positioning of the GameObject.
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
        
        public Target Target
        {
            get { return target; }
            set { target = value; }
        }
        
        
        //Any object can edit the gameObjects of the game while the logic is running.
        //And Also get the delta for timed events.
        public abstract Boolean OnTick(List<GameObject> gameObjects, float delta);

        /* IsColliding */
        /*
         * Checks whether or not this gameobject is coliding with the given gameOjbect
         * The argument is the given gameObject
        */
        public Boolean IsColliding(GameObject gameObject)
        {
            //Check if you are comparing to youself.
            if (this == gameObject)
            {
                return false;
            }

            if (FromLeft < gameObject.FromLeft + gameObject.width  && FromLeft + Width > gameObject.FromLeft)
            {
                if (FromTop < gameObject.FromTop + gameObject.height && FromTop + Height > gameObject.FromTop)
                {
                    return true;
                }
            }
            return false;
        }


        /* CollitionEffect */
        /*
         * Effect that happens when this GameObject collides with the given object.
         * The argument is the given gameObject
        */
        public abstract Boolean CollitionEffect(GameObject gameObject);
    }
}
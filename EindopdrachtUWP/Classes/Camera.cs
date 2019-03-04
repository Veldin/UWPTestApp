using System;
using System.Diagnostics;

namespace UWPTestApp
{
    public class Camera
    {
        private float fromLeft;
        private float fromTop;
        private Target target;

        //For timekeeping (we need to know when the last frame happend when the next frame happens and the delta between)
        private long delta;     //The lenght in time the last frame lasted (so we can use it to calculate speeds of things without slowing down due to low fps)
        private long now;       //This is the time of the frame. (To calculate the delta)
        private long? then;      //This is the time of the previous draw frame. (To calculate the delta)

        public Camera(Target target)
        {
            this.target = target;
            this.fromLeft = target.FromLeft();
            this.fromTop = target.FromTop();
        }

        public bool OnTick()
        {
            now = Stopwatch.GetTimestamp();
            if (then == null)
            {
                then = Stopwatch.GetTimestamp();
            }
            delta = (now - (long)then) / 1000;

            float differenceLeft = (((FromLeft - target.FromLeft()) / 10) * delta) / 100000 / 25;
            float differenceTop = (((FromTop - target.FromTop()) / 10) * delta) / 100000 / 25;

            FromLeft -= differenceLeft;
            FromTop -= differenceTop;
            return true;
        }

        public float LeftOffset()
        {
            return 400 + FromLeft * -1;
        }

        public float TopOffset()
        {
            return 300 + FromTop * -1;
        }

        //Actual left location of the camera
        public float FromLeft
        {
            get { return fromLeft; }
            set { fromLeft = value; }
        }

        //Actual top location of the camera
        public float FromTop
        {
            get { return fromTop; }
            set { fromTop = value; }
        }

        //Target that the camera is moving towards
        public Target Target
        {
            get { return target; }
            set { target = value; }
        }
    }
}

using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using System.Threading; //Note that the Thread Class does not exist in UWP

namespace UWPTestApp
{

    class Engine
    {

        //Arraylist with all the gameObjects in the current game
        private ArrayList gameObjects;

        //Holder for the canvasControl
        private CanvasControl canvasControl;

        //For timekeeping (we need to know when the last frame happend when the next frame happens and the delta between)
        private long delta;     //The lenght in time the last frame lasted (so we can use it to calculate speeds of things without slowing down due to low fps)
        private long now;       //This is the time of the frame. (To calculate the delta)
        private long then;      //This is the time of the previous frame. (To calculate the delta)

        //The max fps we want to run at
        private float fps;  //The set FPS limit
        private float interfal; //Interfal that gets calculated based on the fps

        public Engine()
        {
            gameObjects = new ArrayList();


            gameObjects.Add(new GameObject("A guy", 50, 50, 10, 10));

            gameObjects.Add(new GameObject("A guy", 50, 100, 50, 100));

            gameObjects.Add(new GameObject("A guy", 100, 50, 100, 50));

            //Set the FPS and calculate the interfal!
            fps = 60;
            interfal = 1000 / fps; //1 second is 1000 ms.

            //Set then to the current time to know when we started
            then = Stopwatch.GetTimestamp();
        }

        //Runs the simulation of the gameEngine. 
        //Will calculate if its time to draw a new frame and then will.
        //Run() also always recurcifly schedules itself.
        public void Run(){
            now = Stopwatch.GetTimestamp();
            delta = now - then;

            if (delta > interfal)
            {
                then = now; //Remember when this frame was.

                Logic(); //Run the logic of the simulation.

                //Only draw the simulation if there is a known canvas.
                if (canvasControl != null)
                {
                    Draw();
                } 
            }

            Task.Yield();  //Force this task to complete asynchronously (This way the main thread is not blocked by this task calling itself.
            Task.Run(() => Run());  //Schedule new Run() task
        }

        private void Logic()
        {

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.addFromLeft( delta / 9000 );
                gameObject.addFromTop(delta / 9000);
            }
        }

        //Invilidate the drawing currently on the canvas. The canvas wil call an action to redraw itself.
        private void Draw()
        {
            canvasControl.Invalidate();
        }

        public void DrawEvent(CanvasControl sender, CanvasDrawEventArgs args)
        {

            //Set the canvasControl that called this method so we know what to Invalidate later.
            canvasControl = sender;

            //Draw the frame on this DrawingSession.
            args.DrawingSession.DrawEllipse(delta/10, delta / 10, 80, 30, Colors.Black, 3);

            foreach (GameObject gameObject in gameObjects)
            {

                //Rect Initializes a struct that has the specified from left, from top, width, and height.
                args.DrawingSession.DrawRectangle(
                    new Windows.Foundation.Rect(gameObject.getFromLeft(), gameObject.getFromTop(), gameObject.getWith(), gameObject.getHeight()), 
                    Colors.Red
                );

            }
        }

    }
}

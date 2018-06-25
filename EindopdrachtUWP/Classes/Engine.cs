using EindopdrachtUWP;
using EindopdrachtUWP.Classes;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;


namespace UWPTestApp
{

    class Engine
    {
        //Arraylist with all the gameObjects in the current game
        private List<GameObject> gameObjects;
        private HashSet<String> pressedKeys;

        //Holds the different scenes in the Engine! If a scene is loaded the objects in the scene are put in the gameObjects array!
        private List<Scene> scenes;
        private Scene scene;

        //Holder for the canvasControl
        private CanvasControl canvasControl;

        //For timekeeping (we need to know when the last frame happend when the next frame happens and the delta between)
        private long delta;     //The lenght in time the last frame lasted (so we can use it to calculate speeds of things without slowing down due to low fps)
        private long now;       //This is the time of the frame. (To calculate the delta)
        private long then;      //This is the time of the previous frame. (To calculate the delta)

        //The max fps we want to run at
        private float fps;  //The set FPS limit
        private float interfal; //Interfal that gets calculated based on the fps

        public Boolean music { get; set; }
        public Boolean effects { get; set; }

        private Boolean paused;

        private SoundController soundController;

        private Player player;

        public Engine()
        {
            gameObjects = new List<GameObject>();

            soundController = new SoundController();

            player = new Player(15, 15, 656, 312, 0, 0, 0, 0);

            soundController.AddSound(player.DeathSound);
            soundController.AddSound(player.HitSound);
            soundController.AddSound(player.MoveSound, 0.4);
            soundController.AddSound(player.HealthLowSound, 0.2);

            soundController.AddSound("Generic_Sounds\\levelup.wav", 1);

            foreach (Weapon weapon in player.GetWeapons())
            {
                soundController.AddSound(weapon.shotSound);
            }

            foreach (string pickupSound in Pickup.getSounds())
            {
                soundController.AddSound(pickupSound);
            }

            foreach (string deathSound in Enemy.DeathSounds)
            {
                soundController.AddSound(deathSound, 1);
            }

            scenes = new List<Scene>();

            music = true;
            effects = true;
            paused = true;

            //Add the first scene
            scenes.Add(
                new Scene(new List<GameObject>
                {
                    //Wall takes: width, height, fromLeft, fromTop, widthDrawOffset = 0, heightDrawOffset = 0, fromLeftDrawOffset = 0, fromTopDrawOffset = 0
                    // outer walls
                    new Wall(900, 100, -49, -99, 0, 0, 0, 0),
                    new Wall(900, 100, -49, 599, 0, 0, 0, 0),
                    new Wall(100, 700, -99, -49, 0, 0, 0, 0),
                    new Wall(100, 700, 799, -49, 0, 0, 0, 0),

                    // garden top left
                    new Wall(22, 183, 23, 23, 0, 0, 0, 0),
                    new Wall(183, 22, 23, 23, 0, 0, 0, 0),
                    new Wall(68, 22, 23, 184, 0, 0, 0, 0),
                    new Wall(22, 183, 184, 23, 0, 0, 0, 0),
                    new Wall(45, 68, 115, 23, 0, 0, 0, 0),
                    new Wall(68, 22, 138, 184, 0, 0, 0, 0),
                    new Wall(22, 45, 69, 115, 0, 0, 0, 0),
                    new Wall(68, 22, 138, 138, 0, 0, 0, 0),

                    // bottom rooms
                    new Wall(22, 68, 23, 414, 0, 0, 0, 0),
                    new Wall(91, 22, 23, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 92, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 138, 414, 0, 0, 0, 0),
                    new Wall(91, 22, 138, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 207, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 253, 414, 0, 0, 0, 0),
                    new Wall(91, 22, 253, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 322, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 368, 414, 0, 0, 0, 0),
                    new Wall(91, 22, 368, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 437, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 483, 414, 0, 0, 0, 0),
                    new Wall(91, 22, 483, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 552, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 23, 506, 0, 0, 0, 0),
                    new Wall(91, 22, 23, 552, 0, 0, 0, 0),
                    new Wall(22, 68, 92, 506, 0, 0, 0, 0),
                    new Wall(22, 68, 138, 506, 0, 0, 0, 0),
                    new Wall(91, 22, 138, 552, 0, 0, 0, 0),
                    new Wall(22, 68, 207, 506, 0, 0, 0, 0),
                    new Wall(22, 68, 253, 506, 0, 0, 0, 0),
                    new Wall(91, 22, 253, 552, 0, 0, 0, 0),
                    new Wall(22, 68, 322, 506, 0, 0, 0, 0),
                    new Wall(22, 68, 368, 506, 0, 0, 0, 0),
                    new Wall(91, 22, 368, 552, 0, 0, 0, 0),
                    new Wall(22, 68, 437, 506, 0, 0, 0, 0),
                    new Wall(22, 68, 483, 506, 0, 0, 0, 0),
                    new Wall(91, 22, 483, 552, 0, 0, 0, 0),
                    new Wall(22, 68, 552, 506, 0, 0, 0, 0),

                    // room bottom right
                    new Wall(22, 68, 598, 414, 0, 0, 0, 0),
                    new Wall(160, 22, 598, 414, 0, 0, 0, 0),
                    new Wall(22, 160, 736, 414, 0, 0, 0, 0),
                    new Wall(22, 68, 598, 506, 0, 0, 0, 0),
                    new Wall(160, 22, 598, 552, 0, 0, 0, 0),

                    //cars
                    new Wall(72, 30, 628, 280, 0, 0, 0, 0),

                    // houses
                    new Wall(160, 91, 207, 0, 0, 0, 0, 0),
                    new Wall(160, 91, 414, 0, 0, 0, 0, 0),
                    new Wall(160, 91, 621, 0, 0, 0, 0, 0),
                    new Wall(160, 91, 207, 116, 0, 0, 0, 0),
                    new Wall(160, 91, 414, 116, 0, 0, 0, 0),
                    new Wall(160, 91, 621, 116, 0, 0, 0, 0),

                    // lampposts
                    new Wall(22, 22, 46, 230, 0, 0, 0, 0),
                    new Wall(22, 22, 161, 230, 0, 0, 0, 0),
                    new Wall(22, 22, 276, 230, 0, 0, 0, 0),
                    new Wall(22, 22, 391, 230, 0, 0, 0, 0),
                    new Wall(22, 22, 506, 230, 0, 0, 0, 0),
                    new Wall(22, 22, 621, 230, 0, 0, 0, 0),
                    new Wall(22, 22, 736, 230, 0, 0, 0, 0),
                    new Wall(22, 22, 46, 368, 0, 0, 0, 0),
                    new Wall(22, 22, 161, 368, 0, 0, 0, 0),
                    new Wall(22, 22, 276, 368, 0, 0, 0, 0),
                    new Wall(22, 22, 391, 368, 0, 0, 0, 0),
                    new Wall(22, 22, 506, 368, 0, 0, 0, 0),
                    new Wall(22, 22, 621, 368, 0, 0, 0, 0),
                    new Wall(22, 22, 736, 368, 0, 0, 0, 0),

                    // extra walls that prevent clipping
                    new Wall(26, 35, 33, 33, 0, 0, 0, 0),
                    new Wall(35, 26, 33, 33, 0, 0, 0, 0),
                    new Wall(2, 2, 160, 45, 0, 0, 0, 0),
                    new Wall(2, 2, 206, 90, 0, 0, 0, 0),
                    new Wall(6, 6, -3, -3, 0, 0, 0, 0),
                    new Wall(2, 2, 366, -1, 0, 0, 0, 0),
                    new Wall(2, 2, 573, -1, 0, 0, 0, 0),
                    new Wall(2, 2, 780, -1, 0, 0, 0, 0),
                    new Wall(2, 2, 801, -1, 0, 0, 0, 0),
                    new Wall(2, 2, 44, 435, 0, 0, 0, 0),
                    new Wall(2, 2, 159, 435, 0, 0, 0, 0),
                    new Wall(2, 2, 274, 435, 0, 0, 0, 0),
                    new Wall(2, 2, 389, 435, 0, 0, 0, 0),
                    new Wall(2, 2, 504, 435, 0, 0, 0, 0),
                    new Wall(2, 2, 619, 435, 0, 0, 0, 0),
					
                    new Spawner(10, 10, 400, 300, 0, 0, 0, 0, 5000, 10000)
                })
            );

            pressedKeys = new HashSet<String>();
            
            gameObjects.Add(player);
            gameObjects[0].AddTag("controllable");  //Make the wall controllable

            LoadScene(0);

            //Set the FPS and calculate the interfal!
            fps = 60;
            interfal = 1000 / fps; //1 second is 1000 ms.

            //Set then to the current time to know when we started
            then = Stopwatch.GetTimestamp();

            paused = true;
        }

        public Player getPlayer() => player;
        public SoundController GetSoundController => soundController;

        //Gets the objects of a scene from the scene list on given index.
        public bool LoadScene(int index)
        {
            if (index > -1 && index < scenes.Count)
            {
                scene = scenes[index];                      //Set the scene on acive!
                gameObjects.AddRange(scene.GetScene());     //Add the gameobjects from this scene in the game!
                return true;
            }
            return false;
        }

        //Runs the simulation of the gameEngine. 
        //Will calculate if its time to draw a new frame and then will.
        //Run() also always recurcifly schedules itself.
        public void Run()
        {
            now = Stopwatch.GetTimestamp();
            delta = (now - then) / 1000; //Defide by 1000 to get the delta in MS

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
            paused = MainPage.Current.paused;

            if (!paused && !MainPage.Current.game_over)
            {
                //Check if there are objects in the List to apply logic on
                //Apply the logic to all the bameObjects CURRENTLY in the List.
                //The new List makes a copy so the original arraylist can be modivied in this loop
                foreach (GameObject gameObject in new List<GameObject>(gameObjects))
                {
                    //Handle player input
                    Player player = gameObject as Player;
                    if (player is Player)
                    {
                        if (gameObject.HasTag("controllable") && (IsKeyPressed("E") || IsKeyPressed("GamepadRightShoulder")))
                        {
                            NextWeapon();
                        }

                        if (gameObject.HasTag("controllable") && (IsKeyPressed("Q") || IsKeyPressed("GamepadLeftShoulder")))
                        {
                            PreviousWeapon();
                        }

                        if (gameObject.HasTag("controllable") && (IsKeyPressed("Right") || IsKeyPressed("GamepadRightThumbstickRight")))
                        {
                            if (player.Fire("Right", gameObjects))
                            {
                                soundController.PlaySound(player.GetActiveWeapon().shotSound);
                            }
                        }
                        else if (gameObject.HasTag("controllable") && (IsKeyPressed("Up") || IsKeyPressed("GamepadRightThumbstickUp")))
                        {
                            if (player.Fire("Top", gameObjects))
                            {
                                soundController.PlaySound(player.GetActiveWeapon().shotSound);
                            }
                        }
                        else if (gameObject.HasTag("controllable") && (IsKeyPressed("Down") || IsKeyPressed("GamepadRightThumbstickDown")))
                        {
                            if (player.Fire("Bottom", gameObjects))
                            {
                                soundController.PlaySound(player.GetActiveWeapon().shotSound);
                            }
                        }
                        else if (gameObject.HasTag("controllable") && (IsKeyPressed("Left") || IsKeyPressed("GamepadRightThumbstickLeft")))
                        {
                            if (player.Fire("Left", gameObjects))
                            {
                                soundController.PlaySound(player.GetActiveWeapon().shotSound);
                            }
                        }

                        player.IsWalking = false;
                        //Handle Input (Not only the player might be controlable)
                        if (gameObject.HasTag("controllable") && (IsKeyPressed("S") || IsKeyPressed("GamepadLeftThumbstickDown")))
                        {
                            player.Target.AddFromTop(1000);
                            player.IsWalking = true;
                        }

                        if (gameObject.HasTag("controllable") && (IsKeyPressed("W") || IsKeyPressed("GamepadLeftThumbstickUp")))
                        {
                            player.Target.AddFromTop(-1000);
                            player.IsWalking = true;
                        }

                        if (gameObject.HasTag("controllable") && (IsKeyPressed("D") || IsKeyPressed("GamepadLeftThumbstickRight")))
                        {
                            player.Target.AddFromLeft(1000);
                            player.IsWalking = true;
                        }

                        if (gameObject.HasTag("controllable") && (IsKeyPressed("A") || IsKeyPressed("GamepadLeftThumbstickLeft")))
                        {
                            player.Target.AddFromLeft(-1000);
                            player.IsWalking = true;
                        }

                        if (!paused && (IsKeyPressed("Space") || IsKeyPressed("GamepadView")))
                        {
                            MainPage.Current.getControls();
                        }
                        else if (!paused && (!IsKeyPressed("Space") || !IsKeyPressed("GamepadView")))
                        {
                            MainPage.Current.removeControls();
                        }

                        if (player.IsWalking)
                        {
                            if (player.deltaForWalkingSound > 1300)
                            {
                                soundController.PlaySound(player.MoveSound);
                                player.deltaForWalkingSound = 0;
                            }

                            player.deltaForWalkingSound += delta;
                        }

                    }

                    //On tick
                    gameObject.OnTick(gameObjects, delta);

                    //For every object in this loop, loop trough all objects to check if they are coliding
                    foreach (GameObject gameObjectCheck in new ArrayList(gameObjects))
                    {
                        if (gameObject.IsColliding(gameObjectCheck))
                        {
                            gameObject.CollitionEffect(gameObjectCheck);
                        }
                    }


                    //Key to pauze the screen
                    if (IsKeyPressed("Escape") || IsKeyPressed("GamepadMenu"))
                    {
                        MainPage.Current.getMenu();
                        paused = true;

                        //Empty all keys that were pressed to cause the game to not register keys that were pressed bevore the pause
                        pressedKeys = new HashSet<String>();

                        Task.Delay(300).Wait();

                    }
                }


                //Check if gameobjects want to be destoryed
                foreach (GameObject gameObjectCheck in new ArrayList(gameObjects))
                {

                    if (gameObjectCheck.HasTag("hit") && gameObjectCheck is Player p1)
                    {
                        soundController.PlaySound(p1.HitSound);
                        p1.RemoveTag("hit");
                    }

                    if (gameObjectCheck.HasTag("health_low") && gameObjectCheck is Player p2)
                    {
                        if (p2.deltaForHealthLowSound > 4000)
                        {
                            soundController.PlaySound(p2.HealthLowSound);
                            p2.deltaForHealthLowSound = 0;
                        }

                        p2.deltaForHealthLowSound += delta;
                    }

                    if (gameObjectCheck.HasTag("destroyed"))
                    {
                        if (gameObjectCheck is Pickup pickup)
                        {
                            soundController.PlaySound(pickup.getPickUpSound());
                        }
                        else if (gameObjectCheck is Player p3)
                        {
                            soundController.PlaySound(p3.DeathSound);
                            MainPage.Current.gameover();
                        }
                        else if (gameObjectCheck is MovableObject mo)
                        {
                            if (mo is Enemy enemy)
                            {
                                foreach (var getPlayer in new ArrayList(gameObjects))
                                {
                                    if (getPlayer is Player p4)
                                    {
                                        p4.Kills++;
                                        MainPage.Current.updateHighscore();
                                        MainPage.Current.killstreak++;
                                        MainPage.Current.updateKillstreak();
                                        if (p4.Kills > 5 * (p4.GetLevel() * p4.GetLevel()))
                                        {
                                            p4.IncreaseLevel();
                                            soundController.PlaySound("Generic_Sounds\\levelup.wav");
                                            MainPage.Current.UpdateLevel();
                                            if(p4.GetLevel() == 5)
                                            {
                                                gameObjects.Add(new Spawner(10, 10, 110, 213, 0, 0, 0, 0, 5000, 25000));
                                                MainPage.Current.enableSecondSpawner();
                                            }
                                            if (p4.GetLevel() == 10)
                                            {
                                                gameObjects.Add(new Spawner(10, 10, 770, 400, 0, 0, 0, 0, 5000, 25000));
                                                MainPage.Current.enableThirdSpawner();
                                            }
                                            break;
                                        }       
                                        break;
                                    }
                                }
                                soundController.PlaySound(enemy.DeathSound);
                            }
                        }
                        gameObjects.Remove(gameObjectCheck);
                    }
                }
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("A") || IsKeyPressed("GamepadA")))
            {
                MainPage.Current.removeMenu();
                paused = false;
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("B") || IsKeyPressed("GamepadB")))
            {
                if (music)
                {
                    MainPage.Current.muteMusic();
                    music = false;
                }
                else
                {
                    MainPage.Current.unmuteMusic();
                    music = true;
                }
                Task.Delay(300).Wait();
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("Y") || IsKeyPressed("GamepadY")))
            {
                if (effects)
                {
                    MainPage.Current.muteEffect();
                    effects = false;
                }
                else
                {
                    MainPage.Current.unmuteEffect();
                    effects = true;
                }
                Task.Delay(300).Wait();
            }

            if (MainPage.Current.game_over && (IsKeyPressed("Space") || IsKeyPressed("GamepadMenu")))
            {
                CoreApplication.RequestRestartAsync("");
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("Enter") || IsKeyPressed("GamepadMenu")))
            {
                MainPage.Current.getInfo();
                Task.Delay(300).Wait();
            }
            else if (MainPage.Current.infoScreen && !MainPage.Current.controle && (IsKeyPressed("Space") || IsKeyPressed("GamepadView")))
            {
                MainPage.Current.getControls();
                Task.Delay(300).Wait();
            }

            else if (MainPage.Current.controle && (IsKeyPressed("Space") || IsKeyPressed("GamepadView")))
            {
                MainPage.Current.removeControls();
                Task.Delay(300).Wait();
            }
            else if (MainPage.Current.infoScreen && !MainPage.Current.controle && (IsKeyPressed("Enter") || IsKeyPressed("GamepadMenu")))
            {
                MainPage.Current.removeInfo();
                Task.Delay(300).Wait();
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("X") || IsKeyPressed("GamepadX")))
            {
                MainPage.Current.getAbout();
                Task.Delay(300).Wait();
            }
            else if (MainPage.Current.aboutScreen && (IsKeyPressed("X") || IsKeyPressed("GamepadX")))
            {
                MainPage.Current.removeAbout();
                Task.Delay(300).Wait();
            }

            if(MainPage.Current.activeStartup && pressedKeys.Count() > 0)
            {
                MainPage.Current.startup();
                Task.Delay(300).Wait();
            }

            
        }

        //Invilidate the drawing currently on the canvas. The canvas wil call an action to redraw itself.
        private void Draw()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>{
                canvasControl.Invalidate();
            });
        }

        public void DrawEvent(CanvasControl sender, CanvasDrawEventArgs args)
        {
            //Set the canvasControl that called this method so we know what to Invalidate later.
            canvasControl = sender;

            //Check if there are objects in the arraylist to draw
            if (gameObjects.Count < 1)
            {
                return;
            }

            //Create a new arraylist used to hold the gameobjects for this loop.
            //The copy is made so it does the ontick methods on all the objects even the onces destroyed in the proces.
            ArrayList loopList; 
            lock (gameObjects) //lock the gameobjects for duplication
            { 
                try
                {
                    //Try to duplicate the arraylist.
                    loopList = new ArrayList(gameObjects);
                }
                catch
                {
                    //if it failes for any reason skip this frame.
                    return;
                }
            }

            /* PREPARING THE SPRITES */
            //Load the sprite in this canvasControl so it is usable later
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject.Sprite == null)
                {
                    gameObject.CreateResourcesAsync(sender);
                }
            }

            /* DRAWING THE SPLATTER SPRITES */
            //Draw the splatter first so all other sprites are drawn upon it. (making the splater apear on the ground)
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject.Sprite != null && gameObject is Splatter)
                {
                    args.DrawingSession.DrawImage(
                    gameObject.Sprite,
                    new Rect(
                        gameObject.FromLeft + gameObject.FromLeftDrawOffset,
                        gameObject.FromTop + gameObject.FromTopDrawOffset,
                        gameObject.Width + gameObject.WidthDrawOffset,
                        gameObject.Height + gameObject.HeightDrawOffset
                    ));
                }
            }

            /* DRAWING THE OTHER SPRITES */
            //Draw the loaded sprites on the correct location
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject.Sprite != null && !(gameObject is Splatter))
                {
                    args.DrawingSession.DrawImage(
                    gameObject.Sprite,
                    new Rect(
                        gameObject.FromLeft + gameObject.FromLeftDrawOffset,
                        gameObject.FromTop + gameObject.FromTopDrawOffset,
                        gameObject.Width + gameObject.WidthDrawOffset,
                        gameObject.Height + gameObject.HeightDrawOffset
                    ));
                }
            }

            //Drawing Enemy Healthbars
            foreach (GameObject gameObject in loopList)
            {
                Enemy enemy = gameObject as Enemy;
                if (enemy is Enemy)
                {
                    if (enemy.GetLifePoints() < enemy.GetMaxLifePoints())
                    {
                        //Calculate the percentage health left
                        float percentage = 1 + ((enemy.GetLifePoints() - enemy.GetMaxLifePoints()) / enemy.GetMaxLifePoints());

                        if (percentage < 0) { percentage = 0.1f; } //If the target has negative health here, put the percentage on 0.1. 
                        //(this also stops devided by 0 errors while the user wont see the health left)

                        args.DrawingSession.FillRectangle(
                            new Rect(
                                gameObject.FromLeft - gameObject.Width / 5, //The healthbar starts 1/5th left from the target
                                gameObject.FromTop - gameObject.Width / 2,  //The healthbar starts 1/5th above from the target
                                (gameObject.Width + gameObject.Width / 5),  //The healthbar is 1/5th bigger then the target
                                gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                            Colors.Red
                        );

                        args.DrawingSession.FillRectangle(
                            new Windows.Foundation.Rect(
                                gameObject.FromLeft - gameObject.Width / 5, //The healthbar starts 1/5th left from the target
                                gameObject.FromTop - gameObject.Width / 2,  //The healthbar starts 1/5th above from the target
                                (gameObject.Width + gameObject.Width / 5) * percentage, //Draw only the health left!
                                gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                            Colors.Green
                        );
                    }
                }
            }

            //Drawing Player Healthbars
            foreach (GameObject gameObject in loopList)
            {
                Player player = gameObject as Player;
                if (player is Player)
                {                    
                    //Calculate the percentage health left
                    float percentage = 1 + ((player.getHealth() - player.getMaxHealth()) / player.getMaxHealth());

                    if (percentage < 0) { percentage = 0.1f; } //If the target has negative health here, put the percentage on 0.1. 
                    //(this also stops devided by 0 errors while the user wont see the health left)

                    args.DrawingSession.FillRectangle(
                        new Windows.Foundation.Rect(
                            gameObject.FromLeft - gameObject.Width / 5, //The healthbar starts 1/5th left from the target
                            gameObject.FromTop - gameObject.Width / 2,  //The healthbar starts 1/5th above from the target
                            (gameObject.Width + gameObject.Width / 5),  //The healthbar is 1/5th bigger then the target
                            gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                        Colors.Red
                    );

                    args.DrawingSession.FillRectangle(
                        new Windows.Foundation.Rect(
                            gameObject.FromLeft - gameObject.Width / 5, //The healthbar starts 1/5th left from the target
                            gameObject.FromTop - gameObject.Width / 2,  //The healthbar starts 1/5th above from the target
                            (gameObject.Width + gameObject.Width / 5) * percentage, //Draw only the health left!
                            gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                        Colors.Green
                    );                    
                }
            }

            //Drawing Player Armourbars
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject is Player)
                {
                    //Calculate the percentage health left
                    float percentage = 1 + ((player.getArmour() - player.getMaxArmour()) / player.getMaxArmour());

                    if (percentage < 0) { percentage = 0.1f; } //If the target has negative health here, put the percentage on 0.1. 
                    //(this also stops devided by 0 errors while the user wont see the health left)

                    args.DrawingSession.FillRectangle(
                        new Rect(
                            gameObject.FromLeft - gameObject.Width / 5, //The healthbar starts 1/5th left from the target
                            gameObject.FromTop - gameObject.Width / 3.33,  //The healthbar starts 1/5th above from the target
                            (gameObject.Width + gameObject.Width / 5),  //The healthbar is 1/5th bigger then the target
                            gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                        Colors.Transparent
                    );

                    args.DrawingSession.FillRectangle(
                        new Rect(
                            gameObject.FromLeft - gameObject.Width / 5, //The healthbar starts 1/5th left from the target
                            gameObject.FromTop - gameObject.Width / 3.33,  //The healthbar starts 1/5th above from the target
                            (gameObject.Width + gameObject.Width / 5) * percentage, //Draw only the health left!
                            gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                        Colors.Blue
                    );
                }
            }

            //Drawing textBoxes
            foreach (GameObject gameObject in loopList)
            {
                TextBox textBox = gameObject as TextBox;
                if (textBox is TextBox)
                {
                    args.DrawingSession.DrawText(
                        textBox.Text,
                        new Rect(
                        gameObject.FromLeft + gameObject.FromLeftDrawOffset,
                        gameObject.FromTop + gameObject.FromTopDrawOffset,
                        gameObject.Width + gameObject.WidthDrawOffset,
                        gameObject.Height + gameObject.HeightDrawOffset
                        ),
                        textBox.Color,
                        new CanvasTextFormat()
                        {
                            FontFamily = "Assets\\lunchds.ttf#Lunchtime Doubly So",
                            FontSize = textBox.FontSize
                        }
                    );
                }
            }
        }

        public void NextWeapon()
        {
            player.selectNextWeapon();
            while (player.activeWeapon.GetAmmo() <= 0)
            {
                player.selectNextWeaponDelay = 0;
                player.selectNextWeapon();
            }
            player.selectNextWeaponDelay = 1000;
            MainPage.Current.UpdateWeapon();
        }

        public void PreviousWeapon()
        {
            player.selectPreviousWeapon();
            while (player.activeWeapon.GetAmmo() <= 0)
            {
                player.selectNextWeaponDelay = 0;
                player.selectPreviousWeapon();
            }
            player.selectNextWeaponDelay = 1000;
            MainPage.Current.UpdateWeapon();
        }

        public void KeyDown(String virtualKey)
        {
            pressedKeys.Add(virtualKey);
        }

        public void KeyUp(String virtualKey)
        {
            pressedKeys.Remove(virtualKey);
        }

        public Boolean IsKeyPressed(String virtualKey)
        {
            return pressedKeys.Contains(virtualKey);
        }
    }
}
 
 
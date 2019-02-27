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

        private World world;

        private Camera camera;

        //Holder for the canvasControl
        private CanvasControl canvasControl;

        //For timekeeping (we need to know when the last frame happend when the next frame happens and the delta between)
        private long delta;     //The lenght in time the last frame lasted (so we can use it to calculate speeds of things without slowing down due to low fps)
        private long now;       //This is the time of the frame. (To calculate the delta)
        private long then;      //This is the time of the previous draw frame. (To calculate the delta)

        //The max fps we want to run at
        private float fps;  //The set FPS limit
        private float interfal; //Interfal that gets calculated based on the fps

        public bool music { get; set; }
        public bool effects { get; set; }

        private bool paused;

        private SoundController soundController;

        private Player player;

        public Engine()
        {
            gameObjects = new List<GameObject>();

            soundController = new SoundController();

            player = new Player(15, 15, 656, 312, 0, 0, 0, 0);

            camera = new Camera(new Target(player));

            //Add sounds to the soundController.
            //(The first argument is the location, the second the volume)
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

            //Set states for the engine.
            music = true;
            effects = true;
            paused = true;

            scenes = new List<Scene>();
            world = new World();

            pressedKeys = new HashSet<String>();

            player.AddTag("controllable");  //Make the player controllable
            gameObjects.Add(player); //Add the player to the gameObjects list

            //Load in the world
            gameObjects.AddRange(world.StartingBlock.LoadScene());

            /* Manualy load some of the worldpieces */
            gameObjects.AddRange(world.StartingBlock.Up.LoadScene());
            gameObjects.AddRange(world.StartingBlock.Up.Up.LoadScene());

            gameObjects.AddRange(world.StartingBlock.Down.LoadScene());
            gameObjects.AddRange(world.StartingBlock.Down.Down.LoadScene());

            gameObjects.AddRange(world.StartingBlock.Right.LoadScene());
            gameObjects.AddRange(world.StartingBlock.Right.Right.LoadScene());

            gameObjects.AddRange(world.StartingBlock.Left.LoadScene());
            gameObjects.AddRange(world.StartingBlock.Left.Left.LoadScene());

            gameObjects.AddRange(world.StartingBlock.Right.Up.LoadScene());
            gameObjects.AddRange(world.StartingBlock.Left.Up.LoadScene());

            gameObjects.AddRange(world.StartingBlock.Right.Down.LoadScene());
            gameObjects.AddRange(world.StartingBlock.Left.Down.LoadScene());

            //Set the FPS and calculate the interfal!
            fps = 60;
            interfal = 1000 / fps; //1 second is 1000 ms.

            //Set then to the current time to know when we started
            then = Stopwatch.GetTimestamp();

            paused = true;
        }

        public Player getPlayer() => player;
        public SoundController GetSoundController => soundController;

        /* LoadScene() */
        /* 
         * Load the scene located at the given index
         * If no index is given scene 0 will be loaded
        */
        public bool LoadScene() { return LoadScene(0); }
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

        /* Run() */
        /* 
            * This method is the main cycle of the game.
            * Will calculate if its time to draw a new frame and then will.
            * Also recurcifly schedules itself.
        */
        public void Run()
        {
            /*now = Stopwatch.GetTimestamp();
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
            */
            LogicLoop();
            DrawLoopAsync();
        }

        public async void LogicLoop()
        {
            await Task.Run(() => {
                Logic();
            });
            LogicLoop();
        }

        public async Task DrawLoopAsync()
        {
            //Only draw the simulation if there is a known canvas.
            if (canvasControl != null)
            {
                Draw();
            }

            await Task.Delay((int)interfal);

            Task.Run(() => DrawLoopAsync());  //Schedule new DrawLoopAsync() task
        }

        //Invilidate the drawing currently on the canvas. The canvas wil call an action to redraw itself.
        private void Draw()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () => {
                canvasControl.Invalidate();
            });
        }


        /* Logic */
        /*
         * Logic function is called every frame. 
         * This method is used to handle how GameObjects should interect in the game.
         * This includes Movement and Collition Detection.
         * Also the keybord controls are handled here.
        */
        private void Logic()
        {
            //TaskLogicTwo();

            paused = MainPage.Current.paused;

            HandleMenuControls();

            if (!paused && !MainPage.Current.game_over)
            {
                HandleInGameMenuControls();

                //Check if there are objects in the List to apply logic on
                //Apply the logic to all the bameObjects CURRENTLY in the List.
                //The new List makes a copy so the original arraylist can be modivied
                foreach (GameObject gameObject in new List<GameObject>(gameObjects))
                {
                    //Handle player input
                    Player player = gameObject as Player;

                    
                    if (player is Player)
                    {
                        HandlePlayerWeaponControls(player);
                        HandlePlayerMovementControls(player);

                        
                        if (player.IsWalking)
                        {
                            if (player.deltaForWalkingSound > 1300)
                            {
                                soundController.PlaySound(player.MoveSound);
                                player.deltaForWalkingSound = 0;
                            }
                            player.deltaForWalkingSound += 200;
                        }
                        
                        
                    }


                    //gameObject.OnTick(gameObjects, delta);
                    gameObject.OnTick(gameObjects);

                    //Move the camera
                    camera.OnTick();

                    //For every object in this loop, loop trough all objects to check if they are coliding
                    foreach (GameObject gameObjectCheck in new ArrayList(gameObjects))
                    {
                        //If the two objects are colliding
                        if (gameObject.IsColliding(gameObjectCheck))
                        {
                            //Do the collition effect
                            gameObject.CollitionEffect(gameObjectCheck);
                            gameObjectCheck.CollitionEffect(gameObject);
                        }
                    }
                }

                //Handle the tags for each gameobject if there are any tags to handle
                foreach (GameObject gameObjectCheck in new ArrayList(gameObjects))
                {
                    handleTaggsGameObject(gameObjectCheck);
                }
            }

        }

      



        /* HandlePlayerWeaponControls */
        /* 
            * Handles the player controls that have to do with movement.
            * The player needs to have the tag "controllable" to be controlled. 
            * This is done so control can be taken away. (during cutscenes, stunns/roots. ect)
        */
        private void HandlePlayerMovementControls(Player player)
        {
            player.IsWalking = false;

            if (player.HasTag("controllable") && (IsKeyPressed("S") || IsKeyPressed("GamepadLeftThumbstickDown")))
            {
                player.Target.AddFromTop(1000);
                player.IsWalking = true;
            }

            if (player.HasTag("controllable") && (IsKeyPressed("W") || IsKeyPressed("GamepadLeftThumbstickUp")))
            {
                player.Target.AddFromTop(-1000);
                player.IsWalking = true;
            }

            if (player.HasTag("controllable") && (IsKeyPressed("D") || IsKeyPressed("GamepadLeftThumbstickRight")))
            {
                player.Target.AddFromLeft(1000);
                player.IsWalking = true;
            }

            if (player.HasTag("controllable") && (IsKeyPressed("A") || IsKeyPressed("GamepadLeftThumbstickLeft")))
            {
                player.Target.AddFromLeft(-1000);
                player.IsWalking = true;
            }
        }

        /* HandlePlayerWeaponControls */
        /* 
         * Handles the player controls that have to do with weapons 
        */
        private void HandlePlayerWeaponControls(Player player)
        {
            if (player.HasTag("controllable") && (IsKeyPressed("E") || IsKeyPressed("GamepadRightShoulder")))
            {
                NextWeapon(player);
            }

            if (player.HasTag("controllable") && (IsKeyPressed("Q") || IsKeyPressed("GamepadLeftShoulder")))
            {
                PreviousWeapon(player);
            }

            if (player.HasTag("controllable") && (IsKeyPressed("Right") || IsKeyPressed("GamepadRightThumbstickRight")))
            {
                if (player.Fire("Right", gameObjects))
                {
                    soundController.PlaySound(player.GetActiveWeapon().shotSound);
                }
            }
            else if (player.HasTag("controllable") && (IsKeyPressed("Up") || IsKeyPressed("GamepadRightThumbstickUp")))
            {
                if (player.Fire("Top", gameObjects))
                {
                    soundController.PlaySound(player.GetActiveWeapon().shotSound);
                }
            }
            else if (player.HasTag("controllable") && (IsKeyPressed("Down") || IsKeyPressed("GamepadRightThumbstickDown")))
            {
                if (player.Fire("Bottom", gameObjects))
                {
                    soundController.PlaySound(player.GetActiveWeapon().shotSound);
                }
            }
            else if (player.HasTag("controllable") && (IsKeyPressed("Left") || IsKeyPressed("GamepadRightThumbstickLeft")))
            {
                if (player.Fire("Left", gameObjects))
                {
                    soundController.PlaySound(player.GetActiveWeapon().shotSound);
                }
            }
        }

        /* HandleMenuControls */
        /* 
         * While the game is running this method 
         * Handles gameControls that have to do with menuing
        */
        private void HandleInGameMenuControls()
        {
            if (!paused && (IsKeyPressed("Space") || IsKeyPressed("GamepadView")))
            {
                MainPage.Current.getControls();
            }
            else if (!paused && (!IsKeyPressed("Space") || !IsKeyPressed("GamepadView")))
            {
                MainPage.Current.removeControls();
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

        /* HandleMenuControls */
        /*
         * Handles gameControls that have to do with menuing
        */
        private void HandleMenuControls()
        {

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

            if (MainPage.Current.activeStartup && pressedKeys.Count() > 0)
            {
                MainPage.Current.startup();
                Task.Delay(300).Wait();
            }
        }

        /* handleTaggsGameObject */
        /*
         * GameObjects have a list of tags they can contain. 
         * The logic of those tags is done here.
         * This method is called in Logic();
        */
        private void handleTaggsGameObject(GameObject gameObjectCheck)
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
                                    //if (p4.GetLevel() == 5)
                                    //{
                                    //    gameObjects.Add(new Spawner(10, 10, 110, 213, 0, 0, 0, 0, 5000, 25000));
                                    //    MainPage.Current.enableSecondSpawner();
                                    //}
                                    //if (p4.GetLevel() == 10)
                                    //{
                                    //    gameObjects.Add(new Spawner(10, 10, 770, 400, 0, 0, 0, 0, 5000, 25000));
                                    //    MainPage.Current.enableThirdSpawner();
                                    //}
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


        /* CreateAllResourcesAsync */
        /*
         * To be able to use the sprites on a Canvas the sprites needs to be loaded as CanvasBitmaps.
         * The first argument is the sender, the second is the list of gameObjects.
        */
        private void CreateAllResourcesAsync(CanvasControl sender, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject != null && gameObject.Sprite == null)
                {
                    gameObject.CreateResourcesAsync(sender);
                }
            }
        }

        /* DrawAllSplatterSprites */
        /*
         * Draws every sprite of which the class is splatter.
         * The first argument is the canvasDrawEventArgs (containing data from the draw event).
         * The second is the list of gameObjects.
        */
        private void DrawAllSplatterSprites(CanvasDrawEventArgs args, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject != null && gameObject.Sprite != null && gameObject is Splatter)
                {
                    //Drawing requires the sides to be non-negative
                    if (
                        gameObject.Width + gameObject.WidthDrawOffset > 0 &&
                        gameObject.Height + gameObject.HeightDrawOffset > 0
                    )
                    {

                        if (gameObject.Rectangle == null)
                        {
                            gameObject.Rectangle = new Rect(
                                gameObject.FromLeft + gameObject.FromLeftDrawOffset + camera.LeftOffset(),
                                gameObject.FromTop + gameObject.FromTopDrawOffset + camera.TopOffset(),
                                gameObject.Width + gameObject.WidthDrawOffset,
                                gameObject.Height + gameObject.HeightDrawOffset
                            );
                        }
                        else
                        {
                           gameObject.rectangle.X = gameObject.FromLeft + gameObject.FromLeftDrawOffset + camera.LeftOffset();
                           gameObject.rectangle.Y = gameObject.FromTop + gameObject.FromTopDrawOffset + camera.TopOffset();
                           gameObject.rectangle.Width = gameObject.Width + gameObject.WidthDrawOffset;
                           gameObject.rectangle.Height = gameObject.Height + gameObject.HeightDrawOffset;
                        }

                        
                        args.DrawingSession.DrawImage(
                        gameObject.Sprite,gameObject.rectangle);
                    }
                }
            }
        }

        /* DrawAllNonSplatterSprites */
        /*
         * Draws every sprite of which the class is NOT splatter.
         * The first argument is the canvasDrawEventArgs (containing data from the draw event).
         * The second is the list of gameObjects.
        */
        private void DrawAllNonSplatterSprites(CanvasDrawEventArgs args, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject != null && gameObject.Sprite != null && !(gameObject is Splatter))
                {

                    //Drawing requires the sides to be non-negative
                    if (
                        gameObject.Width + gameObject.WidthDrawOffset > 0 &&
                        gameObject.Height + gameObject.HeightDrawOffset > 0
                    )
                    {

                        if (gameObject.Rectangle == null)
                        {
                            gameObject.Rectangle = new Rect(
                                gameObject.FromLeft + gameObject.FromLeftDrawOffset + camera.LeftOffset(),
                                gameObject.FromTop + gameObject.FromTopDrawOffset + camera.TopOffset(),
                                gameObject.Width + gameObject.WidthDrawOffset,
                                gameObject.Height + gameObject.HeightDrawOffset
                            );
                        }
                        else
                        {
                            gameObject.rectangle.X = gameObject.FromLeft + gameObject.FromLeftDrawOffset + camera.LeftOffset();
                            gameObject.rectangle.Y = gameObject.FromTop + gameObject.FromTopDrawOffset + camera.TopOffset();
                            gameObject.rectangle.Width = gameObject.Width + gameObject.WidthDrawOffset;
                            gameObject.rectangle.Height = gameObject.Height + gameObject.HeightDrawOffset;
                        }


                        args.DrawingSession.DrawImage(
                        gameObject.Sprite, gameObject.rectangle);
                    }
                }
            }
        }

        /* DrawAllEnemyHealthBars */
        /*
         * Draws the healthbar indicating the amount of health an Enemy object has.
         * The first argument is the canvasDrawEventArgs (containing data from the draw event).
         * The second is the list of gameObjects.
        */
        private void DrawAllEnemyHealthBars(CanvasDrawEventArgs args, ArrayList loopList)
        {
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
                                gameObject.FromLeft - gameObject.Width / 5 + camera.LeftOffset(), //The healthbar starts 1/5th left from the target
                                gameObject.FromTop - gameObject.Width / 2 + camera.TopOffset(),  //The healthbar starts 1/5th above from the target
                                (gameObject.Width + gameObject.Width / 5),  //The healthbar is 1/5th bigger then the target
                                gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                            Colors.Red
                        );

                        args.DrawingSession.FillRectangle(
                            new Windows.Foundation.Rect(
                                gameObject.FromLeft - gameObject.Width / 5 + camera.LeftOffset(), //The healthbar starts 1/5th left from the target
                                gameObject.FromTop - gameObject.Width / 2 + camera.TopOffset(),  //The healthbar starts 1/5th above from the target
                                (gameObject.Width + gameObject.Width / 5) * percentage, //Draw only the health left!
                                gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                            Colors.Green
                        );
                    }
                }
            }
        }

        /* DrawAllPlayerHealthBars */
        /*
         * Draws the healthbar indicating the amount of health an Player object has.
         * The first argument is the canvasDrawEventArgs (containing data from the draw event).
         * The second is the list of gameObjects.
        */
        private void DrawAllPlayerHealthBars(CanvasDrawEventArgs args, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                Player player = gameObject as Player;
                if (player is Player)
                {
                    //Calculate the percentage health left
                    float percentage = 1 + ((player.getHealth() - player.getMaxHealth()) / player.getMaxHealth());

                    if (percentage < 0) { percentage = 0.1f; } //If the target has negative health here, put the percentage on 0.1. 
                    //(this also stops devided by 0 errors while the user wont see the health left)

                    /* 
                     * First the red bar is drawn, this bar is used to display the missing proportion of health.
                     * This bar is drawn full with, a second bar wil overlay the first bar in green.
                     * The green portion of the bar indicates the remaining health.
                     */

                    args.DrawingSession.FillRectangle(
                        new Windows.Foundation.Rect(
                            gameObject.FromLeft - gameObject.Width / 5 + camera.LeftOffset(), //The healthbar starts 1/5th left from the target
                            gameObject.FromTop - gameObject.Width / 2 + camera.TopOffset(),  //The healthbar starts 1/5th above from the target
                            (gameObject.Width + gameObject.Width / 5),  //The healthbar is 1/5th bigger then the target
                            gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                        Colors.Red
                    );

                    args.DrawingSession.FillRectangle(
                        new Windows.Foundation.Rect(
                            gameObject.FromLeft - gameObject.Width / 5 + camera.LeftOffset(), //The healthbar starts 1/5th left from the target
                            gameObject.FromTop - gameObject.Width / 2 + camera.TopOffset(),  //The healthbar starts 1/5th above from the target
                            (gameObject.Width + gameObject.Width / 5) * percentage, //Draw only the health left!
                            gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                        Colors.Green
                    );
                }
            }
        }


        /* DrawAllPlayerArmourBars */
        /*
         * Draws the armourbar representing the amount of armour an Player object has.
         * The first argument is the canvasDrawEventArgs (containing data from the draw event).
         * The second is the list of gameObjects.
        */
        private void DrawAllPlayerArmourBars(CanvasDrawEventArgs args, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject is Player)
                {
                    //Calculate the percentage armour left
                    float percentage = 1 + ((player.getArmour() - player.getMaxArmour()) / player.getMaxArmour());

                    if (percentage < 0) { percentage = 0.1f; } //If the target has negative health here, put the percentage on 0.1. 
                    //(this also stops devided by 0 errors while the user wont see the health left)


                    args.DrawingSession.FillRectangle(
                        new Rect(
                            gameObject.FromLeft - gameObject.Width / 5 + camera.LeftOffset(), //The armourbar starts 1/5th left from the target
                            gameObject.FromTop - gameObject.Width / 3.33 + camera.TopOffset(),  //The armourbar starts 1/5th above from the target
                            (gameObject.Width + gameObject.Width / 5) * percentage, //Draw only the armour left!
                            gameObject.Height / 5), //The healthbar is 1/5th the size of the target
                        Colors.Blue
                    );
                }
            }
        }

        /* DrawAllTextBoxes */
        /*
         * Draws the textBoxes on the canvas.
         * The first argument is the canvasDrawEventArgs (containing data from the draw event).
         * The second is the list of gameObjects.
        */
        private void DrawAllTextBoxes(CanvasDrawEventArgs args, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                TextBox textBox = gameObject as TextBox;
                if (textBox is TextBox)
                {
                    //If the class is a textBox draw text on the location of the object.

                    args.DrawingSession.DrawText(
                        textBox.Text,
                        new Rect(
                        gameObject.FromLeft + gameObject.FromLeftDrawOffset + camera.LeftOffset(),
                        gameObject.FromTop + gameObject.FromTopDrawOffset + camera.TopOffset(),
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

        /* DrawEvent */
        /*
         * When the canvasControl is invalidated (canvasControl.Invalidate) this function is called to redraw the canvas.
         * The first argument is the CanvasControl providing Immediate mode 2D rendering.
         * Immediate Mode means that the client can directly call to the canvas what it wants rendered and it wil render it.
         * (Eventhough it seems like a whole frame is redrawn, win2 has certain knowlage on what is rendered so its not as intensive as it seems)
         * The second argument is the canvasDrawEventArgs (containing data from the draw event).
        */
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
                //try
                //{
                    //Try to duplicate the arraylist.
                    loopList = new ArrayList(gameObjects);
                //}
                //catch
                //{
                    //if it failes for any reason skip this frame.
                //    return;
                //}
            }

            /* PREPARING THE SPRITES */
            /*
             * To create the illusion of depth the spirits are drawn in a certain order.
             * The sprites that should be on top of other sprites need to be drawn later.
             * This is why we first draw the Splatter and then all the Non Splatter sprites.
             * After drawing the sprites the UI elements (health and armour bars) are drawn so they are always visable on top.
            */

            //Load the sprite in this canvasControl so it is usable later
            CreateAllResourcesAsync(sender, loopList);

            //Draw the splatter first so all other sprites are drawn upon it. (making the splater apear on the ground)
            DrawAllSplatterSprites(args, loopList);

            //Draw the loaded sprites on the correct location
            DrawAllNonSplatterSprites(args, loopList);

            //Drawing Enemy Healthbars
            DrawAllEnemyHealthBars(args, loopList);

            //Drawing Player Healthbars
            DrawAllPlayerHealthBars(args, loopList);

            //Drawing Player Armourbars
            DrawAllPlayerArmourBars(args, loopList);

            //Drawing textBoxes
            DrawAllTextBoxes(args, loopList);
        }

        /* NextWeapon */
        /* 
         * Select the next weapon the player has, then check its amunition.
         * If the newly selected weapon has no ammunition select the next weapon again.
         * This way it skips over weapons that don't have anny ammunition left.
         * The argument is the given player.
         */
        public void NextWeapon(Player player)
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

        /* PreviousWeapon */
        /* 
         * Select the previous weapon the player has, then check its amunition.
         * If the newly selected weapon has no ammunition select the previous weapon again.
         * This way it skips over weapons that don't have anny ammunition left.
         * The argument is the given player.
         */
        public void PreviousWeapon(Player player)
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

        /* KeyDown */
        /* 
         * Add the given key in the pressedKeys collection.
         * The argument is the given key represented as a string.
         */
        public void KeyDown(String virtualKey)
        {
            pressedKeys.Add(virtualKey);
        }


        /* KeyDown */
        /* 
         * Remove the given key in the pressedKeys collection.
         * The argument is the given key represented as a string.
         */
        public void KeyUp(String virtualKey)
        {
            pressedKeys.Remove(virtualKey);
        }

        /* IsKeyPressed */
        /* 
         * Returns wheater the given key exists within the pressedKeys collection.
         * The argument is the given key represented as a string.
         */
        public bool IsKeyPressed(String virtualKey)
        {
            return pressedKeys.Contains(virtualKey);
        }
    }
}
 
 
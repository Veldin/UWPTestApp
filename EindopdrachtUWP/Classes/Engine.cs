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
        private HashSet<string> pressedKeys;

        //Holds the different scenes in the Engine! If a scene is loaded the objects in the scene are put in the gameObjects array!
        private List<Scene> scenes;
        private Scene scene;

        private World world;
        private int blockFromLeft;
        private int blockFromTop;

        private Camera camera;

        //Holder for the canvasControl
        private CanvasControl canvasControl;

        //For timekeeping (we need to know when the last frame happend when the next frame happens and the delta between)
        private readonly long then;      //This is the time of the previous draw frame. (To calculate the delta)

        //The max fps we want to run at
        private readonly float fps;  //The set FPS limit
        private readonly float interfal; //Interfal that gets calculated based on the fps

        public bool Music { get; set; }
        public bool Effects { get; set; }

        private bool paused;

        private bool enableCheats;

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
            soundController.AddSound(player.HealthLowSound, 1.0);
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
            Music = true;
            Effects = true;
            paused = true;

            scenes = new List<Scene>();
            world = new World();
            blockFromLeft = 0;
            blockFromTop = 0;

            pressedKeys = new HashSet<string>();

            player.AddTag("controllable");  //Make the player controllable
            gameObjects.Add(player); //Add the player to the gameObjects list

            LoadBlock(blockFromLeft, blockFromTop);

            //Load Left
            LoadBlock(blockFromLeft - 1, blockFromTop);
            LoadBlock(blockFromLeft - 2, blockFromTop);
            LoadBlock(blockFromLeft - 3, blockFromTop);


            //Load Right
            LoadBlock(blockFromLeft + 1, blockFromTop);
            LoadBlock(blockFromLeft + 2, blockFromTop);
            LoadBlock(blockFromLeft + 3, blockFromTop);


            //Load Up
            LoadBlock(blockFromLeft, blockFromTop + 1);
            LoadBlock(blockFromLeft, blockFromTop + 2);
            LoadBlock(blockFromLeft, blockFromTop + 3);


            //Load Down
            LoadBlock(blockFromLeft, blockFromTop - 1);
            LoadBlock(blockFromLeft, blockFromTop - 2);
            LoadBlock(blockFromLeft, blockFromTop - 3);


            //Load sides
            LoadBlock(blockFromLeft - 1, blockFromTop + 1);
            LoadBlock(blockFromLeft + 1, blockFromTop - 1);
            LoadBlock(blockFromLeft - 1, blockFromTop - 1);
            LoadBlock(blockFromLeft + 1, blockFromTop + 1);

            //Set the FPS and calculate the interfal!
            fps = 60;
            interfal = 1000 / fps; //1 second is 1000 ms.

            //Set then to the current time to know when we started
            then = Stopwatch.GetTimestamp();

            paused = true;
            enableCheats = false;

        }

        public Player GetPlayer() => player;
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

            Task.Yield();  //Force this task to complete asynchronously (This way the main is not blocked by this task calling itself.
            Task.Run(() => Run());  //Schedule new Run() task
            */
            LogicLoop();
            DrawLoopAsync();
        }

        /* LogicLoop() */
        /* 
            * Calls the Logic method and instandaniusly creates a task that calls itelf if the logic method is completed.
        */
        public async void LogicLoop()
        {
            await Task.Run(() => {
                Logic();
            });
            LogicLoop();
        }

        /* DrawLoopAsync() */
        /* 
            * Calls the Draw method and create a delayed that lasts as long as the interfal is.
            * Doing this limits the FPS to the given FPS (which is used to calculate the interval beforehand)
        */
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

        /* LoadBlock() */
        /* 
            *Get the gameobjects at a certain location and set that worldblock on Loaded.
        */
        public void LoadBlock(int blockFromLeft, int blockFromTop)
        {
            WorldBlock needle = world.StartingBlock;

            //If the from Left is more then 0 move the needle right and reduce the blocks from left
            //do this until the needle is at the right place
            if (blockFromLeft > 0)
            {
                while (blockFromLeft > 0)
                {
                    needle = needle.Right;
                    blockFromLeft--;
                }
            }

            //If the from Left is less then 0 move the needle left and increase the blocks from left
            //do this until the needle is at the right place
            if (blockFromLeft < 0)
            {
                while (blockFromLeft < 0)
                {
                    needle = needle.Left;
                    blockFromLeft++;
                }
            }


            if (blockFromTop > 0)
            {
                while (blockFromTop > 0)
                {
                    needle = needle.Down;
                    blockFromTop--;
                }
            }

            if (blockFromTop < 0)
            {
                while (blockFromTop < 0)
                {
                    needle = needle.Up;
                    blockFromTop++;
                }
            }

            //Now the needle is set where the player is. Load al adjecent rooms too
            if (!needle.IsLoaded())
            {
                gameObjects.AddRange(needle.LoadScene());
            }
        }

        /* Logic */
        /*
         * Logic function is called every frame. 
         * This method is used to handle how GameObjects should interect in the game.
         * This includes Movement and Collision Detection.
         * Also the keybord controls are handled here.
        */
        private void Logic()
        {
            paused = MainPage.Current.paused;

            HandleMenuControls();

            if (!paused && !MainPage.Current.game_over)
            {
                HandleInGameMenuControls();

                
                //Move the camera
                camera.OnTick();

                /* 
                 * 
                 * Split the gameObjects in two lists, a list of gameObjects near the player and far away from the player
                 * 
                 * The gameObjects that are to far don't have to do anything exept update the timer. This is to save CPU power.
                 * If the game is paused all gameobjects are considered inactive.
                 * 
                 * The gameObjects near the target have to do the full logic cycle.
                 */

                IEnumerable<GameObject> inactiveObjects;
                IEnumerable<GameObject> activeObjects;

                if (MainPage.Current.paused)
                {
                    inactiveObjects = gameObjects;
                    activeObjects = null;
                }
                else
                {
                    //if the blockFromLeft/Top are not the same as the currenty calculated blockFromLeft/Top
                    if (
                        blockFromLeft != (int)Math.Floor(player.FromLeft / world.StartingBlock.Width) 
                        ||
                        blockFromTop != (int)Math.Floor(player.FromTop / world.StartingBlock.Height)
                    )
                    {
                        LoadBlock(blockFromLeft, blockFromTop);

                        //Load Left
                        LoadBlock(blockFromLeft - 1, blockFromTop);
                        LoadBlock(blockFromLeft - 2, blockFromTop);
                        LoadBlock(blockFromLeft - 3, blockFromTop);
                        LoadBlock(blockFromLeft - 4, blockFromTop);



                        //Load Right
                        LoadBlock(blockFromLeft + 1, blockFromTop);
                        LoadBlock(blockFromLeft + 2, blockFromTop);
                        LoadBlock(blockFromLeft + 3, blockFromTop);
                        LoadBlock(blockFromLeft + 4, blockFromTop);



                        //Load Up
                        LoadBlock(blockFromLeft, blockFromTop + 1);
                        LoadBlock(blockFromLeft, blockFromTop + 2);
                        LoadBlock(blockFromLeft, blockFromTop + 3);
                        LoadBlock(blockFromLeft, blockFromTop + 4);



                        //Load Down
                        LoadBlock(blockFromLeft, blockFromTop - 1);
                        LoadBlock(blockFromLeft, blockFromTop - 2);
                        LoadBlock(blockFromLeft, blockFromTop - 3);
                        LoadBlock(blockFromLeft, blockFromTop - 4);



                        //Load sides
                        LoadBlock(blockFromLeft - 1, blockFromTop + 1);
                        LoadBlock(blockFromLeft + 1, blockFromTop - 1);
                        LoadBlock(blockFromLeft - 1, blockFromTop - 1);
                        LoadBlock(blockFromLeft + 1, blockFromTop + 1);

                        LoadBlock(blockFromLeft - 1, blockFromTop + 2);
                        LoadBlock(blockFromLeft + 1, blockFromTop - 2);
                        LoadBlock(blockFromLeft - 1, blockFromTop - 2);
                        LoadBlock(blockFromLeft + 1, blockFromTop + 2);
                        
                        LoadBlock(blockFromLeft - 2, blockFromTop + 1);
                        LoadBlock(blockFromLeft + 2, blockFromTop - 1);
                        LoadBlock(blockFromLeft - 2, blockFromTop - 1);
                        LoadBlock(blockFromLeft + 2, blockFromTop + 1);

                        LoadBlock(blockFromLeft - 2, blockFromTop + 2);
                        LoadBlock(blockFromLeft + 2, blockFromTop - 2);
                        LoadBlock(blockFromLeft - 2, blockFromTop - 2);
                        LoadBlock(blockFromLeft + 2, blockFromTop + 2);

                        //Set the blockFromLeft/Top for the next logic loop
                        blockFromLeft = (int)Math.Floor(player.FromLeft / world.StartingBlock.Width);
                        blockFromTop = (int)Math.Floor(player.FromTop / world.StartingBlock.Height);
                    }

                    //Lambda version to get all the gameobjects that are outside of the screen.
                    //also tested with parralel but its slower. could become faster if there are enough enemies 
                    //but most of the time it will be slower

                    //also implemented parallel when there are more than 1250 objects (which is where paralel became more efficient in our tests)
                    if (gameObjects.Count() < 1250)
                    {
                        activeObjects = gameObjects.Where(element => element.IsActive(player) == true).ToList();

                        inactiveObjects = gameObjects.Where(element => element.IsActive(player) != true).ToList();
                    }
                    else
                    {
                        activeObjects = gameObjects.AsParallel().Where(element => element.IsActive(player) == true).ToList();

                        inactiveObjects = gameObjects.AsParallel().Where(element => element.IsActive(player) != true).ToList();
                    }
                    

                    //Check if there are objects in the List to apply logic on
                    //Apply the logic to all the gameObjects CURRENTLY in the List.
                    //The new List makes a copy so the original arraylist can be modified
                    foreach (GameObject gameObject in new List<GameObject>(activeObjects))
                    {
                        //Handle player input
                        Player player = gameObject as Player;


                        if (player is Player)
                        {
                            HandlePlayerWeaponControls(player);
                            HandlePlayerMovementControls(player);
                            HandleOtherControls();

                            if (player.IsWalking)
                            {
                                if (player.DeltaForWalkingSound > 1300)
                                {
                                    soundController.PlaySound(player.MoveSound);
                                    player.DeltaForWalkingSound = 0;
                                }
                                player.DeltaForWalkingSound += 200;
                            }
                        }

                        //Call the OnTick on every activeObject
                        gameObject.OnTick(gameObjects);

                        //For every object in this loop, loop trough all objects to check if they are coliding
                        foreach (GameObject gameObjectCheck in new List<GameObject>(activeObjects))
                        {
                            //If the two objects are colliding
                            if (gameObject.IsColliding(gameObjectCheck))
                            {
                                //Do the collision effect on both objects
                                gameObject.CollisionEffect(gameObjectCheck);
                                gameObjectCheck.CollisionEffect(gameObject);
                            }
                        }
                    }
                }

                //All the inactiveObject should skip the Ontick
                foreach (GameObject GameObject in inactiveObjects)
                {
                    GameObject.SkipTick();
                }

                //Handle the tags for each gameobject if there are any tags to handle
                foreach (GameObject gameObjectCheck in new ArrayList(gameObjects))
                {
                    HandleTaggsGameObject(gameObjectCheck);
                }
            }
        }

        private void HandleOtherControls()
        {

            /*
             * The block here enables cheats, they are activated by having certain keys pressed at the same time.
             * Due to keyboard rollover this is not compatible with all keyboards.
             */

            if (IsKeyPressed("192") && !enableCheats) // 192 is the ` key, this is used to activate cheats
            {
                enableCheats = true;

                gameObjects.Add(new TextBox(50, 50, player.FromLeft, player.FromTop - 20, 0, 0, 0, 0, "Done", 1000));
            }
            
            if (IsKeyPressed("Number1") && enableCheats) //Spawn pickup on frame
            {
                gameObjects.Add(new Pickup(15, 17, player.FromLeft, player.FromTop));
                player.IncreaseMaxHealth(5);
                player.IncreaseHealth(5);

                player.IncreaseMaxArmour(5);
                player.IncreaseArmour(5);
            }

            if (IsKeyPressed("Number2") && enableCheats) //KILL
            {
                foreach (GameObject gameObjectCheck in new List<GameObject>(gameObjects))
                {
                    if (gameObjectCheck.IsActive(player) && gameObjectCheck is Enemy)
                    {
                        Enemy gameObjectEnemy = gameObjectCheck as Enemy;
                        gameObjectEnemy.AddLifePoints(-1 * player.GetLevel());
                    }
                }
            }

            if (IsKeyPressed("Number3") && enableCheats) //RAIN
            {
                Random random = new Random();
                for (int i = 0; i < 2; i++) { 
                    gameObjects.Add(
                        new Splatter(
                            random.Next(2, 9),
                            random.Next(2, 9),
                            random.Next((int)player.FromLeft - 400, (int)player.FromLeft + 400) + (random.Next(-200, 200) / 1000),
                            random.Next((int)player.FromTop - 400, (int)player.FromTop + 400) + (random.Next(-200, 200) / 1000)
                        )
                    );
                }
                
            }


            if (IsKeyPressed("Number4") && enableCheats) //EarthQuake
            {
                Random random = new Random();
                foreach (GameObject gameObjectCheck in new List<GameObject>(gameObjects))
                {
                    gameObjectCheck.AddFromLeft(random.Next(-2, 3));
                    gameObjectCheck.AddFromTop(random.Next(-2, 3));
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

            //Players have to have the tag controllable, else they dont listen to the keyboard.

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
            //Key to pauze the screen
            if (IsKeyPressed("Escape") || IsKeyPressed("GamepadMenu"))
            {
                MainPage.Current.GetMenu();

                //Empty all keys that were pressed to cause the game to not register keys that were pressed bevore the pause
                pressedKeys = new HashSet<string>();

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
                MainPage.Current.RemoveMenu();
                paused = false;
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("B") || IsKeyPressed("GamepadB")))
            {
                if (Music)
                {
                    MainPage.Current.MuteMusic();
                    Music = false;
                }
                else
                {
                    MainPage.Current.UnmuteMusic();
                    Music = true;
                }
                Task.Delay(300).Wait();
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("Y") || IsKeyPressed("GamepadY")))
            {
                if (Effects)
                {
                    MainPage.Current.MuteEffect();
                    Effects = false;
                }
                else
                {
                    MainPage.Current.UnmuteEffect();
                    Effects = true;
                }
                Task.Delay(300).Wait();
            }

            if (MainPage.Current.game_over && (IsKeyPressed("Space") || IsKeyPressed("GamepadMenu")))
            {
                CoreApplication.RequestRestartAsync("");
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("Enter") || IsKeyPressed("GamepadMenu")))
            {
                MainPage.Current.GetInfo();
                Task.Delay(300).Wait();
            }
            else if (MainPage.Current.infoScreen && !MainPage.Current.controle && (IsKeyPressed("Space") || IsKeyPressed("GamepadView")))
            {
                MainPage.Current.GetControls();
                Task.Delay(300).Wait();
            }

            else if (MainPage.Current.controle && (IsKeyPressed("Space") || IsKeyPressed("GamepadView")))
            {
                MainPage.Current.RemoveControls();
                Task.Delay(300).Wait();
            }
            else if (MainPage.Current.infoScreen && !MainPage.Current.controle && (IsKeyPressed("Enter") || IsKeyPressed("GamepadMenu")))
            {
                MainPage.Current.RemoveInfo();
                Task.Delay(300).Wait();
            }

            if (MainPage.Current.menuScreen && (IsKeyPressed("X") || IsKeyPressed("GamepadX")))
            {
                MainPage.Current.GetAbout();
                Task.Delay(300).Wait();
            }
            else if (MainPage.Current.aboutScreen && (IsKeyPressed("X") || IsKeyPressed("GamepadX")))
            {
                MainPage.Current.RemoveAbout();
                Task.Delay(300).Wait();
            }

            if (MainPage.Current.activeStartup && pressedKeys.Count() > 0)
            {
                MainPage.Current.Startup();
                Task.Delay(300).Wait();
            }
        }

        /* handleTaggsGameObject */
        /*
         * GameObjects have a list of tags they can contain. 
         * The logic of those tags is done here.
         * This method is called in Logic();
        */
        private void HandleTaggsGameObject(GameObject gameObjectCheck)
        {
            if (gameObjectCheck.HasTag("hit") && gameObjectCheck is Player p1)
            {
                soundController.PlaySound(p1.HitSound);
                p1.RemoveTag("hit");
            }

            if (gameObjectCheck.HasTag("health_low") && gameObjectCheck is Player p2)
            {
                if (p2.DeltaForHealthLowSound > 4000)
                {
                    soundController.PlaySound(p2.HealthLowSound);
                    p2.DeltaForHealthLowSound = 0;
                }
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
                    MainPage.Current.Gameover();
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
                                MainPage.Current.UpdateHighscore();
                                MainPage.Current.killstreak++;
                                MainPage.Current.UpdateKillstreak();
                                if (p4.Kills > 5 * (p4.GetLevel() * (p4.GetLevel() * 0.3)))
                                {
                                    p4.IncreaseLevel();
                                    soundController.PlaySound("Generic_Sounds\\levelup.wav");
                                    MainPage.Current.UpdateLevel();
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
        private async void CreateAllResourcesAsync(CanvasControl sender, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject != null && gameObject.Sprite == null)
                {
                    // Get the sprite form the Texture class
                    gameObject.Sprite = await Texture.GetTextureAsync(sender, gameObject.Location);       
                }
            }
        }

        /* DrawAllBackgroundSprites */
        /*
         * Draws every sprite of which the class is Background.
         * The first argument is the canvasDrawEventArgs (containing data from the draw event).
         * The second is the list of gameObjects.
        */
        private void DrawAllBackgroundSprites(CanvasDrawEventArgs args, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject != null && gameObject.Sprite != null && gameObject is Background)
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

        /* DrawAllOtherSprites */
        /*
         * Draws every sprite of which the class is NOT splatte or background.
         * The first argument is the canvasDrawEventArgs (containing data from the draw event).
         * The second is the list of gameObjects.
        */
        private void DrawAllOtherSprites(CanvasDrawEventArgs args, ArrayList loopList)
        {
            foreach (GameObject gameObject in loopList)
            {
                if (gameObject != null && gameObject.Sprite != null && !(gameObject is Splatter) && !(gameObject is Background))
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


                        try
                        {
                            args.DrawingSession.DrawImage(
                            gameObject.Sprite, gameObject.rectangle);
                        }
                        catch
                        {
                            //if it failes for any reason skip this frame.
                
                        }
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
                    float percentage = 1 + ((player.GetHealth() - player.GetMaxHealth()) / player.GetMaxHealth());

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
                    float percentage = 1 + ((player.GetArmour() - player.GetMaxArmour()) / player.GetMaxArmour());

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
            /*
             * To create the illusion of depth the spirits are drawn in a certain order.
             * The sprites that should be on top of other sprites need to be drawn later.
             * This is why we first draw the Splatter and then all the Non Splatter sprites.
             * After drawing the sprites the UI elements (health and armour bars) are drawn so they are always visable on top.
            */

            //Load the sprite in this canvasControl so they are usable later
            CreateAllResourcesAsync(sender, loopList);

            //Draw the background first so all other sprites are drawn upon it.
            DrawAllBackgroundSprites(args, loopList);

            //Draw the splatter second so all other sprites are drawn upon it. (making the splater apear on the ground)
            DrawAllSplatterSprites(args, loopList);

            //Draw the loaded sprites on the correct location
            DrawAllOtherSprites(args, loopList);

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
            player.SelectNextWeapon();
            while (player.activeWeapon.GetAmmo() <= 0)
            {
                player.SelectNextWeaponDelay = 0;
                player.SelectNextWeapon();
            }
            player.SelectNextWeaponDelay = 1000;
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
            player.SelectPreviousWeapon();
            while (player.activeWeapon.GetAmmo() <= 0)
            {
                player.SelectNextWeaponDelay = 0;
                player.SelectPreviousWeapon();
            }
            player.SelectNextWeaponDelay = 1000;
            MainPage.Current.UpdateWeapon();
        }

        /* KeyDown */
        /* 
         * Add the given key in the pressedKeys collection.
         * The argument is the given key represented as a string.
         */
        public void KeyDown(string virtualKey)
        {
            pressedKeys.Add(virtualKey);
        }


        /* KeyDown */
        /* 
         * Remove the given key in the pressedKeys collection.
         * The argument is the given key represented as a string.
         */
        public void KeyUp(string virtualKey)
        {
            pressedKeys.Remove(virtualKey);
        }

        /* IsKeyPressed */
        /* 
         * Returns wheater the given key exists within the pressedKeys collection.
         * The argument is the given key represented as a string.
         */
        public bool IsKeyPressed(string virtualKey)
        {
            return pressedKeys.Contains(virtualKey);
        }
    }
}
 
 
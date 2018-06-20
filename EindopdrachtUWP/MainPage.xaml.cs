using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Diagnostics;
using System.Threading.Tasks;
using UWPTestApp;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EindopdrachtUWP
{
    public partial class MainPage : Page
    {
        public static MainPage Current;
        Engine engine;
        public bool menuScreen;
        public bool infoScreen;
        public bool aboutScreen;
        public bool activeStartup;
        public bool paused;
        public bool game_over;
        public double currentScore;
        public double killstreak;
        private double critPercentage;
        private double critMultiPercentage;
        private string weapon;

        public MainPage()
        {
            engine = new Engine();

            InitializeComponent();
            Current = this;

            Debug.WriteLine("MainPage");

            Window.Current.CoreWindow.KeyDown += KeyDown;
            Window.Current.CoreWindow.KeyUp += KeyUP;

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            CoreWindow.GetForCurrentThread().PointerCursor = null;
            Window.Current.CoreWindow.PointerCursor = null;

            info.Opacity = 0;
            about.Opacity = 0;
            stats.Opacity = 0;
            statImage.Opacity = 0;
            game_Over_Screen.Opacity = 0;
            game_over = false;
            menuScreen = false;
            infoScreen = false;
            aboutScreen = false;
            activeStartup = true;
            currentScore = 0;
            killstreak = 0;
            paused = true;           
            
            getWeaponStats();
            currentClip.Text = "12/12";
            currentClipRight.Text = "12/12";

            currentLevel.Text = "1";
            currentKills.Text = "0";
            highscore.Text = "0";
            CurrentKillstreak.Text = "0";

            updateHealth();
            updateArmour();

            engine.Run();
        }

        public void gameover()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                game_Over_Screen_Black.Opacity = 0.75;
                game_Over_Screen.Opacity = 1;
                endScoreText.Text = currentScore.ToString();
                game_over = true;
            }
            );
        }

        public void muteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    musicCheck.Opacity = 0;
                    engine.GetSoundController.muteMusic();
                }
            );
        }

        public void unmuteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    musicCheck.Opacity = 1;
                    engine.GetSoundController.unMuteMusic();
                }
            );
        }

        public void muteEffect()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    effectCheck.Opacity = 0;
                    engine.GetSoundController.muteSFX();
                }
            );
        }

        public void unmuteEffect()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    effectCheck.Opacity = 1;
                    engine.GetSoundController.unMuteSFX();
                }
            );
        }

        public void removeMenu()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    menu.Opacity = 0;
                    musicCheck.Opacity = 0;
                    effectCheck.Opacity = 0;
                    black.Opacity = 0;
                    stats.Opacity = 1;
                    statImage.Opacity = 1;
                    menuScreen = false;
                    paused = false;
                }
            );
        }

        public void getMenu()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    menu.Opacity = 1;
                    stats.Opacity = 0;
                    statImage.Opacity = 0;
                    black.Opacity = 0.75;
                    if (engine.music)
                    {
                        musicCheck.Opacity = 1;
                    }
                    if (engine.effects)
                    {
                        effectCheck.Opacity = 1;
                    }
                    menuScreen = true;
                    paused = true;
                }
            );
        }

        public void removeInfo()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    menu.Opacity = 1;
                    info.Opacity = 0;
                    infoScreen = false;
                    menuScreen = true;
                }
            );
        }

        public void getInfo()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                { 
                    info.Opacity = 1;
                    menu.Opacity = 0;
                    infoScreen = true;
                    menuScreen = false;
                }
            );
        }

        public void removeAbout()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                menu.Opacity = 1;
                about.Opacity = 0;
                aboutScreen = false;
                menuScreen = true;
            }
            );
        }

        public void startup()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                Startup.Opacity = 0;
                startup_Image.Opacity = 0;
                activeStartup = false;
                menuScreen = true;
            }
            );
        }

        public void getAbout()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                about.Opacity = 1;
                menu.Opacity = 0;
                aboutScreen = true;
                menuScreen = false;
            }
            );
        }

        public void getControls()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                controls.Opacity = 1;
            }
            );
        }

        public void removeControls()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                controls.Opacity = 0;
            }
            );
        }

        //Every time the stats of a weapon are altered this function is called to display the stats correctly
        public void getWeaponStats()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                
                activeweapon.Text = engine.getPlayer().activeWeapon.name;
                descripton.Text = engine.getPlayer().activeWeapon.description;
                damage.Text = engine.getPlayer().activeWeapon.damage.ToString();
                clipSize.Text = engine.getPlayer().activeWeapon.clipMax.ToString();
                clipAmount.Text = engine.getPlayer().activeWeapon.clipAmount.ToString();
                critPercentage = engine.getPlayer().activeWeapon.critChance * 100;
                critChance.Text = critPercentage.ToString() + "%";
                critMultiPercentage = engine.getPlayer().activeWeapon.critMultiplier * 100;
                critMulti.Text = critMultiPercentage.ToString() + "%";
            });
        }

        //Every time a zombie gets killed this function is called. It adds the current player level to the score and displays the score and amount of kills
        public void updateHighscore()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                currentScore += engine.getPlayer().GetLevel();
                highscore.Text = currentScore.ToString();
                currentKills.Text = engine.getPlayer().Kills.ToString();
            });
        }

        public void updateKillstreak()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                CurrentKillstreak.Text = killstreak.ToString();
            });
        }

        //Every time the player levels this function is called to increase the displayed level
        public void UpdateLevel()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                currentLevel.Text = engine.getPlayer().GetLevel().ToString();
            });
        }

        //Every time the health of the player is altered this function is called to display the correct amount of healthpoints
        public void updateHealth()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
               health.Text = engine.getPlayer().getHealth().ToString() + "/" + engine.getPlayer().getMaxHealth().ToString();
            });
        }

        //Every time the armour of the player is altered this function is called to display the correct amount of armourpoints
        public void updateArmour()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                armour.Text = engine.getPlayer().getArmour().ToString() + "/" + engine.getPlayer().getMaxArmour().ToString();
            });
        }

        //Every time the amount of a clip is altered this function is called to display the correct amount
        public void UpdateCurrentClip()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                currentClip.Text = engine.getPlayer().activeWeapon.currentClip.ToString() + "/" + engine.getPlayer().activeWeapon.clipMax.ToString();
                currentClipRight.Text = engine.getPlayer().activeWeapon.currentClip.ToString() + "/" + engine.getPlayer().activeWeapon.clipMax.ToString();
            });
        }

        //Every time you switch a weapon this function is called to display the correct weapon and place the selector at the right weapon
        public void UpdateWeapon()
        {
            weapon = engine.getPlayer().activeWeapon.name;
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                switch (weapon)
                {
                    case "Dessert Beagle":
                        selected.Margin = new Thickness(-55, -540, 0, 0);
                        currentClip.Margin = new Thickness(-30, -490, 0, 0);
                        break;
                    case "KA74":
                        selected.Margin = new Thickness(-55, -420, 0, 0);
                        currentClip.Margin = new Thickness(-30, -370, 0, 0);
                        break;
                    case "Knettergun":
                        selected.Margin = new Thickness(-55, -300, 0, 0);
                        currentClip.Margin = new Thickness(-30, -250, 0, 0);
                        break;
                    case "UWP":
                        selected.Margin = new Thickness(-55, -180, 0, 0);
                        currentClip.Margin = new Thickness(-30, -110, 0, 0);
                        break;
                    case "Flame Thrower":
                        selected.Margin = new Thickness(-55, -60, 0, 0);
                        currentClip.Margin = new Thickness(-30, -10, 0, 0);
                        break;
                    case "VLEKKannon":
                        selected.Margin = new Thickness(-55, 60, 0, 0);
                        currentClip.Margin = new Thickness(-30, 110, 0, 0);
                        break;
                    case "Bullet Bill":
                        selected.Margin = new Thickness(-55, 180, 0, 0);
                        currentClip.Margin = new Thickness(-30, 230, 0, 0);
                        break;
                    case "Arriva Gun":
                        selected.Margin = new Thickness(-55, 300, 0, 0);
                        currentClip.Margin = new Thickness(-30, 350, 0, 0);
                        break;
                    case "Batarang":
                        selected.Margin = new Thickness(-55, 420, 0, 0);
                        currentClip.Margin = new Thickness(-30, 470, 0, 0);
                        break;
                    case "Homers Bullets":
                        selected.Margin = new Thickness(-55, 540, 0, 0);
                        currentClip.Margin = new Thickness(-30, 580, 0, 0);
                        break;
                }
            });
            getWeaponStats();
            UpdateCurrentClip();
        }

        //When the amount of ammo of a weapon is not 0 this function displays the weapon
        public void weaponAmmo(string weapon)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                switch (weapon)
                {
                    case "KA74":
                        empty_ka74.Opacity = 0;
                        break;
                    case "Knettergun":
                        empty_knettergun.Opacity = 0;
                        break;
                    case "UWP":
                        empty_uwp.Opacity = 0;
                        break;
                    case "Flame Thrower":
                        empty_flamethrower.Opacity = 0;
                        break;
                    case "VLEKKannon":
                        empty_vlekkannon.Opacity = 0;
                        break;
                    case "Bullet Bill":
                        empty_bullet_bill.Opacity = 0;
                        break;
                    case "Arriva Gun":
                        empty_arriva_gun.Opacity = 0;
                        break;
                    case "Batarang":
                        empty_batarang.Opacity = 0;
                        break;
                    case "Homers Bullets":
                        empty_homers_bullets.Opacity = 0;
                        break;
                    default:
                        break;
                }
            });
        }

        //When the amount of ammo of a weapon is 0 this function displays the weapon
        public void weaponEmpty(string weapon)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                switch (weapon)
                {
                    case "KA74":
                        empty_ka74.Opacity = 1;
                        break;
                    case "Knettergun":
                        empty_knettergun.Opacity = 1;
                        break;
                    case "UWP":
                        empty_uwp.Opacity = 1;
                        break;
                    case "Flame Thrower":
                        empty_flamethrower.Opacity = 1;
                        break;
                    case "VLEKKannon":
                        empty_vlekkannon.Opacity = 1;
                        break;
                    case "Bullet Bill":
                        empty_bullet_bill.Opacity = 1;
                        break;
                    case "Arriva Gun":
                        empty_arriva_gun.Opacity = 1;
                        break;
                    case "Batarang":
                        empty_batarang.Opacity = 1;
                        break;
                    case "Homers Bullets":
                        empty_homers_bullets.Opacity = 1;
                        break;
                    default:
                        break;
                }
            });
        }

        public void enableSecondSpawner()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   secondSpawner.Opacity = 1;

               });
        }

        public void enableThirdSpawner()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   thirdSpawner.Opacity = 1;

               });
        }

        void KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            engine.KeyDown(args.VirtualKey.ToString());
        }

        void KeyUP(CoreWindow sender, KeyEventArgs args)
        {
            engine.KeyUp(args.VirtualKey.ToString());
        }

        private void Grid_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            engine.DrawEvent(sender, args);

            //Examples to test drawing
            //args.DrawingSession.DrawEllipse(155, 115, 80, 30, Colors.Black, 3);
            //args.DrawingSession.DrawText("Hello, world!", 100, 100, Colors.Yellow);
        }
    }
}
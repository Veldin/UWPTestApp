using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Diagnostics;
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
        public bool controle;
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

            Window.Current.CoreWindow.KeyDown += KeyDown;
            Window.Current.CoreWindow.KeyUp += KeyUp;

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            CoreWindow.GetForCurrentThread().PointerCursor = null;
            Window.Current.CoreWindow.PointerCursor = null;

            info.Opacity = 0;
            infoText.Opacity = 0;
            about.Opacity = 0;
            stats.Opacity = 0;
            statImage.Opacity = 1;
            game_Over_Screen.Opacity = 0;
            viewbox.Opacity = 0;
            game_over = false;
            menuScreen = false;
            infoScreen = false;
            controle = false;
            aboutScreen = false;
            activeStartup = true;
            currentScore = 0;
            killstreak = 0;
            paused = true;           
            
            GetWeaponStats();
            currentClip.Text = "12/12";
            currentClipRight.Text = "12/12";

            currentLevel.Text = "1";
            currentKills.Text = "0";
            highscore.Text = "0";
            CurrentKillstreak.Text = "0";

            UpdateHealth();
            UpdateArmour();

            engine.Run();
        }

        public void Gameover()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                game_Over_Screen_Black.Opacity = 0.75;
                game_Over_Screen.Opacity = 1;
                endScoreText.Text = currentScore.ToString();
                viewbox.Opacity = 0;
                game_over = true;
            }
            );
        }

        public void MuteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    musicCheck.Opacity = 0;
                    engine.GetSoundController.MuteMusic();
                }
            );
        }

        public void UnmuteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    musicCheck.Opacity = 1;
                    engine.GetSoundController.UnMuteMusic();
                }
            );
        }

        public void MuteEffect()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    effectCheck.Opacity = 0;
                    engine.GetSoundController.MuteSFX();
                }
            );
        }

        public void UnmuteEffect()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    effectCheck.Opacity = 1;
                    engine.GetSoundController.UnMuteSFX();
                }
            );
        }

        public void RemoveMenu()
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

        public void GetMenu()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    menu.Opacity = 1;
                    stats.Opacity = 0;
                    statImage.Opacity = 1;
                    black.Opacity = 0.75;
                    if (engine.Music)
                    {
                        musicCheck.Opacity = 1;
                    }
                    if (engine.Effects)
                    {
                        effectCheck.Opacity = 1;
                    }
                    menuScreen = true;
                    paused = true;
                }
            );
        }

        public void RemoveInfo()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    menu.Opacity = 1;
                    info.Opacity = 0;
                    infoText.Opacity = 0;
                    infoScreen = false;
                    menuScreen = true;
                }
            );
        }

        public void GetInfo()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                { 
                    info.Opacity = 1;
                    infoText.Opacity = 1;
                    menu.Opacity = 0;
                    infoScreen = true;
                    menuScreen = false;
                }
            );
        }

        public void RemoveAbout()
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

        public void Startup()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                startup.Opacity = 0;
                startup_Image.Opacity = 0;
                viewbox.Opacity = 1;
                activeStartup = false;
                menuScreen = true;
            }
            );
        }

        public void GetAbout()
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

        public void GetControls()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                controls.Opacity = 1;
                controle = true;
            }
            );
        }

        public void RemoveControls()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                controls.Opacity = 0;
                controle = false;
            }
            );
        }

        //Every time the stats of a weapon are altered this function is called to display the stats correctly
        public void GetWeaponStats()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                
                activeweapon.Text = engine.GetPlayer().activeWeapon.name;
                descripton.Text = engine.GetPlayer().activeWeapon.description;
                damage.Text = engine.GetPlayer().activeWeapon.damage.ToString();
                clipSize.Text = engine.GetPlayer().activeWeapon.clipMax.ToString();
                clipAmount.Text = engine.GetPlayer().activeWeapon.clipAmount.ToString();
                critPercentage = engine.GetPlayer().activeWeapon.critChance * 100;
                critChance.Text = critPercentage.ToString() + "%";
                critMultiPercentage = engine.GetPlayer().activeWeapon.critMultiplier * 100;
                critMulti.Text = critMultiPercentage.ToString() + "%";
            });
        }

        //Every time a zombie gets killed this function is called. It adds the current player level to the score and displays the score and amount of kills
        public void UpdateHighscore()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                currentScore += engine.GetPlayer().GetLevel();
                highscore.Text = currentScore.ToString();
                currentKills.Text = engine.GetPlayer().Kills.ToString();
            });
        }

        public void UpdateKillstreak()
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
                currentLevel.Text = engine.GetPlayer().GetLevel().ToString();
            });
        }

        //Every time the health of the player is altered this function is called to display the correct amount of healthpoints
        public void UpdateHealth()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
               health.Text = engine.GetPlayer().GetHealth().ToString() + "/" + engine.GetPlayer().GetMaxHealth().ToString();
            });
        }

        //Every time the armour of the player is altered this function is called to display the correct amount of armourpoints
        public void UpdateArmour()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                armour.Text = engine.GetPlayer().GetArmour().ToString() + "/" + engine.GetPlayer().GetMaxArmour().ToString();
            });
        }

        //Every time the amount of a clip is altered this function is called to display the correct amount
        public void UpdateCurrentClip()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                currentClip.Text = engine.GetPlayer().activeWeapon.currentClip.ToString() + "/" + engine.GetPlayer().activeWeapon.clipMax.ToString();
                currentClipRight.Text = engine.GetPlayer().activeWeapon.currentClip.ToString() + "/" + engine.GetPlayer().activeWeapon.clipMax.ToString();
            });
        }

        //Every time you switch a weapon this function is called to display the correct weapon and place the selector at the right weapon
        public void UpdateWeapon()
        {
            weapon = engine.GetPlayer().activeWeapon.name;
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
            GetWeaponStats();
            UpdateCurrentClip();
        }

        //When the amount of ammo of a weapon is not 0 this function displays the weapon
        public void WeaponAmmo(string weapon)
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
        public void WeaponEmpty(string weapon)
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

        new void KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            engine.KeyDown(args.VirtualKey.ToString());
        }

        new void KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            engine.KeyUp(args.VirtualKey.ToString());
        }

        private void Grid_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            engine.DrawEvent(sender, args);
        }
    }
}
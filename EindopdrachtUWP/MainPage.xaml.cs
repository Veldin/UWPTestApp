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
        public bool paused;
        public bool game_over;
        public int weapon;
        

        public MainPage()
        {
            engine = new Engine();

            this.InitializeComponent();
            Current = this;

            Debug.WriteLine("MainPage");

            Window.Current.CoreWindow.KeyDown += KeyDown;
            Window.Current.CoreWindow.KeyUp += KeyUP;

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            info.Opacity = 0;
            about.Opacity = 0;
            stats.Opacity = 0;
            statImage.Opacity = 0;
            game_Over_Screen.Opacity = 0;
            game_over = false;
            menuScreen = true;
            infoScreen = false;
            aboutScreen = false;
            paused = true;           

            weapon = 1;
            getWeaponStats();

            engine.Run();
        }
        public void gameover()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                game_Over_Screen.Opacity = 0.75;
                game_Over_Screen_image.Opacity = 1;
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
                }
            );
        }

        public void unmuteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    musicCheck.Opacity = 1;
                }
            );
        }

        public void muteEffect()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    effectCheck.Opacity = 0;
                }
            );
        }

        public void unmuteEffect()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    effectCheck.Opacity = 1;
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

        public void getWeaponStats()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                activeweapon.Text = engine.getPlayer().activeWeapon.name;
                descripton.Text = engine.getPlayer().activeWeapon.description;
                damage.Text = engine.getPlayer().activeWeapon.damage.ToString();
                //currentClip.Text = engine.getPlayer().activeWeapon.currentClip.ToString();
                clipSize.Text = engine.getPlayer().activeWeapon.clipMax.ToString();
                clipAmount.Text = engine.getPlayer().activeWeapon.clipAmount.ToString();
                critChance.Text = engine.getPlayer().activeWeapon.critChance.ToString();
                critMulti.Text = engine.getPlayer().activeWeapon.critMultiplier.ToString();
            });
        }

        public void nextWeapon()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                switch (weapon)
                {
                    case 1:
                        selected.Margin = new Thickness(-55, -540, 0, 0);
                        activeweapon.Text = "Dessert Eagle";
                        break;
                    case 2:
                        selected.Margin = new Thickness(-55, -420, 0, 0);
                        activeweapon.Text = "KA74";
                        break;
                    case 3:
                        selected.Margin = new Thickness(-55, -300, 0, 0);
                        activeweapon.Text = "Knetter Gun";
                        break;
                    case 4:
                        selected.Margin = new Thickness(-55, -180, 0, 0);
                        activeweapon.Text = "UWP";
                        break;
                    case 5:
                        selected.Margin = new Thickness(-55, -60, 0, 0);
                        activeweapon.Text = "Flame Thrower";
                        break;
                    case 6:
                        selected.Margin = new Thickness(-55, 60, 0, 0);
                        activeweapon.Text = "VLEKKannon";
                        break;
                    case 7:
                        selected.Margin = new Thickness(-55, 180, 0, 0);
                        activeweapon.Text = "Bullet Bill";
                        break;
                    case 8:
                        selected.Margin = new Thickness(-55, 300, 0, 0);
                        activeweapon.Text = "Arriva Gun";
                        break;
                    case 9:
                        selected.Margin = new Thickness(-55, 420, 0, 0);
                        activeweapon.Text = "Batarang";
                        break;
                    case 10:
                        selected.Margin = new Thickness(-55, 540, 0, 0);
                        activeweapon.Text = "Homer's Bullets";
                        weapon = 0;
                        break;
                }
            });
            weapon++;
            getWeaponStats();
        }

        public void previousWeapon()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                switch (weapon)
                {
                    case 1:
                        selected.Margin = new Thickness(-55, -540, 0, 0);
                        activeweapon.Text = "Dessert Eagle";
                        weapon = 11;
                        break;
                    case 2:
                        selected.Margin = new Thickness(-55, -420, 0, 0);
                        activeweapon.Text = "KA74";
                        break;
                    case 3:
                        selected.Margin = new Thickness(-55, -300, 0, 0);
                        activeweapon.Text = "Knetter Gun";
                        break;
                    case 4:
                        selected.Margin = new Thickness(-55, -180, 0, 0);
                        activeweapon.Text = "UWP";
                        break;
                    case 5:
                        selected.Margin = new Thickness(-55, -60, 0, 0);
                        activeweapon.Text = "Flame Thrower";
                        break;
                    case 6:
                        selected.Margin = new Thickness(-55, 60, 0, 0);
                        activeweapon.Text = "VLEKKannon";
                        break;
                    case 7:
                        selected.Margin = new Thickness(-55, 180, 0, 0);
                        activeweapon.Text = "Bullet Bill";
                        break;
                    case 8:
                        selected.Margin = new Thickness(-55, 300, 0, 0);
                        activeweapon.Text = "Arriva Gun";
                        break;
                    case 9:
                        selected.Margin = new Thickness(-55, 420, 0, 0);
                        activeweapon.Text = "Batarang";
                        break;
                    case 10:
                        selected.Margin = new Thickness(-55, 540, 0, 0);
                        activeweapon.Text = "Homer's Bullets";
                        break;
                }
            });
            weapon--;
            getWeaponStats();
        }

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
        void KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            engine.KeyDown(args.VirtualKey.ToString());
        }

        void KeyUP(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
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
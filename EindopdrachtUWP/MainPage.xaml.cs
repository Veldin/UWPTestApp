using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Diagnostics;
using UWPTestApp;
using Windows.UI.Core;
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

        public MainPage()
        {
            engine = new Engine();

            this.InitializeComponent();
            Current = this;

            Debug.WriteLine("MainPage");

            Window.Current.CoreWindow.KeyDown += KeyDown;
            Window.Current.CoreWindow.KeyUp += KeyUP;

            info.Visibility = Visibility.Collapsed;
            about.Visibility = Visibility.Collapsed;
            menuScreen = true;
            infoScreen = false;
            aboutScreen = false;
            paused = true;

            engine.Run();
        }

        public void muteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    musicCheck.Visibility = Visibility.Collapsed;
                }
            );
        }

        public void unmuteMusic()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    musicCheck.Visibility = Visibility.Visible;
                }
            );
        }

        public void muteEffect()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    effectCheck.Visibility = Visibility.Collapsed;
                }
            );
        }

        public void unmuteEffect()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    effectCheck.Visibility = Visibility.Visible;
                }
            );
        }

        public void removeMenu()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    menu.Visibility = Visibility.Collapsed;
                    musicCheck.Visibility = Visibility.Collapsed;
                    effectCheck.Visibility = Visibility.Collapsed;
                    black.Visibility = Visibility.Collapsed;
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
                    menu.Visibility = Visibility.Visible;
                    black.Visibility = Visibility.Visible;
                    if (engine.music)
                    {
                        musicCheck.Visibility = Visibility.Visible;
                    }
                    if (engine.effects)
                    {
                        effectCheck.Visibility = Visibility.Visible;
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
                    menu.Visibility = Visibility.Visible;
                    info.Visibility = Visibility.Collapsed;
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
                    info.Visibility = Visibility.Visible;
                    menu.Visibility = Visibility.Collapsed;
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
                menu.Visibility = Visibility.Visible;
                about.Visibility = Visibility.Collapsed;
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
                about.Visibility = Visibility.Visible;
                menu.Visibility = Visibility.Collapsed;
                aboutScreen = true;
                menuScreen = false;
            }
            );
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
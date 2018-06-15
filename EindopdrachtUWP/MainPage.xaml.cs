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

            info.Opacity = 0;
            about.Opacity = 0;
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
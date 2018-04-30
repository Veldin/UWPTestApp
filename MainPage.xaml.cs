using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Engine engine;

        public MainPage()
        {
            engine = new Engine();


            this.InitializeComponent();

            Debug.WriteLine("MainPage");

            engine.Run();
        }

        private void Grid_OnLoaded(object sender, RoutedEventArgs e)

        {
            Debug.WriteLine("Grid_OnLoaded");
     

        }

        private void Grid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            //handling code here
            Debug.WriteLine("keyUp");
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //handling code here
            Debug.WriteLine("keyDown");
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

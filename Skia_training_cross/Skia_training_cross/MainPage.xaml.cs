using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Xamarin.Forms;


namespace Skia_training_cross
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    public partial class MainPage : ContentPage
    {
        private SKPaint blueFillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColor.Parse("#4daef7")
        };
        public MainPage()
        {
            InitializeComponent();
            /*this.BindingContext = this;*/
        }
        
        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            
            canvas.Clear(SKColors.White);

            int width = e.Info.Width;
            int height = e.Info.Height;
            
            //Set transforms
            canvas.Translate(width / 2, height / 2);

            canvas.DrawRect(0,0,100,100,blueFillPaint);
        }
        
        public class ItemPO
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }

        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
           
        }

    }
}
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms.Shapes;

namespace Skia_training_cross
{
    public partial class MainPage : ContentPage
    {
        SKPaint blackFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

        private SKPaint whiteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        private SKPaint whiteFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.White
        };

        private SKPaint brownFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.SandyBrown
        };

        private SKPaint blackStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 20,
            StrokeCap = SKStrokeCap.Round
        };

        private SKPaint grayFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            
            Color = SKColors.Gray
        };

        private SKPaint backgroundFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill
        };

        private SKPath pandaNosePath = new SKPath();
        private SKPath pandaMouthPath = new SKPath();
        private SKPath pandaHeadPath = new SKPath();
        
        public MainPage()
        {
            InitializeComponent();
            
            // Make panda nose path
            pandaNosePath.MoveTo(0,-140);
            pandaNosePath.LineTo(10,-140);
            pandaNosePath.LineTo(4,-138);
            pandaNosePath.LineTo(0,-130);
            pandaNosePath.Close();

            // Make panda mouth path
            pandaNosePath.MoveTo(0,-130);
            pandaNosePath.LineTo(0,-125);
            pandaNosePath.LineTo(7,-122);
            pandaNosePath.LineTo(14,-125);

            // Create shader
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("Skia_training_cross.bamboo_background.jpg"))
            using (SKManagedStream skStream = new SKManagedStream(stream))
            using (SKBitmap bitmap = SKBitmap.Decode(skStream))
            using (SKShader shader = SKShader.CreateBitmap(bitmap,SKShaderTileMode.Mirror,SKShaderTileMode.Mirror))
            {
                backgroundFillPaint.Shader = shader;
            }
            
            Device.StartTimer(TimeSpan.FromSeconds(1f/60), () =>
            {
                canvasView.InvalidateSurface();
                return true;
            });
        }

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            
            canvas.DrawPaint(backgroundFillPaint);

            int width = e.Info.Width;
            int height = e.Info.Height;

            // Set transforms
            canvas.Translate(width / 2, height / 2);
            canvas.Scale((Math.Min(width/210f, height/520f)));
            
            // Get DateTime
            DateTime dateTime = DateTime.Now;
            
            // Draw ears
            canvas.Save();
            canvas.DrawCircle(50,-223,26,blackFillPaint);
            canvas.DrawCircle(-50,-223,26,blackFillPaint);
            canvas.Restore();

            // Draw legs
            canvas.DrawOval(40,100, 25, 48,blackFillPaint);
            canvas.DrawOval(-40,100, 25, 48,blackFillPaint);
            
            // Draw arms
            canvas.Save();
            canvas.RotateDegrees(30);
            canvas.DrawOval(52,-70, 60, 28,blackFillPaint);
            canvas.RotateDegrees(-60);
            canvas.DrawOval(-52,-70, 60, 28,blackFillPaint);
            
            // Draw body
            canvas.Restore();
            canvas.DrawOval(0,-10, 90, 100,blackFillPaint);
            canvas.DrawOval(0,0, 85, 65,whiteFillPaint);
            
            // Draw head 
            canvas.DrawOval(0, -155, 80, 70, blackFillPaint);
            canvas.DrawOval(0,-155,75,65, whiteFillPaint);

            // Draw nose, mouth, eyes and whiskers
            for (int i = 0; i < 2; i++)
            {
                canvas.Save();
                canvas.Scale(2*i - 1,1);

                // Draw nose
                canvas.DrawPath(pandaNosePath, blackFillPaint);

                // Draw mouth
                canvas.DrawPath(pandaMouthPath, blackStrokePaint);
                
                // Draw eyes
                canvas.DrawOval(30,-172, 20, 25, blackFillPaint);
                canvas.DrawCircle(28,-175, 11, whiteFillPaint);
                canvas.DrawCircle(27, -175, 7, blackFillPaint);

                // Draw whiskers
                canvas.DrawLine(10,-130,45,-110,blackFillPaint);
                canvas.DrawLine(10,-135,45,-125,blackFillPaint);
                canvas.DrawLine(10,-140,45,-135,blackFillPaint);
                canvas.Restore();
                
            }
            
            //------------------------------Animation training
            
            // Move head
            float t = (float) Math.Sin((dateTime.Second % 2 + dateTime.Millisecond / 1000.0) * Math.PI);
            pandaHeadPath.Reset();
            pandaHeadPath.MoveTo(0,-120);
            pandaHeadPath.LineTo(0,-140);
            
            // Draw head
            canvas.DrawPath(pandaHeadPath,whiteFillPaint);
        }
    }
}
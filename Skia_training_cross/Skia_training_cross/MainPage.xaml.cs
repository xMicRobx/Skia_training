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
            Color = SKColor.Parse("c2d9ff")
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

        private SKPath pandaEarPath = new SKPath();
        private SKPath pandaEyePath = new SKPath();
        private SKPath pandaPupilPath = new SKPath();
        private SKPath pandaTailPath = new SKPath();

        private SKPath hourHandPath =
            SKPath.ParseSvgPathData(
                "M 0 -60 C 0 -30 20 -30 5 -20 L 5 0 C 5 7.5 -5 7.5 -5 0 L -5 -20 C -20 -30 0 -30 0 -60");

        private SKPath minuteHandPath = SKPath.ParseSvgPathData(
            "M 0 -80 C 0 -75 0 -70 2.5 -60 L 2.5 0 C 2.5 5 -2.5 5 -2.5 0 L -2.5 -60 C 0 -70 0 -75 0 -80");
        public MainPage()
        {
            InitializeComponent();
            
            // Make panda ear path
            pandaEarPath.MoveTo(0,0);
            pandaEarPath.LineTo(0,75);
            pandaEarPath.LineTo(100,75);
            pandaEarPath.Close();
            
            // Make panda eye path
            pandaEyePath.MoveTo(0,0);
            pandaEyePath.ArcTo(50,50,0,SKPathArcSize.Small, SKPathDirection.Clockwise, 50, 0);
            pandaEyePath.ArcTo(50,50,0,SKPathArcSize.Small, SKPathDirection.Clockwise, 0, 0);
            pandaEyePath.Close();
            
            // Make panda pupil path
            pandaPupilPath.MoveTo(25,-5);
            pandaPupilPath.ArcTo(6,6,0,SKPathArcSize.Small, SKPathDirection.Clockwise, 25,5);
            pandaPupilPath.ArcTo(6,6,0,SKPathArcSize.Small, SKPathDirection.Clockwise, 25,-5);
            pandaPupilPath.Close();
            
            // Make panda tail path
            pandaTailPath.MoveTo(0,100);
            pandaTailPath.CubicTo(50,200,0,250,-50,200);
            
            // Create shader
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("Skia_training_cross.background.jpg"))
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
            
            // Head 
            canvas.DrawCircle(0,-160,75,blackFillPaint);
            
            // Draw ears and eyes
            for (int i = 0; i < 2; i++)
            {
                canvas.Save();
                canvas.Scale(2*i - 1,1);

                canvas.Save();
                canvas.Translate(-65,-255);
                canvas.DrawPath(pandaEarPath, blackFillPaint);
                canvas.Restore();

                canvas.Save();
                canvas.Translate(10,-170);
                canvas.DrawPath(pandaEyePath, brownFillPaint);
                canvas.DrawPath(pandaPupilPath, blackFillPaint);
                canvas.Restore();
                
                // Draw whiskers
                canvas.DrawLine(10,-120,100,-100,whiteStrokePaint);
                canvas.DrawLine(10,-125,100,-120,whiteStrokePaint);
                canvas.DrawLine(10,-130,100,-140,whiteStrokePaint);
                canvas.DrawLine(10,-135,100,-160,whiteStrokePaint);
                
                canvas.Restore();
            }
            
            // Move tail 
            float t = (float) Math.Sin((dateTime.Second % 2 + dateTime.Millisecond / 1000.0) * Math.PI);
            pandaTailPath.Reset();
            pandaTailPath.MoveTo(0,100);
            SKPoint point1 = new SKPoint(-50 * t, 200);
            SKPoint point2 = new SKPoint(0, 250 - Math.Abs(5 * t));
            SKPoint point3 = new SKPoint(50*t, 250 - Math.Abs(75 * t));
            pandaTailPath.CubicTo(point1,point2,point3);
            
            // Draw tail
            canvas.DrawPath(pandaTailPath, blackStrokePaint);
            
            // Clock.background
            canvas.DrawCircle(0,0,100,blackFillPaint);
            
            // Hour and minute marks
            for (int angle = 0; angle < 360; angle += 6)
            {
                canvas.DrawCircle(0, -90, angle % 30 == 0 ? 4 : 2, whiteFillPaint);
                canvas.RotateDegrees(6);
            }

            // Hour hand
            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            canvas.DrawPath(hourHandPath, grayFillPaint);
            canvas.DrawPath(hourHandPath, whiteStrokePaint);
            canvas.Restore();
            
            // Minute hand
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            canvas.DrawPath(minuteHandPath, grayFillPaint);
            canvas.DrawPath(minuteHandPath, whiteStrokePaint);
            canvas.Restore();
            
            // Second hand
            canvas.Save();
            float seconds = dateTime.Second + dateTime.Millisecond / 1000f;
            canvas.RotateDegrees(6 * seconds);
            canvas.DrawLine(0,10,0,-80,whiteStrokePaint);
            canvas.Restore();
        }
    }
}
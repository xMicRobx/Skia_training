﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Shapes;

namespace Skia_training_cross
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeatsPage : ContentPage
    {
        public SeatsPage(Choice choice)
        {
            InitializeComponent();
            SelectedChoice = choice;
            Init();
            this.BindingContext = this;
        }
        public Choice SelectedChoice { get; set; }
        private Dictionary<int, int> data = new Dictionary<int, int>();
        private SKPaint availablePaint = new SKPaint() { Style = SKPaintStyle.Stroke, Color = SKColor.Parse("#343352") };
        
        private SKPaint reservedPaint = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, Color = SKColor.Parse("#343352") };
        
        private SKPaint yourSeatPaint = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, Color = SKColor.Parse("#9747FF") };
        
        private SKPaint textPaint = new SKPaint() { TextSize = 40, Color = SKColor.Parse("#343352") };

        private void Init()
        {
            var rand = new Random();
            for (int i = 0; i < 120; i++)
            {
                data.Add(i, rand.Next(0,2));
            }
        }
        private void SKCanvasView_OnPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var x = 60;
            var y = 60;
            var row = -1;
            var columns = 14;
            var itemHeight = 40;
            var itemWidth = 40;
            var margin = 20;
            var cornerRadius = 4;
            var items = 0;

            for (int i = 0; i < data.Count; i++)
            {
                if (items == 0)
                {
                    row += 1;
                    items = GetColumn(row);
                    var count = (columns - items) / 2;
                    var offset = (count * itemWidth) + (count * margin);
                    x = 60 + offset;
                    y = (itemWidth + ((itemWidth + margin) * row));
                }
                else
                {
                    x += itemHeight + margin;
                }

                var seatColorIndex = data[i];
                
                if (SelectedChoice.Seats.Any(z => z == i))
                    seatColorIndex = 2;
                
                canvas.DrawRoundRect(new SKRoundRect(new SKRect(x,y,x + itemHeight,y + itemWidth), cornerRadius),GetColor(seatColorIndex));

                items -= 1;

                if (items == 0)
                {
                    canvas.DrawText($"{row + 1}", 0, y + margin + (itemHeight/2), textPaint);
                }
            }
        }

        private SKPaint GetColor(int seatColor)
        {
            switch (seatColor)
            {
                case 0:
                default:
                    return availablePaint;
                case 1:
                    return reservedPaint;
                case 2:
                    return yourSeatPaint;
            }
        }

        private int GetColumn(int row)
        {
            switch (row)
            {
                case 0:
                    return 8;
                case 1:
                case 9:
                    return 10;
                case 2: 
                case 3:
                case 8:
                    return 12;
                default:
                    return 14;
            }
        }
    }
}
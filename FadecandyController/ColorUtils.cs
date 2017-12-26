using System;
using System.Drawing;

namespace FadecandyController
{
    public class PixelColor
    {
        #region Private Fields

        private const double scale = 240.0;

        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale
        private double hue = 1.0;

        private double luminosity = 1.0;
        private double saturation = 1.0;

        #endregion Private Fields

        #region Public Constructors

        public PixelColor()
        {
        }

        public PixelColor(Color color)
        {
            SetRgb(color.R, color.G, color.B);
        }

        public PixelColor(int red, int green, int blue)
        {
            SetRgb(red, green, blue);
        }

        public PixelColor(double hue, double saturation, double luminosity)
        {
            Hue = hue;
            Saturation = saturation;
            Luminosity = luminosity;
        }

        public PixelColor(int wheelPos)
        {
            var pos = 255 - wheelPos;
            if (pos < 85)
            {
                SetRgb(255 - pos * 3, 0, pos * 3);
            }
            else if (pos < 170)
            {
                pos -= 85;
                SetRgb(0, pos * 3, 255 - pos * 3);
            }
            else
            {
                pos -= 170;
                SetRgb(pos * 3, 255 - pos * 3, 0);
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public static PixelColor Black => new PixelColor(0, 0, 0);

        public static PixelColor White => new PixelColor(255, 255, 255);

        public static PixelColor Red => new PixelColor(255, 0, 0);

        public static PixelColor Green => new PixelColor(0, 255, 0);
        
        public static PixelColor Blue => new PixelColor(0, 0, 255);
        
        public static PixelColor Orange => new PixelColor(255,165,0);
        
        public static PixelColor Yellow => new PixelColor(255,255,0);
        

        public double Hue
        {
            get => hue * scale;
            set => hue = CheckRange(value / scale);
        }

        public double Luminosity
        {
            get => luminosity * scale;
            set => luminosity = CheckRange(value / scale);
        }

        public double Saturation
        {
            get => saturation * scale;
            set => saturation = CheckRange(value / scale);
        }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator Color(PixelColor pixelColor)
        {
            double r = 0, g = 0, b = 0;
            if (pixelColor.luminosity != 0)
            {
                if (pixelColor.saturation == 0)
                    r = g = b = pixelColor.luminosity;
                else
                {
                    var temp2 = GetTemp2(pixelColor);
                    var temp1 = 2.0 * pixelColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, pixelColor.hue + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, pixelColor.hue);
                    b = GetColorComponent(temp1, temp2, pixelColor.hue - 1.0 / 3.0);
                }
            }
            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        public static implicit operator PixelColor(Color color)
        {
            var hslColor = new PixelColor
            {
                hue = color.GetHue() / 360.0,
                luminosity = color.GetBrightness(),
                saturation = color.GetSaturation()
            };
            // we store hue as 0-1 as opposed to 0-360
            return hslColor;
        }

        public void SetRgb(int red, int green, int blue)
        {
            var hslColor = (PixelColor)Color.FromArgb(red, green, blue);
            hue = hslColor.hue;
            saturation = hslColor.saturation;
            luminosity = hslColor.luminosity;
        }

        public Pixel ToRgbPixel()
        {
            var color = (Color)this;
            return new Pixel(color.R, color.G, color.B);
        }

        public string ToRgbString()
        {
            var color = (Color)this;
            return string.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
        }

        public override string ToString()
        {
            return string.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
        }

        #endregion Public Methods

        #region Private Methods

        private static double CheckRange(double value)
        {
            if (value < 0.0)
                value = 0.0;
            else if (value > 1.0)
                value = 1.0;
            return value;
        }
        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            if (temp3 < 0.5)
                return temp2;
            if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            return temp1;
        }

        private static double GetTemp2(PixelColor pixelColor)
        {
            double temp2;
            if (pixelColor.luminosity < 0.5)  //<=??
                temp2 = pixelColor.luminosity * (1.0 + pixelColor.saturation);
            else
                temp2 = pixelColor.luminosity + pixelColor.saturation - (pixelColor.luminosity * pixelColor.saturation);
            return temp2;
        }

        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;
            return temp3;
        }

        #endregion Private Methods
    }
}
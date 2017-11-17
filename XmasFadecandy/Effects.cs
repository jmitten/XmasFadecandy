using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace XmasFadecandy
{
    public class Effects
    {
        Client client;
        PixelStrip pixels;
        Random random;

        public static Effects Create(Client client, PixelStrip pixelStrip)
        {
            var _fx = new Effects();
            _fx.client = client;
            _fx.pixels = pixelStrip;
            _fx.random = new Random(DateTime.Now.Millisecond);
            return _fx;
        }

        private void Refresh()
        {
            client.putPixels(pixels);
        }

        public void TwinkleRandom(HSLColor baseColor, HSLColor twinkleColor, int speedDelay, int number, int duration)
        {
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                pixels.SetAll(baseColor);
                for (var j = 0; j < number; j++)
                {
                    var index = random.Next(0, pixels.Count);
                    pixels.Set(index, twinkleColor);
                }
                Refresh();
                Thread.Sleep(speedDelay);
            }
        }

        private class PixelState
        {

            //Is the pixel currently on
            public bool On { get; set; }


            //Time the pixel was turned on or off
            public DateTime TimeSwitched { get; set; }

            //Time the pixel should remain off
            public int TimeOff { get; set; }

            //Color if the pixel is on
            public HSLColor BaseColor { get; set; }
            public HSLColor TwinkleColor { get; set; }


            public int FadeStep { get; set; }

            //Returns the color between the base and twinkle color at the specified step(0-256)
            public HSLColor StepFade(int step, HSLColor baseColor, HSLColor twinkleColor)
            {
                var sColor = baseColor.ToRgbPixel();
                var eColor = twinkleColor.ToRgbPixel();
                var difR = eColor.r - sColor.r;
                var difG = eColor.g - sColor.g;
                var difB = eColor.b - sColor.b;

                var r = (int)sColor.r;
                var g = (int)sColor.g;
                var b = (int)sColor.b;

                r += Convert.ToInt32(difR * step / 255.0);
                g += Convert.ToInt32(difG * step / 255.0);
                b += Convert.ToInt32(difB * step / 255.0);

                return new HSLColor(r, g, b);


            }

        }


        public void FadeTwinkle(HSLColor baseColor, HSLColor twinkleColor, int fadeTime, int duration = -1){
            FadeTwinkleEffect(baseColor,twinkleColor,fadeTime,false,duration);
        }

        public void FadeTwinkleRandom(int fadeTime, int duration = -1){
            FadeTwinkleEffect(null,null,fadeTime,true,duration);
        }
        

        private void FadeTwinkleEffect(HSLColor baseColor, HSLColor twinkleColor, int fadeTime,bool randomColors = false, int duration = -1)
        {
            var pixel = new PixelState[pixels.Count];
            for (var i = 0; i < pixels.Count; i++)
            {
                pixel[i] = new PixelState();
                pixel[i].On = false;
                pixel[i].TimeSwitched = DateTime.Now;
                pixel[i].TimeOff = random.Next(0, fadeTime);
                pixel[i].BaseColor = randomColors ? new HSLColor(random.Next(0,256)) : baseColor;
                pixel[i].TwinkleColor = randomColors ? new HSLColor(random.Next(0,256)) : twinkleColor;
            }
            //pixels.SetAll(baseColor);
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration || duration == -1)
            {
                for (var i = 0; i < pixels.Count; i++)
                {
                    if (pixel[i].On)
                    {//Pixel is on
                        if (DateTime.Now.Subtract(pixel[i].TimeSwitched).TotalMilliseconds > fadeTime)
                        {
                            //Pixel has reached max on time
                            pixel[i].TimeSwitched = DateTime.Now;
                            if(randomColors){
pixel[i].TwinkleColor = new HSLColor(random.Next(0,256));
                            }
                            
                            pixel[i].TimeOff = random.Next(0, fadeTime);
                            pixel[i].FadeStep = 0;
                            pixel[i].On = false;

                        }
                        else
                        {
                            double longevity = 1.0 - DateTime.Now.Subtract(pixel[i].TimeSwitched).TotalMilliseconds / fadeTime;
                            int fadeStep = Convert.ToInt32(255 * longevity);
                            if (fadeStep != pixel[i].FadeStep)
                            {
                                pixel[i].FadeStep = fadeStep;
                                pixels.Set(i, pixel[i].StepFade(fadeStep, pixel[i].BaseColor, pixel[i].TwinkleColor));
                            }
                        }
                    }
                    else
                    {
                        //Pixel is off, check to see if it should be turned on
                        if(DateTime.Now.Subtract(pixel[i].TimeSwitched).TotalMilliseconds > pixel[i].TimeOff){
                            pixel[i].On = true;
                            pixel[i].FadeStep = 0;
                            if(randomColors){
pixel[i].BaseColor = new HSLColor(random.Next(0,256));
                            }
                            pixel[i].TimeSwitched = DateTime.Now;
                        }else{
                             double longevity = 1.0 - DateTime.Now.Subtract(pixel[i].TimeSwitched).TotalMilliseconds / pixel[i].TimeOff;
                            int fadeStep = Convert.ToInt32(255 * longevity);
                            if (fadeStep != pixel[i].FadeStep)
                            {
                                pixel[i].FadeStep = fadeStep;
                                pixels.Set(i, pixel[i].StepFade(fadeStep, pixel[i].TwinkleColor, pixel[i].BaseColor));
                            }
                        }
                    }
                }
                Refresh();
            }
        }

        public void ColorWheel(int sleepDuration, int duration)
        {
            var start = DateTime.Now;
            var step = 0;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                for (var i = 0; i < pixels.Count; i++)
                {
                    var c = new HSLColor(((i * 256 / pixels.Count) + step) % 255);
                    pixels.Set(i, c);
                }
                Refresh();
                Thread.Sleep(sleepDuration);
                step++;
            }
        }



        public void Alternate(HSLColor baseColor, HSLColor secondaryColor, int sleepDuration, int duration)
        {
            var start = DateTime.Now;
            var step = 0;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                pixels.SetAll(baseColor);
                for (var i = step; i < pixels.Count; i += 2)
                {
                    pixels.Set(i, secondaryColor);
                }
                Refresh();
                step++;
                if (step == 2)
                {
                    step = 0;
                }
                Thread.Sleep(sleepDuration);
            }
        }

        public void SnowSparkle(HSLColor backgroundColor, int sparkleDelay, int speedDelay, int duration)
        {
            var start = DateTime.Now;
            pixels.SetAll(backgroundColor);
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                int pixel = random.Next(0, pixels.Count);
                pixels.Set(pixel, HSLColor.White());
                Refresh();
                Thread.Sleep(sparkleDelay);
                pixels.Set(pixel, backgroundColor);
                Refresh();
                Thread.Sleep(speedDelay);
            }
        }


        public void RunningLights(HSLColor color, int waveDelay, int duration)
        {
            var start = DateTime.Now;
            var c = color.ToRgbPixel();
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                int Position = 0;

                for (int j = 0; j < pixels.Count * 2; j++)
                {
                    Position++; // = 0; //Position + Rate;
                    for (int i = 0; i < pixels.Count; i++)
                    {
                        // sine wave, 3 offset waves make a rainbow!
                        //float level = sin(i+Position) * 127 + 128;
                        //setPixel(i,level,0,0);
                        //float level = sin(i+Position) * 127 + 128;
                        var _color = new HSLColor(Convert.ToInt32(((Math.Sin(i + Position) * 127 + 128) / 255) * c.r),
                                   Convert.ToInt32(((Math.Sin(i + Position) * 127 + 128) / 255) * c.g),
                                   Convert.ToInt32(((Math.Sin(i + Position) * 127 + 128) / 255) * c.b));
                        pixels.Set(i, _color);
                    }

                    Refresh();
                    Thread.Sleep(waveDelay);
                }
            }
        }

        public void ColorWipe(HSLColor color, int speedDelay, bool reverse = false)
        {
            for (var i = 0; i < pixels.Count; i++)
            {
                var index = reverse ? pixels.Count - 1 - i : i;
                pixels.Set(index, color);
                Refresh();
                Thread.Sleep(speedDelay);
            }
        }

        public void FadeIn(HSLColor color, int speedDelay = 5)
        {
            var c = color.ToRgbPixel();
            for (var i = 0; i < 255; i++)
            {
                var ratio = i / 255.0;
                var r = (byte)Convert.ToInt32(ratio * c.r);
                var g = (byte)Convert.ToInt32(ratio * c.g);
                var b = (byte)Convert.ToInt32(ratio * c.b);
                pixels.SetAll(r, g, b);
                Refresh();
                if (speedDelay > 0)
                {
                    Thread.Sleep(speedDelay);
                }

            }
        }

        public void FadeOut(HSLColor color, int speedDelay = 5)
        {
            var c = color.ToRgbPixel();
            for (var i = 255; i >= 0; i--)
            {
                var ratio = i / 255.0;
                var r = (byte)Convert.ToInt32(ratio * c.r);
                var g = (byte)Convert.ToInt32(ratio * c.g);
                var b = (byte)Convert.ToInt32(ratio * c.b);
                pixels.SetAll(r, g, b);
                Refresh();
                if (speedDelay > 0)
                {
                    Thread.Sleep(speedDelay);
                }

            }
        }

        public void CrossFade(HSLColor startColor, HSLColor endColor, int speedDelay = 5)
        {
            var sColor = startColor.ToRgbPixel();
            var eColor = endColor.ToRgbPixel();
            var difR = eColor.r - sColor.r;
            var difG = eColor.g - sColor.g;
            var difB = eColor.b - sColor.b;
            var stepR = 0;
            var stepG = 0;
            var stepB = 0;
            if (difR != 0)
            {
                stepR = Convert.ToInt32(255.0 / Math.Abs(difR));
            }
            if (difG != 0)
            {
                stepG = Convert.ToInt32(255.0 / Math.Abs(difG));
            }
            if (difB != 0)
            {
                stepB = Convert.ToInt32(255.0 / Math.Abs(difB));
            }

            var r = sColor.r;
            var g = sColor.g;
            var b = sColor.b;

            for (var i = 0; i < 255; i++)
            {
                if (stepR != 0 && i % stepR == 0)
                {
                    if (difR > 0)
                    {
                        //Need to increase value
                        r++;
                        difR--;
                    }
                    if (difR < 0)
                    {
                        r--;
                        difR++;
                    }
                }
                if (stepG != 0 && i % stepG == 0)
                {
                    if (difG > 0)
                    {
                        //Need to increase value
                        g++;
                        difG--;
                    }
                    if (difG < 0)
                    {
                        g--;
                        difG++;
                    }
                }
                if (stepB != 0 && i % stepB == 0)
                {
                    if (difB > 0)
                    {
                        //Need to increase value
                        b++;
                        difB--;
                    }
                    if (difB < 0)
                    {
                        b--;
                        difB++;
                    }
                }
                pixels.SetAll(r, g, b);
                Refresh();
                if (speedDelay > 0)
                {
                    Thread.Sleep(speedDelay);
                }
            }
        }

        public void RainbowTheaterChase(int speedDelay, int duration)
        {
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                for (var j = 0; j < 256; j++)
                {     // cycle all 256 colors in the wheel
                    for (var q = 0; q < 3; q++)
                    {
                        for (var i = 0; i < pixels.Count; i = i + 3)
                        {
                            var c = new HSLColor((i + j) % 255);
                            if (i + q < pixels.Count)
                            {
                                pixels.Set(i + q, c);    //turn every third pixel on
                            }
                        }
                        Refresh();

                        if (speedDelay > 0)
                        {
                            Thread.Sleep(speedDelay);
                        }

                        for (int i = 0; i < pixels.Count; i = i + 3)
                        {
                            if (i + q < pixels.Count)
                            {
                                pixels.Set(i + q, 0, 0, 0);
                            }

                        }
                    }
                }
            }
        }

        public void TheaterChase(HSLColor color, int speedDelay, int duration)
        {
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                for (var q = 0; q < 3; q++)
                {
                    for (var i = 0; i < pixels.Count; i = i + 3)
                    {
                        if (i + q < pixels.Count)
                        {
                            pixels.Set(i + q, color);    //turn every third pixel on

                        }
                    }
                    Refresh();
                    if (speedDelay > 0)
                    {
                        Thread.Sleep(speedDelay);
                    }

                    for (var i = 0; i < pixels.Count; i = i + 3)
                    {
                        if (i + q < pixels.Count)
                        {
                            pixels.Set(i + q, 0, 0, 0);    //turn every third pixel off

                        }
                    }
                }
            }
        }

    }
}

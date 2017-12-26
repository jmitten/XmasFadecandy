using FadecandyController;
using System;
using System.Collections.Generic;
using System.Text;

namespace XmasFadecandy
{
    /// <summary>
    /// A library of effects for the Fadecandy ws2811 pixel controller
    /// </summary>
    public class PixelEffectsLibrary : PixelController
    {
        /// <summary>
        /// Contains effects for use with the Fadecandy controller
        /// </summary>
        /// <param name="client">The client connecting to the Fadecandy server</param>
        /// <param name="pixelStrip">The strip of pixels</param>
        public PixelEffectsLibrary(Client client, PixelStrip pixelStrip) : base(client, pixelStrip)
        {

        }

        /// <summary>
        /// Alternates between two colors on even and odd numbered pixels
        /// </summary>
        /// <param name="baseColor">The base color</param>
        /// <param name="secondaryColor">The secondary color</param>
        /// <param name="sleepDuration">The amount of seconds to waith before alternating colors</param>
        /// <param name="runCount">The number of times to alternate the colors</param>
        public void Alternate(PixelColor baseColor, PixelColor secondaryColor, double sleepDuration, int runCount)
        {
            var start = DateTime.Now;
            var step = 0;
            for (var j = 0; j < runCount; j++)
            {
                SetAll(baseColor);
                for (var i = step; i < Count; i += 2)
                {
                    SetPixel(i, secondaryColor);
                }
                Apply();
                step++;
                if (step == 2)
                {
                    step = 0;
                }
                Delay(sleepDuration);
            }
        }

        /// <summary>
        /// Cycles through a rainbow over all the pixels a specified number of times.
        /// </summary>
        /// <param name="speed">The speed of the animation in milliseconds. Lower values correspond to faster times.</param>
        public void ColorWheel(int speed)
        {
            for (var step = 0; step < 256; step++)
            {
                for (var i = 0; i < Count; i++)
                {
                    var c = new PixelColor((i * 256 / Count + step) % 255);
                    SetPixel(i, c);
                }
                Apply();
                Delay(speed);
            }
        }


        public void GradientWipe(int speedDelay){
            for(var i = 0; i < Count; i++){
                SetPixel(i, new PixelColor((i*256 / Count)));
                Apply();
                Delay(speedDelay);
            }
            for(var i = 0; i < Count; i++){
                SetPixel(Count-1-i,new PixelColor((i*256 / Count)));
                Apply();
                Delay(speedDelay);
            }
            

        }

        /// <summary>
        /// Lights all pixels up, one after another, and then turns them off in the same order.
        /// </summary>
        /// <param name="color">The pixel color</param>
        /// <param name="speed">The animation speed for each pixel in milliseconds</param>
        /// <param name="reverse">Reverse the animation</param>
        /// <param name="turnOff">Turn the pixels off, one after another, after they have been turned on.</param>
        public void ColorWipe(PixelColor color, int speed, bool reverse = false, bool turnOff = true)
        {
            for (var i = 0; i < Count; i++)
            {
                var index = reverse ? Count - 1 - i : i;
                SetPixel(index, color);
                Apply();
                Delay(speed);
            }
            if (turnOff)
            {
                for (var i = 0; i < Count; i++)
                {
                    var index = reverse ? Count - 1 - i : i;
                    SetPixel(index, PixelColor.Black);
                    Apply();
                    Delay(speed);
                }
            }


        }

        /// <summary>
        /// Cross fade between two colors
        /// </summary>
        /// <param name="startColor">The color at the start of the fade transition</param>
        /// <param name="endColor">The color at the end of the fade transition</param>
        /// <param name="speedDelay">The transition speed in milliseconds. Lower number correspond to faster transitions</param>
        public void CrossFade(PixelColor startColor, PixelColor endColor, int speedDelay = 5)
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
                SetAll(new PixelColor(r, g, b));
                Apply();
                if (speedDelay > 0)
                {
                    Delay(speedDelay);
                }
            }
        }

        /// <summary>
        /// Fade in a specified color
        /// </summary>
        /// <param name="color">The color to fade in</param>
        /// <param name="speedDelay">The transition speed in milliseconds. Lower number correspond to faster transitions</param>
        public void FadeIn(PixelColor color, int speedDelay = 5)
        {
            var c = color.ToRgbPixel();
            for (var i = 0; i < 255; i++)
            {
                var ratio = i / 255.0;
                var r = (byte)Convert.ToInt32(ratio * c.r);
                var g = (byte)Convert.ToInt32(ratio * c.g);
                var b = (byte)Convert.ToInt32(ratio * c.b);
                SetAll(new PixelColor(r, g, b));
                Apply();
                if (speedDelay > 0)
                {
                    Delay(speedDelay);
                }
            }
        }
        /// <summary>
        /// Fade out a specified color
        /// </summary>
        /// <param name="color">The color to fade out</param>
        /// <param name="speedDelay">The transition speed in milliseconds. Lower number correspond to faster transitions</param>
        public void FadeOut(PixelColor color, int speedDelay = 5)
        {
            var c = color.ToRgbPixel();
            for (var i = 255; i >= 0; i--)
            {
                var ratio = i / 255.0;
                var r = (byte)Convert.ToInt32(ratio * c.r);
                var g = (byte)Convert.ToInt32(ratio * c.g);
                var b = (byte)Convert.ToInt32(ratio * c.b);
                SetAll(new PixelColor(r, g, b));
                Apply();
                if (speedDelay > 0)
                {
                    Delay(speedDelay);
                }
            }
        }


        /// <summary>
        /// Twinkle between two colors randomly over all pixels with smooth fades
        /// </summary>
        /// <param name="baseColor">The first colot to use</param>
        /// <param name="secondColor">The second color to use</param>
        /// <param name="fadeTime"></param>
        /// <param name="duration"></param>
        public void FadingTwinkle(PixelColor baseColor, PixelColor secondColor, int fadeTime, int duration = -1)
        {
            FadeTwinkleEffect(baseColor, secondColor, fadeTime, false, duration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeTime"></param>
        /// <param name="duration"></param>
        public void FadeTwinkleRandom(int fadeTime, int duration = -1)
        {
            FadeTwinkleEffect(null, null, fadeTime, true, duration);
        }


        /// <summary>
        /// Cycles all pixels at once through a rainbow
        /// </summary>
        /// <param name="speed"></param>
        public void Rainbow(int speed)
        {
            for (var i = 0; i < 256; i++)
            {
                SetAll(new PixelColor(i));
                Apply();
                Delay(speed);
            }
        }

           /// <summary>
        /// Sets all pixels to a base color and randomly flashes a number of pixels to a specified color.
        /// </summary>
        /// <param name="baseColor">The base color of the pixels</param>
        /// <param name="twinkleColor">The color to flash on single pixels</param>
        /// <param name="speedDelay">The flash duration in seconds</param>
        /// <param name="number">The number of pixels to twinkle</param>
        /// <param name="duration">The duration </param>
        public void TwinkleRandom(PixelColor baseColor, PixelColor twinkleColor, int speedDelay, int number, int duration = -1)
        {
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration || duration == -1)
            {
                SetAll(baseColor);
                var visted = new Dictionary<int,int>();
                for (var j = 0; j < number; j++)
                {
                    var index = Random.Next(0, Count);
                    while(visted.ContainsKey(index)){
                        index = Random.Next(0,Count);
                    }
                    visted.Add(index,index);
                    SetPixel(index, twinkleColor);
                }
                Apply();
                Delay(speedDelay);
            }
        }

/*
public void BouncingColoredBalls(int ballCount, int duration) {
  double gravity = -9.81;
  var startHeight = 1;
  
  var height = new double[ballCount];
  var impactVelocityStart = Math.Sqrt( -2 * gravity * startHeight );
  var impactVelocity = new double[ballCount];
  var timeSinceLastBounce = new double[ballCount];
  var position = new int[ballCount];
  var clockTimeSinceLastBounce = new DateTime[ballCount];
  var dampening = new double[ballCount];
  var colors = new PixelColor[ballCount];
  
  for (var i = 0 ; i < ballCount ; i++) {   
    clockTimeSinceLastBounce[i] = DateTime.Now;
    height[i] = startHeight;
    position[i] = 0; 
    impactVelocity[i] =impactVelocityStart;
    timeSinceLastBounce[i] = 0;
    dampening[i] = 0.90 - i/Math.Pow(ballCount,2); 
    colors[i] = new PixelColor(Random.Next(0,255),Random.Next(0,255),Random.Next(0,255));

  }
    var start = DateTime.Now;
     while (DateTime.Now.Subtract(start).TotalMilliseconds < duration || duration == -1){
    for (var i = 0 ; i < ballCount ; i++) {
      timeSinceLastBounce[i] =  DateTime.Now.Subtract(clockTimeSinceLastBounce[i]).TotalMilliseconds;
      height[i] = 0.5 * gravity * Math.Pow( timeSinceLastBounce[i]/1000 , 2.0 ) + impactVelocity[i] * timeSinceLastBounce[i]/1000;
  
      if ( height[i] < 0 ) {                      
        height[i] = 0;
        impactVelocity[i] = dampening[i] * impactVelocity[i];
        clockTimeSinceLastBounce[i] = DateTime.Now;
  
        if ( impactVelocity[i] < 0.01 ) {
          impactVelocity[i] = impactVelocityStart;
        }
      }
      position[i] = Convert.ToInt32( height[i] * (Count - 1) / startHeight);
    }
  
    for (var i = 0 ; i < ballCount ; i++) {
      SetPixel(position[i],colors[i]);
    }
    
    Apply();
    Delay(1);
    Clear();
  }
}
     */


     

public void BubbleSort() {
  var colors = new PixelColor[Count];
  var colorWeight = new int[Count];
  for (var i = 0; i < Count; i++) {
    int red = 0;
    int green = 0;
    int blue = 0;
    int color = Random.Next(0, 4);
    if (color == 0) {
      red = 255;
      colorWeight[i] = 0;
    } else if (color == 1) {
      green = 255;
            colorWeight[i] = 1;


    } else if (color == 2) {
      red = 255;
      green = 255;
      blue = 255;
            colorWeight[i] = 2;

    }
    else {
      blue = 255;
            colorWeight[i] = 3;

    }


   colors[i] = new PixelColor(red,green,blue);
    SetPixel(i, colors[i]);
  }
  Apply();
  for (int i = 0; i < Count - 1; i++){
    for (int j = 0; j < Count - i - 1; j++) {
      if (colorWeight[j] > colorWeight[j + 1]) {
          var temp = colors[j+1];
          var weightTemp = colorWeight[j+1];
          colors[j+1] = colors[j];
          colorWeight[j+1] = colorWeight[j];
          colors[j] = temp;
          colorWeight[j] = weightTemp;
          SetPixel(j,colors[j]);
          SetPixel(j+1,colors[j+1]);
        Apply();
        Delay(2);
      }
    }
  }
  LeftRotate(colors,Count*5);

}  

/*Function to left rotate array of size n by d*/
public void LeftRotate(PixelColor[] colors, int d)
{
  int i;
  for (i = 0; i < d; i++)
    leftRotateByOne(colors);
}

private void leftRotateByOne(PixelColor[] colors)
{
     var temp = colors[0];
  for (var i = 0; i < Count-1; i++){
     
      colors[i] = colors[i+1];
     SetPixel(i, colors[i]);
  }
     colors[Count - 1] = temp;
     SetPixel(Count - 1, temp);
     Apply();
     Delay(20);
}

        public void RainbowTheaterChase(int speedDelay, int duration)
        {
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                for (var j = 0; j < 256; j++)
                {
                    // cycle all 256 colors in the wheel
                    for (var q = 0; q < 3; q++)
                    {
                        for (var i = 0; i < Count; i = i + 3)
                        {
                            var c = new PixelColor((i + j) % 255);
                            if (i + q < Count)
                            {
                                SetPixel(i + q, c); //turn every third pixel on
                            }
                        }
                        Apply();

                        if (speedDelay > 0)
                        {
                            Delay(speedDelay);
                        }

                        for (var i = 0; i < Count; i = i + 3)
                        {
                            if (i + q < Count)
                            {
                                SetPixel(i + q,PixelColor.Black);
                            }
                        }
                    }
                }
            }
        }

        public void RunningLights(PixelColor color, int waveDelay, int duration)
        {
            var start = DateTime.Now;
            var c = color.ToRgbPixel();
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                var position = 0;

                for (var j = 0; j < Count * 2; j++)
                {
                    position++; // = 0; //Position + Rate;
                    for (var i = 0; i < Count; i++)
                    {
                        var clr = new PixelColor(Convert.ToInt32((Math.Sin(i + position) * 127 + 128) / 255 * c.r),
                            Convert.ToInt32((Math.Sin(i + position) * 127 + 128) / 255 * c.g),
                            Convert.ToInt32((Math.Sin(i + position) * 127 + 128) / 255 * c.b));
                        SetPixel(i, clr);
                    }

                    Apply();
                    Delay(waveDelay);
                }
            }
        }

        /// <summary>
        /// Runs a single pixel back and fourth
        /// </summary>
        /// <param name="backgroundColor">The color to set all the pixels</param>
        /// <param name="pixelColor">The color of the pixel running back and fourth</param>
        /// <param name="speed">The time for a single pixel</param>
        /// <para name="reverse">Reverse the effect</para>
        public void Scan(PixelColor backgroundColor, PixelColor pixelColor, int speedDelay, bool reverse = false)
        {
            SetAll(backgroundColor);
            for (var j = 0; j < 2; j++)
            {
                for (var i = 0; i < Count; i++)
                {
                    var index = reverse ? Count - 1 - i : i;
                    SetPixel(index, pixelColor);
                    if (!reverse && index > 0)
                    {
                        SetPixel(index - 1, backgroundColor);
                    }
                    if (reverse && index < Count - 1)
                    {
                        SetPixel(index + 1, backgroundColor);
                    }
                    Apply();
                    Delay(speedDelay);
                }
                reverse = !reverse;
            }
        }

        public void SnowSparkle(PixelColor backgroundColor, int sparkleDelay, int speedDelay, int duration)
        {
            var start = DateTime.Now;
            SetAll(backgroundColor);
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                var pixel = Random.Next(0, Count);
                SetPixel(pixel, PixelColor.White);
                Apply();
                Delay(sparkleDelay);
                SetPixel(pixel, backgroundColor);
                Apply();
                Delay(speedDelay);
            }
        }

        public void Strobe(PixelColor color, int strobeCount, int flashDelay, int endPause)
        {
            for (var i = 0; i < strobeCount; i++)
            {
                SetAll(color);
                Apply();
                Delay(flashDelay);
                Clear();
                Apply();
                Delay(endPause);
            }
        }

public void CylonBounce(PixelColor color, int eyeSize, int speedDelay, int returnDelay){
var c = color.ToRgbPixel();
  for(int i = 0; i < Count-eyeSize-2; i++) {
    Clear();
    SetPixel(i, new PixelColor(Convert.ToInt32(c.r/10.0), Convert.ToInt32(c.g/10.0), Convert.ToInt32(c.b/10.0)));
    for(int j = 1; j <= eyeSize; j++) {
      SetPixel(i+j, color); 
    }
    SetPixel(i+eyeSize+1, new PixelColor(Convert.ToInt32(c.r/10.0), Convert.ToInt32(c.g/10.0), Convert.ToInt32(c.b/10.0)));
    Apply();
    Delay(speedDelay);
  }

  Delay(returnDelay);

  for(int i = Count-eyeSize-2; i > 0; i--) {
    Clear();
    SetPixel(i, new PixelColor(Convert.ToInt32(c.r/10.0), Convert.ToInt32(c.g/10.0), Convert.ToInt32(c.b/10.0)));
    for(int j = 1; j <= eyeSize; j++) {
      SetPixel(i+j, color); 
    }
    SetPixel(i+eyeSize+1, new PixelColor(Convert.ToInt32(c.r/10.0), Convert.ToInt32(c.g/10.0), Convert.ToInt32(c.b/10.0)));

    Apply();
    Delay(speedDelay);
  }
  
  Delay(returnDelay);
}

        public void TheaterChase(PixelColor color, int speedDelay, int duration)
        {
            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration)
            {
                for (var q = 0; q < 3; q++)
                {
                    for (var i = 0; i < Count; i = i + 3)
                    {
                        if (i + q < Count)
                        {
                            SetPixel(i + q, color); //turn every third pixel on
                        }
                    }
                    Apply();
                    if (speedDelay > 0)
                    {
                        Delay(speedDelay);
                    }

                    for (var i = 0; i < Count; i = i + 3)
                    {
                        if (i + q < Count)
                        {
                            SetPixel(i + q,PixelColor.Black); //turn every third pixel off
                        }
                    }
                }
            }
        }

     
        
        #region Private Methods

        private void FadeTwinkleEffect(PixelColor baseColor, PixelColor twinkleColor, int fadeTime,
            bool randomColors = false, int duration = -1)
        {
            var pixel = new PixelState[Count];
            for (var i = 0; i < Count; i++)
            {
                pixel[i] = new PixelState
                {
                    On = false,
                    TimeSwitched = DateTime.Now,
                    TimeOff = Random.Next(0, fadeTime),
                    BaseColor = randomColors ? new PixelColor(Random.Next(0, 256)) : baseColor,
                    TwinkleColor = randomColors ? new PixelColor(Random.Next(0, 256)) : twinkleColor
                };
            }

            var start = DateTime.Now;
            while (DateTime.Now.Subtract(start).TotalMilliseconds < duration || duration == -1)
            {
                for (var i = 0; i < Count; i++)
                {
                    if (pixel[i].On)
                    {
                        //Pixel is on
                        if (DateTime.Now.Subtract(pixel[i].TimeSwitched).TotalMilliseconds > fadeTime)
                        {
                            //Pixel has reached max on time
                            pixel[i].TimeSwitched = DateTime.Now;
                            if (randomColors)
                            {
                                pixel[i].TwinkleColor = new PixelColor(Random.Next(0, 256));
                            }

                            pixel[i].TimeOff = Random.Next(0, fadeTime);
                            pixel[i].FadeStep = 0;
                            pixel[i].On = false;
                        }
                        else
                        {
                            var longevity = 1.0 - DateTime.Now.Subtract(pixel[i].TimeSwitched).TotalMilliseconds /
                                            fadeTime;
                            var fadeStep = Convert.ToInt32(255 * longevity);
                            if (fadeStep != pixel[i].FadeStep)
                            {
                                pixel[i].FadeStep = fadeStep;
                                SetPixel(i,
                                    PixelState.StepFade(fadeStep, pixel[i].BaseColor, pixel[i].TwinkleColor));
                            }
                        }
                    }
                    else
                    {
                        //Pixel is off, check to see if it should be turned on
                        if (DateTime.Now.Subtract(pixel[i].TimeSwitched).TotalMilliseconds > pixel[i].TimeOff)
                        {
                            pixel[i].On = true;
                            pixel[i].FadeStep = 0;
                            if (randomColors)
                            {
                                pixel[i].BaseColor = new PixelColor(Random.Next(0, 256));
                            }
                            pixel[i].TimeSwitched = DateTime.Now;
                        }
                        else
                        {
                            var longevity = 1.0 - DateTime.Now.Subtract(pixel[i].TimeSwitched).TotalMilliseconds /
                                            pixel[i].TimeOff;
                            var fadeStep = Convert.ToInt32(255 * longevity);
                            if (fadeStep != pixel[i].FadeStep)
                            {
                                pixel[i].FadeStep = fadeStep;
                                SetPixel(i,
                                    PixelState.StepFade(fadeStep, pixel[i].TwinkleColor, pixel[i].BaseColor));
                            }
                        }
                    }
                }
                Apply();
            }
        }

        #endregion Private Methods


        #region Private Classes

        /// <summary>
        /// Class to hold the state of each pixel for fading twinkle effects
        /// </summary>
        private class PixelState
        {
            #region Public Properties

            //Color if the pixel is on
            public PixelColor BaseColor { get; set; }

            public int FadeStep { get; set; }

            //Is the pixel currently on
            public bool On { get; set; }

            //Time the pixel should remain off
            public int TimeOff { get; set; }

            //Time the pixel was turned on or off
            public DateTime TimeSwitched { get; set; }

            public PixelColor TwinkleColor { get; set; }

            #endregion Public Properties

            #region Public Methods

            //Returns the color between the base and twinkle color at the specified step(0-256)
            public static PixelColor StepFade(int step, PixelColor baseColor, PixelColor twinkleColor)
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

                return new PixelColor(r, g, b);
            }

            #endregion Public Methods
        }

        #endregion Private Classes
    }

}

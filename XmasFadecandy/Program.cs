using System;

namespace XmasFadecandy
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("192.168.0.50", 7890);
            PixelStrip pixels = new PixelStrip(50);
            var fx = Effects.Create(client,pixels);
            while (true)
            {
                fx.FadeTwinkle(HSLColor.Red(),new HSLColor(0,0,0),1000,20000);
                fx.FadeTwinkleRandom(5000,20000);
                fx.TheaterChase(HSLColor.Blue(),100,10000);
                fx.RainbowTheaterChase(50,10000);
                
                fx.TheaterChase(HSLColor.Red(),100,10000);
                fx.FadeIn(HSLColor.Blue());
                fx.FadeOut(HSLColor.Blue());
                fx.FadeIn(HSLColor.Red(),10);
                fx.FadeOut(HSLColor.Red(),10);
                
                fx.FadeIn(new HSLColor(128,0,255),10);
                fx.CrossFade(new HSLColor(128,0,255), new HSLColor(255,0,0),10);
                fx.CrossFade(new HSLColor(255,0,0), new HSLColor(0,0,255),10);
                fx.CrossFade(new HSLColor(0,0,255), new HSLColor(216,128,0),10);
                fx.CrossFade(new HSLColor(216,128,0), new HSLColor(128,128,128),10);
                fx.CrossFade(new HSLColor(128,128,128), new HSLColor(255,128,128),10);
                fx.CrossFade(new HSLColor(255,128,128), new HSLColor(128,255,128),10);
                fx.CrossFade(new HSLColor(128,255,128), new HSLColor(128,128,255),10);
                fx.CrossFade(new HSLColor(128,128,255), new HSLColor(0,0,0),10);
                fx.CrossFade(new HSLColor(0,0,0), new HSLColor(255,0,0),10);
                fx.CrossFade(new HSLColor(255,0,0), new HSLColor(0,255,0),10);
                fx.CrossFade(new HSLColor(0,255,0), new HSLColor(0,0,255),10);
            
                fx.ColorWipe(HSLColor.Red(),50);
                fx.ColorWipe(HSLColor.Blue(),50,true);
                fx.RunningLights(new HSLColor(255,255,0),50,20000);
                fx.SnowSparkle(new HSLColor(60,60,60),20,200,20000);
                fx.Alternate(new HSLColor(0,0,255), new HSLColor(255,255,255),500,20000);
                fx.Alternate(new HSLColor(255,0,0), new HSLColor(0,255,0),500,20000);
                fx.TwinkleRandom(new HSLColor(0, 0, 255), new HSLColor(255, 255, 255), 50, 4,10000);
                fx.ColorWheel(30,10000);
                
            }
        }
    }
}

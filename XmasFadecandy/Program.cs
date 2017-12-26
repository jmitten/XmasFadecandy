using FadecandyController;
using System;
using System.Threading;

namespace XmasFadecandy
{
    internal class Program
    {

    
        private static void Main(string[] args)
        {
            var client = new Client("127.0.0.1", 7890);
            var pixels = new PixelStrip(179);
            var fx = new PixelEffectsLibrary(client,pixels);
            
            while (true)
            {

                for(var i = 0; i < 4; i++){
                    fx.GradientWipe(10);
                }
                
                fx.BubbleSort();
                fx.TwinkleRandom(PixelColor.Black,PixelColor.White,100,15,30000);
                fx.CrossFade(PixelColor.Blue,PixelColor.Red);
                fx.Alternate(PixelColor.Red, PixelColor.Green,0.3,30);
                fx.FadingTwinkle(PixelColor.Red, PixelColor.White,500,30000);
                fx.FadingTwinkle(PixelColor.Blue, PixelColor.White,500,30000);

                fx.ColorWipe(PixelColor.Red,10,false,false);
                fx.ColorWipe(PixelColor.Green,10,true,false);
                fx.ColorWipe(PixelColor.Blue,10,false,false);
                fx.ColorWipe(PixelColor.White,10,true,false);
                fx.Strobe(PixelColor.White,10,200,200);
                
                for(var i = 0; i < 5; i++){
                    fx.Rainbow(5);
                }
                
                

                for(var i = 0; i < 5; i++){
                    fx.ColorWheel(5);
                }
                
                fx.Strobe(PixelColor.Red,5,200,200);
                fx.Strobe(PixelColor.White,5,200,200);
                fx.Strobe(PixelColor.Blue,5,200,200);
                
                fx.TheaterChase(PixelColor.Green,100,5000);
                fx.TheaterChase(PixelColor.Blue,100,5000);
                fx.TheaterChase(PixelColor.Red,100,5000);
                fx.TheaterChase(PixelColor.White,100,5000);
               
                fx.Alternate(PixelColor.White, PixelColor.Blue,0.1,60);
                fx.FadeTwinkleRandom(1000,30000);
                
                fx.ColorWipe(PixelColor.White,10);
                fx.FadeIn(PixelColor.Red);
                fx.FadeOut(PixelColor.Red);
                fx.CylonBounce(PixelColor.Red,30,10,50);
                fx.FadeIn(PixelColor.Green);
                fx.FadeOut(PixelColor.Green);
                fx.CylonBounce(PixelColor.Green,30,10,50);
                fx.FadeIn(PixelColor.Blue);
                fx.FadeOut(PixelColor.Blue);         
                fx.CylonBounce(PixelColor.Blue,30,10,50);
                fx.FadeIn(PixelColor.White);
                fx.FadeOut(PixelColor.White);
                fx.CylonBounce(PixelColor.White,30,10,50);
                

                

            }
        }
    }
}
using FadecandyController;

namespace XmasFadecandy
{
    class ChristmasShow : LightShow
    {
        private readonly PixelEffectsLibrary fx;
        public ChristmasShow(Client client, PixelStrip pixelStrip)
        {
            
            fx = new PixelEffectsLibrary(client, pixelStrip);
        }

        public override void AddEffects()
        {
            AddEffect(()=>{
                for(var i = 0; i < 10; i++){

                               fx.ColorWheel(30);

                }
                fx.Scan(PixelColor.Blue, PixelColor.White, 5, true);
                //fx.FadeTwinkle(PixelColor.Red, PixelColor.White, 1000, 20000);
                fx.FadeTwinkleRandom(5000, 20000);
                fx.TheaterChase(PixelColor.Blue, 150, 10000);
               // fx.RainbowTheaterChase(10, 10000);
                fx.TheaterChase(PixelColor.Red,100, 10000); //Theater chase needs worked on...
                fx.FadeIn(PixelColor.Blue);
                fx.FadeOut(PixelColor.Blue);
                fx.FadeIn(PixelColor.Red, 10);
                fx.FadeOut(PixelColor.Red, 10);

                fx.FadeIn(new PixelColor(128, 0, 255), 10);
                fx.CrossFade(new PixelColor(128, 0, 255), new PixelColor(255, 0, 0), 10);
                fx.CrossFade(new PixelColor(255, 0, 0), new PixelColor(0, 0, 255), 10);
                fx.CrossFade(new PixelColor(0, 0, 255), new PixelColor(216, 128, 0), 10);
                fx.CrossFade(new PixelColor(216, 128, 0), new PixelColor(128, 128, 128), 10);
                fx.CrossFade(new PixelColor(128, 128, 128), new PixelColor(255, 128, 128), 10);
                fx.CrossFade(new PixelColor(255, 128, 128), new PixelColor(128, 255, 128), 10);
                fx.CrossFade(new PixelColor(128, 255, 128), new PixelColor(128, 128, 255), 10);
                fx.CrossFade(new PixelColor(128, 128, 255), new PixelColor(0, 0, 0), 10);
                fx.CrossFade(new PixelColor(0, 0, 0), new PixelColor(255, 0, 0), 10);
                fx.CrossFade(new PixelColor(255, 0, 0), new PixelColor(0, 255, 0), 10);
                fx.CrossFade(new PixelColor(0, 255, 0), new PixelColor(0, 0, 255), 10);

                fx.ColorWipe(PixelColor.Red, 50);
                fx.ColorWipe(PixelColor.Blue, 50, true);
                fx.RunningLights(new PixelColor(255, 255, 0), 50, 20000);
                fx.SnowSparkle(new PixelColor(0, 0, 255), 20, 200, 20000);
               // fx.Alternate(new PixelColor(0, 0, 255), new PixelColor(255, 255, 255), 500, 20000);
               // fx.Alternate(new PixelColor(255, 0, 0), new PixelColor(0, 255, 0), 500, 20000);
               // fx.TwinkleRandom(new PixelColor(0, 0, 255), new PixelColor(255, 255, 255), 50, 4, 10000);
            });
            //AddEffect(()=>fx.ColorWheel(6));
           // AddEffect(()=>fx.FadingTwinkle(PixelColor.Blue,PixelColor.Red,3000,60000));
            //AddEffect(() => fx.ColorWheel(6));
           // AddEffect(()=>{
          //      fx.ColorWipe(PixelColor.Orange,100);
          //  });
            //AddEffect(() => fx.Alternate(PixelColor.Blue, PixelColor.Red, 0.1, 60));
            //AddEffect(() => fx.CrossFade(PixelColor.Red, PixelColor.Green));
        }

    }
}

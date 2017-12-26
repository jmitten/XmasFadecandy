using System;
using System.Threading;


namespace FadecandyController
{
    /// <inheritdoc />
    /// <summary>
    /// Fadecandy ws2811 pixel controller
    /// </summary>
    public class PixelController : IPixelController
    {
        private readonly Client _client;
        private readonly PixelStrip _pixels;


        protected readonly Random Random;


        private PixelController()
        {
            
        }
        
        
        protected PixelController(Client client, PixelStrip pixelStrip)
        {
            _client = client;
            _pixels = pixelStrip;
            Random = new Random(DateTime.Now.TimeOfDay.GetHashCode());
        }



        public int Count => _pixels.Count;

        /// <inheritdoc />
        public void Apply()
        {
            _client.PutPixels(_pixels);
        }
        
        /// <inheritdoc />
        public void SetAll(PixelColor color)
        {
            _pixels.SetAll(color);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _pixels.ClearAll();
        }
        
        /// <inheritdoc />
        public void SetPixel(int index, PixelColor color)
        {
            _pixels.Set(index,color);
        }

        /// <inheritdoc />
        public void Delay(int milliseconds)
        {
            if(milliseconds < 0)
            {
                milliseconds = 0;
            }
            Thread.Sleep(milliseconds);
        }
        
        /// <inheritdoc />
        public void Delay(double seconds)
        {
            if (seconds < 0)
            {
                seconds = 0;
            }
            Thread.Sleep(Convert.ToInt32(seconds*1000));
        }
    }
}
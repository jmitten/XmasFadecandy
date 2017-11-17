using System;
using System.Collections.Generic;

namespace XmasFadecandy
{
    public class PixelStrip : List<Pixel>
    {
        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public PixelStrip(int size)
        {
            Size = size;
            for (int i = 0; i < Size; i++)
            {
                Add(200, 200, 200);
            }
        }

        public void Add(byte obj, byte obj2, byte obj3)
        {
            base.Add(new Pixel(obj, obj2, obj3));
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    base.RemoveAt(base.Count);
                }
            }
        }
        public void Set(int index, byte obj, byte obj2, byte obj3)
        {
            base[index] = new Pixel(obj, obj2, obj3);
        }

        public void Set(int index, HSLColor color){
            var c = color.ToRgbPixel();
            Set(index,c.r,c.g,c.b);
        }
        public void SetAll(byte obj, byte obj2, byte obj3)
        {
            for (var i = 0; i < base.Count; i++)
            {
                base[i] = new Pixel(obj, obj2, obj3);
            }
        }

        public void SetAll(HSLColor color){
            var c = color.ToRgbPixel();
            SetAll(c.r,c.g,c.b);
        }

        public void ClearAll(){
            SetAll(0,0,0);
        }
        


    }

    public class Pixel : Tuple<byte, byte, byte>
    {
        public byte r;
        public byte g;
        public byte b;

        public Pixel(byte red, byte green, byte blue) : base(red, green, blue)
        {
            r = red;
            g = green;
            b = blue;
        }
    }
}

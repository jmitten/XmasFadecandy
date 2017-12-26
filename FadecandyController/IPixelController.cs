namespace FadecandyController
{
    /// <summary>
    /// Fadecandy ws2811 pixel controller interface
    /// </summary>
    internal interface IPixelController
    {
        /// <summary>
        /// The number of pixels in the strip
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Send changes to the pixel strip
        /// </summary>
        void Apply();

        /// <summary>
        /// Sets all pixels to a single color. Refresh() must be called to see the changes.
        /// </summary>
        /// <param name="color"></param>
        void SetAll(PixelColor color);

        /// <summary>
        /// Clears all pixels. Refresh() must be called to see the changes.
        /// </summary>
        void Clear();

        /// <summary>
        /// Sets a single pixel to a specified color. Refresh() must be called to see the changes.
        /// </summary>
        /// <param name="index">The index of the pixel</param>
        /// <param name="color">The color to set the pixel</param>
        void SetPixel(int index, PixelColor color);

        /// <summary>
        /// Wait for a specified number of milliseconds
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to wait</param>
        void Delay(int milliseconds);

        /// <summary>
        /// Wait for a specified number of seconds
        /// </summary>
        /// <param name="seconds">The number of seconds to wait</param>
        void Delay(double seconds);

    }
}
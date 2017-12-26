using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FadecandyController
{
    public abstract class LightShow : ILightShow
    {
        private static ConcurrentQueue<Action> showQueue = new ConcurrentQueue<Action>();
        private static AutoResetEvent signal = new AutoResetEvent(false);
        private static Thread performingThread = null;
        private static volatile bool perform = false;
        private static volatile bool terminate = false;

        public LightShow()
        {
            AddEffects();
        }

        /// <summary>
        /// Add show effects here
        /// </summary>
        public abstract void AddEffects();

        /// <summary>
        /// Add an effect to the show queue.
        /// </summary>
        /// <param name="action"></param>
        public void AddEffect(Action action)
        {
            showQueue.Enqueue(action);
        }

        /// <summary>
        /// Play or resume the light show
        /// </summary>
        public void Play()
        {
            PlayShow();
        }

        /// <summary>
        /// Stop the light show. The effect that is currently running will finish.
        /// </summary>
        public void Stop()
        {
            StopShow();
        }

        /// <summary>
        /// End the show in a thread safe way. The effect that is currently running will finish.
        /// </summary>
        public void Close()
        {
           
            CloseShow();
        }

        private static void PlayShow()
        {

            
            perform = true;
            signal.Set();
            if (performingThread == null)
            {
                performingThread = new Thread(Perform);
                performingThread.Start();
            }
        }


        

        private static void StopShow()
        {
            perform = false;
            signal.Reset();
        }

        private static void CloseShow()
        {
            perform = false;
            terminate = true;
            signal.Set();
            performingThread.Join();
            performingThread = null;
        }

        private static void Perform()
        {
            while (signal.WaitOne())
            {
                while (perform)
                {
                    Action effect = null;
                    while (showQueue.TryDequeue(out effect))
                    {
                        effect();
                        showQueue.Enqueue(effect);
                        if (!perform)
                        {
                            break;
                        }
                    }
                }
                if (terminate)
                {
                    break;
                }
            }            
        }
            
            
        
    }
}

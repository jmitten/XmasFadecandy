using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace FadecandyController
{
    public class RgbTupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        public void Add(T1 item, T2 item2, T3 item3)
        {
            Add(new Tuple<T1, T2, T3>(item, item2, item3));
        }
    }

    public class Client : IDisposable
    {
        private bool _verbose;

        private readonly bool _longConnection;

        private readonly string _ip;

        private readonly int _port;

        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Client(string ip, int port, bool longConnecton = true, bool verbose = false)
        {
            _ip = ip;
            _port = port;
            _longConnection = longConnecton;
            _verbose = verbose;

            Debug(string.Format("{0}:{1}", _ip, _port));
        }

        private static void Debug(string message)
        {
            //Console.WriteLine(message);
        }

        private bool EnsureConnected()
        {
            if (_socket.Connected)
            {
                Debug("Ensure Connected: already connected, doing nothing");
                return true;
            }
            try
            {
                Debug("Ensure Connected: trying to connect...");
                _socket.Ttl = 1;
                var ip = IPAddress.Parse(_ip);
                _socket.Connect(ip, _port);
                Debug("Ensure Connected: ....success");
                return true;
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Dispose()
        {
            Debug("Disconnecting");
            if (_socket.Connected)
            {
                _socket.Dispose();
            }
        }

        private bool CanConnect()
        {
            var success = EnsureConnected();
            if (!_longConnection)
            {
                Dispose();
            }
            return success;
        }

        public void PutPixels(PixelStrip pixels, int channel = 0)
        {
            Debug("Put pixels: Connecting");
            var isConnected = EnsureConnected();
            if (!isConnected)
            {
                Debug("Put pixels not connected. Ignoring these pixels.");
            }

            var lenHiByte = pixels.Count * 3 / 256;
            var lenLowByte = (pixels.Count * 3) % 256;

            var pieces = new List<byte>
            {
                Convert.ToByte(channel),
                Convert.ToByte(0),
                Convert.ToByte(lenHiByte),
                Convert.ToByte(lenLowByte)
            };

            foreach (var item in pixels)
            {
                pieces.Add(item.r);
                pieces.Add(item.g);
                pieces.Add(item.b);
            }

            var message = new byte[pieces.Count];

            for (var i = 0; i < pieces.Count; i++)
            {
                message[i] = pieces[i];
            }

            _socket.Send(message);
            _socket.Send(message);
        }
    }
}
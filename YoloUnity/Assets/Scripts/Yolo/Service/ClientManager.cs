using UnityEngine;
using Grpc.Core;
using System;
using System.Diagnostics;

namespace Yolo
{
    public class ClientManager : IDisposable
    {
        private readonly HiResScreenShots _screenShotService;
        private readonly Channel _channel;
        private readonly ClientWrapper _client;
        private readonly YoloResult _result;
        private readonly DetectionEventArgs _detectionArgs;

        private readonly Stopwatch _timer;
        private const int IntervalInMilliseconds = 200; // throttle requests

        public ClientManager(HiResScreenShots screenShotService)
        {
            this._screenShotService = screenShotService;
            _channel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            _client = new ClientWrapper(new YoloService.YoloServiceClient(_channel));

            _result = new YoloResult();
            _detectionArgs = new DetectionEventArgs(_result);

            _timer = new Stopwatch();
            _timer.Start();
        }

        public void Dispose() => _channel.ShutdownAsync();

        public event EventHandler<DetectionEventArgs> RaiseDetectionEvent;

        public void Update(Camera camera, Vector2Int size)
        {
            if (_client.IsIdle)
            {
                if (_timer.ElapsedMilliseconds >= IntervalInMilliseconds)
                {
                    _timer.Restart();
                    _result.Clear();
                    var image = _screenShotService.GetScreenShot(camera, size);
                    _client.Detect(image, _result);
                    
                    // TODO Modification
                    //client.Detect(ImageConversion.EncodeToPNG(texture), result);
                }
            }
            else if (_client.HasNewResponse)
            {
                UnityEngine.Debug.Log(string.Format("Detection time: {0}ms, Roundtrip time: {1}ms",
                   _result.ElapsedMilliseconds, _timer.Elapsed.Milliseconds));

                _timer.Restart();
                _client.Reset();
                OnRaiseDetectionEvent(_detectionArgs);
            }
        }

        void OnRaiseDetectionEvent(DetectionEventArgs e)
        {
            RaiseDetectionEvent?.Invoke(this, e);
        }
    }
}
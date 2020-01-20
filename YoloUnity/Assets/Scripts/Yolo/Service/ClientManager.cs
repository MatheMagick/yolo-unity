using UnityEngine;
using Grpc.Core;
using System;
using System.Diagnostics;

namespace Yolo
{
    public class ClientManager : IDisposable
    {
      private readonly HiResScreenShots screenShotService;
      public event EventHandler<DetectionEventArgs> RaiseDetectionEvent;
        DetectionEventArgs detectionEventArgs; // re-use

        Channel channel;
        ClientWrapper client;
        YoloResult result; // re-use, reference

        Stopwatch timer;
        const int minInterval = 1000; // throttle requests
        bool requestEnabled => timer.ElapsedMilliseconds >= minInterval;

        public ClientManager(HiResScreenShots screenShotService)
        {
          this.screenShotService = screenShotService;
          channel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            client = new ClientWrapper(new YoloService.YoloServiceClient(channel));

            result = new YoloResult();
            detectionEventArgs = new DetectionEventArgs(result);

            timer = new Stopwatch();
            timer.Start();
        }

        public void Dispose()
        {
            channel.ShutdownAsync();
        }

        public void Update(Camera camera, Vector2Int size)
        {
            if (client.IsIdle)
            {
                if (requestEnabled)
                {
                    timer.Restart();
                    result.Clear();
                    var image = screenShotService.GetScreenShot(camera, size);
                    client.Detect(image, result);
                    // TODO Modification
                    //client.Detect(ImageConversion.EncodeToPNG(texture), result);
                }
            }
            else if (client.HasNewResponse)
            {
                UnityEngine.Debug.Log(string.Format("Detection time: {0}ms, Roundtrip time: {1}ms",
                   result.ElapsedMilliseconds, timer.Elapsed.Milliseconds));

                timer.Restart();
                client.Reset();
                OnRaiseDetectionEvent(detectionEventArgs);
            }
        }

        void OnRaiseDetectionEvent(DetectionEventArgs e)
        {
            EventHandler<DetectionEventArgs> handler = RaiseDetectionEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class DetectionEventArgs : EventArgs
    {
        public YoloResult Result { get; private set; }

        public DetectionEventArgs(YoloResult result)
        {
            Result = result;
        }
    }
}
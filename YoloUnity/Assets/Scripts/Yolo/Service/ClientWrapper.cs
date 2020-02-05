using Grpc.Core;
using Google.Protobuf;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Yolo
{
    public class ClientWrapper
    {
        private readonly YoloService.YoloServiceClient _client;

        public ClientWrapper(YoloService.YoloServiceClient client) => this._client = client;

        enum State
        {
          Idle,
          Busy,
          NewResponse
        }

        private State _state = State.Idle;
        public bool IsIdle => _state == State.Idle;
        public bool IsBusy => _state == State.Busy;
        public bool HasNewResponse => _state == State.NewResponse;

        public void Reset()
        {
            _state = State.Idle;
        }

        public async Task Detect(byte[] imageData, YoloResult result)
        {
            try
            {
                _state = State.Busy;
                DetectionRequest request = new DetectionRequest { Image = ByteString.CopyFrom(imageData) };

                using (var call = _client.Detect(request))
                {
                    var responseStream = call.ResponseStream;
                    while (await responseStream.MoveNext())
                    {
                        DetectionResponse response = responseStream.Current;
                        foreach (DetectionResult r in response.YoloItems)
                        {
                            result.Add(new YoloItem(r.Type, r.Confidence, r.GetX(), r.GetY(), r.GetZ(), r.Width, r.Height));
                        }
                        result.ElapsedMilliseconds = response.ElapsedMilliseconds;
                        _state = State.NewResponse;
                    }
                }
            }
            catch (RpcException e)
            {
                UnityEngine.Debug.LogError("RPC failed " + e);
                throw;
            }
        }
    }
}

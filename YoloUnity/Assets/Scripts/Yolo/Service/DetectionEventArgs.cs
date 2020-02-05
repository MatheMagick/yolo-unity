using System;

namespace Yolo
{
  public class DetectionEventArgs : EventArgs
  {
    public YoloResult Result { get; private set; }

    public DetectionEventArgs(YoloResult result)
    {
      Result = result;
    }
  }
}
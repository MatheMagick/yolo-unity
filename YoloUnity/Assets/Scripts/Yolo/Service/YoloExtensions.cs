namespace Yolo
{
  internal static class YoloExtensions
  {
    public static int GetX(this DetectionResult result)
    {
      return result.X;
    }
    public static int GetY(this DetectionResult result)
    {
      return result.Y;
    }
    public static float GetZ(this DetectionResult result)
    {
      return Position;
    }

    public static float Position { get; set; }
  }
}
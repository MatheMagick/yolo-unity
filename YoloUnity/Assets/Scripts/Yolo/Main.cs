using System.Linq;
using UnityEngine;

namespace Yolo
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        float confidenceThreshold = 0;
        public float warningDistance = 100;

        ClientManager clientManager;
        SizeConfig sizeConfig;
        Texture2D texture;
        Monitor monitor;
        Cam cam;
        private Vector2Int _size;
        private GameObject _targetIndicatorObject;
        private GameObject _plane;

        public void Initialize()
        {
          _targetIndicatorObject = FindObjectsOfType<GameObject>().First(x => x.tag == "target");
          _plane = FindObjectsOfType<GameObject>().First(x => x.tag == "plane");
            sizeConfig = GetComponent<SizeConfig>();
            sizeConfig.RaiseResizeEvent += OnScreenResize;
            Size size = sizeConfig.Initialize();
            _size = size.Image;
            texture = new Texture2D(size.Image.x, size.Image.y, TextureFormat.RGB24, false);
            cam = GameObject.FindObjectOfType<Cam>();
            cam.Initialize(ref texture, size);

            monitor = GameObject.FindObjectOfType<Monitor>();
            monitor.Initialize(size, LabelColors.CreateFromJSON(Resources.Load<TextAsset>("LabelColors").text));

            var hiResScreenShots = new HiResScreenShots();
            clientManager = new ClientManager(hiResScreenShots);
            clientManager.RaiseDetectionEvent += OnDetection;
        }

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            var camera = Camera.main;
            clientManager.Update(camera, _size);
        }

        void OnDetection(object sender, DetectionEventArgs e)
        {
            var yoloItems = e.Result.ToList(confidenceThreshold);
            var yoloAeroplaneItem = yoloItems.FirstOrDefault(x => x.Type.StartsWith("aero"));

            if (yoloAeroplaneItem != null)
            {
                var xRatio = (float) Camera.main.scaledPixelWidth / _size.x;
                var yRatio = (float) Camera.main.scaledPixelHeight / _size.y;
                var yoloPlanePositionOnScreen = yoloAeroplaneItem.Rect.center;
                var yoloGuessedPosition = Camera.main.ScreenToWorldPoint(new Vector3(yoloPlanePositionOnScreen.x * 2 * xRatio, yoloPlanePositionOnScreen.y * yRatio, yoloAeroplaneItem.Depth + 4));

                // Set the red target sphere to recognized plane coordinates.
                // Y coordinate is flipped because canvas starts at top
                // Depth (Z) is -4 so it is displayed in front of the plane, not potentially inside it
                _targetIndicatorObject.transform.position = new Vector3(yoloGuessedPosition.x, -yoloGuessedPosition.y, yoloGuessedPosition.z - 4);
            }

            monitor.UpdateLabels(yoloItems, warningDistance);
        }

        void OnScreenResize(object sender, ResizeEventArgs e)
        {
            _size = e.Size.Image;
            texture.Resize(e.Size.Image.x, e.Size.Image.y);
            monitor.SetSize(e.Size);
            cam.SetSize(e.Size);
        }

        void OnApplicationQuit()
        {
            sizeConfig.RaiseResizeEvent -= OnScreenResize;
            clientManager.RaiseDetectionEvent -= OnDetection;
            clientManager.Dispose();
        }
    }
}
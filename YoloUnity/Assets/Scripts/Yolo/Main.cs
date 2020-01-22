using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// NOTE: Comment out BindService method in YoloServiceGrpc.cs, lines 91-94

namespace Yolo
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        float confidenceThreshold = 0;

        ClientManager clientManager;
        SizeConfig sizeConfig;
        Texture2D texture;
        Monitor monitor;
        Cam cam;
        private Vector2Int _size;
        private GameObject _targetPosition;

        public void Initialize()
        {
          _targetPosition = FindObjectsOfType<GameObject>().First(x => x.tag == "kur");
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
      //var _otherSphere = FindObjectsOfType<GameObject>().First(x => x.tag == "imageControl");
      //var typeoft = _otherSphere.GetType();
      //    var image = FindObjectsOfType<Image>();
            clientManager.Update(camera, _size);
        }

        void OnDetection(object sender, DetectionEventArgs e)
        {
          var yoloItems = e.Result.ToList(confidenceThreshold);
          var aeroplane = yoloItems.FirstOrDefault(x => x.Type.StartsWith("aero"));
          if (aeroplane != null)
          {
            var xRatio = (float) Camera.main.scaledPixelWidth / _size.x;
            var yRatio = (float) Camera.main.scaledPixelHeight / _size.y;
            var aeroPlanePositionOnScreen = aeroplane.Rect.center;
            var planePosition =
              Camera.main.ScreenToWorldPoint(new Vector3(aeroPlanePositionOnScreen.x * 2 * xRatio, aeroPlanePositionOnScreen.y * yRatio, _targetPosition.transform.position.z + 4));
          }

          monitor.UpdateLabels(yoloItems);
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
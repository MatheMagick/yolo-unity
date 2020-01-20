using UnityEngine;
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

        public void Initialize()
        {
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
            monitor.UpdateLabels(e.Result.ToList(confidenceThreshold));
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
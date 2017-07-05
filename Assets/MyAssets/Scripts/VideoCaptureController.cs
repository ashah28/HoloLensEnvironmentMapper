using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.VR.WSA.WebCam;
using UnityEngine.UI;

public class VideoCaptureController : MonoBehaviour
{
    [SerializeField]
    Text commonLogger;

    static readonly float MaxRecordingTime = 20.0f;

    VideoCapture m_VideoCapture = null;
    float m_stopRecordingTimer = float.MaxValue;

    // Use this for initialization
    void Start()
    {
        if (Application.platform == RuntimePlatform.WSAPlayerX86)
            StartVideoCaptureTest();
    }

    void Update()
    {
        if (m_VideoCapture == null || !m_VideoCapture.IsRecording)
        {
            return;
        }

        if (Time.time > m_stopRecordingTimer)
        {
            m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
        }
    }

    void StartVideoCaptureTest()
    {
        commonLogger.text = ("Cam Init\n") + commonLogger.text;
        Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        commonLogger.text = (cameraResolution) + commonLogger.text;

        float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();
        commonLogger.text = (cameraFramerate) + commonLogger.text;

        VideoCapture.CreateAsync(false, delegate (VideoCapture videoCapture)
        {
            if (videoCapture != null)
            {
                m_VideoCapture = videoCapture;
                commonLogger.text = ("Created VideoCapture Instance!\n") + commonLogger.text;

                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.frameRate = cameraFramerate;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                m_VideoCapture.StartVideoModeAsync(cameraParameters,
                                                   VideoCapture.AudioState.ApplicationAndMicAudio,
                                                   OnStartedVideoCaptureMode);
            }
            else
            {
                commonLogger.text = ("Failed to create VideoCapture Instance!\n") + commonLogger.text;
            }
        });
    }

    void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        commonLogger.text = ("Started Video Capture Mode!\n") + commonLogger.text;
        string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
        string filename = string.Format("TestVideo_{0}.mp4", timeStamp);
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        filepath = filepath.Replace("/", @"\");
        m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Video Capture Mode!");
        commonLogger.text = ("Stopped Video Capture Mode!\n") + commonLogger.text;
    }

    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Recording Video!");
        commonLogger.text = ("Started Recording Video!\n") + commonLogger.text;
        m_stopRecordingTimer = Time.time + MaxRecordingTime;
    }

    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Recording Video!");
        commonLogger.text = ("Stopped Recording Video!\n") + commonLogger.text;
        m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }


    public void ButtonClicked()
    {
        commonLogger.text = ("Button clicked\n") + commonLogger.text;
    }
}
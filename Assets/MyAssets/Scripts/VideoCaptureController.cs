using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.VR.WSA.WebCam;
using UnityEngine.UI;

public class VideoCaptureController : MonoBehaviour
{
    static readonly float MaxRecordingTime = 20.0f;

    public VideoCapture m_VideoCapture = null;
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

        //if (Time.time > m_stopRecordingTimer)
        //{
        //    m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
        //}
    }

    void StartVideoCaptureTest()
    {
        DebugManager.Instance.PrintToInfoLog("Cam Init");
        Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        DebugManager.Instance.PrintToInfoLog(cameraResolution.ToString());

        float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();
        DebugManager.Instance.PrintToInfoLog("Res: " + cameraResolution + ", fps:" + cameraFramerate);

        VideoCapture.CreateAsync(false, delegate (VideoCapture videoCapture)
        {
            if (videoCapture != null)
            {
                m_VideoCapture = videoCapture;
                DebugManager.Instance.PrintToInfoLog("Created VideoCapture Instance!");

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
                DebugManager.Instance.PrintToInfoLog("Failed to create VideoCapture Instance!");
            }
        });
    }

    public void StartRecording()
    {
        DebugManager.Instance.PrintToInfoLog("Start Video Clicked!\n");
        string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
        string filename = string.Format("TestVideo_{0}.mp4", timeStamp);
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        filepath = filepath.Replace("/", @"\");
        m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
    }

    public void StopRecording()
    {
        DebugManager.Instance.PrintToInfoLog("Stop Video Clicked");
        m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
    }

    void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        DebugManager.Instance.PrintToInfoLog("Started Video Capture Mode!");
        //string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
        //string filename = string.Format("TestVideo_{0}.mp4", timeStamp);
        //string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        //filepath = filepath.Replace("/", @"\");
        //m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Video Capture Mode!");
        DebugManager.Instance.PrintToInfoLog("Stopped Video Capture Mode!\n");
    }

    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Recording Video!");
        DebugManager.Instance.PrintToInfoLog("Started Recording Video!\n");
        m_stopRecordingTimer = Time.time + MaxRecordingTime;
    }

    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Recording Video!");
        DebugManager.Instance.PrintToInfoLog("Stopped Recording Video!\n");
        m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }


    public void ButtonClicked()
    {
        DebugManager.Instance.PrintToInfoLog("Button clicked\n");
    }
}
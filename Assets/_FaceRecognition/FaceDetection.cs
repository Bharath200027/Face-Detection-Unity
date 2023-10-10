using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetection : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    private CascadeClassifier cascade;
    private OpenCvSharp.Rect firstFace;

    void Start()
    {
        // setup webcam feed
        webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        webCamTexture.Play();
        //GetComponent<Renderer>().material.mainTexture = webCamTexture;

        // initialize OpenCV
        cascade = new CascadeClassifier(Application.streamingAssetsPath + @"/haarcascade_frontalface_default.xml");
    }

    void Update()
    {
        Mat frame = OpenCvSharp.Unity.TextureToMat(webCamTexture);
        findNewFace(frame);
        displayWebCam(frame);
    }

    private void findNewFace(Mat frame)
    {
        var faces = cascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);
        firstFace = (faces.Length >= 1) ? faces[0] : OpenCvSharp.Rect.Empty;
    }

    private void displayWebCam(Mat frame)
    {
        if (firstFace != OpenCvSharp.Rect.Empty)
        {
            frame.Rectangle(firstFace, new Scalar(250, 0, 0), 2);
        }

        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newTexture;
    }
}

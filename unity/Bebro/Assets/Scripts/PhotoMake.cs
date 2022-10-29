using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Photomake : MonoBehaviour
{
    public RawImage _CameraImage;
    public Texture2D _TextureImage;
    public void Photo()
    {
        Camera Cam = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();


        _TextureImage.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        _TextureImage.Apply();
        _CameraImage.texture = _TextureImage;
    }
}


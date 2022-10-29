using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoMake : MonoBehaviour
{
    public RawImage _CameraImage;
    public void Photo()
    {
        Camera Cam = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();

        Texture2D texture = new Texture2D(currentRT.width, currentRT.height);

        texture.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        texture.Apply();
        _CameraImage.texture = texture;
    }
}


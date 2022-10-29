using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMake : MonoBehaviour
{
    // Start is called before the first frame update
    static public Texture2D GetRTPixels(RenderTexture rt)
    {
        //_PhotoCamera.GetComponent<Camera>().Render();
        //_MakedPhoto.texture = _PhotoCamera.activeTexture;
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);

        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoMake : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera _PhotoCamera;
    public RawImage _MakedPhoto;
    public void MakePhoto()
    {
        _PhotoCamera.GetComponent<Camera>().Render();
        _MakedPhoto.texture = _PhotoCamera.activeTexture;
    }
}

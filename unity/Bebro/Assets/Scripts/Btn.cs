using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Btn : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRealese;
    GameObject presser;
    bool isPressed;
    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, -0.317f, 0.015f)
;
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0,-0.3f, 0.015f);
            onRealese.Invoke();
            isPressed = false;
        }
    }

    public void WwqWwwEeeEewQwe()
    {

    }
}

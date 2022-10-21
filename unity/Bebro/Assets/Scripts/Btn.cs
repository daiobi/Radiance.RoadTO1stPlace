using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rover;
using System;

public class Btn : MonoBehaviour
{
    [SerializeField] private float _yPressed = -0.0525f;
    [SerializeField] private float _yReleased = -0.0397f;

    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRealese;
    GameObject presser;
    bool isPressed;
    public AudioSource sound;
    private int TrackIndex;
    public AudioClip[] audioTracks;
    private bool Vpered = false;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
        sound = GetComponent<AudioSource>();
        TrackIndex = 0;
        print(audioTracks);
        print(TrackIndex);
        sound.clip = audioTracks[TrackIndex];
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, _yPressed, button.transform.localPosition.z)
;
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            presser = null;
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, _yReleased, button.transform.localPosition.z);
            onRealese.Invoke();
            isPressed = false;
        }
    }

    public void TrackSwap()
    {
        if (Vpered == true)
        {
            TrackIndex++;
            UpdateTrack(TrackIndex);
            sound.Play();
        }
        else
        {
            TrackIndex--;
            UpdateTrack(TrackIndex);
            sound.Play();
        }
    }

    private void Update()
    {
        if (TrackIndex == audioTracks.Length)
        {
            Vpered = !Vpered;
            print("122434");
        }
    }

    private void UpdateTrack(int Index)
    {
        print($"123: {TrackIndex}");
        sound.clip = audioTracks[Index];
    }

    public void RadioOn()
    {
        sound.Play();
    }
}

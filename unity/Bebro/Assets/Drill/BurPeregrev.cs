using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurPeregrev : MonoBehaviour
{
    public GameObject Bur;
    private float timer = 0;
    private float _maxTime = 10f;
    public ParticleSystem _DustDrill;
    private bool _overheat = false;
    private bool _buttonPressed = false;

    private void OnTriggerStay(Collider other)
    {
        //if()
        _buttonPressed = true;
        _DustDrill.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        _buttonPressed = false;
        _DustDrill.Stop();
    }

    private void Update()
    {
        if (_overheat)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
                _overheat = false;
                Debug.Log("Остыл");
            }
        }
        else
        {
            if (_buttonPressed)
            {
                Debug.Log(timer + "Time");
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }

            if (timer > _maxTime)
            {
                _overheat = true;
                Debug.Log("Перегрев");
            }
        }

        
    }

    public void OnButtonPress()
    {
        _DustDrill.Play();
        _buttonPressed = true;
    }

    public void OnButtonRelease()
    {
        _DustDrill.Stop();
        _buttonPressed = false;
    }
}

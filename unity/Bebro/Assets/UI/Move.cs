using Rover;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public Image _moveImage;
    private int _CalibVal = 0;
    [SerializeField] private Image _target;
    [SerializeField] private float _trueDistance;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;

    public int CalibVal => _CalibVal;

    public UnityEvent OnWin;
    public UnityEvent OnFail;

    private float _min;
    private float _max;
    private RadarMinigame _minigame;

    private float _targetX = 405;
    private bool _th = false;
    // Start is called before the first frame update
    void Start()
    {
        _min = _leftBorder.transform.localPosition.x;
        _max = _rightBorder.transform.localPosition.x;
        _targetX = _max;
        //_target.transform.localPosition = new Vector2(Random.Range(_min, _max), _target.transform.localPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        _th = Mathf.Abs(_target.transform.localPosition.x - _moveImage.transform.localPosition.x) / (_max - _min) < _trueDistance;

        if (_moveImage.transform.localPosition.x == _max)
        {
            _targetX = _min;
        }
        else if (_moveImage.transform.localPosition.x == _min)
        {
            _targetX = _max;
        }


        _moveImage.transform.localPosition = new Vector2(Mathf.MoveTowards(_moveImage.transform.localPosition.x, _targetX, (_max - _min) / 2 *Time.deltaTime), _moveImage.transform.localPosition.y);
        
    }

    internal void Restart()
    {
        GameStatistics.Instance.RadarFixAttemts++;
        _CalibVal = 0;
    }

    public void btn()
    {
        if (_th)
        {
            _CalibVal++;

            if (_CalibVal >= 3) OnWin?.Invoke();
        }
        else
        {
            OnFail?.Invoke(); 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("12332432332");
        if(collision.tag == "TriggerImg")
        {
            _th = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TriggerImg")
        {
            _th = false;
        }
    }
}

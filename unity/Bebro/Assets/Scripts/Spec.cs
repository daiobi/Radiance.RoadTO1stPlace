using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rover;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Spec : MonoBehaviour
{
    [SerializeField] private Rover.Wheel _wheel;
    [SerializeField] private Rover.Rover _rover;
    [SerializeField] private int _dst;
    public Text _WheelDest;
    public GameObject _gmobj;
    public GameObject _rov;
    

    private void Update()
    {
        var tel = _rover.GetTelemetry();
        List<string> a = new List<string>();
        a.Add($"Оставшийся заряд: {(int)tel.Battery}\n");
        a.Add($"Поломок: {tel.HitCount}\n");
        a.Add($"Скорость: {(int)tel.Speed} км/ч\n");
        a.Add($"До ровера: {(int)Vector3.Distance(_gmobj.transform.position, _rov.transform.position)} м\n");
        if (tel.BodyBroken)
            a.Add("Корпус сломан\n");
        if (tel.LFBroken)
            a.Add("1 колесо сломано\n");
        if (tel.RFBroken)
            a.Add("4 колесо сломано\n");
        if (tel.LCBroken)
            a.Add("2 колесо сломано\n");
        if (tel.RCBroken)
            a.Add("5 колесо сломано\n");
        if (tel.LBBroken)
            a.Add("3 колесо сломано\n");
        if (tel.RBBroken)
            a.Add("6 колесо сломано\n");
        if (tel.LBBroken && tel.RFBroken && tel.LCBroken && tel.RCBroken && tel.LBBroken && tel.RBBroken)
        { 
            _rover.TurnOff();
            Debug.Log("Игра окончена");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        _WheelDest.text = string.Join("", a);

        
    }
    
    


    //using Rover;
    //[SerializeField] private Rover.Rover _rover;
    //_rover.RepairWheel(n);
}

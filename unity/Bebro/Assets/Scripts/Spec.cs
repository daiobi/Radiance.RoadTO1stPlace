using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rover;

public class Spec : MonoBehaviour
{
    [SerializeField] private Rover.Wheel _wheel;
    [SerializeField] private Rover.Rover _rover;
    public GameObject leveler;
    public Text _WheelDest;
    private void Start()
    {
        
    }
    private void Update()
    {
        var tel = _rover.GetTelemetry();
        List<string> a = new List<string>();
        a.Add($"Оставшийся заряд: {(int)tel.Battery}\n");
        a.Add($"Поломок: {tel.HitCount}\n");
        a.Add($"Скорость: {(int)tel.Speed} км/ч\n");
        if (tel.LFBroken)
            a.Add("LeftForward broken\n");
        if (tel.RFBroken)
            a.Add("RightForward broken\n");
        if (tel.LCBroken)
            a.Add("LeftCenter broken\n");
        if (tel.RCBroken)
            a.Add("RightCenter broken\n");
        if (tel.LBBroken)
            a.Add("LeftBackward broken\n");
        if (tel.RBBroken)
            a.Add("RightBackward broken\n");






        _WheelDest.text = string.Join("", a);
    }


    //using Rover;
    //[SerializeField] private Rover.Rover _rover;
    //_rover.RepairWheel(n);
}

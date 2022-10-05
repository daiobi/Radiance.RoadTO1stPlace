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
        
        _WheelDest.text = $"Повреждено: {name}";
    }


    //using Rover;
    //[SerializeField] private Rover.Rover _rover;
    //_rover.RepairWheel(n);
}

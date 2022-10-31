using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTools : MonoBehaviour
{
    public enum Tool
    {
        Wheels,
        Grab,
        Drill
    }

    public Tool Selected { get; private set; }

    void Start()
    {
        Selected = Tool.Wheels;
    }

    public void DrillActive()
    {
        if (Selected == Tool.Drill)
        {
            Selected = Tool.Wheels;
        }
        else
        {
            Selected = Tool.Drill;
        }
    }

    public void ArmActive()
    {
        if (Selected == Tool.Grab)
        {
            Selected = Tool.Wheels;
        }
        else
        {
            Selected = Tool.Grab;
        }
    }
}

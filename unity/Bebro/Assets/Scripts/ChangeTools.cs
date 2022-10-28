using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTools : MonoBehaviour
{

    private bool _drill;
    private bool _arm;
    private bool _rover_;
    // Start is called before the first frame update
    void Start()
    {
        _drill = false;
        _arm = false;
        _rover_ = false;
    }

    // Update is called once per frame
    public void DrillActive()
    {
        _drill = !_drill;
        _arm = false;
        _rover_ = false;
    }

    public void ArmActive()
    {
        _arm = !_arm;
        _drill = false;
        _rover_ = false;
    }

    public void RoverActive()
    {
        _rover_ = !_rover_;
        _drill = false;
        _arm = false;
    }

    private void Update()
    {
        if (_rover_ == true)
        {
            ///
        }
        else if (_arm == true)
        {
            ///
        }

        else if(_drill == true)
        {
            ///
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTools : MonoBehaviour
{

    private bool _drill;
    private bool _arm;
    //private bool _rover_;
    // Start is called before the first frame update
    void Start()
    {
        _drill = false;
        _arm = false;
    }

    // Update is called once per frame
    public void DrillActive()
    {
        _drill = !_drill;
        _arm = false;
    }

    public void ArmActive()
    {
        _arm = !_arm;
        _drill = false;
    }

    //public void RoverActive()
    //{
    //    _rover_ = !_rover_;
    //    _drill = false;
    //    _arm = false;
    //}

    private void Update()
    {
        if (_arm == false && _drill == false)
        {
            /// Управление ровером
        }
        else if (_arm == true)
        {
            /// Управление рукой
        }

        else if(_drill == true)
        {
            /// Управление буром
        }
    }
}

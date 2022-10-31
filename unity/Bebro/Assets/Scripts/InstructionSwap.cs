using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSwap : MonoBehaviour
{
    public Material[] _Instruction;
    public MeshRenderer _image;
    int n = 0;
    
    // Start is called before the first frame update

    // Update is called once per frame
    public void SwapPlus()
    {
        if (n < _Instruction.Length-1)
        {
            n++;
            _image.material = _Instruction[n];
        }
    }
    public void SwapMinus()
    {
            if (n > 0)
            {
                n--;
                _image.material = _Instruction[n];
            }
    }

       
}

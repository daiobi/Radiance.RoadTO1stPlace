using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubesEat : MonoBehaviour
{
    public GameObject _Tubes;
    public Mesh _NoEat;
    public Mesh _Eat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("1234535");
            //_NoEat. = _Eat;
        }
            
    }
}

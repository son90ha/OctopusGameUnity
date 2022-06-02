using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{   
    public Transform orderLayout;
    // Start is called before the first frame update
    void Start()
    {
        Game.inst.AddNewCustomer(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

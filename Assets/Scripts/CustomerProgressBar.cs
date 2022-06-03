using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerProgressBar : MonoBehaviour
{   
    public Transform fill;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setProgress(float percent)
    {
        fill.localScale = new Vector3(Mathf.Clamp(percent, 0, 1), 1, 1);
    }
}

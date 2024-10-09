using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSky : MonoBehaviour
{
    public float vSecond;
    private bool night = false;

    public float fog;

    public float nightFog;
    private float dayFog;
    private float currentFog;

    void Start()
    {
        dayFog = RenderSettings.fogDensity;        
    }

    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * vSecond * Time.deltaTime);

        if(transform.eulerAngles.x >= 170)
        {
            night = true;                        
        }
        else if(transform.eulerAngles.x <= 160)
        {
            night = false;                        
        }

        if(night)
        {
            if(currentFog <= nightFog)
            {
                currentFog += 0.1f * fog * Time.deltaTime;
                RenderSettings.fogDensity = currentFog;
            }
        }
        else
        {
            if(currentFog >= dayFog)
            {
                currentFog -= 0.1f * fog * Time.deltaTime;
                RenderSettings.fogDensity = currentFog;
            }
        }
        
    }
    
}

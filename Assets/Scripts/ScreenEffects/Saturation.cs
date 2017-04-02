using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saturation : AScreenEffect 
{
    public void SetSaturation(float val)
    {
        usedMaterial.SetFloat("_Factor", val);
    }

    protected override void BeforeRenderImage()
    {
        
    }

    public float GetSaturation()
    {
        return usedMaterial.GetFloat("_Factor");
    }
}

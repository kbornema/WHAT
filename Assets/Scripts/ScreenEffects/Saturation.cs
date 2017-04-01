using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saturation : AScreenEffect 
{
    public void SetSaturation(float val)
    {
        usedMaterial.SetFloat("_Saturation", val);
    }

    protected override void BeforeRenderImage()
    {
        
    }
}

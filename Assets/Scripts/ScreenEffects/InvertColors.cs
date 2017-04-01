using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertColors : AScreenEffect
{
    public void SetInvertFactor(float factor)
    {
        materialPrefab.SetFloat("_Factor", factor);
    }

    protected override void BeforeRenderImage()
    {   
        
    }
}

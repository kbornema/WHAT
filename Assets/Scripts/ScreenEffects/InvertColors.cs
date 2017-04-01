using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertColors : AScreenEffect
{
    public enum Cut { _CutLeft, _CutRight, _CutUp, _CutDown }

    public void SetInvertFactor(float factor)
    {
        usedMaterial.SetFloat("_Factor", factor);
    }

    public void SetCut(Cut cutDir, float cut)
    {
        usedMaterial.SetFloat(cutDir.ToString(), cut);
    }

    public float GetCut(Cut cutDir)
    {
        return usedMaterial.GetFloat(cutDir.ToString());
    }

    protected override void BeforeRenderImage()
    {   
        
    }
}

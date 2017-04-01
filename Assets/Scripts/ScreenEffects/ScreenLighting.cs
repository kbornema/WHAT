using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenLighting : AScreenEffect
{
    [SerializeField]
    private Camera myCam;

    [SerializeField]
    private Camera lightCam;
    private RenderTexture lightTexture;



    protected override void Start() 
    {
        base.Start();

        if (!myCam)
            myCam = GetComponent<Camera>();

        SetRenderTexture();
	}

    private void SetRenderTexture()
    {
        lightTexture = lightCam.targetTexture;
        usedMaterial.SetTexture("_MultTex", lightTexture);
    }

    protected override void BeforeRenderImage()
    {
        Debug.Assert(usedMaterial);

        if (lightTexture != lightCam.targetTexture)
        {
            SetRenderTexture();
        }
    }
}

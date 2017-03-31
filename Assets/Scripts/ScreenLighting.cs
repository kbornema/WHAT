using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenLighting : MonoBehaviour 
{
    [SerializeField]
    private Camera myCam;

    [SerializeField]
    private Camera lightCam;
    private RenderTexture lightTexture;

    [SerializeField]
    private Material lightingMaterialPrefab;
    private Material lightingMaterialInstance;

	void Start () 
    {
        if (!myCam)
            myCam = GetComponent<Camera>();

        lightingMaterialInstance = Instantiate(lightingMaterialPrefab);

        SetRenderTexture();
	}

    private void SetRenderTexture()
    {
        lightTexture = lightCam.targetTexture;
        lightingMaterialInstance.SetTexture("_MultTex", lightTexture);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Debug.Assert(lightingMaterialInstance);

        if(lightTexture != lightCam.targetTexture)
        {
            SetRenderTexture();
        }

        Graphics.Blit(src, dest, lightingMaterialInstance);
    }
}

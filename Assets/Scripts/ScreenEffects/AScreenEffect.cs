using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AScreenEffect : MonoBehaviour 
{
    [SerializeField]
    protected Material materialPrefab;
    protected Material usedMaterial;

    [SerializeField]
    protected bool applyEffect = true;
    public bool ApplyEffect { get { return applyEffect; } set { applyEffect = value; } }

    protected virtual void Start()
    {
        if (GameManager.Instance.GameCam.InstantiateImageEffects)
        {
            usedMaterial = Instantiate(materialPrefab);
        }

        else
            usedMaterial = materialPrefab;
    }

    protected void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        BeforeRenderImage();

        if (applyEffect)
        {
            Debug.Assert(usedMaterial);
            Graphics.Blit(src, dest, usedMaterial);
        }

        else
            Graphics.Blit(src, dest);
    }

    protected abstract void BeforeRenderImage();
}

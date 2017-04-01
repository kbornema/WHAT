using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour 
{
    [SerializeField]
    private bool instantiateImageEffects = true;
    public bool InstantiateImageEffects { get { return instantiateImageEffects; } }

    [SerializeField]
    private Camera cam;
    public Camera Cam { get { return cam; } }

    [SerializeField]
    private Camera lightCam;
    public Camera LightCam { get { return lightCam; } }

    [SerializeField]
    private InvertColors invertColors;
    public InvertColors InvColors { get { return invertColors; } }


}

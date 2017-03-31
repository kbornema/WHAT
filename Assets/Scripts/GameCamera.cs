using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour 
{
    [SerializeField]
    private Camera cam;
    public Camera Cam { get { return cam; } }
}

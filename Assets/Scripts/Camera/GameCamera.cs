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
    private InvertColors invertCols;
    public InvertColors InvertCols { get { return invertCols; } } 

    private float curShakeDur;
    private float shakePower;

    [SerializeField]
    private float moveSpeed = 0.5f;

    public void Shake(float dur, float strength)
    {
        this.curShakeDur = dur;
        this.shakePower = strength;
    }

    private void LateUpdate()
    {
        if(this.curShakeDur > 0.0f)
        {
            this.curShakeDur -= Time.deltaTime;

            Vector3 pos = gameObject.transform.position;

            Vector2 dir = VecUtil.RandDir() * shakePower * Random.value;

            pos.x += dir.x;
            pos.y += dir.y;

            gameObject.transform.position = pos;
        }

        MoveBackToCenter();
    }

    private void MoveBackToCenter()
    {
        if (gameObject.transform.position.x != 0.0f || gameObject.transform.position.y != 0.0f)
        {
            Vector3 dir = -gameObject.transform.position;
            dir.z = 0.0f;

            gameObject.transform.position += dir * Time.deltaTime * moveSpeed;

            if (Vector2.Distance(gameObject.transform.pos2(), Vector2.zero) < 0.025f)
                gameObject.transform.position = new Vector3(0.0f, 0.0f, gameObject.transform.position.z);
        }
    }

}

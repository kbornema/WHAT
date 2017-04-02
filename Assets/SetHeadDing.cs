using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeadDing : MonoBehaviour {

    [SerializeField]
    Sprite[] textureArray;
    [SerializeField]
    Actor actor;
    SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        actor.onLookDirChanged.AddListener(DerSpiegelOfDoom);

    }

    private void DerSpiegelOfDoom(Actor arg0)
    {
        Vector3 scale = gameObject.transform.localScale;

        if (actor.LookDirection.x < 0.0f)
        {
            scale.x = -Mathf.Abs(scale.x);
            gameObject.transform.localScale = scale;
        }

        else if (actor.LookDirection.x > 0.0f)
        {
            scale.x = +Mathf.Abs(scale.x);
            gameObject.transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update () {
		//ToDo Logik mit Bomben
	}

    public void AciveHeadDing(Player.Index PlayerIndex)
    {
        if (PlayerIndex == Player.Index.One && textureArray.Length >= 1)
            spriteRenderer.sprite = textureArray[0];
        if (PlayerIndex == Player.Index.Two && textureArray.Length >= 2)
            spriteRenderer.sprite = textureArray[1];
    }
    public void DeactiveHeadDing(Player.Index PlayerIndex)
    {
        if (PlayerIndex == Player.Index.One && textureArray.Length >= 1)
            spriteRenderer.sprite = null;
        if (PlayerIndex == Player.Index.Two && textureArray.Length >= 2)
            spriteRenderer.sprite = null;
    }
    //public void UpdatePosition(Vector3 pos)
    //{
    //    transform.position = pos;
    //}


}

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
    int curBomb;
    [SerializeField]
    Sprite bombSprite;
    [SerializeField]
    SpriteRenderer[] bombArray;
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

        if (curBomb == 3)
        {
            bombArray[0].sprite = bombSprite;
            bombArray[1].sprite = bombSprite;
            bombArray[2].sprite = bombSprite;
            return;
        }
        if (curBomb == 2)
        {
            bombArray[0].sprite = bombSprite;
            bombArray[1].sprite = bombSprite;
            bombArray[2].sprite = null;
            return;
        }

        if (curBomb == 1)
        {

            bombArray[0].sprite = bombSprite;
            bombArray[1].sprite = null;
            bombArray[2].sprite = null;
            return;
        }


        if (curBomb == 0)
        {
            bombArray[0].sprite = null;
            bombArray[1].sprite = null;
            bombArray[2].sprite = null;
        }



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

    internal void CurBomb(int v)
    {
        Debug.Log(v);
        curBomb = v;
    }
    //public void UpdatePosition(Vector3 pos)
    //{
    //    transform.position = pos;
    //}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeadDing : MonoBehaviour {

    [SerializeField]
    Sprite[] textureArray;

    SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void DerSpiegelOfDoom(Vector3 vec, Player.Index PlayerIndex)
    {
        if(vec.x == -1)
        {
           
        }

    }
}

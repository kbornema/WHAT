using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour 
{
    [SerializeField]
    private float minAlpha;
    [SerializeField]
    private float maxAlpha;
    [SerializeField]
    private float speed;

    private float curTime;

    private SpriteRenderer sprite;
    private Color color;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        color = sprite.color;
        curTime = 0.0f;
    }

	// Update is called once per frame
	void Update () 
    {
        curTime += Time.deltaTime * speed;

        float t = Mathf.Sin(curTime) * 0.5f + 0.5f;

        color.a = Mathf.Lerp(minAlpha, maxAlpha, t);

        sprite.color = color;
	}
}

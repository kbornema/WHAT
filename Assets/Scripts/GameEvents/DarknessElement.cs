using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessElement : MonoBehaviour 
{
    private SpriteRenderer sprite;
    private float normalAlpha;

    [SerializeField]
    private GameObject[] objects;

	// Use this for initialization
	void Start () 
    {
        sprite = GetComponent<SpriteRenderer>();

        if(sprite)
        {
            normalAlpha = sprite.color.a;

            if(!EventManager.Instance.IsDark)
                Set(0.0f);
        }

        if (!EventManager.Instance.IsDark)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(false);
            }
        }
        
        EventManager.Instance.onDarknessArrived.AddListener(OnDarknessArrived);
        EventManager.Instance.onDarknessFading.AddListener(OnDarknessFading);
	}

    private void OnDarknessArrived(AGameEvent eve)
    {
        if (!this.enabled || !gameObject.activeSelf)
            return;

        if (sprite)
            FadeIn(eve);

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
        }
    }

    private void OnDarknessFading(AGameEvent eve)
    {
        if (!this.enabled || !gameObject.activeSelf)
            return;

        if (sprite)
            FadeOut(eve);

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }

    private void FadeOut(AGameEvent eve)
    {
        StartCoroutine(Fade(0.0f));
    }

    private void FadeIn(AGameEvent eve)
    {
        StartCoroutine(Fade(normalAlpha));
    }

    private IEnumerator Fade(float alpha)
    {
        float dur = 0.0f;
        float maxDur = Random.Range(0.5f, 1.0f);

        Color c = sprite.color;
        float startAlpha = c.a;

        float t = 0.0f;

        while(dur < maxDur)
        {
            t = dur / maxDur;
            c.a = Mathf.Lerp(startAlpha, alpha, t);
            sprite.color = c;

            dur += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Set(alpha);
    }

    private void Set(float alpha)
    {
        Color c = sprite.color;

        c.a = alpha;

        sprite.color = c;
    }
	
	
}

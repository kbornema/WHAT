using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessEvent : AGameEvent 
{
    [SerializeField]
    private float darknessFadeInOutTime = 2.0f;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color darknessColor;

    [SerializeField]
    private float spawnCooldownMin = 0.75f;
    [SerializeField]
    private float spawnCooldownMax = 1.5f;
    [SerializeField]
    private int minNumSpawns = 1;
    [SerializeField]
    private int maxNumSpawns = 3;

    protected override void _StartEvent()
    {
        StartCoroutine(DarknessRoutine());
    }

    private IEnumerator DarknessRoutine()
    {
        float duration = Random.Range(minDur, maxDur);

        yield return Fade(darknessColor, darknessFadeInOutTime);

        EventManager.Instance.StartDarkness(this);

        StartCoroutine(AdditionalSpawnsRoutine(duration));

        yield return new WaitForSeconds(duration);

        EventManager.Instance.EndDarkness(this);

        yield return Fade(normalColor, darknessFadeInOutTime);

        EventManager.Instance.EndEvent(this);
    }

    private IEnumerator AdditionalSpawnsRoutine(float dur)
    {
        while (dur > 0.0f)
        {
            dur -= Time.deltaTime;
            int numSpawns = Random.Range(minNumSpawns, maxNumSpawns + 1);

            for (int i = 0; i < numSpawns; i++)
            {
                GameManager.Instance.SpawnNormalTurtle();
                yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
            }
         
            yield return new WaitForSeconds(Random.Range(spawnCooldownMin, spawnCooldownMax));
        }
    }

    private IEnumerator Fade(Color target, float dur)
    {
        Color startColor = GameManager.Instance.GameCam.LightCam.backgroundColor;
        Color curColor = startColor;

        float maxDur = dur;
        float curDur = 0.0f;

        while (curDur < dur)
        {
            float t = curDur / maxDur;

            curColor = Color.Lerp(startColor, target, t);

            GameManager.Instance.GameCam.LightCam.backgroundColor = curColor;

            curDur += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.GameCam.LightCam.backgroundColor = target;
    }

    protected override void _EndEvent()
    {
       
    }

    public override bool GetGameEventWon()
    {
        return true;
    }

    public override void OnGameEventWon()
    {
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMoEvent : AGameEvent 
{
    [SerializeField]
    private float saturationFadeTime = 1.0f;

    [SerializeField]
    private float timeScale = 0.5f;

    protected override void _StartEvent()
    {
        float dur = Random.Range(minDur, maxDur);

        Time.timeScale = timeScale;

        StartCoroutine(EndIn(dur));
    }

    private IEnumerator EndIn(float dur)
    {
        StartCoroutine(Fade(0.0f));
        
        yield return new WaitForSeconds(dur);

        StartCoroutine(Fade(1.0f));

        EventManager.Instance.EndEvent(this);
    }

    private IEnumerator Fade(float val)
    {
        float curDur = 0.0f;
        float t = 0.0f;

        float startVal = GameManager.Instance.GameCam.SaturationEffect.GetSaturation();

        while(curDur < saturationFadeTime)
        {
            t = curDur / saturationFadeTime;

            float curVal = Mathf.Lerp(startVal, val, t);
            GameManager.Instance.GameCam.SaturationEffect.SetSaturation(curVal);

            curDur += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    protected override void _EndEvent()
    {
        Time.timeScale = GameManager.Instance.NormalTimeScale;
    }

    public override bool GetGameEventWon()
    {
        return true;
    }

    public override void OnGameEventWon()
    {
        
    }
}

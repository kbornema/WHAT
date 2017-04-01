using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertEvent : AGameEvent 
{
    [SerializeField]
    private float fadeTime = 1.0f;

    private float[] targetPos = new float[4];
    private float[] offsets = new float[4];
    private float[] end = { 0.0f, 0.0f, 0.0f, 0.0f };
    private bool[] isActive = new bool[4];

    private InvertColors invertColors;
    
    private IEnumerator EventRoutine()
    {
        float dur = Random.Range(minDur, maxDur);
        float curDur = 0.0f;

        float time = 0.0f;

        float[] times = new float[4];
        float[] speeds = new float[4];

        FillValues();

        int numActive = 0;

        for (int i = 0; i < times.Length; i++)
        {
            isActive[i] = Random.value < 0.75f;

            if (isActive[i])
            {
                numActive++;
            }

            times[i] = 0.0f;
            speeds[i] = Random.Range(0.0f, 2.0f);
        }

        if (numActive == 0)
        {
            int id = Random.Range(0, 4);
            isActive[id] = true;
            speeds[id] = Random.Range(0.0f, 2.0f);
        }

        for (int i = 0; i < times.Length; i++)
        {
            if (!isActive[i])
            {
                speeds[i] = 0.0f;
                targetPos[i] = 0.0f;
                offsets[i] = 0.0f;
            }
        }

        yield return Fade(targetPos);
        
        while(curDur < dur)
        {
            for (int i = 0; i < targetPos.Length; i++)
            {
                times[i] += Time.deltaTime * speeds[i];
                invertColors.SetCut((InvertColors.Cut)i, targetPos[i] + offsets[i] * (Mathf.Sin(time + times[i]) * 0.5f));
            }

            time += Time.deltaTime;
            curDur += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return Fade(end);

        EventManager.Instance.EndEvent(this);
    }

    private IEnumerator Fade(float[] targets)
    {
        float t = 0.0f;
        float curDur = 0.0f;

        float[] starts = new float[4];

        for (int i = 0; i < targetPos.Length; i++)
        {
            starts[i] = invertColors.GetCut((InvertColors.Cut)i);
        }


        while (curDur < fadeTime)
        {
            t = curDur / fadeTime;


            for (int i = 0; i < targetPos.Length; i++)
            {
                float val = Mathf.Lerp(starts[i], targets[i], t);
                invertColors.SetCut((InvertColors.Cut)i, val);
            }

            curDur += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        for (int i = 0; i < targetPos.Length; i++)
        {
            invertColors.SetCut((InvertColors.Cut)i, targets[i]);
        }
    }

    private void FillValues()
    {
        for (int i = 0; i < targetPos.Length; i++)
        {
            targetPos[i] = Random.value;
            offsets[i] = Random.value;
        }
    }


    protected override void _StartEvent()
    {
        invertColors = GameManager.Instance.GameCam.InvertCols;

        StartCoroutine(EventRoutine());
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

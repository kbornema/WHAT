using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertEvent : AGameEvent 
{
    [SerializeField]
    private float fadeTime = 1.0f;

    private float[] targetPos = new float[4];
    private float[] offsets = new float[4];

    private InvertColors invertColors;

    

    private IEnumerator EventRoutine()
    {
        float dur = Random.Range(minDur, maxDur);
        float curDur = 0.0f;
        float time = 0.0f;

        FillValues();

        yield return Fade(1.0f);
        
        while(curDur < dur)
        {
            time += Time.deltaTime;



            yield return new WaitForEndOfFrame();
        }

        yield return Fade(0.0f);


        yield return null;
    }

    private IEnumerator Fade(float target)
    {
        float t = 0.0f;
        float dur = 2.0f;
        float curDir = 0.0f;

        float[] starts = new float[4];

        for (int i = 0; i < targetPos.Length; i++)
        {
            starts[i] = invertColors.GetCut((InvertColors.Cut)i);
        }
        

        for (int i = 0; i < targetPos.Length; i++)
        {
            float val = Mathf.Lerp(starts[i], target, t);
            invertColors.SetCut((InvertColors.Cut)i, val);
        }

        yield return null;
    }

    private void FillValues()
    {
        for (int i = 0; i < targetPos.Length; i++)
        {
            targetPos[i] = Random.value;
            offsets[i] = Random.value;
        }
    }

    private object Fade()
    {
        throw new System.NotImplementedException();
    }

    protected override void _StartEvent()
    {
        invertColors = GameManager.Instance.GameCam.InvertCols;
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

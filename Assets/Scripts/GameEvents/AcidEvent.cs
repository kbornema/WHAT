using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidEvent : AGameEvent 
{
    [SerializeField]
    private GameObject acidRoot;

    [SerializeField]
    private float acidLowerPos;
    [SerializeField]
    private float acidUpperPosMin;
    [SerializeField]
    private float acidUpperPosMax;

    protected override void _StartEvent()
    {
        StartCoroutine(AcidRoutine());
    }

    private IEnumerator AcidRoutine()
    {
        float totalDur = Random.Range(minDur, maxDur);
        float curDur = 0.0f;

        float t = 0.0f;

        float maxHeight = Random.Range(acidUpperPosMin, acidUpperPosMax);

        while(curDur < totalDur)
        {
            t = curDur / totalDur;

            float sinusT = Mathf.Sin(t * Mathf.PI);

            float height = Mathf.Lerp(acidLowerPos, maxHeight, sinusT);

            Vector3 pos = acidRoot.transform.position;
            pos.y = height;
            acidRoot.transform.position = pos;

            curDur += Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }

        EventManager.Instance.EndEvent(this);
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

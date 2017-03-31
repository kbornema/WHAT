using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMoveDir : MonoBehaviour {

    void Start()
    {
        GetComponent<Actor>().onMovementChanged.AddListener(OnMovementChanged);
    }

    private void OnMovementChanged(Actor actor)
    {
        Vector3 scale = gameObject.transform.localScale;

        if (actor.MoveDirection.x < 0.0f)
        {
            scale.x = -Mathf.Abs(scale.x);
            gameObject.transform.localScale = scale;
        }

        else if (actor.MoveDirection.x > 0.0f)
        {
            scale.x = Mathf.Abs(scale.x);
            gameObject.transform.localScale = scale;
        }
    }

}

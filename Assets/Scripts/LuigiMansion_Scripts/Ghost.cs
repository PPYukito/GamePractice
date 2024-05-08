using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ghost : MonoBehaviour
{
    public delegate void GhostGetSucked(bool getSucked);
    public GhostGetSucked ghostJustGotScuked;

    [Header("Ghost Setting")]
    public float suckingSpeed = 2.0f;

    [Header("Debugging")]
    public bool isStunned = false;

    [SerializeReference]
    private bool isBeingSuck = false;

    private Transform suckingPoint;

    private void Update()
    {
        if (isBeingSuck)
        {
            if (isStunned)
            {
                // move to sucking point;
                transform.position += (suckingPoint.position - transform.position) * Time.deltaTime/* * suckingSpeed*/;

                if (transform.position == suckingPoint.position)
                    ghostJustGotScuked?.Invoke(true);
            }
        }
    }

    public void SetBeingSuck(bool sucking, Transform suckingPoint = null)
    {
        isBeingSuck = sucking;

        // just for testing;
        if (!isBeingSuck)
            isStunned = false;
        else
        {
            if (suckingPoint)
                this.suckingPoint = suckingPoint;
        }
    }

    public void SetBeingAttack(bool attacking)
    {
        isStunned = attacking;

        if (isStunned)
        {
            DOVirtual.Int(2, 0, 2, TweenCallback)
                .SetEase(Ease.Linear);
        }
    }


    private void TweenCallback(int num)
    {
        // it needs to be put in;
    }
}

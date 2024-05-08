using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ghost : MonoBehaviour
{
    [Header("Ghost Setting")]
    public float suckingSpeed = 2.0f;

    [Header("Debugging")]
    [SerializeReference]
    private bool isBeingAttack = false;

    [SerializeReference]
    private bool isBeingSuck = false;

    private Transform suckingPoint;

    private void Update()
    {
        if (isBeingSuck)
        {
            if (isBeingAttack)
            {
                // move to sucking point;
                transform.position += (suckingPoint.position - transform.position) * Time.deltaTime * suckingSpeed;
            }
        }
    }

    public void SetBeingSuck(bool sucking, Transform suckingPoint = null)
    {
        isBeingSuck = sucking;

        // just for testing;
        if (!isBeingSuck)
            isBeingAttack = false;
        else
        {
            if (suckingPoint)
                this.suckingPoint = suckingPoint;
        }
    }

    public void SetBeingAttack(bool attacking)
    {
        isBeingAttack = attacking;

        if (isBeingAttack)
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

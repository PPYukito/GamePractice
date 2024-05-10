using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ghost : MonoBehaviour
{
    public delegate void GhostGetSucked(bool getSucked);
    public GhostGetSucked ghostJustGotScuked;

    [Header("Ghost Capture Setting")]
    public float suckingSpeed = 1.0f;
    public string captureSphereTag;
    public LayerMask captureSphereLayer;

    [Header("Ghost")]
    public float hp;
    public float heavyDamage;
    public float normalDamage;
    public float distanceToActivateEscape;

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
                Vector3 suckVector = (suckingPoint.position - transform.position);
                Vector3 escapeVector = (transform.position - suckingPoint.position);

                transform.position += suckVector * Time.deltaTime * suckingSpeed;
                transform.forward = escapeVector;

                if (Vector3.Distance(transform.position, suckingPoint.position) <= distanceToActivateEscape)
                {
                    ghostJustGotScuked?.Invoke(true);
                }
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //detect area to get capture
    //    if (other.gameObject.CompareTag(captureSphereTag))
    //    {
    //        if (isBeingSuck && isStunned)
    //            ghostJustGotScuked?.Invoke(true);
    //    }

    //    //I don't undertand how this works, but it works;
    //    //if ( (captureSphereLayer & (1 << other.gameObject.layer)) != 0 )
    //    //{
    //    //    //Call method you want;
    //    //}
    //}

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

    public void Stunned(bool attacking)
    {
        isStunned = attacking;

        if (isStunned)
        {
            DOVirtual.Int(4, 0, 4, TweenCallback)
                .SetEase(Ease.Linear)
                .OnComplete(() => isStunned = false);
        }
    }

    public void TakeDamage(float angle)
    {
        if (angle >= 130)
            hp -= heavyDamage;
        else
            hp -= normalDamage;

        //show to UI
    }

    private void TweenCallback(int num)
    {
        // it needs to be put in;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ghost : MonoBehaviour
{
    public delegate void GhostGetSucked(bool getSucked);
    public GhostGetSucked ghostJustGotScuked;

    [Header("Ghost Setting")]
    public float suckingSpeed = 1.0f;
    public string captureSphereTag;
    public LayerMask captureSphereLayer;

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
                transform.position += (suckingPoint.position - transform.position) * Time.deltaTime * suckingSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //detect area to get capture
        if (other.gameObject.CompareTag(captureSphereTag))
        {
            Debug.Log("Hello");
            if (isBeingSuck && isStunned)
                ghostJustGotScuked?.Invoke(true);
        }

        //I don't undertand how this works, but it works;
        //if ( (captureSphereLayer & (1 << other.gameObject.layer)) != 0 )
        //{
        //    //Call method you want;
        //}
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

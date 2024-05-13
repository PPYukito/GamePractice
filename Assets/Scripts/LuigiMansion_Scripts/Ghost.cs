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
    public GameObject damageText;

    [Header("Ghost")]
    [SerializeField]
    private float hp;
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
        if (hp > 0)
        {
            isBeingSuck = sucking;
            StopAllCoroutines();

            if (!isBeingSuck)
                StartCoroutine(SetCountDownAfterStunned(3.0f));
            else
            {
                if (suckingPoint)
                    this.suckingPoint = suckingPoint;
            }
        }
    }

    public void Stunned(bool attacking)
    {
        isStunned = attacking;

        //show hp
        GeneralInstance.instance.ResetGhostColor(this);
        GeneralInstance.instance.ShowHP(this);

        StartCoroutine(SetCountDownAfterStunned(4.0f));
    }

    public void TakeDamage(float angle)
    {
        if (angle >= 130)
            hp -= heavyDamage;
        else
            hp -= normalDamage;

        //show to UI
        GeneralInstance.instance.ShowHP(this);

        if (hp <= 0)
        {
            isStunned = false;
            isBeingSuck = false;
            transform.parent = null;
            //destroy ghost;
        }
    }

    public float GetHP()
    {
        return hp;
    }

    IEnumerator SetCountDownAfterStunned(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isStunned = false;
        GeneralInstance.instance.FadeGhostHP(this);
    }
}

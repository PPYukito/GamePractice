using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuigiPlayerController : MonoBehaviour
{
    public WeaponControl WeaponControl;
    public RandomRotation randomRotation;

    private bool attackingGhost = false;

    private void Start()
    {
        attackingGhost = false;
        WeaponControl.Init(AttackingGhost);
    }

    private void AttackingGhost(bool attacking)
    {
        if (attackingGhost != attacking)
            attackingGhost = attacking;

        randomRotation.enabled = attacking;
    }
}

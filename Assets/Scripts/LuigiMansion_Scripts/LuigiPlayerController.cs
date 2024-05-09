using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuigiPlayerController : MonoBehaviour
{
    [Header("Controllers")]
    public WeaponControl WeaponControl;
    public RandomMovement RandomMovement;
    public MovementInput MovementInput;

    private bool attackingGhost = false;

    private void Start()
    {
        attackingGhost = false;
        WeaponControl.Init(CapturingGhost, ActivePlayerRotation);
    }

    private void CapturingGhost(bool attacking)
    {
        if (attackingGhost != attacking)
            attackingGhost = attacking;

        if (attacking)
            RandomMovement.StartRandomMovement();
        else
            RandomMovement.StopRandomMovement();
    }

    private void ActivePlayerRotation(bool activeRotate)
    {
        MovementInput.blockRotationPlayer = activeRotate;
    }
}

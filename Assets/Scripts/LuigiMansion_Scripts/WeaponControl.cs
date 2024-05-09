using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponControl : MonoBehaviour
{
    public delegate void AttackingGhost(bool attacking);
    public AttackingGhost attackingGhost;

    public delegate void SetPlayerRotation(bool activeRotate);
    public SetPlayerRotation setPlayerRotate;

    [Header("Transform Settings")]
    public Transform playerTransform;
    public Transform suckingPoint;

    private MyPlayer controls;
    private List<Ghost> listOfGhost;

    //private bool isSucking = false;
    //private bool isBlowing = false;

    private void Start()
    {
        listOfGhost = new List<Ghost>();
        listOfGhost.Clear();
    }

    public void Init(AttackingGhost attackingGhostCallback, SetPlayerRotation setPlayerRotateCallback)
    {
        attackingGhost = attackingGhostCallback;
        setPlayerRotate = setPlayerRotateCallback;
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new MyPlayer();
            controls.Player.Transform.performed += OnFlash; //able to use OnTransform by PlayerInout component for reading input instead of subscibe;
            controls.Player.Fire.performed += OnSucking;
            controls.Player.Fire.canceled += OnStopSucking;
        }

        controls.Player.Enable();
    }

    private void OnDisable()
    {
        listOfGhost.Clear();
        controls.Player.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        Ghost enteredGhost = other.GetComponent<Ghost>();
        if (enteredGhost)
        {
            if (!listOfGhost.Contains(enteredGhost))
            {
                listOfGhost.Add(enteredGhost);
                enteredGhost.ghostJustGotScuked += SetAttackingGhosts;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ghost exitedGhost = other.GetComponent<Ghost>();
        if (exitedGhost)
        {
            if (listOfGhost.Contains(exitedGhost))
            {
                exitedGhost.ghostJustGotScuked -= SetAttackingGhosts;
                listOfGhost.Remove(exitedGhost);
            }
        }
    }

    //private void OnTransform(InputValue value)
    //{
    //    foreach (Ghost ghost in listOfGhost)
    //    {
    //        ghost.SetBeingAttack(true);
    //    }
    //}

    private void OnFlash(InputAction.CallbackContext callback)
    {
        foreach (Ghost ghost in listOfGhost)
        {
            ghost.SetBeingAttack(true);
        }
    }

    private void OnSucking(InputAction.CallbackContext callback)
    {
        //isSucking = true;
        setPlayerRotate?.Invoke(true);

        foreach (Ghost ghost in listOfGhost)
        {
            if (ghost.isStunned)
            {
                ghost.transform.SetParent(playerTransform);
                ghost.SetBeingSuck(true, suckingPoint);
            }
        }
    }

    private void OnStopSucking(InputAction.CallbackContext callback)
    {
        //isSucking = false;
        setPlayerRotate?.Invoke(false);
        SetAttackingGhosts(false);

        foreach (Ghost ghost in listOfGhost)
        {
            ghost.transform.SetParent(null);
            ghost.SetBeingSuck(false);
        }
    }

    private void SetAttackingGhosts(bool setAttack)
    {
        attackingGhost?.Invoke(setAttack);
    }
}

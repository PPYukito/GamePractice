using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponControl : MonoBehaviour
{
    private MyPlayer controls;
    private List<Ghost> listOfGhost;

    private bool isSucking = false;
    private bool isBlowing = false;

    private void Start()
    {
        listOfGhost = new List<Ghost>();
        listOfGhost.Clear();
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new MyPlayer();
            controls.Player.Transform.performed += OnFlash;
            controls.Player.Fire.performed += OnFireStart;
            controls.Player.Fire.canceled += OnFireStop;
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
                listOfGhost.Add(enteredGhost);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ghost exitedGhost = other.GetComponent<Ghost>();
        if (exitedGhost)
        {
            if (listOfGhost.Contains(exitedGhost))
                listOfGhost.Remove(exitedGhost);
        }
    }

    private void OnFlash(InputAction.CallbackContext callback)
    {
        foreach (Ghost ghost in listOfGhost)
        {
            Debug.Log(ghost);
        }
    }

    private void OnFireStart(InputAction.CallbackContext callback)
    {
        isSucking = true;
    }

    private void OnFireStop(InputAction.CallbackContext callback)
    {
        isSucking = false;
    }
}

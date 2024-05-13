using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GeneralInstance : MonoBehaviour
{
    public static GeneralInstance instance;

    [Header("Display Settings")]
    public RectTransform canvas;
    public Camera cam;
    public GameObject ghostHPGameObj;

    [Header("Ghosts")]
    public List<Ghost> listOfGhostInScene;

    //private List<Ghost>
    private Dictionary<Ghost, GhostHealth> ghostDisplays;
    private IDisposable dispose;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;

        ghostDisplays = new Dictionary<Ghost, GhostHealth>();
        foreach (Ghost ghost in listOfGhostInScene)
        {
            GhostHealth ghostHPComp = Instantiate(ghostHPGameObj, canvas).GetComponent<GhostHealth>();
            ghostDisplays.Add(ghost, ghostHPComp);
        }
    }

    private void Update()
    {
        foreach (KeyValuePair<Ghost, GhostHealth> ghostData in ghostDisplays)
        {
            ghostData.Value.transform.position = cam.WorldToScreenPoint(ghostData.Key.transform.position);
        }
    }

    public void ShowHP(Ghost ghost)
    {
        if (ghostDisplays.ContainsKey(ghost))
        {
            int hp = (int)ghost.GetHP();

            if (hp < 100)
                ghostDisplays[ghost].SetHPText(hp.ToString());

            if (hp <= 0)
            {
                GameObject hpObj = ghostDisplays[ghost].gameObject;
                ghostDisplays.Remove(ghost);
                Destroy(hpObj);
            }
        }
    }

    public void ResetGhostColor(Ghost ghost)
    {
        if (ghostDisplays.ContainsKey(ghost))
            ghostDisplays[ghost].ResetColor();
    }

    public void FadeGhostHP(Ghost ghost)
    {
        if (ghostDisplays.ContainsKey(ghost))
            ghostDisplays[ghost].FadeHP();
    }
}

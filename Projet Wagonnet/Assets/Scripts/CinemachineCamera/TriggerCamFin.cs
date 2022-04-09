using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using UnityEngine;

public class TriggerCamFin : MonoBehaviour
{
    private bool playerCamera = true;
    public CinemachineVirtualCamera Player; //Player
    public CinemachineVirtualCamera CamFin; //GroupCamera
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SwitchPriority();
        }
    }


    private void SwitchPriority()
    {
        if (playerCamera)
        {
            Player.Priority = 0;
            CamFin.Priority = 5;
            playerCamera = false;

        }
        else
        {
            Player.Priority = 5;
            CamFin.Priority = 0;
            playerCamera = true;
        }

    }
}

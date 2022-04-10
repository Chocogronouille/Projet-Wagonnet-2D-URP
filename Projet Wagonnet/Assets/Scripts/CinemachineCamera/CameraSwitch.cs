using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    
    public bool playerCamera = true;
    public CinemachineVirtualCamera Player; //Player
    public CinemachineVirtualCamera GroupAttraction; //GroupCamera
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SwitchPriority();
        }
    }


    private void SwitchPriority()
    {
        if (playerCamera == true)
        {
            Player.Priority = 0;
            GroupAttraction.Priority = 1;
            playerCamera = false;

        }
        else
        {
            Player.Priority = 1;
            GroupAttraction.Priority = 0;
            playerCamera = true;
        }

    }
}

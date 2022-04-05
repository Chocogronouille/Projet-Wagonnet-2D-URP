using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private bool playerCamera = true;
    public CinemachineVirtualCamera Player; //Player
    public CinemachineVirtualCamera GroupAttraction; //GroupCamera
    
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
            GroupAttraction.Priority = 1;
        }
        else
        {
            Player.Priority = 1;
            GroupAttraction.Priority = 0;
            
        }

        playerCamera = !playerCamera;
    }
}

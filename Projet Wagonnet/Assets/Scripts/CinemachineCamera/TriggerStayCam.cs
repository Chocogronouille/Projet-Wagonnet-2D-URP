using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using UnityEngine;

public class TriggerStayCam : MonoBehaviour
{
    
    public CinemachineVirtualCamera Player; //Player
    public CinemachineVirtualCamera GroupAttraction; //GroupCamera
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SwitchPriority();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SwitchPriority2();
    }

    private void SwitchPriority()
    {
        {
            Player.Priority = 0;
            GroupAttraction.Priority = 1;

        }
    }

    private void SwitchPriority2()
    {
        Player.Priority = 1;
        GroupAttraction.Priority = 0;
    }

}

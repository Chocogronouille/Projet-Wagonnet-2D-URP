using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[ExecuteAlways]

public class GetComponentConfiner : MonoBehaviour
{
    private CinemachineVirtualCamera camera;

    private void Start()
    {
        camera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        foreach (GameObject x in GameObject.FindGameObjectsWithTag("Confiner"))
        {
            if (x.GetComponent<PolygonCollider2D>() != null)
            {
                camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = x.GetComponent<PolygonCollider2D>();
            }
        }
    }
}

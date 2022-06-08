using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[ExecuteAlways]

public class GetComponentConfiner2 : MonoBehaviour
{ 
    [SerializeField] CinemachineVirtualCamera camera;
    private PolygonCollider2D[] confiners;

    private void Start()
    {
        camera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        confiners = FindObjectsOfType<PolygonCollider2D>();
        
        foreach (PolygonCollider2D x in confiners)
        {
            if (x.gameObject.CompareTag("Confiner2"))
            {
                camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = x;
            }
        }
    }
}

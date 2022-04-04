using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;

public class EssaiDotWeen : MonoBehaviour
{
    public AnimationCurve curve;
    private bool canRunCurve;
    private float graph, incrementCurve;
    private Vector3 oldPosition;




    // Start is called before the first frame update
    void Start()
    {
        canRunCurve = true;
        oldPosition = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (canRunCurve)
        {
            incrementCurve += Time.deltaTime;
            graph = curve.Evaluate(incrementCurve);
            gameObject.transform.position = new Vector3(oldPosition.x + graph, transform.position.y, transform.position.z);

        }

        // if (graph > curve.length - 0.01f)
        // {
        //     canRunCurve = false;
        //     graph = 0;
        //     incrementCurve = 0;
        // }
    }

    // private IEnumerator MouvementPlatforme()
    // {
    //     gameObject.transform.DOMoveX(transform.position.x + 20f, 1f).SetEase(Ease.InSine);
    //
    //     yield return new WaitForSeconds(3f);
    //     
    //     gameObject.transform.DOMoveX(transform.position.x - 20f, 1f).SetEase(Ease.InSine);
    //
    //
    //     yield return new WaitForSeconds(3f);
    //
    //     StartCoroutine(MouvementPlatforme());
    // }
    
}

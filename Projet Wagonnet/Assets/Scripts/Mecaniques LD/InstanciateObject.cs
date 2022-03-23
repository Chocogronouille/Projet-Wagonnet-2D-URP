using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciateObject : MonoBehaviour
{
    public bool isInstancie;
    public GameObject EmptyObject;
    public GameObject Player;
    // Start is called before the first frame update

    public static InstanciateObject instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de InstanciateObject dans la scène");
            return;
        }

        instance = this;
    }
    void Start()
    {
         EmptyObject = GameObject.Find("V2EmptyObject");
         Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(isInstancie)
        {
            Instantiate(EmptyObject);
     //       EmptyObject.transform.parent = gameObject.transform;
            EmptyObject.transform.position = EmptyObjectScript.instance.PlayerPos;
            EmptyObject.AddComponent<Cinemachine.CinemachinePath>();
            EmptyObject.AddComponent<DirectionGizmo>();
           TrackCreator.instance.GenerateTrack();
       //     isInstancie = false;
        }
        
    }
}

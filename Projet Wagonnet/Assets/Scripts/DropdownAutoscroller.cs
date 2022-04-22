using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
 
public class DropdownAutoscroller : MonoBehaviour {
 
    [Tooltip("Assign to the dropdown that should automatically scroll according to the currently selected item.")]
    public Dropdown dropdown;
 
    // Use this for initialization
    void Start ()
    {
        
    }
   
    // Update is called once per frame
    void Update () {
        if (EventSystem.current.currentSelectedGameObject == dropdown.gameObject)
        {
            if (Input.GetButtonUp("Vertical"))
            {
                Transform dropdownListTransform = dropdown.gameObject.transform.FindChild("Dropdown List");
                if (dropdownListTransform == null)
                {
                    // Show the dropdown when the user hits the arrow keys if the dropdown is not already showing
                    dropdown.Show();
                }
            }
        }
        else
        {  
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
   //         eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count > 0)
            {
                if (results[0].gameObject.transform.IsChildOf(dropdown.gameObject.transform))
                {
                    // Pointer over the list, use default behavior
                    return;
                }
            }
           
            // Autoscroll list as the selected object is changed from the arrow keys
            if (EventSystem.current.currentSelectedGameObject.transform.IsChildOf(dropdown.gameObject.transform))
            {
                if (EventSystem.current.currentSelectedGameObject.name.StartsWith("Item "))
                {
                    // Skip disabled items
                    Transform parent = EventSystem.current.currentSelectedGameObject.transform.parent;
                    int activeChildren = 0;
                    int totalChildren = parent.childCount;
                    for (int childIndex = 0; childIndex < totalChildren; childIndex++)
                    {
                        if (parent.GetChild(childIndex).gameObject.activeInHierarchy)
                        {
                            activeChildren++;
                        }
                    }
                    int myActiveIndex = 0;
                    for (int childIndex = 0; childIndex < totalChildren; childIndex++)
                    {
                        if (parent.GetChild(childIndex).gameObject == EventSystem.current.currentSelectedGameObject)
                        {
                            break;
                        }
                        else if (parent.GetChild(childIndex).gameObject.activeInHierarchy)
                        {
                            myActiveIndex++;
                        }
                    }
 
                    if (activeChildren > 1)
                    {
                        GameObject scrollbarGameObject = GameObject.Find ("Scrollbar");
                        if (scrollbarGameObject != null && scrollbarGameObject.activeInHierarchy)
                        {
                            Scrollbar scrollbar = scrollbarGameObject.GetComponent<Scrollbar>();
                            if (scrollbar.direction == Scrollbar.Direction.TopToBottom)
                                scrollbar.value = (float) myActiveIndex / (float) (activeChildren-1);
                            else
                                scrollbar.value = 1.0f - (float) myActiveIndex / (float) (activeChildren-1);
                        }
                    }
                }
            }
        }
    }
}
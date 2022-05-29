using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
//[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(BoxCollider2D))]
public class ButtonAdd : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button b;
    EventSystem eventSystem;
    public bool entered = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        b = GetComponent<Button>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if(b == null || eventSystem == null)
        {
            Debug.Log("failed");
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if(b == null || eventSystem == null) return;
        else
        {
            //if(eventSystem.currentSelectedGameObject == null) return;
            //else if(eventSystem.currentSelectedGameObject.gameObject == gameObject) return;
            //else
            //{
                b.Select();
                Debug.Log("MouseEnter");
            //}
        }
        entered = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        entered = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler*/
{
    public GameObject[] buttons;
    public string decisionKey = "Jump";
    int pointer = 0;
    bool wait = false;
    bool decided = false;
    float waitTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        Color c = buttons[pointer].GetComponent<Button>().colors.pressedColor;
        buttons[pointer].GetComponent<Image>().color = c;
        //Button b = buttons[pointer].GetComponent<Button>();
        wait = true;
        Invoke("StopWait", waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(decided)
        {
            return;
        }
        if(!wait)
        {
            if(Input.GetAxis("Vertical") != 0)
            {
                float axisV = Input.GetAxisRaw("Vertical");

                //Button b = buttons[pointer].GetComponent<Button>();
                //b.OnPointerExit(EventSystems.PointerEventData);
                Color c = buttons[pointer].GetComponent<Button>().colors.normalColor;
                buttons[pointer].GetComponent<Image>().color = c;

                if(axisV > 0) pointer = (pointer - 1 + buttons.Length) % buttons.Length;
                else pointer = (pointer + 1) % buttons.Length;

                Debug.Log(pointer);

                c = buttons[pointer].GetComponent<Button>().colors.pressedColor;
                buttons[pointer].GetComponent<Image>().color = c;
                //b = buttons[pointer].GetComponent<Button>();
                //b.OnPointerEnter(EventSystems.PointerEventData);
                wait = true;
                Invoke("StopWait", waitTime);
            }
        }

        if(Input.GetButtonDown(decisionKey))
        {
            Debug.Log("x");
            Button b = buttons[pointer].GetComponent<Button>();
            Color c = b.colors.pressedColor;
            buttons[pointer].GetComponent<Image>().color = c;
            b.onClick.Invoke();
            //b.OnPointerDown(EventSystems.PointerEventData);
            wait = true;
            decided = true;
        }
    }

    void StopWait()
    {
        wait = false;
    }

    bool AllActive()
    {
        int i;
        bool ret = true;
        for(i = 0; i < buttons.Length; i++)
        {
            ret = ret && buttons[i].activeSelf;
        }
        return ret;
    }

    /*public void OnPointerEnter(PointerEventData exentData)
    {

    }

    public void OnPointerExit(PointerEventData exentData)
    {

    }

    public void OnPointerDown(PointerEventData exentData)
    {

    }*/
}

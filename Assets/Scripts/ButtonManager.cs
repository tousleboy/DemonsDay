using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler*/
{
    public GameObject[] buttons;
    public GameObject cursor;
    public string decisionKey = "Jump";
    int pointer = 0;
    bool wait = false;
    bool decided = false;
    float waitTime = 0.3f;
    public bool circle = false;

    // Start is called before the first frame update
    void Start()
    {
        //Color c = buttons[pointer].GetComponent<Button>().colors.pressedColor;
        //buttons[pointer].GetComponent<Image>().color = c;
        if(cursor != null) cursor.transform.position = buttons[pointer].transform.position;
        //Button b = buttons[pointer].GetComponent<Button>();
        //StartCoroutine(WaitTime(waitTime));
        /*wait = true;
        Invoke("StopWait", waitTime);*/
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("wait" + wait);
        if(decided || !gameObject.activeSelf)
        {
            return;
        }
        if(!wait)
        {
            if(Input.GetAxisRaw("Vertical") != 0)
            {
                float axisV = Input.GetAxisRaw("Vertical");

                //Button b = buttons[pointer].GetComponent<Button>();
                //b.OnPointerExit(EventSystems.PointerEventData);
                Color c = buttons[pointer].GetComponent<Button>().colors.normalColor;
                buttons[pointer].GetComponent<Image>().color = c;

                if(circle)
                {
                    if(axisV > 0) pointer = (pointer - 1 + buttons.Length) % buttons.Length;
                    else if(axisV < 0) pointer = (pointer + 1) % buttons.Length;
                }
                else
                {
                    if(axisV > 0) pointer = Mathf.Max(pointer - 1, 0);
                    else if(axisV < 0) pointer = Mathf.Min(pointer + 1, buttons.Length - 1);
                }

                Debug.Log(pointer);

                //c = buttons[pointer].GetComponent<Button>().colors.pressedColor;
                //buttons[pointer].GetComponent<Image>().color = c;
                //b = buttons[pointer].GetComponent<Button>();
                //b.OnPointerEnter(EventSystems.PointerEventData);
                if(cursor != null) cursor.transform.position = buttons[pointer].transform.position;

                StartCoroutine(WaitTime(waitTime));
                /*wait = true;
                Invoke("StopWait", waitTime);*/
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
            decided = true;
        }
    }

    void OnEnable()
    {
        wait = false;
        decided = false;
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

    IEnumerator WaitTime(float t)
    {
        Debug.Log("coroutine");
        wait = true;
        yield return new WaitForSecondsRealtime(t);
        Debug.Log("done");
        wait = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler*/
{
    public GameObject[] buttons;
    public GameObject cursor;
    EventSystem eventSystem;
    public string decisionKey = "Jump";
    int pointer = 0;
    bool wait = false;
    bool decided = false;
    float waitTime = 0.3f;
    public bool circle = false;

    AudioSource soundPlayer;
    public AudioClip selectSound;

    GameObject nowSelected;
    GameObject pastSelected;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
                //Color c = buttons[pointer].GetComponent<Button>().colors.normalColor;
                //buttons[pointer].GetComponent<Image>().color = c;

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

                //StartCoroutine(WaitTime(waitTime));

                //buttons[pointer].GetComponent<Button>().Select();

                Debug.Log(pointer);
            }
        }

        if(eventSystem.currentSelectedGameObject != null) nowSelected = eventSystem.currentSelectedGameObject.gameObject;
        else
        {
            Debug.Log("null selection");
            nowSelected.GetComponent<Button>().Select();
        }
        if(nowSelected != pastSelected)
        {
            pastSelected = nowSelected;
            if(cursor != null) cursor.transform.position = nowSelected.transform.position;
            soundPlayer.PlayOneShot(selectSound);
        }
    }

    void OnEnable()
    {
        soundPlayer = GetComponent<AudioSource>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        wait = false;
        decided = false;
        int i;
        for(i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().interactable = true;
        }
        pointer = 0;
        if(cursor != null) cursor.transform.position = buttons[pointer].transform.position;
        buttons[pointer].GetComponent<Button>().Select();

        nowSelected = buttons[pointer];
        pastSelected = nowSelected; 
    }

    public void Decided()
    {
        decided = true;
        Debug.Log("decided");
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

    public void AllInteractableFalse()
    {
        int i;
        decided = true;
        for(i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().interactable = false;
        }
        decided = true;
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

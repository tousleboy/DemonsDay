using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoManager : MonoBehaviour
{
    GameObject logo;
    GameObject button;
    GameObject panel;
    Image Il;
    Image Ip;

    // Start is called before the first frame update
    void Start()
    {
        logo = transform.Find("Logo").gameObject;
        button = transform.Find("Button").gameObject;
        panel = transform.Find("Panel").gameObject;
        Il = logo.GetComponent<Image>();
        Ip = panel.GetComponent<Image>();

        Il.color = Color.clear;
        Ip.color = Color.black;
        panel.SetActive(true);
        button.SetActive(false);
        StartCoroutine("TurnOn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TurnOn()
    {
        float t = 0.0f;
        float speed1 = 1.0f;
        float speed2 = 0.5f;
        float time1 = 0.5f;
        float time2 = 2f;
        float time3 = 5f;
        yield return new WaitForSeconds(time1);
        Il.color = Color.red;
        while(t <= 1.0f)
        {
            Il.color = Color.Lerp(Color.red, Color.white, t);
            t += speed1 * Time.deltaTime;
            yield return null;
        }
        t = 0.0f;
        yield return new WaitForSeconds(time2);
        while(t <= 1.0f)
        {
            Ip.color = Color.Lerp(Color.black, Color.clear, t);
            t += speed2 * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(time3);
        button.SetActive(true);
        panel.SetActive(false);
    }

    IEnumerator TurnOff()
    {
        float t = 0.0f;
        float speed = 0.5f;
        //Button bt = button.GetComponent<Button>();
        //bt.interactable = false;
        panel.SetActive(true);
        while(t <= 1.0f)
        {
            Ip.color = Color.Lerp(Color.clear, Color.black, t);
            Il.color = Color.Lerp(Color.white, Color.clear, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
    }

    public void StartTurnOff()
    {
        StartCoroutine("TurnOff");
    }
}

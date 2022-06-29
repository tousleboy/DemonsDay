using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    //public GameObject mainImage;
    public GameObject startButton;
    public GameObject panel;
    public GameObject Fade;
    Image I;
    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Load();
        SaveManager.Show();
        //mainImage = transform.Find("MainImage").gameObject;
        //button = transform.Find("Button").gameObject;
        I = Fade.GetComponent<Image>();

        I.color = Color.black;
        Fade.SetActive(true);
        if(GameManager.nowScene != "") startButton.GetComponent<ChangeScene>().sceneName = GameManager.nowScene;

        panel.SetActive(false);
        StartCoroutine("TurnOn");

        TutorialScript.onceCalled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TurnOn()
    {
        float t = 0.0f;
        float speed = 0.5f;
        while(t <= 1.0f)
        {
            I.color = Color.Lerp(Color.black, Color.clear, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
        I.color = Color.clear;
        panel.SetActive(true);
        Fade.SetActive(false);
    }

    IEnumerator TurnOff()
    {
        float t = 0.0f;
        float speed = 0.5f;
        //Button bt = button.GetComponent<Button>();
        //bt.interactable = false;
        Fade.SetActive(true);
        while(t <= 1.0f)
        {
            I.color = Color.Lerp(Color.clear, Color.black, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
        I.color = Color.black;
    }

    public void StartTurnOff()
    {
        StartCoroutine("TurnOff");
    }
}

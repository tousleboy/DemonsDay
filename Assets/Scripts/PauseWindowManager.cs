using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindowManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject buttonPannel;
    public GameObject mainImage;
    bool active = false;
    string state;
    // Start is called before the first frame update
    void Start()
    {
        mainPanel.SetActive(false);
        buttonPannel.SetActive(false);
        mainImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool stateCorrect =  PlayerController.gameState == "playing" || PlayerController.gameState == "pause" || PlayerController.gameState == "waiting";
        if(Input.GetButtonDown("Pause") && stateCorrect)
        {
            if(!active) Activate();
            else Deactivate();
        }
    }

    void Activate()
    {
        state = PlayerController.gameState;
        mainPanel.SetActive(true);
        buttonPannel.SetActive(true);
        mainImage.SetActive(true);
        active = true;
        Time.timeScale = 0.0f;
        PlayerController.gameState = "pause";
    }

    public void Deactivate()
    {
        mainPanel.SetActive(false);
        buttonPannel.SetActive(false);
        mainImage.SetActive(false);
        active = false;
        Time.timeScale = 1.0f;
        PlayerController.gameState = state;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrateGaugeManager : MonoBehaviour
{
    public GameObject[] gauges;
    int gaugeLen;
    int oneGaugeValue = 10;
    public static int maxCon;
    int prevCon;
    int nowCon;

    public Sprite concentrateBlue;
    public Sprite gray;

    // Start is called before the first frame update
    void Start()
    {
        gaugeLen = gauges.Length;
        maxCon = gaugeLen * oneGaugeValue;
        prevCon = PlayerController.concentration;
        nowCon = PlayerController.concentration;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController.concentration = Mathf.Min(PlayerController.concentration, maxCon);
        nowCon = PlayerController.concentration;

        if((prevCon / oneGaugeValue) < (nowCon / oneGaugeValue))
        {
            TurnGauge(true, (nowCon / oneGaugeValue) - 1);
            prevCon = nowCon;
        }
        else if((prevCon / oneGaugeValue) > (nowCon / oneGaugeValue))
        {
            TurnGauge(false, (prevCon / oneGaugeValue) - 1);
            prevCon = nowCon;
        }
    }

    void TurnGauge(bool mode, int pointer)
    {
        if(mode) gauges[pointer].GetComponent<Image>().sprite = concentrateBlue;
        else  gauges[pointer].GetComponent<Image>().sprite = gray;
    }
}

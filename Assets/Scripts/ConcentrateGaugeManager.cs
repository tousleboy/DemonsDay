using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrateGaugeManager : MonoBehaviour
{
    public GameObject[] gauges;
    int gaugeLen;
    int oneGaugeValue = 3;
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
        nowCon = PlayerController.concentration;
        prevCon = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController.concentration = Mathf.Min(PlayerController.concentration, maxCon);
        nowCon = PlayerController.concentration;

        int i;
        if((prevCon / oneGaugeValue) < (nowCon / oneGaugeValue))
        {
            for(i = 0; i < (nowCon / oneGaugeValue) - (prevCon / oneGaugeValue); i++) TurnGauge(true, (nowCon / oneGaugeValue) - 1 - i);
            
            prevCon = nowCon;
        }
        else if((prevCon / oneGaugeValue) > (nowCon / oneGaugeValue))
        {
            for(i = 0; i < (prevCon / oneGaugeValue) - (nowCon / oneGaugeValue); i++) TurnGauge(false, (prevCon / oneGaugeValue) - 1 - i);
            
            prevCon = nowCon;
        }
    }

    void TurnGauge(bool mode, int pointer)
    {
        if(mode) gauges[pointer].GetComponent<Image>().sprite = concentrateBlue;
        else  gauges[pointer].GetComponent<Image>().sprite = gray;
    }
}

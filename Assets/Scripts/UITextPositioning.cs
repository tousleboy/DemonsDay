using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextPositioning : MonoBehaviour
{
    GameObject player;
    GameObject mainCamera;
    float leftLimit = -300f;
    float rightLimit = 471.76f;
    RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        rt = GetComponent<RectTransform>();
        if(rt == null)
        {
            Debug.Log("null");
        }
        else
        {
            Debug.Log("not null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = player.transform.position.x - mainCamera.transform.position.x;
        x = Mathf.Min(x, rightLimit);
        x = Mathf.Max(x, leftLimit);

        Vector2 pos = new Vector2(x, rt.anchoredPosition.y);
        Debug.Log(pos);
        rt.anchoredPosition = pos;
    }
}

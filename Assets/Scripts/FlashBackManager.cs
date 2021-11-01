using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBackManager : MonoBehaviour
{
    public GameObject[] flashBacks;
    public GameObject back;
    public GameObject wall;
    GameObject player;
    public float speed = 2.0f;
    //public float interval = 5.0f;
    public float range = 5.0f;
    public string direction = "holizontal";
    bool activated = false;
    bool go = true;
    // Start is called before the first frame update
    void Start()
    {
        int i;
        for(i = 0; i < flashBacks.Length; i++) flashBacks[i].SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");

        if(flashBacks.Length == 0 || player == null || back == null) activated = true;
    }

    // Update is called once per frame
    void Update()
    {
        float x = player.transform.position.x;
        if(x >= transform.position.x - range && x <= transform.position.x + range && !activated)
        {
            activated = true;
            StartCoroutine("FlashBack");
        }
    }

    IEnumerator FlashBack()
    {
        float t = 0;
        Renderer R = back.GetComponent<Renderer>();
        while(t <= 1.0f)
        {
            R.material.color = Color.Lerp(Color.white, Color.clear, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
        R.material.color = Color.clear;
        
        int i;
        for(i = 0; i < flashBacks.Length; i++)
        {
            StartCoroutine(Move(flashBacks[i], direction));
            go = false;
            //yield return new WaitForSeconds(interval);
            while(!go) yield return null;

        }

        t = 0;
        while(t <= 1.0f)
        {
            R.material.color = Color.Lerp(Color.clear, Color.white, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
        R.material.color = Color.white;
        wall.SetActive(false);
    }

    IEnumerator Move(GameObject obj, string d)
    {
        Renderer R = obj.GetComponent<Renderer>();
        obj.SetActive(true);
        if(d == "vertical")
        {
            while(obj.transform.position.y < transform.position.y || R.isVisible)
            {
                obj.transform.Translate(0, speed * Time.deltaTime, 0);
                yield return null;
            }
        }
        else
        {
            while(obj.transform.position.x < transform.position.x || R.isVisible)
            {
                obj.transform.Translate(speed * Time.deltaTime, 0, 0);
                yield return null;
            }
        }

        obj.SetActive(false);
        go = true;
    }
}

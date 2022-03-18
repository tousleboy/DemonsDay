using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningManScript : MonoBehaviour
{
    public float range = 10.0f;
    public float speed = 2.0f;
    float howfar = 0.0f;
    bool activated = false;

    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("RunningAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        if(activated && howfar <= range)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            howfar += speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !activated) activated = true;
    }

    IEnumerator RunningAnimation()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int i = 0;
        while(true)
        {
            sr.sprite = sprites[i];
            i = (i + 1) % sprites.Length;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

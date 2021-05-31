using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicCameraController : MonoBehaviour
{
    public GameObject rootAnchor;
    GameObject Anchor;
    ComicAnchor ca;
    public float leaveSpeed = 1.0f;
    float allowableRange = 1.0f;
    Vector2 direction;
    bool comicEnd = false;
    bool comicStop = false;

    AudioSource soundPlayer;

    ChangeScene cs;
    // Start is called before the first frame update
    void Start()
    {
        Anchor = rootAnchor;
        ca = Anchor.GetComponent<ComicAnchor>();
        direction = Direction(Anchor.transform.position);

        soundPlayer = GetComponent<AudioSource>();

        cs = GetComponent<ChangeScene>();
    }

    // Update is called once per frame
    void Update()
    {
        if(comicEnd || comicStop)
        {
            return;
        }
        if(Vector2.Distance(Anchor.transform.position, transform.position) > allowableRange)
        {
            transform.position += (Vector3)direction * leaveSpeed * Time.deltaTime;
        }
        else
        {
            //Debug.Log(transform.position);
            if(ca.sound != null)
            {
                soundPlayer.PlayOneShot(ca.sound);
            }
            if(ca.end)
            {
                comicEnd = true;
                cs.Load();
            }
            else
            {
                if(ca.waitTime != 0.0f)
                {
                    comicStop = true;
                    Invoke("Restart", ca.waitTime);
                }
                leaveSpeed =  ca.leaveSpeed;
                Anchor = ca.next;
                ca = Anchor.GetComponent<ComicAnchor>();
                direction = Direction(Anchor.transform.position);
            }
            Debug.Log("target" + Anchor + "speed" + leaveSpeed);
        }
    }

    Vector2 Direction(Vector2 targetPos)
    {
        Vector2 direction = targetPos - (Vector2)transform.position;
        return direction.normalized;
    }

    void Restart()
    {
        comicStop = false;
    }
}

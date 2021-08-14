using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBagScript : MonoBehaviour
{
    public int life = 3;
    public string targetAnime = "SandBagTarget1";
    AudioSource soundPlayer;
    public GameObject target;
    Animator animator;
    public AudioClip punchHit;
    bool damaged = false;
    bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        animator = target.GetComponent<Animator>();
        animator.Play(targetAnime);
    }

    // Update is called once per frame
    void Update()
    {
        if(dead) return;
        if(life <= 0) StartCoroutine("Die");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack" && !damaged)
        {
            life -= collision.gameObject.GetComponent<AttackManager>().val;
            soundPlayer.PlayOneShot(punchHit);
            if(!damaged) StartCoroutine("Swing");
        }
    }

    IEnumerator Swing()
    {
        float speed = -100f;
        damaged = true;
        Quaternion now = transform.rotation;
        while(Quaternion.Angle(now, transform.rotation) < 10)
        {
            transform.Rotate(new Vector3(0, 0, -speed * Time.deltaTime));
            yield return null;
        }
        while(Quaternion.Angle(transform.rotation, now) > 1.0)
        {
            transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
            yield return null;
        }
        damaged = false;
    }

    IEnumerator Die()
    {
        Vector2 direction = new Vector2(5, -10);
        float t;
        float length = 1.0f;
        GetComponent<BoxCollider2D>().enabled = false;
        dead = true;
        
        for(t = 0; t < length; t += Time.deltaTime)
        {
            transform.Translate(direction * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}

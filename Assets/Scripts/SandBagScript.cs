using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBagScript : MonoBehaviour
{
    public int life = 3;
    public string targetAnime = "SandBagTarget1";
    AudioSource soundPlayer;
    public GameObject target;
    GameObject player;
    Animator animator;
    public AudioClip punchHit;
    bool damaged = false;
    bool dead = false;
    float length = 4.5f;
    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        animator = target.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(dead) return;
        if(CheckLength(player.transform.position))
        {
            if(target.activeSelf == false)
            {
                target.SetActive(true);
                animator.Play(targetAnime);
            }
        }
        else target.SetActive(false);

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

    bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position,targetPos);
        if(length >= d)
        {
            ret = true;
        }
        return ret;
    }

    IEnumerator Swing()
    {
        float speed = -200f;
        damaged = true;
        Quaternion now = transform.rotation;
        while(Quaternion.Angle(now, transform.rotation) < 10)
        {
            transform.Rotate(new Vector3(0, 0, -speed * Time.deltaTime));
            yield return null;
        }
        while(Quaternion.Angle(transform.rotation, now) > 2.0)
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
        target.GetComponent<AudioSource>().volume = 0;
        dead = true;
        
        for(t = 0; t < length; t += Time.deltaTime)
        {
            transform.Translate(direction * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}

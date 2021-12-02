using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBagScript : MonoBehaviour
{
    public int life = 3;
    public string targetAnime = "SandBagTarget1";
    AudioSource soundPlayer;
    AudioSource soundPlayerT;
    public GameObject target;
    GameObject player;
    //Animator animator;
    public AudioClip punchHit;
    public AudioClip hyun;
    bool damaged = false;
    bool dead = false;
    float length = 4.5f;

    public Sprite j;
    public Sprite k;

    public enum HITTYPE
    {
        j,
        k
    }
    public HITTYPE[] hitType;
    int pointer = 0;

    Coroutine _currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        soundPlayerT = target.GetComponent<AudioSource>();
        //animator = target.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        target.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(dead) return;
        if(CheckLength(player.transform.position))
        {
            if(_currentCoroutine == null)
            {
                _currentCoroutine = StartCoroutine("Signal");
                //animator.Play(targetAnime);
            }
        }
        else
        {
            StopCoroutine("Signal");
            _currentCoroutine = null;
            target.SetActive(false);
        }

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
        soundPlayerT.volume = 0;
        dead = true;
        
        for(t = 0; t < length; t += Time.deltaTime)
        {
            transform.Translate(direction * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator Signal()
    {
        target.SetActive(true);
        SpriteRenderer r = target.GetComponent<SpriteRenderer>();
        r.color = Color.white;
        int i;
        float f = 0;
        float interval = 0.3f;

        while(true)
        {
            for(i = 0; i < hitType.Length; i++)
            {
                if(hitType[i] == HITTYPE.j) r.sprite = j;
                else r.sprite = k;

                r.color = Color.white;
                soundPlayerT.PlayOneShot(hyun);

                yield return new WaitForSeconds(interval / 2f);

                while(f <= 1f)
                {
                    r.color = Color.Lerp(Color.white, Color.clear, f);
                    f += Time.deltaTime / (interval / 2);
                    yield return null;
                }
                f = 0;
                r.color = Color.clear;
            }
            yield return  new WaitForSeconds(interval);
        }
    }
}

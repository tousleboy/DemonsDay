using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public Sprite[] attack;
    public Sprite[] parry;
    public SpriteRenderer r;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackEffect()
    {
        gameObject.SetActive(true);
        StartCoroutine(Animation(attack));
    }
    public void ParryEffect()
    {
        gameObject.SetActive(true);
        StartCoroutine(Animation(parry));
    }

    /*public void EffectGen(Vector3 pos, int type)
    {
        if(pos.x >= transform.position.x) effect.transform.position = right.transform.position;
        else effect.transform.position = left.transform.position;

        effect.transform.localScale = new Vector3(Mathf.Sign(pos.x - transform.position.x) * Mathf.Abs(effect.transform.localScale.x), effect.transform.localScale.y, effect.transform.localScale.z);
        if(type == 0) StartCoroutine(Animation(attack));
        else StartCoroutine(Animation(parry));
    }*/

    IEnumerator Animation(Sprite[] sprites)
    {
        int i;
        for(i = 0; i < sprites.Length; i++)
        {
            r.sprite = sprites[i];
            yield return new WaitForSeconds(0.02f);
        }
        gameObject.SetActive(false);
    }
}

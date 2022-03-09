using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSightScript : MonoBehaviour
{
    public int maxEnemy = 10;
    public float maxScale = 2.5f;
    public float minScale = 1.1f;

    int prevD;
    int nowD;
    // Start is called before the first frame update
    void Start()
    {
        prevD = GameManager.stageDefeats;
        nowD = GameManager.stageDefeats;
        transform.localScale = new Vector3(maxScale, maxScale, transform.localScale.z);
        //StartCoroutine("Moyamoya");
    }

    // Update is called once per frame
    void Update()
    {
        nowD = Mathf.Min(GameManager.stageDefeats, maxEnemy);
        if(nowD != prevD)
        {
            prevD = nowD;
            float s = maxScale - ((maxScale - minScale) * ((float)nowD / (float)maxEnemy));
            transform.localScale = new Vector3(s, s, transform.localScale.z);
        }
    }

    IEnumerator Moyamoya()
    {
        while(true)
        {
            int r = Random.Range(0, 4);

            if(r == 0) transform.localScale = Vector3.Scale(new Vector3(1f, 1f, 1f), transform.localScale);
            else if(r == 1) transform.localScale = Vector3.Scale(new Vector3(-1f, 1f, 1f), transform.localScale);
            else if(r == 2) transform.localScale = Vector3.Scale(new Vector3(1f, -1f, 1f), transform.localScale);
            else transform.localScale = Vector3.Scale(new Vector3(-1f, -1f, 1f), transform.localScale);

            yield return new WaitForSeconds(0.05f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossComment : MonoBehaviour
{
    float span = 3f, delta = 0f, AngleParam = 0f;
    float BossR = 0.3f, BulletR = 0.2f;
    int h, N;
    
    [SerializeField] GameObject[] Comment;
    GameObject player, boss;

    Transform[] Comments;

    // Start is called before the first frame update
    void Start()
    {
        N = 18;

        Random.InitState(System.DateTime.Now.Millisecond);
        this.player = GameObject.Find("Player");
        this.boss = GameObject.Find("Boss");

        Comment = new GameObject[N];
        Comments = new Transform[N];
        for (int i = 0; i < N; ++i)
        {
            Comments[i] = new GameObject("Comments" + (i + 1).ToString()).transform;
            Comment[i] = GameObject.Find("Comment" + (i + 1).ToString());
            Instantiate(Comment[i], new Vector3(0, -3, 0), Quaternion.Euler(0f, 0f, 0f), Comments[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(BossHP.HP < 5f)
        {
            this.span = 1.5f;
        }

        this.delta += Time.deltaTime;
        if (this.delta > this.span)
        {
            h = Random.Range(0, N);
            this.delta = 0f;
            InstBullet(new Vector3(12, Random.Range(-2.0f, 4.5f), 0), Comment[h], Comments[h]);
        }


        // Collision detection
        Vector2 PlayerPosition = this.player.transform.position;
        for (int i = 0; i < N; ++i)
        {
            foreach (Transform t in Comments[i])
            {
                GameObject g = t.gameObject;
                if (t.gameObject.activeSelf && Mathf.Abs(PlayerPosition.x - t.position.x) < 3.8f && Mathf.Abs(PlayerPosition.y - t.position.y) < 0.4f)
                {
                    PlayerHP.HP -= 0.5f;
                    GameObject director = GameObject.Find("GameDirector");
                    director.GetComponent<GameDirector>().DecreasePlayerHP(PlayerHP.HP, PlayerHP.MaxHP);
                    t.gameObject.SetActive(false);
                }
            }
        }
    }


    /********************************
     * 
     * íeñãÇê∂ê¨Ç∑ÇÈä÷êî
     * 
     ********************************/
    void InstBullet(Vector3 pos, GameObject Comment, Transform Comments)
    {
        foreach (Transform t in Comments)
        {
            if (! t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, Quaternion.Euler(0f, 0f, 0f));
                t.gameObject.SetActive(true);
                return;
            }
        }
        Instantiate(Comment, pos, Quaternion.Euler(0f, 0f, 0f), Comments);
    }
}

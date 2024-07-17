using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    float span = 0.6f, delta = 0f, AngleParam = 0f, AngleDif = 1f;

    public static float PlayerR = 0.2f;
    float BulletR = 0.1f;

    [SerializeField] GameObject bullet;
    GameObject player;

    Transform bullets;
    
    void Start()
    {
        this.player = GameObject.Find("Player");
        bullet = GameObject.Find("Bullets");
        bullets = new GameObject("Bullet").transform;
    }
    
    void Update()
    {
        this.delta += Time.deltaTime;

        if (this.delta > this.span)
        {
            // ï˙éÀèÛÇ…çLÇ™ÇÈÇ≤Ç≠àÍî ìIÇ»íeñã
            this.AngleParam += this.AngleDif;
            if (this.AngleParam >= 45f) this.AngleDif = -1f;
            if(this.AngleParam < 0f) this.AngleDif = 1f;

            this.delta = 0f;
            for (int angle = 0; angle < 32; ++angle)
            {
                InstBullet(transform.position, Quaternion.Euler(0f, 0f, angle * 11.25f + AngleParam), bullet);
            }
        }


        // ìñÇΩÇËîªíË
        Vector2 PlayerPosition = this.player.transform.position;
        foreach (Transform t in bullets)
        {
            if (t.gameObject.activeSelf)
            {
                Vector2 BulletPosition = t.position;
                Vector2 dir = BulletPosition - PlayerPosition;
                float d = dir.magnitude;
                
                // íeñãÇ…ÇﬂÇ¡ÇøÇ·ãﬂÇ©Ç¡ÇΩÇÁScoreÇâ¡éZ
                if(d < PlayerR + BulletR + 0.2f)
                {
                    PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 30);
                    PlayerPrefs.Save();
                }

                if (d < PlayerR + BulletR)
                {
                    --PlayerHP.HP;
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
    void InstBullet(Vector3 pos, Quaternion rotation, GameObject bullet)
    {
        foreach (Transform t in bullets)
        {
            if (!t.gameObject.activeSelf)
            {
                t.SetPositionAndRotation(pos, rotation);
                t.gameObject.SetActive(true);
                return;
            }
        }

        Instantiate(bullet, pos, rotation, bullets);
    }
}

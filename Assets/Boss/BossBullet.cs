using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    float span = 0.6f, delta = 0f, AngleParam = 0f, AngleDif = 1f, d;
    float span2 = 6f, delta2 = 0;

    public static float PlayerR = 0.1f;
    float BulletR = 0.1f, FollowBulletR = 0.15f;

    [SerializeField] GameObject bullet, followBullet;
    GameObject player;

    Transform bullets, followBullets;
    
    void Start()
    {
        d = 0;
        this.player = GameObject.Find("Player");
        bullet = GameObject.Find("Bullets");
        bullets = new GameObject("Bullet").transform;
        followBullet = GameObject.Find("FollowBullets");
        followBullets = new GameObject("FollowBullet").transform;
    }
    
    void Update()
    {
        if(PlayerHP.HP <= 0)
        {
            SceneChange.Result();
        }
        
        this.delta += Time.deltaTime; this.delta2 += Time.deltaTime;

        // ’Ç]‚¶‚á‚È‚¢’e
        if (this.delta > this.span && BossHP.HP > 8f)
        {
            this.delta = 0f;
            RadialBullet(45, 1, bullet, bullets);
        }else if (this.delta > this.span && BossHP.HP > 6f)
        {
            this.span = 0.3f;
            this.delta = 0f;
            RadialBullet(10, 2, bullet, bullets);
        }
        else if(this.delta > this.span)
        {
            this.span2 = 3f;
            this.span = 0.4f;
            this.delta = 0f;
            RadialBullet(180, this.AngleDif, bullet, bullets);
            ++this.AngleDif;
            if (this.AngleDif > 360) this.AngleDif = 0;
        }

        // ’Ç]‚µ‚Ä‚­‚é’e
        if(this.delta2 > this.span2)
        {
            this.delta2 = 0f;
            FollowBullet(followBullet, followBullets);
        }

        // “–‚½‚è”»’è
        CollisionDetection(BulletR, bullet, bullets);
        CollisionDetection(FollowBulletR, followBullet, followBullets);

    }




    /**********************************
     * 
     * •úŽËó‚ÉL‚ª‚é‚²‚­ˆê”Ê“I‚È’e–‹
     * 
     **********************************/
    void RadialBullet(float limit, float difference, GameObject bullet, Transform bullets)
    {
        this.AngleParam += this.AngleDif;
        if (this.AngleParam >= limit) this.AngleDif = -difference;
        if (this.AngleParam < 0f) this.AngleDif = difference;
        
        for (int angle = 0; angle < 20; ++angle)
        {
            InstBullet(transform.position, Quaternion.Euler(0f, 0f, angle * 18f + AngleParam), bullet, bullets);
        }
    }


    /**********************************
     * 
     * ’Ç]‚µ‚Ä‚­‚é‚¤‚´‚¢‚â‚Â
     * 
     **********************************/
    void FollowBullet(GameObject bullet, Transform bullets)
    {
        float angle, tmp;
        tmp = (transform.position.y - this.player.transform.position.y) / (transform.position.x - this.player.transform.position.x);
        angle = Mathf.Abs(Mathf.Atan(tmp)) * 180f / Mathf.PI + 180f;

        InstBullet(transform.position, Quaternion.Euler(0f, 0f, angle), bullet, bullets);
    }


    /********************************
     * 
     * ’e–‹‚ð¶¬‚·‚éŠÖ”
     * 
     ********************************/
    void InstBullet(Vector3 pos, Quaternion rotation, GameObject bullet, Transform bullets)
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


    /**********************************
     * 
     * “–‚½‚è”»’è‚ð‚·‚éŠÖ”
     * 
     **********************************/
    void CollisionDetection(float BulletR, GameObject bullet, Transform bullets)
    {
        Vector2 PlayerPosition = this.player.transform.position;
        foreach (Transform t in bullets)
        {
            if (t.gameObject.activeSelf)
            {
                Vector2 BulletPosition = t.position;
                Vector2 dir = BulletPosition - PlayerPosition;
                float d = dir.magnitude;

                // ’e–‹‚É‚ß‚Á‚¿‚á‹ß‚©‚Á‚½‚çScore‚ð‰ÁŽZ
                if (d < PlayerR + BulletR + 0.2f)
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
}

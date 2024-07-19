using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    float span = 0.6f, delta = 0f, AngleParam = 0f, AngleDif = 1f, d;

    public static float PlayerR = 0.1f;
    float BulletR = 0.1f;

    [SerializeField] GameObject bullet;
    GameObject player;

    Transform bullets;
    
    void Start()
    {
        d = 0;
        this.player = GameObject.Find("Player");
        bullet = GameObject.Find("Bullets");
        bullets = new GameObject("Bullet").transform;
    }
    
    void Update()
    {
        this.delta += Time.deltaTime;

        if (this.delta > this.span && BossHP.HP > 8f)
        {
            this.delta = 0f;
            RadialBullet(45, 1);
        }else if (this.delta > this.span && BossHP.HP > 6f)
        {
            this.span = 0.3f;
            this.delta = 0f;
            RadialBullet(10, 2);
        }
        else if(this.delta > this.span)
        {
            this.span = 0.4f;
            this.delta = 0f;
            RadialBullet(180, this.AngleDif);
            ++this.AngleDif;
            if (this.AngleDif > 360) this.AngleDif = 0;
        }


        // “–‚½‚è”»’è
        Vector2 PlayerPosition = this.player.transform.position;
        foreach (Transform t in bullets)
        {
            if (t.gameObject.activeSelf)
            {
                Vector2 BulletPosition = t.position;
                Vector2 dir = BulletPosition - PlayerPosition;
                float d = dir.magnitude;
                
                // ’e–‹‚É‚ß‚Á‚¿‚á‹ß‚©‚Á‚½‚çScore‚ð‰ÁŽZ
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

    // •úŽËó‚ÉL‚ª‚é‚²‚­ˆê”Ê“I‚È’e–‹
    void RadialBullet(float limit, float difference)
    {
        this.AngleParam += this.AngleDif;
        if (this.AngleParam >= limit) this.AngleDif = -difference;
        if (this.AngleParam < 0f) this.AngleDif = difference;
        
        for (int angle = 0; angle < 32; ++angle)
        {
            InstBullet(transform.position, Quaternion.Euler(0f, 0f, angle * 11.25f + AngleParam));
        }
    }


    /********************************
     * 
     * ’e–‹‚ð¶¬‚·‚éŠÖ”
     * 
     ********************************/
    void InstBullet(Vector3 pos, Quaternion rotation)
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

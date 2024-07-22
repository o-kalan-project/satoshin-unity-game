using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBullet : MonoBehaviour
{
    float span = 0.6f, delta = 0f, AngleParam = 0f, AngleDif = 1f, d;
    float span2 = 6f, delta2 = 0, HachioujiSpan = 1f, HachioujiDelta = 0f;

    public static float PlayerR = 0.1f;
    float BulletR = 0.1f, FollowBulletR = 0.15f;

    [SerializeField] GameObject bullet, followBullet;
    GameObject player;
    GameObject[][] BulletGen = new GameObject[3][];

    Transform bullets, followBullets;
    
    void Start()
    {
        d = 0;
        this.player = GameObject.Find("Player");
        bullet = GameObject.Find("Bullets");
        bullets = new GameObject("Bullet").transform;
        followBullet = GameObject.Find("FollowBullets");
        followBullets = new GameObject("FollowBullet").transform;
        for(int i = 0; i < 3; ++i){
            BulletGen[i] = new GameObject[3];
            for(int j = 0; j < 3; ++j){
                BulletGen[i][j] = GameObject.Find("T" + i.ToString() + j.ToString());
                BulletGen[i][j].GetComponent<Text>().text = "　";
            }
        }
    }
    
    void Update()
    {
        if(PlayerHP.HP <= 0)
        {
            SceneChange.Result();
        }
        
        this.delta += Time.deltaTime; this.delta2 += Time.deltaTime;

        // �Ǐ]����Ȃ��e
        if (this.delta > this.span && BossHP.HP > 8f)
        {
            this.delta = 0f;
            RadialBullet(45, 1, bullet, bullets, transform);
        }else if (this.delta > this.span && BossHP.HP > 6f)
        {
            this.span = 0.3f;
            this.delta = 0f;
            RadialBullet(10, 2, bullet, bullets, transform);
        }
        else if(this.delta > this.span)
        {
            this.span2 = 3f;
            this.span = 0.4f;
            this.delta = 0f;
            RadialBullet(180, this.AngleDif, bullet, bullets, transform);
            ++this.AngleDif;
            if (this.AngleDif > 360) this.AngleDif = 0;
        }

        // �Ǐ]���Ă���e
        if(this.delta2 > this.span2)
        {
            this.delta2 = 0f;
            FollowBullet(followBullet, followBullets);
        }

        HachioujiBullet(bullet, bullets);

        // �����蔻��
        CollisionDetection(BulletR, bullet, bullets);
        CollisionDetection(FollowBulletR, followBullet, followBullets);

    }



     /// <summary>
     /// 放射状に広がるごく一般的な弾幕
     /// </summary>
     /// <param name="limit">弾幕の角度</param>
     /// <param name="difference">弾幕の角度の差</param>
     /// <param name="bullet">弾</param>
     /// <param name="bullets">bulletのclone</param>
    void RadialBullet(float limit, float difference, GameObject bullet, Transform bullets, Transform BossTrans)
    {
        this.AngleParam += this.AngleDif;
        if (this.AngleParam >= limit) this.AngleDif = -difference;
        if (this.AngleParam < 0f) this.AngleDif = difference;
        
        for (int angle = 0; angle < 20; ++angle)
        {
            InstBullet(BossTrans.position, Quaternion.Euler(0f, 0f, angle * 18f + this.AngleParam), bullet, bullets);
        }
    }



     /// <summary>
     /// 追いかけてくるうざい弾幕
     /// </summary>
     /// <param name="bullet">弾</param>
     /// <param name="bullets">bulletのclone</param>
    void FollowBullet(GameObject bullet, Transform bullets)
    {
        float angle, tmp;
        tmp = (transform.position.y - this.player.transform.position.y) / (transform.position.x - this.player.transform.position.x);
        angle = 180f + Mathf.Abs(Mathf.Atan(tmp)) * 180f / Mathf.PI;

        InstBullet(transform.position, Quaternion.Euler(0f, 0f, angle), bullet, bullets);
    }


    /// <summary>
    /// 八王子弾幕を実現するための関数
    /// </summary>
    /// <param name="bullet">弾</param>
    /// <param name="bullets">bulletのcloneたち</param>
    void HachioujiBullet(GameObject bullet, Transform bullets){
        this.HachioujiDelta += Time.deltaTime;
        if(HachioujiDelta >= 1f){
            BulletGen[0][0].GetComponent<Text>().text = "八";
            BulletGen[0][1].GetComponent<Text>().text = "王";
            BulletGen[0][2].GetComponent<Text>().text = "子";
        }
        if(HachioujiDelta >= 2f){
            BulletGen[1][0].GetComponent<Text>().text = "八";
            BulletGen[1][1].GetComponent<Text>().text = "王";
            BulletGen[1][2].GetComponent<Text>().text = "子";
        }
        if(HachioujiDelta >= 3f){
            BulletGen[2][0].GetComponent<Text>().text = "八";
            BulletGen[2][1].GetComponent<Text>().text = "王";
            BulletGen[2][2].GetComponent<Text>().text = "子";
        }
        if(HachioujiDelta >= 5f){
            for(int i = 0; i < 3; ++i){
                for(int j = 0; j < 3; ++j){
                    BulletGen[i][j].GetComponent<Text>().text = "　";
                    //RadialBullet(0, 0, bullet, bullets, BulletGen[i][j].transform);
                }
            }
            this.HachioujiDelta = 0f;
        }
    }


    /********************************
     * 
     * �e���𐶐�����֐�
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
     * �����蔻�������֐�
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

                // �e���ɂ߂�����߂�������Score�����Z
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

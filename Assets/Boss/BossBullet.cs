using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBullet : MonoBehaviour
{
    float span = 0.6f, delta = 0f, AngleParam = 0f, AngleDif = 1f, d;
    float span2 = 6f, delta2 = 0, HachioujiSpan = 1f, HachioujiDelta = 0f;
    int _p_;
    bool Hachiouji1, Hachiouji2, Hachiouji3;

    public static float PlayerR = 0.1f;
    float BulletR = 0.1f, FollowBulletR = 0.15f;

    [SerializeField] GameObject bullet, followBullet;
    GameObject player;
    GameObject[][] BulletGen = new GameObject[3][];

    Transform bullets, followBullets;
    
    void Start()
    {
        d = 0; _p_ = 1;
        Hachiouji1 = Hachiouji2 = Hachiouji3 = false;
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
            RadialBullet(20, 45, 1, bullet, bullets, transform.position);
        }else if (this.delta > this.span && BossHP.HP > 6f)
        {
            this.span = 0.3f;
            this.delta = 0f;
            RadialBullet(20, 10, 2, bullet, bullets, transform.position);
        }
        else if(this.delta > this.span)
        {
            this.span2 = 3f;
            this.span = 0.4f;
            this.delta = 0f;
            RadialBullet(32, 180, this.AngleDif, bullet, bullets, transform.position);
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
    void RadialBullet(int split, float limit, float difference, GameObject bullet, Transform bullets, Vector3 BossPos)
    {
        this.AngleParam += this.AngleDif;
        if (this.AngleParam >= limit) this.AngleDif = -difference;
        if (this.AngleParam < 0f) this.AngleDif = difference;
        
        for (int angle = 0; angle < split; ++angle)
        {
            InstBullet(BossPos, Quaternion.Euler(0f, 0f, angle * 360f / split + this.AngleParam), bullet, bullets);
        }
    }
    void RadialBullet(int split, GameObject bullet, Transform bullets, Vector3 BossPos)
    {
        for (int angle = 0; angle < split; ++angle)
        {
            InstBullet(BossPos, Quaternion.Euler(0f, 0f, angle * 360f / split), bullet, bullets);
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
        if(HachioujiDelta >= 1f && ! Hachiouji1){
            string tmp = HachioujiString(_p_);
            BulletGen[0][0].GetComponent<Text>().text = tmp[0].ToString();
            BulletGen[0][1].GetComponent<Text>().text = tmp[1].ToString();
            BulletGen[0][2].GetComponent<Text>().text = tmp[2].ToString();
            Hachiouji1 = true;
        }
        if(HachioujiDelta >= 2f && ! Hachiouji2){
            string tmp = HachioujiString(_p_);
            BulletGen[1][0].GetComponent<Text>().text = tmp[0].ToString();
            BulletGen[1][1].GetComponent<Text>().text = tmp[1].ToString();
            BulletGen[1][2].GetComponent<Text>().text = tmp[2].ToString();
            Hachiouji2 = true;
        }
        if(HachioujiDelta >= 3f && ! Hachiouji3){
            string tmp = HachioujiString(_p_);
            BulletGen[2][0].GetComponent<Text>().text = tmp[0].ToString();
            BulletGen[2][1].GetComponent<Text>().text = tmp[1].ToString();
            BulletGen[2][2].GetComponent<Text>().text = tmp[2].ToString();
            Hachiouji3 = true;
        }
        if(HachioujiDelta >= 5f){
            for(int i = 0; i < 3; ++i){
                for(int j = 0; j < 3; ++j){
                    BulletGen[i][j].GetComponent<Text>().text = "　";
                    RadialBullet(8, bullet, bullets, Camera.main.ScreenToWorldPoint(BulletGen[i][j].transform.position) + new Vector3(0, 0, 10));
                }
            }
            Hachiouji1 = Hachiouji2 = Hachiouji3 = false;
            ++_p_; if(_p_ == 10) _p_ = 1;
            this.HachioujiDelta = 0f;
        }
    }


    /// <summary>
    /// 八王子弾幕に用いる三字熟語を生成する関数
    /// </summary>
    /// <param name="p">なんかパラメタ</param>
    /// <returns>三字熟語</returns>
    string HachioujiString(int p){
        if(p == 1){
            int N = 6, t = Random.Range(0, N);
            if(t % N == 0) return "一番星";
            if(t % N == 1) return "一盃口";
            if(t % N == 2) return "一張羅";
            if(t % N == 3) return "清一色";
            if(t % N == 4) return "緑一色";
            if(t % N == 5) return "混一色";
        }
        if(p == 2){
            int N = 3, t = Random.Range(0, N);
            if(t % N == 0) return "二盃口";
            if(t % N == 1) return "二条城";
            if(t % N == 2) return "二分法";
            if(t % N == 3) return "二同刻";
        }
        if(p == 3){
            int N = 10, t = Random.Range(0, N);
            if(t % N == 0) return "三槓子";
            if(t % N == 1) return "三暗刻";
            if(t % N == 2) return "三十路";
            if(t % N == 3) return "三ノ宮";
            if(t % N == 4) return "御三家";
            if(t % N == 5) return "三つ巴";
            if(t % N == 6) return "三角州";
            if(t % N == 7) return "大三元";
            if(t % N == 8) return "小三元";
            if(t % N == 9) return "三連刻";
        }
        if(p == 4){
            int N = 8, t = Random.Range(0, N);
            if(t % N == 0) return "四槓子";
            if(t % N == 1) return "四暗刻";
            if(t % N == 2) return "四十肩";
            if(t % N == 3) return "四畳半";
            if(t % N == 4) return "四天王";
            if(t % N == 5) return "小四喜";
            if(t % N == 6) return "大四喜";
            if(t % N == 7) return "四連刻";
        }
        if(p == 5){
            int N = 4, t = Random.Range(0, N);
            if(t % N == 0) return "五賢帝";
            if(t % N == 1) return "五十肩";
            if(t % N == 2) return "五門斉";
            if(t % N == 3) return "五線譜";
        }
        if(p == 6){
            int N = 3, t = Random.Range(0, N);
            if(t % N == 0) return "六本木";
            if(t % N == 1) return "大六天";
            if(t % N == 2) return "六甲山";
        }
        if(p == 7){
            int N = 4, t = Random.Range(0, N);
            if(t % N == 0) return "七対子";
            if(t % N == 1) return "大七星";
            if(t % N == 2) return "七分丈";
            if(t % N == 3) return "初七日";
        }
        if(p == 8){
            return "八王子";
        }
        if(p == 9){
            int N = 4, t = Random.Range(0, N);
            if(t % N == 0) return "九番茶";
            if(t % N == 1) return "九条葱";
            if(t % N == 2) return "幺九牌";
            if(t % N == 3) return "断幺九";
        }
        // avoid error
        return "";
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

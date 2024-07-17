using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    float span = 0.1f, delta = 0f, AngleParam = 0f;
    float BossR = 0.3f, BulletR = 0.2f;

    [SerializeField] GameObject bullet;
    GameObject player, boss;

    Transform bullets;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");
        this.boss = GameObject.Find("Boss");
        bullet = GameObject.Find("Shooting");
        bullets = new GameObject("Shootings").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;

        if (Input.GetKey(KeyCode.Z))
            if (this.delta > this.span)
            {
                InstBullet(transform.position, Quaternion.Euler(0f, 0f, 0f));
                this.delta = 0f;
            }


        // Collision detection
        Vector2 BossPosition = this.boss.transform.position;
        foreach (Transform t in bullets)
        {
            if (t.gameObject.activeSelf)
            {
                Vector2 BulletPosition = t.position;
                Vector2 dir = BulletPosition - BossPosition;
                float d = dir.magnitude;

                if (d < BossR + BulletR)
                {
                    PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 100);
                    PlayerPrefs.Save();

                    BossHP.HP -= 0.01f;
                    GameObject director = GameObject.Find("GameDirector");
                    director.GetComponent<GameDirector>().DecreaseBossHP(BossHP.HP, BossHP.MaxHP);
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

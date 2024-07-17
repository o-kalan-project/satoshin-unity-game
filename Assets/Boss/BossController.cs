using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    // HP of a boss
    float HP = 20f, MaxHP = 20f;

    public Text Score;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            PlayerPrefs.SetInt("__STAGE__", PlayerPrefs.GetInt("__STAGE__") + 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Stage" + PlayerPrefs.GetInt("__STAGE__").ToString());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        --HP;

        /*“¾“_‚Ì‰ÁŽZ*/
        PlayerPrefs.SetInt("__SCORE__", PlayerPrefs.GetInt("__SCORE__") + 100);
        PlayerPrefs.Save();

        Score.text = PlayerPrefs.GetInt("__SCORE__").ToString();


        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().DecreaseBossHP(HP, MaxHP);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using unityroom.Api;

public class Score : MonoBehaviour
{
    public Text score;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Result")
        {
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.Save();
        }
        else
        {
            score.text = PlayerPrefs.GetInt("Score").ToString();
            if(PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("MaxScore"))
            {
                UnityroomApiClient.Instance.SendScore(1, PlayerPrefs.GetInt("Score"), ScoreboardWriteMode.Always);
                PlayerPrefs.SetInt("MaxScore", PlayerPrefs.GetInt("Score"));
                PlayerPrefs.Save();
            }
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Result")
        {
            score.text = PlayerPrefs.GetInt("Score").ToString();
        }
    }
}

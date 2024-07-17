using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text score;

    void Start()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();
    }

    void Update()
    {
        score.text = PlayerPrefs.GetInt("Score").ToString();
    }
}

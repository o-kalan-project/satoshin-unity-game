using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static void Result()
    {
        SceneManager.LoadScene("Result");
    }

    public static void Title()
    {
        SceneManager.LoadScene("Title");
    }

    public static void Battle()
    {
        BossHP.HP = PlayerHP.HP = 10f;
        SceneManager.LoadScene("BattleScene");
    }

    public static void Manual(){
        SceneManager.LoadScene("Manual");
    }
}

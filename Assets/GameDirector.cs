using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject PlayerHP;
    GameObject BossHP;

    // Start is called before the first frame update
    void Start()
    {
        this.PlayerHP = GameObject.Find("PlayerHP");
        this.BossHP = GameObject.Find("BossHP");
    }

    public void DecreasePlayerHP(float HP, float HPmax)
    {
        this.PlayerHP.GetComponent<Image>().fillAmount = Mathf.Max(HP / HPmax, 0.0f);
    }

    public void DecreaseBossHP(float HP, float HPmax)
    {
        this.BossHP.GetComponent<Image>().fillAmount = Mathf.Max(HP / HPmax, 0.0f);
    }
}

public class PlayerHP
{
    public static float HP = 10f, MaxHP = 10f;
}

public class BossHP
{
    public static float HP = 10f, MaxHP = 10f;
}
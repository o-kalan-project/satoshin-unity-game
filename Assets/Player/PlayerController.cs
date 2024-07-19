using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool right = false, left = false, up = false, down = false, slow = false;
    //public static float key = 0.25f;

    // Update is called once per frame
    void Update()
    {
        /***********************************
         * 
         * 挙動
         * 
        ************************************/
        if (Input.GetKeyDown(KeyCode.RightArrow)) right = true;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) left = true;
        if (Input.GetKeyDown(KeyCode.UpArrow)) up = true;
        if (Input.GetKeyDown(KeyCode.DownArrow)) down = true;

        /* 低速移動モード*/
        if (Input.GetKeyDown(KeyCode.LeftShift)) slow = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) slow = false;

        /* 動きを止める*/
        if (Input.GetKeyUp(KeyCode.RightArrow)) right = false;
        if (Input.GetKeyUp(KeyCode.LeftArrow)) left = false;
        if (Input.GetKeyUp(KeyCode.UpArrow)) up = false;
        if (Input.GetKeyUp(KeyCode.DownArrow)) down = false;


        /* 左右のキーで自機を動かす*/
        Vector3 Pos = transform.position;
        if (! slow)
        {
            if (right && Pos.x < 8) transform.Translate(0.04f, 0, 0);
            if (left && Pos.x > -8) transform.Translate(-0.04f, 0, 0);
            if (up && Pos.y < 5) transform.Translate(0, 0.04f, 0);
            if (down && Pos.y > -2) transform.Translate(0, -0.04f, 0);
        }
        else
        {
            if (right && Pos.x < 8) transform.Translate(0.01f, 0, 0);
            if (left && Pos.x > -8) transform.Translate(-0.01f, 0, 0);
            if (up && Pos.y < 5) transform.Translate(0, 0.01f, 0);
            if (down && Pos.y > -2) transform.Translate(0, -0.01f, 0);
        }
    }
}

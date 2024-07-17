using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Motion of bullets
        transform.position += 3 * transform.up * Time.deltaTime;
    }

    // Bullets being invisible, turns it non-active.
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}

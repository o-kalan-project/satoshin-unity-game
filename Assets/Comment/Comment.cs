using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Motion of bullets
        transform.position -= 2 * transform.right * Time.deltaTime;
    }

    // Bullets being invisible, turns it non-active.
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}

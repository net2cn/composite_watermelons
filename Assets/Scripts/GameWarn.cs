using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWarn : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.AddComponent<SpriteMask>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(collision.gameObject.GetComponent<SpriteMask>());
    }
}

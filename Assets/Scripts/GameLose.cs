using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLose : MonoBehaviour
{
    public bool IsLost = false;

    private int triggered = 0;
    private Coroutine routine;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        triggered += 1;
        if (triggered == 1)
        {
            IEnumerator gameLoseDetection()
            {
                Debug.Log($"Start game lose detection countdown on {Time.time}.");
                yield return new WaitForSeconds(5);
                if (triggered > 0)
                {
                    IsLost = true;
                    Debug.Log($"Game Lost on {Time.time}!");
                }
            }
            routine=StartCoroutine(gameLoseDetection());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        triggered -= 1;
        Debug.Log($"triggered: {triggered}");
        if (triggered == 0 && routine!=null)
        {
            Debug.Log($"Stop countdown coroutine");
            StopCoroutine(routine);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionZone : MonoBehaviour
{
    private bool isInRange = false;

    public bool IsInRange
    {
        get { return isInRange; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is out of range");
        }
    }
}

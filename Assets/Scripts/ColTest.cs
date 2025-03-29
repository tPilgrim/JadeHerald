using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColTest : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Point")
        {
            Debug.Log("Point");
        }

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player");
        }

        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy");
        }
    }
}

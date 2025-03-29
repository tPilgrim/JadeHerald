using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPatrol : MonoBehaviour
{
    public GameObject SnailComponent;

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Point")
        {
            SnailComponent.GetComponent<Snail>().Turn(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Point")
        {
            SnailComponent.GetComponent<Snail>().Turn(false);
        }
    }
}

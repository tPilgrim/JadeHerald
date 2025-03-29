using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailHide : MonoBehaviour
{
    public GameObject SnailComponent;
    private bool CanOpen;

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            CanOpen = true;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        if(CanOpen == true)
        {
            SnailComponent.GetComponent<Snail>().Hide(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            SnailComponent.GetComponent<Snail>().Hide(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            CanOpen = false;
        }
    }
}

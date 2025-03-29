using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElytraSnarkhRestrain : MonoBehaviour
{
    public GameObject EnemyComponent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            EnemyComponent.GetComponent<ElytraSnarkh>().Restrain(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            EnemyComponent.GetComponent<ElytraSnarkh>().Restrain(false);
        }
    }
}

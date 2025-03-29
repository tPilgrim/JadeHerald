using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject EnemyComponent;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (EnemyComponent.GetComponent<Snarkh>() != null)
            {
                EnemyComponent.GetComponent<Snarkh>().CanPatrol(true);
            }

            if (EnemyComponent.GetComponent<Phosp>() != null)
            {
                EnemyComponent.GetComponent<Phosp>().CanPatrol(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (EnemyComponent.GetComponent<Snarkh>() != null)
            {
                EnemyComponent.GetComponent<Snarkh>().CanPatrol(false);
            }

            if (EnemyComponent.GetComponent<Phosp>() != null)
            {
                EnemyComponent.GetComponent<Phosp>().CanPatrol(false);
            }
        }
    }
}

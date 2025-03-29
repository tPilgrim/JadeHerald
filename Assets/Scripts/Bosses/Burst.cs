using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    private GameObject PlayerComponent;
    public int MagicDamage;

    void Start()
    {
        PlayerComponent = GameObject.FindGameObjectWithTag("Player");
    }

    void ActivateCollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    void DeactivateCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(0, MagicDamage, 0, false);
        }

        if (other.gameObject.tag == "Sheild")
        {
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(0, MagicDamage, 0, true);
        }
    }
}

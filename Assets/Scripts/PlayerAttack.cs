using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject PlayerComponent;

    public int Damage;

    void Start()
    {
        PlayerComponent = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyHealth>().TakeDamage(Damage);
            PlayerComponent.GetComponent<PlayerController>().Regenerate();
        }
    }
}

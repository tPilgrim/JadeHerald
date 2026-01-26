using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public GameObject EnemyComponent;

    private IEnemy Enemy;

    private void Awake()
    {
        if (EnemyComponent.GetComponent<IEnemy>() != null)
            Enemy = EnemyComponent.GetComponent<IEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            if(Enemy!=null)
            Enemy.JumpCheck(true);

            if (EnemyComponent.GetComponent<Scout>() != null)
            {
                EnemyComponent.GetComponent<Scout>().StartJump(true);
            }

            if (EnemyComponent.GetComponent<Sentinel>() != null)
            {
                EnemyComponent.GetComponent<Sentinel>().StartJump(true);
            }

            if (EnemyComponent.GetComponent<Bogle>() != null)
            {
                EnemyComponent.GetComponent<Bogle>().StartJump(true);
            }
        }

        if (other.gameObject.tag == "Player")
        {
            if (Enemy != null)
                Enemy.JumpCheck(true);

            if (EnemyComponent.GetComponent<Warrior>() != null)
            {
                EnemyComponent.GetComponent<Warrior>().StartJump(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            if (Enemy != null)
                Enemy.JumpCheck(false);

            if (EnemyComponent.GetComponent<Scout>() != null)
            {
                EnemyComponent.GetComponent<Scout>().StartJump(false);
            }

            if (EnemyComponent.GetComponent<Sentinel>() != null)
            {
                EnemyComponent.GetComponent<Sentinel>().StartJump(false);
            }

            if (EnemyComponent.GetComponent<Bogle>() != null)
            {
                EnemyComponent.GetComponent<Bogle>().StartJump(false);
            }
        }

        if (other.gameObject.tag == "Player")
        {
            if (Enemy != null)
                Enemy.JumpCheck(false);

            if (EnemyComponent.GetComponent<Warrior>() != null)
            {
                EnemyComponent.GetComponent<Warrior>().StartJump(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnarkhPatrol : MonoBehaviour
{
    public GameObject EnemyComponent;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Point")
        {
            EnemyComponent.GetComponent<Snarkh>().StartRestrain(true);
        }
    }

    /*
        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.gameObject.tag == "Point")
            {
                EnemyComponent.GetComponent<Snarkh>().Turn(true);
                EnemyComponent.GetComponent<Snarkh>().DashRestrain(false);
            }

            if(other.gameObject.tag == "Enemy")
            {
                EnemyComponent.GetComponent<Snarkh>().Turn(true);
                EnemyComponent.GetComponent<Snarkh>().DashRestrain(false);
            }

            if(other.gameObject.tag == "Player")
            {
                EnemyComponent.GetComponent<Snarkh>().CombatRestrain(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Point")
            {
                EnemyComponent.GetComponent<Snarkh>().Turn(false);
                EnemyComponent.GetComponent<Snarkh>().DashRestrain(true);
            }

            if(other.gameObject.tag == "Enemy")
            {
                EnemyComponent.GetComponent<Snarkh>().Turn(false);
                EnemyComponent.GetComponent<Snarkh>().DashRestrain(true);
            }

            if(other.gameObject.tag == "Player")
            {
                EnemyComponent.GetComponent<Snarkh>().CombatRestrain(true);
            }
        }

        /*
        private float TurnTime = 0.05f;
        private float TurnTimeCounter;
        private bool TurnAround;

        void Update()
        {

            if(TurnTimeCounter < 0 && TurnAround == false)
            {
                EnemyComponent.GetComponent<Snarkh>().Turn(false);
            }
            else
            {
                TurnTimeCounter -= Time.deltaTime;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.gameObject.tag == "Enemy")
            {
                TurnTimeCounter = TurnTime;
                EnemyComponent.GetComponent<Snarkh>().Turn(true);
                TurnAround = true;
            }

            if(other.gameObject.tag == "Player")
            {
                EnemyComponent.GetComponent<Snarkh>().CanCombat(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Enemy")
            {
                TurnAround = false;
            }

            if(other.gameObject.tag == "Player")
            {
                EnemyComponent.GetComponent<Snarkh>().CanCombat(true);
            }
        }
        */
}

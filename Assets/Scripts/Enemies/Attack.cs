using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int PhysicalDamage;
    public int MagicDamage;
    public int PoiseDamage;
    //private bool HitSheild;
    ///public float XOffset;
    ///public float LaserLength;
    //private Vector2 StartPosition;
    //private LayerMask PlayerMask;

    private GameObject PlayerComponent;
    private Transform PlayerPosition;
    public GameObject Sheild;

    void Start()
    {
        PlayerComponent = GameObject.FindGameObjectWithTag("Player");
        //Sheild = GameObject.Find("Sheild");
        PlayerPosition = GameObject.Find("Player").transform;
    }

    void Update()
    {
        //PlayerPos = GameObject.Find("Player").transform;

        /*
        if (EnemyTransform.localScale.x == 1)
        {
            StartPosition = (Vector2)transform.position + new Vector2(XOffset, 0f);
            Debug.DrawRay(StartPosition, Vector2.left * LaserLength, Color.red);
        }

        if (EnemyTransform.localScale.x == -1)
        {
            StartPosition = (Vector2)transform.position + new Vector2(-XOffset, 0f);
            Debug.DrawRay(StartPosition, Vector2.right * LaserLength, Color.red);
        }
        */
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sheild" || other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerCollider")
        {
            if(Sheild.activeSelf == true)
            {
                if ((transform.position.x < PlayerPosition.position.x && PlayerPosition.localScale.x == -1) || (transform.position.x > PlayerPosition.position.x && PlayerPosition.localScale.x == 1))
                {
                    PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, true);
                    gameObject.SetActive(false);
                }
                else
                {
                    PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
                gameObject.SetActive(false);
            }
        }

        /*
        if (other.gameObject.tag == "Sheild")
        {
            HitSheild = true;
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, true);
            gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Player" && HitSheild == false)
        {
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
           /gameObject.SetActive(false);
        }
        */

            /*
            if (other.gameObject.tag == "Sheild" || other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerCollider")
            {
                if (EnemyTransform.localScale.x == 1)
                {
                    RaycastHit2D hit1 = Physics2D.Raycast(StartPosition, Vector2.left, LaserLength, PlayerMask);
                    if (hit1 != false)
                    {
                        if (hit1.collider.tag == "Sheild")
                        {
                            PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, true);
                            gameObject.SetActive(false);
                        }
                        if (hit1.collider.tag == "Player" || hit1.collider.tag == "PlayerCollider")
                        {
                            PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
                            gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
                        gameObject.SetActive(false);
                    }
                }

                if (EnemyTransform.localScale.x == -1)
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(StartPosition, Vector2.right, LaserLength, PlayerMask);
                    if (hit2 != false)
                    {
                        if (hit2.collider.tag == "Sheild")
                        {
                            PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, true);
                            gameObject.SetActive(false);
                        }
                        if (hit2.collider.tag == "Player" || hit2.collider.tag == "PlayerCollider")
                        {
                            PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
                            gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
                        gameObject.SetActive(false);
                    }
                }
            }
            */
    }
    /*
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sheild")
        {
            HitSheild = false;
        }
    }
    */
}

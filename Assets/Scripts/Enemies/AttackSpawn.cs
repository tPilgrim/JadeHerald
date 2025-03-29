using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpawn : MonoBehaviour
{
    public int PhysicalDamage;
    public int MagicDamage;
    public int PoiseDamage;

    private GameObject PlayerComponent;
    private Transform PlayerPosition;
    private GameObject Sheild;
    private Collider2D Col;

    void Start()
    {
        PlayerComponent = GameObject.FindGameObjectWithTag("Player");
        Sheild = GameObject.Find("Sheild");
        PlayerPosition = GameObject.Find("Player").transform;
        Col = GetComponent<PolygonCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sheild" || other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerCollider")
        {
            if (Sheild!=null)
            {
                if ((transform.position.x < PlayerPosition.position.x && PlayerPosition.localScale.x == -1) || (transform.position.x > PlayerPosition.position.x && PlayerPosition.localScale.x == 1))
                {
                    PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, true);
                    Col.enabled = false;
                }
                else
                {
                    PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
                    Col.enabled = false;
                }
            }
            else
            {
                PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
                Col.enabled = false;
            }
        }
    }
}

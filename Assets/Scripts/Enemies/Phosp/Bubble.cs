using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D BubbleRb;
    private GameObject PlayerComponent;
    private Transform Player;

    public int MagicDamage;
    public ParticleSystem SpawnParticles;
    private bool HitGround;
    public float ChargeTime;
    private bool Attack = false;
    private float RotationZ;
    private Vector3 Direction;
    public GameObject CollisonParticles;
    public bool Rotate;

    void Start()
    {
        BubbleRb = GetComponent<Rigidbody2D>();
        PlayerComponent = GameObject.FindGameObjectWithTag("Player");
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(Charge());
        SpawnParticles.Play();
    }

    void FixedUpdate()
    {
        if (Attack == true)
        {
            BubbleRb.velocity = Direction * Speed * Time.deltaTime * 100;
        }
    }

    private IEnumerator Charge()
    {
        yield return new WaitForSeconds(ChargeTime);
        Direction = Player.transform.position - transform.position;
        Direction.Normalize();
        RotationZ = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        if(Rotate == true)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, RotationZ);
        }
        Attack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Attack")
        {
            HitGround = true;
            Speed = 0f;
            //Instantiate(CollisonParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player" && HitGround == false)
        {
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(0, MagicDamage, 0, false);
            //Instantiate(CollisonParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Sheild" && HitGround == false)
        {
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(0, MagicDamage, 0, true);
            //Instantiate(CollisonParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}

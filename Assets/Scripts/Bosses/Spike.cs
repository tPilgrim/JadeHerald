using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D SpikeRb;
    private GameObject PlayerComponent;

    public int MagicDamage;
    public ParticleSystem SpawnParticles;
    private bool HitGround;
    public float ChargeTime;
    private bool Attack = false;
    private float RotationZ;
    private Vector3 Direction;
    public GameObject CollisonParticles;
    public bool Left;

    public int minRotation;
    public int maxRotation;

    void Start()
    {
        SpikeRb = GetComponent<Rigidbody2D>();
        PlayerComponent = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Charge());
        SpawnParticles.Play();
    }

    void FixedUpdate()
    {
        if (Attack == true)
        {
            SpikeRb.linearVelocity = Direction * Speed * Time.deltaTime * 100;
        }
    }

    private IEnumerator Charge()
    {
        yield return new WaitForSeconds(ChargeTime);
        if(Left == true)
        {
            Direction.x = Random.Range(-maxRotation, minRotation);
        }
        else
        {
            Direction.x = Random.Range(minRotation, maxRotation);
        }
        Direction.y = Random.Range(-1, -10);
        Direction.Normalize();
        RotationZ = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, RotationZ);
        Attack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
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

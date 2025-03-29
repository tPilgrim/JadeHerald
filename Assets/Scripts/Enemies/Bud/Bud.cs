using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bud : MonoBehaviour
{
    private Animator Anim;
    private Transform Player;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;

    private bool CanCombat;
    private float Speed;
    public float FollowSpeed;
    private bool IsDead;
    private bool IsAttacking;
    private bool IsFalling;
    private bool LandBurst;
    public GameObject Burst;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
    }

    void Update()
    {
        if(IsDead == false)
        {
            Fall();
            if(IsAttacking == false && IsFalling == false)
            {
                Follow();
            }
        }
    }

    void Turn()
    {
        Vector3 Scaler = transform.localScale;

        if (Player.transform.position.x < transform.position.x && Scaler.x < 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        if (Player.transform.position.x > transform.position.x && Scaler.x > 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    public void Combat(bool EnterCombat)
    {
        if(EnterCombat == true)
        {
            CanCombat = EnterCombat;
        }
    }

    void Follow()
    {
        if (CanCombat == true)
        {
            Turn();
            Speed = FollowSpeed;
            Anim.SetBool("IsRunning", true);

            if ((Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x))
            {
                Speed = 0;
                Anim.SetBool("IsRunning", false);
            }
            else if (Player.transform.position.x < transform.position.x)
            {
                Speed = -FollowSpeed;
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                Speed = FollowSpeed;
            }

            EnemyRb.velocity = new Vector2(Speed, EnemyRb.velocity.y);
        }
    }

    void Fall()
    {
        Anim.SetBool("IsFalling", IsFalling);

        if (EnemyRb.velocity.y < -1)
        {
            IsFalling = true;
            EnemyRb.velocity = new Vector2(0f, EnemyRb.velocity.y);
        }
        else
        {
            IsFalling = false;
        }

        if(EnemyRb.velocity.y < -5)
        {
            LandBurst = true;
        }

        if(LandBurst == true && EnemyRb.velocity.y >= 0)
        {
            Attack();
        }
    }

    public void StartAttack(bool CanAttack)
    {
        if(CanAttack == true)
        {
            IsAttacking = true;
            Anim.SetTrigger("IsAttacking");
            EnemyRb.velocity = new Vector2(0f, EnemyRb.velocity.y);
        }
    }

    void Attack()
    {
        Instantiate(Burst, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void Death()
    {
        EnemyRb.velocity = new Vector2(0f, 0f);
        gameObject.layer = DefaultLayer;
        IsDead = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phosp : MonoBehaviour
{
    private Animator Anim;
    public Animator SpellAnimation;
    private Transform Player;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;
    private Vector3 Scaler;
    public ParticleSystem DeathParticles;

    private bool IsInRangedCombat;
    private bool IsInCombat;
    private bool Cooldown;
    private bool CanTurn;
    private bool CanFollow;
    private bool IsDead;

    public float Speed;
    public float DashSpeed;

    private bool IsAttacking;
    private bool IsDashing;
    private int AttackCount;
    private int DashCount;
    private int WhichAttack;
    private bool NextAttack = true;
    public GameObject AttackHitbox;
    public GameObject AttackPoint;

    public Transform SpellPoint;
    public GameObject Bubble;

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
            Ranged();
            Follow();
            Turn();
            Melee();
        }
        else
        {
            StopCoroutine(Attack());
            StopCoroutine(Spell());
            StopCoroutine(Dash());
            EnemyRb.velocity = new Vector2(0f, 0f);
            AttackPoint.SetActive(false);
        }
    }

    void Turn()
    {
        Scaler = transform.localScale;

        if(IsAttacking == false && IsDashing == false || CanTurn == true)
        {
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
    }

    public void CanPatrol(bool CanPatrol)
    {
        CanFollow = CanPatrol;
        /*
        if (CanFollow == false)
        {
            Debug.Log("asd");
            StopCoroutine(Dash());
            IsDashing = false;
            IsAttacking = false;
            EnemyRb.velocity = new Vector2(0f, 0f);
            NextAttack = true;
            WhichAttack = Random.Range(1, 10);
            Anim.SetBool("IsDashing", false);
        }
        */
    }

    public void RangedCombat(bool EnterRangedCombat)
    {
        IsInRangedCombat = EnterRangedCombat;
    }

    public void Combat(bool EnterCombat)
    {
        IsInCombat = EnterCombat;

        if(EnterCombat == false)
        {
            DashCount = 3;
        }
    }

    void Ranged()
    {
        if (IsInRangedCombat == true && IsAttacking == false && IsDashing == false && Cooldown == false && IsInCombat == false)
        {
            StartCoroutine(Spell());
        }
    }
    /*
        private IEnumerator Spell()
        {
            IsAttacking = true;
            Anim.SetBool("IsCasting", true);
            yield return new WaitForSeconds(1.65f);
            if(IsDead == false)
            {
                Instantiate(Bubble, SpellPoint.position, SpellPoint.rotation);
            }
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(SpellCooldown());
            Anim.SetBool("IsRunning", false);
            Anim.SetBool("IsCasting", false);
            DashCount = 3;
            IsAttacking = false;
        }
    */

    private IEnumerator Spell()
    {
        IsAttacking = true;
        Anim.SetBool("IsCasting", true);
        yield return new WaitForSeconds(2.83f);
        StartCoroutine(SpellCooldown());
        Anim.SetBool("IsRunning", false);
        Anim.SetBool("IsCasting", false);
        DashCount = 3;
        IsAttacking = false;
    }

    void CastBubble()
    {
        Instantiate(Bubble, SpellPoint.position, SpellPoint.rotation);
    }

    private IEnumerator SpellCooldown()
    {
        Cooldown = true;
        yield return new WaitForSeconds(5f);
        Cooldown = false;
    }

    void Follow()
    {
        if(IsAttacking == false && IsDashing == false && CanFollow == true)
        {
            Anim.SetBool("IsRunning", true);

            if (Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x)
            {
                EnemyRb.velocity = new Vector2(0f, 0f);
                Anim.SetBool("IsRunning", false);
            }
            else if (Player.transform.position.x < transform.position.x)
            {
                EnemyRb.velocity = new Vector2(-Speed, 0f);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                EnemyRb.velocity = new Vector2(Speed, 0f);
            }
        }
        else
        {
            Anim.SetBool("IsRunning", false);
        }

        if(IsAttacking == true)
        {
            EnemyRb.velocity = new Vector2(0f, 0f);
        }

        if(IsDashing == true)
        {
            if (Player.transform.position.x < transform.position.x)
            {
                EnemyRb.velocity = new Vector2(DashSpeed, 0f);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                EnemyRb.velocity = new Vector2(-DashSpeed, 0f);
            }
        }
    }

    void Melee()
    {
        if (IsInCombat == true && IsAttacking == false && NextAttack == true)
        {
            if (WhichAttack < 3)
            {
                if(CanFollow == true)
                {
                    NextAttack = false;

                    if (DashCount < 2)
                    {
                        DashCount++;
                        StartCoroutine(Dash());
                    }
                    else
                    {
                        DashCount = 0;
                        StartCoroutine(Attack());
                    }
                }
            }
            else
            {
                NextAttack = false;

                if (AttackCount < 1)
                {
                    AttackCount++;
                    StartCoroutine(Attack());
                }
                else if (CanFollow == true)
                {
                    AttackCount = 0;
                    StartCoroutine(Dash());
                }
            }
        }

        if(CanFollow == false)
        {
            WhichAttack = 8;
            AttackCount = 0;
        }
    }

    private IEnumerator Attack()
    {
        Anim.SetBool("IsAttacking", true);
        IsAttacking = true;
        yield return new WaitForSeconds(0.25f);
        CanTurn = true;
        yield return new WaitForSeconds(0.925f);
        CanTurn = false;
        if(IsDead == false)
        {
            AttackHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        AttackHitbox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        IsAttacking = false;
        Anim.SetBool("IsAttacking", false);
        NextAttack = true;
        WhichAttack = Random.Range(1, 10);
    }

    void Cast()
    {
        SpellAnimation.SetTrigger("Cast");
    }

    private IEnumerator Dash()
    {
        Anim.SetBool("IsDashing", true);
        IsAttacking = true;
        yield return new WaitForSeconds(0.1f);
        IsDashing = true;
        yield return new WaitForSeconds(0.52f);
        IsDashing = false;
        yield return new WaitForSeconds(0.1f);
        StopCoroutine(SpellCooldown());
        Cooldown = false;
        IsAttacking = false;
        NextAttack = true;
        WhichAttack = Random.Range(1, 10);
        Anim.SetBool("IsDashing", false);
    }

    void Death()
    {
        IsDead = true;
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}

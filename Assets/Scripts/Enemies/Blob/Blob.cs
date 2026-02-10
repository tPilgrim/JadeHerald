using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    private Animator Anim;
    private Transform Player;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;
    public ParticleSystem DeathParticles;

    private float Speed;
    public float FollowSpeed;
    private bool CanMove = true;

    public float JumpForce;
    public float JumpSpeed;
    private bool JumpAgain = true;
    private bool CanJump;
    private bool IsJumping;
    public GameObject JumpCheck;

    private float SleepTimeCounter;
    public float SleepTime;
    private bool StartCounting;

    private bool CanCombat;
    public GameObject AttackRange;
    public GameObject AttackHitbox;
    public ParticleSystem ChargeParticles;
    public ParticleSystem AttackParticles;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
    }

    void Update()
    {
        Sleep();
        Jump();

        if (!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Sleep") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Attack") && EnemyRb.linearVelocity.y >= 0 && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Death") && IsJumping == false)
        {
            Turn();
        }

        if (this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Awake") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Attack") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Sleep") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Death"))
        {
            CanMove = false;
        }
        else
        {
            CanMove = true;
        }

        if (EnemyRb.linearVelocity.y < 0)
        {
            Anim.SetBool("IsRunning", false);
        }

    }

    void FixedUpdate()
    {
        Follow();
    }

    public void Combat(bool EnterCombat)
    {
        if (EnterCombat == true)
        {
            CanCombat = EnterCombat;
            Anim.SetBool("IsSleeping", false);
            Anim.SetTrigger("IsAwake");
            JumpCheck.SetActive(true);
            SleepTimeCounter = SleepTime;
            StartCounting = false;
        }
        else
        {
            StartCounting = true;
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

    void Follow()
    {
        if (CanCombat == true && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Attack") && EnemyRb.linearVelocity.y >= 0 && IsJumping == false && JumpAgain == true)
        {
            Speed = FollowSpeed;
            Anim.SetBool("IsRunning", true);

            if ((Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x) || CanMove == false)
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

            EnemyRb.linearVelocity = new Vector2(Speed, 0f);
        }
    }

    public void StartJump(bool ShouldJump)
    {
        CanJump = ShouldJump;
    }

    void Jump()
    {
        if (Player.transform.position.x < transform.position.x)
        {
            JumpSpeed = Mathf.Abs(JumpSpeed) * -1f;
        }
        else
        {
            JumpSpeed = Mathf.Abs(JumpSpeed);
        }

        if (CanJump == true && JumpAgain == true && IsJumping == false && CanMove == true && EnemyRb.linearVelocity.y >= 0 && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Dash"))
        {
            StartCoroutine(JumpStart());
        }

        if (CanJump == false)
        {
            JumpAgain = true;
        }
        else
        {
            Anim.SetBool("IsRunning", false);
        }
    }

    IEnumerator JumpStart()
    {
        JumpAgain = false;
        IsJumping = true;
        Anim.SetBool("IsJumping", true);
        EnemyRb.linearVelocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(0.4f);
        EnemyRb.linearVelocity = new Vector2(JumpSpeed, JumpForce);
        yield return new WaitForSeconds(0.3f);
        EnemyRb.linearVelocity = new Vector2(JumpSpeed, EnemyRb.linearVelocity.y);
        yield return new WaitForSeconds(0.3f);
        EnemyRb.linearVelocity = new Vector2(0f, EnemyRb.linearVelocity.y);
        yield return new WaitForSeconds(0.1f);
        IsJumping = false;
        Anim.SetBool("IsJumping", false);
    }

    void Sleep()
    {
        if (StartCounting == true)
        {
            SleepTimeCounter -= Time.deltaTime;
        }

        if (SleepTimeCounter <= 0)
        {
            Anim.SetBool("IsSleeping", true);
        }
    }

    public void StartAttack(bool CanAttack)
    {
        if (CanAttack == true && IsJumping == false && EnemyRb.linearVelocity.y == 0)
        {
            EnemyRb.linearVelocity = new Vector2(0f, 0f);
            Anim.SetBool("IsAttacking", true);
        }
    }

    private IEnumerator Spit()
    {
        ChargeParticles.Play();
        yield return new WaitForSeconds(0.85f);
        if(!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sentinel Death"))
        {
            AttackParticles.Play();
            AttackHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(0.45f);
        AttackHitbox.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        Anim.SetBool("IsAttacking", false);
    }

    void Death()
    {
        ChargeParticles.Stop();
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}

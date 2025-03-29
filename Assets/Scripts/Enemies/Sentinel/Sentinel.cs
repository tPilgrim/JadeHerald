using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel : MonoBehaviour
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
    private bool NextAttack = true;
    private bool IsAttacking;
    private bool Stop;
    private bool IsSleeping;

    private AudioSource AudioManager;
    public AudioSource Footsteps;
    public AudioClip AttackSound;
    public AudioClip AwakeSound;
    public AudioClip JumpSound;
    public AudioClip LandSound;
    public AudioClip DeathSound;
    public ParticleSystem AwakeParticles;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
        Anim.SetBool("IsSleeping", true);
        AudioManager = GetComponent<AudioSource>();
    }

    void Update()
    {
        Sleep();
        Jump();

        if (!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sleep") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && EnemyRb.velocity.y >= 0 && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && IsJumping == false)
        {
            Turn();
        }

        if (Stop == true || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Awake") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Sleep") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            CanMove = false;
        }
        else
        {
            CanMove = true;
        }

        if (EnemyRb.velocity.y < 0)
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
            IsSleeping = false;
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

    private bool FirstPlay = true;

    void PlayAwakeSound()
    {
        if (!FirstPlay)
        {
            AwakeParticles.Play();
            AudioManager.PlayOneShot(AwakeSound, 0.6f);
        }
        FirstPlay = false;
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
        if(this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if(!Footsteps.isPlaying)
                Footsteps.Play();
        }
        else
        {
            Footsteps.Stop();
        }

        if (CanCombat == true && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && EnemyRb.velocity.y >= 0 && IsJumping == false && JumpAgain == true && IsSleeping == false)
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

            EnemyRb.velocity = new Vector2(Speed, 0f);
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

        if (CanJump == true && JumpAgain == true && IsJumping == false && CanMove == true && EnemyRb.velocity.y >= 0)
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
        EnemyRb.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(0.4f);
        AudioManager.PlayOneShot(JumpSound, 0.6f);
        EnemyRb.velocity = new Vector2(JumpSpeed, JumpForce);
        yield return new WaitForSeconds(0.3f);
        EnemyRb.velocity = new Vector2(JumpSpeed, EnemyRb.velocity.y);
        yield return new WaitForSeconds(0.3f);
        EnemyRb.velocity = new Vector2(JumpSpeed, EnemyRb.velocity.y);
        yield return new WaitForSeconds(0.25f);
        EnemyRb.velocity = new Vector2(0f, EnemyRb.velocity.y);
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
            IsSleeping = true;
            EnemyRb.velocity = new Vector2(0f, 0f);
        }
    }

    public void StartAttack(bool CanAttack)
    {
        Stop = CanAttack;

        if (CanAttack == true && IsJumping == false && EnemyRb.velocity.y == 0 && NextAttack == true && IsAttacking == false)
        {
            EnemyRb.velocity = new Vector2(0f, 0f);
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        IsAttacking = true;
        Anim.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.5f);
        if (!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            AudioManager.PlayOneShot(AttackSound, 0.4f);
            AttackHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(0.33f);
        NextAttack = false;
        StartCoroutine(AttackAgain());
        AttackHitbox.SetActive(false);
        Anim.SetBool("IsAttacking", false);
        IsAttacking = false;
    }

    private IEnumerator AttackAgain()
    {
        yield return new WaitForSeconds(1f);
        AttackRange.SetActive(false);
        AttackRange.SetActive(true);
        NextAttack = true;
    }

    private bool FirstCollision = true;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            if(!FirstCollision)
            {
                AudioManager.PlayOneShot(LandSound, 0.4f);
            }
            FirstCollision = false;
        }
    }

    void Death()
    {
        AudioManager.PlayOneShot(DeathSound, 0.4f);
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}

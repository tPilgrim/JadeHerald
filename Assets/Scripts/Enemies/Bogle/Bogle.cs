using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bogle : MonoBehaviour
{
    private Animator Anim;
    private Transform Player;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;
    public ParticleSystem DeathParticles;

    private float Speed;
    public float FollowSpeed;
    private bool CanMove = true;
    private bool IsFaceing;

    public float JumpForce;
    public float JumpSpeed;
    private bool IsJumping;
    private bool JumpAgain = true;
    private bool CanJump;
    public GameObject JumpCheck;

    private float SleepTimeCounter;
    public float SleepTime;
    private bool StartCounting;

    private bool CanCombat;
    private int WhichAttack;
    
    private bool DidDamage;

    private bool IsAttacking;
    public GameObject AttackHitbox;
    public GameObject DashHitbox;
    public GameObject SpitHitbox;
    public ParticleSystem ChargeParticles;
    public ParticleSystem SpitParticles;

    private AudioSource AudioManager;
    public AudioSource Footsteps;
    public AudioClip AttackSound;
    public AudioClip AwakeSound;
    public AudioClip DashSound;
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
        AudioManager = GetComponent<AudioSource>();
    }

    void Update()
    {
        Sleep();
        Jump();

        if (!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Sleep") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Death") && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Awake") && IsJumping == false && IsAttacking == false)
        {
            Turn();
        }

        if(this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Stance"))
        {
            Turn();
        }

        if (this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Awake") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Attack") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Asleep") || this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Death") || IsAttacking == true)
        {
            CanMove = false;
        }
        else
        {
            CanMove = true;
        }

        if(this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Death"))
        {
            ChargeParticles.Stop();
            SpitParticles.Stop();
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
            Anim.SetTrigger("IsAwake");
            Anim.SetBool("IsSleeping", false);
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

    private Vector3 Scaler;

    void Turn()
    {
        Scaler = transform.localScale;

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
        if (this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Run"))
        {
            if (!Footsteps.isPlaying)
                Footsteps.Play();
        }
        else
        {
            Footsteps.Stop();
        }

        if (CanCombat == true && EnemyRb.velocity.y >= 0 && IsJumping == false && JumpAgain == true)
        {
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

            if(IsAttacking == false)
            {
                EnemyRb.velocity = new Vector2(Speed, 0f);
            }
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

        if (CanJump == true && JumpAgain == true && IsJumping == false && CanMove == true && EnemyRb.velocity.y >= 0 && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scout Dash"))
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
        EnemyRb.velocity = new Vector2(0f, EnemyRb.velocity.y);
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

    private int AttackCount;
    private int SpitCount;
    private int StanceCount;

    public void StartAttack(bool CanAttack)
    {
        if (CanAttack == true && IsJumping == false && IsAttacking == false && CanMove == true)
        {
            WhichAttack = Random.Range(1, 11);

            if (WhichAttack >= 1 && WhichAttack <= 4 && IsAttacking == false)
            {
                if(AttackCount < 2)
                {
                    AttackCount++;
                    StartCoroutine(Attack());
                }
                else
                {
                    AttackCount = 0;
                    WhichAttack = Random.Range(5, 11);
                }
            }

            if (WhichAttack >= 5 && WhichAttack <= 7 && IsAttacking == false)
            {
                if (SpitCount < 2)
                {
                    SpitCount++;
                    StartCoroutine(Spit());
                }
                else
                {
                    SpitCount = 0;
                    StartCoroutine(Attack());
                }
            }

            if (WhichAttack >= 8 && WhichAttack <= 10 && IsAttacking == false)
            {
                if (StanceCount < 1)
                {
                    StanceCount++;
                    StartCoroutine(Stance());
                }
                else
                {
                    StanceCount = 0;
                    WhichAttack = Random.Range(1, 8);
                }
            }
        }
    }

    IEnumerator Attack()
    {
        IsAttacking = true;
        EnemyRb.velocity = new Vector2(0f, 0f);
        Anim.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.75f);
        StartCoroutine(Slash());
        yield return new WaitForSeconds(0.915f);
        StartCoroutine(DashSlash());
        yield return new WaitForSeconds(0.4f);
        Anim.SetBool("IsAttacking", false);
        IsAttacking = false;
    }

    IEnumerator Spit()
    {
        IsAttacking = true;
        EnemyRb.velocity = new Vector2(0f, 0f);
        Anim.SetBool("IsSpitting", true);
        ChargeParticles.Play();
        yield return new WaitForSeconds(0.75f);
        SpitHitbox.SetActive(true);
        SpitParticles.Play();
        yield return new WaitForSeconds(0.5f);
        SpitHitbox.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        Anim.SetBool("IsSpitting", false);
        IsAttacking = false;
    }

    IEnumerator DashSlash()
    {
        if(!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Death"))
        {
            AttackHitbox.SetActive(true);
            AudioManager.PlayOneShot(AttackSound, 0.4f);
            Scaler = transform.localScale;
            EnemyRb.velocity = new Vector2(Scaler.x * -6f, 0f);
            /*
            if(Player.transform.position.x < transform.position.x)
            {
                EnemyRb.velocity = new Vector2(-6f, 0f);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                EnemyRb.velocity = new Vector2(6f, 0f);
            }
            */
        }
        yield return new WaitForSeconds(0.15f);
        EnemyRb.velocity = new Vector2(0f, 0f);
        AttackHitbox.SetActive(false);
    }

    IEnumerator ChargedDashSlash()
    {
        if (!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Death"))
        {
            DashHitbox.SetActive(true);
            AudioManager.PlayOneShot(AttackSound, 0.4f);
            Scaler = transform.localScale;
            EnemyRb.velocity = new Vector2(Scaler.x * -6f, 0f);
            /*
            if (Player.transform.position.x < transform.position.x)
            {
                EnemyRb.velocity = new Vector2(-6f, 0f);
            }
            else if (Player.transform.position.x > transform.position.x)
            {
                EnemyRb.velocity = new Vector2(6f, 0f);
            }
            */
        }
        yield return new WaitForSeconds(0.15f);
        EnemyRb.velocity = new Vector2(0f, 0f);
        DashHitbox.SetActive(false);
    }

    IEnumerator Slash()
    {
        AttackHitbox.SetActive(true);
        AudioManager.PlayOneShot(AttackSound, 0.4f);
        yield return new WaitForSeconds(0.1f);
        EnemyRb.velocity = new Vector2(0f, 0f);
        AttackHitbox.SetActive(false);
    }

    IEnumerator Stance()
    {
        IsAttacking = true;
        EnemyRb.velocity = new Vector2(0f, 0f);
        Anim.SetBool("Stance", true);
        gameObject.GetComponent<EnemyHealth>().Defend(true);
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<EnemyHealth>().Defend(false);
        Anim.SetBool("Stance", false);
        if(!this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Bogle Parry"))
        {
            IsAttacking = false;
        }
    }

    public void StartParry()
    {
        StartCoroutine(Parry());
    }

    IEnumerator Parry()
    {
        IsAttacking = true;
        gameObject.GetComponent<EnemyHealth>().Defend(false);
        Anim.SetBool("Parry", true);
        yield return new WaitForSeconds(0.75f);
        Anim.SetBool("Stance", false);
        StartCoroutine(ChargedDashSlash());
        yield return new WaitForSeconds(0.425f);
        Anim.SetBool("Parry", false);
        IsAttacking = false;
    }

    void Death()
    {
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}

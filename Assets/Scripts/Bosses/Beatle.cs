using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class Beatle : MonoBehaviour, IEnemy
{
    private Animator Anim;
    public Animator SpellAnimation;
    private Transform Player;
    private Rigidbody2D EnemyRb;
    private int DefaultLayer;
    private Vector3 Scaler;
    public ParticleSystem DeathParticles;
    public ParticleSystem LandParticles;
    public ParticleSystem BurstParticles;
    public ParticleSystem StruckParticles;

    private bool IsMelee;
    private bool Cooldown;
    private float CooldownTime;
    private bool CanTurn = true;
    private bool IsDead;

    public float Speed;
    public float DashSpeed;
    public float FloatSpeed;

    private bool IsAttacking;
    private bool IsDashing;
    private bool FloatUp;
    private int AttackCount;
    private int Randomizer;
    private int JumpRand;
    private bool IsJumping;
    public float JumpSpeed;
    public float JumpHeight;
    private bool JumpDirection;
    private int WhichAttack;
    public GameObject[] AttackHitbox;
    public GameObject AttackPoint;

    public Transform SpellPoint;
    public Transform SpawnPoint;
    public float SpawnRate;
    private bool CanSpawn;
    public GameObject Bubble;
    public GameObject SpikeRight;
    public GameObject SpikeLeft;
    public GameObject BurstObject;
    public Transform BurstPoint;

    public Renderer MaterialRenderer;
    public Material[] Glows;

    private AudioSource AudioManager;
    public AudioSource AudioRun;
    public AudioClip Attack1Sound;
    public AudioClip Attack2Sound;
    public AudioClip Attack3Sound;
    public AudioClip Attack4Sound;
    public AudioClip Attack5Sound;
    public AudioClip Attack6Sound;
    public AudioClip Attack7Sound;
    public AudioClip SpikeSound;
    public AudioClip RunSound;
    void Start()
    {
        MaterialRenderer.sharedMaterial = Glows[8];
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRb = GetComponent<Rigidbody2D>();
        DefaultLayer = LayerMask.NameToLayer("Dead");
        AudioManager = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (IsDead == false)
        {
            SpellCooldown();
            Follow();
            if (IsAttacking == false && IsDashing == false && CanTurn == true && WhichAttack != 7)
            {
                Turn();
            }
            if(IsMelee)
            {
                //ChooseAttack(WhichAttack);
            }
        }
        else
        {
            EnemyRb.velocity = new Vector2(0f, 0f);
            AttackPoint.SetActive(false);
        }
    }

    public void Turn()
    {
        Scaler = transform.localScale;

        if (Player.transform.position.x < transform.position.x && Scaler.x > 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        if (Player.transform.position.x > transform.position.x && Scaler.x < 0)
        {
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            LandParticles.Play();
        }
    }

    public void Combat(bool EnterCombat, int AreaId)
    {
        IsMelee = EnterCombat;

        Randomizer = Random.Range(1, 101);

        if (AreaId == 0)
        {
            if (Randomizer >= 1 && Randomizer <= 60)
            {
                WhichAttack = 1;
            }
            else if (Randomizer >= 61 && Randomizer <= 100)
            {
                WhichAttack = 6;
            }
        }
        else if (AreaId == 1)
        {
            if (Randomizer >= 1 && Randomizer <= 60)
            {
                WhichAttack = 3;
            }
            else if (Randomizer >= 61 && Randomizer <= 100)
            {
                WhichAttack = 6;
            }
        }
        else
        {
            if (Randomizer >= 1 && Randomizer <= 70)
            {
                WhichAttack = 7;
            }
            else if (Randomizer >= 31 && Randomizer <= 100)
            {
                WhichAttack = 6;
            }
        }

        ExecuteAttack();
    }

    public void JumpCheck(bool ShouldJump)
    {
        if (ShouldJump == true)
        {
            JumpRand = Random.Range(1, 11);

            if (1 <= JumpRand && JumpRand <= 4)
            {
                WhichAttack = 2;
                ExecuteAttack();
            }
        }
    }

    void SpellCooldown()
    {
        if (CooldownTime > 0)
        {
            Cooldown = true;
            CooldownTime -= Time.deltaTime;
        }
        else
        {
            Cooldown = false;
            //ChooseAttack(WhichAttack);
        }
    }

    void Follow()
    {
        if (IsAttacking == false && IsDashing == false)
        {
            if (!AudioRun.isPlaying)
                AudioRun.Play();
            MaterialRenderer.sharedMaterial = Glows[0];
            Anim.SetBool("IsRunning", true);
            SpellAnimation.SetBool("IsRunning", true);

            if (Player.position.x + 0.1 > transform.position.x && Player.position.x - 0.1 < transform.position.x)
            {
                AudioRun.Stop();
                EnemyRb.velocity = new Vector2(0f, 0f);
                Anim.SetBool("IsRunning", false);
                SpellAnimation.SetBool("IsRunning", false);
                MaterialRenderer.sharedMaterial = Glows[8];
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
            AudioRun.Stop();
            Anim.SetBool("IsRunning", false);
            SpellAnimation.SetBool("IsRunning", false);
        }

        if (IsAttacking == true && !this.Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            EnemyRb.velocity = new Vector2(0f, 0f);
        }

        Scaler = transform.localScale;

        if (IsDashing == true)
        {
            if (Scaler.x > 0)
            {
                EnemyRb.velocity = new Vector2(DashSpeed, 0f);
            }
            else if (Scaler.x < 0)
            {
                EnemyRb.velocity = new Vector2(-DashSpeed, 0f);
            }
        }

        if (IsJumping == true)
        {
            if (JumpDirection == true)
            {
                EnemyRb.velocity = new Vector2(-JumpSpeed, JumpHeight);
            }
            else
            {
                EnemyRb.velocity = new Vector2(JumpSpeed, JumpHeight);
            }
        }

        if (FloatUp == true)
        {
            EnemyRb.velocity = new Vector2(EnemyRb.velocity.x, FloatSpeed);
        }
    }

    void ChooseAttack(int PrevAttack)
    {
        //if (!IsAttacking)
        //{
        //    do
        //    {
        //        Randomizer = Random.Range(1, 101);

        //        if (Randomizer >= 1 && Randomizer <= 35)
        //        {
        //            WhichAttack = 1;
        //        }
        //        else if (Randomizer >= 36 && Randomizer <= 60)
        //        {
        //            WhichAttack = 3;
        //        }
        //        else if (Randomizer >= 61 && Randomizer <= 80)
        //        {
        //            WhichAttack = 6;
        //        }
        //        else if (Randomizer >= 81 && Randomizer <= 100)
        //        {
        //            WhichAttack = 7;
        //        }

        //    } while (WhichAttack == PrevAttack);
        //}

        ExecuteAttack();
    }

    void ExecuteAttack()
    {
        if (!IsAttacking)
        {
            switch (WhichAttack)
            {
                case 1: Attack1(); break;
                case 2: Attack2(); break;
                case 3: Attack3(); break;
                case 4: if (Cooldown == false) Attack4(); break;
                case 5: Attack5(); break;
                case 6: Attack6(); break;
                case 7: if (Cooldown == false) Attack7(); break;
            }
        }
    }

    void Attack1()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[1];
        Anim.SetBool("Attack 1", true);
        SpellAnimation.SetBool("Spell 1", true);
    }

    void Attack2()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[2];
        Anim.SetBool("Attack 2", true);
        SpellAnimation.SetBool("Spell 2", true);
    }

    void Attack3()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[3];
        Anim.SetBool("Attack 3", true);
        SpellAnimation.SetBool("Spell 3", true);
    }

    void Attack4()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[4];
        Anim.SetBool("Attack 4", true);
        SpellAnimation.SetBool("Spell 4", true);
    }

    void Attack5()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[5];
        Anim.SetBool("Attack 5", true);
        SpellAnimation.SetBool("Spell 5", true);
    }

    void Attack6()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[6];
        Anim.SetBool("Attack 6", true);
        SpellAnimation.SetBool("Spell 6", true);
    }

    void Attack7()
    {
        IsAttacking = true;
        //AudioManager.PlayOneShot(Attack1Sound);
        MaterialRenderer.sharedMaterial = Glows[7];
        Anim.SetBool("Attack 7", true);
        SpellAnimation.SetBool("Spell 7", true);
    }

    public void SpawnHitbox(int AttackId)
    {
        AttackHitbox[AttackId].SetActive(true);
    }

    public void DespawnHitbox(int AttackId)
    {
        AttackHitbox[AttackId].SetActive(false);
    }
    public void StopAttack(int AttackId)
    {
        IsAttacking = false;
        Anim.SetBool("Attack " + AttackId, false);
        SpellAnimation.SetBool("Spell " + AttackId, false);
    }

    public void StartJump()
    {
        IsJumping = true;
    }

    public void StopJump()
    {
        IsJumping= false;
    }

    void Death()
    {
        IsDead = true;
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}

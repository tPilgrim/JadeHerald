using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprout : MonoBehaviour
{
    private Animator Anim;
    public Animator SpellAnim;

    private int DefaultLayer;
    private Transform Player;
    private Vector3 Scaler;
    private bool IsDead;
    public ParticleSystem DeathParticles;
    public ParticleSystem BurstParticles;

    public Transform SpellPoint;
    public GameObject Bubble;
    public GameObject AttackPoint;
    private bool IsInRangedCombat;
    private bool IsInCombat;
    private bool IsAttacking;
    //private bool CanCancel = true;
    public GameObject AttackHitbox;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Scaler = transform.localScale;
        DefaultLayer = LayerMask.NameToLayer("Dead");
    }

    void Update()
    {
        if(IsInRangedCombat == false)
        {
            Anim.SetBool("Stance", false);
        }

        if (IsDead == false)
        {
            if (IsAttacking == false && IsDead == false && IsInRangedCombat == true)
            {
                Turn();
            }
        }
        else
        {
            AttackHitbox.SetActive(false);
        }
        /*
        if(IsDead == false)
        {
            Anim.SetBool("IsAttacking", IsInCombat);

            if (IsAttacking == false && IsDead == false)
            {
                Turn();
            }
        }
        else
        {
            AttackHitbox.SetActive(false);
            Anim.SetBool("IsAttacking", false);
            StopCoroutine(Attack());
            if(CanCancel == true)
            {
                SpellAnim.SetBool("IsDead", true);
            }
        }
        */
    }

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

    public void RangedCombat(bool EnterRangedCombat)
    {
        IsInRangedCombat = EnterRangedCombat;
        if (IsInRangedCombat == true && IsInCombat == false && IsDead == false)
        {
            Anim.SetBool("Stance", true);
        }
    }

    public void Combat(bool EnterCombat)
    {
        IsInCombat = EnterCombat;
        if (IsInCombat == true && IsDead == false)
        {
            Anim.SetBool("Stance", true);
        }
    }

    void Projectile()
    {
        Instantiate(Bubble, SpellPoint.position, SpellPoint.rotation);
    }

    void AttackManager()
    {
        if (IsInCombat == true && IsAttacking == false)
        {
            StartCoroutine(Attack());
        }

        if (IsInCombat == false && IsInRangedCombat == true && IsAttacking == false)
        {
            StartCoroutine(Cast());
        }
    }

    IEnumerator Cast()
    {
        IsAttacking = true;
        Anim.SetBool("Cast", true);
        yield return new WaitForSeconds(2.08f);
        Anim.SetBool("Cast", false);
        IsAttacking = false;
    }

    IEnumerator Attack()
    {
        IsAttacking = true;
        SpellAnim.SetBool("Burst", true);
        Anim.SetBool("Burst", true);
        yield return new WaitForSeconds(1.6f);
        if (IsDead == false)
        {
            AttackHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(0.475f);
        AttackHitbox.SetActive(false);
        SpellAnim.SetBool("Burst", false);
        yield return new WaitForSeconds(0.3f);
        Anim.SetBool("Burst", false);
        IsAttacking = false;
    }

    void Burst()
    {
        BurstParticles.Play();
    }

    /*
    void StartAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        IsAttacking = true;
        SpellAnim.SetBool("Burst",true);
        yield return new WaitForSeconds(1.23f);
        CanCancel = false;
        if(IsDead == false)
        {
            AttackHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(0.3f);
        AttackHitbox.SetActive(false);
        SpellAnim.SetBool("Burst", false);
        yield return new WaitForSeconds(0.5f);
        IsAttacking = false;
        CanCancel = true;
    }
    */

    void Death()
    {
        IsDead = true;
        AttackPoint.SetActive(false);
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}

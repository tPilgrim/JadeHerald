using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrum : MonoBehaviour
{
    private Animator Anim;
    public Animator SpellAnim;

    private int DefaultLayer;
    private Transform Player;
    private Vector3 Scaler;
    private bool IsDead;
    public ParticleSystem DeathParticles;

    public Transform SpellPoint;
    public GameObject Bubble;
    private bool IsInRangedCombat;
    private bool IsInCombat;
    private bool IsAttacking;
    private bool CanCancel = true;
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
        if(EnterRangedCombat == true && IsInCombat == false && IsDead == false)
        {
            Anim.SetBool("Stance", true);
        }
        IsInRangedCombat = EnterRangedCombat;
    }

    public void Combat(bool EnterCombat)
    {
        IsInCombat = EnterCombat;
    }

    void Projectile()
    {
        Instantiate(Bubble, SpellPoint.position, SpellPoint.rotation);
    }

    void StartAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        IsAttacking = true;
        yield return new WaitForSeconds(0.1f);
        SpellAnim.SetTrigger("Burst");
        yield return new WaitForSeconds(1.23f);
        CanCancel = false;
        if(IsDead == false)
        {
            AttackHitbox.SetActive(true);
        }
        yield return new WaitForSeconds(0.3f);
        AttackHitbox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        IsAttacking = false;
        CanCancel = true;
    }

    void Death()
    {
        IsDead = true;
        DeathParticles.Play();
        gameObject.layer = DefaultLayer;
    }
}

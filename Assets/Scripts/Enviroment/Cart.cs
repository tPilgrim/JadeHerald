using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    private Rigidbody2D CartRb;

    public PhysicsMaterial2D LowFriction;
    public PhysicsMaterial2D HighFriction;
    private AudioSource AudioManager;
    public AudioClip DragSound;
    public AudioClip RollSound;
    private bool IsUpright = true;

    private Vector3 Rotation;

    private Transform PlayerTransform;
    public float Speed;
    private int cartLayer;
    private int enemyLayer;

    void Start()
    {
        CartRb = GetComponent<Rigidbody2D>();
        AudioManager = GetComponent<AudioSource>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        cartLayer = LayerMask.NameToLayer("Cart");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void FixedUpdate()
    {
        if(CartRb.linearVelocity.y < -1f)
        {
            Physics2D.IgnoreLayerCollision(cartLayer, enemyLayer, false);
        }
        else
        {
            StartCoroutine(ActivateCollision());
        }
    }

    IEnumerator ActivateCollision()
    {
        yield return new WaitForSeconds(0.1f);
        Physics2D.IgnoreLayerCollision(cartLayer, enemyLayer, true);
    }

    void Update()
    {
        Debug.Log(CartRb.linearVelocity.y < -1f);
        Rotation = transform.localEulerAngles;

        if(IsUpright )
        {
            AudioManager.clip = RollSound;
        }
        else
        {
            AudioManager.clip = DragSound;
        }

        if(!AudioManager.isPlaying)
            AudioManager.Play();


        if (Mathf.Abs(CartRb.linearVelocity.x) > 0.1f && CartRb.linearVelocity.y >= -0.5f)
        {
            if (IsUpright)
            {
                AudioManager.volume = Mathf.Abs(CartRb.linearVelocity.x) / 10;
            }
            else
            {
                AudioManager.volume = Mathf.Abs(CartRb.linearVelocity.x) / 5;
            }
        }
        else
        {
            AudioManager.volume = 0;
        }
    }

    private void Slide()
    {
        float direction = transform.position.x - PlayerTransform.position.x;

        direction = Mathf.Sign(direction);

        CartRb.AddForce(new Vector2(direction * Speed, 0f), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Attack")
        {
            Slide();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>()?.TakeDamage(999);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Rail" && (Rotation.z > 350 || Rotation.z <10))
        {
            CartRb.sharedMaterial = LowFriction;
            IsUpright = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Rail")
        {
            CartRb.sharedMaterial = HighFriction;
            IsUpright = false;
        }
    }
}

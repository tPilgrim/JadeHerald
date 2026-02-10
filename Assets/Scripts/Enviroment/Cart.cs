using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    private Rigidbody2D EnemyRb;

    public PhysicsMaterial2D LowFriction;
    public PhysicsMaterial2D HighFriction;
    private AudioSource AudioManager;
    public AudioClip DragSound;
    public AudioClip RollSound;
    private bool IsUpright = true;

    private Vector3 Rotation;

    void Start()
    {
        EnemyRb = GetComponent<Rigidbody2D>();
        AudioManager = GetComponent<AudioSource>();
    }

    void Update()
    {
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


        if (Mathf.Abs(EnemyRb.linearVelocity.x) > 0.1f && EnemyRb.linearVelocity.y >= -0.5f)
        {
            if (IsUpright)
            {
                AudioManager.volume = Mathf.Abs(EnemyRb.linearVelocity.x) / 10;
            }
            else
            {
                AudioManager.volume = Mathf.Abs(EnemyRb.linearVelocity.x) / 5;
            }
        }
        else
        {
            AudioManager.volume = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Rail" && (Rotation.z > 350 || Rotation.z <10))
        {
            EnemyRb.sharedMaterial = LowFriction;
            IsUpright = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Rail")
        {
            EnemyRb.sharedMaterial = HighFriction;
            IsUpright = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateTime : MonoBehaviour
{
    public float TimeBeforeDestruction;

    void Start()
    {
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(TimeBeforeDestruction);
        gameObject.SetActive(false);    
    }
}

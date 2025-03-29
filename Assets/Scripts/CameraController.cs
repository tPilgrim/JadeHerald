using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject GroundCamera;
    public GameObject AirCamera;
    public GameObject GroundZone;
    //public GameObject VirtualCamera;
    //public GameObject LookDownCamera;
    //public GameObject LookUpCamera;

    void Start()
    {
        //AirCamera.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D GroundZone)
    {
        if (GroundZone.gameObject.tag == "Player")
        {
            GroundCamera.SetActive(true);
            AirCamera.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D GroundZone)
    {
        if(GroundZone.gameObject.tag == "Player")
        {
            GroundCamera.SetActive(true);
            AirCamera.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D GroundZone)
    {
        if(GroundZone.gameObject.tag == "Player")
        {
            AirCamera.SetActive(true);
            GroundCamera.SetActive(false);
        }
    }

    /*
    void Update()
    {
        LookDown();
        LookUp();
    }

    void LookDown()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow) && AirCamera.activeSelf)
        {
            LookDownCamera.gameObject.SetActive(true);
        }

        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            LookDownCamera.gameObject.SetActive(false);
        }
    }

    void LookUp()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && AirCamera.activeSelf)
        {
            LookUpCamera.SetActive(true);
        }

        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            LookUpCamera.SetActive(false);
        }
    }
    */
}

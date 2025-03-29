using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    void Start()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(PlayerPrefs.GetInt("Scene" + SceneManager.GetActiveScene().buildIndex) == 1)
            {
                PlayerPrefs.SetInt(gameObject.transform.name + SceneManager.GetActiveScene().buildIndex, 1);
            }

            if(PlayerPrefs.GetInt(enemy.transform.name + SceneManager.GetActiveScene().buildIndex) == 0 && enemy.GetComponent<EnemyHealth>() != null)
            {
                enemy.GetComponent<EnemyHealth>().DisableEnemy(enemy.transform.name);
            }
        }
    }
}

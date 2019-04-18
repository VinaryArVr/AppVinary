using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChargueTime : MonoBehaviour
{
    public float Tiempo = 3f;

    void Update()
    {
        Tiempo -= Time.deltaTime;
        if (Tiempo <= 0)
        {
            SceneManager.LoadScene("1_Instrucciones");

        }
    }
}

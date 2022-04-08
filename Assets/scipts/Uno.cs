using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Uno : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cambioEscena()
    {

        SceneManager.LoadScene("MenuPuntuacion");


    }

    public void anterior()
    {

        SceneManager.LoadScene("Niveles");

    }
}

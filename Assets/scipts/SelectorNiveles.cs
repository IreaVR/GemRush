using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SelectorNiveles : MonoBehaviour
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

        SceneManager.LoadScene("Nivel1");


    }

    public void anterior()
    {

        SceneManager.LoadScene("Menu Principal");

    }

    //public void nivel(string nivel)
    //{

    //    SceneManager.LoadScene(nivel);

    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    Cuadricula[,] gemas;
    Cuadricula gema;

    int tamX;
    int tamY;

    // Start is called before the first frame update
    void Start()
    {

        gemas = new Cuadricula[tamX, tamY];
        generar();

    }

    public void generar()
    {

        for (int i = 0; i < tamX; i++)
        {

            for (int j = 0; j < tamY; j++)
            {

                Vector2 aux = new Vector2(i, j);
                Instantiate(gema, aux, Quaternion.identity);

            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

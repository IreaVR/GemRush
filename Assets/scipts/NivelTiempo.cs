using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelTiempo : Nivel
{

    public int segundos;
    public int puntuacionNivel;

    private float timer;
    private bool fin = false;

    // Start is called before the first frame update
    void Start()
    {

        tipo = TipoNivel.TIEMPO;

        Debug.Log("Tiempo: " + segundos + " Puntuacion: " + puntuacion);

    }

    // Update is called once per frame
    void Update()
    {

        if (!fin)
        {
            timer += Time.deltaTime;

            if (segundos - timer <= 0)
            {
                if (puntuacion >= puntuacionNivel)
                {
                    Victoria();
                }
                else
                {
                    Derrota();
                }

                fin = true;

            }
        }
    }
}

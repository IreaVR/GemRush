using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelMovimientos : Nivel
{

    public int numMovimientos;
    public int puntuacionNivel;

    private int movimientosUsados = 0;

    // Start is called before the first frame update
    void Start()
    {

        tipo = TipoNivel.MOVIMIENTOS;
        Debug.Log("movimientos: "+numMovimientos+" puntuacion nivel: "+puntuacionNivel);

    }

    public override void OnMove()
    {
        movimientosUsados++;

        Debug.Log("movimientos restantes: " + (numMovimientos - movimientosUsados));

        if (numMovimientos - movimientosUsados == 0)
        {

            if (puntuacion >= puntuacionNivel)
            {
                Victoria();
            }
            else
            {
                Derrota();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

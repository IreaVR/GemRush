using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelObstaculos : Nivel
{

    public int numMovimientos;
    public Cuadricula.Tipo[] tipoObstaculos;

    private int movimientosUsados=0;
    private int numObstaculos;

    // Start is called before the first frame update
    void Start()
    {

        tipo = TipoNivel.OBSTACULO;

        for (int i = 0; i < tipoObstaculos.Length; i++)
        {

            numObstaculos += cuad.GetTipoGemas(tipoObstaculos[i]).Count;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnMove()
    {

        movimientosUsados++;

        Debug.Log("Movimientos restantes: " + (numMovimientos - movimientosUsados));

        if (numMovimientos-movimientosUsados==0 && numObstaculos>0)
        {
            Derrota();
        }

    }

    public override void OnClearedGem(Gema gema)
    {
        base.OnClearedGem(gema);

        for (int i = 0; i < tipoObstaculos.Length; i++)
        {
            if (tipoObstaculos[i]==gema.Tipo)
            {

                numObstaculos--;

                if (numObstaculos==0)
                {

                    puntuacion += 1000 * (numMovimientos - movimientosUsados);
                    Debug.Log("puntuacion actual: " + puntuacion);
                    Victoria();

                }

            }
        }

    }

}

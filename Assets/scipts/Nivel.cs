using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nivel : MonoBehaviour
{

    public enum TipoNivel
    {
        TIEMPO, 
        OBSTACULO,
        MOVIMIENTOS,
    };

    public Cuadricula cuad;

    public int estrella1;
    public int estrella2;
    public int estrella3;

    protected TipoNivel tipo;

    public TipoNivel Tipo
    {
        get
        {
            return tipo;
        }
    }

    protected int puntuacion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Victoria()
    {

        Debug.Log("victoria");
        cuad.FinPartida();

    }

    public virtual void Derrota()
    {

        Debug.Log("derrota");
        cuad.FinPartida();

    }

    public virtual void OnMove()
    {

        //Debug.Log("movimiento");

    }

    public virtual void OnClearedGem(Gema gema)
    {

        puntuacion += gema.puntuacion;
        Debug.Log("Puntos: " + puntuacion);

    }

}

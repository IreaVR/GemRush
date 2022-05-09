using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimpiarGemaEspecial : GemaCombinada
{

    public bool esFila=false;

    public bool EsFila
    {
        get
        {
            return esFila;
        }
    }

    public override void Clear()
    {
        base.Clear();

        if (esFila)
        {
            gema.Cuad.limpiarFila(gema.Y);
        } else
        {
            gema.Cuad.limpiarColumna(gema.X);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

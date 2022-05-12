using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimpiarColores : GemaCombinada
{

    private TipoGema.Tipo color;

    public TipoGema.Tipo Color
    {
        set
        {
            color = value;
        }
        get
        {
            return color;
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

    public override void Clear()
    {
        base.Clear();

        gema.Cuad.LimpiarColor(color);

    }
}

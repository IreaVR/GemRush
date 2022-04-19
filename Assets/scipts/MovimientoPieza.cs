using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPieza : MonoBehaviour
{

    private Gema gema;


    void Awake()
    {
        gema = GetComponent<Gema>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Movimiento(int x, int y)
    {

        gema.X = x;
        gema.Y = y;

        gema.transform.localPosition = gema.Cuad.PosicionCamara(x, y);

    }
}

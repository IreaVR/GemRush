using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TipoGema;

public class Gema : MonoBehaviour
{

    private int x, y;

    public int X
    {
        set
        {
            if (seMueve())
            {
                x = value;
            }

        }

        get
        {
            return x;
        }
    }

    public int Y
    {
        set
        {
            if (seMueve())
            {
                y = value;
            }
        }

        get
        {
            return y;
        }
    }

    private Cuadricula.Tipo tipo;

    public Cuadricula.Tipo Tipo
    {
        get
        {
            return tipo;
        }
    }

    private Cuadricula cuad;

    public Cuadricula Cuad
    {
        get
        {
            return cuad;
        }
    }

    private MovimientoPieza movimiento;

    public MovimientoPieza Movimiento
    {

        get
        {
            return movimiento;
        }

    }

    private TipoGema colorComponente;

    //colorPiece
    public TipoGema ColorComponente
    {

        get
        {
            return colorComponente;
        }

    }

    void Awake()
    {

        movimiento = GetComponent<MovimientoPieza>();
        colorComponente = GetComponent<TipoGema>();

    }

    public void Constructor(int x, int y, Cuadricula cuad, Cuadricula.Tipo tipo)
    {

        this.x = x;
        this.y = y;
        this.cuad = cuad;
        this.tipo = tipo;

    }

    public bool seMueve()
    {

        return movimiento != null;

    }

    public bool sePinta()
    {
        return colorComponente != null;
    }

    void OnMouseEnter()
    {
        cuad.GemaIntroducida(this);
    }

    void OnMouseDown()
    {
        cuad.GemaPulsada(this);
    }

    void OnMouseUp()
    {
        cuad.GemaLiberada();
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

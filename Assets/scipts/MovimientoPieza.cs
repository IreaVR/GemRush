using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPieza : MonoBehaviour
{

    private Gema gema;
    private IEnumerator movimientoCoroutine;

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

    public void Mover(int x, int y, float tiempo)
    {
        if (movimientoCoroutine!=null)
        {
            StopCoroutine(movimientoCoroutine);
        }

        movimientoCoroutine = MovimientoCoroutine(x, y, tiempo);
        StartCoroutine(movimientoCoroutine);

    }

    public IEnumerator MovimientoCoroutine(int x, int y, float tiempo)
    {
        gema.X = x;
        gema.Y = y;

        Vector3 posInicial = transform.position;
        Vector3 posFinal = gema.Cuad.PosicionCamara(x, y);

        for (float i = 0; i <= 1* tiempo; i+=Time.deltaTime)
        {
            gema.transform.position = Vector3.Lerp(posInicial, posFinal, i / tiempo);
            yield return 0;
        }

        gema.transform.position = posFinal;
    }
}

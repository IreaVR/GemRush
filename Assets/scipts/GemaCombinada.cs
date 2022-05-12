using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemaCombinada : MonoBehaviour
{

    public AnimationClip animacionCombinacion;

    private bool seEstaLimpiando = false;

    public bool SeEstaLimpiando
    {
        get
        {
            return seEstaLimpiando;
        }
    }

    protected Gema gema;

    private void Awake()
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

    public virtual void Clear()
    {
        gema.Cuad.nivel.OnClearedGem(gema);
        seEstaLimpiando = true;
        StartCoroutine(ClearCoroutine());
    }

    private IEnumerator ClearCoroutine()
    {
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            //TODO animator.Play(animacionCombinacion.name);

            yield return new WaitForSeconds(animacionCombinacion.length);

            Destroy(gameObject);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class TipoGema : MonoBehaviour
{

    public enum Tipo
    {

        AZUL,
        VERDE,
        ROJA, 
        NARANJA, 
        ANY,
        COUNT

    };

    [System.Serializable]
    public struct TipoSprite
    {

        public Tipo tipo;
        public Sprite sprite;

    }

    public TipoSprite[] tipoSprites;

    private Tipo colorGema;

    public Tipo ColorGema
    {
        set
        {
            //SetTipoGema(value);
        }
        get
        {
            return colorGema;
        }

    }

    private Dictionary<Tipo, Sprite> tipoSpriteDiccionario;

    private void Awake()
    {

        tipoSpriteDiccionario = new Dictionary<Tipo, Sprite>();

        for (int i = 0; i < tipoSprites.Length; i++)
        {

            if (!tipoSpriteDiccionario.ContainsKey(tipoSprites[i].tipo))
            {

                tipoSpriteDiccionario.Add(tipoSprites[i].tipo, tipoSprites[i].sprite);

            }

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

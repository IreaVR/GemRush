using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuadricula : MonoBehaviour
{

    public enum Tipo
    {
        NORMAL,
        COUNT,
    };

    [System.Serializable]
    public struct GemaPrefab
    {

        public Tipo tipo;
        public GameObject prefab;

    };

    public int tamX, tamY;

    public GemaPrefab[] gemaPrefabs;
    public GameObject fondoPrefab;

    private Dictionary<Tipo, GameObject> gemaPrefabDiccionario;

    private Gema[,] gemas;

    //public Manager manager;

    // Start is called before the first frame update
    void Start()
    {

        gemaPrefabDiccionario = new Dictionary<Tipo, GameObject>();

        for (int i = 0; i < gemaPrefabs.Length; i++)
        {
            if (!gemaPrefabDiccionario.ContainsKey(gemaPrefabs[i].tipo))
            {

                gemaPrefabDiccionario.Add(gemaPrefabs[i].tipo, gemaPrefabs[i].prefab);

            }
        }

        for (int i = 0; i < tamX; i++)
        {

            for (int j = 0; j < tamY; j++)
            {

                GameObject fondo = (GameObject)Instantiate(fondoPrefab, PosicionCamara(i, j), Quaternion.identity);
                fondo.transform.parent = transform;

            }

        }

        gemas = new Gema[tamX, tamY];

        for (int i = 0; i < tamX; i++)
        {
            for (int j = 0; j < tamY; j++)
            {
                GameObject nuevaGema = (GameObject)Instantiate(gemaPrefabDiccionario[Tipo.NORMAL], Vector3.zero, Quaternion.identity);
                nuevaGema.name = "Gema(" + i + "," + j + ")";
                nuevaGema.transform.parent = transform;

                gemas[i, j] = nuevaGema.GetComponent<Gema>();

                gemas[i, j].Constructor(i, j, this, Tipo.NORMAL);

                if (gemas[i, j].seMueve())
                {

                    gemas[i, j].Movimiento.Movimiento(i, j);

                }

            }
        }

    }


    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 PosicionCamara(int x, int y)
    {

        return new Vector2(transform.position.x - tamX / 2.0f + x, transform.position.y - tamY / 2.0f + y);

    }

    //public void Constructor(Manager manager, int x, int y)
    //{

    //    this.manager = manager;
    //    this.tamX = x;
    //    this.tamY = y; 

    //}

    //public void posicion(int x, int y)
    //{

    //    this.tamX = x;
    //    this.tamY = y;

    //}
}

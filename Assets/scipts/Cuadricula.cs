using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cuadricula : MonoBehaviour
{

    public enum Tipo
    {
        EMPTY,
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
    public float tiempoRellenar;

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
                //GameObject nuevaGema = (GameObject)Instantiate(gemaPrefabDiccionario[Tipo.NORMAL], Vector3.zero, Quaternion.identity);
                //nuevaGema.name = "Gema(" + i + "," + j + ")";
                //nuevaGema.transform.parent = transform;

                //gemas[i, j] = nuevaGema.GetComponent<Gema>();

                //gemas[i, j].Constructor(i, j, this, Tipo.NORMAL);

                //if (gemas[i, j].seMueve())
                //{

                //    gemas[i, j].Movimiento.Movimiento(i, j);

                //}

                //if (gemas[i, j].sePinta())
                //{                                                       
                //    gemas[i, j].ColorComponente.SetColor((TipoGema.Tipo)Random.Range(0, gemas[i, j].ColorComponente.NumColores));
                //}
                SpawnNuevaGema(i, j, Tipo.EMPTY);

            }
        }

        StartCoroutine(Fill());

    }

    public Vector2 PosicionCamara(int x, int y)
    {

        return new Vector2(transform.position.x - tamX / 2.0f + x, transform.position.y - tamY / 2.0f + y);

    }

    public Gema SpawnNuevaGema(int x, int y, Tipo tipo)
    {
        GameObject nuevaGema = (GameObject)Instantiate(gemaPrefabDiccionario[tipo], PosicionCamara(x, y), Quaternion.identity);
        nuevaGema.transform.parent = transform;
        gemas[x, y] = nuevaGema.GetComponent<Gema>();
        gemas[x, y].Constructor(x, y, this, tipo);

        return gemas[x, y];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Fill()
    {

        while (FillStep())
        {
            yield return new WaitForSeconds(tiempoRellenar);
        }

    }

    public bool FillStep()
    {

        bool gemaMovida = false;

        for (int y = tamY - 2; y >= 0; y--)
        {
            for (int x = 0; x < tamX; x++)
            {

                Gema gema = gemas[x, y];

                if (gema.seMueve())
                {

                    Gema gemaInferior = gemas[x, y + 1];

                    if (gemaInferior.Tipo == Tipo.EMPTY)
                    {

                        gema.Movimiento.Mover(x, y + 1, tiempoRellenar);
                        gemas[x, y + 1] = gema;
                        SpawnNuevaGema(x, y, Tipo.EMPTY);
                        gemaMovida = true;

                    }

                }

            }

        }

        for (int i = 0; i < tamX; i++)
        {

            Gema piezaInferior = gemas[i, 0];

            if (piezaInferior.Tipo==Tipo.EMPTY)
            {

                GameObject nuevaGema = (GameObject)Instantiate(gemaPrefabDiccionario[Tipo.NORMAL], PosicionCamara(i, -1), Quaternion.identity);
                nuevaGema.transform.parent = transform;

                gemas[i, 0] = nuevaGema.GetComponent<Gema>();
                gemas[i, 0].Constructor(i, -1, this, Tipo.NORMAL);
                gemas[i, 0].Movimiento.Mover(i, 0, tiempoRellenar);
                gemas[i, 0].ColorComponente.SetColor((TipoGema.Tipo)Random.Range(0, gemas[i, 0].ColorComponente.NumColores));
                gemaMovida = true;

            }

        }

        return gemaMovida;

    }
}

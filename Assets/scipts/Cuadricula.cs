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
        MURO,  //esto es para bloquear el paso a las gemas
        COUNT
    }

    [System.Serializable]
    public struct GemaPrefab
    {

        public Tipo tipo;
        public GameObject prefab;

    }

    public int tamX, tamY;
    public float tiempoRellenar;

    public GemaPrefab[] gemaPrefabs;
    public GameObject fondoPrefab;

    private Dictionary<Tipo, GameObject> gemaPrefabDiccionario;

    private Gema[,] gemas;

    private bool inverso = false;

    private Gema gemaPulsada;
    private Gema gemaIntroducida;

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

        //estas lienas son para poner la localizacion del muro y para decir que no aparezcan gemas debajo
        //error null object
        //Destroy(gemas[1, 4].gameObject);
        //SpawnNuevaGema(1, 4, Tipo.MURO);    

        //Destroy(gemas[2, 4].gameObject);
        //SpawnNuevaGema(2, 4, Tipo.MURO);

        //Destroy(gemas[3, 4].gameObject);
        //SpawnNuevaGema(3, 4, Tipo.MURO);

        //Destroy(gemas[5, 4].gameObject);
        //SpawnNuevaGema(5, 4, Tipo.MURO);

        //Destroy(gemas[6, 4].gameObject);
        //SpawnNuevaGema(6, 4, Tipo.MURO);

        //Destroy(gemas[7, 4].gameObject);
        //SpawnNuevaGema(7, 4, Tipo.MURO);

        //Destroy(gemas[4, 0].gameObject);
        //SpawnNuevaGema(4, 0, Tipo.MURO);

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
        Debug.Log("x: "+x+" y: "+y+" cuadricula: "+this+" tipo: "+tipo);

        gemas[x, y].Constructor(x, y, this, tipo);      //error NullReferennceException aqui

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
            inverso = !inverso;
            yield return new WaitForSeconds(tiempoRellenar);
        }

    }

    public bool FillStep()
    {

        bool gemaMovida = false;

        for (int y = tamY - 2; y >= 0; y--)
        {
            for (int loopX = 0; loopX < tamX; loopX++)
            {
                int x = loopX;

                if (inverso)
                {
                    x = tamX - 1 - loopX;
                }

                Gema gema = gemas[x, y];

                if (gema.seMueve())
                {

                    Gema gemaInferior = gemas[x, y + 1];

                    if (gemaInferior.Tipo == Tipo.EMPTY)
                    {
                        Destroy(gemaInferior.gameObject);
                        gema.Movimiento.Mover(x, y + 1, tiempoRellenar);
                        gemas[x, y + 1] = gema;
                        SpawnNuevaGema(x, y, Tipo.EMPTY);
                        gemaMovida = true;

                    }

                //esto es para rellenar debajo del obsaculo
                } else
                {

                    for (int diag = -1; diag <= 1; diag++)
                    {
                        if (diag !=0)
                        {

                            int diagX = x + diag;

                            if (inverso)
                            {

                                diagX = x - diag;

                            }

                            if (diagX>= 0 && diagX < tamX)
                            {

                                Gema gemaDiagonal = gemas[diagX, y + 1];

                                if (gemaDiagonal.Tipo==Tipo.EMPTY)
                                {

                                    bool tieneGemaEncima = true;

                                    for (int arribaY = y; arribaY >= 0; arribaY--)
                                    {

                                        Gema gemaArriba = gemas[diagX, arribaY];

                                        if (gemaArriba.seMueve())
                                        {
                                            break;
                                        } 
                                        else if(!gemaArriba.seMueve() && gemaArriba.Tipo != Tipo.EMPTY)
                                        {

                                            tieneGemaEncima = false;
                                            break;

                                        }

                                    }

                                    if (!tieneGemaEncima)
                                    {

                                        Destroy(gemaDiagonal.gameObject);
                                        gema.Movimiento.Mover(diagX, y + 1, tiempoRellenar);
                                        gemas[diagX, y + 1] = gema;
                                        SpawnNuevaGema(x, y, Tipo.EMPTY);
                                        gemaMovida = true;
                                        break;

                                    }

                                }

                            }

                        }
                    }

                }



            }

        }

        for (int i = 0; i < tamX; i++)
        {

            Gema gemaInferior = gemas[i, 0];

            if (gemaInferior.Tipo==Tipo.EMPTY)
            {
                Destroy(gemaInferior.gameObject);
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

    public bool EsAdyacente(Gema gema1, Gema gema2)
    {

        return (gema1.X == gema2.X && (int)Mathf.Abs(gema1.Y - gema2.Y) == 1) || (gema1.Y == gema2.Y && (int)Mathf.Abs(gema1.X - gema2.X) == 1);

    }

    public void IntercambioGemas(Gema gema1, Gema gema2)
    {
        if (gema1.seMueve() && gema2.seMueve())
        {
            
            gemas[gema1.X, gema1.Y] = gema2;
            gemas[gema2.X, gema2.Y] = gema1;

            if (GetCombinacion(gema1, gema2.X, gema2.Y) != null || GetCombinacion(gema2, gema1.X, gema1.Y) != null)
            {

                int gema1X = gema1.X;
                int gema1Y = gema1.Y;

                gema1.Movimiento.Mover(gema2.X, gema2.Y, tiempoRellenar);
                gema2.Movimiento.Mover(gema1X, gema1Y, tiempoRellenar);
           
            } 
            else
            {

                gemas[gema1.X, gema1.Y] = gema1;
                gemas[gema2.X, gema2.Y] = gema2;

            }

        }
    }

    public void GemaPulsada(Gema gema)
    {

        gemaPulsada = gema;

    }

    public void GemaIntroducida(Gema gema)
    {

        gemaIntroducida = gema;

    }

    public void GemaLiberada()
    {

        if (EsAdyacente(gemaPulsada, gemaIntroducida))
        {

            IntercambioGemas(gemaPulsada, gemaIntroducida);

        }

    }

    public List<Gema> GetCombinacion(Gema gema, int nuevaX, int nuevaY)
    {

        if (gema.sePinta())
        {

            TipoGema.Tipo color = gema.ColorComponente.ColorGema;
            List<Gema> gemasHorizontales = new List<Gema>();
            List<Gema> gemasVerticales = new List<Gema>();
            List<Gema> gemasCombinadas = new List<Gema>();

            //comprobacion horizontales
            gemasHorizontales.Add(gema);

            for (int i = 0; i <= 1; i++)
            {
                for (int xOffset = 1; xOffset < tamX; xOffset++)
                {

                    int x;

                    if (i == 0) //izquierda
                    {
                        x = nuevaX - xOffset;
                    }
                    else //derecha
                    {
                        x = nuevaX + xOffset;
                    }

                    if (x<0 || x >= tamX)
                    {
                        break;
                    }

                    if (gemas[x, nuevaY].sePinta() && gemas[x, nuevaY].ColorComponente.ColorGema == color)
                    {
                        gemasHorizontales.Add(gemas[x, nuevaY]);
                    } 
                    else
                    {
                        break;
                    }
                }
            }

            if (gemasHorizontales.Count >= 3)
            {
                for (int i = 0; i < gemasHorizontales.Count; i++)
                {
                    gemasCombinadas.Add(gemasHorizontales[i]);
                }
            }

            //combinacion vertical en forma de L y T
            if (gemasHorizontales.Count >= 3)
            {

                for (int i = 0; i < gemasHorizontales.Count; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        for (int yOffset = 1; yOffset < tamY; yOffset++)
                        {
                            int y;

                            if (j==0) //arriba
                            {
                                y = nuevaY - yOffset;
                            }
                            else //abajo
                            {
                                y = nuevaY + yOffset;
                            }

                            if (yOffset < 0 || yOffset >= tamY)
                            {
                                break;
                            }
                            
                            if (gemas[gemasHorizontales[i].X, y].sePinta() && gemas[gemasHorizontales[i].X, y].ColorComponente.ColorGema == color)
                            {
                                gemasVerticales.Add(gemas[gemasHorizontales[i].X, y]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (gemasVerticales.Count < 2)
                    {
                        gemasVerticales.Clear();
                    }
                    else
                    {
                        for (int j = 0; j < gemasVerticales.Count; j++)
                        {
                            gemasCombinadas.Add(gemasVerticales[j]);
                        }
                        break;
                    }

                }

            }

            if (gemasCombinadas.Count >= 3)
            {

                return gemasCombinadas;

            }

            //comprobacion verticales
            gemasHorizontales.Clear();
            gemasVerticales.Clear();
            gemasVerticales.Add(gema);

            for (int i = 0; i <= 1; i++)
            {
                for (int yOffset = 1; yOffset < tamX; yOffset++)
                {

                    int y;

                    if (i == 0) //arriba
                    {
                        y = nuevaX - yOffset;
                    }
                    else //abajo
                    {
                        y = nuevaX + yOffset;
                    }

                    if (y < 0 || y >= tamY)
                    {
                        break;
                    }

                    if (gemas[nuevaX, y].sePinta() && gemas[nuevaX, y].ColorComponente.ColorGema == color)
                    {
                        gemasVerticales.Add(gemas[nuevaX, y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (gemasVerticales.Count >= 3)
            {
                for (int i = 0; i < gemasVerticales.Count; i++)
                {
                    gemasCombinadas.Add(gemasVerticales[i]);
                }
            }

            //combinacion horizontal en forma de L y T
            if (gemasVerticales.Count >= 3)
            {

                for (int i = 0; i < gemasVerticales.Count; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        for (int xOffset = 1; xOffset < tamX; xOffset++)
                        {
                            int x;

                            if (j == 0) //izquierda
                            {
                                x = nuevaX - xOffset;
                            }
                            else //derecha
                            {
                                x = nuevaX + xOffset;
                            }

                            if (x < 0 || x >= tamX)
                            {
                                break;
                            }

                            if (gemas[x, gemasVerticales[i].Y].sePinta() && gemas[x, gemasVerticales[i].Y].ColorComponente.ColorGema == color)
                            {
                                gemasVerticales.Add(gemas[x, gemasVerticales[i].Y]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (gemasHorizontales.Count < 2)
                    {
                        gemasHorizontales.Clear();
                    }
                    else
                    {
                        for (int j = 0; j < gemasHorizontales.Count; j++)
                        {
                            gemasCombinadas.Add(gemasHorizontales[j]);
                        }
                        break;
                    }

                }

            }

            if (gemasCombinadas.Count >= 3)
            {

                return gemasCombinadas;

            }

        }

        return null;

    }











}

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
        FILA,
        COLUMNA,
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

                SpawnNuevaGema(i, j, Tipo.EMPTY);

            }
        }

        //estas lienas son para poner la localizacion del muro y para decir que no aparezcan gemas debajo
        //error null object
        Destroy(gemas[1, 4].gameObject);
        SpawnNuevaGema(1, 4, Tipo.MURO);

        Destroy(gemas[2, 4].gameObject);
        SpawnNuevaGema(2, 4, Tipo.MURO);

        Destroy(gemas[3, 4].gameObject);
        SpawnNuevaGema(3, 4, Tipo.MURO);

        Destroy(gemas[5, 4].gameObject);
        SpawnNuevaGema(5, 4, Tipo.MURO);

        Destroy(gemas[6, 4].gameObject);
        SpawnNuevaGema(6, 4, Tipo.MURO);

        Destroy(gemas[7, 4].gameObject);
        SpawnNuevaGema(7, 4, Tipo.MURO);

        Destroy(gemas[4, 0].gameObject);
        SpawnNuevaGema(4, 0, Tipo.MURO);

        StartCoroutine(Fill());

    }

    public Vector2 PosicionCamara(int x, int y)
    {

        return new Vector2(transform.position.x - tamX / 2.0f + x, transform.position.y + tamY / 2.0f - y);

    }

    public Gema SpawnNuevaGema(int x, int y, Tipo tipo)
    {
        GameObject nuevaGema = (GameObject)Instantiate(gemaPrefabDiccionario[tipo], PosicionCamara(x, y), Quaternion.identity);
        nuevaGema.transform.parent = transform;
        gemas[x, y] = nuevaGema.GetComponent<Gema>();
        Debug.Log("x: " + x + " y: " + y + " cuadricula: " + this + " tipo: " + tipo);

        gemas[x, y].Constructor(x, y, this, tipo);  

        return gemas[x, y];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Fill()
    {

        bool rellenar = true;

        while (rellenar)
        {
            yield return new WaitForSeconds(tiempoRellenar);

            while (FillStep())
            {
                inverso = !inverso;

                yield return new WaitForSeconds(tiempoRellenar);
            }

            rellenar = LimpiarTodasCombinaciones();

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
                    //TODO revisar este fragmento de codigo para solucionar los fallos
                }
                else
                {

                    for (int diag = -1; diag <= 1; diag++)
                    {
                        if (diag != 0)
                        {

                            int diagX = x + diag;

                            if (inverso)
                            {

                                diagX = x - diag;

                            }

                            if (diagX >= 0 && diagX < tamX)
                            {

                                Gema gemaDiagonal = gemas[diagX, y /*+ 1*/];

                                if (gemaDiagonal.Tipo == Tipo.EMPTY)
                                {

                                    bool tieneGemaEncima = true;

                                    for (int arribaY = y; arribaY >= 0; arribaY--)
                                    {

                                        Gema gemaArriba = gemas[diagX, arribaY];

                                        if (gemaArriba.seMueve())
                                        {
                                            break;
                                        }
                                        else if (!gemaArriba.seMueve() && gemaArriba.Tipo != Tipo.EMPTY)
                                        {

                                            tieneGemaEncima = false;
                                            break;

                                        }

                                    }

                                    if (!tieneGemaEncima)
                                    {

                                        Destroy(gemaDiagonal.gameObject);
                                        //gema.Movimiento.Mover(diagX, y + 1, tiempoRellenar); TODO
                                        gemas[diagX, y /*+ 1*/] = gema;
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

            if (gemaInferior.Tipo == Tipo.EMPTY)
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

                LimpiarTodasCombinaciones();

                if (gema1.Tipo == Tipo.FILA || gema1.Tipo == Tipo.COLUMNA)
                {
                    LimpiarGema(gema1.X, gema1.Y);
                }

                if (gema2.Tipo == Tipo.FILA || gema2.Tipo == Tipo.COLUMNA)
                {
                    LimpiarGema(gema2.X, gema2.Y);
                }

                gemaPulsada = null;
                gemaIntroducida = null;

                StartCoroutine(Fill());
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

                    if (x < 0 || x >= tamX)
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
            //if (gemasHorizontales.Count >= 3)
            //{

            //    for (int i = 0; i < gemasHorizontales.Count; i++)
            //    {
            //        for (int j = 0; j <= 1; j++)
            //        {
            //            for (int yOffset = 1; yOffset < tamY; yOffset++)
            //            {
            //                int y;

            //                if (j == 0) //arriba
            //                {
            //                    y = nuevaY - yOffset;
            //                }
            //                else //abajo
            //                {
            //                    y = nuevaY + yOffset;
            //                }

            //                if (yOffset < 0 || yOffset >= tamY)
            //                {
            //                    break;
            //                }

            //                if (/*gemas[gemasHorizontales[i].X, y].sePinta() &&*/ gemas[gemasHorizontales[i].X, y].ColorComponente.ColorGema == color)
            //                {
            //                    gemasVerticales.Add(gemas[gemasHorizontales[i].X, y]);
            //                }
            //                else
            //                {
            //                    break;
            //                }
            //            }
            //        }

            //        if (gemasVerticales.Count < 2)
            //        {
            //            gemasVerticales.Clear();
            //        }
            //        else
            //        {
            //            for (int j = 0; j < gemasVerticales.Count; j++)
            //            {
            //                gemasCombinadas.Add(gemasVerticales[j]);
            //            }
            //            break;
            //        }

            //    }

            //}

            //

            if (gemasCombinadas.Count >= 3)
            {
                //for (int i = 0; i < gemasHorizontales.Count; i++)
                //{
                //    gemasCombinadas.Remove(gemasHorizontales[i]);
                //}
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
            //if (gemasVerticales.Count >= 3)
            //{

            //    for (int i = 0; i < gemasVerticales.Count; i++)
            //    {
            //        for (int j = 0; j <= 1; j++)
            //        {
            //            for (int xOffset = 1; xOffset < tamX; xOffset++)
            //            {
            //                int x;

            //                if (j == 0) //izquierda
            //                {
            //                    x = nuevaX - xOffset;
            //                }
            //                else //derecha
            //                {
            //                    x = nuevaX + xOffset;
            //                }

            //                if (x < 0 || x >= tamX)
            //                {
            //                    break;
            //                }

            //                if (/*gemas[x, gemasVerticales[i].Y].sePinta() &&*/ gemas[x, gemasVerticales[i].Y].ColorComponente.ColorGema == color)
            //                {
            //                    gemasVerticales.Add(gemas[x, gemasVerticales[i].Y]);
            //                }
            //                else
            //                {
            //                    break;
            //                }
            //            }
            //        }

            //        if (gemasHorizontales.Count < 2)
            //        {
            //            gemasHorizontales.Clear();
            //        }
            //        else
            //        {
            //            for (int j = 0; j < gemasHorizontales.Count; j++)
            //            {
            //                gemasCombinadas.Add(gemasHorizontales[j]);
            //            }
            //            break;
            //        }

            //    }

            //}

            //

            if (gemasCombinadas.Count >= 3)
            {
                //for (int i = 0; i < gemasHorizontales.Count; i++)
                //{
                //    gemasCombinadas.Remove(gemasHorizontales[i]);
                //}
                return gemasCombinadas;

            }

        }

        return null;

    }

    public bool LimpiarTodasCombinaciones()
    {
        bool rellenar = false;

        for (int y = 0; y < tamY; y++)
        {
            for (int x = 0; x < tamX; x++)
            {
                if (gemas[x, y].SeCombina())
                {
                    List<Gema> combinacion = GetCombinacion(gemas[x, y], x, y);

                    if (combinacion != null)
                    {

                        Tipo gemaEspecial = Tipo.COUNT;
                        Gema gemaAleatoria = combinacion[Random.Range(0, combinacion.Count)];
                        int gemaEspecialX = gemaAleatoria.X;
                        int gemaEspecialY = gemaAleatoria.Y;

                        if (combinacion.Count==4)
                        {
                            if (gemaPulsada == null || gemaIntroducida==null)
                            {
                                gemaEspecial = (Tipo)Random.Range((int)Tipo.FILA, (int)Tipo.COLUMNA); //1:15:31
                            }
                            else if (gemaPulsada.Y == gemaIntroducida.Y)
                            {
                                gemaEspecial = Tipo.FILA;
                            } else
                            {
                                gemaEspecial = Tipo.COLUMNA;
                            }
                        }

                        for (int i = 0; i < combinacion.Count; i++)
                        {
                            if (LimpiarGema(combinacion[i].X, combinacion[i].Y))
                            {
                                rellenar = true;

                                if (combinacion[i] == gemaPulsada || combinacion[i] == gemaIntroducida)
                                {
                                    gemaEspecialX = combinacion[i].X;
                                    gemaEspecialY = combinacion[i].Y;
                                }

                            }
                        }

                        if (gemaEspecial != Tipo.COUNT)
                        {
                            Destroy(gemas[gemaEspecialX, gemaEspecialY]);
                            Gema nuevaGema = SpawnNuevaGema(gemaEspecialX, gemaEspecialY, gemaEspecial);

                            if ((gemaEspecial == Tipo.FILA || gemaEspecial == Tipo.COLUMNA) && nuevaGema.sePinta() && combinacion[0].sePinta())
                            {
                                nuevaGema.ColorComponente.SetColor(combinacion[0].ColorComponente.ColorGema);
                            }
                        }
                    }
                }
            }
        }

        return rellenar;

    }

    public bool LimpiarGema(int x, int y)
    {

        if (gemas[x, y].SeCombina() && !gemas[x, y].ComponenteCombinado.SeEstaLimpiando)
        {
            gemas[x, y].ComponenteCombinado.Clear();
            SpawnNuevaGema(x, y, Tipo.EMPTY);

            LimpiarObstaculos(x, y);

            return true;
        }

        return false;

    }

    public void LimpiarObstaculos(int x, int y)
    {

        for (int xAdyacente = x - 1; xAdyacente <= x + 1; xAdyacente++)
        {
            if (xAdyacente != x && xAdyacente >= 0 && xAdyacente < tamX)
            {
                if (gemas[xAdyacente, y].Tipo == Tipo.MURO && gemas[xAdyacente, y].SeCombina())
                {
                    if (gemas[xAdyacente, y].Tipo == Tipo.MURO)
                    {
                        Destroy(gemas[xAdyacente, y].gameObject);

                    }
                    gemas[xAdyacente, y].ComponenteCombinado.Clear();
                    SpawnNuevaGema(xAdyacente, y, Tipo.EMPTY);

                }
            }
        }

        for (int yAdyacente = y - 1; yAdyacente <= y + 1; yAdyacente++)
        {
            if (yAdyacente != y && yAdyacente >= 0 && yAdyacente < tamY)
            {
                if (gemas[x, yAdyacente].Tipo == Tipo.MURO && gemas[x, yAdyacente].SeCombina())
                {
                    if (gemas[x, yAdyacente].Tipo == Tipo.MURO)
                    {
                        Destroy(gemas[x, yAdyacente].gameObject);

                    }
                    gemas[x, yAdyacente].ComponenteCombinado.Clear();
                    SpawnNuevaGema(x, yAdyacente, Tipo.EMPTY);

                }
            }
        }

    }

    public void limpiarFila(int fila)
    {

        for (int x = 0; x < tamX; x++)
        {
            LimpiarGema(x, fila);
        }

    }

    public void limpiarColumna(int columna)
    {

        for (int y = 0; y < tamY; y++)
        {
            LimpiarGema(y, columna);
        }

    }

}

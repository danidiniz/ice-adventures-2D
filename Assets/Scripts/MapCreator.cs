using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {

    // TEMPORARIO
    public short numberOfCrates;
    private int count = 0;

    public float zoomDaCamera;


    // Para o player salvar o mapa que criou, ele deve conseguir jogar o próprio mapa e vencer. (dessa forma permito apenas mapas com solução)

    // Elementos obrigatórios no mapa:
    // 1) Ice
    // 2) Start
    // 3) End

    // Elementos que podem estar no mapa:
    // 1) Crates (que podem ser PULADAS, QUEBRADAS, EMPURRADAS)
    // 2) Buraco que leva a outro buraco
    // 3) Ices que quebram ao passar por cima. Niveis 1,2,3 (ao chegar no nivel 3 ele quebra)
    // 4) Ice quebrado (no caso se passar em cima perde)
    // 5) Crate com 1, 2, 3 ou 4 pinguins.
    //    Ao libertá-los escolhe a(s) direção(ões) que eles irão correr (pode passar pelo player).
    //    Os pinguins quebram todas as caixas no caminho (obs.: se o pinguim passar por um ice quebrado ele 'morre' igual o player e some)
    //    Os pinguins também interagem com os elementos do mapa. Por exemplo no buraco ela irá sumir, dessa forma evito loops
    //    -Pensar melhor em mais interações-
    // 6) Especial super poder
    //    Exemplo: Peixes (1,2 ou 3) (dá super poder ao pinguim e ele quebra o numero de caixas igual o numero de peixes que tinha no ice)
    //    Voar? Assim ele pode voar por cima de Ices Quebrados
    // 7) Casinhas iglu com 2 saídas que transferem de uma saída pra outra o que passar por elas (não pára na casinha, entra de um lado e sai do outro e continua o movimento)
    // 8) Animais que sejam boss?
    //    Exemplo: Orcas assassinas que soltam ondas em uma direção de tempo em tempo 
    //    (o player 'surfa' em cima da onda e pode movimentar no eixo oposto da onda) 
    //    Se ele não movimentar ele irá interagir com algo. 
    //    Exemplo: se bater em uma caixa ele morre. Se cair fora do mapa ele morre. Se cair em um buraco ele teleporta (no caso se o buraco estiver cheio ele continua o movimento)
    //    Outro Exemplo de boss: Urso polar que aparece em uma direção e destrói tudo pela frente
    // 9) Armadilhas que 'matam' o player (no caso os animais boss podem destruir essas armadilhas)

    // Pensando:
    // As ondas das orcas são tiros, tiros afetam 'players'(players podem ser o player, pinguim, urso polar)
    // tiros afetam proprios tiros
    // tiros afetam objetos (crates)


    public bool modoCriarMapaAtivado;


    public enum elementosPossiveisNoMapa {
        // Tipos de ice
        ICE, ICE_QUEBRADO_1, ICE_QUEBRADO_2, ICE_QUEBRADO_3, ICE_QUEBRADO_FINAL, ICE_QUEBRADO_COM_CRATE_EM_CIMA, BURACO, IGLU, START, END,
        // Tipos de crate
        CRATE, CRATE_COM_PINGUIM_1, CRATE_COM_PINGUIM_2, CRATE_COM_PINGUIM_3, CRATE_COM_PINGUIM_4,
        // Tipos de especial
        ESPECIAL_PEIXE, ESPECIAL_ASAS,
        // Players
        PLAYER, PINGUIM, ONDA_DA_ORCA, URSO_POLAR
    };
    public elementosPossiveisNoMapa elementoSelecionado;

    
    public static IcesDefault[,] map;
    [SerializeField]
    private IcesDefault[,] mapOriginal; // mapa sem as alterações para quando o player perder ou dar restart
    public List<IcesDefault> mapOriginalList;
    public List<IcesDefault> mapList;

    public Transform mapIcesParent;

    public GameObject ice;
    private float iceExtentsX;
    private float iceExtentsY;
    public GameObject start;
    public GameObject end;
    public GameObject crate;
    public GameObject buraco;

    // TEMPORARIO
    public GameObject ice_quebrado_1;
    public GameObject ice_quebrado_2;
    public GameObject ice_quebrado_3;
    public GameObject ice_quebrado_final;

    [SerializeField]
    private short linhas;
    [SerializeField]
    private short colunas;

    static MapCreator _instance;

    public static MapCreator instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapCreator>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // TEMPORARIO
        numberOfCrates = (short)(linhas * colunas / 3);
        mapOriginalList = new List<IcesDefault>();
        mapList = new List<IcesDefault>();

        modoCriarMapaAtivado = true;

        elementoSelecionado = elementosPossiveisNoMapa.ICE;

        map = new IcesDefault[Linhas, Colunas];
        mapOriginal = new IcesDefault[Linhas, Colunas];

        iceExtentsX = ice.GetComponent<Renderer>().bounds.extents.x;
        iceExtentsY = ice.GetComponent<Renderer>().bounds.extents.y;

        PoolManager.instance.CreatePool(ice, linhas*colunas*2);
        PoolManager.instance.CreatePool(crate, linhas * colunas*2);

        CriarMapa();
    }

    #region Getters and Setters
    public short Linhas
    {
        get
        {
            return linhas;
        }

        set
        {
            linhas = value;
        }
    }

    public short Colunas
    {
        get
        {
            return colunas;
        }

        set
        {
            colunas = value;
        }
    }
    #endregion

    /*
    void CriarMapa()
    {
        // Para centralizar mapa
        float linhasSoma = (Linhas % 2 == 0) ? (Linhas / 2 - 1) * iceExtentsY * 2 + iceExtentsY : (Linhas / 2) * iceExtentsY * 2;
        float colunasSoma = (Colunas % 2 == 0) ? (Colunas / 2 - 1) * iceExtentsX * 2 + iceExtentsX : (Colunas / 2) * iceExtentsX * 2;
        Vector2 pos = mapIcesParent.position + new Vector3(0.0f,linhasSoma,0.0f) - new Vector3(colunasSoma,0.0f,0.0f);
        
        for (int i = 0; i < Linhas; i++)
        {
            for (int j = 0; j < Colunas; j++)
            {
                //GameObject iceTemp = Instantiate(ice, new Vector2(pos.x + iceExtentsX * 2 * j, pos.y), Quaternion.identity) as GameObject;
                GameObject novoIce = RandomIce(new Vector2(pos.x + iceExtentsX * 2 * j, pos.y)) as GameObject;
                //IcesDefault novoIceComponente = novoIce.GetComponent(typeof(IcesDefault)) as IcesDefault;
                map[i, j] = novoIce.GetComponent(typeof(IcesDefault)) as IcesDefault;
                map[i, j].InitIce((short)i, (short)j);
                //novoIceComponente.InitIce((short)i, (short)j);
                //map[i, j] = novoIceComponente;
                novoIce.name += " [" + i + "][" + j + "]";
                novoIce.transform.parent = mapIcesParent;

                GameObject copiaDoIce = RetornarElemento(map[i,j].Tipo);
                mapOriginal[i, j] = copiaDoIce.GetComponent<IcesDefault>();
                mapOriginal[i, j].InitIce((short)i, (short)j);

                mapOriginalList.Add(mapOriginal[i,j]);
                mapList.Add(mapOriginal[i, j]);
            }
            pos = new Vector2(pos.x, pos.y - iceExtentsY * 2);
        }

        // Zoom in e Zoom out main camera
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(map[Linhas/2,0].gameObject.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        while (!onScreen)
        {
            Camera.main.orthographicSize += zoomDaCamera;
            screenPoint = Camera.main.WorldToViewportPoint(map[Linhas/2, 0].gameObject.transform.position);
            onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }
        //Camera.main.orthographicSize = widthToBeSeen * Screen.height / Screen.width * 0.5;
    }
    */

    void CriarMapa()
    {
        // Para centralizar mapa
        float linhasSoma = (Linhas % 2 == 0) ? (Linhas / 2 - 1) * iceExtentsY * 2 + iceExtentsY : (Linhas / 2) * iceExtentsY * 2;
        float colunasSoma = (Colunas % 2 == 0) ? (Colunas / 2 - 1) * iceExtentsX * 2 + iceExtentsX : (Colunas / 2) * iceExtentsX * 2;
        Vector2 pos = mapIcesParent.position + new Vector3(0.0f, linhasSoma, 0.0f) - new Vector3(colunasSoma, 0.0f, 0.0f);

        for (int i = 0; i < Linhas; i++)
        {
            for (int j = 0; j < Colunas; j++)
            {
                PoolManager.instance.ReuseObject(ice, new Vector2(pos.x + iceExtentsX * 2 * j, pos.y), Quaternion.identity, (short)i, (short)j);
            }
            pos = new Vector2(pos.x, pos.y - iceExtentsY * 2);
        }

        // Zoom in e Zoom out main camera
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(map[Linhas / 2, 0].gameObject.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        while (!onScreen)
        {
            Camera.main.orthographicSize += zoomDaCamera;
            screenPoint = Camera.main.WorldToViewportPoint(map[Linhas / 2, 0].gameObject.transform.position);
            onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }
        //Camera.main.orthographicSize = widthToBeSeen * Screen.height / Screen.width * 0.5;
    }

    public void RestartMap()
    {
        for (int i = 0; i < Linhas; i++)
        {
            for (int j = 0; j < Colunas; j++)
            {
                Destroy(map[i,j].gameObject);
            }
        }

        // Para centralizar mapa
        float linhasSoma = (Linhas % 2 == 0) ? (Linhas / 2 - 1) * iceExtentsY * 2 + iceExtentsY : (Linhas / 2) * iceExtentsY * 2;
        float colunasSoma = (Colunas % 2 == 0) ? (Colunas / 2 - 1) * iceExtentsX * 2 + iceExtentsX : (Colunas / 2) * iceExtentsX * 2;
        Vector2 pos = mapIcesParent.position + new Vector3(0.0f, linhasSoma, 0.0f) - new Vector3(colunasSoma, 0.0f, 0.0f);

        for (int i = 0; i < Linhas; i++)
        {
            for (int j = 0; j < Colunas; j++)
            {
                // Reseto o mapa
                map = new IcesDefault[Linhas, Colunas];
                mapList = new List<IcesDefault>();

                GameObject novoIce = Instantiate(mapOriginal[i, j].gameObject, new Vector2(pos.x + iceExtentsX * 2 * j, pos.y), Quaternion.identity);
                map[i, j] = mapOriginal[i,j];
                map[i, j].InitIce((short)i, (short)j);

                novoIce.name = " [" + i + "][" + j + "]";
                novoIce.transform.parent = mapIcesParent;

                mapList.Add(mapOriginal[i, j]);
            }
            pos = new Vector2(pos.x, pos.y - iceExtentsY * 2);
        }
    }


    public bool VerificarSeEstaDentroDoMapa(short i, short j)
    {
        return (i < Linhas && i >= 0) && (j < Colunas && j >= 0);
    }


    public GameObject RetornarElemento(elementosPossiveisNoMapa element)
    {
        switch (element)
        {
            case elementosPossiveisNoMapa.ICE:
                return ice;
            case elementosPossiveisNoMapa.CRATE:
                return crate;
            case elementosPossiveisNoMapa.BURACO:
                return buraco;
            case elementosPossiveisNoMapa.START:
                return start;
            case elementosPossiveisNoMapa.END:
                return end;
            case elementosPossiveisNoMapa.ICE_QUEBRADO_1:
                return ice_quebrado_1;
            case elementosPossiveisNoMapa.ICE_QUEBRADO_2:
                return ice_quebrado_2;
            case elementosPossiveisNoMapa.ICE_QUEBRADO_3:
                return ice_quebrado_3;
            case elementosPossiveisNoMapa.ICE_QUEBRADO_FINAL:
                return ice_quebrado_final;
            default:
                Debug.Log("Class MapCreator, Function InstanciarElemento: elemento não existe");
                return ice;
        }
    }


    /*
    GameObject RandomIce(Vector2 pos)
    {
        GameObject temp;
        int x;
        if (numberOfCrates > 0 && count == 4)
            x = Random.Range(1, 3);
        else
        {
            count++;
            x = 1;
        }
        switch (x)
        {
            case 1:
                temp = Instantiate(ice, pos, Quaternion.identity);
                temp.name = "Ice";
                return temp;
            case 2:
                temp = Instantiate(crate, pos, Quaternion.identity);
                temp.name = "Crate";
                numberOfCrates--;
                count = 0;
                return temp;
            case 3:
                temp = Instantiate(end, pos, Quaternion.identity);
                temp.name = "End";
                return temp;
            case 4:
                temp = Instantiate(start, pos, Quaternion.identity);
                temp.name = "Start";
                return temp;
            case 5:
                temp = Instantiate(buraco, pos, Quaternion.identity);
                temp.name = "Buraco";
                return temp;
            default:
                temp = Instantiate(ice, pos, Quaternion.identity);
                temp.name = "Ice";
                return temp;
        }
    }
    */

    GameObject RandomIce(Vector2 pos)
    {
        GameObject temp;
        int x;
        if (numberOfCrates > 0 && count == 4)
            x = Random.Range(1, 3);
        else
        {
            count++;
            x = 1;
        }
        switch (x)
        {
            case 1:
                temp = Instantiate(ice, pos, Quaternion.identity);
                temp.name = "Ice";
                return temp;
            case 2:
                temp = Instantiate(crate, pos, Quaternion.identity);
                temp.name = "Crate";
                numberOfCrates--;
                count = 0;
                return temp;
            case 3:
                temp = Instantiate(end, pos, Quaternion.identity);
                temp.name = "End";
                return temp;
            case 4:
                temp = Instantiate(start, pos, Quaternion.identity);
                temp.name = "Start";
                return temp;
            case 5:
                temp = Instantiate(buraco, pos, Quaternion.identity);
                temp.name = "Buraco";
                return temp;
            default:
                temp = Instantiate(ice, pos, Quaternion.identity);
                temp.name = "Ice";
                return temp;
        }
    }

}

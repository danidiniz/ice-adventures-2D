using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCreatorGUIManager : MonoBehaviour
{
    #region Crate
    // informações do que é permitido fazer com a crate
    public GameObject canvasInGameCrateBooleansInfo;

    public GameObject crateInfoPanel; // esse é o parente dos panels da caixa
    public GameObject[] panelsInformacoesDaCaixa;
    public GameObject[] numerosPanelDaCaixa;
    public Toggle toggleIsQuebravel;
    public Toggle toggleIsPulavel;
    public Toggle toggleIsEmpurravel;
    public Text quantidadeDeVezesQuePodeEmpurrar;
    public Color corNumeroCaixaSelecionado;
    public Color corNumeroCaixaSemEstarSelecionado;
    public IceCrate crateSelecionada;
    #endregion

    #region Buraco
    public static IceBuraco buracoSelecionado;

    // prefab da arrow para colocar no inspector
    public GameObject prefabArrowParaMostrarBuracos;
    // game object que sera usado pelas outras classes
    public static GameObject arrowParaMostrarBuracos;
    // centro do buraco e posição que a arrow será instanciada
    public static Vector3 posicaoParaInstanciarArrow = new Vector3(0.0f, 0.2f, 0.0f);
    // offset da arrow em cima do buraco
    public static Vector3 offSetArrowEmCimaDoBuraco = new Vector3(0.0f, 0.3f, 0.0f);

    public bool isSelecionandoBuraco;
    #endregion


    public GameObject elementosPossiveisNoMapaPanel;

    public Image objetoSelecionado;

    static MapCreatorGUIManager _instance;

    public static MapCreatorGUIManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapCreatorGUIManager>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        arrowParaMostrarBuracos = Instantiate(prefabArrowParaMostrarBuracos, Vector3.zero, Quaternion.identity) as GameObject;
        arrowParaMostrarBuracos.name = prefabArrowParaMostrarBuracos.name;
        arrowParaMostrarBuracos.SetActive(false);
    }

    public void AbrirOuFecharPanelElementosDoMapa()
    {
        crateInfoPanel.SetActive(false);
        elementosPossiveisNoMapaPanel.SetActive(!elementosPossiveisNoMapaPanel.activeSelf);
    }


    #region Crate
    public void FecharPanelsInformacoesDaCaixa()
    {
        for (int i = 0; i < panelsInformacoesDaCaixa.Length; i++)
        {
            panelsInformacoesDaCaixa[i].SetActive(!panelsInformacoesDaCaixa[i].activeSelf);
        }
    }

    public void MudarPosicaoDoCanvasDaCrateParaCrateSelecionada()
    {
        if(crateSelecionada != null)
        {
            canvasInGameCrateBooleansInfo.SetActive(true);
            canvasInGameCrateBooleansInfo.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            canvasInGameCrateBooleansInfo.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
            canvasInGameCrateBooleansInfo.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(true);
            if (!crateSelecionada.IsCrateQuebravel)
            {
                canvasInGameCrateBooleansInfo.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            if (!crateSelecionada.IsCratePulavel)
            {
                canvasInGameCrateBooleansInfo.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
            if (!crateSelecionada.IsCrateEmpurravel && crateSelecionada.QuantidadeDeVezesQueACratePodeSerEmpurrada <= 0)
            {
                canvasInGameCrateBooleansInfo.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(false);
            }
            canvasInGameCrateBooleansInfo.transform.position = crateSelecionada.transform.position;
        }
    }

    #region Quebrar, Pular, Empurrar da Crate
    public void Quebrar()
    {
        AbrirOuFecharCanvasDaCrate();
        crateSelecionada.Quebrar(PlayerInfo.instance);
        crateSelecionada = null;
    }
    public void Pular()
    {
        AbrirOuFecharCanvasDaCrate();
        crateSelecionada.Pular(PlayerInfo.instance);
        crateSelecionada = null;
    }
    public void Empurrar()
    {
        AbrirOuFecharCanvasDaCrate();
        crateSelecionada.Empurrar(PlayerInfo.instance);
        crateSelecionada = null;
    }
    #endregion

    public void AbrirOuFecharCanvasDaCrate()
    {
        if (crateSelecionada != null)
        {
            canvasInGameCrateBooleansInfo.SetActive(!canvasInGameCrateBooleansInfo.activeSelf);
        }
    }
    
    // Método que abre panel e atualiza informações da caixa
    public void AbrirPanelInformacaoDaCaixa(int numeroDoPanel)
    {
        crateInfoPanel.SetActive(true);
        FecharPanelsInformacoesDaCaixa();

        for (int i = 0; i < panelsInformacoesDaCaixa.Length; i++)
        {
            if (i == numeroDoPanel)
            {
                panelsInformacoesDaCaixa[i].SetActive(true);
                numerosPanelDaCaixa[i].GetComponent<Image>().color = corNumeroCaixaSelecionado;
            }
            else
            {
                numerosPanelDaCaixa[i].GetComponent<Image>().color = corNumeroCaixaSemEstarSelecionado;
                panelsInformacoesDaCaixa[i].SetActive(false);
            }
        }

        toggleIsQuebravel.isOn = crateSelecionada.IsCrateQuebravel;
        toggleIsPulavel.isOn = crateSelecionada.IsCratePulavel;
        toggleIsEmpurravel.isOn = crateSelecionada.IsCrateEmpurravel;
        quantidadeDeVezesQuePodeEmpurrar.text = "Quantidade de vezes que pode empurrar " + crateSelecionada.QuantidadeDeVezesQueACratePodeSerEmpurrada.ToString();
    }

    public void AlterandoToggleDaCrate(int toggle)
    {
        switch (toggle)
        {
            case 0:
                crateSelecionada.IsCrateQuebravel = toggleIsQuebravel.isOn;
                break;
            case 1:
                crateSelecionada.IsCratePulavel = toggleIsPulavel.isOn;
                break;
            case 2:
                crateSelecionada.IsCrateEmpurravel = toggleIsEmpurravel.isOn;
                break;
            default:
                Debug.Log("Toggle não encontrado");
                break;
        }
    }
    #endregion

    #region Buraco
    // Método para mostrar buracos relacionados
    public void MostrarArrowNosBuracos()
    {
        if (buracoSelecionado == null)
            return;

        IceBuraco temp = MapCreator.map[buracoSelecionado.posicaoDoOutroBuracoI, buracoSelecionado.posicaoDoOutroBuracoJ].GetComponent(typeof(IceBuraco)) as IceBuraco;
        if (temp != null)
        {
            //temp.MostrarArrowNosBuracos();
        }
    }
    #endregion


    #region Map Creator
    public void MapCreatorAtivarOuDesativar()
    {
        MapCreator.instance.modoCriarMapaAtivado = !MapCreator.instance.modoCriarMapaAtivado;

        // Fechando canvas da crate
        canvasInGameCrateBooleansInfo.SetActive(false);
        crateSelecionada = null;

        objetoSelecionado.gameObject.SetActive(!objetoSelecionado.gameObject.activeSelf);

    }
#endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IceCrate : IcesDefault
{
    public bool isCrateComum;
    [SerializeField]
    protected bool isCrateQuebravel;
    [SerializeField]
    protected bool isCratePulavel;
    [SerializeField]
    protected bool isCrateEmpurravel;
    [SerializeField]
    protected short quantidadeDeVezesQueACratePodeSerEmpurrada;
    
    public MapCreator.elementosPossiveisNoMapa elementoDentroDaCaixa;
    


    private void Awake()
    {
        // Temporario
        isCrateComum = true;
        IsCratePulavel = true;
        IsCrateEmpurravel = true;
        IsCrateQuebravel = true;
        quantidadeDeVezesQueACratePodeSerEmpurrada = 1;
    }

    void Start()
    {
        isWalkable = false;
        pararMovimentoDeQuemPassarPorCima = true;

        Tipo = MapCreator.elementosPossiveisNoMapa.CRATE;
    }

    #region Getters and Setters
    public bool IsCrateQuebravel
    {
        get
        {
            return isCrateQuebravel;
        }

        set
        {
            isCrateQuebravel = value;
        }
    }

    public bool IsCratePulavel
    {
        get
        {
            return isCratePulavel;
        }

        set
        {
            isCratePulavel = value;
        }
    }

    public bool IsCrateEmpurravel
    {
        get
        {
            return isCrateEmpurravel;
        }

        set
        {
            isCrateEmpurravel = value;
        }
    }

    public short QuantidadeDeVezesQueACratePodeSerEmpurrada
    {
        get
        {
            return quantidadeDeVezesQueACratePodeSerEmpurrada;
        }

        set
        {
            quantidadeDeVezesQueACratePodeSerEmpurrada = value;
        }
    }
    #endregion

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Tipo == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:

                break;

            case MapCreator.elementosPossiveisNoMapa.CRATE:
                break;
            default:
                break;
        }
        return false;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if (MapCreator.instance.modoCriarMapaAtivado)
        {
            if(MapCreator.instance.elementoSelecionado != MapCreator.elementosPossiveisNoMapa.CRATE)
            {
                return;
            }
            MapCreatorGUIManager.instance.crateSelecionada = this;
            MapCreatorGUIManager.instance.AbrirPanelInformacaoDaCaixa(0);
        }
        else
        {
            // Abrir opções do que o player pode fazer
            // Se clicar na mesma caixa eu simplesmente abro ou fecho o canvas
            if(MapCreatorGUIManager.instance.crateSelecionada == this)
            {
                MapCreatorGUIManager.instance.AbrirOuFecharCanvasDaCrate();
            }
            // Se não, ao clicar em outra crate eu mudo a posicao do canvas até la
            else
            {
                MapCreatorGUIManager.instance.crateSelecionada = this;
                MapCreatorGUIManager.instance.MudarPosicaoDoCanvasDaCrateParaCrateSelecionada();
            }
            
            if (PlayerInfo.instance != null)
            {
                /*
                if(IsCrateQuebravel)
                {
                    Quebrar(PlayerInfo.instance);
                }
                else if (IsCrateEmpurravel)
                {
                    Empurrar(PlayerInfo.instance);
                }
                else if (IsCratePulavel)
                {
                    Pular(PlayerInfo.instance);
                }*/
            }
            else
                Debug.Log("PlayerInfo null");
        }
    }

    public virtual void Quebrar(SerVivo quemEstaQuebrando)
    {
        // Pode conter algo dentro?
        // Qualquer elemento do jogo!! xD

        Debug.Log("O " + quemEstaQuebrando.NameDoElemento + " está tentando quebrar a " + name);

        if (!IsCrateQuebravel)
            return;
        
        // Verificando se player está em volta da caixa
        if (((quemEstaQuebrando.PosI == posI + 1 || (quemEstaQuebrando.PosI == posI - 1)) && quemEstaQuebrando.PosJ == posJ) ||
        (((quemEstaQuebrando.PosJ == posJ + 1) || (quemEstaQuebrando.PosJ == posJ - 1)) && quemEstaQuebrando.PosI == posI))
        {
            SerTransformadoEm(tipoDeIceAntesDeSerTransformado);
        }

    }

    public virtual void Pular(SerVivo quemEstaPulando)
    {
        // Pula apenas se não houver outra Crate no Ice do outro lado
        // Pode pular caso tenha um buraco do outro lado


        Debug.Log("O " + quemEstaPulando.NameDoElemento + " está tentando pular a " + name);

        if (!IsCratePulavel)
            return;
        
        // Verificando se player está em volta da caixa
        if (((quemEstaPulando.PosI == posI + 1 || (quemEstaPulando.PosI == posI - 1)) && quemEstaPulando.PosJ == posJ) ||
        (((quemEstaPulando.PosJ == posJ + 1) || (quemEstaPulando.PosJ == posJ - 1)) && quemEstaPulando.PosI == posI))
        {
            // Faço a diferença das posições da Crate e de quem está empurrando
            // Dessa forma basta eu somar à posição da Crate para saber em qual eixo vou andar
            short movimentaI = (short)(PosI - quemEstaPulando.PosI);
            short movimentaJ = (short)(PosJ - quemEstaPulando.PosJ);

            if(MapCreator.instance.VerificarSeEstaDentroDoMapa((short)(PosI + movimentaI), (short)(PosJ + movimentaJ)))
            {
                bool crateFoiPulada = MapCreator.map[posI + movimentaI, posJ + movimentaJ].AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa.PLAYER, quemEstaPulando);
                if (!crateFoiPulada)
                {
                    return;
                }
                quemEstaPulando.PosI = (short)(posI + movimentaI);
                quemEstaPulando.PosJ = (short)(posJ + movimentaJ);
                
            }
        }
    }

    public virtual void Empurrar(SerVivo quemEstaEmpurrandoInfo)
    {
        string log = "O " + quemEstaEmpurrandoInfo.NameDoElemento + " está tentando empurrar a " + name;

        // DELETAR
        if (!IsCrateEmpurravel)
        {
            log += " mas ela é não é empurrável";
            Debug.Log(log);
        }

        if (!IsCrateEmpurravel)
            return;
        
        // Verificando se player está em volta da caixa
        if (((quemEstaEmpurrandoInfo.PosI == posI + 1 || (quemEstaEmpurrandoInfo.PosI == posI - 1)) && quemEstaEmpurrandoInfo.PosJ == posJ) ||
        (((quemEstaEmpurrandoInfo.PosJ == posJ + 1) || (quemEstaEmpurrandoInfo.PosJ == posJ - 1)) && quemEstaEmpurrandoInfo.PosI == posI))
        {
            bool crateFoiEmpurrada = false;

            // Player à cima da Crate
            if (quemEstaEmpurrandoInfo.PosI < posI)
            {
                // Verifica se pra onde a Crate vai ser empurrada está dentro do map
                if (MapCreator.instance.VerificarSeEstaDentroDoMapa((short)(posI + 1), posJ))
                {
                    crateFoiEmpurrada = MapCreator.map[posI + 1, posJ].AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa.CRATE, this);
                }
            }
            // Player à direita da Crate
            else if (quemEstaEmpurrandoInfo.PosJ > posJ)
            {
                if (MapCreator.instance.VerificarSeEstaDentroDoMapa(posI, (short)(posJ - 1)))
                {
                    crateFoiEmpurrada = MapCreator.map[posI, posJ - 1].AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa.CRATE, this);
                }
            }
            // Player embaixo da Crate
            else if (quemEstaEmpurrandoInfo.PosI > posI)
            {
                // Verifica se pra onde a Crate vai ser empurrada está dentro do map
                if (MapCreator.instance.VerificarSeEstaDentroDoMapa((short)(posI - 1), posJ))
                {
                    crateFoiEmpurrada = MapCreator.map[posI - 1, posJ].AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa.CRATE, this);
                }
            }
            // Player à esquerda da Crate
            else if (quemEstaEmpurrandoInfo.PosJ < posJ)
            {
                if (MapCreator.instance.VerificarSeEstaDentroDoMapa(posI, (short)(posJ + 1)))
                {
                    crateFoiEmpurrada = MapCreator.map[posI, posJ + 1].AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa.CRATE, this);
                }
            }

            // Se a crate foi empurrada com sucesso, transformo esse ice no que ele era antes de virar crate
            if (crateFoiEmpurrada)
            {
                SerTransformadoEm(tipoDeIceAntesDeSerTransformado);
            }
        }

        // DELETAR
        else
        {
            log += " mas está distante dela";
        }
        Debug.Log(log);

    }

}

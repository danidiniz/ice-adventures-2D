using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IceCrate : ObjetoDoMapa, IQuebravel<SerVivo>, IPulavel<SerVivo>, IEmpurravel<SerVivo>, IUndoInteraction<ElementoDoMapa, ElementoDoMapa>
{
    
    
    public MapCreator.elementosPossiveisNoMapa elementoDentroDaCaixa;

    private void Awake()
    {
        isWalkable = false;
        pararMovimentoDeQuemPassarPorCima = true;

        // Temporario
        isComum = true;
        IsCratePulavel = true;
        IsCrateEmpurravel = true;
        IsCrateQuebravel = true;
        quantidadeDeVezesQuePodeSerEmpurrada = 1;

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
                MapCreator.map[posI, posJ].elementoEmCimaDoIce.gameObject.SetActive(false);
                MapCreator.map[posI, posJ].elementoEmCimaDoIce = null;
                MapCreator.map[posI, posJ].SerTransformadoEm(MapCreator.instance.elementoSelecionado);
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
        // Criando a interaction
        CriarInteraction(quemEstaQuebrando);






        // Pode conter algo dentro?
        // Qualquer elemento do jogo!! xD

        Debug.Log("O " + quemEstaQuebrando.NameDoElemento + " está tentando quebrar a " + name);

        if (!IsCrateQuebravel)
            return;
        
        // Verificando se player está em volta da caixa
        if (((quemEstaQuebrando.PosI == posI + 1 || (quemEstaQuebrando.PosI == posI - 1)) && quemEstaQuebrando.PosJ == posJ) ||
        (((quemEstaQuebrando.PosJ == posJ + 1) || (quemEstaQuebrando.PosJ == posJ - 1)) && quemEstaQuebrando.PosI == posI))
        {
            Destroy(gameObject);
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
            // Faço a diferença das posições da Crate e de quem está pulando
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
            bool objetoFoiEmpurrado = false;

            // Player à cima da Crate
            if (quemEstaEmpurrandoInfo.PosI < posI)
            {
                // Verifica se pra onde o Objeto vai ser empurrado está dentro do map
                if (MapCreator.instance.VerificarSeEstaDentroDoMapa((short)(posI + 1), posJ))
                {
                    // Verifico se pra onde o Objeto vai ser empurrado está vazio
                    if(!MapCreator.map[posI + 1, posJ].temAlgoEmCima())
                    {
                        objetoFoiEmpurrado = true;
                        MapCreator.map[posI + 1, posJ].ColocarEmCimaDoIce(this);
                    }
                }
            }
            // Player à direita da Crate
            else if (quemEstaEmpurrandoInfo.PosJ > posJ)
            {
                if (MapCreator.instance.VerificarSeEstaDentroDoMapa(posI, (short)(posJ - 1)))
                {
                    // Verifico se pra onde o Objeto vai ser empurrado está vazio
                    if (!MapCreator.map[posI, posJ - 1].temAlgoEmCima())
                    {
                        objetoFoiEmpurrado = true;
                        MapCreator.map[posI, posJ - 1].ColocarEmCimaDoIce(this);
                    }
                }

            }
            // Player embaixo da Crate
            else if (quemEstaEmpurrandoInfo.PosI > posI)
            {
                // Verifica se pra onde a Crate vai ser empurrada está dentro do map
                if (MapCreator.instance.VerificarSeEstaDentroDoMapa((short)(posI - 1), posJ))
                {
                    // Verifico se pra onde o Objeto vai ser empurrado está vazio
                    if (!MapCreator.map[posI - 1, posJ].temAlgoEmCima())
                    {
                        objetoFoiEmpurrado = true;
                        MapCreator.map[posI - 1, posJ].ColocarEmCimaDoIce(this);
                    }
                }
            }
            // Player à esquerda da Crate
            else if (quemEstaEmpurrandoInfo.PosJ < posJ)
            {
                if (MapCreator.instance.VerificarSeEstaDentroDoMapa(posI, (short)(posJ + 1)))
                {
                    // Verifico se pra onde o Objeto vai ser empurrado está vazio
                    if (!MapCreator.map[posI, posJ + 1].temAlgoEmCima())
                    {
                        objetoFoiEmpurrado = true;
                        MapCreator.map[posI, posJ + 1].ColocarEmCimaDoIce(this);
                    }
                }
            }
            if (objetoFoiEmpurrado)
            {
                // Retiro o que está em cima desse Ice
                MapCreator.map[PosI, PosJ].elementoEmCimaDoIce = null;
                // Atualizo a posição do objeto
                setPosition(posI, posJ);
            }
            else
            {
                log += " mas já tem algo em cima do outro Ice.";
            }
        }
        // DELETAR
        else
        {
            log += " mas está distante dela";
        }

        // Simplificado
        /*
        short i = (short)(Mathf.Abs(quemEstaEmpurrandoInfo.PosI - PosI));
        short j = (short)(Mathf.Abs(quemEstaEmpurrandoInfo.PosJ - PosJ));
        if(i > 1 || j > 1)
        {
            log += " mas está distante dela.";
        }
        else
        {

        }
        */
        Debug.Log(log);
    }

    public void CriarInteraction(ElementoDoMapa elementoQuePassouPorCima)
    {
        UndoRedo.interactionsTemp.Add(new UndoInteraction(elementoQuePassouPorCima, this));
    }

    public void ExecutarUndoInteraction(ElementoDoMapa elementoQuePassouPorCima, ElementoDoMapa elementoQueInteragiu)
    {
        this.gameObject.SetActive(true);

        /*
        if(UndoRedo.steps.Peek().interactions.Count > 0)
        {
            for (int i = 0; i < UndoRedo.steps.Peek().interactions.Count; i++)
            {
                Debug.Log("Caixas colocadas na pos [" + UndoRedo.steps.Peek().interactions[i].ElementoQueInteragiu.PosI + "][" +
                    UndoRedo.steps.Peek().interactions[i].ElementoQueInteragiu.PosI + "]");
            }
        }
        */
    }
}

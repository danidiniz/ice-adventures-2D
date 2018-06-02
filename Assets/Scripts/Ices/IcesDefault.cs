using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class IcesDefault : ElementoDoMapa, IColliderIce<MapCreator.elementosPossiveisNoMapa, ElementoDoMapa>, IUndoInteraction<ElementoDoMapa, ElementoDoMapa, Passo.tiposDeInteraction>
{

    // Todo ice tem um tipo
    // Todo ice é clicável
    //Todo ice tem uma posição i,j
    // Todo ice tem um sprite?
    // Todo ice tem um método "collider" pra quando o player estiver em cima
    

    public enum coisasQuePodemEstarEmCimaDoIce
    {
        PLAYER,
        PINGUIM,
        CRATE, CRATE_COM_PINGUIM_1, CRATE_COM_PINGUIM_2, CRATE_COM_PINGUIM_3, CRATE_COM_PINGUIM_4,
        ESPECIAL_PEIXE, ESPECIAL_ASAS,
        START, END,
        ONDA_DA_ORCA, URSO_POLAR,
    };

    public ObjetoDoMapa elementoEmCimaDoIce;

    public IcesDefault()
    {
        TipoDoElemento = MapCreator.tipoDeElemento.ICE;
    }

    void Awake()
    {
        TipoDoElemento = MapCreator.tipoDeElemento.ICE;
    }

    public virtual bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoQuePassouNoIce)
    {
        //Debug.Log(elementoEmCimaDoIce.name + "["+PosI+"]["+PosJ+"]");
        if (ElementoNoMapa == oQueEstaEmCima)
            return false;
        
        // Nem todo Ice possui interação, por isso a interface de Interação não está aqui.
        // porém, todo Ice pode possuir um Objeto em cima (todos possuem interação),
        // então preciso verificar em todo Ice se ele possui Objeto e, se possuir,
        // crio uma interação desse objeto.
        if (elementoEmCimaDoIce != null)
        {
            // Um problema aqui..
            // O CriarInteraction tem como parametro esse objeto, porém
            // no ExecutarObjeto tem chance de eu desativar o objeto, o que
            // implica um NullException no Criar..
            // Para evitar isso eu preciso ciar sempre o dobro de GameObjects na cena..
            
            // Crio a Interaction do Objeto
            elementoEmCimaDoIce.CriarInteraction(elementoQuePassouNoIce, elementoEmCimaDoIce, Passo.tiposDeInteraction.INTERACTION_OBJETO);

            // Executo o que o Objeto faz primeiro
            // pois caso ele seja Reusado, tenho que desativar ele primeiro
            elementoEmCimaDoIce.ExecutarObjeto(elementoQuePassouNoIce);
        }       

        return true;
    }
    
    public override void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (MapCreator.instance.modoCriarMapaAtivado)
        {
            // Não transforma elementos do mesmo tipo
            if (ElementoNoMapa == MapCreator.instance.elementoSelecionado)
            {
                Debug.Log("Esse elemento já é um " + GetName());
                return;
            }

            // Verificando se o elemento selecionado é o Start ou End
            // se for, só posso ter 1 deles no mapa,
            // então verifico se já existe algum no mapa e deleto se houve
            // depois crio o novo Start ou End no ice que a pessoa clicou
            switch (MapCreator.instance.elementoSelecionado)
            {
                case MapCreator.elementosPossiveisNoMapa.START:
                    if(MapCreator.map[GameController.instance.posicaoDoStart.i, GameController.instance.posicaoDoStart.j].ElementoNoMapa == MapCreator.elementosPossiveisNoMapa.START)
                    {
                        MapCreator.map[GameController.instance.posicaoDoStart.i, GameController.instance.posicaoDoStart.j].SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE);
                    }
                    GameController.instance.posicaoDoStart.i = PosI;
                    GameController.instance.posicaoDoStart.j = PosJ;
                    break;
                case MapCreator.elementosPossiveisNoMapa.END:
                    if (MapCreator.map[GameController.instance.posicaoDoEnd.i, GameController.instance.posicaoDoEnd.j].ElementoNoMapa == MapCreator.elementosPossiveisNoMapa.END)
                    {
                        MapCreator.map[GameController.instance.posicaoDoEnd.i, GameController.instance.posicaoDoEnd.j].SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE);
                    }
                    GameController.instance.posicaoDoEnd.i = PosI;
                    GameController.instance.posicaoDoEnd.j = PosJ;
                    break;
            }
            
            // Verificando o tipo do elemento (ICE ou OBJETO)
            if (ElementoNoMapa != MapCreator.instance.elementoSelecionado)
            {
                // Independente se for ICE ou OBJETO
                // preciso retirar o que está em cima primeiro
                if (elementoEmCimaDoIce != null)
                {
                    elementoEmCimaDoIce.gameObject.SetActive(false);
                    elementoEmCimaDoIce = null;
                }

                if (MapCreator.instance.tipoDoElementoSelecionado == MapCreator.tipoDeElemento.OBJETO)
                {
                    GameObject prefabDoElemento = MapCreator.instance.RetornarElemento(MapCreator.instance.elementoSelecionado);

                    PoolManager.instance.ReuseObjectEmCima(prefabDoElemento, transform.position, transform.rotation, posI, posJ);
                }
                else
                {
                    SerTransformadoEm(MapCreator.instance.elementoSelecionado);
                }
            }
        }
        else
        {
//            Debug.Log("Cliquei no " + name);
        }
    }

    protected string GetName()
    {
        switch (ElementoNoMapa)
        {
            case MapCreator.elementosPossiveisNoMapa.ICE:
                return "Ice";
            case MapCreator.elementosPossiveisNoMapa.CRATE:
                return "Crate";
            case MapCreator.elementosPossiveisNoMapa.BURACO:
                return "Buraco";
            case MapCreator.elementosPossiveisNoMapa.START:
                return "Start";
            case MapCreator.elementosPossiveisNoMapa.END:
                return "End";
            case MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_1:
                return "Ice 1";
            case MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2:
                return "Ice 2";
            case MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_3:
                return "Ice 3";
            case MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_FINAL:
                return "Ice final";
            default:
                return "Elemento não existe [" + posI + "][" + posJ + "]";
        }
    }

    

    // Apenas coloca um elemento que já existe em cima desse ice
    // No PollManager o ReuseObjectEmCima 'deleta' e cria um elemento novo.. (usando na opção map creator)
    public virtual void ColocarEmCimaDoIce(ObjetoDoMapa elemento)
    {
        elementoEmCimaDoIce = elemento;

        elemento.transform.position = transform.position;
        
        AlgoPassouPorAqui(elemento.ElementoNoMapa, elemento);

        //GameObject prefabDoElemento = MapCreator.instance.RetornarElemento(elemento);

        //PoolManager.instance.ColocarEmCima(prefabDoElemento, transform.position, transform.rotation, posI, posJ);
        
    }

    public virtual bool temAlgoEmCima()
    {
        if (elementoEmCimaDoIce != null)
            return true;
        return false;
    }

    public override void ResetarInformacoesDoElemento()
    {
        elementoEmCimaDoIce = null;
    }
    
    public virtual void CriarInteraction(ElementoDoMapa elementoQuePassouPorCima, ElementoDoMapa elementoQueSofreuInteraction, Passo.tiposDeInteraction tipoDaInteraction)
    {
        UndoRedo.interactionsTemp.Add(new UndoInteraction(elementoQuePassouPorCima, elementoQueSofreuInteraction, tipoDaInteraction));
    }

    // Mapa editor
    public void CriarInteraction(ElementoDoMapa elementoQueSofreuInteraction)
    {
        if(elementoQueSofreuInteraction.TipoDoElemento == MapCreator.tipoDeElemento.OBJETO)
        {

        }
    }

    public virtual void ExecutarUndoInteraction(ElementoDoMapa elementoQuePassouPorCima)
    {
        Debug.Log("Elemento na posicao desse ice: " + MapCreator.map[this.PosI, this.PosJ].ElementoNoMapa + " | Esse elemento: " + ElementoNoMapa + "[" + PosI + "][" + PosJ + "]");
        // Fazendo o Undo, transformando o elemento atual no que ele era antes
        MapCreator.map[this.PosI, this.PosJ].SerTransformadoEm(this.ElementoNoMapa);
        // Copiando informações do elemento holder para o Ice que estava na posição desse
        // lembrando que esse holder é apenas um componente que está segurando
        // as informações (quando eu crio o UndoInteraction) do ice antigo, 
        // então como Re usei um elementoele ainda não possui as informações
        this.CopiarInformacoesDesseElementoPara(MapCreator.map[this.PosI, this.PosJ]);
    }

    public override void CopiarInformacoesDesseElementoPara(ElementoDoMapa target)
    {
        base.CopiarInformacoesDesseElementoPara(target);

        // A única coisa que um Ice deve passar para outro é garantir que não passe o elemento em cima
        // porque quem destrói o elemento é ele próprio quando interage com algo
        ((IcesDefault)target).elementoEmCimaDoIce = null;
    }

    
}

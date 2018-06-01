using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class IcesDefault : ElementoDoMapa, IColliderIce<MapCreator.elementosPossiveisNoMapa, ElementoDoMapa> {

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

    void Awake()
    {
        TipoDoElemento = MapCreator.tipoDeElemento.ICE;
    }

    public virtual bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoQuePassouNoIce)
    {
        //Debug.Log(elementoEmCimaDoIce.name + "["+PosI+"]["+PosJ+"]");
        if (Elemento == oQueEstaEmCima)
            return false;
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
            if (Elemento == MapCreator.instance.elementoSelecionado)
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
                    if(MapCreator.map[GameController.instance.posicaoDoStart.i, GameController.instance.posicaoDoStart.j].Elemento == MapCreator.elementosPossiveisNoMapa.START)
                    {
                        MapCreator.map[GameController.instance.posicaoDoStart.i, GameController.instance.posicaoDoStart.j].SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE);
                    }
                    GameController.instance.posicaoDoStart.i = PosI;
                    GameController.instance.posicaoDoStart.j = PosJ;
                    break;
                case MapCreator.elementosPossiveisNoMapa.END:
                    if (MapCreator.map[GameController.instance.posicaoDoEnd.i, GameController.instance.posicaoDoEnd.j].Elemento == MapCreator.elementosPossiveisNoMapa.END)
                    {
                        MapCreator.map[GameController.instance.posicaoDoEnd.i, GameController.instance.posicaoDoEnd.j].SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE);
                    }
                    GameController.instance.posicaoDoEnd.i = PosI;
                    GameController.instance.posicaoDoEnd.j = PosJ;
                    break;
            }
            
            // Verificando o tipo do elemento (ICE ou OBJETO)
            if (Elemento != MapCreator.instance.elementoSelecionado)
            {
                if (MapCreator.instance.tipoDoElementoSelecionado == MapCreator.tipoDeElemento.OBJETO)
                {
                    GameObject prefabDoElemento = MapCreator.instance.RetornarElemento(MapCreator.instance.elementoSelecionado);

                    PoolManager.instance.ReuseObjectEmCima(prefabDoElemento, transform.position, transform.rotation, posI, posJ);
                }
                else
                {
                    if (elementoEmCimaDoIce != null)
                    {
                        elementoEmCimaDoIce.gameObject.SetActive(false);
                        elementoEmCimaDoIce = null;
                    }
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
        switch (Elemento)
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

    public virtual void SerTransformadoEm(MapCreator.elementosPossiveisNoMapa elemento)
    {
        GameObject prefabDoElemento = MapCreator.instance.RetornarElemento(elemento);

        PoolManager.instance.ReuseObject(prefabDoElemento, transform.position, transform.rotation, posI, posJ);
        
        gameObject.SetActive(false);
    }

    // Apenas coloca um elemento que já existe em cima desse ice
    // No PollManager o ReuseObjectEmCima 'deleta' e cria um elemento novo.. (usando na opção map creator)
    public virtual void ColocarEmCimaDoIce(ObjetoDoMapa elemento)
    {
        elementoEmCimaDoIce = elemento;

        elemento.transform.position = transform.position;
        
        AlgoPassouPorAqui(elemento.Elemento, elemento);

        //GameObject prefabDoElemento = MapCreator.instance.RetornarElemento(elemento);

        //PoolManager.instance.ColocarEmCima(prefabDoElemento, transform.position, transform.rotation, posI, posJ);
        
    }

    public virtual bool temAlgoEmCima()
    {
        if (elementoEmCimaDoIce != null)
            return true;
        return false;
    }

    public override void CopiarInformacoesDesseElementoPara(ElementoDoMapa target)
    {
        base.CopiarInformacoesDesseElementoPara(target);

        // Informações importantes de qualquer Ice
        // elemento em cima
        try
        {
            ((IcesDefault)target).elementoEmCimaDoIce = elementoEmCimaDoIce;
        } catch(Exception e)
        {
            print("Não copiou informações para o IcesDefault. Erro: " + e);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoNivel1 : IcesDefault
{
    //temp para testar se esta funcionando o CopiarInformacoes
    // resultado: sim, esta funcionando xD
    public int rand;

    public byte nivelDoIceQuebrado;

    public IceQuebradoNivel1()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;

        ElementoNoMapa = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_1;
        nivelDoIceQuebrado = 1;
    }

    void Awake()
    {
        //temp
        rand = UnityEngine.Random.Range(0, 100);

        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;

        ElementoNoMapa = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_1;
        nivelDoIceQuebrado = 1;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoQuePassouNoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoQuePassouNoIce);

        if (ElementoNoMapa == oQueEstaEmCima)
            return false;
        
        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
            case MapCreator.elementosPossiveisNoMapa.PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.URSO_POLAR:
            case MapCreator.elementosPossiveisNoMapa.CRATE:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_2:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_3:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_4:
                // Criando a Interaction desse Ice
                CriarInteraction(elementoQuePassouNoIce, this, Passo.tiposDeInteraction.INTERACTION_ICE);

                SerTransformadoEm(RetornarElementoDeAcordoComNivelDoIce());

                // Se ainda existir um Objeto em cima desse ice, passo para o Ice transformado
                if(elementoEmCimaDoIce != null)
                {
                    MapCreator.map[PosI, PosJ].elementoEmCimaDoIce = elementoEmCimaDoIce;
                }
                
                break;
            default:
                Debug.Log("Não encontrei o elemento " + oQueEstaEmCima + " no " + name);
                break;
        }
        
        return true;
    }

    public override void CopiarInformacoesDesseElementoPara(ElementoDoMapa target)
    {
        base.CopiarInformacoesDesseElementoPara(target);

        // Informações importantes de um IceQuebrado:
        // nivel
        // 
        try
        {
            ((IceQuebradoNivel1)target).rand = rand;
            ((IceQuebradoNivel1)target).nivelDoIceQuebrado = nivelDoIceQuebrado;
            Debug.Log("Copiei informações do " + ElementoNoMapa + " para o target " + target.ElementoNoMapa);
        }catch(Exception e)
        {
            Debug.Log("Não copiou informações do IceQuebradoNivel1. Erro: " + e);
        }
    }

    MapCreator.elementosPossiveisNoMapa RetornarElementoDeAcordoComNivelDoIce()
    {
        switch (nivelDoIceQuebrado)
        {
            case 1:
                return MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2;
            case 2:
                return MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_3;
            case 3:
                return MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_FINAL;
            default:
                Debug.Log("Falhou em retornar nivel do ice");
                return MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_1;
        }
    }
    
    /*
    public virtual void ExecutarUndoInteraction(ElementoDoMapa elementoQuePassouPorCima, ElementoDoMapa elementoAntesDoUndo)
    {
        // Lembrando que ESSE componente é o que está salvo na lista de Interactions
        // então todas informações pertinentes estão aqui
        // portanto, tenho que copiar essas informações pro próximo elemento que for usado na fila

        // Fazendo o Undo, transformando o elemento atual no que ele era antes
        MapCreator.map[elementoAntesDoUndo.PosI, elementoAntesDoUndo.PosJ].SerTransformadoEm(elementoAntesDoUndo.Elemento);
        // Agora preciso verificar se no Dictionary de interactionsObjetos há
        // algum objeto relacionado com esse Ice
        if (UndoRedo.steps.Peek().interactionsObjetos.ContainsKey(GetInstanceID()))
        {
            // Agora ativo o Objeto, caso ele esteja desativado
            // se ele não estiver ativado dá exception...
            if (!UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].gameObject.activeSelf)
            {
                UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].gameObject.SetActive(true);
            }


            Debug.Log("O " + gameObject.name + " possui interactions " + UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].gameObject.name);

            // Se há o objeto, vou executar o UndoInteraction dele 
            // no caso de um especial peixe, por exemplo, ele vai retirar o especial.. por isso os objetos também tem ExecutarUndo
            // Obs.: o elementoAntesDoUndo não serve pra nada no caso de objeto, pelo menos por enquanto
            UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].ExecutarUndoInteraction(elementoQuePassouPorCima, elementoAntesDoUndo);

            // Executo o método Reuse para colocar um objeto na fila do mesmo tipo
            // em cima do Ice
            GameObject g = UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].gameObject;
            PoolManager.instance.ReuseObjectEmCima(g, g.transform.position, g.transform.rotation,
                UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].PosI, UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].PosJ
                );

            // Agora copio as informações desse Objeto para o novo que veio da fila
            Debug.Log("Copiando para " + UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].PosI + ", " + UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].PosJ);

            UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].CopiarInformacoesDesseElementoPara(
                MapCreator.map[UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].PosI, UndoRedo.steps.Peek().interactionsObjetos[GetInstanceID()].PosJ]
                );

            // Por fim, removo essa interaction da lista, já que já executei
            //UndoRedo.steps.Peek().interactionsObjetos.Remove(GetInstanceID());

        }
    }
    */
}

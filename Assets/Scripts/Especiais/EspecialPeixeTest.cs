using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EspecialPeixeTest : ObjetoDoMapa
{

    // para testes se esta copiando certo
    public int randomPoints;

    public EspecialPeixeTest()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;
        TipoDoElemento = MapCreator.tipoDeElemento.OBJETO;
        Elemento = MapCreator.elementosPossiveisNoMapa.ESPECIAL_PEIXE;
    }

    private void Awake()
    {
        // temp
        randomPoints = UnityEngine.Random.Range(0, 100);

        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;
    }

    public override void ExecutarObjeto(ElementoDoMapa quemEstaEmCima)
    {
        if(quemEstaEmCima.Elemento == MapCreator.elementosPossiveisNoMapa.PLAYER)
        {
            Debug.Log("Comeu peixe de " + randomPoints + " points");

            
        }

        // Retiro o peixe de cima do Ice em qualquer caso?
        MapCreator.map[PosI, PosJ].elementoEmCimaDoIce = null;


        gameObject.SetActive(false);
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
                Debug.Log("Esse elemento já é um " + gameObject.name);
                return;
            }

            if (MapCreator.instance.elementoSelecionado != MapCreator.elementosPossiveisNoMapa.ESPECIAL_PEIXE)
            {
                MapCreator.map[posI, posJ].elementoEmCimaDoIce.gameObject.SetActive(false);
                MapCreator.map[posI, posJ].elementoEmCimaDoIce = null;
                MapCreator.map[posI, posJ].SerTransformadoEm(MapCreator.instance.elementoSelecionado);
                return;
            }
            // se não
            // abro as informações do especial
            // tempo que dura, qnt de vida, sei la, essas coisas
        }

    }

    public override void CopiarInformacoesDesseElementoPara(ElementoDoMapa target)
    {
        base.CopiarInformacoesDesseElementoPara(target);

        // Informações importantes de um Especial Peixe
        // pontos teste exemplo
        try
        {
            ((EspecialPeixeTest)target).randomPoints = randomPoints;
        }
        catch (Exception e)
        {
            print("Não copiou informações para o Peixe especial. Erro: " + e);
        }
    }

    public override void CriarInteraction(ElementoDoMapa elementoQuePassouPorCima, ElementoDoMapa elementoQueSofreuInteraction, Passo.tiposDeInteraction tipoDaInteraction)
    {
        UndoRedo.interactionsTemp.Add(new UndoInteraction(elementoQuePassouPorCima, elementoQueSofreuInteraction, tipoDaInteraction));
    }

    public override void ExecutarUndoInteraction(ElementoDoMapa elementoQuePassouPorCima)
    {
        // Coloquei os this para deixar bem claro que esse Componente é o Holder
        // lá no UndoInteraction
        // Eu só preciso do elementoQuePassouPorCima porque o peixe reage de uma forma
        // para cada ElementoDoMapa.
        try
        {
            if (elementoQuePassouPorCima.Elemento == MapCreator.elementosPossiveisNoMapa.PLAYER)
            {
                Debug.Log("Retirei " + randomPoints + " points.");
            }
            
            // Reuso um Objeto desse tipo e depois coloco na posição que estava
            PoolManager.instance.ReuseObjectEmCima(
                MapCreator.instance.RetornarElemento(Elemento), MapCreator.map[this.PosI, this.PosJ].transform.position, MapCreator.map[this.PosI, this.PosJ].transform.rotation, this.PosI, this.PosJ
                );

            // Como o Reuse já colocou o Objeto em cima do Ice
            // apenas copio as informações para o Objeto que está na posição
            this.CopiarInformacoesDesseElementoPara(MapCreator.map[this.PosI, this.PosJ].elementoEmCimaDoIce);
        }
        catch(Exception e)
        {
            Debug.Log("Não executou Undo do Peixe. Erro: " + e);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoInteraction : Passo {

    // temporario
    public static int contadorInteract;

    ElementoDoMapa holderElementoQueMovimentouEmCimaHolder;
    ElementoDoMapa holderElementoQueSofreuInteraction;
    short posI;
    short posJ;
    
    public UndoInteraction(ElementoDoMapa queMovimentou, ElementoDoMapa elementoQueSofreuInteraction, Passo.tiposDeInteraction t)
    {
        // temp
        contadorInteract++;


        //elementoTemporario = new ElementoDoMapa[2];
        //elementoTemporario[0] = new ElementoDoMapa();
        //elementoTemporario[1] = new ElementoDoMapa();
        //QueMovimentou.CopiarInformacoesDesseElementoPara(elementoTemporario[0]);
        //_elementoAntesDoUndo.CopiarInformacoesDesseElementoPara(elementoTemporario[1]);

        // Crio os ElementosDoMapa que irão segurar as informações desses elementos da interação
        // (preciso fazer isso porque se caso os elementos da interação se percam no caminho,
        //  não irá dar nullExcpetion)
        holderElementoQueMovimentouEmCimaHolder = CriarComponenteDeAcordoComTipo(queMovimentou.Elemento);
        //CriarComponenteDeAcordoComTipo(holderElementoQueMovimentouEmCimaHolder, queMovimentou.Elemento);
        holderElementoQueSofreuInteraction = CriarComponenteDeAcordoComTipo(elementoQueSofreuInteraction.Elemento);
        //CriarComponenteDeAcordoComTipo(holderElementoQueSofreuInteraction, elementoQueSofreuInteraction.Elemento);
        // Limpa o Holder após utilizá-lo
        MapCreator.instance.LimparHolder();

        //Debug.Log("Criou holder elemento antes de Undo: " + holderElementoQueMovimentouEmCimaHolder.Elemento + " [" + holderElementoQueMovimentouEmCimaHolder.PosI + "][" + holderElementoQueMovimentouEmCimaHolder.PosJ + "] | TARGET: " + queMovimentou.Elemento);
        //Debug.Log("Criou holder elemento antes de Undo: " + holderElementoQueSofreuInteraction.Elemento + " [" + holderElementoQueSofreuInteraction.PosI + "][" + holderElementoQueSofreuInteraction.PosJ + "] | TARGET: " + elementoQueSofreuInteraction.Elemento);

        // Agora copio as informações dos elementos da interação para
        // esses elementos holders
        queMovimentou.CopiarInformacoesDesseElementoPara(holderElementoQueMovimentouEmCimaHolder);
        elementoQueSofreuInteraction.CopiarInformacoesDesseElementoPara(holderElementoQueSofreuInteraction);

        DefinirTipoDePasso(t);
    }

    protected override void DefinirTipoDePasso(tiposDeInteraction tipo)
    {
        tipoDaInteractionQueAconteceu = tipo;
    }

    ElementoDoMapa CriarComponenteDeAcordoComTipo(MapCreator.elementosPossiveisNoMapa tipo)
    {
        // Gambiarra mas funciona.. ;/
        switch (tipo)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
                 MapCreator.Holder.AddComponent<PlayerInfo>();
                 return MapCreator.Holder.GetComponent<PlayerInfo>();
            case MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_1:
                MapCreator.Holder.AddComponent<IceQuebradoNivel1>();
                return MapCreator.Holder.GetComponent<IceQuebradoNivel1>();
            case MapCreator.elementosPossiveisNoMapa.ESPECIAL_PEIXE:
                MapCreator.Holder.AddComponent<EspecialPeixeTest>();
                return MapCreator.Holder.GetComponent<EspecialPeixeTest>();
            default:
                Debug.Log("Elemento " + tipo + " não está no CriarComponenteDeAcordoComTipo em UndoInteraction");
                return null;
        }
    }

    #region Getters and Setters
    public short PosI
    {
        get
        {
            return posI;
        }

        set
        {
            posI = value;
        }
    }

    public short PosJ
    {
        get
        {
            return posJ;
        }

        set
        {
            posJ = value;
        }
    }

    public ElementoDoMapa HolderElementoQueMovimentouEmCimaHolder
    {
        get
        {
            return holderElementoQueMovimentouEmCimaHolder;
        }

        set
        {
            holderElementoQueMovimentouEmCimaHolder = value;
        }
    }

    public ElementoDoMapa HolderElementoQueSofreuInteraction
    {
        get
        {
            return holderElementoQueSofreuInteraction;
        }

        set
        {
            holderElementoQueSofreuInteraction = value;
        }
    }
    #endregion
}

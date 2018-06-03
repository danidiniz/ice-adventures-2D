using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoInteractionIce : UndoInteraction {

    ElementoDoMapa holderElementoQueMovimentouEmCimaHolder;

    // Ingame
    public UndoInteractionIce(ElementoDoMapa elementoQuePassouEmCima, ElementoDoMapa iceQueSofreuInteraction, Passo.tiposDeInteraction t)
    {
        // temp
        contadorInteract++;

        // Crio os ElementosDoMapa que irão segurar as informações desses elementos da interação
        // (preciso fazer isso porque se caso os elementos da interação se percam no caminho,
        //  não irá dar nullExcpetion)
        holderElementoQueMovimentouEmCimaHolder = CriarComponenteDeAcordoComTipo(elementoQuePassouEmCima.ElementoNoMapa);
        holderElementoQueSofreuInteraction = CriarComponenteDeAcordoComTipo(iceQueSofreuInteraction.ElementoNoMapa);

        // Limpa o Holder após utilizá-lo
        MapCreator.instance.LimparHolder();

        // Agora copio as informações dos elementos da interação para
        // esses elementos holders
        elementoQuePassouEmCima.CopiarInformacoesDesseElementoPara(holderElementoQueMovimentouEmCimaHolder);
        iceQueSofreuInteraction.CopiarInformacoesDesseElementoPara(holderElementoQueSofreuInteraction);

        DefinirTipoDePasso(t);
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

}

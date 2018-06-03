using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoInteractionObjeto : UndoInteraction {


    // Ingame
    public UndoInteractionObjeto(ElementoDoMapa elementoQuePassouEmCima, ElementoDoMapa objetoQueSofreuInteraction, Passo.tiposDeInteraction t)
    {
        // temp
        contadorInteract++;
        
        // Crio os ElementosDoMapa que irão segurar as informações desses elementos da interação
        // (preciso fazer isso porque se caso os elementos da interação se percam no caminho,
        //  não irá dar nullExcpetion)
        holderElementoQueMovimentouEmCimaHolder = CriarComponenteDeAcordoComTipo(elementoQuePassouEmCima.ElementoNoMapa);
        holderElementoQueSofreuInteraction = CriarComponenteDeAcordoComTipo(objetoQueSofreuInteraction.ElementoNoMapa);

        // Limpa o Holder após utilizá-lo
        MapCreator.instance.LimparHolder();

        //Debug.Log("Criou holder elemento antes de Undo: " + holderElementoQueMovimentouEmCimaHolder.Elemento + " [" + holderElementoQueMovimentouEmCimaHolder.PosI + "][" + holderElementoQueMovimentouEmCimaHolder.PosJ + "] | TARGET: " + queMovimentou.Elemento);
        //Debug.Log("Criou holder elemento antes de Undo: " + holderElementoQueSofreuInteraction.Elemento + " [" + holderElementoQueSofreuInteraction.PosI + "][" + holderElementoQueSofreuInteraction.PosJ + "] | TARGET: " + elementoQueSofreuInteraction.Elemento);

        // Agora copio as informações dos elementos da interação para
        // esses elementos holders
        elementoQuePassouEmCima.CopiarInformacoesDesseElementoPara(holderElementoQueMovimentouEmCimaHolder);
        objetoQueSofreuInteraction.CopiarInformacoesDesseElementoPara(holderElementoQueSofreuInteraction);

        DefinirTipoDePasso(t);
    }
}

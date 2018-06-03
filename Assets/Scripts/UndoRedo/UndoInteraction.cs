using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoInteraction : Passo {

    // temporario
    public static int contadorInteract;
    
    ElementoDoMapa holderElementoQueSofreuInteraction;


    // Map editor
    public UndoInteraction(ElementoDoMapa elementoQueSofreuInteraction, Passo.tiposDeInteraction t)
    {
        // temp
        contadorInteract++;

        holderElementoQueSofreuInteraction = CriarComponenteDeAcordoComTipo(elementoQueSofreuInteraction.ElementoNoMapa);

        // Limpa o Holder após utilizá-lo
        MapCreator.instance.LimparHolder();
        
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

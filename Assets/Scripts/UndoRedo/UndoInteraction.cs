using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoInteraction : Passo {


    ElementoDoMapa elementoQueMovimentouEmCima;
    ElementoDoMapa elementoQueInteragiu;

    public UndoInteraction(ElementoDoMapa QueMovimentou, ElementoDoMapa elementoInteragiu)
    {
        DefinirTipoDePasso();
        ElementoQueMovimentouEmCima = QueMovimentou;
        ElementoQueInteragiu = elementoInteragiu;
    }

    public ElementoDoMapa ElementoQueInteragiu
    {
        get
        {
            return elementoQueInteragiu;
        }

        set
        {
            elementoQueInteragiu = value;
        }
    }

    public ElementoDoMapa ElementoQueMovimentouEmCima
    {
        get
        {
            return elementoQueMovimentouEmCima;
        }

        set
        {
            elementoQueMovimentouEmCima = value;
        }
    }

    protected override void DefinirTipoDePasso()
    {
        tipoDePasso = tiposDePasso.INTERACTION;
    }
}

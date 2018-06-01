using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoInteraction : Passo {

    // temporario
    public static int contadorInteract;

    // Preciso salvar os GameObjects porque
    // não tem como acessar o Componente
    // de um GameObject desativado.
    GameObject elementoQueMovimentouEmCimaGameObject;
    GameObject elementoQueInteragiuCimaGameObject;

    ElementoDoMapa elementoQueMovimentouEmCima;
    ElementoDoMapa elementoQueInteragiu;
    

    public UndoInteraction(ElementoDoMapa QueMovimentou, ElementoDoMapa elementoInteragiu)
    {
        // temp
        contadorInteract++;

        DefinirTipoDePasso();

        elementoQueMovimentouEmCimaGameObject = QueMovimentou.gameObject;
        elementoQueInteragiuCimaGameObject = elementoInteragiu.gameObject;

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

    public GameObject ElementoQueMovimentouEmCimaGameObject
    {
        get
        {
            return elementoQueMovimentouEmCimaGameObject;
        }

        set
        {
            elementoQueMovimentouEmCimaGameObject = value;
        }
    }

    public GameObject ElementoQueInteragiuCimaGameObject
    {
        get
        {
            return elementoQueInteragiuCimaGameObject;
        }

        set
        {
            elementoQueInteragiuCimaGameObject = value;
        }
    }

    protected override void DefinirTipoDePasso()
    {
        tipoDePasso = tiposDePasso.INTERACTION;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjetoDoMapa : ElementoDoMapa, IUndoInteraction<ElementoDoMapa, ElementoDoMapa, Passo.tiposDeInteraction> {

    private void Awake()
    {
        TipoDoElemento = MapCreator.tipoDeElemento.OBJETO;
    }

    public abstract void ExecutarObjeto(ElementoDoMapa quemEstaEmCima);

    public override void CopiarInformacoesDesseElementoPara(ElementoDoMapa target)
    {
        base.CopiarInformacoesDesseElementoPara(target);
    }

    public override void ResetarInformacoesDoElemento()
    {
        throw new System.NotImplementedException();
    }

    public abstract void CriarInteraction(ElementoDoMapa elementoQuePassouPorCima, ElementoDoMapa elementoQueSofreuInteraction, Passo.tiposDeInteraction tipo);

    public abstract void ExecutarUndoInteraction(ElementoDoMapa elementoQuePassouPorCima);
}

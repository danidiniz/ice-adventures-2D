using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCrateComPinguim4 : IceCrateComPinguim1
{
    void Start()
    {
        quantidadeDePinguins = 4;
        Tipo = MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_4;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        if (!base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce))
        {
            return false;
        }

        if (Tipo == oQueEstaEmCima)
            return false;

        return false;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    public override void Quebrar(SerVivo quemEstaQuebrando)
    {
        // Pode conter algo dentro?
    }
}

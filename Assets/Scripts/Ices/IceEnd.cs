using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEnd : IcesDefault
{
    // Criar evento pra quando player chegar no Ice do End?

    void Awake()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = true;

        ElementoNoMapa = MapCreator.elementosPossiveisNoMapa.END;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (ElementoNoMapa == oQueEstaEmCima)
            return false;

        return false;

        // Debug.Log("Player está em cima do " + name + " Classe end");
    }

    public override bool temAlgoEmCima()
    {
        return true;
    }
}

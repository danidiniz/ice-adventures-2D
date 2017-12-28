using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEnd : IcesDefault
{
    // Criar evento pra quando player chegar no Ice do End?

    void Start()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = true;

        Tipo = MapCreator.elementosPossiveisNoMapa.END;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Tipo == oQueEstaEmCima)
            return false;

        return false;

        // Debug.Log("Player está em cima do " + name + " Classe end");
    }
}

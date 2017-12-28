using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStart : IcesDefault
{
    void Start()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = true;

        Tipo = MapCreator.elementosPossiveisNoMapa.START;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Tipo == oQueEstaEmCima)
            return false;

        return true;

        //Debug.Log("Player está em cima do " + name + " Classe start");
    }
}

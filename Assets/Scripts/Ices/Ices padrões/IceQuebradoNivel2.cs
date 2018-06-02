using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoNivel2 : IceQuebradoNivel1
{
    void Awake()
    {
        //temp
        rand = UnityEngine.Random.Range(0, 100);

        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;
        ElementoNoMapa = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2;
        nivelDoIceQuebrado = 2;
    }
}

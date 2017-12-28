using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoNivel2 : IceQuebradoNivel1
{
    void Start()
    {
        Tipo = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2;
        nivelDoIceQuebrado = 2;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Tipo == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
            case MapCreator.elementosPossiveisNoMapa.PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.URSO_POLAR:
                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_3);
                break;

            case MapCreator.elementosPossiveisNoMapa.CRATE:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_1:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_2:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_3:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_4:
                //SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_3);
                // instanciar crate aqui?
                break;

            default:
                Debug.Log("Não encontrei o elemento " + oQueEstaEmCima + " no " + name);
                break;
        }

        return true;

    }
}

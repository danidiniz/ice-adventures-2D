using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoNivelFinalCrate : IcesDefault {

    // DESABILITAR OPÇÕES DE QUEBRAR ETC DA CRATE QUE ESTÁ EM CIMA DESSE ICE

    void Awake()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;

        ElementoNoMapa = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_FINAL_CRATE;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        //base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (ElementoNoMapa == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
                break;
            case MapCreator.elementosPossiveisNoMapa.PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.URSO_POLAR:
                // Somem do mapa
                break;

            case MapCreator.elementosPossiveisNoMapa.CRATE:
                break;
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_2:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_3:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_4:
                // Vira um IceQuebradoComCrate
                // Aparece mensagem de que vc matou afogado 'x' pinguins!!
                // :(
                // Malvadão
                break;

            default:
                Debug.Log("Não encontrei o elemento " + oQueEstaEmCima + " no " + name);
                break;
        }

        return true;

    }
}

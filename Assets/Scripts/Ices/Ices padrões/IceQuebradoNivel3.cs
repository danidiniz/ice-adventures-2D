using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoNivel3 : IceQuebradoNivel1
{
    void Start()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;

        Tipo = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_3;
        nivelDoIceQuebrado = 3;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        if (Tipo == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
            case MapCreator.elementosPossiveisNoMapa.PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.URSO_POLAR:
                Debug.Log(name + " quebrado passou do nível 3 para nível final");
                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_FINAL);
                // Game over
                break;

            case MapCreator.elementosPossiveisNoMapa.CRATE:
                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_COM_CRATE_EM_CIMA);
                break;
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_1:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_2:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_3:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_4:
                // // Vira um IceQuebradoComCrate
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

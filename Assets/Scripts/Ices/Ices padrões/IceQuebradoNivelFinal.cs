using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoNivelFinal : IceQuebradoComCrate
{
    void Start()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = true;

        Tipo = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_FINAL;
        nivelDoIceQuebrado = 4;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        //base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Tipo == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
                Debug.Log("Game over!");
                break;
            case MapCreator.elementosPossiveisNoMapa.PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.URSO_POLAR:
                // Somem do mapa
                break;

            case MapCreator.elementosPossiveisNoMapa.CRATE:
                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_COM_CRATE_EM_CIMA);
                IceQuebradoComCrate temp = MapCreator.map[PosI, PosJ].GetComponent(typeof(IceQuebradoComCrate)) as IceQuebradoComCrate;
                temp.atualizarNivelDoIce(4);
                break;
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_1:
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

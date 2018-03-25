using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoNivel2 : IceQuebradoNivel1
{
    void Awake()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;
        Elemento = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2;
        nivelDoIceQuebrado = 2;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        if (Elemento == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
            case MapCreator.elementosPossiveisNoMapa.PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.URSO_POLAR:
                Debug.Log(name + " quebrado passou do nível 2 para nível 3");
                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_3);
                break;

            case MapCreator.elementosPossiveisNoMapa.CRATE:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_2:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_3:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_4:
                // Gambiarra? Pensar pra ajeitar isso
                // problema: quando eu empurro a caixa pra cima do Ice 1, eu coloco ela em cima dele antes de transforma-lo,
                // então ao transformá-lo em Ice 2, a caixa 'some' 
                IceCrate temp = MapCreator.map[PosI, PosJ].elementoEmCimaDoIce.GetComponent(typeof(IceCrate)) as IceCrate;
                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_3);
                MapCreator.map[PosI, PosJ].elementoEmCimaDoIce = temp;
                break;

            default:
                Debug.Log("Não encontrei o elemento " + oQueEstaEmCima + " no " + name);
                break;
        }

        return true;

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoNivel1 : IcesDefault
{
    public byte nivelDoIceQuebrado;
    
    void Awake()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;

        Elemento = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_1;
        nivelDoIceQuebrado = 1;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Elemento == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
            case MapCreator.elementosPossiveisNoMapa.PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.URSO_POLAR:
                Debug.Log(name + " quebrado passou do nível 1 para nível 2");
                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2);
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
                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2);
                MapCreator.map[PosI, PosJ].elementoEmCimaDoIce = temp;

                /*
                tipoDeIceAntesDeSerTransformado = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2;

                // Salvando posição atual desse ice para pode acessar apos transformar
                short i = posI;
                short j = posJ;

                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_COM_CRATE_EM_CIMA);

                IceQuebradoComCrate temp2 = MapCreator.map[i, j].GetComponent(typeof(IceQuebradoComCrate)) as IceQuebradoComCrate;
                temp2.nivelDoIceQuebrado = 2;
                IceCrate tempCrate = elementoEmCimaDoIce.GetComponent(typeof(IceCrate)) as IceCrate;
                temp2.IsCrateEmpurravel = tempCrate.IsCrateEmpurravel;
                temp2.IsCratePulavel= tempCrate.IsCratePulavel;
                temp2.IsCrateQuebravel = tempCrate.IsCrateQuebravel;

                // instanciar crate aqui?

                //SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.CRATE);

                /*
                // Atualizando crate nessa posicao para que ela não seja mais empurrada
                IceCrate temp = MapCreator.map[i, j].GetComponent(typeof(IceCrate)) as IceCrate;
                if (temp != null)
                {
                    //temp.isCrateEmpurravel = false;
                }
                */

                break;

            default:
                Debug.Log("Não encontrei o elemento " + oQueEstaEmCima + " no " + name);
                break;
        }

        return true;

    }
}

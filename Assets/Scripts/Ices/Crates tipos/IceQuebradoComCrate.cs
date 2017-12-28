using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceQuebradoComCrate : IceCrate
{

    // Preciso ter informações sobre esse ice quebrado
    // e preciso das informações da crate

    public byte nivelDoIceQuebrado;

    public IceCrate crateEmCimaDoIceQuebrado;

    void Start()
    {
        isWalkable = false;
        pararMovimentoDeQuemPassarPorCima = true;

        Tipo = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_COM_CRATE_EM_CIMA;
        if(nivelDoIceQuebrado != 1 && nivelDoIceQuebrado != 2 && nivelDoIceQuebrado != 3)
        {
            Debug.Log("Nivel do ice não foi inicializado");
        }
        if(crateEmCimaDoIceQuebrado == null)
        {
            Debug.Log("Componente da crate em cima do ice NULL");
        }
    }

    public void InitIceQuebrado(short _i, short _j, byte nivel, IceCrate crate)
    {
        posI = _i;
        posJ = _j;
        nivelDoIceQuebrado = nivel;
        crateEmCimaDoIceQuebrado = crate;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Tipo == oQueEstaEmCima)
            return false;

        switch (oQueEstaEmCima)
        {
            case MapCreator.elementosPossiveisNoMapa.PLAYER:
                break;
            case MapCreator.elementosPossiveisNoMapa.PINGUIM:
            case MapCreator.elementosPossiveisNoMapa.URSO_POLAR:
                // Destruo a Crate

                // Transformo no nivel seguinte
                switch (nivelDoIceQuebrado)
                {
                    case 1:
                        SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2);
                        break;
                    case 2:
                        SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_3);
                        break;
                    case 3:
                        SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_FINAL);
                        break;
                }

                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2);
                break;

            case MapCreator.elementosPossiveisNoMapa.CRATE:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_1:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_2:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_3:
            case MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_4:
                // Salvando posição atual desse ice para pode acessar apos transformar
                short i = posI;
                short j = posJ;

                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_2);

                // instanciar crate aqui?

                SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.CRATE);

                // Atualizando crate nessa posicao para que ela não seja mais empurrada
                IceCrate temp = MapCreator.map[i, j].GetComponent(typeof(IceCrate)) as IceCrate;
                if (temp != null)
                {
                    //temp.isCrateEmpurravel = false;
                }
                
                break;

            default:
                Debug.Log("Não encontrei o elemento " + oQueEstaEmCima + " no " + name);
                break;
        }

        return false;

    }


}

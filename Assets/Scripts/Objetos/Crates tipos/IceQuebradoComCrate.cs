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
        //Tipo = MapCreator.elementosPossiveisNoMapa.ICE_QUEBRADO_COM_CRATE_EM_CIMA;
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

    public void atualizarNivelDoIce(byte nivel)
    {
        nivelDoIceQuebrado = nivel;
        if (nivelDoIceQuebrado >= 3)
        {
            isWalkable = true;
            pararMovimentoDeQuemPassarPorCima = false;
        }
        else
        {
            isWalkable = false;
            pararMovimentoDeQuemPassarPorCima = true;
        }
    }
    
}

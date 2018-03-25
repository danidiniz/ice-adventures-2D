using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStart : IcesDefault
{
    void Awake()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = true;

        Elemento = MapCreator.elementosPossiveisNoMapa.START;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Elemento == oQueEstaEmCima)
            return false;

        return true;

        //Debug.Log("Player está em cima do " + name + " Classe start");
    }

    public override void ColocarEmCimaDoIce(ObjetoDoMapa elemento)
    {
        elementoEmCimaDoIce = elemento;

        elemento.transform.position = transform.position;

        // Atualizo a posição do elemento
        elemento.setPosition(posI, posJ);

        AlgoPassouPorAqui(elemento.Elemento, elemento);

        //GameObject prefabDoElemento = MapCreator.instance.RetornarElemento(elemento);

        //PoolManager.instance.ColocarEmCima(prefabDoElemento, transform.position, transform.rotation, posI, posJ);

    }

    public override bool temAlgoEmCima()
    {
        return true;
    }
}

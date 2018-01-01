﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IceCrateComPinguim3 : IceCrateComPinguim1
{
    void Start()
    {
        quantidadeDePinguins = 3;
        Tipo = MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM_3;
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        if (!base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce))
        {
            return false;
        }

        if (Tipo == oQueEstaEmCima)
            return false;

        return false;

    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    }

    public override void Quebrar(SerVivo quemEstaQuebrando)
    {

        // Escolher direção que a foca vai
    }
}
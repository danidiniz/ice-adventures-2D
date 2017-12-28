using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ice : IcesDefault
{
    void Start()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;
        Tipo = MapCreator.elementosPossiveisNoMapa.ICE;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    }

    public override bool AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa oQueEstaEmCima, ElementoDoMapa elementoEmCimaDoIce)
    {
        base.AlgoPassouPorAqui(oQueEstaEmCima, elementoEmCimaDoIce);

        if (Tipo == elementoEmCimaDoIce.Tipo)
            return false;

        if (elementoEmCimaDoIce.Tipo == MapCreator.elementosPossiveisNoMapa.CRATE)
        {
            IceCrate componenteDaCrateQueFoiEmpurrada = elementoEmCimaDoIce.GetComponent(typeof(IceCrate)) as IceCrate;
            
            SerTransformadoEm(MapCreator.elementosPossiveisNoMapa.CRATE);

            IceCrate thisIceTransformado = MapCreator.map[posI, posJ].GetComponent(typeof(IceCrate)) as IceCrate;
            
            if (thisIceTransformado != null)
            {
                thisIceTransformado.QuantidadeDeVezesQueACratePodeSerEmpurrada -= 1;
                if (thisIceTransformado.QuantidadeDeVezesQueACratePodeSerEmpurrada <= 0)
                {
                    thisIceTransformado.QuantidadeDeVezesQueACratePodeSerEmpurrada = 0;
                    thisIceTransformado.IsCrateEmpurravel = false;
                }
            }
            
            return true;
        }

        return true;
    }
}

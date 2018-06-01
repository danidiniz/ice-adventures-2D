using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EspecialPeixeTest : ObjetoDoMapa
{

    private void Awake()
    {
        isWalkable = true;
        pararMovimentoDeQuemPassarPorCima = false;
    }

    public override void OnMouseDown()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (MapCreator.instance.modoCriarMapaAtivado)
        {
            if (MapCreator.instance.elementoSelecionado != MapCreator.elementosPossiveisNoMapa.CRATE)
            {
                MapCreator.map[posI, posJ].elementoEmCimaDoIce.gameObject.SetActive(false);
                MapCreator.map[posI, posJ].elementoEmCimaDoIce = null;
                MapCreator.map[posI, posJ].SerTransformadoEm(MapCreator.instance.elementoSelecionado);
                return;
            }

        }

    }

}

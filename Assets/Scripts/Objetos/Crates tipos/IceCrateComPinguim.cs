using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IceCrateComPinguim : ObjetoDoMapa, IQuebravel<SerVivo>, IEmpurravel<SerVivo>
{
    public byte quantidadeDePinguins;

    void Awake()
    {
        quantidadeDePinguins = 1;
        Elemento = MapCreator.elementosPossiveisNoMapa.CRATE_COM_PINGUIM;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    }

    public virtual void Quebrar(SerVivo quemEstaQuebrando)
    {

        // Escolher direção que a foca vai
    }

    public virtual void Empurrar(SerVivo quemEstaQuebrando)
    {

        // Escolher direção que a foca vai
    }
}

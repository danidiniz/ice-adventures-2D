using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIElementsOnClick : MonoBehaviour, IPointerDownHandler {

    [SerializeField]
    private MapCreator.elementosPossiveisNoMapa tipo;

    public void AlterarElementoSelecionado()
    {
        MapCreator.instance.elementoSelecionado = tipo;
        MapCreatorGUIManager.instance.objetoSelecionado.sprite = this.GetComponent<Image>().sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AlterarElementoSelecionado();
    }

}

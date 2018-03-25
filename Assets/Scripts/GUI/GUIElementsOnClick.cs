using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIElementsOnClick : MonoBehaviour, IPointerDownHandler {

    [SerializeField]
    private MapCreator.elementosPossiveisNoMapa elemento;
    [SerializeField]
    private MapCreator.tipoDeElemento tipo;

    public void AlterarElementoSelecionado()
    {
        MapCreator.instance.elementoSelecionado = elemento;
        MapCreator.instance.tipoDoElementoSelecionado = tipo;
        MapCreatorGUIManager.instance.objetoSelecionado.sprite = this.GetComponent<Image>().sprite;
        MapCreatorGUIManager.instance.objetoSelecionado.GetComponent<Image>().color = this.GetComponent<Image>().color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AlterarElementoSelecionado();
    }

}

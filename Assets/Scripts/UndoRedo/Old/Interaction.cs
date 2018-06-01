using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

    public enum tiposDeInteracao
    {
        QUEBRAR, EMPURRAR, PULAR, TELEPORTAR
    };
    public tiposDeInteracao tipoDaInteracao;
    public ObjetoDoMapa elemento;
    public short posI;
    public short posJ;

    public static Dictionary<int, Stack<Interaction>> interactions;

    public Interaction(tiposDeInteracao t, ObjetoDoMapa obj)
    {
        tipoDaInteracao = t;
        elemento = obj;
        posI = obj.PosI;
        posJ = obj.PosJ;
    }

    private void Awake()
    {
        interactions = new Dictionary<int, Stack<Interaction>>();
    }
}

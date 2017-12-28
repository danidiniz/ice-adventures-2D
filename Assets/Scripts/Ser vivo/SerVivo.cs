using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SerVivo : ElementoDoMapa {

    [SerializeField]
    float tempoParaAparecerNoMapa;

    [SerializeField]
    short posicaoParaAparecerNoMapaI;
    [SerializeField]
    short posicaoParaAparecerNoMapaJ;



    public string DizerPosicaoDebug()
    {
        return "[" + PosI + "][" + PosJ + "]";
    }

}

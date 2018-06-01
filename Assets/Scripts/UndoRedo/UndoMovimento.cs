using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoMovimento : Passo {

    ElementoDoMapa elementoQueMovimentou;
    public ElementoDoMapa iceOndeComecouMovimento;
    public ElementoDoMapa iceOndeTerminouMovimento;

    public UndoMovimento(ElementoDoMapa QueMovimentou, ElementoDoMapa ondeComecou, ElementoDoMapa ondeTerminou)
    {
        interactions = new List<UndoInteraction>();

        DefinirTipoDePasso();
        elementoQueMovimentou = QueMovimentou;
        iceOndeComecouMovimento = ondeComecou;
        iceOndeTerminouMovimento = ondeTerminou;
    }

    public void ExecutarUndo()
    {
        elementoQueMovimentou.transform.position = MapCreator.map[iceOndeComecouMovimento.PosI, iceOndeComecouMovimento.PosJ].transform.position;
        elementoQueMovimentou.PosI = iceOndeComecouMovimento.PosI;
        elementoQueMovimentou.PosJ = iceOndeComecouMovimento.PosJ;
        Debug.Log("Voltou para: [" + iceOndeComecouMovimento.PosI + "][" + iceOndeComecouMovimento.PosJ + "]");
        

        /*
        short dirI = (short)(iceOndeComecouMovimento.PosI - iceOndeTerminouMovimento.PosI);
        short dirJ = (short)(iceOndeComecouMovimento.PosJ - iceOndeTerminouMovimento.PosJ);
        Debug.Log("dirI: " + dirI + ", dirJ: " + dirJ);
        if (dirI < 0)
            dirI = -1;
        else if (dirI > 0)
            dirI = 1;

        if (dirJ < 0)
            dirJ = -1;
        else if (dirJ > 0)
            dirJ = 1;

        

        PlayerMovementAgrVai.instance.DirecaoDoMovimentoI = dirI;
        PlayerMovementAgrVai.instance.DirecaoDoMovimentoJ = dirJ;
        */
    }

    protected override void DefinirTipoDePasso()
    {
        tipoDePasso = tiposDePasso.MOVIMENTAR;
    }
}

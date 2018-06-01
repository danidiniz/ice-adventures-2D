using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoMovimento : Passo {

    // temp
    public int id;

    ElementoDoMapa elementoQueMovimentou;
    public ElementoDoMapa iceOndeComecouMovimento;
    public ElementoDoMapa iceOndeTerminouMovimento;

    public UndoMovimento(ElementoDoMapa QueMovimentou, ElementoDoMapa ondeComecou, ElementoDoMapa ondeTerminou)
    {
        //Ttemporario
        UndoInteraction.contadorInteract = 0;
        UndoRedo.contador++;
        id = UndoRedo.contador;
        Debug.Log("Novo UndoMovimento criado (" + id + ")\n" + QueMovimentou.name + " -> [" + ondeComecou.PosI + "][" + ondeComecou.PosJ + "] -> [" + ondeTerminou.PosI + "][" + ondeTerminou.PosJ + "]");

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

        // Atualizo o lastPlayerPos
        UndoMovimento temp = UndoRedo.steps.Peek() as UndoMovimento;
        if(temp == null)
        {
            Debug.Log("Não é um UndoMovimento.");
        }
        else
        {
            PlayerMovementAgrVai.instance.playerLastPosI = temp.iceOndeComecouMovimento.PosI;
            PlayerMovementAgrVai.instance.playerLastPosJ = temp.iceOndeComecouMovimento.PosJ;
        }
        
        
        //Debug.Log("Voltou para: [" + iceOndeComecouMovimento.PosI + "][" + iceOndeComecouMovimento.PosJ + "]");
        

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

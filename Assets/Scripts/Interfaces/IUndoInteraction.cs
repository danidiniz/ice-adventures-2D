﻿internal interface IUndoInteraction<T1, T2, T3>
{
    void CriarInteraction(T1 elementoQuePassouPorCima, T2 elementoQueSofreuInteraction, T3 tipoDaInteraction);
    void CriarInteraction(T1 elementoQueSofreuInteraction);

    void ExecutarUndoInteraction(T1 elementoQueCausouInteraction);
}
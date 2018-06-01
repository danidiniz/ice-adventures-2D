internal interface IUndoInteraction<T1, T2>
{
    void CriarInteraction(T1 elementoQuePassouPorCima);

    // Preciso saber o elementoQuePassouPorCima porque caso ele tenha sido afetado
    // de alguma forma, eu faço Undo nele também.
    void ExecutarUndoInteraction(T1 elementoQuePassouPorCima, T2 elementoQueInteragiu);
}
internal interface IUndoInteraction<T1>
{
    void CriarInteraction(T1 elementoQuePassouPorCima);
    void ExecutarUndoInteraction(T1 elementoQuePassouPorCima);
}
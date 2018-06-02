using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementAgrVai : MonoBehaviour
{
    public Text debugTemporario;

    public static bool playerEmMovimento; // boolean que será utilizada pra criar os steps
    public bool podePegarInputDoPlayer;
    public bool pegouInputDoPlayer;
    public bool pararMovimento;

    // Variaveis do Undo
    public bool undoJaCriado;
    public bool finalizouMovimento; // boolean para setar posicao em que player parou
    public short playerLastPosI;
    public short playerLastPosJ;


    [SerializeField]
    short direcaoDoMovimentoI;
    [SerializeField]
    short direcaoDoMovimentoJ;

    public float movementSpeed;

    public enum objectPossiveisDirections { SEM_MOVIMENTO, INDO_PARA_CIMA, INDO_PARA_DIREITA, INDO_PARA_BAIXO, INDO_PARA_ESQUERDA };
    [SerializeField]
    private objectPossiveisDirections objectCurrentDirection;

    public Vector2 startPosTouch;
    public Vector2 direction;
    public bool directionChosen;

    protected SerVivo serVivoInfoComponente;

    public bool jaExecutouAlgoPassouPorAquiDoIce;

    #region Getters and Setters
    public objectPossiveisDirections ObjectCurrentDirection
    {
        get
        {
            return objectCurrentDirection;
        }

        set
        {
            objectCurrentDirection = value;
        }
    }

    public short DirecaoDoMovimentoI
    {
        get
        {
            return direcaoDoMovimentoI;
        }

        set
        {
            direcaoDoMovimentoI = value;
        }
    }

    public short DirecaoDoMovimentoJ
    {
        get
        {
            return direcaoDoMovimentoJ;
        }

        set
        {
            direcaoDoMovimentoJ = value;
        }
    }
    #endregion

    static PlayerMovementAgrVai _instance;

    public static PlayerMovementAgrVai instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerMovementAgrVai>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        serVivoInfoComponente = GetComponent(typeof(SerVivo)) as SerVivo;

        playerEmMovimento = false;
        podePegarInputDoPlayer = true;
        pegouInputDoPlayer = false;
        pararMovimento = false;

        undoJaCriado = false;
        finalizouMovimento = true;
        playerLastPosI = serVivoInfoComponente.PosI;
        playerLastPosJ = serVivoInfoComponente.PosJ;

        ObjectCurrentDirection = objectPossiveisDirections.SEM_MOVIMENTO;

        transform.position = MapCreator.map[0, 0].gameObject.transform.position;
        
        DirecaoDoMovimentoI = 0;
        DirecaoDoMovimentoJ = 0;

        jaExecutouAlgoPassouPorAquiDoIce = false;
    }

    private void Update()
    {
        if (serVivoInfoComponente == null)
            return;

        // deletar
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (UndoRedo.steps.Peek().tipoDaInteractionQueAconteceu == Passo.tiposDeInteraction.MOVIMENTAR)
            {
                
                /*
                Debug.Log("Pilha:");
                UndoMovimento temp = UndoRedo.steps.Pop() as UndoMovimento;
                Debug.Log("Player last pos: " + playerLastPosI + ", " + playerLastPosJ);
                Debug.Log("Ice onde começou: [" + temp.iceOndeComecouMovimento.PosI + "][" + temp.iceOndeComecouMovimento.PosJ + "]");
                Debug.Log("Ice onde terminou: [" + temp.iceOndeTerminouMovimento.PosI + "][" + temp.iceOndeTerminouMovimento.PosJ + "]");
            */    

                
                Debug.Log("Objetos que interagiram durante movimento:");
                for (int i = 0; i < UndoRedo.steps.Peek().interactions.Count; i++)
                {
                    Debug.Log(UndoRedo.steps.Peek().interactions[i].HolderElementoQueSofreuInteraction.Elemento);
                }
                Debug.Log("Objetos que interagiram com player parado:");
                for (int i = 0; i < UndoRedo.interactionsTemp.Count; i++)
                {
                    Debug.Log(UndoRedo.interactionsTemp[i].HolderElementoQueSofreuInteraction.Elemento);
                }

            }
        }


        playerEmMovimento = !podePegarInputDoPlayer;

        // Importante criar o Undo antes do método de Movimento
        // porque dessa forma eu crio o Undo de Movimento ANTES
        // de um Undo de teleportar, por exemplo.
        if (!playerEmMovimento) // Só entra aqui quando o player terminar de movimentar
        {
            if (!undoJaCriado)
            {
                undoJaCriado = true;

                //Debug.Log("Começou: " + playerLastPosI + ", " + playerLastPosJ + "\nTerminou: " + serVivoInfoComponente.PosI + ", " + serVivoInfoComponente.PosJ);

                // Preciso refazer isso de uma forma genérica igual fiz o UndoInteractions,
                // pois o UndoMovimento será usado por qualquer elemento que se movimente (urso, pinguim, etc)

                UndoRedo.steps.Push(new UndoMovimento(
                    serVivoInfoComponente,
                    MapCreator.map[playerLastPosI, playerLastPosJ],
                    MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ]
                    ));
                playerLastPosI = serVivoInfoComponente.PosI;
                playerLastPosJ = serVivoInfoComponente.PosJ;

                // Coloco as interactions que ocorreram na lista de interactions desse Step
                if(UndoRedo.interactionsTemp.Count > 0)
                {
                    UndoRedo.steps.Peek().interactions = new List<UndoInteraction>(UndoRedo.interactionsTemp);              
                    UndoRedo.interactionsTemp.Clear();
                   
                }
            }
        }
        if (playerEmMovimento)
        {
            undoJaCriado = false;
        }

        if (DirecaoDoMovimentoI == 0 && DirecaoDoMovimentoJ == 0)
        {
            playerEmMovimento = false;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            MovimentarPlayerNoMapa();

            if (podePegarInputDoPlayer)
            {
                ZerarDirecaoMovimentos();
                PegarInputDoPlayer();
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            MovimentarPlayerNoMapa();

            if (!podePegarInputDoPlayer)
                return;

            // Track a single touch as a direction control.
            if ((Input.touchCount > 0))
            {
                Touch touch = Input.GetTouch(0);

                // Handle finger movements based on touch phase.
                switch (touch.phase)
                {
                    // Record initial touch position.
                    case TouchPhase.Began:
                        debugTemporario.text = "Touch Began";
                        startPosTouch = touch.position;
                        directionChosen = false;
                        break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                        debugTemporario.text = "Touch Moved";
                        direction = touch.position - startPosTouch;
                        break;

                    // Report that a direction has been chosen when the finger is lifted.
                    case TouchPhase.Ended:
                        debugTemporario.text = "Touch Ended";
                        directionChosen = true;
                        break;
                }
            }
            if (directionChosen)
            {
                // Something that uses the chosen direction...
                directionChosen = false;
                ZerarDirecaoMovimentos();
                PegarInputSwipe();
                direction = Vector3.zero;
                debugTemporario.text = "Pegou swipe | dirI " + DirecaoDoMovimentoI + " dirJ " + DirecaoDoMovimentoJ;
            }
        }
    }

    // Esse método serve tanto para Inputs Keyboard
    // quanto para Inputs Touch
    private void MovimentarPlayerNoMapa()
    {
        // Se o player já estiver na posição correta, 
        // posso executar o AlgoPassouAqui do ice que ele está
        if (transform.position == MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ].gameObject.transform.position)
        {
            // Se ainda não tiver executado o AlgoPassouPorAqui do ice que está em cima
            if (!jaExecutouAlgoPassouPorAquiDoIce)
            {
                MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ].AlgoPassouPorAqui(MapCreator.elementosPossiveisNoMapa.PLAYER, serVivoInfoComponente);
                jaExecutouAlgoPassouPorAquiDoIce = true;
            }
            // Se já pegou o input do player,
            // significa que já PODE ter uma direcaoMovimentoI e direcaoMovimentoJ
            if (pegouInputDoPlayer)
            {
                // Se já possuir uma direção
                if (DirecaoDoMovimentoI != 0 || DirecaoDoMovimentoJ != 0)
                {
                    // Verifico se posso continuar movimentando
                    bool podeContinuarMovendo = VerificarSePossoMovimentarParaProximoIce(DirecaoDoMovimentoI, DirecaoDoMovimentoJ);
                    if (podeContinuarMovendo)
                    {
                        // Se puder, atualizo a posição do player
                        serVivoInfoComponente.PosI += DirecaoDoMovimentoI;
                        serVivoInfoComponente.PosJ += DirecaoDoMovimentoJ;
                        // Não permito o input do player pois ele está movendo
                        podePegarInputDoPlayer = false;
                        // Atualizo pararMovimento
                        pararMovimento = MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ].pararMovimentoDeQuemPassarPorCima;
                        // Se o ice que o player está indo obrigá-lo a parar
                        // eu zero as direções para ele não movimentar quando chegar lá
                        if (pararMovimento)
                        {
                            ZerarDirecaoMovimentos();
                        }
                        // Gambiarra de mierda
                        transform.position = Vector3.MoveTowards(transform.position, MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ].transform.position, movementSpeed * Time.deltaTime);
                    }
                    // Se não puder mais mover, permito o player input
                    else
                    {
                        podePegarInputDoPlayer = true;
                    }
                }
                // Se não possuir direção, permito o player input
                else
                {
                    podePegarInputDoPlayer = true;
                }
            }
        }
        // Se não, movimento o player até lá
        else
        {
            jaExecutouAlgoPassouPorAquiDoIce = false;
            podePegarInputDoPlayer = false;
            transform.position = Vector3.MoveTowards(transform.position, MapCreator.map[serVivoInfoComponente.PosI, serVivoInfoComponente.PosJ].gameObject.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    private void PegarInputDoPlayer()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_CIMA;
            DirecaoDoMovimentoI = -1;
            DirecaoDoMovimentoJ = 0;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_DIREITA;
            DirecaoDoMovimentoI = 0;
            DirecaoDoMovimentoJ = 1;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_BAIXO;
            DirecaoDoMovimentoI = 1;
            DirecaoDoMovimentoJ = 0;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_ESQUERDA;
            DirecaoDoMovimentoI = 0;
            DirecaoDoMovimentoJ = -1;
            pegouInputDoPlayer = true;
        }
    }

    private bool VerificarSePossoMovimentarParaProximoIce(short direcaoI, short direcaoJ)
    {
        if (MapCreator.instance.VerificarSeEstaDentroDoMapa((short)(serVivoInfoComponente.PosI + direcaoI), (short)(serVivoInfoComponente.PosJ + direcaoJ)))
        {

            // Temporario
            if (MapCreator.map[serVivoInfoComponente.PosI + direcaoI, serVivoInfoComponente.PosJ + direcaoJ].elementoEmCimaDoIce != null)
            {
                if (MapCreator.map[serVivoInfoComponente.PosI + direcaoI, serVivoInfoComponente.PosJ + direcaoJ].elementoEmCimaDoIce.Elemento == MapCreator.elementosPossiveisNoMapa.CRATE)
                {
                    MapCreator.map[serVivoInfoComponente.PosI + direcaoI, serVivoInfoComponente.PosJ + direcaoJ].elementoEmCimaDoIce.GetComponent<IceCrate>().Quebrar(GetComponent<SerVivo>());
                    return true;
                }
            }

            if (MapCreator.map[serVivoInfoComponente.PosI + direcaoI, serVivoInfoComponente.PosJ + direcaoJ].isWalkable)
            {
                if(MapCreator.map[serVivoInfoComponente.PosI + direcaoI, serVivoInfoComponente.PosJ + direcaoJ].elementoEmCimaDoIce == null)
                {
                    return true;
                }
                else
                {
                    if(MapCreator.map[serVivoInfoComponente.PosI + direcaoI, serVivoInfoComponente.PosJ + direcaoJ].elementoEmCimaDoIce.isWalkable)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return false;
    }

    private void PegarInputSwipe()
    {
        float x = direction.x;
        float y = direction.y;

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (y > 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_CIMA;
                DirecaoDoMovimentoI = -1;
                DirecaoDoMovimentoJ = 0;
                pegouInputDoPlayer = true;
            }
            else if (y < 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_BAIXO;
                DirecaoDoMovimentoI = 1;
                DirecaoDoMovimentoJ = 0;
                pegouInputDoPlayer = true;
            }
        }
        else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (x > 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_DIREITA;
                DirecaoDoMovimentoI = 0;
                DirecaoDoMovimentoJ = 1;
                pegouInputDoPlayer = true;
            }
            else if (x < 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_ESQUERDA;
                DirecaoDoMovimentoI = 0;
                DirecaoDoMovimentoJ = -1;
                pegouInputDoPlayer = true;
            }
        }
    }

    // Método específico pra zerar o direcaoDoMovimentoI e direcaoDoMovimentoJ
    // Dessa forma posso parar o movimento do player de outras classes
    // Por exemplo: ao pular uma crate eu zero as direções pois não quero que ele se mova,
    // apenas execute o AlgoPassouPorAqui do ice que ele acabou de pular
    public void ZerarDirecaoMovimentos()
    {
        DirecaoDoMovimentoI = 0;
        DirecaoDoMovimentoJ = 0;
    }

}

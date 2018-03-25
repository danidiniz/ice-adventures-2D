using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementAgrVai : MonoBehaviour
{
    public Text debugTemporario;


    public bool podePegarInputDoPlayer;
    public bool pegouInputDoPlayer;
    public bool pararMovimento;

    [SerializeField]
    short direcaoDoMovimentoI;
    [SerializeField]
    short direcaoDoMovimentoJ;

    public float movementSpeed;

    public enum objectPossiveisDirections { SEM_MOVIMENTO, INDO_PARA_CIMA, INDO_PARA_DIREITA, INDO_PARA_BAIXO, INDO_PARA_ESQUERDA };
    [SerializeField]
    private objectPossiveisDirections objectCurrentDirection;

    public Vector2 startPos;
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
        podePegarInputDoPlayer = true;
        pegouInputDoPlayer = false;
        pararMovimento = false;

        ObjectCurrentDirection = objectPossiveisDirections.SEM_MOVIMENTO;

        transform.position = MapCreator.map[0, 0].gameObject.transform.position;

        serVivoInfoComponente = GetComponent(typeof(SerVivo)) as SerVivo;

        direcaoDoMovimentoI = 0;
        direcaoDoMovimentoJ = 0;

        jaExecutouAlgoPassouPorAquiDoIce = false;
    }

    private void Update()
    {
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
                        startPos = touch.position;
                        directionChosen = false;
                        break;

                    // Determine direction by comparing the current touch position with the initial one.
                    case TouchPhase.Moved:
                        debugTemporario.text = "Touch Moved";
                        direction = touch.position - startPos;
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
                debugTemporario.text = "Pegou swipe | dirI " + direcaoDoMovimentoI + " dirJ " + direcaoDoMovimentoJ;
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
                if (direcaoDoMovimentoI != 0 || direcaoDoMovimentoJ != 0)
                {
                    // Verifico se posso continuar movimentando
                    bool podeContinuarMovendo = VerificarSePossoMovimentarParaProximoIce(direcaoDoMovimentoI, direcaoDoMovimentoJ);
                    if (podeContinuarMovendo)
                    {
                        // Se puder, atualizo a posição do player
                        serVivoInfoComponente.PosI += direcaoDoMovimentoI;
                        serVivoInfoComponente.PosJ += direcaoDoMovimentoJ;
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
            direcaoDoMovimentoI = -1;
            direcaoDoMovimentoJ = 0;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_DIREITA;
            direcaoDoMovimentoI = 0;
            direcaoDoMovimentoJ = 1;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_BAIXO;
            direcaoDoMovimentoI = 1;
            direcaoDoMovimentoJ = 0;
            pegouInputDoPlayer = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_ESQUERDA;
            direcaoDoMovimentoI = 0;
            direcaoDoMovimentoJ = -1;
            pegouInputDoPlayer = true;
        }
    }

    private bool VerificarSePossoMovimentarParaProximoIce(short direcaoI, short direcaoJ)
    {
        if (MapCreator.instance.VerificarSeEstaDentroDoMapa((short)(serVivoInfoComponente.PosI + direcaoI), (short)(serVivoInfoComponente.PosJ + direcaoJ)))
        {
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
                direcaoDoMovimentoI = -1;
                direcaoDoMovimentoJ = 0;
                pegouInputDoPlayer = true;
            }
            else if (y < 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_BAIXO;
                direcaoDoMovimentoI = 1;
                direcaoDoMovimentoJ = 0;
                pegouInputDoPlayer = true;
            }
        }
        else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (x > 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_DIREITA;
                direcaoDoMovimentoI = 0;
                direcaoDoMovimentoJ = 1;
                pegouInputDoPlayer = true;
            }
            else if (x < 0)
            {
                ObjectCurrentDirection = objectPossiveisDirections.INDO_PARA_ESQUERDA;
                direcaoDoMovimentoI = 0;
                direcaoDoMovimentoJ = -1;
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
        direcaoDoMovimentoI = 0;
        direcaoDoMovimentoJ = 0;
    }

}

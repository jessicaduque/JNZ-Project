using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    private Rigidbody Corpo;
    private Animator Anim;
    private GameObject CorpoMonge;

    [SerializeField]
    float sensibilidadeGiro = 500f;
    [SerializeField]
    float velocidadeAndar = 4;
    private float velocidadeFinal = 0;
    [SerializeField]
    bool esperandoSegundos = false;
    public GameObject pezinho;
    private bool recebeuInputMover;
    public bool estaNoChao = true;
    bool movimentoPermitido = true;
    bool touroDomado = false;

    [SerializeField]
    private Vector3 posInicial;

    float tempo = 0.0f;
    float segundosParaEsperar;

    // Estado da fase
    enum EstadoFase { SemPuzzleSemDom = 0, PuzzlePedrasSemDom = 1, PuzzlePedrasComDom = 2, PuzzleRuinas = 3, SemPuzzleComDom = 4};
    private bool pisandoEmRuinas = false;


    // Sistema checkpoints
    Vector3[] infoCheckpoint = new Vector3[2];
    GameObject[] PedrasParaReset;
    int raizesAtivadosCheckpoint;

    // Puzzle das pedras
    bool empurrandoPedra = false;
    Transform Pedra;
    Vector3 PedraPosInicial;
    public Vector3 frentePedra;


    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        Corpo = GetComponent<Rigidbody>();
        transform.position = posInicial;
        CorpoMonge = this.gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        // Se necessário esperar segundos sem movimento nenhum, o controle está aqui
        if (esperandoSegundos)
        {
            EsperarSegundos(segundosParaEsperar);
        }
        else
        {
            tempo = 0.0f;
        }

        // O player só pode se mover se não estiver no meio de empurrar uma pedra
        ControleMovimento();

        // Puzzle das pedras
        ResetarPuzzlePedras();
        EmpurrarPedra();
    }

    private void FixedUpdate()
    {
        if (recebeuInputMover && movimentoPermitido && !esperandoSegundos && !empurrandoPedra)
        {
            Mover();
        }
    }

    void ControleMovimento()
    {
        if (movimentoPermitido)
        {
            if (!esperandoSegundos)
            {
                if (!empurrandoPedra)
                {
                    ReceberInputs();
                    Girar();
                    Corpo.constraints = RigidbodyConstraints.FreezeRotation;
                    AnimacaoAndar();
                    VirarPersonagemMovimento();
                }


            }
        }
    }
    void ReceberInputs()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            recebeuInputMover = true;
        }
        else
        {
            recebeuInputMover = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Run") > 0)
        {
            velocidadeFinal = velocidadeAndar * 1.7f;
        }
        else
        {
            velocidadeFinal = velocidadeAndar;
        }

    }
    void VirarPersonagemMovimento()
    {
        if (movimentoPermitido)
        {
            if (Input.GetAxis("Vertical") < 0 && (Input.GetAxis("Horizontal") < 0.6 && Input.GetAxis("Horizontal") > -0.6))
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, -transform.forward, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && (Input.GetAxis("Horizontal") < 0.6 && Input.GetAxis("Horizontal") > -0.6))
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, transform.forward, 10f);
            }
            else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") > 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") < 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, -transform.right, 10f);
            }
            else if(Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, -transform.forward + transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, -transform.forward + -transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, transform.forward + transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, transform.forward + -transform.right, 10f);
            }
            else
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge.gameObject, this.transform.forward, 10f);
            }

        }

    }

    void AnimacaoAndar()
    {
        if (movimentoPermitido)
        {
            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Input.GetAxis("Run") == 0)
            {
                Anim.SetBool("Correndo", false);
                Anim.SetBool("Andando", true);
                
            }
            else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Input.GetAxis("Run") > 0)
            {
                Anim.SetBool("Andando", false);
                Anim.SetBool("Correndo", true);
            }
            else
            {
                Anim.SetBool("Correndo", false);
                Anim.SetBool("Andando", false);
            }

        }
    }

    void Mover()
    {
        float velocidadeZ;
        float velocidadeX;

        velocidadeZ = Input.GetAxis("Vertical") * velocidadeFinal;
        velocidadeX = Input.GetAxis("Horizontal") * velocidadeFinal;

        //velocidadeX = 0;
        Vector3 velocidadeCorrigida = velocidadeX * transform.right + velocidadeZ * transform.forward;

        Corpo.velocity = new Vector3(velocidadeCorrigida.x, Corpo.velocity.y, velocidadeCorrigida.z);
    }

    void Girar()
    {
        float GiroY = Input.GetAxis("Mouse X") * sensibilidadeGiro * Time.deltaTime;
        transform.Rotate(Vector3.up * GiroY);
    }

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Checkpoint")
        {
            if (colidiu.gameObject.GetComponent<Checkpoint>().UltimoCheck)
            {
                infoCheckpoint[0] = new Vector3(0, 0, 0);
                infoCheckpoint[1] = new Vector3(0, 0, 0);
                Destroy(colidiu.gameObject);

            }
            else
            {
                infoCheckpoint[0] = new Vector3(colidiu.gameObject.transform.position.x, transform.position.y, colidiu.gameObject.transform.position.z);
                infoCheckpoint[1] = colidiu.gameObject.transform.eulerAngles;
                PedrasParaReset = colidiu.gameObject.GetComponent<Checkpoint>().pedrasParaResetar;
                raizesAtivadosCheckpoint = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetRaizesAtivados();
                Destroy(colidiu.gameObject);
            }
        }

        if(colidiu.gameObject.tag == "PuzzleRuinas")
        {
            pisandoEmRuinas = true;
            estaNoChao = true;
        }
    }

    private void OnTriggerExit(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "PuzzleRuinas")
        {
            pisandoEmRuinas = false;
        }
    }

    void RotacionarEmDirecaoAAlgo(GameObject obj, Vector3 ondeOlhar, float velocidadeGiro)
    {
        Vector3 newDirection = Vector3.RotateTowards(obj.transform.forward, ondeOlhar, velocidadeGiro * Time.deltaTime, 0.0f);
        obj.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public bool SeTouroEstaDomado()
    {
        return touroDomado;
    }

    void EmpurrarPedra()
    {
        /*
        if (!empurrandoPedra)
        {
            Pedra = ChecarSePertoDePedra();
            if (Pedra != null && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)) && ChecarSePodeMoverPedra(Pedra) && EncontrarFrentePedra(Pedra) != new Vector3(0, 0, 0))
            {
                frentePedra = EncontrarFrentePedra(Pedra);
                Pedra.GetComponent<Pedra>().MudarEstadoMovimento();
                PedraPosInicial = Pedra.transform.position;
                empurrandoPedra = true;

            }
        }
        else
        {
            float velocidadeGiroParaPedra = 1f;
            float rapidezEmpurrar = 3f;

            RotacionarEmDirecaoAAlgo(frentePedra, velocidadeGiroParaPedra);

            AnimacaoEmpurrarPedra();

            transform.position += rapidezEmpurrar * frentePedra * Time.deltaTime;
            if (Mathf.Abs((Pedra.transform.position - PedraPosInicial).magnitude) > 8f)
            {
                Pedra.transform.position = new Vector3(Mathf.Round(Pedra.transform.position.x), Pedra.transform.position.y, Mathf.Round(Pedra.transform.position.z));
                Pedra.GetComponent<Pedra>().MudarEstadoMovimento();
                empurrandoPedra = false;
            }
        }
        */


        // SUGESTÃO DO FABIANO
        
        if (!empurrandoPedra)
        {
            Pedra = ChecarSePertoDePedra();
            if (Pedra != null && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)) && ChecarSePodeMoverPedra(Pedra) && EncontrarFrentePedra(Pedra) != new Vector3(0, 0, 0))
            {
                frentePedra = EncontrarFrentePedra(Pedra);
                //Pedra.GetComponent<Pedra>().MudarEstadoMovimento();
                PedraPosInicial = Pedra.transform.position;
                empurrandoPedra = true;
                
            }
        }
        else
        {
            float velocidadeGiroParaPedra = 1f;
            //float rapidezEmpurrar = 5f;

            RotacionarEmDirecaoAAlgo(this.gameObject, frentePedra, velocidadeGiroParaPedra);
            AnimacaoEmpurrarPedra();
            PrenderPersonagem();
            Pedra.GetComponent<Rigidbody>().mass = 1;

            // Para prender o movimento da pedra nos eixos não desejado

            if (frentePedra.x == 0)
            {
                Debug.Log("entrou");
                Pedra.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                Corpo.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; 
            }
            else
            {
                Pedra.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                Corpo.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            Corpo.velocity = velocidadeAndar * 0.7f * frentePedra;

            Debug.Log(PedraPosInicial);
            Debug.Log(transform.position);
            Debug.Log(PedraPosInicial - new Vector3(0, 0, (8 * PedraPosInicial.z)));

            if (Pedra.transform.position == PedraPosInicial + new Vector3(0, 0, (8 * PedraPosInicial.z)) || Pedra.transform.position == PedraPosInicial - new Vector3(0, 0, (8 * PedraPosInicial.z)) || Pedra.transform.position == PedraPosInicial + new Vector3((8 * PedraPosInicial.x), 0, 0) || Pedra.transform.position == PedraPosInicial - new Vector3((8 * PedraPosInicial.x), 0, 0))
            {
                Debug.Log("acabou");
                Corpo.velocity = new Vector3(0, 0, 0);
                Pedra.GetComponent<Rigidbody>().mass = 1000000f;
                movimentoPermitido = true;
                empurrandoPedra = false;
            }
        }
    }
    void AnimacaoEmpurrarPedra()
    {
        if (touroDomado)
        {
            // Sequência de animações com touro
        }
        else
        {
            // Sequência de animações sem touro
        }
    }

    Transform ChecarSePertoDePedra()
    {
        GameObject[] PedrasLeves;
        PedrasLeves = GameObject.FindGameObjectsWithTag("PedraLeve");

        float minimumDistance = 6f;

        Transform PedraMaisPerto = null;

        foreach (GameObject pedra in PedrasLeves)
        {
            float distance = Vector3.Distance(transform.position, pedra.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                PedraMaisPerto = pedra.transform;
            }
        }

        if (PedraMaisPerto != null)
        {
            return PedraMaisPerto;
        }
        else
        {
            return null;
        }
    }

    bool ChecarSePodeMoverPedra(Transform Pedra)
    {
        Vector3 direction = transform.position - Pedra.position;

        // Verificação que pode mover a pedra se for pesada
        if (Pedra.gameObject.tag == "PedraPesada")
        {
            if (!touroDomado)
            {
                return false;
            }
        }

        // Verificação de que a rotação do player está dentro do escopo mínimo esperado para poder empurrar a pedra
        if(Vector3.Angle(transform.forward, -direction) > 45)
        {
            return false;
        }

        // Verificação de se há pedras para onde será empurrado
        RaycastHit meuRay;
        if (Physics.Raycast(Pedra.position, -direction, out meuRay, 8f))
        {
            string colisor = meuRay.collider.gameObject.tag;
            if (colisor != "Parede" && colisor != "PedraLeve" && colisor != "PedraPesada" && colisor != "Raiz1" && colisor != "Raiz2")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }

    }

    Vector3 EncontrarFrentePedra(Transform Pedra)
    {
        Vector3 direction = transform.position - Pedra.position;

        // No eixo Z
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z) && direction.z > -1.7f && direction.z < 1.7f)
        {
            if (direction.x > 0)
            {
                return new Vector3(-1, 0, 0);
            }
            else
            {
                return new Vector3(1, 0, 0);
            }
        }
        // No eixo X
        else if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x) && direction.x > -1.7f && direction.x < 1.7f)
        {
            if (direction.z > 0)
            {
                return new Vector3(0, 0, -1);
            }
            else
            {
                return new Vector3(0, 0, 1);
            }
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    void ResetarPuzzlePedras()
    {
        if(!esperandoSegundos && !empurrandoPedra && movimentoPermitido && (PegarEstadoFase() == (int)EstadoFase.PuzzlePedrasSemDom || PegarEstadoFase() == (int)EstadoFase.PuzzlePedrasComDom))
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                if(infoCheckpoint[0] != new Vector3(0f, 0f, 0f))
                {
                    transform.position = infoCheckpoint[0];
                    transform.eulerAngles = infoCheckpoint[1];
                    segundosParaEsperar = 1.2f;
                    Corpo.velocity = new Vector3(0f, 0f, 0f);

                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().AtivacaoRaizes(raizesAtivadosCheckpoint);

                    for (int i=0; i < PedrasParaReset.Length; i++)
                    {
                        PedrasParaReset[i].transform.position = PedrasParaReset[i].GetComponent<Pedra>().PosicaoInicial;
                    }

                    esperandoSegundos = true;
                }
            }
        }
    }

    int PegarEstadoFase()
    {
        return GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetEstadoFase();
    }

    public bool EsperarSegundos(float segundos)
    {
        tempo += Time.deltaTime;
        Debug.Log(tempo);

        if(tempo > segundos)
        {
            esperandoSegundos = false;
            return true;
        }
        else
        {
            esperandoSegundos = true;
            return false;
        }
    }

    public bool PisandoEmRuinas()
    {
        return pisandoEmRuinas;
    }
    public void PrenderPersonagem()
    {
        movimentoPermitido = false;
        Corpo.velocity = new Vector3(0, 0, 0);
    }
    public void DesprenderPersonagem()
    {
        movimentoPermitido = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Personagem : MonoBehaviour
{
    public LayerMask semRC;

    private Rigidbody Corpo;
    public Animator Anim;
    [SerializeField]
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
    private GameObject MaoColisor;

    [SerializeField]
    private Vector3 posInicial;

    float tempo = 0.0f;
    float segundosParaEsperar;

    Vector3 teleporteCavernaFora;
    Vector3 teleporteForaCaverna;

    [SerializeField]
    private GameObject TelaPretaPanel;

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

    [SerializeField]
    private Image BotaoInteracao;

    bool semPedra = true;
    bool longeCanalizador = true;


    void Start()
    {
        //Anim = GetComponentInChildren<Animator>();
        Corpo = GetComponent<Rigidbody>();
        transform.position = posInicial;
        //CorpoMonge = this.gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {

        Anim = CorpoMonge.GetComponent<Animator>();
        CorpoMonge.transform.position = transform.position;

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
        ControleBotaoInteracao();
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
            velocidadeFinal = velocidadeAndar * 1.5f;
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
            else if(Input.GetAxis("Mouse X") == 0 && this.gameObject.transform.forward != CorpoMonge.transform.forward)
            {
                RotacionarEmDirecaoAAlgo(this.gameObject, CorpoMonge.transform.forward, 2f);
            }
            else
            {
                CorpoMonge.transform.rotation = transform.rotation;
            }
        }
        else
        {
            RotacionarEmDirecaoAAlgo(CorpoMonge.gameObject, transform.forward, 2.5f);
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

    public void RotacionarEmDirecaoAAlgo(GameObject obj, Vector3 ondeOlhar, float velocidadeGiro)
    {
        Vector3 newDirection = Vector3.RotateTowards(obj.transform.forward, ondeOlhar, velocidadeGiro * Time.deltaTime, 0.0f);
        obj.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public bool SeTouroEstaDomado()
    {
        return touroDomado;
    }

    void ControleBotaoInteracao()
    {
        PertoCanalizador();
        if(longeCanalizador && semPedra)
        {
            BotaoInteracao.gameObject.SetActive(false);
        }
    }

    void EmpurrarPedra()
    {
        if (!empurrandoPedra)
        {
            Pedra = ChecarSePertoDePedra();
            if (Pedra != null)
            {
                if (ChecarSePodeMoverPedra() && EncontrarFrentePedra(Pedra) != new Vector3(0, 0, 0))
                {
                    semPedra = false;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)))
                    {
                        //Pedra.gameObject.layer = LayerMask.NameToLayer("Default");
                        frentePedra = EncontrarFrentePedra(Pedra);
                        PedraPosInicial = Pedra.transform.position;
                        AnimacaoEmpurrarPedra(0);
                        Corpo.transform.position -= Corpo.transform.forward * 0.6f;
                        MaoColisor.SetActive(true);
                        BotaoInteracao.gameObject.SetActive(false);
                        empurrandoPedra = true;
                    }
                    else
                    {
                        BotaoInteracao.gameObject.SetActive(true);
                    }
                }
                else
                {
                    semPedra = true;
                }
            }
        }
        else
        {
            float velocidadeGiroParaPedra = 1f;
            float rapidezEmpurrar = 0.5f;

            RotacionarEmDirecaoAAlgo(this.gameObject, frentePedra, velocidadeGiroParaPedra);
            RotacionarEmDirecaoAAlgo(CorpoMonge, frentePedra, velocidadeGiroParaPedra);
            PrenderPersonagem();
            Pedra.GetComponent<Rigidbody>().mass = 1;

            // Para prender o movimento da pedra nos eixos não desejado

            if (frentePedra.x == 0)
            {
                Pedra.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                Corpo.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; 
            }
            else
            {
                Pedra.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                Corpo.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            Corpo.velocity = velocidadeAndar * rapidezEmpurrar * frentePedra;


            float tpzI = PedraPosInicial.z;
            float tpz = Pedra.transform.position.z;
            float tpxI = PedraPosInicial.x;
            float tpx = Pedra.transform.position.x;

            if (System.Math.Round(tpz, 0) == System.Math.Round(tpzI - 8, 0) || System.Math.Round(tpz, 0) == System.Math.Round(tpzI + 8, 0) || System.Math.Round(tpx, 0) == System.Math.Round(tpxI + 8, 0) || System.Math.Round(tpx, 0) == System.Math.Round(tpxI - 8, 0))
            {
                Corpo.velocity = new Vector3(0, 0, 0);
                Pedra.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                Pedra.GetComponent<Rigidbody>().mass = 1000000f;
                AnimacaoEmpurrarPedra(1);
                MaoColisor.SetActive(false);
                movimentoPermitido = true;
                empurrandoPedra = false;
            }
        }
    }
    void AnimacaoEmpurrarPedra(int estado)
    {
        if (touroDomado)
        {
            // Sequência de animações com touro
        }
        else
        {
            if(estado == 0)
            {
                Anim.SetBool("Andando", false);
                Anim.SetBool("Correndo", false);
                Anim.SetBool("Empurrando", true);
            }
            else
            {
                Anim.SetBool("Empurrando", false);
            }
        }
    }

    void PertoCanalizador()
    {
        GameObject[] Canalizadores;
        Canalizadores = GameObject.FindGameObjectsWithTag("Canalizador");

        float minimumDistance = 4.5f;

        Transform CanalizadorMaisPerto = null;

        foreach (GameObject can in Canalizadores)
        {
            float distance = Vector3.Distance(transform.position, can.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                CanalizadorMaisPerto = can.transform;
            }
        }

        if (CanalizadorMaisPerto != null)
        {
            longeCanalizador = false;
        }
        else
        {
            longeCanalizador = true;
        }
    }

    Transform ChecarSePertoDePedra()
    {
        GameObject[] PedrasLeves;
        PedrasLeves = GameObject.FindGameObjectsWithTag("PedraLeve");

        float minimumDistance = 6.5f;

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

    bool ChecarSePodeMoverPedra()
    {
        //Pedra.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        frentePedra = EncontrarFrentePedra(Pedra);
        Vector3 direction = frentePedra;

        // Verificação que pode mover a pedra se for pesada
        if (Pedra.gameObject.tag == "PedraPesada")
        {
            if (!touroDomado)
            {
                return false;
            }
        }
        if (Vector3.Angle(CorpoMonge.transform.forward, direction) > 45)
        {
            return false;
        }

        RaycastHit meuRay;
        if (Physics.Raycast(Pedra.transform.position, direction, out meuRay, 10f, ~semRC))
        {
            string colisor = meuRay.collider.gameObject.tag;
            if (colisor != "Parede" && colisor != "PedraLeve" && colisor != "PedraPesada" && colisor != "Raiz1" && colisor != "Raiz2" && colisor != "Canalizador")
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
        Vector3 direction = CorpoMonge.transform.position - Pedra.position;

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
                    TelaPretaPanel.GetComponent<TelaPretaFade>().enabled = true;
                    TelaPretaPanel.GetComponent<TelaPretaFade>().FadeIn(infoCheckpoint[0], infoCheckpoint[1], true);
                    segundosParaEsperar = 1.2f;
                    Corpo.velocity = new Vector3(0f, 0f, 0f);
                    Anim.SetBool("Correndo", false);
                    Anim.SetBool("Andando", false);
                    BotaoInteracao.gameObject.SetActive(false);

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
        Anim.SetBool("Correndo", false);
        Anim.SetBool("Andando", false);
        movimentoPermitido = false;
        Corpo.velocity = new Vector3(0, 0, 0);
    }
    public void DesprenderPersonagem()
    {
        movimentoPermitido = true;
    }
}
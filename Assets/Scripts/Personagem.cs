using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    private Rigidbody Corpo;

    [SerializeField]
    float sensibilidadeGiro = 100f;

    [SerializeField]
    float velocidadeAndar = 4;

    [SerializeField]
    bool esperandoSegundos = false;
    float forcaPulo = 1000;
    public GameObject pezinho;
    bool estaNoChao = true;

    float tempo = 0.0f;
    float segundosParaEsperar;

    // Estado da fase
    enum EstadoFase { SemPuzzleSemDom = 0, PuzzlePedrasSemDom = 1, PuzzlePedrasComDom = 2, PuzzleRuinas = 3, SemPuzzleComDom = 4};

    // Sistema checkpoints
    Vector3[] infoCheckpoint = new Vector3[2];

    // Puzzle das pedras
    bool comTouro = false;
    bool empurrandoPedra = false;
    Transform Pedra;
    Vector3 PedraPosInicial;
    public Vector3 frentePedra;


    void Start()
    {
        Corpo = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Se necessário esperar segundos sem movimento nenhum, o controle está aqui
        if (esperandoSegundos)
        {
            EsperarSegundos(segundosParaEsperar);
        }

        // O player só pode se mover se não estiver no meio de empurrar uma pedra
        ControleMovimento();

        // Puzzle das pedras
        ResetarPuzzlePedras();
        EmpurrarPedra();
    }

    void ControleMovimento()
    {
        if (!esperandoSegundos)
        {
            if (!empurrandoPedra)
            {
                Pular();
                Mover();
            }
        }
    }

    void Mover()
    {
        float velocidadeZ;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocidadeZ = Input.GetAxis("Vertical") * velocidadeAndar * 1.7f;
        }
        else
        {
            velocidadeZ = Input.GetAxis("Vertical") * velocidadeAndar;
        }
        
        float velocidadeX = 0;
        Vector3 velocidadeCorrigida = velocidadeX * transform.right + velocidadeZ * transform.forward;

        Corpo.velocity = new Vector3(velocidadeCorrigida.x, Corpo.velocity.y, velocidadeCorrigida.z);

        Girar();
    }

    void Girar()
    {
        float GiroY = Input.GetAxis("Horizontal") * sensibilidadeGiro * Time.deltaTime;
        transform.Rotate(Vector3.up * GiroY);
    }

    void Pular()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            Corpo.AddForce(Vector3.up * forcaPulo);
            estaNoChao = false;
        }
    }

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Chao")
        {
            estaNoChao = true;
        }

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
                infoCheckpoint[0] = colidiu.gameObject.GetComponent<Checkpoint>().Posicao;
                infoCheckpoint[1] = colidiu.gameObject.GetComponent<Checkpoint>().Rotacao;
                Destroy(colidiu.gameObject);
            }
        }
    }
    void RotacionarEmDirecaoAAlgo(Vector3 ondeOlhar, float velocidadeGiro)
    {
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, ondeOlhar, velocidadeGiro * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void EmpurrarPedra()
    {
        if (!empurrandoPedra)
        {
            Pedra = ChecarSePertoDePedra();
            if (Pedra != null && Input.GetKeyDown(KeyCode.E) && ChecarSePodeMoverPedra(Pedra) && EncontrarFrentePedra(Pedra) != new Vector3(0, 0, 0))
            {
                frentePedra = EncontrarFrentePedra(Pedra);
                Pedra.GetComponent<Pedra>().enabled = true;
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
                Pedra.GetComponent<Pedra>().enabled = false;
                empurrandoPedra = false;
            }
        }
    }
    void AnimacaoEmpurrarPedra()
    {
        if (comTouro)
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

        float minimumDistance = 4.5f;

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
            if (!comTouro)
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
            if (colisor != "Parede" && colisor != "PedraLeve" && colisor != "PedraPesada")
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
        if(PegarEstadoFase() == (int)EstadoFase.PuzzlePedrasSemDom || PegarEstadoFase() == (int)EstadoFase.PuzzlePedrasComDom)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if(infoCheckpoint[0] != new Vector3(0f, 0f, 0f))
                {
                    transform.position = infoCheckpoint[0];
                    transform.Rotate(infoCheckpoint[1]);
                    segundosParaEsperar = 2f;
                    Corpo.velocity = new Vector3(0f, 0f, 0f);
                    esperandoSegundos = true;
                }
            }
        }
    }

    int PegarEstadoFase()
    {
        return GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetEstadoFase();
    }

    bool EsperarSegundos(float segundos)
    {
        tempo += Time.deltaTime;
        if(tempo > segundos)
        {
            tempo = 0.0f;
            esperandoSegundos = false;
            return true;
        }
        else
        {
            esperandoSegundos = true;
            return false;
        }
    }

}
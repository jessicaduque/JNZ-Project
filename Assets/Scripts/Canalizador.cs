using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canalizador : MonoBehaviour
{
    GameObject Player;
    bool trocandoRaizes = false;
    [SerializeField]
    private GameObject CorpoMonge;
    [SerializeField]
    private Image BotaoInteracao;

    [SerializeField]
    GameObject EfeitosPrefab;
    GameObject Efeitos;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (trocandoRaizes)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().AtivacaoRaizes(GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetRaizesAtivados());
            Vector3 relativePos = new Vector3(transform.position.x, Player.transform.position.y, transform.position.z) - Player.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            CorpoMonge.transform.rotation = Quaternion.Lerp(CorpoMonge.transform.rotation, toRotation, 3 * Time.deltaTime);
            GetComponent<EfeitoVisualCanalizar>().Canalizando();

        }
        else
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < 5f)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    AnimacaoAtivacaoPersonagem();
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().TrocarRaizesAtivos();
                    Player.GetComponent<Personagem>().PrenderPersonagem();
                    CorpoMonge.GetComponent<CorpoMonge>().ReceberCanalizador(this.gameObject);
                    BotaoInteracao.gameObject.SetActive(false);
                    trocandoRaizes = true;
                    Efeitos = Instantiate(EfeitosPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    BotaoInteracao.gameObject.SetActive(true);
                }
            }
        }
    }

    void AnimacaoAtivacaoPersonagem()
    {
        Player.GetComponent<Personagem>().Anim.SetTrigger("Canalizar");
    }

    public void AcabouTroca()
    {
        GetComponent<EfeitoVisualCanalizar>().NaoCanalizando();
        Player.GetComponent<Personagem>().DesprenderPersonagem();
        Destroy(Efeitos);
        trocandoRaizes = false;
    }
}

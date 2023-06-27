using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canalizador : MonoBehaviour
{
    GameObject Player;
    bool trocandoRaizes = false;
    [SerializeField]
    private GameObject CorpoMonge;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (trocandoRaizes)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().AtivacaoRaizes(GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetRaizesAtivados());
            Player.GetComponent<Personagem>().RotacionarEmDirecaoAAlgo(CorpoMonge, new Vector3(transform.position.x, CorpoMonge.transform.position.y, transform.position.z), 2f);
            //CorpoMonge.transform.LookAt(new Vector3(transform.position.x, CorpoMonge.transform.position.y, transform.position.z));
        }
        else
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < 3.8f)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    AnimacaoAtivacaoPersonagem();
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().TrocarRaizesAtivos();
                    Player.GetComponent<Personagem>().PrenderPersonagem();
                    CorpoMonge.GetComponent<CorpoMonge>().ReceberCanalizador(this.gameObject);
                    trocandoRaizes = true;
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
        Player.GetComponent<Personagem>().DesprenderPersonagem();
        trocandoRaizes = false;
    }
}

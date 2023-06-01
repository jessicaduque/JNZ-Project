using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canalizador : MonoBehaviour
{
    GameObject Player;
    bool trocandoRaizes = false;
    int raizesAtivados;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        raizesAtivados = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().raizesIniciais;
    }

    void Update()
    {
        if (trocandoRaizes)
        {
            //GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().AtivacaoRaizes(raizesAtivados);
        }
        else
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < 3.8f)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    AnimacaoAtivacaoPersonagem();
                    TrocarRaizesAtivos();
                    // Deletar dps
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().AtivacaoRaizes(raizesAtivados);
                    //trocandoRaizes = true;
                }
            }
        }
    }

    void AnimacaoAtivacaoPersonagem()
    {
        //Player.GetComponent<Animator>().SetTrigger();
    }

    public void TrocarRaizesAtivos()
    {
        if (raizesAtivados == 1)
        {
            raizesAtivados = 2;
        }
        else
        {
            raizesAtivados = 1;
        }

    }

    public void AcabouTroca()
    {
        trocandoRaizes = false;
    }
}

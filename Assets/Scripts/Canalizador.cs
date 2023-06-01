using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canalizador : MonoBehaviour
{
    GameObject Player;
    bool trocandoRaizes = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (trocandoRaizes)
        {
            //GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().AtivacaoRaizes(GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetRaizesAtivados());
        }
        else
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < 3.8f)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    AnimacaoAtivacaoPersonagem();
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().TrocarRaizesAtivos();
                    // Deletar dps
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().AtivacaoRaizes(GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetRaizesAtivados());
                    //trocandoRaizes = true;
                }
            }
        }
    }

    void AnimacaoAtivacaoPersonagem()
    {
        //Player.GetComponent<Animator>().SetTrigger();
    }

    public void AcabouTroca()
    {
        trocandoRaizes = false;
    }
}

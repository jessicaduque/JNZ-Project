using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touro : MonoBehaviour
{
    bool domado = false;
    [SerializeField]
    float velocidadeGiro = 2f;
    GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        if (!domado && GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetEstadoFase() == 3)
        {
            Debug.Log("entrou");
            OlharPlayer();
        }
        else
        {
            MoverComPlayer();
        }
    }

    public void TouroDomado()
    {
        domado = true;
    }

    void OlharPlayer()
    {
        transform.LookAt(Player.transform.position);
    }

    void MoverComPlayer()
    {
        // Por enquanto nada
    }

}

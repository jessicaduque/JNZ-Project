using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedra : MonoBehaviour
{
    GameObject Player;
    Vector3 frente;
    float rapidez = 10f;
    public Vector3 PosicaoInicial;
    bool podeMover = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PosicaoInicial = transform.position;
    }

    void Update()
    {
        if (podeMover)
        {
            frente = Player.GetComponent<Personagem>().frentePedra;
        }
    }

    private void FixedUpdate()
    {
        if (podeMover)
        {
            Debug.Log(frente);
            Player.GetComponent<Rigidbody>().velocity = rapidez * frente;
        }
    }

    public void MudarEstadoMovimento()
    {
        podeMover = !podeMover;
    }
    
}

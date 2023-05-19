using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedra : MonoBehaviour
{
    GameObject Player;
    Vector3 frente;
    float rapidez = 2.5f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        frente = Player.GetComponent<Personagem>().frentePedra;
        transform.position += rapidez * frente * Time.deltaTime;
    }
}

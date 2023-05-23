using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorFase : MonoBehaviour
{
    enum EstadoFase { SemPuzzleSemDom, PuzzlePedrasSemDom, PuzzlePedrasComDom, PuzzleRuinas, SemPuzzleComDom};

    EstadoFase estadoDaFase;
    bool touroDomado = false;
    GameObject Player;

    // 1o elemento: Canto esquerda superior
    // 2o elemento: Canto direita inferior
    public Vector3[] areaPuzzlePedras;
    public Vector3[] areaPuzzleRuinas;

    void Start()
    {
        estadoDaFase = EstadoFase.SemPuzzleSemDom;
        TrancarMouse();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        AtualizarEstadoFase();
    }

    void TrancarMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void AtualizarEstadoFase()
    {
        if (dentroAreaPuzzle(areaPuzzlePedras))
        {
            if (touroDomado)
            {
                estadoDaFase = EstadoFase.PuzzlePedrasComDom;
            }
            else
            {
                estadoDaFase = EstadoFase.PuzzlePedrasSemDom;
            }
        }
        else if (dentroAreaPuzzle(areaPuzzleRuinas))
        {
            estadoDaFase = EstadoFase.PuzzleRuinas;
        }
        else if (touroDomado)
        {
            estadoDaFase = EstadoFase.SemPuzzleComDom;
        }
        else
        {
            estadoDaFase = EstadoFase.SemPuzzleSemDom;
        }
    }
    
    bool dentroAreaPuzzle(Vector3[] area)
    {
        if((Player.transform.position.x >= area[0].x && Player.transform.position.x <= area[1].x) && (Player.transform.position.z >= area[1].z && Player.transform.position.z <= area[0].z))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetEstadoFase()
    {
        return ((int)estadoDaFase);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorFase : MonoBehaviour
{
    enum EstadoFase { SemPuzzleSemDom, PuzzlePedrasSemDom, PuzzlePedrasComDom, PuzzleRuinas, SemPuzzleComDom};
    [SerializeField]
    public int raizesIniciais = 1;
    GameObject[] Raizes1;
    GameObject[] Raizes2;

    EstadoFase estadoDaFase;
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

        Raizes1 = GameObject.FindGameObjectsWithTag("Raiz1");
        Raizes2 = GameObject.FindGameObjectsWithTag("Raiz2");
        AtivacaoRaizes(raizesIniciais);
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
            if (Player.GetComponent<Personagem>().SeTouroEstaDomado())
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
        else if (Player.GetComponent<Personagem>().SeTouroEstaDomado())
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

    public void AtivacaoRaizes(int raizes)
    {

        if (raizes == 2)
        {
            Debug.Log("bro2");
            for (int i = 0; i < Raizes1.Length; i++)
            {
                Raizes1[i].SetActive(false);
            }

            for (int i = 0; i < Raizes2.Length; i++)
            {
                Raizes2[i].SetActive(true);
            }
        }
        else
        {
            Debug.Log("bro1");
            for (int i = 0; i < Raizes1.Length; i++)
            {
                Raizes1[i].SetActive(true);
            }

            for (int i = 0; i < Raizes2.Length; i++)
            {
                Raizes2[i].SetActive(false);
            }
        }
    }

}

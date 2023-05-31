using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canalizador : MonoBehaviour
{
    GameObject Player;
    GameObject[] Raizes1;
    GameObject[] Raizes2;
    [SerializeField]
    int raizesAtivados = 1;
    bool trocandoRaizes = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Raizes1 = GameObject.FindGameObjectsWithTag("Raiz1");
        Raizes2 = GameObject.FindGameObjectsWithTag("Raiz2");

        if (raizesAtivados == 2)
        {
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

    void Update()
    {
        if (trocandoRaizes)
        {
            //TrocarRaizes(raizesAtivados);
        }
        else
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < 3.8f)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    AnimacaoAtivacaoPersonagem();
                    if(raizesAtivados == 1)
                    {
                        raizesAtivados = 2;
                    }
                    else
                    {
                        raizesAtivados = 1;
                    }
                    //trocandoRaizes = true;
                    TrocarRaizes(raizesAtivados);
                }
            }
        }
    }

    void AnimacaoAtivacaoPersonagem()
    {
        //Player.GetComponent<Animator>().SetTrigger();
    }

    void TrocarRaizes(int raizes)
    {
        if(raizes == 2)
        {
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

    public void AcabouTroca()
    {
        trocandoRaizes = false;
    }
}

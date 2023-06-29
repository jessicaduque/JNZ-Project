using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporteCaverna : MonoBehaviour
{
    [SerializeField]
    private Vector3 Destino;
    [SerializeField]
    private Vector3 ondeOlhar;
    [SerializeField]
    private GameObject TelaPretaPanel;

    private GameObject Player;
    private bool personagemPreso = false;

    [SerializeField]
    List<GameObject> ItensParaDestruir = new List<GameObject>();

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (personagemPreso)
        {
            if (Player.GetComponent<Personagem>().EsperarSegundos(3))
            {
                Player.GetComponent<Personagem>().DesprenderPersonagem();
                personagemPreso = false;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            TelaPretaPanel.GetComponent<TelaPretaFade>().enabled = true;
            TelaPretaPanel.GetComponent<TelaPretaFade>().FadeIn(Destino, ondeOlhar, false);
            Player.GetComponent<Personagem>().PrenderPersonagem();
            personagemPreso = true;

            for(int i = 0; i < ItensParaDestruir.Count; i++)
            {
                if(ItensParaDestruir[i] != null)
                {
                    Destroy(ItensParaDestruir[i]);
                }
                
            }
        }   
    }
}

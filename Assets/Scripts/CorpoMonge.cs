using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpoMonge : MonoBehaviour
{
    private GameObject Canalizador;

    public void ReceberCanalizador(GameObject canali)
    {
        Canalizador = canali;
    }

    public void AcabouTroca()
    {
        Canalizador.GetComponent<Canalizador>().AcabouTroca();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedra : MonoBehaviour
{
    public Vector3 PosicaoInicial;

    void Start()
    {
        PosicaoInicial = transform.position;
    }

}

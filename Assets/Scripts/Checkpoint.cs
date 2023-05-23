using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 Posicao;
    public Vector3 Rotacao;
    public bool UltimoCheck;

    private void Start()
    {
        Posicao = GetComponent<Transform>().position;
        Rotacao = GetComponent<Transform>().rotation.eulerAngles;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EfeitoVisualCanalizar : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vcam;
    [SerializeField]
    float zOriginal;
    private float zMin = -8.5f;
    [SerializeField]
    float Zdec;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    float Pretodec;

    bool finalizando = false;

    bool cam = false;
    bool preta = false;

    private void Update()
    {
        if (finalizando)
        {
            NaoCanalizando();
        }
        else
        {
            preta = false;
            cam = false;
        }
    }

    public void Canalizando()
    {
        // Parte da cam
        var transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        float zAtual = transposer.m_FollowOffset.z;
        if (zAtual < zMin)
        {
            transposer.m_FollowOffset += new Vector3(0, 0, Zdec) * Time.deltaTime;
        }

        // Parte da tela preta
        if (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Pretodec * Time.deltaTime;
        }
    }

    public void NaoCanalizando()
    {
        // Parte da cam
        finalizando = true;
        var transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        float zAtual = transposer.m_FollowOffset.z;
        if (zAtual > zOriginal)
        {
            transposer.m_FollowOffset -= new Vector3(0, 0, Zdec) * Time.deltaTime;
            cam = false;
        }
        else
        {
            transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, zOriginal);
            cam = true;
        }

        // Parte da tela preta
        if (canvasGroup.alpha > 0)
        {
            preta = false;
            canvasGroup.alpha -= Pretodec * Time.deltaTime;
        }
        else
        {
            preta = true;
        }

        if(preta && cam)
        {
            finalizando = false;
        }
    }

}

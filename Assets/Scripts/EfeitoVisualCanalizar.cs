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

    bool finalizando = false;


    private void Update()
    {
        if (finalizando)
        {
            NaoCanalizando();
        }
    }

    public void Canalizando()
    {
        var transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        float zAtual = transposer.m_FollowOffset.z;
        if(zAtual < zMin)
        {
            transposer.m_FollowOffset += new Vector3(0, 0, Zdec) * Time.deltaTime;
        }
    }

    public void NaoCanalizando()
    {
        finalizando = true;
        var transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        float zAtual = transposer.m_FollowOffset.z;
        if (zAtual > zOriginal)
        {
            transposer.m_FollowOffset -= new Vector3(0, 0, Zdec) * Time.deltaTime;
        }
        else
        {
            transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, zOriginal);
            finalizando = false;
        }
    }

}

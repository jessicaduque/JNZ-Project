using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaPretaFade : MonoBehaviour
{
    private GameObject Player;
    [SerializeField]
    private CanvasGroup canvasGroup;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private Vector3 ondeOlharPlayer;
    private Vector3 posDestinoPlayer;
    [SerializeField]
    private GameObject CorpoMonge;
    bool usarEuler;

    [SerializeField] float timeToFade;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        fadeIn = false;
        fadeOut = false;
    }

    void Update()
    {
        if (fadeIn)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += timeToFade * Time.deltaTime;
                if (canvasGroup.alpha >= 1)
                {
                    Player.transform.position = posDestinoPlayer;
                    if (usarEuler)
                    {
                        CorpoMonge.transform.eulerAngles = ondeOlharPlayer;
                        Player.transform.eulerAngles = CorpoMonge.transform.eulerAngles;
                        Player.GetComponent<Personagem>().TerminarResetPuzzle();
                    }
                    else
                    {
                        Player.transform.LookAt(ondeOlharPlayer);
                        CorpoMonge.transform.rotation = Player.transform.rotation;
                    }
                    
                    fadeIn = false;
                }
            }
        }
        else
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= timeToFade * Time.deltaTime;
                if (canvasGroup.alpha == 0)
                {
                    this.enabled = false;
                }
            }
        }
    }

    public void FadeIn(Vector3 posDestino, Vector3 ondeOlhar, bool euler)
    {
        usarEuler = euler;
        posDestinoPlayer = posDestino;
        ondeOlharPlayer = ondeOlhar;
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }

}

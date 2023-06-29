using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject menu;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ComecarJogo()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(1);
    }
    //public void Configuracoes()
    //{
    //    SceneManager.LoadScene(0);
    //}

    //public void Creditos()
    //{
    //    SceneManager.LoadScene(0);
    //}


    public void SairJogo()
    {
        Application.Quit();
    }

    public void Recomeçar()
    {
        SceneManager.LoadScene(0);
    }

    public void MostrarCreditos()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }

    public void Voltar()
    {
        menu.SetActive(true);
        credits.SetActive(false);
    }

}

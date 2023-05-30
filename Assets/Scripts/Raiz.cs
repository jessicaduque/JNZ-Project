using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raiz : MonoBehaviour
{

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "PedraLeve" || collision.gameObject.tag == "PedraPesada")
        {
            this.gameObject.SetActive(false);
        }
    }
}

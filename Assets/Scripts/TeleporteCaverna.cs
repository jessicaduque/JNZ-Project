using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporteCaverna : MonoBehaviour
{
    [SerializeField]
    private Vector3 Destino;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = Destino;
        }   
    }
}

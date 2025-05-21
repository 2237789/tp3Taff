using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupteur : MonoBehaviour

{
    private GameObject porte;

    void Start()
    {
        // On récupère la porte au démarrage pour éviter de la chercher à chaque fois
        porte = GameObject.FindGameObjectWithTag("porte");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && porte != null)
        {
            // Inverse l’état actuel de la porte
            porte.SetActive(!porte.activeSelf);

            // Affiche un message dans la console
            Debug.Log("Porte " + (porte.activeSelf ? "fermée" : "ouverte"));
        }
    }
}


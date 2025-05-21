using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupteur : MonoBehaviour

{
    private GameObject porte;

    void Start()
    {
        // On r�cup�re la porte au d�marrage pour �viter de la chercher � chaque fois
        porte = GameObject.FindGameObjectWithTag("porte");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && porte != null)
        {
            // Inverse l��tat actuel de la porte
            porte.SetActive(!porte.activeSelf);

            // Affiche un message dans la console
            Debug.Log("Porte " + (porte.activeSelf ? "ferm�e" : "ouverte"));
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionGlobale : MonoBehaviour
{

    // Référence à tous les objets avec le tag "obstacle"
    private GameObject[] obstacles;

    // Couleurs à appliquer
    public Color redColor = Color.red;
    public Color greenColor = Color.green;

    void Start()
    {
        // Récupérer tous les obstacles au démarrage
        UpdateObstaclesList();
    }

    // Pour mettre à jour la liste des obstacles si nécessaire
    void UpdateObstaclesList()
    {
        obstacles = GameObject.FindGameObjectsWithTag("obstacle");
    }

    // Changer la couleur de tous les obstacles
    void ChangeObstaclesColor(Color newColor)
    {
        foreach (GameObject obstacle in obstacles)
        {
            Renderer renderer = obstacle.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
        }
    }

    // Détecter les collisions
    void OnCollisionEnter(Collision collision)
    {
        // Si collision avec un obstacle
        if (collision.gameObject.CompareTag("obstacle"))
        {
            ChangeObstaclesColor(redColor);
        }
    }

    // Détecter les triggers
    void OnTriggerEnter(Collider other)
    {
        // Si le joueur touche la porte
        if (other.CompareTag("porte"))
        {
            ChangeObstaclesColor(redColor);
        }

        // Si le joueur passe la bande "fin"
        if (other.CompareTag("fin"))
        {
            ChangeObstaclesColor(greenColor);
        }
    }
}

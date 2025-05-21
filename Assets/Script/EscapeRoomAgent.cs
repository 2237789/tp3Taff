using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class EscapeRoomAgent : Agent
{
    [SerializeField] private Transform interrupteurTransform;
    [SerializeField] private Transform sortieTransform;
    [SerializeField] private GameObject porte;
    [SerializeField] private float speed = 11f;
    [SerializeField] private Renderer solRenderer;
    [SerializeField] private Material materielSucces;
    [SerializeField] private Material materielEchec;
    private Rigidbody rb;
    private bool porteOuverte = false;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody manquant sur l'agent. Ajoutez un composant Rigidbody.");
        }
    }

    public override void OnEpisodeBegin()
    {
        // Reset agent et éléments
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Utilisez des positions locales plutôt que mondiales pour être cohérent
       // transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));
        transform.position = new Vector3(Random.Range(-1380f, -1377f), 982.5f, Random.Range(1782f, 1785f));

       // interrupteurTransform.localPosition = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f));
        sortieTransform.localPosition = new Vector3(3, 0.5f, 0);

        // Réinitialisation de l'état de la porte
        porte.SetActive(true); // porte fermée
        porteOuverte = false;

        // Réinitialisation du matériau du sol
        if (solRenderer != null)
        {
            solRenderer.material = null; // Remettre le matériau par défaut
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Position de l'agent
        sensor.AddObservation(transform.localPosition);

        // Position de l'interrupteur
        sensor.AddObservation(interrupteurTransform.localPosition);

        // Position de la sortie
        sensor.AddObservation(sortieTransform.localPosition);

        // État de la porte (1 = fermée, 0 = ouverte)
        sensor.AddObservation(porteOuverte ? 0f : 1f);

        // Direction vers l'interrupteur
        Vector3 dirToInterrupteur = interrupteurTransform.localPosition - transform.localPosition;
        sensor.AddObservation(dirToInterrupteur.normalized);

        // Direction vers la sortie
        Vector3 dirToSortie = sortieTransform.localPosition - transform.localPosition;
        sensor.AddObservation(dirToSortie.normalized);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Récupérer les actions
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        // Créer le vecteur de mouvement
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Appliquer le mouvement
        transform.Translate(move * Time.deltaTime * speed);

        // Pénalité pour chaque pas de temps pour encourager l'efficacité
        AddReward(-0.001f);

        // Récompense de proximité avec l'objectif actuel
        if (!porteOuverte)
        {
            // Si la porte n'est pas ouverte, récompenser la proximité avec l'interrupteur
            float distToInterrupteur = Vector3.Distance(transform.localPosition, interrupteurTransform.localPosition);
            AddReward(-0.001f * distToInterrupteur);
        }
        else
        {
            // Si la porte est ouverte, récompenser la proximité avec la sortie
            float distToSortie = Vector3.Distance(transform.localPosition, sortieTransform.localPosition);
            AddReward(-0.001f * distToSortie);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision avec: " + other.tag);

        if (other.CompareTag("free"))
        {
            Debug.Log("Interrupteur activé");
            // Inverser l'état de la porte
            porteOuverte = !porteOuverte;
            porte.SetActive(!porteOuverte);

            if (porteOuverte)
            {
                AddReward(0.5f); // Récompense pour avoir ouvert la porte
            }
        }
        else if (other.CompareTag("fin"))
        {
            Debug.Log("Sortie atteinte");
            if (porteOuverte)
            {
                Debug.Log("Succès!");
                AddReward(1f); // Succès
                solRenderer.material = materielSucces;
            }
            else
            {
                Debug.Log("Échec: porte fermée");
                AddReward(-1f); // Tentative de sortie par porte fermée
                solRenderer.material = materielEchec;
            }
            EndEpisode();
        }
        else if (other.CompareTag("obstacle"))
        {
            Debug.Log("Collision avec obstacle");
            AddReward(-1f); // Collision avec un mur
            solRenderer.material = materielEchec;
            EndEpisode();
        }
        else if (other.CompareTag("porte") && !porteOuverte)
        {
            Debug.Log("Collision avec porte fermée");
            AddReward(-1f); // Collision avec la porte fermée
            solRenderer.material = materielEchec;
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
    }
}
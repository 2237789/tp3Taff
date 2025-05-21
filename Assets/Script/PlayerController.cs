using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;
    private Vector3 input;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Récupérer l'entrée clavier
        float moveX = Input.GetAxis("Horizontal"); // A/D ou flèches gauche/droite
        float moveZ = Input.GetAxis("Vertical");   // W/S ou flèches haut/bas
        input = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate()
    {
        // Appliquer le déplacement
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
    }
}

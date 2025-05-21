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
        // R�cup�rer l'entr�e clavier
        float moveX = Input.GetAxis("Horizontal"); // A/D ou fl�ches gauche/droite
        float moveZ = Input.GetAxis("Vertical");   // W/S ou fl�ches haut/bas
        input = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate()
    {
        // Appliquer le d�placement
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
    }
}

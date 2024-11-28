using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Velocidades del jugador
    public float velocidadJugador = 2f;
    public float fuerzaSaltoJugador = 2f;

    // Layers
    public LayerMask suelo;

    // Raycasts
    public float raycastSaltoLength = 2f; // Salto

    // Elementos
    private Rigidbody rb;

    // Verificaciones
    private bool estaEnSuelo;
    public bool sePuedeMover;
    public bool estaMuerto;

    // Contador de monedas
    private int contadorMonedas = 0;  // Contador de monedas recogidas
    public int contadorMonedasMaximo = 20;  // Monedas necesarias para algo (por ejemplo, cambiar de nivel)

    // Audio
    public AudioClip saltoSonido;
    public AudioClip muerteSonido;

    private void Start()
    {
        sePuedeMover = true;
        estaMuerto = false;
        rb = GetComponent<Rigidbody>();

        // Bloquear las rotaciones en X y Z para que no ruede
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    private void Update()
    {
        // Si se puede mover y no está muerto
        if (sePuedeMover && !estaMuerto) {
            caminar();
        }
    }

    // -------------------------- MOVIMIENTO INICIO -------------------------- 
    void caminar()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Crear dirección de movimiento en el plano
        Vector3 direccionMovimiento = Camera.main.transform.right * horizontal + Camera.main.transform.forward * vertical;
        direccionMovimiento.y = 0; // Evitar movimiento vertical

        // Aplicar movimiento ajustando directamente la velocidad del Rigidbody
        Vector3 nuevaVelocidad = direccionMovimiento.normalized * velocidadJugador;
        nuevaVelocidad.y = rb.velocity.y; // Mantener la velocidad vertical actual
        rb.velocity = nuevaVelocidad;

        // Rotar el jugador para mirar en la dirección de movimiento
        if (direccionMovimiento.magnitude > 0)
        {
            // Verifica si el movimiento es hacia adelante
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0; // Mantener solo el plano horizontal
            Vector3 direction = Vector3.ProjectOnPlane(direccionMovimiento, Vector3.up); // Ignorar inclinación vertical

            // Si el movimiento es hacia adelante, rota el jugador
            if (Vector3.Dot(forward, direction) > 0)
            {
                Quaternion rotacion = Quaternion.LookRotation(direccionMovimiento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 10f);
            }
        }
    }

    // Detectar cuando el jugador recoge una moneda
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))  // Comprobar si el objeto que tocamos tiene la etiqueta "Coin"
        {
            // Recoger la moneda
            contadorMonedas++;

            // Destruir la moneda para que desaparezca
            Destroy(other.gameObject);

            // Mostrar el contador de monedas en la consola (puedes añadir UI si lo deseas)
            Debug.Log("Monedas: " + contadorMonedas);

            // Si alcanzamos el número máximo de monedas, podríamos cambiar de escena o hacer algo
            if (contadorMonedas >= contadorMonedasMaximo)
            {
                // Lógica para cambiar de escena, por ejemplo:
                // SceneManager.LoadScene("NextLevel");
                Debug.Log("¡Has recogido suficientes monedas para avanzar!");
            }
        }
    }
    // -------------------------- MOVIMIENTO FINAL -------------------------- 

    // -------------------------- GIZMOS INICIO -------------------------- 
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastSaltoLength);
    }
    // -------------------------- GIZMOS FINAL -------------------------- 
}

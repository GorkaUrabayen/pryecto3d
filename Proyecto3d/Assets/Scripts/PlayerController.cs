using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Velocidades del jugador
    public float velocidadJugador = 2f;

    // Elementos
    private Rigidbody rb;

    // Verificaciones
    public bool sePuedeMover;
    public bool estaMuerto;

    // Contador de monedas
    private int contadorMonedas = 0;  // Contador de monedas recogidas
    public int contadorMonedasMaximo = 20;  // Monedas necesarias para algo (por ejemplo, cambiar de nivel)

    // Referencia al ManagerController
    private ManagerController managerController;

    private void Start()
    {
        sePuedeMover = true;
        estaMuerto = false;
        rb = GetComponent<Rigidbody>();

        // Buscar el ManagerController una vez al iniciar
        managerController = FindObjectOfType<ManagerController>();
        if (managerController == null)
        {
            Debug.LogError("ManagerController no encontrado. Asegúrate de que exista en la escena.");
        }

        // Bloquear las rotaciones en X y Z para que no ruede
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    private void Update()
    {
        // Si se puede mover y no está muerto
        if (sePuedeMover && !estaMuerto)
        {
            caminar();
        }
    }

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
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0; // Mantener solo el plano horizontal
        Vector3 direction = Vector3.ProjectOnPlane(direccionMovimiento, Vector3.up); // Ignorar inclinación vertical

        if (Vector3.Dot(forward, direction) > 0)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);

            // Rotación suave del jugador
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f); // Ajusta el factor para mayor suavidad
        }
    }
}


    // Detectar cuando el jugador recoge una moneda
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin")) // Comprobar si el objeto tiene la etiqueta "Coin"
        {
            if (managerController != null)
            {
                // Notificar al ManagerController que una moneda ha sido recogida
                managerController.MonedaRecogida();
            }
            else
            {
                Debug.LogWarning("ManagerController no está asignado. No se puede notificar la recogida de la moneda.");
            }

            // Destruir la moneda
            Destroy(other.gameObject);

            // Mostrar el contador de monedas en la consola
            contadorMonedas++;
            Debug.Log("Monedas recogidas por el jugador: " + contadorMonedas);
        }
    }
}

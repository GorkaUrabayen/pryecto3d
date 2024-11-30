using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoController : MonoBehaviour
{
    public Transform jugador; // Referencia al transform del jugador
    public float velocidad = 3f; // Velocidad de movimiento del enemigo
    public float distanciaDeteccionSonido = 10f; // Distancia a la que el sonido debe reproducirse
    public float alturaSuelo = 1f; // Distancia desde el suelo
    public float fuerzaGravedad = 9.8f; // Fuerza de gravedad para simular el efecto físico

    // Referencia al AudioSource
    private AudioSource audioSource;

    // Sonido a reproducir cuando se acerque al jugador
    public AudioClip sonidoCerca;

    // Referencia al Rigidbody del humano
    private Rigidbody rb;

    void Start()
    {
        // Obtener el AudioSource adjunto al objeto
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se ha encontrado un AudioSource en el Humano.");
        }

        // Obtener el Rigidbody adjunto al objeto
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se ha encontrado un Rigidbody en el Humano.");
        }

        // Asegurarse de que la rotación del Rigidbody esté congelada
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (jugador != null)
        {
            // Calcula la dirección hacia el jugador
            Vector3 direccion = (jugador.position - transform.position).normalized;

            // Mantener la dirección en el plano horizontal (sin afectar el eje Y)
            direccion.y = 0;

            // Mover al enemigo hacia el jugador usando Rigidbody para movimiento físico
            rb.velocity = new Vector3(direccion.x * velocidad, rb.velocity.y, direccion.z * velocidad);

            // Hacer que el enemigo mire hacia el jugador
            transform.LookAt(new Vector3(jugador.position.x, transform.position.y, jugador.position.z));

            // Verificar la distancia entre el humano y el jugador
            float distancia = Vector3.Distance(transform.position, jugador.position);

            // Si la distancia es menor que el umbral de detección
            if (distancia <= distanciaDeteccionSonido)
            {
                // Reproducir el sonido si no se está reproduciendo ya
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(sonidoCerca);
                }
            }
            else
            {
                // Detener el sonido si estamos alejándonos del jugador
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }

            // Asegurarnos de que el enemigo no atraviese el suelo (usamos un raycast para detectar el terreno)
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, alturaSuelo))
            {
                // Asegurar que el enemigo se quede sobre el suelo
                rb.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else
            {
                // Si no tocamos el suelo, aplicar gravedad
                rb.AddForce(Vector3.down * fuerzaGravedad, ForceMode.Acceleration);
            }
        }
    }

    // Detectar colisiones con objetos, como paredes o el suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            // Si el humano choca con el suelo, puede reaccionar de alguna manera
            // En este caso, simplemente mostramos un mensaje
           // Debug.Log("El humano ha tocado el suelo.");
        }
    }
}

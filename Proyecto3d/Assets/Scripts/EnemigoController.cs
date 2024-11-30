using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    public Transform jugador; // Referencia al jugador
    public float velocidad = 3f; // Velocidad de movimiento del enemigo
    public float distanciaDeteccion = 2f; // Distancia de detección para evitar paredes
    public LayerMask layerSuelo; // La capa que se considera como obstáculos (paredes)

    // Vector que define el lado de "visión" del enemigo (derecha o izquierda)
    public Vector3 offsetVision = Vector3.right; // Puedes usar Vector3.left para el lado izquierdo

    // Referencia al AudioSource del enemigo
    private AudioSource audioSource;
    public AudioClip sonidoEnemigo; // Sonido del enemigo (por ejemplo, música ambiental o un sonido de alerta)

    private void Start()
    {
        // Obtener el AudioSource del enemigo
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se ha encontrado un AudioSource en el enemigo.");
        }

        // Configurar el AudioSource para que sea 3D
        audioSource.spatialBlend = 0f; // Hacer que el sonido sea 3D
        audioSource.loop = true; // Si el sonido debe ser en bucle
        audioSource.clip = sonidoEnemigo;
        
        // Aumentar el rango de escucha del sonido
        audioSource.maxDistance = 300f; // Ajusta la distancia máxima según el tamaño del mapa
        audioSource.minDistance = 100f;  // Distancia mínima a la que el sonido es "full volume"
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Configurar la disminución lineal del volumen

        // Reproducir el sonido
        audioSource.Play();
    }

    private void Update()
    {
        if (jugador == null) return;

        // Dirección hacia el jugador
        Vector3 direccionJugador = (jugador.position - transform.position).normalized;

        // Detectar si hay una pared en la dirección del movimiento
        if (Physics.Raycast(transform.position, direccionJugador, distanciaDeteccion, layerSuelo))
        {
            // Si hay una pared, intentar esquivarla
            EsquivarPared(direccionJugador);
        }
        else
        {
            // No hay paredes, seguir al jugador directamente
            MoverHacia(jugador.position);
        }

        // Asegurar que el enemigo esté orientado correctamente
        AjustarOrientacion();

        // Ajustar el volumen del sonido en función de la distancia al jugador
        AjustarVolumenAudio();
    }

    private void MoverHacia(Vector3 destino)
    {
        // Calcular dirección hacia el destino
        Vector3 direccion = (destino - transform.position).normalized;

        // Mantener posición en el plano horizontal
        direccion.y = 0;

        // Mover al enemigo hacia el destino
        transform.position += direccion * velocidad * Time.deltaTime;
    }

    private void EsquivarPared(Vector3 direccionJugador)
    {
        // Calcular direcciones de rayos para esquivar
        Vector3 direccionIzquierda = Quaternion.Euler(0, -45, 0) * direccionJugador;
        Vector3 direccionDerecha = Quaternion.Euler(0, 45, 0) * direccionJugador;

        // Rayos para detectar obstáculos a la izquierda y derecha
        bool obstaculoIzquierda = Physics.Raycast(transform.position, direccionIzquierda, distanciaDeteccion, layerSuelo);
        bool obstaculoDerecha = Physics.Raycast(transform.position, direccionDerecha, distanciaDeteccion, layerSuelo);

        // Determinar la dirección para esquivar
        Vector3 direccionEsquiva;
        if (!obstaculoIzquierda)
        {
            direccionEsquiva = direccionIzquierda;
        }
        else if (!obstaculoDerecha)
        {
            direccionEsquiva = direccionDerecha;
        }
        else
        {
            // Si ambos lados tienen obstáculos, retrocede
            direccionEsquiva = -direccionJugador;
        }

        // Mover al enemigo en la dirección de esquiva
        MoverHacia(transform.position + direccionEsquiva);
    }

    private void AjustarOrientacion()
    {
        // Dirección hacia el jugador
        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;

        // Rotar para que el "lado de visión" (offsetVision) esté orientado hacia el jugador
        Vector3 nuevaDireccion = Vector3.ProjectOnPlane(direccionHaciaJugador, Vector3.up); // Ignorar inclinación vertical
        Quaternion rotacionObjetivo = Quaternion.LookRotation(nuevaDireccion, Vector3.up);

        // Aplicar rotación al enemigo ajustando el offset de visión
        transform.rotation = rotacionObjetivo * Quaternion.Euler(0, 90, 0); // Girar 90 grados para que el lado esté hacia el jugador
    }

    private void AjustarVolumenAudio()
    {
        if (audioSource == null || jugador == null) return;

        // Calcular la distancia entre el enemigo y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Ajustar el volumen del audio en función de la distancia
        // La distancia máxima en la que se puede escuchar el sonido es 50 unidades
        float volumen = Mathf.Clamp01(1 - (distancia / 50f)); // El volumen disminuirá a medida que te alejas
        audioSource.volume = volumen;
    }

    private void OnDrawGizmos()
    {
        if (jugador != null)
        {
            // Dibujar la dirección hacia el jugador
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, jugador.position);
        }

        // Dibujar rayos para esquivar
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * distanciaDeteccion);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -45, 0) * transform.forward * distanciaDeteccion);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 45, 0) * transform.forward * distanciaDeteccion);
    }
}

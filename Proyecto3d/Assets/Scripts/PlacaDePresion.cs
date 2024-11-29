using UnityEngine;

public class PlacaDepresion : MonoBehaviour
{
    private bool yaActivada = false; // Evita activar la misma placa varias veces

    private void OnTriggerEnter(Collider other)
    {
        if (!yaActivada && other.CompareTag("Player")) // Verifica si es el jugador
        {
            yaActivada = true;

            // Notificar al PressurePlateManager
            FindObjectOfType<PlacaDepresionManager>().ActivarPlaca();

            // Cambiar color o animación para indicar que la placa ha sido activada (opcional)
            GetComponent<Renderer>().material.color = Color.green;
        }
    }
}

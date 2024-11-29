using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class ManagerController : MonoBehaviour
{
    // Cantidad de monedas en la escena
    private int totalMonedas;

    private void Start()
    {
        // Contar cuántas monedas hay en la escena al inicio
        totalMonedas = GameObject.FindGameObjectsWithTag("Coin").Length;
        Debug.Log("Total de monedas en el mapa: " + totalMonedas);
    }

    public void MonedaRecogida()
    {
        // Reducir el total de monedas al recoger una
        totalMonedas--;

        Debug.Log("Monedas restantes: " + totalMonedas);

        // Si no quedan monedas, cambiar a la escena final
        if (totalMonedas <= 0)
        {
            Debug.Log("¡Todas las monedas recogidas! Cambiando a la escena Final...");
            SceneManager.LoadScene("Final");
        }
    }
}

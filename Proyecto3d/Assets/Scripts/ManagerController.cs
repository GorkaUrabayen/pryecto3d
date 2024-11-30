using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class ManagerController : MonoBehaviour
{
    // Cantidad de monedas en la escena
    private int totalMonedas;

    // Método Start para contar cuántas monedas hay al inicio de la escena
    private void Start()
    {
        // Contar cuántas monedas hay en la escena al inicio
        totalMonedas = GameObject.FindGameObjectsWithTag("Coin").Length;
        Debug.Log("Total de monedas en el mapa: " + totalMonedas);
    }

    // Método que se llama cuando una moneda es recogida
    public void MonedaRecogida()
    {
        // Reducir el total de monedas al recoger una
        totalMonedas--;

        Debug.Log("Monedas restantes: " + totalMonedas);

        // Si no quedan monedas, cambiar a la escena final
        if (totalMonedas <= 0)
        {
            Debug.Log("¡Todas las monedas recogidas! Cambiando a la escena Final...");
          
           // CambiarEscena(); // no va asique la unica manera de vovler al inicio es muriendo, esto crea una parodia en la que no importa lo que hagas solo puedes morir
           SceneManager.LoadScene("Final"); // Cambia a la escena Final
        }
    }

    // Método que puede ser usado para comprobar si estamos en la escena final
    public void CambiarEscena()
    {
        // Obtener la escena actual
        Scene actualScene = SceneManager.GetActiveScene();

        if (actualScene.name == "Nivel1") // Si estamos en el nivel 1
        {
            // Cambiar a la escena final
            SceneManager.LoadScene("Final");
        }
        else if (actualScene.name == "Final") // Si estamos en la escena final
        {
            // Cambiar a la escena de inicio
            SceneManager.LoadScene("Inicio");
        }
    }
    
}

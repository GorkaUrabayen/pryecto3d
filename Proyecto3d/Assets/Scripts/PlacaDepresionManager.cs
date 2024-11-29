using UnityEngine;
using UnityEngine.SceneManagement;

public class PlacaDepresionManager : MonoBehaviour
{
    // Total de placas que deben activarse
    public int totalPlacas = 4;
    private int placasActivadas = 0;

    // Método llamado cuando una placa de presión es activada
    public void ActivarPlaca()
    {
        placasActivadas++;
        Debug.Log("Placas activadas: " + placasActivadas);

        // Comprobar si todas las placas están activadas
        if (placasActivadas >= totalPlacas)
        {
            Debug.Log("¡Todas las placas activadas! Cambiando de escena...");
            SceneManager.LoadScene("Inicio");
        }
    }
}

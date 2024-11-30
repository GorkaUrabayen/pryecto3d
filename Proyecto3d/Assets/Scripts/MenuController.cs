using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class MenuController : MonoBehaviour
//No se el porque no va en la escena Indromacion los botnes
{
    // Nombres de las escenas
    public string nombreEscenaNivel1 = "Nivel1"; // Escena del primer nivel
    public string nombreEscenaInformacion = "Informacion"; // Escena de información
    public string nombreEscenaInicio = "Inicio"; // Escena de inicio

    // Método para iniciar el juego
    public void Jugar()
    {
        Debug.Log("Cargando la escena: " + nombreEscenaNivel1);
        SceneManager.LoadScene(nombreEscenaNivel1);
    }

    // Método para mostrar la información
    public void MostrarInformacion()
    {
        Debug.Log("Cargando la escena de información: " + nombreEscenaInformacion);
        SceneManager.LoadScene(nombreEscenaInformacion);
    }

    // Método para regresar a la escena de inicio
    public void RegresarInicio()
    {
        Debug.Log("Regresando a la escena de inicio: " + nombreEscenaInicio);
        SceneManager.LoadScene(nombreEscenaInicio);
    }

    // Método para salir del juego
    public void Salir()
    {
        Debug.Log("Saliendo del juego.");
        Application.Quit(); // Cierra la aplicación
    }
}

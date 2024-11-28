using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float rotacionHaciaAtras = 180f; // Ángulo de rotación para mirar hacia atrás
    private float rotacionInicial; // Rotación original de la cámara

    private void Start()
    {
        // Guardar la rotación inicial de la cámara
        rotacionInicial = transform.eulerAngles.y;
    }

    private void Update()
    {
        // Verificar si se presiona la tecla "L"
        if (Input.GetKey(KeyCode.L))
        {
            // Rotar la cámara para mirar hacia atrás
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, player.eulerAngles.y + rotacionHaciaAtras, transform.eulerAngles.z);
        }
        else
        {
            // Restaurar la rotación original
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, player.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}

using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Configuración de Respawn")]
    [Tooltip("Altura límite para caer al vacío")]
    public float alturaMinima = -10f;

    [Tooltip("El objeto vacío que sirve como ancla de reaparición")]
    public Transform puntoDeRespawn;

    [Tooltip("Arrastra aquí el bloque Locomotor de la jerarquía de Meta")]
    public GameObject metaLocomotor;

    [Tooltip("Arrastra aquí tu cámara (CenterEyeAnchor o Main Camera)")]
    public Transform camaraJugador;

    private CharacterController _characterController;
    private Vector3 _velocidadCaida;
    private float _gravedad = -9.81f; // Fuerza de gravedad estándar

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Usamos LateUpdate para que la gravedad se calcule DESPUÉS de que el joystick de Meta mueva al jugador
    void LateUpdate()
    {
        if (_characterController != null && _characterController.enabled)
        {
            // 1. SINCRONIZACIÓN ROOM-SCALE: El cuerpo sigue a la cabeza
            if (camaraJugador != null)
            {
                Vector3 centroCamara = camaraJugador.localPosition;
                centroCamara.y = _characterController.center.y; // Mantiene los pies en el piso
                _characterController.center = centroCamara;
            }

            // 2. GRAVEDAD POR RAYCAST: Ignoramos la cápsula y usamos un láser para detectar el suelo real
            // Disparamos desde 20cm arriba de la base del jugador hacia abajo, con 40cm de largo
            Vector3 origenRayo = transform.position + (Vector3.up * 0.2f);
            bool tocandoSueloReal = Physics.Raycast(origenRayo, Vector3.down, 0.4f);

            if (!tocandoSueloReal)
            {
                // Si el láser no toca el piso, obligamos la caída
                _velocidadCaida.y += _gravedad * Time.deltaTime;
                _characterController.Move(_velocidadCaida * Time.deltaTime);
            }
            else
            {
                // Si el láser toca piso, mantenemos al jugador firme
                _velocidadCaida.y = -2f;
            }

            // 3. RESPAWN: Si caemos por debajo de la altura mínima, ejecutamos el salto
            if (transform.position.y < alturaMinima)
            {
                StartCoroutine(RutinaDeRespawn());
            }
        }
    }

    private IEnumerator RutinaDeRespawn()
    {
        // A. Apagar colisiones y motores de movimiento de Meta para evitar conflictos
        if (_characterController != null) _characterController.enabled = false;
        if (metaLocomotor != null) metaLocomotor.SetActive(false);

        // B. Calcular el desfase de la cabeza para aparecer exactamente en el punto correcto
        Vector3 desfaseCamara = Vector3.zero;
        if (camaraJugador != null)
        {
            desfaseCamara = camaraJugador.position - transform.position;
            desfaseCamara.y = 0; // Ignoramos el eje vertical
        }

        // C. Mover al jugador al punto seguro
        transform.position = puntoDeRespawn.position - desfaseCamara;
        transform.rotation = puntoDeRespawn.rotation;

        // D. Sincronizar el motor de físicas de Unity y hacer una pequeña pausa
        Physics.SyncTransforms();
        yield return new WaitForSeconds(0.1f);

        // E. Reiniciar la memoria de caída
        _velocidadCaida.y = 0f;

        // F. Encender el sistema de movimiento
        if (metaLocomotor != null) metaLocomotor.SetActive(true);
        if (_characterController != null) _characterController.enabled = true;
    }
}
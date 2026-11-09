using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public float alturaMinima = -10f;
    public Transform puntoDeRespawn;
    public GameObject metaLocomotor;

    [Tooltip("Arrastra aquí la cámara de tu jugador")]
    public Transform camaraJugador;

    private CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (transform.position.y < alturaMinima && _characterController.enabled)
        {
            StartCoroutine(RutinaDeRespawn());
        }
    }

    private IEnumerator RutinaDeRespawn()
    {
        if (_characterController != null) _characterController.enabled = false;
        if (metaLocomotor != null) metaLocomotor.SetActive(false);

        Vector3 desfaseCamara = Vector3.zero;
        if (camaraJugador != null)
        {
            desfaseCamara = camaraJugador.position - transform.position;
            desfaseCamara.y = 0; 
        }

        transform.position = puntoDeRespawn.position - desfaseCamara;
        transform.rotation = puntoDeRespawn.rotation;

        Physics.SyncTransforms();
        yield return new WaitForSeconds(0.1f);

        if (metaLocomotor != null) metaLocomotor.SetActive(true);
        if (_characterController != null) _characterController.enabled = true;
    }
}
using UnityEngine;

public class ParallexEffect : MonoBehaviour {
    public Camera cam;
    public Transform followTarget;
    Vector2 startingPosition; // Starting position for the parallex game object
    float startingPositionZ; // Starting Z position for the parallex game object

    Vector2 CameraMovementSinceStart => (Vector2)cam.transform.position - startingPosition; // Movement since the start of the game
    float ZDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    float ClippingPlane => (cam.transform.position.z + (ZDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float ParallaxFactor => Mathf.Abs(ZDistanceFromTarget) / ClippingPlane;
    void Start() {
        startingPosition = transform.position;
        startingPositionZ = transform.position.z;
    }

    void Update() {
        Vector2 newPosition = startingPosition + CameraMovementSinceStart * ParallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingPositionZ);
    }
}

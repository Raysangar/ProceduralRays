using UnityEngine;

public class RayMovementComponent : MonoBehaviour {

  [SerializeField]
  private float movementAmplitude;

  [SerializeField]
  private float movementFrequency;

  private float initialHeigth;
  private float randomOffset;

  private void Start () {
    initialHeigth = transform.localPosition.y;
    randomOffset = Random.value * 10;
  }

	void Update () {
    float newHeigth = Mathf.Sin (Time.time + randomOffset) * movementAmplitude + initialHeigth;
    Vector3 position = transform.localPosition;
    position.y = newHeigth;
    transform.localPosition = position;
	}
}

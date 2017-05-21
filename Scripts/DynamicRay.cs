using UnityEngine;

public class DynamicRay : RayComponent {

  private Vector3 currentInitialPosition, currentFinalPosition;

  protected override void Start () {
    base.Start ();
    generateRay ();
  }

  private void FixedUpdate () {
    currentInitialPosition = transform.InverseTransformPoint (initialPoint.position);
    currentFinalPosition = transform.InverseTransformPoint (finalPoint.position);
    if (!rayInfo.FinalPosition.Equals(currentFinalPosition) || !rayInfo.InitialPosition.Equals(currentInitialPosition)) {
      generateRay (ref currentInitialPosition, ref currentFinalPosition);
    }
  }
}

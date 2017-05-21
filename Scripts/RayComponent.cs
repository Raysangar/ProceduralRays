using UnityEngine;

[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public abstract class RayComponent : MonoBehaviour {
  [SerializeField]
  protected Transform initialPoint;

  [SerializeField]
  protected Transform finalPoint;

  [SerializeField]
  protected RayInfo rayInfo;

  protected MeshFilter meshFilter;
  protected MeshRenderer meshRenderer;

  private const string RaysCountMaterialProperty = "_RaysCount";

  protected void generateRay () {
    meshFilter.mesh = RayFactory.CreateRayMesh (rayInfo);
  }

  protected void generateRay (ref Vector3 initialPosition, ref Vector3 finalPosition) {
    rayInfo.InitialPosition = initialPosition;
    rayInfo.FinalPosition = finalPosition;
    generateRay ();
  }

  protected virtual void Awake () {
    meshFilter = GetComponent<MeshFilter> ();
    meshRenderer = GetComponent<MeshRenderer> ();
  }

  protected virtual void Start () {
    rayInfo.InitialPosition = transform.InverseTransformPoint (initialPoint.position);
    rayInfo.FinalPosition = transform.InverseTransformPoint (finalPoint.position);
    meshRenderer.material.SetInt (RaysCountMaterialProperty, rayInfo.RaysCount);
  }
}

using UnityEngine;

[System.Serializable]
public class RayInfo {
  [SerializeField]
  private float radius;

  [SerializeField]
  private int segmentsCount;

  [SerializeField]
  private float freedomDistanceOnEveryAxis;

  [SerializeField]
  private int raysCount;

  private Vector3 initialPosition;
  private Vector3 finalPosition;

  public float Radius {
    get { return radius; }
  }

  public int SegmentsCount {
    get { return segmentsCount; }
  }

  public float FreedomDistanceOnEveryAxis {
    get { return freedomDistanceOnEveryAxis; }
  }

  public int RaysCount {
    get { return raysCount; }
  }

  public Vector3 InitialPosition {
    get { return initialPosition; }
    set { initialPosition = value; }
  }

  public Vector3 FinalPosition {
    get { return finalPosition; }
    set { finalPosition = value; }
  }
}

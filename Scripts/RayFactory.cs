using System.Collections.Generic;
using UnityEngine;

public static class RayFactory {
  private static readonly List<Vector3> VertexNormalPerFace;
  private static readonly List<Vector2> BaseUVsPerVertex;
  private static readonly Vector2 UVOffset;

  private const int RayFacesCount = 4;
  private const int RayVertexPerFace = 4;

  private static List<Vector3> vertices;
  private static List<int> indices;
  private static List<Vector3> normals;
  private static List<Vector2> uvs;

  public static Mesh CreateRayMesh (RayInfo rayInfo) {
    Mesh mesh = new Mesh ();

    float segmentLength = (rayInfo.FinalPosition - rayInfo.InitialPosition).magnitude / rayInfo.SegmentsCount;
    Vector3 rayDirection = (rayInfo.FinalPosition - rayInfo.InitialPosition).normalized;

    Vector3[] segmentPositions = new Vector3[rayInfo.SegmentsCount + 1];
    segmentPositions[0] = rayInfo.InitialPosition;
    segmentPositions[rayInfo.SegmentsCount] = rayInfo.FinalPosition;

    for (int iRay = 0; iRay < rayInfo.RaysCount; ++iRay) {
      for (int iBifurcation = 1; iBifurcation < rayInfo.SegmentsCount; ++iBifurcation) {

        Vector3 segmentPosition = rayInfo.InitialPosition + rayDirection * segmentLength * iBifurcation;
        segmentPosition.y = segmentPosition.y + Random.Range (-rayInfo.FreedomDistanceOnEveryAxis, rayInfo.FreedomDistanceOnEveryAxis);
        segmentPosition.z = segmentPosition.z + Random.Range (-rayInfo.FreedomDistanceOnEveryAxis, rayInfo.FreedomDistanceOnEveryAxis);

        segmentPositions[iBifurcation] = segmentPosition;
      }

      for (int iSegment = 0; iSegment < segmentPositions.Length - 1; ++iSegment) {
        createRaySegment (segmentPositions[iSegment], segmentPositions[iSegment + 1], rayInfo.Radius, iRay, iSegment, rayInfo.SegmentsCount);
      }
    }

    mesh.SetVertices (vertices);
    mesh.SetIndices (indices.ToArray (), MeshTopology.Triangles, 0);
    mesh.SetNormals (normals);
    mesh.SetUVs (0, uvs);

    cleanTempVariables ();

    return mesh;
  }

  static RayFactory () {
    VertexNormalPerFace = new List<Vector3> () {
      Vector3.back,
      Vector3.up,
      Vector3.forward,
      Vector3.down
    };

    BaseUVsPerVertex = new List<Vector2> () {
      new Vector2 (0, 0),
      new Vector2 (1, 0),
      new Vector2 (0, 1),
      new Vector2 (1, 1)
    };

    UVOffset = new Vector2 (2, 2);

    vertices = new List<Vector3> ();
    indices = new List<int> ();
    normals = new List<Vector3> ();
    uvs = new List<Vector2> ();
  }

  private static void createRaySegment (Vector3 initialPosition, Vector3 finalPosition, float radius, int rayIndex, int segmentIndex, int segmentsCount) {
    vertices.Add (new Vector3 (initialPosition.x, initialPosition.y - radius, initialPosition.z - radius));
    vertices.Add (new Vector3 (finalPosition.x, finalPosition.y - radius, finalPosition.z - radius));
    vertices.Add (new Vector3 (initialPosition.x, initialPosition.y + radius, initialPosition.z - radius));
    vertices.Add (new Vector3 (finalPosition.x, finalPosition.y + radius, finalPosition.z - radius));

    vertices.Add (new Vector3 (initialPosition.x, initialPosition.y + radius, initialPosition.z - radius));
    vertices.Add (new Vector3 (finalPosition.x, finalPosition.y + radius, finalPosition.z - radius));
    vertices.Add (new Vector3 (initialPosition.x, initialPosition.y + radius, initialPosition.z + radius));
    vertices.Add (new Vector3 (finalPosition.x, finalPosition.y + radius, finalPosition.z + radius));

    vertices.Add (new Vector3 (finalPosition.x, finalPosition.y - radius, finalPosition.z + radius));
    vertices.Add (new Vector3 (initialPosition.x, initialPosition.y - radius, initialPosition.z + radius));
    vertices.Add (new Vector3 (finalPosition.x, finalPosition.y + radius, finalPosition.z + radius));
    vertices.Add (new Vector3 (initialPosition.x, initialPosition.y + radius, initialPosition.z + radius));

    vertices.Add (new Vector3 (finalPosition.x, finalPosition.y - radius, finalPosition.z - radius));
    vertices.Add (new Vector3 (initialPosition.x, initialPosition.y - radius, initialPosition.z - radius));
    vertices.Add (new Vector3 (finalPosition.x, finalPosition.y - radius, finalPosition.z + radius));
    vertices.Add (new Vector3 (initialPosition.x, initialPosition.y - radius, initialPosition.z + radius));

    for (int iFace = 0; iFace < RayFacesCount; ++iFace) {
      int faceOffset = iFace * RayFacesCount;
      int segmentOffset = segmentIndex * RayFacesCount * RayVertexPerFace;
      int rayOffset = rayIndex * RayFacesCount * RayVertexPerFace * segmentsCount;
      int offset = faceOffset + segmentOffset + rayOffset;


      indices.Add (offset);
      indices.Add (2 + offset);
      indices.Add (1 + offset);

      indices.Add (2 + offset);
      indices.Add (3 + offset);
      indices.Add (1 + offset);

      for (int iVertex = 0; iVertex < RayVertexPerFace; ++iVertex) {
        normals.Add (VertexNormalPerFace[iFace]);
        uvs.Add (BaseUVsPerVertex[iVertex] + UVOffset * rayIndex);
      }
    }
  }

  private static void cleanTempVariables () {
    vertices.Clear ();
    indices.Clear ();
    normals.Clear ();
    uvs.Clear ();
  }
}

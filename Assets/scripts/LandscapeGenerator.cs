using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public class LandscapeGenerator : MonoBehaviour {

	public float northSpan = 10;
	public float westSpan = 10;

	public int northSegments = 100;
	public int westSegments = 100;

	private Mesh mMesh = null;

	private int VertexCount {
		get { return (1 + northSegments) * (1 + westSegments); }
	}

	private int TriangleCount {
		get { return (northSegments * westSegments * 2); }
	}

	[ContextMenu ("Generate")]
	void DoGenerate () {
		mMesh = null;
		Generate ();
	}

	[ContextMenu ("Clear Geometry")]
	void ClearGeometry () {
		mMesh = null;
		var mf = GetComponent<MeshFilter>();
		mf.mesh = null;
	}

	void Start () {
		Generate ();
	}

	// Use this for initialization
	private void Generate () {
		if (mMesh != null) {
			return;
		}
		Debug.Log ("Generating Mesh");

		var mf = GetComponent<MeshFilter>();
		mMesh = new Mesh();
		mf.mesh = mMesh;
		const int TRI_VERTS = 3;
		var vertices = new Vector3[VertexCount];
		var tri = new int[TriangleCount * TRI_VERTS];
		var normals = new Vector3[VertexCount];
		var uv = new Vector2[VertexCount];

		var westStep = westSpan / westSegments;
		var northStep = northSpan / northSegments;
		var uStep = 1.0f / westSegments;
		var vStep = 1.0f / northSegments;
		var index = 0;
		var triIndex = 0;
		for (int x = 0; x <= westSegments; ++x) {
			var generateTris = x < westSegments;
			for (int y = 0; y <= northSegments; ++y) {
				vertices [index] = new Vector3 (x * westStep, 0, y * northStep);
				normals [index] = Vector3.up;
				uv [index] = new Vector2 (x * uStep, y * vStep);

				if (generateTris && y < northSegments) {
					tri [triIndex + 0] = index;
					tri [triIndex + 1] = index + 1;
					tri [triIndex + 2] = index + northSegments + 1;
					triIndex += TRI_VERTS;
					tri [triIndex + 0] = index + 1;
					tri [triIndex + 1] = index + northSegments + 2;
					tri [triIndex + 2] = index + northSegments + 1;
					triIndex += TRI_VERTS;
				}
				++index;
			}
		}
		mMesh.vertices = vertices;
		mMesh.triangles = tri;
		mMesh.normals = normals;
		mMesh.uv = uv;

		mMesh.RecalculateBounds();
		mMesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

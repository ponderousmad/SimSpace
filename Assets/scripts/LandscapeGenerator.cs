using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public class LandscapeGenerator : MonoBehaviour {

	public float north = 10;
	public float west = 10;

	private Mesh mMesh = null;

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
		var vertices = new Vector3[4];

		vertices[0] = new Vector3(0, 0, 0);
		vertices[1] = new Vector3(west, 0, 0);
		vertices[2] = new Vector3(0, 0, north);
		vertices[3] = new Vector3(west, 0, north);

		mMesh.vertices = vertices;

		var tri = new int[6];

		//  Lower left triangle.
		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;

		//  Upper right triangle.   
		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;

		mMesh.triangles = tri;

		var normals = new Vector3[4];

		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		normals[3] = -Vector3.forward;

		mMesh.normals = normals;

		var uv = new Vector2[4];

		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);

		mMesh.uv = uv;

		mMesh.RecalculateBounds();
		mMesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

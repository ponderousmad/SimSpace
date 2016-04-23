using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public class LandscapeGenerator : MonoBehaviour {

	public float northSpan = 10;
	public float eastSpan = 10;

	public float northStart = 0;
	public float eastStart = 0;

	public int northSegments = 100;
	public int eastSegments = 100;

	public float heightScale = 0.001f;

	public TextAsset altitudeData;

	private Mesh mMesh = null;

	private int VertexCount {
		get { return (1 + northSegments) * (1 + eastSegments); }
	}

	private int TriangleCount {
		get { return (northSegments * eastSegments * 2); }
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

		var eastStep = eastSpan / eastSegments;
		var northStep = northSpan / northSegments;

		var heightSum = new float[eastSegments + 1, northSegments + 1];
		var heightCount = new int[eastSegments + 1, northSegments + 1];

		var count = 0;
		var minEast = float.MaxValue;
		var maxEast = float.MinValue;
		var minNorth = float.MaxValue;
		var maxNorth = float.MinValue;
		var minHeight = float.MaxValue;
		var maxHeight = float.MinValue;

		foreach (var line in altitudeData.text.Split('\r')) {
			++count;

			if (count > 1) {
				var parts = line.Split(',');
				float east, north, height;
				if (!float.TryParse (parts [0].Trim (), out east)) {
					Debug.Log ("Could not parse: " + parts [0] + " at " + count);
					break;
				}
				if (!float.TryParse (parts [1].Trim (), out north)) {
					Debug.Log ("Could not parse: " + parts [1] + " at " + count);
					break;
				}
				if (!float.TryParse (parts [2].Trim (), out height)) {
					Debug.Log ("Could not parse: " + parts [2] + " at " + count);
					break;
				}
				// Debug.Log (string.Format("{0}, {1}, {2}", east, north, height));

				minEast = Mathf.Min (minEast, east);
				maxEast = Mathf.Max (maxEast, east);
				minNorth = Mathf.Min (minNorth, north);
				maxNorth = Mathf.Max (maxNorth, north);
				minHeight = Mathf.Min (minHeight, height);
				maxHeight = Mathf.Max (maxHeight, height);

				if (east < eastStart || eastStart + eastSpan + eastStep < east) {
					continue;
				}

				if (north < northStart || northStart + northSpan + northStep < north) {
					continue;
				}

				var x = (int) ((east - eastStart) / eastStep);
				var y = (int) ((north - northStart) / northStep);
				heightSum [x, y] += height;
				heightCount [x, y] += 1;
			}
		}

		Debug.Log ("Lines: " + count);
		Debug.Log (string.Format ("{0} - {1}", minEast, maxEast - minEast));
		Debug.Log (string.Format ("{0} - {1}", minNorth, maxNorth - minNorth));
		Debug.Log (string.Format ("{0} - {1}", minHeight, maxHeight - minHeight));

		var mf = GetComponent<MeshFilter>();
		mMesh = new Mesh();
		mf.mesh = mMesh;
		const int TRI_VERTS = 3;
		var vertices = new Vector3[VertexCount];
		var tri = new int[TriangleCount * TRI_VERTS];
		var normals = new Vector3[VertexCount];
		var uv = new Vector2[VertexCount];

		var uStep = 1.0f / eastSegments;
		var vStep = 1.0f / northSegments;
		var index = 0;
		var triIndex = 0;
		for (int x = 0; x <= eastSegments; ++x) {
			var generateTris = x < eastSegments;
			float? lastHeight = null;
			for (int y = 0; y <= northSegments; ++y) {
				var heights = heightCount [x, y];
				if (heights > 0) {
					lastHeight = heightSum [x, y] / heights;
				} else if (!lastHeight.HasValue) {
					var offset = 0;
					do {
						++offset;
						if (y + offset >= northSegments) {
							heights = 1;
							offset = 0;
						} else {
							heights = heightCount[x, y + offset];
						}
					} while (heights == 0);
					lastHeight = heightSum[x, y + offset] / heights;
				}
				vertices [index] = new Vector3 (x * eastStep, lastHeight.Value * heightScale, y * northStep);
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

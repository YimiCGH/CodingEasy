using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CastShadowMode = UnityEngine.Rendering.ShadowCastingMode;

public class MergeMesh : MonoBehaviour
{
    public Material Mat;
    public StaticEditorFlags Flags;
    public bool DestroyChildren;
    public bool CreateMeshCollider;

    public CastShadowMode CastShadow;

    public bool ReciveShadows;
    public ReceiveGI GI;
    public bool ReciveGI;

    private Mesh _combineMesh;
    void Awake()
    {
        Debug.Log("Combine meshes");
        DoMerge();
    }

    private void OnDestroy()
    {
        if (_combineMesh != null) {
            DestroyImmediate(_combineMesh);
            _combineMesh = null;
        }
    }

    private void DoMerge() {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        Matrix4x4 matrix = transform.worldToLocalMatrix;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = matrix * meshFilters[i].transform.localToWorldMatrix;
        }

        _combineMesh = new Mesh();
        _combineMesh.CombineMeshes(combineInstances);

        var meshfilter = gameObject.AddComponent<MeshFilter>();
        meshfilter.sharedMesh = _combineMesh;

        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material = Mat;
        renderer.shadowCastingMode = CastShadow;
        renderer.receiveShadows = ReciveShadows;

        bool contributeGI = ((int)Flags & (int)StaticEditorFlags.ContributeGI) != 0;
        if (contributeGI)
        {
            renderer.receiveGI = GI;
        }
        


        if (CreateMeshCollider) {
            var meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = _combineMesh;            
        }

        if (DestroyChildren)
        {
            for (int i = 0; i < meshFilters.Length; i++)
            {
                DestroyImmediate(meshFilters[i].gameObject);
            }
        }
        else {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Grid.HexagonGridGeneration
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Hexagon : MonoBehaviour
    {
        public float SideLength = 20f;
        public Vector3 HexPosition;
        public bool Rendered
        {
            get => _rendered;
            set
            {
                if (_rendered)
                    return;
                
                if (value)
                    Triangulate();
                else 
                    Clear();
                _rendered = value;
            }
        }

        private bool _rendered;
        private Mesh _hexagon;
        private List<Vector3> _vertexList;
        private List<int> _triList;

        private List<int> _subMeshTris;

        public void Awake()
        {
            _hexagon = new Mesh();
            GetComponent<MeshFilter>().mesh = _hexagon;
            _hexagon.name = "Hexagon";

            _vertexList = new List<Vector3>();
            _triList = new List<int>();
            _subMeshTris = new List<int>();
        }

        public void Triangulate()
        {
            Clear();
            
            var center = this.transform.localPosition;
            for (var i = 0; i < 6; ++i)
            {
                AddTriangle(center, center + HexTile.Points[i], center + HexTile.Points[i + 1]);
                var yOffset = -Vector3.up * SideLength;
                SubMeshAddTriangle(center + HexTile.Points[i], center + HexTile.Points[i] + yOffset, center + HexTile.Points[i + 1]);
                SubMeshAddTriangle(center + HexTile.Points[i] + yOffset, center + HexTile.Points[i + 1] + yOffset, center + HexTile.Points[i + 1]);
            }
            
            _hexagon.vertices = _vertexList.ToArray();
            _hexagon.triangles = _triList.ToArray();

            _hexagon.subMeshCount = 2;
            _hexagon.SetTriangles(_subMeshTris, 1);
            _hexagon.RecalculateNormals();
        }

        public void Clear()
        {
            _hexagon.Clear();
            _triList.Clear();
            _vertexList.Clear();
            _subMeshTris.Clear();
        }

        private int AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var index = _vertexList.Count;
            _vertexList.Add(v1);
            _vertexList.Add(v2);
            _vertexList.Add(v3);
            _triList.Add(index);
            _triList.Add(index + 1);
            _triList.Add(index + 2);

            return index;
        }

        private void SubMeshAddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var index = AddTriangle(v1, v2, v3);
            _subMeshTris.AddRange(new[]{index, index + 1, index + 2});
        }
    }
}
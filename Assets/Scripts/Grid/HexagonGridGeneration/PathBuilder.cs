using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;
using Utility.Math;

namespace Grid.HexagonGridGeneration {
    public class PathBuilder : SerializedMonoBehaviour
    {
        public int MaxSize = 10;
        public GameObject HexagonPrefab;

        private Dictionary<Vector3, Hexagon> Hexagons = new Dictionary<Vector3, Hexagon>();

        public Hexagon Start;
        public Hexagon End;
        
        private List<Vector3> GenRandomPoints(int count, int size)
        {
            var positions = new List<Vector3>();
            for (var i = 1; i <= count; ++i)
            {
                var rand = new Random();
                var x = rand.Next(-size, size);
                const int y = 0;
                var z = rand.Next(-size, size);

                var iterations = 0;
                bool placed = true;
                // Ensure tile is far enough away from existing tiles
                while (positions.Any(v => !Comparison.IntDistanceGreaterThan((int)v.x, x, 1) &&
                                          !Comparison.IntDistanceGreaterThan((int)v.z, z, 1)) && iterations < 100)
                {
                    ++iterations;
                    if (iterations == 100)
                    {
                        placed = false;
                        break;
                    }
                    x = rand.Next(-size, size);
                    z = rand.Next(-size, size);
                }
                if (placed)
                    positions.Add(new Vector3Int(x, y, z));
            }
            return positions;
        }
        [Button]
        public void GenerateHexGrid()
        {
            Start = null;
            End = null;
            
            if (Hexagons.Any())
            {
                while (Hexagons.Any())
                {
                    var hex = Hexagons.First();
                    if (hex.Value != null)
                        DestroyImmediate(hex.Value.gameObject);
                    Hexagons.Remove(hex.Key);
                }
            }

            if (transform.childCount > 0)
            {
                while (transform.childCount > 0)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }
            
            for (var x = -MaxSize; x < MaxSize; ++x)
            {
                for (var z = -MaxSize; z < MaxSize; ++z)
                {
                    var point = new Vector3Int(x, 0, z);
                    var hex = CreateHexagon(HexTile.GetCoordinateFromOffset(point), true);
                    hex.HexPosition = point;
                    Hexagons.Add(point, hex);
                }
            }
            var points = GenRandomPoints(MaxSize + 2, MaxSize);
            var rand = new Random();
            foreach (var point in points)
            {
                Hexagons[point].Rendered = false;
                var tilesToHide = rand.Next(3, 10);
                for (var i = 0; i < tilesToHide; ++i)
                {
                    var randomPoint = point + new Vector3(rand.Next(-2, 2), 0, rand.Next(-2, 2));
                    if (Hexagons.ContainsKey(randomPoint))
                        Hexagons[randomPoint].Rendered = false;
                }

            }
        }

        public void BuildPath(Vector3 start, Vector3 finish)
        {
            var xPoints = Comparison.GetRangeBetween((int)start.x, (int)finish.x);
            var zPoints = Comparison.GetRangeBetween((int)start.z, (int)finish.z);
            var longerAxis = xPoints.Count > zPoints.Count ? xPoints : zPoints;
            
        }

        private Hexagon CreateHexagon(Vector3 position, bool render = false)
        {
            var hex = Instantiate(HexagonPrefab, transform).GetComponent<Hexagon>();
            hex.transform.localPosition = position;
            if (!Application.isPlaying && render)
                hex.Awake();
            
            if (render)
                hex.Triangulate();

            return hex;
        }
    }
}

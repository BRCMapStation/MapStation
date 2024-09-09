using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Tools.Runtime {
    public class CopterNavMeshBuilder {
        public float CopterHeight = 8f;
        public float MinHeight = 8f;
        public LayerMask LayerMask;
        public Vector3 Origin = Vector3.zero;
        public int Width = 10;
        public int Height = 10;
        public float TileSize = 10f;
        public Mesh Mesh = null;

        private float GetHeightAt(Vector3 position) {
            var height = MinHeight;
            if (Physics.Raycast(position, Vector3.down, out var hit, Mathf.Infinity, LayerMask)) {
                height = hit.point.y;
                height += CopterHeight;
                if (height < MinHeight)
                    height = MinHeight;
            } else
                height = MinHeight;
            return height;
        }

        public void Build() {
            if (Mesh == null)
                Mesh = new Mesh();
            var vertices = new List<Vector3>();
            var triangles = new List<int>();

            for (var i = 0; i < Width; i++) {
                for (var n = 0; n < Height; n++) {
                    var pos = Origin;
                    pos.x += i * TileSize;
                    pos.z += n * TileSize;

                    var posArray = new Vector3[4];
                    // Top Left
                    posArray[0] = pos;
                    // Top Right
                    posArray[1] = pos + new Vector3(TileSize, 0, 0);
                    // Bottom Left
                    posArray[2] = pos + new Vector3(0, 0, TileSize);
                    // Bottom Right
                    posArray[3] = pos + new Vector3(TileSize, 0, TileSize);

                    for (var j = 0; j < posArray.Length; j++) {

                        var myHeight = GetHeightAt(posArray[j]);

                        var adjacentHeights = new float[4] {
                            GetHeightAt(posArray[j] + new Vector3(0, 0, TileSize)),
                            GetHeightAt(posArray[j] + new Vector3(TileSize, 0, 0)),
                            GetHeightAt(posArray[j] + new Vector3(0, 0, -TileSize)),
                            GetHeightAt(posArray[j] + new Vector3(-TileSize, 0, 0))
                        };

                        var averageHeight = myHeight;
                        var highestHeight = myHeight;
                        for(var k = 0; k < adjacentHeights.Length; k++) {
                            var height = adjacentHeights[k];
                            if (height > highestHeight)
                                highestHeight = height;
                        }

                        if (highestHeight > myHeight)
                            averageHeight = (myHeight + highestHeight) / 2;

                        posArray[j].y = averageHeight;
                    }

                    var baseVertexIndex = vertices.Count;

                    vertices.Add(posArray[2]);
                    vertices.Add(posArray[1]);
                    vertices.Add(posArray[0]);

                    vertices.Add(posArray[1]);
                    vertices.Add(posArray[2]);
                    vertices.Add(posArray[3]);

                    triangles.Add(baseVertexIndex);
                    triangles.Add(baseVertexIndex + 1);
                    triangles.Add(baseVertexIndex + 2);

                    triangles.Add(baseVertexIndex + 3);
                    triangles.Add(baseVertexIndex + 4);
                    triangles.Add(baseVertexIndex + 5);
                }
            }

            Mesh.SetVertices(vertices);
            Mesh.SetTriangles(triangles, 0);
        }
    }
}

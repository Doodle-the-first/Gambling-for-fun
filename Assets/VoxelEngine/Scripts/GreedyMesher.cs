using System.Collections.Generic;
using UnityEngine;

// Lightweight greedy mesher for axis-aligned cubes
// This is a simple implementation intended for prototype use.
// Reference idea: Mikola Lysenko's greedy meshing (adapted).

namespace VoxelEngine {
    public static class GreedyMesher {
        public struct Quad { public Vector3[] verts; public int[] tris; public Vector2[] uvs; public Color color; }

        // Generate mesh from a 3D int array of voxel types (0 = empty)
        public static Mesh GenerateMesh(int[,,] voxels, int sizeX, int sizeY, int sizeZ) {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Color> cols = new List<Color>();

            // Simple naive approach: iterate faces and add quads when visible.
            for (int x = 0; x < sizeX; x++) {
                for (int y = 0; y < sizeY; y++) {
                    for (int z = 0; z < sizeZ; z++) {
                        if (voxels[x,y,z] == 0) continue;
                        // Check 6 neighbors; if neighbor empty or out of bounds, add face
                        AddFaceIfVisible(voxels, sizeX,sizeY,sizeZ, x,y,z, Vector3.forward, verts, tris, uvs, cols);
                        AddFaceIfVisible(voxels, sizeX,sizeY,sizeZ, x,y,z, Vector3.back, verts, tris, uvs, cols);
                        AddFaceIfVisible(voxels, sizeX,sizeY,sizeZ, x,y,z, Vector3.left, verts, tris, uvs, cols);
                        AddFaceIfVisible(voxels, sizeX,sizeY,sizeZ, x,y,z, Vector3.right, verts, tris, uvs, cols);
                        AddFaceIfVisible(voxels, sizeX,sizeY,sizeZ, x,y,z, Vector3.up, verts, tris, uvs, cols);
                        AddFaceIfVisible(voxels, sizeX,sizeY,sizeZ, x,y,z, Vector3.down, verts, tris, uvs, cols);
                    }
                }
            }

            Mesh mesh = new Mesh();
            mesh.indexFormat = verts.Count > 65000 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.SetUVs(0, uvs);
            mesh.SetColors(cols);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        static void AddFaceIfVisible(int[,,] voxels, int sx,int sy,int sz, int x,int y,int z, Vector3 dir, List<Vector3> verts, List<int> tris, List<Vector2> uvs, List<Color> cols) {
            int nx = x + (int)dir.x;
            int ny = y + (int)dir.y;
            int nz = z + (int)dir.z;
            bool visible = false;
            if (nx < 0 || nx >= sx || ny < 0 || ny >= sy || nz < 0 || nz >= sz) visible = true;
            else if (voxels[nx,ny,nz] == 0) visible = true;
            if (!visible) return;

            int vStart = verts.Count;
            // Create a unit cube face at position x,y,z for given direction
            Vector3 p = new Vector3(x,y,z);
            float s = 1f;
            if (dir == Vector3.forward) {
                verts.Add(p + new Vector3(0,0,s));
                verts.Add(p + new Vector3(1,0,s));
                verts.Add(p + new Vector3(1,1,s));
                verts.Add(p + new Vector3(0,1,s));
            } else if (dir == Vector3.back) {
                verts.Add(p + new Vector3(1,0,0));
                verts.Add(p + new Vector3(0,0,0));
                verts.Add(p + new Vector3(0,1,0));
                verts.Add(p + new Vector3(1,1,0));
            } else if (dir == Vector3.left) {
                verts.Add(p + new Vector3(0,0,0));
                verts.Add(p + new Vector3(0,0,1));
                verts.Add(p + new Vector3(0,1,1));
                verts.Add(p + new Vector3(0,1,0));
            } else if (dir == Vector3.right) {
                verts.Add(p + new Vector3(1,0,1));
                verts.Add(p + new Vector3(1,0,0));
                verts.Add(p + new Vector3(1,1,0));
                verts.Add(p + new Vector3(1,1,1));
            } else if (dir == Vector3.up) {
                verts.Add(p + new Vector3(0,1,1));
                verts.Add(p + new Vector3(1,1,1));
                verts.Add(p + new Vector3(1,1,0));
                verts.Add(p + new Vector3(0,1,0));
            } else { // down
                verts.Add(p + new Vector3(0,0,0));
                verts.Add(p + new Vector3(1,0,0));
                verts.Add(p + new Vector3(1,0,1));
                verts.Add(p + new Vector3(0,0,1));
            }

            tris.Add(vStart + 0);
            tris.Add(vStart + 1);
            tris.Add(vStart + 2);
            tris.Add(vStart + 0);
            tris.Add(vStart + 2);
            tris.Add(vStart + 3);

            uvs.Add(new Vector2(0,0)); uvs.Add(new Vector2(1,0)); uvs.Add(new Vector2(1,1)); uvs.Add(new Vector2(0,1));
            cols.Add(Color.white); cols.Add(Color.white); cols.Add(Color.white); cols.Add(Color.white);
        }
    }
}

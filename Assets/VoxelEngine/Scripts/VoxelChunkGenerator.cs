using UnityEngine;

// Generates a single chunk mesh at Start for quick prototype

namespace VoxelEngine {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VoxelChunkGenerator : MonoBehaviour {
        public int sizeX = 32;
        public int sizeY = 16;
        public int sizeZ = 32;
        public int[,,] voxels;

        void Start() {
            voxels = new int[sizeX, sizeY, sizeZ];
            // Simple terrain: fill voxels below a height function
            for (int x=0;x<sizeX;x++){
                for (int z=0;z<sizeZ;z++){
                    int h = Mathf.FloorToInt((Mathf.PerlinNoise(x*0.1f,z*0.1f) * (sizeY-4)) + 2);
                    for (int y=0;y<sizeY;y++){
                        voxels[x,y,z] = y <= h ? 1 : 0;
                    }
                }
            }

            Mesh mesh = GreedyMesher.GenerateMesh(voxels, sizeX, sizeY, sizeZ);
            MeshFilter mf = GetComponent<MeshFilter>();
            mf.mesh = mesh;
            MeshRenderer mr = GetComponent<MeshRenderer>();
            // Simple material
            mr.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit")) { color = Color.white };

            // Position chunk so that origin is near center
            transform.position = new Vector3(-sizeX/2f, 0, -sizeZ/2f);
        }
    }
}

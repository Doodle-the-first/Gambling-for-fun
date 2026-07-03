This folder contains a tiny voxel prototype used by the unity-voxel branch.

How to run the prototype:
1. Open Unity Hub and install Unity 2022.3 LTS (recommended). Import this repository as a Unity project root.
2. In Package Manager, install Universal Render Pipeline (URP) and assign a URP pipeline asset (optional but recommended for mobile builds).
3. Create an empty Scene. Add an empty GameObject named "Chunk" and attach the VoxelChunkGenerator script. Also add a MeshFilter and MeshRenderer.
4. Create a Capsule, attach a CharacterController and the PlayerController script. Position above the chunk and press Play.

Notes:
- The included VoxelChunkGenerator uses a simple greedy-mesh-like naive mesh generator for prototype. It is enough to validate movement and visuals. Replace with a multi-threaded greedy mesher for performance and chunking.
- On mobile, configure input and quality settings for 30 FPS.

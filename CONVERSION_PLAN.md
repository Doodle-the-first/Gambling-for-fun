# Conversion plan — Unity voxel conversion

This file documents the initial plan, implementation notes, and next steps for converting the existing game to a Unity-based voxel 3D version.

Goals & constraints
- Engine: Unity (target: 2022.3 LTS)
- Style: Voxel (chunked, greedy meshing)
- Platforms: Desktop, WebGL (partial), Android, iOS
- Mobile performance target: 30 FPS on typical devices
- Only open-source dependencies

High-level steps
1. Project skeleton (this branch)
   - Add Unity-compatible project files (Packages manifest) and scaffolding code under Assets/VoxelEngine.
   - Add .gitattributes recommending Git LFS for large assets.
2. Core voxel engine
   - Chunked voxel world (e.g., 16x16x16 or 32x32x32 chunks).
   - Greedy meshing to reduce triangles and draw calls.
   - Multi-threaded mesh generation worker (Unity Job System + Burst recommended later).
3. Graphics
   - Use URP (Universal Render Pipeline) with unlit/flat shaders or vertex-colored materials for stylized voxel look.
   - Use light probes/baked lighting for mobile performance; avoid many realtime lights.
4. Gameplay port
   - Translate game logic into C# preserving rules; redesign UI as needed.
   - Implement player controller, camera, and input (touch controls for mobile).
5. Asset pipeline
   - Use MagicaVoxel for authored voxel models (export .vox -> obj) or create block-based models procedurally.
   - Track large assets with Git LFS.
6. Testing & tuning
   - Profiling on target mobile devices; adjust chunk size, LOD distance, and occlusion.

Open-source references & choices
- Greedy meshing algorithm: original concept by Mikola Lysenko (portable; many MIT/BSD ports exist). We include a small C# implementation in Assets/VoxelEngine.
- Avoid paid asset store packages. Later we can consider open-source packages if needed.

Prototype included in this branch
- A simple chunk generator that builds a single chunk with basic terrain and uses a greedy mesher to produce a Mesh.
- A tiny player controller to move around the chunk.

Next developer tasks (after merging prototype)
- Import URP and configure quality settings for mobile.
- Replace prototype materials with URP unlit/vertex color shader.
- Implement chunk streaming, background mesh generation, and saving/loading.
- Add touch on-screen controls and gamepad support.

Performance tips
- Use greedy meshing to reduce triangles dramatically.
- Batch chunks and use GPU instancing for repeated meshes.
- Limit view distance and implement occlusion/face culling per chunk.
- Use texture atlases for any textured voxels.

If you want, I can continue by:
- Adding a Unity ProjectSettings stub and a basic URP setup, or
- Implementing chunk streaming and basic mobile input next.


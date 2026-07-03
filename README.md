# Gambling-for-fun — Unity voxel conversion branch

This branch contains the initial Unity project skeleton and a minimal open-source voxel prototype to get the 3D conversion started.

What is included in this commit:
- .gitignore (Unity)
- .gitattributes (recommendations for Git LFS)
- CONVERSION_PLAN.md (detailed conversion plan and next steps)
- Packages/manifest.json (minimal so Unity recognizes a package manifest)
- Assets/VoxelEngine/Scripts: simple voxel engine scaffold (Chunk generator, greedy mesher, player controller)
- Assets/VoxelEngine/README.md (how to open and run the prototype)

Notes:
- This is a lightweight prototype focused on portability and open-source implementation. Use Git LFS for large binary assets (textures, models, audio).
- Target Unity LTS 2022.3 (URP recommended for mobile). See CONVERSION_PLAN.md for details.

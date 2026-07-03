# WebGL hosting instructions

This file explains how the automatic WebGL build & deploy works and what to do to enable GitHub Pages.

1) The GitHub Actions workflow (/.github/workflows/unity-webgl-build.yml) builds the project for WebGL when you push to the unity-voxel branch or when manually triggered.
2) The workflow attempts to run the Unity editor in batch mode and execute the BuildWebGL.PerformBuild editor method to create the WebGL build into Builds/WebGL.
3) After a successful build the workflow deploys the contents of Builds/WebGL to the gh-pages branch using peaceiris/actions-gh-pages.

Notes & troubleshooting
- The workflow installs Unity using game-ci/unity-installer. If the specific Unity version is unavailable on the runner, update the unityVersion in the workflow to a concrete version you have or that the installer supports.
- WebGL builds can be large. Check the Actions build logs and the uploaded artifact if the build fails.
- After the gh-pages branch is populated, enable GitHub Pages in the repository Settings → Pages if not auto-enabled. The site will be available at:
  https://Doodle-the-first.github.io/Gambling-for-fun/

If the automatic build fails due to Unity installer differences, you can also run a local build:
- Open the project in Unity 2022.3 LTS locally and run Tools → Build WebGL (added by Assets/Editor/BuildWebGL.cs), or execute:
  Unity -batchmode -projectPath . -executeMethod BuildWebGL.PerformBuild -quit


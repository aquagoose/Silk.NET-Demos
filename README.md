# Shadow map sample
An easy to understand shadow map sample for [Silk.NET](https://github.com/dotnet/Silk.NET) (OpenGL) and [Cubic.Graphics](https://github.com/IsometricSoftware/Cubic/tree/Cubic.Next/Cubic.Graphics).

<img src="https://i.rollbot.net/shadowdemo1.png" alt="Demo 1" height="256" /> <img src="https://i.rollbot.net/shadowdemo2.png" alt="Demo 2" height="256" />

The code has been designed to be easy to understand, and, apart from the base rendering framework, follows along verbatim with the [LearnOpenGL shadow mapping tutorial](https://learnopengl.com/Advanced-Lighting/Shadows/Shadow-Mapping).

### Building & executing
Building and executing is easy!

Simply clone the project, run `git submodule init`, and then pick one of the two projects to run: `ShadowMapGL` or `ShadowMap.CubicGraphics`. The results *should* be identical in both (except for the cube positions, they're randomized.)

#### Controls
* **W, A, S, D** - Move camera in the direction it is facing
* **E, Q** - Move camera up or down, respectively
* **Escape** - Press once, show mouse cursor. Press once more, quit application
* **M** - Toggle mouse cursor visibility
* **R** - Randomize the positions of the cubes, however they will be locked to the plane (like the second screenshot)
* **T** - Randomize the positions of the cubes, randomizing the height and rotation as well (like the first screenshot)
* **C** - Hold to view the scene from the perspective of the light.

### License
As this project uses some code from [LearnOpenGL](https://learnopengl.com/), which I do not own, it has been released into the public domain. If there are any issues with the license, please contact me and I will change it.

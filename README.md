Team members: (names and student IDs)
* Una Garcia - 7403658
* Vlad Puscaru - 5248973

Tick the boxes below for the implemented features. Add a brief note only if necessary, e.g., if it's only partially working, or how to turn it on.

Formalities:
[ ] This readme.txt
[ ] Cleaned (no obj/bin folders)

Minimum requirements implemented:
[x] Camera: position and orientation controls, field of view in degrees
Controls: ...
[x] Primitives: plane, sphere
[x] Lights: at least 2 point lights, additive contribution, shadows without "acne"
[x] Diffuse shading: (N.L), distance attenuation
[x] Phong shading: (R.V) or (N.H), exponent
[x] Diffuse color texture: only required on the plane primitive, image or procedural, (u,v) texture coordinates
[x] Mirror reflection: recursive
[x] Debug visualization: sphere primitives, rays (primary, shadow, reflected, refracted)

Bonus features implemented:
[x] Triangle primitives: single triangles or meshes (+0.5p)
[] Interpolated normals: only required on triangle primitives, 3 different vertex normals must be specified (+0.5p)
[ ] Spot lights: smooth falloff optional
[ ] Glossy reflections: not only of light sources but of other objects
[x] Anti-aliasing (n ray samples found in settings.cs) (+0.5p)
[x] Parallelized: using parallel-for, async tasks, threads, or [fill in other method] (+0.5p)
[x] Textures: on all implemented primitives (+1p)
[ ] Bump or normal mapping: on all implemented primitives
[ ] Environment mapping: sphere or cube map, without intersecting actual sphere/cube/triangle primitives
[ ] Refraction: also requires a reflected ray at every refractive surface, recursive
[ ] Area lights: soft shadows
[ ] Acceleration structure: bounding box or hierarchy, scene with 5000+ primitives
Note: [provide one measurement of speed/time with and without the acceleration structure]
[ ] GPU implementation: using a fragment shader, CUDA, OptiX, RTX, DXR, or [fill in other method]

Notes:
Controls:
    Camera:
        W,A,S,D = forward, left, back, right, 
        Arrow keys left,right = rotation about Y,
        E,C = up, down,
        V,B = FOV down, up
        F,G = focal distance down, up
    Scene:
        Z = switch active scene
        Light movement:
            I,J,K,L = move all lights in scene, not taking camera direction into account.
            U,N = move all lights up/down

For Anti-aliasing: n ray samples is found in settings.cs. Default is 4 rays per pixel.

Resources:
https://opentk.net/api/index.html
https://raytracing.github.io/books/RayTracingInOneWeekend.html
# Tango-doodle

### Blog entry - rough draft

As with any Tango project, the first step is to import the Tango assets into your project.

Download from here [public link to Tango unity sdk, once available]

Go to `Assets > Import Package > Custom package` and select the package file. Leave all assets selected and import them. You should now have a `TangoSDK` folder in your project.

As with all Unity / Tango apps, you'll need to add the prefab at `TangoSDK/Core/Prefabs/Tango` to your scene, and take a look at it's components. We only need pose data for this demo, not depth, so leave everything as it is.

Take a look at the `DrawingRig` object in the root of the scene. All of the drawing logic happens in the `Sketchpad.cs` script, which is attached to a particle system. The "drawing" is actually a regular old ParticleSystem. The only difference is that Looping, Play on awake, Emission, and Shape are all disabled (Leaving Renderer on).

Note that `DrawingRig` has a `TangoController` script on it. This is found in the SDK at `TangoSDK/Examples/Scripts/Controllers/TangoController.cs` and used without modification.

The actual drawing logic, in `Sketchpad.cs`, is fairly standard unity code. Here's hos it works:

- Listen for mouse events
- Project a ray onto the `InvisibleCanvas` plane that is attached to `DrawingRig`
- Create a bunch of new points from the last point to the new point
- Update the ParticleSystem with the latest points

### Todo

Take pretty screenshots if this gets published.

cspotcode's TODO list, should be removed before merge

# Generalize the grind editor buttons to a little overlay on the scene view

Every time selection changes, re-compute which button renderers should be called.

Every custom editor has a `ActionButtonsGUI` method.
It calls this in its own inspector.
Can also be called by a scene `Overlay`.

Scene overlay watches your Selection, when it changes, re-creates Editors.
Share logic with Grind Inspector for getting Selection objects and Editors.

Buttons should appear relevant to selected object(s).

# [x] Extract "Doctor" to a whole-map analyzer

Not limited to grinds, can detect other mapping problems.

# When GrindLine is selected, also highlight the next line for any ambiguous intersections

If there are more than 3 lines connected to a node, then it's ambiguous which line
the game will choose next.  Highlight it in the editor.

# Strip editor-only components from map prefab when building map

There's a hook called within Asset bundling

```csharp
public class StripScene : IProcessSceneWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnProcessScene(Scene scene, BuildReport report)
    {
        Debug.Log("MyCustomBuildProcessor.OnProcessScene " + scene.name);
        var roots = scene.GetRootGameObjects();
        var firstRoot = roots[0];
        var brcMap = firstRoot.GetComponent<BRCMap>();
        DestroyImmediate(brcMap);
    }
}
```

# Hotkeys

MenuItems can add global hotkeys
but what about for specific objects?
I think `ShortcutAttribute` can do it

# Fix undo for "split grind line"

## Reproduction:

nodes A, B, C in chain, connected by lines
Select line between A and B
"split line"
move new node
undo move
undo split
Lines between A, B, and C disappear

# Spline-based grindline

[x] Copy Reptile script
[x] Already have the catlikecoding stuff
[ ] button to convert GrindLine into Spline
  [x] Mark (intermediate) nodes non-movable
  [x] Every time spline changes, re-generate intermediate lines and nodes
    [ ] Move existing lines and nodes?  That way, you can still tweak line params and that'll be remembered
[ ]

[ ] Implement spline-based grindline
    [x] Reuse reptile script
    [WONTDO] Add bake and un-bake buttons to preview results
    [WONTDO] Auto-bake checkbox
    [ ] Button on `GrindLine` to convert to `SplineLine`

[ ] mark lines non-editable so they don't show "split line" buttons

[ ] set pathReference on all created lines

[ ] "add curve" button moves node1 to new end of curve

[ ] auto-create node0 and node1 in case user deletes them

[ ] loop toggle detaches from node1, loops back to node0 instead
    still remembers node1, will use it if you uncheck loop
    if you delete or detach node1, unchecking loop will create new node1

[ ] "merge nodes" button to merge two nodes
    remove the line connecting them
    re-attach all lines from node1 onto node0
    delete node1

# [x] Make all gizmos conditional on gizmos enabled

`if (sceneView != null && sceneView.drawGizmos)`

Nevermind, this isn't necessary, I removed the logic.

# [ ] Add toggle to render collider gizmos for all grindlines

Currently only SplineGrindLineGenerator does this

# [ ] Stop calling OnValidate coroutines when building map!

# Make each spline knot its own gameobject

Why? Because editor UI works better that way
Drag-select around objects works
Ctrl-click and move multiples at once works
In advanced cases, even expand the inspector and select multiples

We could reinvent the wheel, yes.  But that's too complex

# Bug: Drag-select multiple nodes and a line, try to move them

They shoot off into distance.  I think moving nodes also moves line, 
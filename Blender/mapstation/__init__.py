import importlib

# To support auto-reload of local imports: https://blenderartists.org/t/is-reload-scripts-not-working-for-anyone-else-for-addon-development/1146804
if "bpy" in locals():
    importlib.reload(mapstation.auto_assign_guids)
    importlib.reload(mapstation.hint_ui)
else:
    import mapstation.auto_assign_guids
    import mapstation.hint_ui

import bpy

bl_info = {
    "name": "MapStation",
    "description": "Mapping tools for Bomb Rush Cyberfunk",
    "author": "cspotcode",
    "version": (1, 0),
    "blender": (4, 0, 0),
    "location": "",
    "category": "Object"
}

def register():
    print("Register")
    mapstation.auto_assign_guids.register()

def unregister():
    print("Unregister")
    mapstation.auto_assign_guids.unregister()

if __name__ == "__main__":
    register()

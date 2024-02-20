import bpy

# Define the hints array
hints = ["_Grind", "_NonStableSurface", "_Spawn"]

class ObjectHintPanel(bpy.types.Panel):
    bl_label = "BRC MapStation Hints"
    bl_idname = "OBJECT_PT_hint_panel"
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'UI'
    bl_category = 'Object'

    def draw(self, context):
        layout = self.layout

        obj = context.active_object

        if obj:
            layout.label(text="Selected Object: " + obj.name)
            layout.separator()

            for hint in hints:
                # Check if the hint is present in the object name
                has_hint = hint in obj.name

                # Toggle hint button
                if has_hint:
                    layout.operator("object.remove_hint_operator", text="Remove " + hint).hint = hint
                else:
                    layout.operator("object.add_hint_operator", text="Add " + hint).hint = hint
        else:
            layout.label(text="No object selected")


class AddHintOperator(bpy.types.Operator):
    bl_idname = "object.add_hint_operator"
    bl_label = "Add Hint"

    hint: bpy.props.StringProperty()

    def execute(self, context):
        obj = context.active_object
        obj.name += self.hint
        return {'FINISHED'}


class RemoveHintOperator(bpy.types.Operator):
    bl_idname = "object.remove_hint_operator"
    bl_label = "Remove Hint"

    hint: bpy.props.StringProperty()

    def execute(self, context):
        obj = context.active_object
        obj.name = obj.name.replace(self.hint, "")
        return {'FINISHED'}


def register():
    bpy.utils.register_class(ObjectHintPanel)
    bpy.utils.register_class(AddHintOperator)
    bpy.utils.register_class(RemoveHintOperator)


def unregister():
    bpy.utils.unregister_class(ObjectHintPanel)
    bpy.utils.unregister_class(AddHintOperator)
    bpy.utils.unregister_class(RemoveHintOperator)


if __name__ == "__main__":
    register()

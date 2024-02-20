import bpy
import uuid
guid_prop = "guid"

def gen_guid():
    return str(uuid.uuid4())

def assign_guids():
    guids = set()
    for obj in bpy.data.objects:
        if guid_prop in obj:
            guid = obj[guid_prop]
            if guid in guids:
                guid = gen_guid()
                obj[guid_prop] = guid
        else:
            guid = gen_guid()
            obj[guid_prop] = guid
        guids.add(guid)

    # return delay so timer calls us again
    return 1

def register():
    print("register auto_assign_guids: " + __file__)
    print("hi there")
    bpy.app.timers.register(assign_guids)

def unregister():
    bpy.app.timers.unregister(assign_guids)

if __name__ == "__main__":
    register()

using System.Collections.Generic;

namespace MapStation.Plugin;

class SceneNameMapper {
    public static SceneNameMapper Instance;

    public Dictionary<string, string> Mappings = new Dictionary<string, string>();
}

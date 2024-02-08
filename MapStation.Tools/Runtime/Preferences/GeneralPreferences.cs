using System;
using UnityEngine;

/// Settings to control the editor experience for mapping, does not affect how the
/// map plays.
[Serializable]
public class GeneralPreferences {
    [TextArea(1, 3)]
    public string testMapDirectory;
    public bool checkDoctorErrorsBeforeBuildingMap = true;
}

// https://gist.github.com/anastasiadevana/2783a15edf1a969c62186e4c2ec0fa8b

using UnityEngine;

// Place this somewhere in your project (do NOT place in an "Editor" folder)

public class ButtonInvoke : PropertyAttribute
{
    /// <summary>
    /// If you choose to display in editor or both, make sure that your script has a flag to run in editor.
    /// </summary>
    public enum DisplayIn
    {
        PlayMode,
        EditMode,
        PlayAndEditModes
    }

    public readonly string customLabel;
    public readonly string methodName;
    public readonly object methodParameter;
    public readonly DisplayIn displayIn;

    /// <summary>
    /// Add this attribute to any dummy field in order to show a button in inspector.
    /// </summary>
    /// <param name="methodName">Name of the method to call. I recommend using nameof() function to prevent typos.</param>
    /// <param name="methodParameter">Optional parameter to pass into the method.</param>
    /// <param name="displayIn">Should the button show in play mode, edit mode, or both.</param>
    /// <param name="customLabel">Optional custom label.</param>
    public ButtonInvoke(string methodName, object methodParameter = null, DisplayIn displayIn = DisplayIn.PlayMode, string customLabel = "")
    {
        this.methodName = methodName;
        this.methodParameter = methodParameter;
        this.displayIn = displayIn;
        this.customLabel = customLabel;
    }
}
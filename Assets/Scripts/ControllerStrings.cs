using System;
[Serializable]
public struct ControllerStrings
{
    private static string VERTICAL_AXIS_PREFIX = "Vertical";
    private static string HORIZONTAL_AXIS_PREFIX = "Horizontal";
    private static string BUTTON_1_PREFIX = "Fire";
    private static string BUTTON_2_PREFIX = "Jump";
    
    public ControllerStrings(int playerIndex)
    {
        VerticalAxis = VERTICAL_AXIS_PREFIX + playerIndex;
        HorizontalAxis = HORIZONTAL_AXIS_PREFIX + playerIndex;
        Button1 = BUTTON_1_PREFIX + playerIndex;
        Button2 = BUTTON_2_PREFIX + playerIndex;
    }
    
    public string VerticalAxis;
    public string HorizontalAxis;
    public string Button1;
    public string Button2;
}

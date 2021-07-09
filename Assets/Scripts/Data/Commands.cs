
public static class Commands
{
    public static void ClearDropDownMenus()
    {
        foreach (var popUp in EntetieSearchUI.DropListTracker)
        {
            if(popUp != null)popUp.Disable();
        }
    }
}

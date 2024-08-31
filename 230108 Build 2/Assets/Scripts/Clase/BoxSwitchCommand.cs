public class BoxSwitchCommand : ICommands
{
    DialogBox Box;

    public BoxSwitchCommand(DialogBox _box)
    {
        Box = _box;
    }

    public void Execute()
    {
        Box.ToggleBox();
    }

    // TODO Revisar si esto va
    public void Execute (float x, float y, float z)
    {

    }
}

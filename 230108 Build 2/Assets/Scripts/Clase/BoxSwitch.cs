public class BoxSwitch
{
    ICommands command;

    public BoxSwitch(ICommands onCommand)
    {
        command = onCommand;
    }

    public void ToggleBox()
    {
        command.Execute();
    }
}

public class InputHandleReceiver
{
    ICommands commands;

    public InputHandleReceiver(ICommands _command)
    {
        commands = _command;
    }

    public void StorePosition(float x, float y, float z)
    {
        commands.Execute(x, y, z);
    }
}

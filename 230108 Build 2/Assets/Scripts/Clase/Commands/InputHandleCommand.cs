public class InputHandleCommand : ICommands
{
    InputHandle handle;

    public InputHandleCommand(InputHandle _handle)
    {
        handle = _handle;
    }

    // TODO Revisar si esto va
    public void Execute()
    {

    }

    public void Execute(float x, float y, float z)
    {
        handle.StorePosition(x, y, z);
    }
}

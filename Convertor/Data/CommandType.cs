namespace Convertor.Data
{
    public enum CommandType
    {
        DELETE,
        CONT,
        REMOVELABEL,
        POINTER
    }

    public static class CommandTypeResolver
    {
        public static CommandType Resolve(string command) => command switch
        {
            "<DEL>" => CommandType.DELETE,
            "<CONT>" => CommandType.CONT,
            "<RL>" => CommandType.REMOVELABEL,
            _ => ResolvePointer(command),
        };

        //TODO: add better check for pointer.
        private static CommandType ResolvePointer(string pointer) => (pointer.Contains("<") && pointer.Contains(">") && pointer.Contains(":"))
            ? CommandType.POINTER : throw new ArgumentException("Invalid pointer command.");

    }
}

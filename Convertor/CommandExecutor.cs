using Convertor.Data;
using Convertor.Helpers;

namespace Convertor
{
    public class CommandExecutor
    {
        private List<Command> commands;

        public CommandExecutor()
        {
            commands = new List<Command>();
        }

        public bool IsInitialized { get; private set; } = false;

        public void Initialize(List<string> indexData)
        {
            foreach (var line in indexData)
            {
                string[] data = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        
                if (data.Length >= 2)
                {
                    string indexator = StringHelper.TrimLeadingZeroes(data[0]);    
                    string command = data[1];

                    if (data.Length >= 3)
                    {
                        command += data[2].Contains(":") && data[2].Contains(">") ? (" " + data[2]) : String.Empty;
                    }

                    bool hasParsedIndex = long.TryParse(indexator, out long index);
                    bool hasCommand = command.Contains("<") && command.Contains(">");

                    if (hasParsedIndex && hasCommand)
                    {
                        commands.Add(new Command(index, CommandTypeResolver.Resolve(command), command));
                    }
                }
            }

            IsInitialized = commands.Count != 0;
        }

        public string ExecuteCommand(string text)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Commands not loaded!");

            if (text == String.Empty)
            {
                return String.Empty;
            }

            if (text.StartsWith("$$$ 0"))
            {
                return String.Empty;
            }

            string[] firstLine = text.Substring(0, text.IndexOf(Environment.NewLine))
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            long index = long.Parse(firstLine[1]);

            var command = commands.FirstOrDefault(c => c.Index == index);

            return command?.Execute(text) ?? throw new EntryPointNotFoundException("Command not found!");
        }

        public List<Command> GetCommands() => this.commands;
    }
}

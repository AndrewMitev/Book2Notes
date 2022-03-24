namespace Convertor.Data
{
    public class Command
    {
        public Command(long index, CommandType type, string originalCommand)
        {
            Index = index;
            Type = type;
            OriginalCommand = (originalCommand.Replace("<", string.Empty)).Replace(">", string.Empty);
        }

        public long Index { get; set; }

        public CommandType Type { get; set; }

        public string OriginalCommand { get; set; }

        public string Execute(string originalText) => Type switch
        {
            CommandType.DELETE => string.Empty,
            CommandType.CONT => originalText,
            CommandType.REMOVELABEL => RemoveLabel(originalText),
            CommandType.POINTER => ReplacePointer(originalText),
            _ => throw new ArgumentException()
        };

        private string RemoveLabel(string originalText) => originalText
            .Substring(originalText.IndexOf(Environment.NewLine)).TrimStart();


        private string ReplacePointer(string originalText)
        {
            int indexOfPointer = originalText.IndexOf(Index.ToString());
            return (originalText.Remove(indexOfPointer, Index.ToString().Length))
                .Insert(indexOfPointer, OriginalCommand);
        }

    }
}

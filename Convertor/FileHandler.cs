using Convertor.Helpers;
using System.Text;

namespace Convertor
{
    public class FileHandler
    {
        private string _fileName;
        private string _outputFilename => _fileName.Insert(_fileName.LastIndexOf('.'), "_Notes");

        private CommandExecutor _commandExecutor;

        public FileHandler(CommandExecutor executor, string filename)
        {
            _fileName = filename;
            _commandExecutor = executor;

            IndexData = new List<string>();
        }

        public List<string> IndexData { get; private set; }

        public void Handle()
        {
            if (!_commandExecutor.IsInitialized)
                throw new InvalidOperationException("Commands not loaded!");

            using (StreamReader reader = new StreamReader(_fileName))
            {
                using (StreamWriter writer = new StreamWriter(_outputFilename))
                {
                    string line = string.Empty;
                    StringBuilder section = new StringBuilder();

                    while (true)
                    {
                        line = reader.ReadLine();

                        if (line is null)
                        {
                            InvokeExecutor(section, writer);
                            break;
                        }

                        if (line.StartsWith("$$$"))
                        {
                            InvokeExecutor(section, writer);
                        }

                        section.AppendLine(line);
                    }
                }
            }
        }

        public (bool, string) Validate()
        {
            List<int> indexators = new List<int>();
            List<int> usedIndexators = new List<int>();

            IndexData = LoadIndexData();
            _commandExecutor.Initialize(IndexData);

            foreach (var line in IndexData)
            {
                if (line.StartsWith("0"))
                {
                    string[] data = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int.TryParse(StringHelper.TrimLeadingZeroes(data[0]), out var index);
                    indexators.Add(index);
                }
            }

            using (StreamReader reader = new StreamReader(_fileName))
            {
                string line = string.Empty;

                while (true)
                {
                    line = reader.ReadLine();

                    if (line is null)
                    {
                        break;
                    }

                    if (line.StartsWith("$$$") && !line.StartsWith("$$$ 0"))
                    {
                        string[] data = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (data.Length < 2)
                        { 
                            //TODO:Log error
                        }

                        if (int.TryParse(data[1], out int usedIndexator))
                        { 
                            usedIndexators.Add(usedIndexator);
                        }
                    }
                }
            }

            StringBuilder errorMessage = new StringBuilder();
            StringBuilder errorMessageCommands = new StringBuilder();

            foreach (var usedIndex in usedIndexators)
            {
                if (!indexators.Contains(usedIndex))
                {
                    errorMessage.AppendLine($"$$$ {usedIndex} not mentioned in index.");
                }
            }

            var commands = _commandExecutor.GetCommands();

            foreach (var indexator in indexators)
            {
                if (!usedIndexators.Contains(indexator))
                {
                    errorMessage.AppendLine($"$$$ {indexator} is not available in the file.");
                }

                if(indexator != 0 && !commands.Any(command => command.Index == indexator))
                {
                    errorMessageCommands.AppendLine($"There is no command for index: {StringHelper.AddLeadingZeroes(indexator)}");
                }
            }

            foreach (var command in commands)
            {
                if (command.Type == Data.CommandType.CONT 
                    && !commands.Any(c => c.Type != Data.CommandType.CONT && c.Index == command.Index))
                {
                    errorMessageCommands.AppendLine($"There should be available a normal conversion string or RL-command for command {StringHelper.AddLeadingZeroes(command.Index)}");
                }
            }

            if (errorMessage.Length == 0 && errorMessageCommands.Length == 0)
            { 
                return (true, String.Empty);
            }

            return (false, errorMessage.ToString() + errorMessageCommands.ToString());
        }

        public void LogErrors(string errors, string outputErrorsFilename)
        {
            using (StreamWriter writer = new StreamWriter(outputErrorsFilename))
            {
                writer.WriteLine(errors);
            }
        }

        private List<string> LoadIndexData()
        {
            List<string> indexData = new List<string>();
            bool isActiveArea = false;

            using (StreamReader reader = new StreamReader(_fileName))
            {
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    if (line.Contains("$$$ 0"))
                    {
                        isActiveArea = true;
                    }

                    if (line.Contains("$$$ 1"))
                    {
                        isActiveArea = false;
                        break;
                    }

                    if (isActiveArea && line != string.Empty)
                    {
                        indexData.Add(line);
                    }
                }
            }

            return indexData;
        }

        private void InvokeExecutor(StringBuilder section, StreamWriter writer)
        {
            string previousSection = section.ToString();
            string editedText = _commandExecutor.ExecuteCommand(previousSection);

            if (editedText != String.Empty)
            {
                writer.WriteLine(editedText);
            }

            section.Clear();
        }
    }
}
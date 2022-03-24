using System.Windows;
using System.Collections.Generic;

using Convertor;
using System;

namespace Book2Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private CommandExecutor commandExecutor;
        private FileHandler fileHandler;

        public void OnExecute(object sender, RoutedEventArgs args)
        {
            fileHandler.Handle();

            //success
            FileNameDisplay.Content = string.Empty;
            FileNameDisplay.Visibility = Visibility.Hidden;
            DropImage.Visibility = Visibility.Visible;

            ExecuteButton.IsEnabled = false;
            ValidateButton.IsEnabled = false;

            var notification = new Notification("Notes generated successfully!", true);
            notification.Show();
        }

        public void OnValidate(object sender, RoutedEventArgs args)
        {
            (bool isValid, string errorMessage) = fileHandler.Validate();
            Notification notificationWindow;

            if (isValid)
            {
                notificationWindow = new Notification("No errors found!", true);
                ExecuteButton.IsEnabled = true;
            }
            else
            {
                string currentFilename = FileNameDisplay.Content.ToString() ?? string.Empty;
                string directory = currentFilename.Substring(0, currentFilename.LastIndexOf("\\"));
                string outputErrorsFile = $"{directory}\\Error_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Year}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.txt";

                notificationWindow = new Notification("Errors in file " + outputErrorsFile, false);

                fileHandler.LogErrors(errorMessage, outputErrorsFile);

                FileNameDisplay.Content = string.Empty;
                FileNameDisplay.Visibility = Visibility.Hidden;

                ValidateButton.IsEnabled = false;
            }

            notificationWindow.Show();
        }

        private void Drag_Drop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                commandExecutor = new CommandExecutor();
                fileHandler = new FileHandler(commandExecutor, files[0] ?? string.Empty);

                FileNameDisplay.Content = files[0];
                FileNameDisplay.Visibility = Visibility.Visible;                

                DropImage.Visibility = Visibility.Hidden;
                ValidateButton.IsEnabled = true;
            }
        }
    }
}

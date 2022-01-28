using System.IO;
using System.Windows;
using RoslynPad.Editor;

namespace RevitConsole
{
    /// <summary>
    /// Interaction logic for RoslynEditor.xaml
    /// </summary>
    public partial class RoslynEditor : Window
    {        
        public RoslynEditor(RoslynEditorViewModel document)
        {
            InitializeComponent();
            DataContext = document;            
        }

        private void CodeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            var editor = (RoslynCodeEditor)sender;
            editor.Loaded -= CodeEditor_Loaded;
            editor.Focus();

            var viewModel = (RoslynEditorViewModel)DataContext;
            var documentViewModel = (DocumentViewModel)editor.DataContext;
            var workingDirectory = Directory.GetCurrentDirectory();

            var documentId = editor.Initialize(viewModel.Host, new ClassificationHighlightColors(),
                workingDirectory, string.Empty);

            documentViewModel.Initialize(documentId);
            editor.Document.TextChanged += documentViewModel.OnTextChanged;
        }

        private void dockManager_DocumentClosing(object sender, AvalonDock.DocumentClosingEventArgs e)
        {
            e.Cancel = true;
            var documentViewModel = (DocumentViewModel)e.Document.Content;
            var viewModel = (RoslynEditorViewModel)DataContext;
            viewModel.Close(documentViewModel);
        }
    }
}

﻿using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.UI;
using Microsoft.Win32;
// ReSharper disable All
#pragma warning disable CS8625

namespace RevitConsole
{
    public class RoslynEditorViewModel : ViewModelBase
    {
        private readonly ExternalEvent _externalEvent;
        private readonly ScriptRunnerHandler _scriptRunnerHandler;
        private string _result = null!;
        private DocumentViewModel _activeDocument = null;
        private Visibility _isRunning = Visibility.Collapsed;


        public RoslynEditorViewModel(RevitRoslynHost host, ExternalEvent externalEvent, ScriptRunnerHandler scriptRunnerHandler)
        {
            Host = host;
            scriptRunnerHandler.RoslynEditorViewModel = this;
            _externalEvent = externalEvent;
            _scriptRunnerHandler = scriptRunnerHandler;
        }

        public RevitRoslynHost Host { get; }

        public string Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

        public Visibility IsRunning
        {
            get { return _isRunning; }
            set { SetProperty(ref _isRunning, value); }
        }

        ObservableCollection<DocumentViewModel> _documents = new ObservableCollection<DocumentViewModel>();
        ReadOnlyObservableCollection<DocumentViewModel> _readonlydocuments = null;
        public ReadOnlyObservableCollection<DocumentViewModel> Documents
        {
            get
            {
                if ( _readonlydocuments == null )
                    _readonlydocuments = new ReadOnlyObservableCollection<DocumentViewModel>(_documents);

                return _readonlydocuments;
            }
        }

        RelayCommand _openCommand = null;
        public ICommand OpenCommand
        {
            get
            {
                if ( _openCommand == null )
                {
                    _openCommand = new RelayCommand((p) => OnOpen(p), (p) => CanOpen(p));
                }

                return _openCommand;
            }
        }

        private bool CanOpen(object parameter)
        {
            return true;
        }

        private void OnOpen(object parameter)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "C# Script file (*.csx)|*.csx";
            if ( dlg.ShowDialog().GetValueOrDefault() )
            {
                var DocumentViewModel = Open(dlg.FileName);
                ActiveDocument = DocumentViewModel;
            }
        }

        public DocumentViewModel Open(string filepath)
        {
            var DocumentViewModel = _documents.FirstOrDefault(fm => fm.FilePath == filepath);
            if ( DocumentViewModel != null )
                return DocumentViewModel;

            DocumentViewModel = new DocumentViewModel(this, filepath);
            _documents.Add(DocumentViewModel);

            return DocumentViewModel;
        }

        RelayCommand _newCommand = null;
        public ICommand NewCommand
        {
            get
            {
                if ( _newCommand == null )
                {
                    _newCommand = new RelayCommand((p) => OnNew(p), (p) => CanNew(p));
                }

                return _newCommand;
            }
        }

        private bool CanNew(object parameter)
        {
            return true;
        }

        private void OnNew(object parameter)
        {
           // _documents.Add(new DocumentViewModel(this) { Document = new TextDocument() });
            _documents.Add(new DocumentViewModel(this));
            ActiveDocument = _documents.Last();
        }

        
        public DocumentViewModel ActiveDocument
        {
            get { return _activeDocument; }
            set
            {
                if ( _activeDocument != value )
                {
                    _activeDocument = value;
                    OnPropertyChanged(nameof(ActiveDocument));
                    ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ActiveDocumentChanged = null!;

        internal void Close(DocumentViewModel fileToClose)
        {
            if ( fileToClose.IsDirty )
            {
                var res = MessageBox.Show(string.Format("Save changes for file '{0}'?", fileToClose.FileName), "Revit ScriptCS", MessageBoxButton.YesNoCancel);
                if ( res == MessageBoxResult.Cancel )
                    return;
                if ( res == MessageBoxResult.Yes )
                {
                    Save(fileToClose);
                }
            }
            _documents.Remove(fileToClose);
        }

        internal void Save(DocumentViewModel fileToSave, bool saveAsFlag = false)
        {
            if ( fileToSave.FilePath == null || saveAsFlag )
            {
                var dlg = new SaveFileDialog();
                dlg.Filter = "C# Script file (*.csx)|*.csx";
                if ( dlg.ShowDialog().GetValueOrDefault() )
                    fileToSave.FilePath = dlg.FileName;
                else
                    return;
            }

            File.WriteAllText(fileToSave.FilePath, fileToSave.Text);
            ActiveDocument.IsDirty = false;
        }

        internal void Run(DocumentViewModel documentViewModel)
        {
            Result = string.Empty;
            IsRunning = Visibility.Visible;
            _scriptRunnerHandler.ScriptText = documentViewModel.Text;
            _externalEvent.Raise();
        }
    }
}

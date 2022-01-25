using ACadAddinManager.Model;

namespace ACadAddinManager.Command
{

    public sealed class AddinManagerBase
    {
      

        public string ActiveTempFolder
        {
            get => _mActiveTempFolder;
            set => _mActiveTempFolder = value;
        }


        
        public static AddinManagerBase Instance
        {
            get
            {
                if (_mInst == null)
                {
#pragma warning disable RCS1059 // Avoid locking on publicly accessible instance.
                    lock (typeof(AddinManagerBase))
                    {
                        if (_mInst == null)
                        {
                            _mInst = new AddinManagerBase();
                        }
                    }
#pragma warning restore RCS1059 // Avoid locking on publicly accessible instance.
                }
                return _mInst;
            }
        }

        private AddinManagerBase()
        {
            this._mAddinManager = new ViewModel.AddinManager();
            this._mActiveCmd = null;
            this._mActiveCmdItem = null;
            this._mActiveApp = null;
            this._mActiveAppItem = null;
        }


        //public IExternalCommand ActiveEC
        //{
        //    get => this._mActiveEc;
        //    set => this._mActiveEc = value;
        //}


        public Addin ActiveCmd
        {
            get => this._mActiveCmd;
            set => this._mActiveCmd = value;
        }

        public AddinItem ActiveCmdItem
        {
            get => this._mActiveCmdItem;
            set => this._mActiveCmdItem = value;
        }


        public Addin ActiveApp
        {
            get => this._mActiveApp;
            set => this._mActiveApp = value;
        }
        public AddinItem ActiveAppItem
        {
            get => this._mActiveAppItem;
            set => this._mActiveAppItem = value;
        }

        public ViewModel.AddinManager AddinManager
        {
            get => this._mAddinManager;
            set => this._mAddinManager = value;
        }

        private string _mActiveTempFolder = string.Empty;

        private static volatile AddinManagerBase _mInst;

        //private IExternalCommand _mActiveEc;

        private Addin _mActiveCmd;

        private AddinItem _mActiveCmdItem;

        private Addin _mActiveApp;

        private AddinItem _mActiveAppItem;

        private ViewModel.AddinManager _mAddinManager;
    }
}

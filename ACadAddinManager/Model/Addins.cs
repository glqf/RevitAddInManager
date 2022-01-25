using System.IO;
using System.Reflection;
using System.Text;
using AddinManagerCore;

namespace ACadAddinManager.Model
{
    public abstract class Addins
    {
        public SortedDictionary<string, Addin> AddinDict
        {
            get => this.m_addinDict;
            set => this.m_addinDict = value;
        }

        public int Count => this.m_addinDict.Count;

        public Addins()
        {
            this.m_addinDict = new SortedDictionary<string, Addin>();
        }

        public void SortAddin()
        {
            foreach (Addin addin in this.m_addinDict.Values)
            {
                addin.SortAddinItem();
            }
        }


        public void AddAddIn(Addin addin)
        {
            string fileName = Path.GetFileName(addin.FilePath);
            if (this.m_addinDict.ContainsKey(fileName))
            {
                this.m_addinDict.Remove(fileName);
            }
            this.m_addinDict[fileName] = addin;
        }

        public bool RemoveAddIn(Addin addin)
        {
            string fileName = Path.GetFileName(addin.FilePath);
            if (this.m_addinDict.ContainsKey(fileName))
            {
                this.m_addinDict.Remove(fileName);
                return true;
            }
            return false;
        }

        public void AddItem(AddinItem item)
        {

            string assemblyName = item.AssemblyName;
            if (!this.m_addinDict.ContainsKey(assemblyName))
            {
                this.m_addinDict[assemblyName] = new Addin(item.AssemblyPath);
            }
            this.m_addinDict[assemblyName].ItemList.Add(item);

        }

        public List<AddinItem> LoadItems(Assembly assembly, string fullName, string originalAssemblyFilePath, AddinType type)
        {
            List<AddinItem> list = new List<AddinItem>();
            Type[] array = null;
            try
            {
                array = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                array = ex.Types;
                if (array == null)
                {
                    return list;
                }
            }
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            foreach (Type type2 in array)
            {
                try
                {
                    if (!(null == type2) && !type2.IsAbstract)
                    {
                        Type @interface = type2.GetInterface(fullName);
                        if (null != @interface)
                        {
                            AddinItem item = new AddinItem(originalAssemblyFilePath, Guid.NewGuid(), type2.FullName, type);
                            list.Add(item);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new System.ArgumentException(e.ToString());
                }
                IL_1A7:;
            }
            if (list2.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("The following Classes: ");
                foreach (string value in list2)
                {
                    stringBuilder.AppendLine(value);
                }
                stringBuilder.Append("implements IExternalCommand but doesn't contain both RegenerationAttribute and TransactionAttribute!");
                MessageBox.Show(stringBuilder.ToString(), DefaultSetting.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (list3.Count > 0)
            {
                StringBuilder stringBuilder2 = new StringBuilder();
                stringBuilder2.AppendLine("The TransactionMode set to Classes: ");
                foreach (string value2 in list3)
                {
                    stringBuilder2.AppendLine(value2);
                }
                stringBuilder2.Append(" are not the same as the mode set to Add-In Manager!");
                MessageBox.Show(stringBuilder2.ToString(), DefaultSetting.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return list;
        }

        protected SortedDictionary<string, Addin> m_addinDict;

        protected int m_maxCount = 100;

        protected int m_count;
    }
}

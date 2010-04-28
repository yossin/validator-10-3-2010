using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using ValidatorSDK;
using ValidatorCoreLib;

namespace HostSysSim
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    ///         
    public partial class Window1 : Window
    {
        public const string sLoadFolder = @"c:\";
        public const string sFileHostSysSimData             = @"c:\HostSysSimData.xml";
        HostSysSimData hssd = new HostSysSimData();
 
        ValidationData validationData = new ValidationData();

        public Window1()
        {
            InitializeComponent();
            AddEmptyPropTree();
        }

        private void AddEmptyPropTree()
        {
            Prop_TreeViewItem.IsExpanded = true;
            PropRow_AddNew pr = new PropRow_AddNew(Prop_TreeViewItem);
            Prop_TreeViewItem.Items.Add(pr);
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {

            if (File.Exists(sFileHostSysSimData))
            {
                LoadPropData(sFileHostSysSimData);
                AddPropsToGUI(Prop_TreeViewItem);
                AddEmptyPropTree();
            }
            else
            {
                string sMsg = sFileHostSysSimData + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK);
            }
        }

        private void LoadPropData(string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(HostSysSimData));
            hssd = (HostSysSimData)s2.Deserialize(w2);
            w2.Close();
        }

        private void AddPropsToGUI(TreeViewItem tvi)
        {
            tvi.Items.Clear();
            tvi.IsExpanded = true;
            foreach (PropRowData prd in hssd.PropRowsData)
            {
                PropRow pr = new PropRow(prd, tvi);
                tvi.Items.Add(pr);
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            hssd.ClearPropRowData();

            GetHostSysSimDataFromTree(Prop_TreeViewItem);
            SaveHostSysSimData(sFileHostSysSimData);
        }

        private void GetHostSysSimDataFromTree(TreeViewItem tvi)
        {
            foreach (Object ob in tvi.Items)
            {
                if (ob.GetType().Equals(typeof(PropRow)))
                {
                    PropRow pr = (PropRow)ob;
                    PropRowData prd = pr.GetPropRowData();
                    hssd.AddPropRowData(prd);
                }
            }
        }

        private void SaveHostSysSimData(string sFile)
        {
            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(HostSysSimData));
            s.Serialize(w, hssd);
            w.Close();
        }

        private void OnValidate(object sender, RoutedEventArgs e)
        {
            hssd.ClearPropRowData();
            GetHostSysSimDataFromTree(Prop_TreeViewItem);
            hssd.GetValidationData(validationData);
            if (LoadFlow() && LoadConvertionComparedItems() && LoadConvertion())  // Load validation data
            {
                ValidatorSDK.ValidationResult result = Validator.Validate(validationData);
                MessageBox.Show(result.Message, "File load error", MessageBoxButton.OK);
            }
        }

        private bool LoadFlow()
        {
            if (!validationData.LoadValidationFlow(sLoadFolder))
            {
                string sMsg = ValidatorSDK.ValidationData.sFileFlow + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK);
                return true;
            }
            return true ;
        }

        private bool LoadConvertion()
        {
            if ( !validationData.LoadValidationConvertionItems(sLoadFolder) )
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertion + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
                return true;
            }
            return true ;
        }

        private bool LoadConvertionComparedItems()
        {
            if ( ! validationData.LoadConvertionComparedItems(sLoadFolder) )
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertionComparedItems + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
                return true;
            }
            return true ;
        }
    }
}

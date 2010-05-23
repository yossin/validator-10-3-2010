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
        public string sLoadFolder = @"C:\validationFiles\";
        public const string sFileHostSysSimFile = @"HostSysSimData.xml";
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
            if (!SetLoadFolder()) return;
            if (File.Exists(sLoadFolder + sFileHostSysSimFile))
            {
                LoadPropData(sLoadFolder + sFileHostSysSimFile);
                AddPropsToGUI(Prop_TreeViewItem);
                AddEmptyPropTree();
            }
            else
            {
                string sMsg = sLoadFolder + sFileHostSysSimFile + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK);
            }
        }

        private bool SetLoadFolder()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.DefaultExt = ".txt";
            //dlg.Filter = "Text documents (.xml)|*.xml";
            dlg.Title = "Select one of the files from your project";
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox
            if (result != true)
                return false;

            sLoadFolder = dlg.FileName;
            int nInd = sLoadFolder.LastIndexOf("\\");
            sLoadFolder = sLoadFolder.Remove(nInd + 1);
            return true;
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
            if (!SetLoadFolder()) return;

            hssd.ClearPropRowData();

            GetHostSysSimDataFromTree(Prop_TreeViewItem);
            SaveHostSysSimData(sLoadFolder + sFileHostSysSimFile);
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
            if (LoadFlow() && LoadBindingContainer())  // Load validation data
            {
                ValidatorCoreLib.ValidationResult result = Validator.Validate(validationData);
                // todo: proceed later
                if (!result.ContainsError)   
                    MessageBox.Show(@"There are no errors", "no conflicts", MessageBoxButton.OK);
                else
                {
                    foreach (ValidatorCoreLib.ValidationErrorEvents.ErrorValidationEvent eve in result.ErrorValidationEvents)
                    {
                        if (eve is ValidatorCoreLib.ValidationErrorEvents.RuleValidationEvent)
                        {
                            ValidatorCoreLib.ValidationErrorEvents.RuleValidationEvent eve_ = (ValidatorCoreLib.ValidationErrorEvents.RuleValidationEvent)eve;
                            if (eve_.Rule.validationResolve.AutoResolve)
                            {
                                StringBuilder sRes = new StringBuilder(@"There is a conflict in rule name=");
                                sRes.Append(eve_.Rule.RuleName);
                                sRes.Append(", \nThis is an auto resolver");
                                sRes.Append(", \n\tResolve=");
                                sRes.Append(eve_.Rule.validationResolve.ResolveObjects[eve_.Rule.validationResolve.ResolveObjectSelected]);
                                MessageBox.Show(sRes.ToString(), "conflict", MessageBoxButton.OK);
                            }
                            else
                            {
                                StringBuilder sRes = new StringBuilder(@"There is a conflict in rule name=");
                                sRes.Append(eve_.Rule.RuleName);
                                sRes.Append(", \nResolve message=");
                                sRes.Append(eve_.Rule.validationResolve.ResolveStringForUI);
                                sRes.Append(", \nResolves are: \n\t");
                                foreach (object obj in eve_.Rule.validationResolve.ResolveObjects)
                                {
                                    sRes.Append(obj);
                                    sRes.Append(",\n\t");
                                }
                                MessageBox.Show(sRes.ToString(), "conflict", MessageBoxButton.OK);
                            }
                        }
                        else
                            MessageBox.Show(result.ToString(), "conflict", MessageBoxButton.OK);
                        
                    }
                }
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

        private bool LoadBindingContainer()
        {
            if ( !validationData.LoadBindingContainer(sLoadFolder) )
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertionBindingContainer + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
                return true;
            }
            return true ;
        }

    }
}

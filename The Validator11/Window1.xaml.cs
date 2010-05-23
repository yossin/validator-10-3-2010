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

namespace The_Validator11
{    
    public partial class Window1 : Window
    {
        public string sLoadFolder = @"C:\validationFiles\";
 
        ValidationData validationData = new ValidationData();
        ConvertionPathItems convertionPathItems = new ConvertionPathItems();
        ConvertionTreeListItem convertionTreeListItem = new ConvertionTreeListItem();

        public Window1()
        {
            InitializeComponent();
            CreateEmpteFlowTree();
            CreateEmpteConvertionTree();
        }

        private void CreateEmpteFlowTree()
        {
            FlowRow fr = new FlowRow("New Flow", true, Flow_TreeViewItem);
            fr.RemoveFlow.IsEnabled = false;
            Flow_TreeViewItem.Header = fr;
            Flow_TreeViewItem.IsExpanded = true;
        }
        
        private void CreateEmpteConvertionTree()
        {
           ConvertionClassItem_AddNew pr = new ConvertionClassItem_AddNew(Convertion_TreeViewItem);
           Convertion_TreeViewItem.Items.Add(pr);
           Convertion_TreeViewItem.IsExpanded = true;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            if (! SetLoadFolder()) return ;

            LoadFlow();

            LoadConvertionTree();
            LoadNameConvertion();
            CreateEmpteConvertionTree();
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
            sLoadFolder = sLoadFolder.Remove(nInd+1);
            return true;
        }

        private void LoadFlow()
        {
            if (!validationData.LoadValidationFlow(sLoadFolder))
            {
                string sMsg = ValidatorSDK.ValidationData.sFileFlow + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK);
            }
            else
            {
                Flow_TreeViewItem.Items.Clear();
                AddFlowsAndRules(validationData.flow, Flow_TreeViewItem);
                ((FlowRow)Flow_TreeViewItem.Header).RemoveFlow.IsEnabled = false;
                Change_FlowTreeItemsSize(Flow_TreeViewItem, tabs.ActualWidth);
            }
        }

        private void LoadConvertionTree()
        {
            Convertion_TreeViewItem.Items.Clear();

            convertionTreeListItem = ValidationData.LoadConvertionTreeListItem(sLoadFolder);
            if (convertionTreeListItem != null)
            {
                AddConvertionListItem(convertionTreeListItem);
            }
            else
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertionTreeListItem + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }

        private void LoadNameConvertion()
        {
            validationData.bindingContainer.Clear();

            if (validationData.LoadBindingContainer(sLoadFolder))
            {
                SetNameConvertionTable();
            }
            else
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertionBindingContainer + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            SetLoadFolder();

            validationData.Clear();

            if (!GetValidationFlowFromTree(validationData.flow, Flow_TreeViewItem))
                validationData.flow = null;
            else
                validationData.SaveFlowData(sLoadFolder);

            convertionTreeListItem.Clear();
            if (!GetValidationConvertionTreeItemFromTree(convertionTreeListItem))
                convertionTreeListItem = null;
            else
            {
                ValidationData.SaveConvertionTreeListItemData(sLoadFolder, convertionTreeListItem);
                validationData.bindingContainer = CreateBindingContainer();
                validationData.SaveBindingContainerData(sLoadFolder);
            }     
        }

        // return false when the flow is empty and should be removed(caller resposability)
        private bool GetValidationFlowFromTree(ValidatorCoreLib.ValidationFlow flow, TreeViewItem tvi)
        {
            if (tvi.Header == null)
                return false;

            ValidatorCoreLib.ValidationFlow copiedFlow = (((FlowRow)tvi.Header)).GetValidationFlow();
            flow.Name = copiedFlow.Name;
            flow.UseAndOperator = copiedFlow.UseAndOperator;

            foreach (Object ob in tvi.Items)
            {
                if (ob.GetType().Equals(typeof(WrapPanel)))   // RuleRow
                {
                    WrapPanel wp = (WrapPanel)ob;
                    foreach (UIElement child in wp.Children)
                    {
                        flow.rules.Add(((RuleRow)child).GetValidationRule());
                    }
                }
                else  // flow
                {
                    ValidatorCoreLib.ValidationFlow flowToAdd = new ValidatorCoreLib.ValidationFlow();
                    if (GetValidationFlowFromTree(flowToAdd, (TreeViewItem)ob))
                        flow.flows.Add(flowToAdd);
                }
            }
            return true;
        }

        // return false when the ConvertionItem is empty and should be removed(caller resposability)
        private bool GetValidationConvertionTreeItemFromTree(ValidatorCoreLib.ConvertionTreeListItem ctli)
        {
            ctli.Clear();
            bool bRes = true;
//            foreach (TreeViewItem tvi in Convertion_TreeViewItem.Items)
            for (int ii = 0; ii < Convertion_TreeViewItem.Items.Count - 1; ii ++ )
            {
                TreeViewItem tvi = (TreeViewItem )Convertion_TreeViewItem.Items[ii];
                if (tvi.Header == null || tvi.Header.GetType().ToString().Contains("add"))
                    continue;
                ValidatorCoreLib.ConvertionTree convertionTree = new ConvertionTree();
                bRes |= GetValidationConvertionTreeItemFromTreeHelper(convertionTree, tvi);
                ctli.Add(convertionTree);             
            }
            return bRes;
        }
        private bool GetValidationConvertionTreeItemFromTreeHelper(ValidatorCoreLib.ConvertionTree convertionItem, TreeViewItem tvi)
        {
            if (tvi.Header == null)
                return false;

            ValidatorCoreLib.ConvertionTree copiedConvertionItem = (((ConvertionClassItem)tvi.Header)).GetConvertionItem();
            convertionItem.convertionPath = copiedConvertionItem.convertionPath;
            convertionItem.convertionAttribute = copiedConvertionItem.convertionAttribute;

            foreach (Object ob in tvi.Items)
            {
                if (ob.GetType().Equals(typeof(TreeViewItem)))  // ConvertionItem
                {
                    ValidatorCoreLib.ConvertionTree convertionItemAdd = new ValidatorCoreLib.ConvertionTree();
                    if (GetValidationConvertionTreeItemFromTreeHelper(convertionItemAdd, (TreeViewItem)ob))
                        convertionItem.Add(convertionItemAdd);
                }
            }
            return true;
        }

        private void AddFlowsAndRules(ValidatorCoreLib.ValidationFlow in_flow, TreeViewItem tvi)
        {
            FlowRow fr = new FlowRow(in_flow.Name, in_flow.UseAndOperator, tvi);
            tvi.Header = fr;
            tvi.IsExpanded = true;

            WrapPanel RuleWrapPanel = new WrapPanel();

            foreach (ValidatorCoreLib.ValidationRule rule in in_flow.rules)
            {
                RuleRow rr = new RuleRow(rule, RuleWrapPanel);
                RuleWrapPanel.Children.Add(rr);
            }
            tvi.Items.Add(RuleWrapPanel);

            foreach (ValidatorCoreLib.ValidationFlow flow in in_flow.flows)
            {
                TreeViewItem Newitem = new TreeViewItem();
                Newitem.IsExpanded = true;
                AddFlowsAndRules(flow, Newitem);
                tvi.Items.Add(Newitem);
            }
        }

        private void AddConvertionListItem(ValidatorCoreLib.ConvertionTreeListItem ctli)
        {
            foreach (ConvertionTree ct in convertionTreeListItem.convertionTreeListItem)
            {
                TreeViewItem tvi = new TreeViewItem();
                AddConvertionRec(ct, tvi, true);
                Convertion_TreeViewItem.Items.Add(tvi);
            }
        }
        private void AddConvertionRec(ValidatorCoreLib.ConvertionTree vci, TreeViewItem tvi, Boolean bHead)
        {
            ConvertionClassItem cci = new ConvertionClassItem(vci.convertionPath, vci.convertionAttribute, tvi);
            if (bHead)
            {
                cci.convertionAttributeTB.Visibility = Visibility.Hidden;
                cci.convertionAttribute.Visibility = Visibility.Hidden;
                cci.convertionAttribute.Text = "";
            }
            tvi.Header = cci;
            tvi.IsExpanded = true;
 
            foreach (ValidatorCoreLib.ConvertionTree chiled_vci in vci.convertionItems)
            {
                TreeViewItem Newitem = new TreeViewItem();
                Newitem.IsExpanded = true;
                AddConvertionRec(chiled_vci, Newitem, false);
                tvi.Items.Add(Newitem);
            }
        }

        private double nWidthToRemove = 30;
        private void FlowTreeSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Change_FlowTreeItemsSize(Flow_TreeViewItem, e.NewSize.Width - nWidthToRemove);
        }

        private void Change_FlowTreeItemsSize(TreeViewItem tvi, double nWidth)
        {
            foreach (Object ob in tvi.Items)
            {
                if (ob.GetType().Equals(typeof(WrapPanel)))
                {
                    WrapPanel wp  = (WrapPanel)ob ;
                    wp.Width = nWidth ;
                }
                else  // flow = TreeViewItem
                {
                    Change_FlowTreeItemsSize((TreeViewItem)ob, nWidth - nWidthToRemove);
                }
            }
        }

        public void GetPathListFromAllRules(TreeViewItem tvi, List<string> PathList)
        {
            if (tvi.Header == null)
                return;

            foreach (Object ob in tvi.Items)
            {
                if (ob.GetType().Equals(typeof(WrapPanel)))   // RuleRow
                {
                    WrapPanel wp = (WrapPanel)ob;
                    foreach (UIElement child in wp.Children)
                        PathList.Add(((RuleRow)child).ProperyPath.Text);
                }
                else  // flow
                    GetPathListFromAllRules((TreeViewItem)ob, PathList);
            }
        }

        public void UpdateConvertionPathItemsFromFlow()
        {
            List<string> PathList = new List<string>();
            GetPathListFromAllRules(Flow_TreeViewItem, PathList);

            convertionPathItems.Add(PathList, ConvertionPathItemsContainer.ConvertionCompareItemType.FromFlow);
        }

        private BindingContainer CreateBindingContainer()
        {
            Dictionary<string, string> referenceBinding = new Dictionary<string, string>();
            Dictionary<string, string> contextBinding = new Dictionary<string, string>();

            foreach (ConvertionTree ct in convertionTreeListItem.convertionTreeListItem)
                CreateBindingContainerRec(ct, ct.convertionAttribute, referenceBinding);
            GetNameConvertionData(contextBinding);

            return new BindingContainer(contextBinding, referenceBinding);
        }

        private void CreateBindingContainerRec(ValidatorCoreLib.ConvertionTree convertionTree, string sBuiltKey, Dictionary<string, string> referenceBinding)
        {
            if ( ! convertionTree.convertionPath.Equals(ConvertionClassItem.sIgnoredString) )
                referenceBinding[convertionTree.convertionPath] = sBuiltKey;

            foreach (ValidatorCoreLib.ConvertionTree chiled_convertionTree in convertionTree.convertionItems)
            {
                string sNewKey = "" ;
                if ( ! sBuiltKey.Equals(""))
                {
                    sNewKey = sBuiltKey ;
                    sNewKey += "." ;
                }
                sNewKey += chiled_convertionTree.convertionAttribute;

                CreateBindingContainerRec(chiled_convertionTree, sNewKey, referenceBinding);
            }
        }
    
        public class NameConvertionData
        {
            public NameConvertionData(string DesignerName, string ContextName) { this.DesignerName = DesignerName; this.ContextName = ContextName; }
            public string DesignerName { get; set; }
            public string ContextName { get; set; }
        }

        public void GetNameConvertionData(Dictionary<string, string> contextBinding)
        {
            validationData.bindingContainer.ContextBinding.Clear();
            foreach (NameConvertionData ncd in NameConvertionItems.Items)
            {
                if ( ! ncd.DesignerName.Equals("") && ! contextBinding.ContainsKey(ncd.DesignerName) )
                    contextBinding.Add(ncd.DesignerName, ncd.ContextName);
            }
        }

        private void OnNameConvertionRemoveSelectedRow(object sender, RoutedEventArgs e)
        {
            NameConvertionItems.Items.RemoveAt(NameConvertionItems.SelectedIndex);
        }

        private void SetNameConvertionTable()
        {
            NameConvertionItems.Items.Clear();
            foreach (KeyValuePair<string, string> ContextBindingLine in validationData.bindingContainer.ContextBinding)
            {
                NameConvertionData ncs = new NameConvertionData(ContextBindingLine.Key, ContextBindingLine.Value);
                NameConvertionItems.Items.Add(ncs);
            }
        }

        private void OnNameConvertionAddRow(object sender, RoutedEventArgs e)
        {
            NameConvertionData ncs = new NameConvertionData("", "");
            NameConvertionItems.Items.Add(ncs);
        }
    }
}

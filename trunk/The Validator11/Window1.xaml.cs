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
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    ///         
    public partial class Window1 : Window
    {

        public const string sFileFlow                       = @"c:\Flow1.xml";
        public const string sFileConvertion                 = @"c:\Convertion.xml";
        public const string sFileConvertionComparedItems    = @"c:\ConvertionComparedItems.xml";
 
        ValidationData validationData = new ValidationData();

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
            ConvertionClassItem cci = new ConvertionClassItem(@"New Convertion Item", @"Name",0 , validationData.convertionComparedItems, Convertion_TreeViewItem, Convertion_TreeViewItem);
            cci.RemoveConvertionClassItem.IsEnabled = false;
            Convertion_TreeViewItem.Header = cci;
            Convertion_TreeViewItem.IsExpanded = true;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            LoadFlow(validationData, sFileFlow);
            LoadConvertionComparedItems(validationData, sFileConvertionComparedItems);

            UpdateConvertionComparedItemsFromFlow(validationData);
            
            LoadConvertion(validationData, sFileConvertion);
        }

        private void LoadFlow(ValidationData validationData, string sFile)
        {
            Flow_TreeViewItem.Items.Clear();

            if (File.Exists(sFile))
            {
                LoadFlowData(validationData, sFile);
                AddFlowsAndRules(validationData.flow, Flow_TreeViewItem);
                ((FlowRow)Flow_TreeViewItem.Header).RemoveFlow.IsEnabled = false;
                Change_FlowTreeItemsSize(Flow_TreeViewItem, tabs.ActualWidth);
            }
            else
            {
                string sMsg = sFile + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }

        private void LoadConvertion(ValidationData validationData, string sFile)
        {
            Convertion_TreeViewItem.Items.Clear();

            if (File.Exists(sFile))
            {
                LoadConvertionData(validationData, sFile);
                AddConvertion(validationData.convertionItem, validationData.convertionComparedItems, Convertion_TreeViewItem);
                ((ConvertionClassItem)Convertion_TreeViewItem.Header).RemoveConvertionClassItem.IsEnabled = false;
            }
            else
            {
                string sMsg = sFile + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }

        private void LoadConvertionComparedItems(ValidationData validationData, string sFile)
        {
            validationData.convertionComparedItems.Clear();

            if (File.Exists(sFile))
            {
                LoadConvertionComparedItemsData(validationData, sFile);
            }
            else
            {
                string sMsg = sFile + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }
        
        private void LoadFlowData(ValidationData validationData, string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ValidationFlow));
            validationData.flow = (ValidatorCoreLib.ValidationFlow)s2.Deserialize(w2);
            w2.Close();
        }

        private void SaveFlowData(ValidationData validationData, string sFile)
        {
            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ValidationFlow));
            s.Serialize(w, validationData.flow);
            w.Close();
        }

        private void LoadConvertionData(ValidationData validationData, string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ValidationConvertionItem));
            validationData.convertionItem = (ValidatorCoreLib.ValidationConvertionItem)s2.Deserialize(w2);
            w2.Close();
        }

        private void SaveConvertionData(ValidationData validationData, string sFile)
        {
            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ValidationConvertionItem));
            s.Serialize(w, validationData.convertionItem);
            w.Close();
        }

        private void LoadConvertionComparedItemsData(ValidationData validationData, string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ConvertionComparedItems));
            validationData.convertionComparedItems = (ValidatorCoreLib.ConvertionComparedItems)s2.Deserialize(w2);
            w2.Close();
        }

        private void SaveConvertionComparedItemsData(ValidationData validationData, string sFile)
        {
            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ConvertionComparedItems));
            s.Serialize(w, validationData.convertionComparedItems);
            w.Close();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            validationData.Clear();

            if (!GetValidationFlowFromTree(validationData.flow, Flow_TreeViewItem))
                validationData.flow = null;
            else
                SaveFlowData(validationData, sFileFlow);

            if (!GetValidationConvertionItemFromTree(validationData.convertionItem, Convertion_TreeViewItem))
                validationData.convertionItem = null;
            else
                SaveConvertionData(validationData, sFileConvertion);

            if (!GetConvertionComparedItems(validationData.convertionComparedItems))
                validationData.convertionItem = null;
            else
                SaveConvertionComparedItemsData(validationData, sFileConvertionComparedItems);
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
        private bool GetValidationConvertionItemFromTree(ValidatorCoreLib.ValidationConvertionItem convertionItem, TreeViewItem tvi)
        {
            if (tvi.Header == null)
                return false;

            ValidatorCoreLib.ValidationConvertionItem copiedConvertionItem = (((ConvertionClassItem)tvi.Header)).GetConvertionItem();
            convertionItem.convertionItemName = copiedConvertionItem.convertionItemName;
            convertionItem.convertTo = copiedConvertionItem.convertTo;

            foreach (Object ob in tvi.Items)
            {
                if (ob.GetType().Equals(typeof(TreeViewItem)))  // ConvertionItem
                {
                    ValidatorCoreLib.ValidationConvertionItem convertionItemAdd = new ValidatorCoreLib.ValidationConvertionItem();
                    if (GetValidationConvertionItemFromTree(convertionItemAdd, (TreeViewItem)ob))
                        convertionItem.Add(convertionItemAdd);
                }
            }
            return true;
        }
        
        // return false when the ConvertionItem is empty and should be removed(caller resposability)
        private bool GetConvertionComparedItems(ValidatorCoreLib.ConvertionComparedItems convertionComparedItems)
        {
            convertionComparedItems.Clear();
            ((ConvertionClassItem)Convertion_TreeViewItem.Header).GetConvertionComparedItems(convertionComparedItems);
            return true;
        }       

        private ValidatorSDK.ValidationResult Validate(IValidatorFactory factory, ValidationData validationData, string flowName)
        {
            Validator validator = factory.CreateValidator(flowName);
            ValidatorSDK.ValidationResult result = validator.Validate(validationData);
            return result;
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
                //tvi.Items.Add(rr);
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

        private void AddConvertion(ValidatorCoreLib.ValidationConvertionItem vci, ConvertionComparedItems convertionComparedItems, TreeViewItem tvi)
        {
            int nSelectedComboIndex = validationData.convertionComparedItems.IsItemExist(vci.convertTo);
            ConvertionClassItem cci = new ConvertionClassItem(vci.convertionItemName, vci.convertTo, nSelectedComboIndex, validationData.convertionComparedItems, tvi, Convertion_TreeViewItem);
            tvi.Header = cci;
            tvi.IsExpanded = true;
 
            foreach (ValidatorCoreLib.ValidationConvertionItem chiled_vci in vci.convertionItems)
            {
                TreeViewItem Newitem = new TreeViewItem();
                Newitem.IsExpanded = true;
                AddConvertion(chiled_vci, convertionComparedItems, Newitem);
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

        public void UpdateConvertionComparedItemsFromFlow(ValidationData validationData)
        {
            List<string> PathList = new List<string>();
            GetPathListFromAllRules(Flow_TreeViewItem, PathList);

            validationData.convertionComparedItems.Add(PathList, ConvertionCompareItemsContainer.ConvertionCompareItemType.FromFlow);
        }
    }
}

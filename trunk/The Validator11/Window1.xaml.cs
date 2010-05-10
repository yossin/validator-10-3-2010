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
        public const string sLoadFolder = @"c:\";
 
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
            // load flow
            LoadFlow();

            LoadConvertionComparedItems();

            UpdateConvertionComparedItemsFromFlow(validationData);
            
            LoadConvertion();
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

        private void LoadConvertion()
        {
            Convertion_TreeViewItem.Items.Clear();

            if ( validationData.LoadValidationConvertionItems(sLoadFolder) )
            {
                AddConvertion(validationData.BindingContainer, validationData.convertionComparedItems, Convertion_TreeViewItem);
                ((ConvertionClassItem)Convertion_TreeViewItem.Header).RemoveConvertionClassItem.IsEnabled = false;
            }
            else
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertion + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }

        private void LoadConvertionComparedItems()
        {
            validationData.convertionComparedItems.Clear();
 
            if ( ! validationData.LoadConvertionComparedItems(sLoadFolder) )
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertionComparedItems + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            validationData.Clear();

            if (!GetValidationFlowFromTree(validationData.flow, Flow_TreeViewItem))
                validationData.flow = null;
            else
                validationData.SaveFlowData(sLoadFolder);

            if (!GetValidationConvertionItemFromTree(validationData.BindingContainer, Convertion_TreeViewItem))
                validationData.BindingContainer = null;
            else
                validationData.SaveConvertionData(sLoadFolder);

            if (!GetConvertionComparedItems(validationData.convertionComparedItems))
                validationData.BindingContainer = null;
            else
                validationData.SaveConvertionComparedItemsData(sLoadFolder);
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

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
        public const string sLoadFolder = @"C:\validationFiles\";
 
        ValidationData validationData = new ValidationData();
        ConvertionPathItems convertionPathItems = new ConvertionPathItems();
        ConvertionTree convertionTreeItem = new ConvertionTree();

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
            ConvertionClassItem cci = new ConvertionClassItem(ConvertionClassItem.sIgnoredString, @"Attribute Name"/*, convertionPathItems*/, Convertion_TreeViewItem/*, Convertion_TreeViewItem*/);
            cci.RemoveConvertionClassItem.IsEnabled = false;
            Convertion_TreeViewItem.Header = cci;
            Convertion_TreeViewItem.IsExpanded = true;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            // load flow
            LoadFlow();

            //LoadConvertionPathItems();
            //UpdateConvertionPathItemsFromFlow();
            
            LoadConvertionTree();
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

            convertionTreeItem = ValidationData.LoadConvertionTree(sLoadFolder) ;
            if (convertionTreeItem != null)
            {
                AddConvertion(convertionTreeItem/*, convertionPathItems*/, Convertion_TreeViewItem);
                ((ConvertionClassItem)Convertion_TreeViewItem.Header).RemoveConvertionClassItem.IsEnabled = false;
            }
            else
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertionTree + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }

        /*private void LoadConvertionPathItems()
        {
            convertionPathItems.Clear();

            convertionPathItems = ValidationData.LoadConvertionPathItems(sLoadFolder);
            if ( convertionPathItems == null)
            {
                string sMsg = ValidatorSDK.ValidationData.sFileConvertionPathItems + @" doesn't exist";
                MessageBox.Show(sMsg, "File load error", MessageBoxButton.OK );
            }
        }*/

        private void OnSave(object sender, RoutedEventArgs e)
        {
            validationData.Clear();

            if (!GetValidationFlowFromTree(validationData.flow, Flow_TreeViewItem))
                validationData.flow = null;
            else
                validationData.SaveFlowData(sLoadFolder);

            convertionTreeItem.Clear();
            if (!GetValidationConvertionTreeItemFromTree(convertionTreeItem, Convertion_TreeViewItem))
                convertionTreeItem = null;
            else
            {
                ValidationData.SaveConvertionTreeData(sLoadFolder, convertionTreeItem);
                validationData.bindingContainer = CreateBindingContainer();
                // To do - serelize not working for dictionary
              //  validationData.SaveBindingContainerData(sLoadFolder);
            }
            
            


            /*if (!GetConvertionPathItems(convertionPathItems))
                convertionPathItems = null;
            else
                ValidationData.SaveConvertionConvertionPathItemsData(sLoadFolder, convertionPathItems);*/
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
        private bool GetValidationConvertionTreeItemFromTree(ValidatorCoreLib.ConvertionTree convertionItem, TreeViewItem tvi)
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
                    if (GetValidationConvertionTreeItemFromTree(convertionItemAdd, (TreeViewItem)ob))
                        convertionItem.Add(convertionItemAdd);
                }
            }
            return true;
        }
        
        // return false when the ConvertionItem is empty and should be removed(caller resposability)
    /*    private bool GetConvertionPathItems(ValidatorCoreLib.ConvertionPathItems convertionPathItems)
        {
            convertionPathItems.Clear();
            ((ConvertionClassItem)Convertion_TreeViewItem.Header).GetConvertionPathItems(convertionPathItems);
            return true;
        }*/       

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

        private void AddConvertion(ValidatorCoreLib.ConvertionTree vci/*, ConvertionPathItems convertionPathItems*/, TreeViewItem tvi)
        {
            //int nSelectedComboIndex = convertionPathItems.IsItemExist(vci.convertionAttribute) + 1/*none is the first item*/ ;  
            ConvertionClassItem cci = new ConvertionClassItem(vci.convertionPath, vci.convertionAttribute/*nSelectedComboIndex, convertionPathItems*/, tvi/*, Convertion_TreeViewItem*/);
            tvi.Header = cci;
            tvi.IsExpanded = true;
 
            foreach (ValidatorCoreLib.ConvertionTree chiled_vci in vci.convertionItems)
            {
                TreeViewItem Newitem = new TreeViewItem();
                Newitem.IsExpanded = true;
                AddConvertion(chiled_vci/*, convertionPathItems*/, Newitem);
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

            CreateBindingContainerRec(convertionTreeItem, "", referenceBinding);

            // TBD - get contextBinding

            return new BindingContainer(contextBinding, referenceBinding);
        }
    
        private void CreateBindingContainerRec(ValidatorCoreLib.ConvertionTree convertionTree, string sBuiltKey, Dictionary<string, string> referenceBinding)
        {
            if ( ! convertionTree.convertionPath.Equals(ConvertionClassItem.sIgnoredString) ) 
                referenceBinding[convertionTree.convertionPath] = convertionTree.convertionAttribute ;

            foreach (ValidatorCoreLib.ConvertionTree chiled_convertionTree in convertionTree.convertionItems)
            {
                string sNewKey = sBuiltKey ;
                sNewKey += "." ;
                sNewKey += convertionTree.convertionAttribute;

                CreateBindingContainerRec(chiled_convertionTree, sNewKey, referenceBinding);
            }
        }
    }
}

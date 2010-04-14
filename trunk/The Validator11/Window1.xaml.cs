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

        public const string sFile = @"c:\list.xml";

        public Window1()
        {
            InitializeComponent();
            AddValueToConvertionGrid("yyy", "fds");
            AddValueToConvertionGrid("yyy2", "fds2");
            AddValueToConvertionGrid("yyy3", "fds4");
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Flow_TreeViewItem.Items.Clear();

            ValidationData validationData = new ValidationData();
            GetPropertiesData(validationData);

            LoadFlowData(validationData, sFile);

            AddFlowsAndRules(validationData.flow, Flow_TreeViewItem);

//            bool Res = validationData.flow.Validate(validationData.Contexts);
            
            //            CreateFlowTree();
        }

        private void LoadFlowData(ValidationData validationData, string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ValidationFlow));
            validationData.flow = (ValidatorCoreLib.ValidationFlow)s2.Deserialize(w2);
            w2.Close();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            ValidationData validationData = new ValidationData();
            if (!GetValidationFlowFromTree(validationData.flow, Flow_TreeViewItem))
                validationData.flow = null;

            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ValidationFlow));
            s.Serialize(w, validationData.flow);
            w.Close();
        }

        // bool return false then the flow is empty and should be removed(caller resposability)
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

        private ValidatorSDK.ValidationResult Validate(IValidatorFactory factory, ValidationData validationData, string flowName)
        {
            Validator validator = factory.CreateValidator(flowName);
            ValidatorSDK.ValidationResult result = validator.Validate(validationData);
            return result;
        }

        protected void GetPropertiesData(ValidationData validationData)
        {
            validationData.Add("Property 1", (int)20);
            validationData.Add("Property 2", "text2");
        }

        private void AddFlowsAndRules(ValidatorCoreLib.ValidationFlow in_flow, TreeViewItem tvi)
        {
            FlowRow fr = new FlowRow(in_flow.Name, in_flow.UseAndOperator, tvi);
            tvi.Header = fr;
            tvi.IsExpanded = true;

            WrapPanel RuleWrapPanel = new WrapPanel();



            foreach (ValidatorCoreLib.ValidationRule rule in in_flow.rules)
            {
                RuleRow rr = new RuleRow(rule.id, rule.key, rule.Operator, rule.contextContain, rule.ComparedObject, RuleWrapPanel);
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

        private void AddValueToConvertionGrid(String sFrom, String sTo)
        {
            ConvertionGrid.RowDefinitions.Add(new RowDefinition());
            ConvertionRow cgr = new ConvertionRow(sFrom, sTo);
            Grid.SetRow(cgr, ConvertionGrid.Children.Count);
            ConvertionGrid.Children.Add(cgr);
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
        
    }
}

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

        public const string sFile = @"../../../bin/list.xml";

        public Window1()
        {
            InitializeComponent();
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
                if (ob.GetType().Equals(typeof(RuleRow)))
                    flow.rules.Add(((RuleRow)ob).GetValidationRule());
                else  // flow
                {
                    ValidatorCoreLib.ValidationFlow flowToAdd = new ValidatorCoreLib.ValidationFlow(); 
                    if(  GetValidationFlowFromTree(flowToAdd, (TreeViewItem)ob) )
                        flow.flows.Add(flowToAdd);
                }
            }
            return true;
        }
/*
        private void CreateFlowTree()
        {
            ValidationData validationData = new ValidationData();
            GetPropertiesData(validationData);

            validationData.flow.Name = "Main Flow";
            validationData.flow.UseAndOperator = true;

            ValidatorCoreLib.ValidationFlow flowInt = new ValidatorCoreLib.ValidationFlow("Int Check", true);
            ValidatorCoreLib.ValidationRule rrMin = new ValidatorCoreLib.ValidationRule(1, "System.Int32", "Property 1", (int)5, new GreaterOrEqualOperator());
            ValidatorCoreLib.ValidationRule rrMax = new ValidatorCoreLib.ValidationRule(2, "System.Int32", "Property 1", (int)15, new LowerOrEqualOperator());
            flowInt.Add(rrMin);
            flowInt.Add(rrMax);
            validationData.flow.Add(flowInt);

            ValidatorCoreLib.ValidationFlow flowString = new ValidatorCoreLib.ValidationFlow("String Check", true);
            ValidatorCoreLib.ValidationRule rrString = new ValidatorCoreLib.ValidationRule(3, "System.string", "Property 2", "value to compare", new EqualOperator());
            flowString.Add(rrString);
            validationData.flow.Add(flowString);
        
            // Serialize
            TextWriter w = new StreamWriter(@"c:\list.xml");
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ValidationFlow));
            s.Serialize(w, validationData.flow);
            w.Close();

            // deSerialize
            TextReader w2 = new StreamReader(@"c:\list.xml");
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ValidationFlow));
            ValidatorCoreLib.ValidationFlow vf = (ValidatorCoreLib.ValidationFlow)s2.Deserialize(w2);
            w2.Close();

            bool Res = validationData.flow.Validate(validationData.Contexts);
            AddFlowsAndRules(validationData.flow, Flow_TreeViewItem);
        }
*/
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
                 
            foreach (ValidatorCoreLib.ValidationRule rule in in_flow.rules)
            {
                RuleRow rr = new RuleRow(rule.id, rule.key, rule.Operator, rule.contextContain, rule.ComparedObject);
                tvi.Items.Add(rr);
            }

            foreach (ValidatorCoreLib.ValidationFlow flow in in_flow.flows)
            {
                TreeViewItem Newitem = new TreeViewItem();
                Newitem.IsExpanded = true;
                AddFlowsAndRules(flow, Newitem);
                tvi.Items.Add(Newitem);
            }
        }
    }
}

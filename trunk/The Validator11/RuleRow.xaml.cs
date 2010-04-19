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

using ValidatorCoreLib;

namespace The_Validator11
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RuleRow : UserControl
    {
        private WrapPanel ParentWrapPanel;

        public RuleRow(ValidatorCoreLib.ValidationRule rule, WrapPanel ParentWrapPanel)
        {
            InitializeComponent();
            SetComboValues();

            ProperyKey.Text = rule.id.ToString();
            ProperyPath.Text = rule.key;
            setOperatorComboValue(rule.Operator);
            ObjectValue.Text = rule.ComparedObject.ToString();
            setTypeComboValue(rule.contextContain);

            ResolveCheck.IsChecked = rule.validationResolve.AutoResolve;
            if (!rule.validationResolve.AutoResolve)
                ResolveMsg.Text = rule.validationResolve.ResolveStringForUI;

            ResolveMsg.IsEnabled = ResolveCheck.IsChecked != true;

            ResolveValues.Items.Clear();
            foreach (string sResolve in rule.validationResolve.ResolveObjects)
            {
                ResolveObjectRow ror = new ResolveObjectRow(this, sResolve);
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = ror;
                ResolveValues.Items.Add(cbi);
            }


            ResolveObjectRow_AddNew rorNew = new ResolveObjectRow_AddNew(this);
            ComboBoxItem cbiNew = new ComboBoxItem();
            cbiNew.Content = rorNew;
            ResolveValues.Items.Add(cbiNew);

            if (rule.validationResolve.ResolveObjectSelected >= 0 && rule.validationResolve.ResolveObjectSelected < ResolveValues.Items.Count )
                ResolveValues.SelectedIndex = rule.validationResolve.ResolveObjectSelected ;
            else 
                ResolveValues.SelectedIndex = ResolveValues.Items.Count - 1 ;

            this.ParentWrapPanel = ParentWrapPanel;
        }
    
        public void AddResoveObject()
        {
            ResolveObjectRow ror = new ResolveObjectRow(this, @"New Resolve");
            ResolveValues.Items.Insert(ResolveValues.Items.Count-1, ror);
        }

        public void RemoveResoveObject(ResolveObjectRow ror)
        {
            ResolveValues.Items.Remove(ror);
        }

        private void setOperatorComboValue(IOperator op)
        {
            string sOp = op.GetType().Name ;
            
            if (sOp.Equals(typeof(EqualOperator).Name)) Operator.SelectedIndex = 0;
            if (sOp.Equals(typeof(NotEqualOperator).Name)) Operator.SelectedIndex = 1;            
            if (sOp.Equals(typeof(GreaterOperator).Name)) Operator.SelectedIndex = 2;
            if (sOp.Equals(typeof(GreaterOrEqualOperator).Name)) Operator.SelectedIndex = 3;
            if (sOp.Equals(typeof(LowerOperator).Name)) Operator.SelectedIndex = 4;
            if (sOp.Equals(typeof(LowerOrEqualOperator).Name)) Operator.SelectedIndex = 5;
        }

        private IOperator getOperatorFromCombo()
        {
            switch (Operator.SelectedIndex)
            {
                case 0: return new EqualOperator();
                case 1: return new NotEqualOperator();
                case 2: return new GreaterOperator();
                case 3: return new GreaterOrEqualOperator();
                case 4: return new LowerOperator();
                case 5: return new LowerOrEqualOperator();
            }
            return new EqualOperator();
        }

        private void setTypeComboValue(string str)
        {
            if (str.ToLower().Contains("int")) ObjectType.SelectedIndex = 0;
            if (str.ToLower().Contains("string")) ObjectType.SelectedIndex = 1;
            if (str.ToLower().Contains("float")) ObjectType.SelectedIndex = 2;
            if (str.ToLower().Contains("object")) ObjectType.SelectedIndex = 3;
        }

        private string getTypeFromCombo()
        {
            switch (ObjectType.SelectedIndex)
            {
                case 0: return "System.Int32";
                case 1: return "System.String";
                case 2: return "System.Single";     // Float
                case 3: return "Object";
            }
            return "System.Int32";
        }

        private void SetComboValues()
        {
            Operator.Items.Add("=");
            Operator.Items.Add("!=");
            Operator.Items.Add(">");
            Operator.Items.Add(">=");
            Operator.Items.Add("<");
            Operator.Items.Add("<=");

            ObjectType.Items.Add("Int");
            ObjectType.Items.Add("String");
            ObjectType.Items.Add("Float");
            ObjectType.Items.Add("Object");
        }

        public ValidatorCoreLib.ValidationRule GetValidationRule()
        {
            string resolveMsg = @"";
            if (ResolveCheck.IsChecked != true) resolveMsg = ResolveMsg.Text;

            ValidatorCoreLib.ValidationRule vr = new ValidatorCoreLib.ValidationRule(Convert.ToInt32(ProperyKey.Text), getTypeFromCombo(),
                ProperyPath.Text, ObjectValue.Text, getOperatorFromCombo(),
                ResolveCheck.IsChecked == true, resolveMsg);

            foreach (Object resolveItem in ResolveValues.Items)
            {
                Object realResolveItem = resolveItem;
                while (realResolveItem.GetType().Equals(typeof(ComboBoxItem)))
                    realResolveItem = ((Object)(((ComboBoxItem)realResolveItem).Content));

                if (realResolveItem.GetType().Equals(typeof(ResolveObjectRow)))
                {
                    vr.validationResolve.ResolveObjects.Add(((ResolveObjectRow)realResolveItem).ResolveValue.Text);
                }
            }

            vr.validationResolve.ResolveObjectSelected = ResolveValues.SelectedIndex;

            return vr;                 
        }

        private void OnX_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in ParentWrapPanel.Children)
            {
                if (child.Equals(this))
                {
                    ParentWrapPanel.Children.Remove(child);
                    return; // done here!
                }
            }
            
        }

        private void ResolveCheck_Clicked(object sender, RoutedEventArgs e)
        {
            ResolveMsg.IsEnabled = ResolveCheck.IsChecked != true ;
        }

        private void ResolveValueSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResolveValues.SelectedIndex == ResolveValues.Items.Count - 1)
            {
                AddResoveObject();
                ResolveValues.SelectedIndex = ResolveValues.Items.Count - 2;
            }
        }        
    }
}

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

            RuleName.Text = rule.RuleName;
            ProperyName.Text = rule.Property.ContextKey;
            ProperyPath.Text = rule.Property.ReferenceMeaning;
            
            setOperatorComboValue(rule.Operator);
 
            setTypeComboValue(rule.ComparedObject);

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

            if (rule.validationResolve.ResolveObjectSelected >= 0 && rule.validationResolve.ResolveObjectSelected < ResolveValues.Items.Count)
                ResolveValues.SelectedIndex = rule.validationResolve.ResolveObjectSelected;
            else
                ResolveValues.SelectedIndex = ResolveValues.Items.Count - 1;

            this.ParentWrapPanel = ParentWrapPanel;
        }

        public ValidatorCoreLib.ValidationRule GetValidationRule()
        {
            string resolveMsg = @"";
            if (ResolveCheck.IsChecked != true) resolveMsg = ResolveMsg.Text;

            ValidatorCoreLib.ValidationRule vr = new ValidatorCoreLib.ValidationRule(RuleName.Text, new PropertySelection(ProperyName.Text, ProperyPath.Text), getTypeFromCombo(),
                getOperatorFromCombo(), ResolveCheck.IsChecked == true, resolveMsg);

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

        private void setTypeComboValue(PropertySelection comparedObject)
        {
            if (comparedObject.ReferenceMeaning.ToLower().Contains("system.int32"))ObjectType.SelectedIndex = 0;
            else if (comparedObject.ReferenceMeaning.ToLower().Contains("system.string"))ObjectType.SelectedIndex = 1;
            else if (comparedObject.ReferenceMeaning.ToLower().Contains("system.single"))ObjectType.SelectedIndex = 2;
            else // object
            {
                ObjectType.SelectedIndex = 3;
                setObjectGUI(true);

                CompareToPath.Text = comparedObject.ReferenceMeaning;
            }

            CompareTo.Text = comparedObject.ContextKey;
        }

        private void setObjectGUI(bool bObject)
        {
            label_Path_.Visibility = bObject ? Visibility.Visible : Visibility.Hidden;
            CompareToPath.Visibility = bObject ? Visibility.Visible : Visibility.Hidden;
            label_key_.Text = bObject ? @"Name:" : @"Value:";
        }

        private PropertySelection getTypeFromCombo()
        {
            if ( ObjectType.SelectedIndex < 3 )
            {
                switch (ObjectType.SelectedIndex)
                {
                    case 0: return new PropertySelection(CompareTo.Text, @"System.Int32");
                    case 1: return new PropertySelection(CompareTo.Text, @"System.String");
                    case 2: return new PropertySelection(CompareTo.Text, @"System.Single");     // Float
                }
            }
            return new PropertySelection(CompareTo.Text, CompareToPath.Text);     // object
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

        private void onObjectTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setObjectGUI(ObjectType.SelectedIndex > 2);
        }        
    }
}

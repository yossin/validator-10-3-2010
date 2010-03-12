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
        public RuleRow(int id, string key, IOperator op, string contextContain, Object ComparedObject)
        {
            InitializeComponent();
            SetComboValues();

            ID.Text = id.ToString();
            Name.Text = key ;
            setOperatorComboValue(op);
            ObjectValue.Text = ComparedObject.ToString();
            setTypeComboValue(contextContain);
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

        private void setTypeComboValue(string str)
        {
            if (str.ToLower().Contains("int")) ObjectType.SelectedIndex = 0;
            if (str.ToLower().Contains("string")) ObjectType.SelectedIndex = 1;
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
        }
    }
}

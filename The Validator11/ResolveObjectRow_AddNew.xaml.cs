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
    public partial class ResolveObjectRow_AddNew : UserControl
    {
        RuleRow parentRuleRow;
        public ResolveObjectRow_AddNew(RuleRow ruleRow)
        {
            InitializeComponent();
            parentRuleRow = ruleRow;
        }

        private void AddNewObjectRow_Click(object sender, RoutedEventArgs e)
        {
            parentRuleRow.AddResoveObject();
        }
    }
}

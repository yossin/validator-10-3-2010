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
    public partial class FlowRow : UserControl
    {
        private TreeViewItem tvItem;
        public FlowRow(string sText, bool bAndOperator, TreeViewItem tvParent)
        {
            this.tvItem = tvParent;
            InitializeComponent();
            Name.Text = sText;
            Operator.SelectedIndex = bAndOperator ? 0 : 1;
        }

        private void RemoveFlow_Click(object sender, RoutedEventArgs e)
        {
            tvItem.Items.Clear();
            tvItem.Header = null;
        }

        private void AddFlow_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem Newitem = new TreeViewItem();
            FlowRow fr = new FlowRow("New Flow", true, Newitem);
            Newitem.Header = fr;
            Newitem.IsExpanded = true;
            tvItem.Items.Add(Newitem);
        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem Newitem = new TreeViewItem();
            RuleRow rr = new RuleRow(0, "0", new EqualOperator(), "System.Int32", "0");
            tvItem.Items.Add(rr);
        }

        public ValidatorCoreLib.ValidationFlow GetValidationFlow()
        {
            return new ValidatorCoreLib.ValidationFlow(Name.Text, Operator.SelectedIndex==0? true:false);
        }
    }
}

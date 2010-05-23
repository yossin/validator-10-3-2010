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
            WrapPanel wp = new WrapPanel();
            bool WrapPanelFound = false ;
            foreach (Object ob in tvItem.Items)
            {
                if (ob.GetType().Equals(typeof(WrapPanel)))   // RuleRow
                {   
                    wp = (WrapPanel)ob;
                    WrapPanelFound = true;
                    break;
                }
            }

            if (!WrapPanelFound)
            {
                tvItem.Items.Add(wp);
            }

            ValidatorCoreLib.ValidationRule rule = new ValidatorCoreLib.ValidationRule(@"New Rule", new PropertySelection("", ""), @"", new EqualOperator(), true, @"");
            RuleRow rr = new RuleRow(rule, wp);
            wp.Children.Add(rr);
            return; // done here!
        }

        public ValidatorCoreLib.ValidationFlow GetValidationFlow()
        {
            return new ValidatorCoreLib.ValidationFlow(Name.Text, Operator.SelectedIndex==0? true:false);
        }
    }
}

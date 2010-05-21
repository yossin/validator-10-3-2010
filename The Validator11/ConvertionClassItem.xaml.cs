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
    public partial class ConvertionClassItem : UserControl
    {
        private TreeViewItem tvItem;
 
        static public String sIgnoredString = @"None";

        public ConvertionClassItem(string convertionPath, string convertionAttribute,/* ConvertionPathItems convertionPathItems,*/ TreeViewItem tvParent/*, TreeViewItem root_convertionClassItem*/)
        {
            this.tvItem = tvParent;
            InitializeComponent();

            this.convertionAttribute.Text = convertionAttribute;
            this.convertionPath.Text = convertionPath;
        }
        
        private void RemoveConvertionClassItem_Click(object sender, RoutedEventArgs e)
        {
            tvItem.Items.Clear();
            tvItem.Header = null;
        }

        private void AddConvertionClassItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem Newitem = new TreeViewItem();
            ConvertionClassItem fr = new ConvertionClassItem(sIgnoredString, @"Attribute Name", Newitem);
            Newitem.Header = fr;
            Newitem.IsExpanded = true;
            tvItem.Items.Add(Newitem);
        }

        public ValidatorCoreLib.ConvertionTree GetConvertionItem()
        {
            return new ValidatorCoreLib.ConvertionTree(convertionPath.Text, convertionAttribute.Text);
        }
    }
}

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
    public partial class ConvertionClassItem_AddNew : UserControl
    {
        TreeViewItem tviParent;
        public ConvertionClassItem_AddNew(TreeViewItem tvi)
        {
            InitializeComponent();
            tviParent = tvi;
        }

        private void AddNewObjectRow_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = new TreeViewItem();
            ConvertionClassItem fr = new ConvertionClassItem(ConvertionClassItem.sIgnoredString, @"Attribute Name", tvi);
            fr.convertionAttributeTB.Visibility = Visibility.Hidden;
            fr.convertionAttribute.Visibility = Visibility.Hidden;
            fr.convertionAttribute.Text = "";
            tvi.Header = fr;
            tvi.IsExpanded = true;
            tviParent.Items.Insert(tviParent.Items.Count - 1, tvi);
        }
    }
}

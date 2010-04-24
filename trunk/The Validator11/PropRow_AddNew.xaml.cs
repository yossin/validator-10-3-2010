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

namespace HostSysSim
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PropRow_AddNew : UserControl
    {
        TreeViewItem tviParent;
        public PropRow_AddNew(TreeViewItem tvi)
        {
            InitializeComponent();
            tviParent = tvi;
        }

        private void AddNewObjectRow_Click(object sender, RoutedEventArgs e)
        {
            PropRow pr = new PropRow(new PropRowData(), tviParent);
            tviParent.Items.Insert(tviParent.Items.Count - 1, pr);
        }
    }
}

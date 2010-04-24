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
    public partial class PropRow : UserControl
    {
        private TreeViewItem tvItem;
        public PropRow(PropRowData prd, TreeViewItem tvParent)
        {
            this.tvItem = tvParent;
            InitializeComponent();

            PropKey.Text = prd.Key;
            rb1.IsChecked = true;
            switch (prd.RadioBChecked)
            {
                case 0: rb1.IsChecked = true; break;
                case 1: rb2.IsChecked = true; break;
                case 2: rb3.IsChecked = true; break;
                case 3: rb4.IsChecked = true; break;
            }

            this.StringData.Text = prd.StringData;
            this.IntData.Text = prd.IntData.ToString();
            this.FloatData.Text = prd.FloatData.ToString();
            this.ObjectSet.SelectedIndex = prd.ObjectDataComboSelected;
            this.ObjectStringValue.Text = prd.ObjectDataStringData;
        }

        public PropRowData GetPropRowData()
        {
            int nRadioSelected = (rb1.IsChecked == true) ? 0 : (rb2.IsChecked == true) ? 1 : (rb3.IsChecked == true) ? 2 : 1 ;

            PropRowData prd = new PropRowData(PropKey.Text, nRadioSelected, StringData.Text, 
                Convert.ToInt32(IntData.Text), (float)(Convert.ToDouble(FloatData.Text)), 
                ObjectSet.SelectedIndex, ObjectStringValue.Text);

            return prd;
        }

        private void RemoveProp_Click(object sender, RoutedEventArgs e)
        {
            tvItem.Items.Remove(this);
        }
    }
}

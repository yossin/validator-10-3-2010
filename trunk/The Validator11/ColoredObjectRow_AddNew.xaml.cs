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
    public partial class ColoredObjectRow_AddNew : UserControl
    {
        private TreeViewItem tvItemParent;
        private TreeViewItem root_convertionClassItem;
        
        public ColoredObjectRow_AddNew(/*TreeViewItem tvItemParent, TreeViewItem root_convertionClassItem*/)
        {
            InitializeComponent();
            this.tvItemParent = tvItemParent;
            this.root_convertionClassItem = root_convertionClassItem;
        }

        /* <TextBlock Name="convertPath" Text="Path:" Width="25" Padding="0,5" Margin="3,0"></TextBlock>    
        private void TextFocusLost(object sender, RoutedEventArgs e)
        {
            if (!ColoredObjectRow_NewValueTextBox.Text.Equals(@"New"))
            {
                AddNewItemToConvertionPathItems(ColoredObjectRow_NewValueTextBox.Text, root_convertionClassItem);
                ((ConvertionClassItem)(tvItemParent.Header)).convertToCombo.SelectedIndex = (((ConvertionClassItem)(tvItemParent.Header)).convertToCombo).Items.Count - 2;
                ColoredObjectRow_NewValueTextBox.Text = @"New";
            }
        }

        private void AddNewItemToConvertionPathItems(string sValue, TreeViewItem tvi)
        {
            ConvertionClassItem cci = (ConvertionClassItem)(tvi.Header);

            ComboBoxItem cbiNew = new ComboBoxItem();
            ColoredObjectRow cor = new ColoredObjectRow(sValue, ConvertionClassItem.GetColorFromType(ConvertionPathItemsContainer.ConvertionCompareItemType.NotFromFlow), tvi);
            cbiNew.Content = cor;
            cci.convertToCombo.Items.Insert(cci.convertToCombo.Items.Count - 1, cbiNew);

            foreach (TreeViewItem tvi_ in tvi.Items)
            {
                AddNewItemToConvertionPathItems(sValue, tvi_);
            }
        }*/
    }
}

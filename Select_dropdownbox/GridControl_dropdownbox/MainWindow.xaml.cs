using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GridControl_dropdownbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            grid.Model.RowCount = 7;
            grid.Model.ColumnCount = 4;

            grid.Model[1, 2].CellType = "ComboBox";
            grid.Model[1, 2].CellValue = "Select";
            grid.Model[1, 2].ChoiceList = new System.Collections.Specialized.StringCollection() { "one", "two" };
            grid.CurrentCellClosedDropDown += Grid_CurrentCellClosedDropDown;
            grid.CommitCellInfo += Grid_CommitCellInfo;
        }

        private void Grid_CommitCellInfo(object sender, Syncfusion.Windows.Controls.Grid.GridCommitCellInfoEventArgs e)
        {
            e.Style.DropDownStyle = Syncfusion.Windows.Controls.Grid.GridDropDownStyle.Editable;
        }

        private void Grid_CurrentCellClosedDropDown(object sender, Syncfusion.Windows.ComponentModel.SyncfusionRoutedEventArgs args)
        {
            if(grid.CurrentCell.IsEditing)
            {
                //To update the cell value
                grid.CurrentCell.EndEdit();
            }
        }
    }
}

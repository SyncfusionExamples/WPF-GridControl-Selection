using Syncfusion.Windows.Controls.Grid;
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

namespace GridControl_selectionheader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            grid.Model.RowCount = 10;
            grid.Model.ColumnCount = 9;
            grid.Model.HeaderStyle.Borders.All = new Pen(Brushes.LightGray, 1);
            grid.Model.QueryCellInfo += Model_QueryCellInfo;
            grid.SelectionChanging += Grid_SelectionChanging;
        }

        private void Grid_SelectionChanging(object sender, Syncfusion.Windows.Controls.Grid.GridSelectionChangingEventArgs e)
        {
            if (e.Range.Contains(GridRangeInfo.Row(0)) || e.Range.Contains(GridRangeInfo.Col(0)))
                e.Cancel = true;
        }

        private void Model_QueryCellInfo(object sender, Syncfusion.Windows.Controls.Grid.GridQueryCellInfoEventArgs e)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (e.Style.RowIndex == 0 && e.Style.ColumnIndex > 0)
                    e.Style.CellValue = " Col " + e.Style.ColumnIndex;
                else if (e.Style.ColumnIndex == 0 && e.Style.RowIndex > 0)
                    e.Style.CellValue = " Row " + e.Style.RowIndex;
                else if (e.Style.ColumnIndex == 0 && e.Style.RowIndex == 0)
                    e.Style.CellValue = " Grid";
                else
                    e.Style.CellValue = " Row " + e.Style.RowIndex + ", Col " + e.Style.ColumnIndex;
            }
        }
    }
}

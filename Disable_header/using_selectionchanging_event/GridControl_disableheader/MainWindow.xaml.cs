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

namespace GridControl_disableheader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            grid.Model.RowCount = 6;
            grid.Model.HeaderStyle.Borders.All = new Pen(Brushes.LightGray, 1);
            grid.Model.ColumnCount = 4;
            grid.Model.QueryCellInfo += Model_QueryCellInfo;
            grid.Model.SelectionChanging += Model_SelectionChanging;
        }

        private void Model_SelectionChanging(object sender, Syncfusion.Windows.Controls.Grid.GridSelectionChangingEventArgs e)
        {
            if (e.Range.IsTable)
            {
                e.Range = GridRangeInfo.Cells(1, 1, grid.Model.RowCount, grid.Model.ColumnCount);
            }
        }

        private void Model_QueryCellInfo(object sender, Syncfusion.Windows.Controls.Grid.GridQueryCellInfoEventArgs e)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (e.Style.RowIndex > 0 && e.Style.ColumnIndex > 0)
                    e.Style.CellValue = " R" + e.Style.RowIndex + ", C" + e.Style.ColumnIndex;
            }
        }
    }
}

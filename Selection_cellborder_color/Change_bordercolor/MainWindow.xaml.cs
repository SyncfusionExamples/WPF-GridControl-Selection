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

namespace Change_bordercolor
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
            grid.Model.ColumnCount = 7;
            grid.Model.Options.ExcelLikeSelectionFrame = true;
            grid.Model.HeaderStyle.Borders.All = new Pen(Brushes.LightGray, 1);
            grid.Model.QueryCellInfo += Model_QueryCellInfo;            
            grid.Model.Options.AllowSelection = Syncfusion.Windows.Controls.Grid.GridSelectionFlags.Row;
            grid.Model.Options.HighlightSelectionBorder = Brushes.Green;
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
            }
        }
    }
}

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

namespace Selection_highlight_cellborder
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
            grid.Model.RowStyles[0].Borders.All = new Pen(Brushes.LightGray, 1);
            grid.Model.ColStyles[0].Borders.All = new Pen(Brushes.LightGray, 1);            
            grid.Model.SelectionChanged += Model_SelectionChanged;
            grid.Model.QueryCellInfo += Model_QueryCellInfo;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    grid.Model[i, j].CellValue = "R" + i + "C" + j;
                }
            }

        }

        private void Model_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 0)
            {
                if (grid.Model.SelectedRanges.AnyRangeContains(GridRangeInfo.Cell(e.Cell.RowIndex, e.Cell.ColumnIndex)))
                {
                    e.Style.Borders.All = new Pen(Brushes.DarkGreen, 2);
                }
            }
        }

        private void Model_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if(e.Reason==GridSelectionReason.MouseDown || e.Reason==GridSelectionReason.MouseUp)
            {
                grid.InvalidateCells();
            }
        }
    }
}

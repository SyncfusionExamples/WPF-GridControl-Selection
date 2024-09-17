using Syncfusion.Windows.Controls.Cells;
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

namespace Change_cursor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            gridControl.Model.RowCount = 15;
            gridControl.Model.ColumnCount = 8;
            gridControl.Model.HeaderStyle.Borders.All = new Pen(Brushes.LightGray, 1);
            gridControl.Model.Options.AllowSelection = Syncfusion.Windows.Controls.Grid.GridSelectionFlags.Any;
            gridControl.AllowDrop = true;
            gridControl.AllowDragDrop = true;
            gridControl.Model.Options.ShowCurrentCell = false;

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gridControl.Model[i, j].Text = "R" + i.ToString() + "C" + j.ToString();
                }
            }

            gridControl.Model.Options.ActivateCurrentCellBehavior = Syncfusion.Windows.Controls.Grid.GridCellActivateAction.DblClickOnCell;
            gridControl.MouseMove += GridControl_MouseMove;
        }

        private void GridControl_MouseMove(object sender, MouseEventArgs e)
        {
            var _grid = sender as GridControl;
            RowColumnIndex rowColumn = _grid.PointToCellRowColumnIndex(e);
            if (_grid.Model.SelectedRanges.AnyRangeContains(GridRangeInfo.Cell(rowColumn.RowIndex, rowColumn.ColumnIndex)))  
            {
                if ((_grid.CurrentCell.IsEditing))
                {
                    _grid.Cursor = Cursors.Hand;
                }
            }
            else
                _grid.Cursor = Cursors.Arrow;
        }
    }
}

using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GridControl_movecursor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExcelLikeDragDropMouseController mouseController = null;

        public MainWindow()
        {
            InitializeComponent();
            this.PopulatingGrid(this.grid1);
            this.GridSettings(this.grid1);
        }

        public void GridSettings(GridControl gridControl)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gridControl.Model[i, j].Text = "R" + i.ToString() + "C" + j.ToString();
                }
            }
        }

        public void PopulatingGrid(GridControl gridControl)
        {
            gridControl.Model.RowCount = 15;
            gridControl.Model.ColumnCount = 8;
            gridControl.Model.HeaderStyle.Borders.All = new Pen(Brushes.LightGray, 1);
            //gridControl.Model.heade
            gridControl.Model.Options.AllowSelection = Syncfusion.Windows.Controls.Grid.GridSelectionFlags.Any;
            gridControl.AllowDrop = true;
            gridControl.AllowDragDrop = true;
            gridControl.Model.Options.DataObjectConsumerOptions = GridDataObjectConsumerOptions.Text;
            gridControl.Model.Options.ActivateCurrentCellBehavior = Syncfusion.Windows.Controls.Grid.GridCellActivateAction.DblClickOnCell;
            mouseController = new ExcelLikeDragDropMouseController(gridControl);
            mouseController.EnableExcelLikeDragDrop(gridControl.Model.Options.DragDropDropTargetFlags | GridDragDropFlags.Multiple);
            gridControl.MouseControllerDispatcher.Add(mouseController);            
            gridControl.CellMouseDown += GridControl_CellMouseDown;
            gridControl.DragLeave += Grid_DragLeave;
            gridControl.Model.Options.ShowCurrentCell = false;
            gridControl.DragEnter += GridControl_DragEnter;
            gridControl.MouseMove += Grid_MouseMove;
            gridControl.GiveFeedback += GridControl_GiveFeedback;
        }

        private void GridControl_CellMouseDown(object sender, GridCellMouseControllerEventArgs args)
        {
            var _grids = sender as GridControl;
            var rowColIndex = _grids.PointToCellRowColumnIndex(args.MouseControllerEventArgs.SourceEventArgs);
            if(_grids.Model.SelectedRanges.AnyRangeContains(GridRangeInfo.Cell(rowColIndex.RowIndex,rowColIndex.ColumnIndex)))
            {
                mouseController.HitTest(args.MouseControllerEventArgs, null);
                mouseController.MouseDown(args.MouseControllerEventArgs);
            }
        }

        private void GridControl_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Copy || e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;
                //To change the cursor while dragging.
                Mouse.SetCursor(Cursors.SizeAll);
                e.Handled = true;
            }
        }

        private void GridControl_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            if (e.Effects == DragDropEffects.Copy)
            {
                Mouse.SetCursor(Cursors.Hand);
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            var _grid = sender as GridControl;
            RowColumnIndex rowColumn = _grid.PointToCellRowColumnIndex(e);
            if (_grid.Model.SelectedRanges.AnyRangeContains(GridRangeInfo.Cell(rowColumn.RowIndex, rowColumn.ColumnIndex)))
            {
                Mouse.SetCursor(Cursors.Arrow);
            }
            else
                Mouse.SetCursor(Cursors.Arrow);
        }

        private void Grid_DragLeave(object sender, DragEventArgs e)
        {
            var _grid = (sender as GridControl);
            if (_grid != null)
                _grid.InvalidateCells();
        }       
    }
}

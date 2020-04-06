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

namespace Cusror_style
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new Model(this.grid.Model);
            InitModel();
            this.grid.Drop += grid_Drop;
            this.grid1.Drop += grid_Drop;
            grid.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
            grid1.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
            grid.AllowDragDrop = true;
        }

        void grid_Drop(object sender, DragEventArgs e)
        {
            grid.InvalidateCells();
            grid1.InvalidateCells();
        }

        private void InitModel()
        {
            SetGridProperties(this.grid);
            this.grid.Model.Options.DataObjectConsumerOptions = GridDataObjectConsumerOptions.None;
            this.grid.Model.Options.CopyPasteOption |= CopyPaste.IncludeStyle;
            this.grid1.Model.Options.CopyPasteOption |= CopyPaste.IncludeStyle;
            this.grid1.Model.RowCount = 35;
            this.grid1.Model.ColumnCount = 25;
            this.grid1.AllowDrop = true;
            this.grid1.Model.Options.ExcelLikeSelectionFrame = true;
            this.grid1.Model.Options.DataObjectConsumerOptions = GridDataObjectConsumerOptions.Styles;
            this.grid.Model.Options.DataObjectConsumerOptions = GridDataObjectConsumerOptions.All;
        }

        private void SetGridProperties(GridControl gridControl)
        {
            gridControl.AllowDrop = true;
            gridControl.Model.Options.ExcelLikeSelectionFrame = true;
            Random r = new Random();
            gridControl.Model.RowCount = 30;
            gridControl.Model.ColumnCount = 25;
            gridControl.Model.RowHeights[1] = 50;
            gridControl.Model.ColumnWidths[2] = 100;
            GridStyleInfo ci = new GridStyleInfo();
            for (int row = 1; row < 100; row++)
            {
                for (int col = 1; col <= 8; col++)
                {
                    if (col > 7)
                    {
                        continue;
                    }

                    if (r.Next(1, 4) == 2)
                    {
                        gridControl.Model[row, col].CellValue = r.Next(10, 100);
                    }
                    else if (r.Next(1, 4) == 3)
                    {
                        gridControl.Model[row, col].CellValue = "Text" + r.Next(10, 100).ToString();
                    }
                    else
                    {
                        gridControl.Model[row, col].CellValue = (r.Next(1000, 10000) * .01);
                    }

                    if (r.Next(10, 14) == 12)
                    {
                        gridControl.Model[row, col].Font.FontStyle = FontStyles.Italic;
                        gridControl.Model[row, col].Font.FontWeight = FontWeights.Bold;
                        gridControl.Model[row, col].Font.FontSize = 13;
                    }

                    if (r.Next(10, 14) == 13)
                    {
                        gridControl.Model[row, col].Background = Brushes.Orange;
                        gridControl.Model[row, col].Foreground = Brushes.Blue;
                    }

                    if (r.Next(10, 14) == 13)
                    {
                        gridControl.Model[row, col].HorizontalAlignment = HorizontalAlignment.Right;
                    }
                }
            }

            for (int row = 1; row <= gridControl.Model.RowCount; row++)
            {
                gridControl.Model[row, 7].CellType = "DateTimeEdit";
                gridControl.Model[row, 7].CellValue = DateTime.Now;
            }

            gridControl.Model.CoveredCells.Add(new Syncfusion.Windows.Controls.Cells.CoveredCellInfo(4, 4, 6, 6));
            gridControl.Model.CoveredCells.Add(new Syncfusion.Windows.Controls.Cells.CoveredCellInfo(8, 10, 12, 12));
            gridControl.Model.ColumnWidths[7] = 100;
            gridControl.MouseMove += GridControl_MouseMove;
            gridControl.GiveFeedback += GridControl_GiveFeedback;
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

        private void GridControl_MouseMove(object sender, MouseEventArgs e)
        {
            var _grid = sender as GridControl;
            RowColumnIndex rowColumn = _grid.PointToCellRowColumnIndex(e);
            if (_grid.Model.SelectedRanges.AnyRangeContains(GridRangeInfo.Cell(rowColumn.RowIndex, rowColumn.ColumnIndex)))
            {
                Mouse.SetCursor(Cursors.Hand);
            }
            else
                Mouse.SetCursor(Cursors.Arrow);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combo.SelectedIndex == 0)
                grid.AllowDragDrop = false;
            else
                grid.AllowDragDrop = true;
        }
    }

    public class Model
    {
        public Model(GridModel model)
        {
            this.model = model;
        }

        GridModel model
        {
            get;
            set;
        }

        public GridDataObjectConsumerOptions ConsumerOptions
        {
            get
            {
                return this.model.Options.DataObjectConsumerOptions;
            }
            set
            {
                this.model.Options.DataObjectConsumerOptions = value;
            }
        }

        public GridDragDropFlags DragDropTagets
        {
            get
            {
                return this.model.Options.DragDropDropTargetFlags;
            }
            set
            {
                this.model.Options.DragDropDropTargetFlags = value;
            }
        }
    }

}

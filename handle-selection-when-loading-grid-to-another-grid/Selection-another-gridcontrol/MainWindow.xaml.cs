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

namespace Selection_another_gridcontrol
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
            grid.Model.ColumnCount = 4;
            grid.Model.CellModels.Add("CustomGrid", new CustomGridCellModel());
            for (int i = 1; i < grid.Model.RowCount; i++)
            {
                grid.Model.RowHeights[i] = 100;
                for (int j = 1; j < grid.Model.ColumnCount; j++)
                {
                    if (j == 1)
                        grid.Model.ColumnWidths[j] = 400;
                    if (i > 0 && j == 1)
                    {
                        grid.Model[i, j].CellType = "CustomGrid";
                    }
                    else
                    {
                        grid.Model[i, j].CellType = "TextBlock";
                        grid.Model[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                        grid.Model[i, j].VerticalAlignment = VerticalAlignment.Center;
                        grid.Model[i, j].CellValue = "R" + i + "C" + j;
                    }
                }
            }
            grid.Model.Options.DrawSelectionOptions = GridDrawSelectionOptions.AlphaBlend;
            grid.Model.SelectionChanging += Model_SelectionChanging;
            grid.CellMouseUp += Grid_CellMouseUp;
        }

        private void Grid_CellMouseUp(object sender, GridCellMouseControllerEventArgs args)
        {
            if(grid.CurrentCell.ColumnIndex==0)
            {
                grid.InvalidateRenderCell(GridRangeInfo.Table());
            }
        }

        private void Model_SelectionChanging(object sender, GridSelectionChangingEventArgs e)
        {
            if (e.Reason == GridSelectionReason.MouseDown)
            {
                grid.Tag = e.Range;
                if(e.Range.Top != 0)
                {
                    var currentCell = this.grid.CurrentCell;
                    UIElement element;

                    if (currentCell.RowIndex != e.Range.Top)
                    {
                        element = this.GetInnerElement(currentCell.RowIndex, 1);
                        var userControl = element as UserControl1;
                        if (userControl != null)
                        {
                            userControl.child.Model.SelectedRanges.Clear();
                            userControl.child.InvalidateCells();
                        }
                    }
                    element = this.GetInnerElement(e.Range.Top, 1);
                    if (element != null)
                    {
                        var innerElement = element as UserControl1;
                        if (innerElement != null)
                        {
                            innerElement.child.Model.SelectedRanges.Clear();
                            innerElement.child.Model.SelectedRanges.Add(GridRangeInfo.Table());
                            innerElement.child.InvalidateCells();
                        }

                    }
                }
            }
            else if (e.Reason == GridSelectionReason.Clear)
            {
                this.grid.Tag = null;
                var style = this.grid.Model[this.grid.CurrentCell.RowIndex, 1];
                var element = this.GetInnerElement(this.grid.CurrentCell.RowIndex, 1);
                if (element != null)
                {
                    var innerElement = element as UserControl1;
                    if (innerElement != null)
                    {
                        innerElement.child.Model.SelectedRanges.Clear();
                        innerElement.child.InvalidateCells();

                    }
                }
            }
        }
        private UIElement GetInnerElement(int rowIndex, int colIndex)
        {
            var element = this.grid.GetCellUIElements(rowIndex, colIndex);
            if (element != null && element.UIElements.Count > 0)
            {
                var innerElement = element.UIElements[0];
                if (innerElement != null)
                {
                    return innerElement;
                }
            }
            return null;
        }
    }

    public class CustomGrid : GridControl
    {
        public CustomGrid()
        {
            this.Model.RowCount = 5;
            this.Model.ColumnCount = 5;
            this.Model.Options.AllowSelection = GridSelectionFlags.None;
            this.Model.HeaderColumns = 0;
            this.Model.ColStyles[0].Background = Brushes.LightGray;
            this.Model.RowStyles[0].Background = Brushes.LightGray;
            this.Model.HeaderStyle.CellType = "Static";
            this.Model.HeaderStyle.Background = Brushes.AliceBlue;
            this.Model.HeaderRows = 0;
            this.Model[0, 0].CellType = "Static";
            this.Model[0, 0].Background = Brushes.LightGray;
            this.Model.TableStyle.ReadOnly = true;
            this.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.None;
            this.Model.Options.ShowCurrentCell = false;

            for (int i = 1; i < this.Model.RowCount; i++)
            {
                for (int j = 1; j < this.Model.ColumnCount; j++)
                {
                    this.Model[i, j].CellValue = "R" + i + "C" + j;
                }
            }
            this.Model.TableStyle.ReadOnly = true;
        }

        protected override void OnQueryCellInfo(GridQueryCellInfoEventArgs e)
        {
            base.OnQueryCellInfo(e);
            if (this.Model.SelectedRanges.Count > 0 && e.Cell.RowIndex == 0 && e.Cell.ColumnIndex == 0)
            {
                e.Style.CellType = "Static";
                e.Style.Background = new SolidColorBrush(Color.FromRgb(132, 177, 213));
            }
        }
    }

    public class CustomGridCellModel : GridCellModel<CustomGridCellRenderer>
    {

    }

    public class CustomGridCellRenderer : GridVirtualizingCellRenderer<UserControl1>
    {
        public CustomGridCellRenderer()
        { }
        public override void CreateRendererElement(UserControl1 uiElement, GridRenderStyleInfo style)
        {
            if (this.GridControl.Tag != null)
            {
                var range = this.GridControl.Model.SelectedRanges;
                var grid = uiElement.child;
                if (range != null && range.AnyRangeContains(GridRangeInfo.Cell(style.CellIdentity.RowIndex, style.CellIdentity.ColumnIndex)))
                {
                    if (grid.Model.SelectedRanges.Count == 0)
                    {
                        grid.Model.SelectedRanges.Add(GridRangeInfo.Table());
                        grid.InvalidateCell(GridRangeInfo.Cell(0, 0));
                    }
                }
                else
                {
                    grid.Model.SelectedRanges.Clear();
                    grid.InvalidateCells();
                }
            }
            base.CreateRendererElement(uiElement, style);
        }
    }
}

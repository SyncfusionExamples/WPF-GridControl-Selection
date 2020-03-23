using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GridControl_movecursor
{
    public class ExcelLikeDragDropMouseController : IMouseController
    {
        private GridControlBase grids;
        private GridDragDropFlags dragDropFlags;
        private ExcelLikeDragDropHitTestInfo hitTestInfo = null;

        public ExcelLikeDragDropMouseController(GridControlBase grid)
        {
            this.grids = grid;
        }

        public string Name
        {
            get
            {
                return "ExcelLikeDragDrop";
            }
        }

        private Cursor _cursor;
        public Cursor Cursor
        {
            get
            {
                return _cursor;
            }
            set
            {
                _cursor = value;
            }
        }

        void IMouseController.MouseHover(MouseControllerEventArgs e)
        {
            
        }

        void IMouseController.MouseHoverEnter(MouseEventArgs e)
        {
            
        }

        void IMouseController.MouseHoverLeave(MouseEventArgs e)
        {
            
        }

        public bool SupportsCancelMouseCapture
        {
            get
            {
                return false;
            }
        }

        public bool SupportsMouseTracking
        {
            get
            {
                return false;
            }
        }

        public bool EnableExcelLikeDragDrop(GridDragDropFlags flags)
        {
            this.dragDropFlags = flags;
            return true;
        }

        public void MouseDown(MouseControllerEventArgs e)
        {
            if (hitTestInfo != null && e.Button == MouseButton.Left)
            {
                var mouseButtonEventArgs = e.SourceEventArgs as MouseButtonEventArgs;
                if (mouseButtonEventArgs != null && mouseButtonEventArgs.ClickCount == 2)
                {
                    if (this.grids.Model.SelectedRanges.Count > 0)
                    {
                        this.grids.Model.SelectedRanges.Clear();
                        this.grids.InvalidateCells();
                    }
                }
                else
                {
                    GridRangeInfo rgCovered;
                    grids.Model.CoveredRanges.Find(hitTestInfo.rowIndex, hitTestInfo.columnIndex, out rgCovered);
                    if (e.ClickCount == 1)
                    {
                        if (this.StartDragDrop(rgCovered.Top, rgCovered.Left))
                        {
                            this.grids.MouseControllerDispatcher.ActiveController = null;
                        }
                    }
                }
            }
        }

        void IMouseController.CancelMode()
        {
            
        }
        void IMouseController.MouseMove(MouseControllerEventArgs e)
        {
            
        }

        void IMouseController.MouseUp(MouseControllerEventArgs e)
        {
            
        }

        void IMouseController.RestoreMode()
        {
            
        }

        public int HitTest(MouseControllerEventArgs e, IMouseController controller)
        {
            Point pt = new Point(e.Location.X, e.Location.Y);
            // This HitTest code has higher priority than "SelectCells"
            hitTestInfo = null;
            if ((e.Button == MouseButton.Left || e.ClickCount == 1))
            {
                if (controller == null
                    || controller.Name == "SelectCellsMouseController")
                {
                    hitTestInfo = new ExcelLikeDragDropHitTestInfo(grids, pt);
                    if (!hitTestInfo.hitTestResult)
                    {
                        hitTestInfo = null;
                    }
                }
            }

            if (hitTestInfo != null && hitTestInfo.hitTestResult)
            {
                return 1;
            }
            return 0;
        }

        internal sealed class ExcelLikeDragDropHitTestInfo
        {
            internal bool hitTestResult = false;
            internal Point point;
            internal Rect cellBounds = Rect.Empty;
            internal int clientCol;
            internal int clientRow;
            internal int rowIndex;
            internal int columnIndex;
            internal GridRangeInfoList pSelList;
            internal GridRangeInfo activeRange;
            internal bool bTableSel;
            public ExcelLikeDragDropHitTestInfo(GridControlBase grid, Point pt)
            {
                this.point = pt;
                var CurrentRowColumnIndex = grid.PointToCellRowColumnIndex(pt);
                clientCol = GetClientCol(grid, CurrentRowColumnIndex.ColumnIndex);
                clientRow = GetClientRow(grid, CurrentRowColumnIndex.RowIndex);
                if (clientCol >= 0 && clientRow >= 0)
                {
                    rowIndex = GetRow(grid, clientRow);
                    columnIndex = GetColumn(grid, clientCol);
                    if (rowIndex <= grid.Model.RowCount && columnIndex <= grid.Model.ColumnCount)
                    {
                        cellBounds = grid.RangeToClippedVisibleRect(GridRangeInfo.Cell(rowIndex, columnIndex));
                        int edge = 4;
                        bool bSelEdge = false;
                        pSelList = grid.Model.Selections.Ranges;
                        activeRange = GridRangeInfo.Empty;

                        bTableSel = pSelList.AnyRangeContains(GridRangeInfo.Table());
                        if (bTableSel)
                        {
                            bSelEdge = rowIndex == 0 && columnIndex == 0
                                        && ((point.X >= cellBounds.Left && point.X - cellBounds.Left <= edge)
                                        || (point.Y >= cellBounds.Top && point.Y - cellBounds.Top <= edge));
                        }
                        else
                        {
                            activeRange = pSelList.ActiveRange;
                            if (activeRange.IsEmpty)
                            {
                                GridRangeInfo rg;
                                grid.Model.CoveredRanges.Find(rowIndex, columnIndex, out rg);
                                if (rg != null && grid.CurrentCell.HasCurrentCellAt(rg.Top, rg.Left))
                                {
                                    activeRange = rg;
                                }
                            }

                            GridRangeInfo rgCell = GridRangeInfo.Cell(rowIndex, columnIndex);
                            if (pSelList.AnyRangeContains(rgCell))
                            {
                                bSelEdge = true;
                            }
                        }

                        if (bSelEdge && RaiseQueryCanOleDragRange(grid, activeRange))
                        {
                            hitTestResult = true;
                        }
                        else
                        {
                            hitTestResult = false;
                        }
                    }
                }
            }

            bool RaiseQueryCanOleDragRange(GridControlBase grid, GridRangeInfo range)
            {
                GridQueryCanDragRangeEventArgs e = new GridQueryCanDragRangeEventArgs(range);
                grid.RaiseQueryCanOleDragRange(e);
                return !e.Cancel;
            }

        }

        internal static int GetClientRow(GridControlBase grid, int currentRow)
        {
            VisibleLineInfo line = grid.ScrollRows.GetVisibleLineAtLineIndex(currentRow);
            return line.VisibleIndex;
        }

        internal static int GetClientCol(GridControlBase grid, int currentCol)
        {
            VisibleLineInfo line = grid.ScrollColumns.GetVisibleLineAtLineIndex(currentCol);
            if (line != null)
            {
                return line.VisibleIndex;
            }
            return -1;
        }

        internal static int GetRow(GridControlBase grid, int currentRow)
        {
            if (grid.ScrollRows.GetVisibleLines().Count > currentRow)
            {
                VisibleLineInfo line = grid.ScrollRows.GetVisibleLines()[currentRow];
                return line.LineIndex;
            }
            else
            {
                return -1;
            }
        }

        internal static int GetColumn(GridControlBase grid, int currentCol)
        {
            if (currentCol > -1)
            {
                VisibleLineInfo line = grid.ScrollColumns.GetVisibleLines()[currentCol];
                if (line != null)
                {
                    return line.LineIndex;
                }
            }
            return -1;
        }

        private bool StartDragDrop(int rowIndex, int colIndex)
        {
            if (this.dragDropFlags == GridDragDropFlags.Disabled)
            {
                return false;
            }

            GridRangeInfoList selList;
            DragDropEffects dropEffect;

            // If user did click into a selected cell (or current cell), start OLE drag-and-drop.
            if (grids.Model.Selections.GetSelectedRanges(out selList, true) && selList.AnyRangeContains(GridRangeInfo.Cell(rowIndex, colIndex)))
            {
                bool bMulti = (this.dragDropFlags & GridDragDropFlags.Multiple) != 0;

                if (!bMulti)
                {
                    // Build up a single-range list.
                    GridRangeInfo rg = selList.GetRangesContaining(GridRangeInfo.Cell(rowIndex, colIndex)).ActiveRange;
                    selList = new GridRangeInfoList();
                    selList.Add(rg);
                }

                grids.Model.DragDropData.dndForceDropCol = int.MaxValue - 1;
                grids.Model.DragDropData.dndForceDropRow = int.MaxValue - 1;

                // Check if we should exclude headers.
                int nFirstRow = 0;
                int nFirstCol = 0;

                if ((this.dragDropFlags & GridDragDropFlags.RowHeader) != 0)
                {
                    nFirstCol = grids.Model.HeaderColumns + 1;
                }

                if ((this.dragDropFlags & GridDragDropFlags.ColHeader) != 0)
                {
                    nFirstRow = grids.Model.HeaderRows + 1;
                }

                // Loop through all selected ranges and expand them.
                // Row and column headers will be excluded from expanded range.
                foreach (GridRangeInfo rangeItem in selList)
                {
                    // If a whole row or column is dragged, allow
                    // it only to paste it as a whole row or column
                    // if pasted into the same grid.
                    if (rangeItem.IsTable)
                    {
                        grids.Model.DragDropData.dndForceDropCol = nFirstCol;
                        grids.Model.DragDropData.dndForceDropRow = nFirstRow;
                    }
                    else if (rangeItem.IsRows)
                    {
                        grids.Model.DragDropData.dndForceDropCol = nFirstCol;
                    }
                    else if (rangeItem.IsCols)
                    {
                        grids.Model.DragDropData.dndForceDropRow = nFirstRow;
                    }
                }

                selList = selList.ExpandRanges(
                    nFirstRow,
                    nFirstCol,
                    grids.Model.RowCount,
                    grids.Model.ColumnCount);

                grids.Model.DragDropData.dndCurrentCellText = false;
                grids.Model.DragDropData.dndCurrentCellControl = null;

                // Copy data to the OLE data source object
                DataObject dataObject = new DataObject();

                int dndRowsCopied;
                int dndColsCopied;

                // Raises OnQueryOleDataSourceData event.
                this.OnCacheGlobalData(dataObject, selList, out dndRowsCopied, out dndColsCopied);


                //To determine the control name while dropping to avoid the drag and drop within the control
                var data = dataObject.GetData("Text");
                if (data != null)
                {
                    data = this.grids.Name + ":" + data.ToString();
                }
                dataObject.SetData("Text", data);



                GridModel.InternalGridDragDropData.dndRowsCopied = dndRowsCopied;
                GridModel.InternalGridDragDropData.dndColsCopied = dndColsCopied;

                //// Store settings in parameter object so that when the
                //// user drops data into a different grid window
                //// which is bound to the same parameter object can use
                //// this information.

                grids.Model.DragDropData.dndSource = true;
                grids.Model.DragDropData.dndSelList = selList;
                grids.Model.DragDropData.dndStartRow = Math.Min(grids.Model.DragDropData.dndForceDropRow, Math.Max(rowIndex, nFirstRow));
                grids.Model.DragDropData.dndStartCol = Math.Min(grids.Model.DragDropData.dndForceDropCol, Math.Max(colIndex, nFirstCol));
                GridRangeInfo pRange = selList.ActiveRange;
                if (selList.Count == 1 && !pRange.IsEmpty)
                {
                    // Adjust start row / col to avoid that when
                    // the programmer selects a range of cells and
                    // drags the cells by selecting the lower right corner
                    // of the range that this corner becomes the upper left corner.
                    grids.Model.DragDropData.dndRowOffset = Math.Max(grids.Model.DragDropData.dndStartRow, pRange.Top) - pRange.Top;
                    grids.Model.DragDropData.dndColOffset = Math.Max(grids.Model.DragDropData.dndStartCol, pRange.Left) - pRange.Left;
                    grids.Model.DragDropData.dndStartRow = Math.Min(grids.Model.DragDropData.dndStartRow, pRange.Top);
                    grids.Model.DragDropData.dndStartCol = Math.Min(grids.Model.DragDropData.dndStartCol, pRange.Left);
                }
                else
                {
                    grids.Model.DragDropData.dndRowOffset = 0;
                    grids.Model.DragDropData.dndColOffset = 0;
                }

                // Show a drop cursor as soon as the user drags out
                // of the current cell.
                Rect cellBounds = grids.RangeToClippedVisibleRect(GridRangeInfo.Cell(rowIndex, colIndex));//, GridRangeOptions.MergeCoveredCells);
                                                                                                          //cellBounds = grid.GridRectangleToScreen(cellBounds);

                // Initiallize static member.
                GridModel.InternalGridDragDropData.dndGridSource = true; // data source is a GridControlBase grid
                GridModel.InternalGridDragDropData.dndGridTargetStyle = false; // Droptarget is a grid and style info is copied

                //if (gridData != null && gridData.Name != this.grid.Name)
                {
                    // Start the drag-and-drop operation.
                    if ((this.grids.Model.Options.DragDropDropTargetFlags & GridDragDropFlags.Move) == GridDragDropFlags.Move)
                        dropEffect = DragDrop.DoDragDrop(this.grids, dataObject, DragDropEffects.Copy | DragDropEffects.Move);
                    else
                        dropEffect = DragDrop.DoDragDrop(this.grids, dataObject, DragDropEffects.Copy);

                    // Remove selected cells if move operation.
                    if ((dropEffect & DragDropEffects.Move) == DragDropEffects.Move)
                    {
                        CancelEventArgs e = new CancelEventArgs();
                        grids.Model.RaiseQueryDragDropMoveClearCells(e);
                        if (!e.Cancel)
                        {
                            // dndCurrentCellText is set in OnDndCacheGlobalData when
                            // selected text from the current cell is copied to clipboard.
                            if (grids.Model.DragDropData.dndCurrentCellText)
                            {
                                // IGridCellRenderer pCtrl = grid.Model.DragDropData.dndCurrentCellControl; Unused local variable
                                grids.CurrentCell.EndEdit();
                            }
                            //else
                            //{
                            //    grid.Model.ClearCells(selList, GridModel.InternalGridDragDropData.dndGridTargetStyle);
                            //}
                        }
                    }
                }
                // Reset static member.
                GridModel.InternalGridDragDropData.dndGridSource = false;
                GridModel.InternalGridDragDropData.dndGridTargetStyle = false;

                // Reset settings in parameter object.
                grids.Model.DragDropData.dndSource = false;
                grids.Model.DragDropData.dndStartRow = 0;
                grids.Model.DragDropData.dndStartCol = 0;
                grids.Model.DragDropData.dndSelList = null;
                grids.Model.DragDropData.dndCurrentCellText = false;
                grids.Model.DragDropData.dndCurrentCellControl = null;

                return true;
            }
            return false;
        }

        bool OnCacheGlobalData(IDataObject dataObject, GridRangeInfoList selList, out int nDndRowExt, out int nDndColExt)
        {
            nDndRowExt = nDndColExt = 0;

            GridQueryOleDataSourceDataEventArgs e = new GridQueryOleDataSourceDataEventArgs(dataObject, selList, this.dragDropFlags, 0, 0);
            grids.Model.RaiseQueryOleDataSourceData(e);
            this.dragDropFlags = e.DragDropFlags;
            nDndRowExt = e.RowCount;
            nDndColExt = e.ColCount;
            if (e.Handled)
            {
                return e.Result;
            }

            bool bText = (this.grids.Model.Options.DataObjectConsumerOptions & GridDataObjectConsumerOptions.Text) != 0;
            bText &= ((this.grids.Model.Options.DragDropDropTargetFlags & GridDragDropFlags.Text) != 0);
            bool bStyles = (this.grids.Model.Options.DataObjectConsumerOptions & GridDataObjectConsumerOptions.Styles) != 0;
            bStyles &= ((this.grids.Model.Options.DragDropDropTargetFlags & GridDragDropFlags.Styles) != 0);
            bool retVal = false;

            bool clear = false;//flag for clearing the data. Always set to false.

            // Copy text
            if (bText)
            {
                string s = string.Empty;
                if (grids.CurrentCell.Renderer != null)
                {
                    s = grids.CurrentCell.Renderer.ControlText;
                }

                int rowIndex, colIndex;

                bool bCCell = grids.CurrentCell.GetCurrentCell(out rowIndex, out colIndex) && !e.IgnoreCurrentCell;

                if (selList.Count == 1 && bCCell && selList[0] == GridRangeInfo.Cell(rowIndex, colIndex)
                    //&& grid.CurrentCell.HasControlFocus && grid.CurrentCell.Renderer.ge.GetSelectedText(out s)
                    && s.Length > 0)
                {
                    dataObject.SetData(DataFormats.UnicodeText, s);
                    nDndRowExt = 1;
                    nDndColExt = 1;

                    // Mark this attribute if selected text from an edit control is dragged.
                    grids.Model.DragDropData.dndCurrentCellText = true;
                    grids.Model.DragDropData.dndCurrentCellControl = grids.CurrentCell.Renderer;
                }
                else if (grids.Model.TextDataExchange.CopyTextToBuffer(out s, selList, out nDndRowExt, out nDndColExt, clear))
                {
                    var gridData = dataObject.GetData(typeof(GridControl)) as GridControl;
                    //if (gridData != null && gridData.Name != grid.Name)
                    {
                        dataObject.SetData(DataFormats.UnicodeText, s);
                    }
                }
            }

            //Copy styles.
            if (bStyles)
            {
                GridCellData data = grids.Model.CutPaste.CopyCellsToDataObject(selList, clear);//GridUtil.IsSet(this.dragDropFlags, GridDragDropFlags.Compose));
                dataObject.SetData(data);
                retVal = true;
            }

            return retVal;
        }

    }
}

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

namespace Selection_copytext
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
            grid.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
            grid.Model.HeaderStyle.Borders.All = new Pen(Brushes.LightGray, 1);
            
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    grid.Model[1, 2].CellValue = "R" + grid.Model[1, 2].RowIndex + "C" + grid.Model[1, 2].ColumnIndex;
                    grid.Model[2, 4].CellValue = "R" + grid.Model[2, 4].RowIndex + "C" + grid.Model[2, 4].ColumnIndex;
                    grid.Model[3, 1].CellValue = "R" + grid.Model[3, 1].RowIndex + "C" + grid.Model[3, 1].ColumnIndex;
                    grid.Model[4, 5].CellValue = "R" + grid.Model[4, 5].RowIndex + "C" + grid.Model[4, 5].ColumnIndex;
                    grid.Model[5, 3].CellValue = "R" + grid.Model[5, 3].RowIndex + "C" + grid.Model[5, 3].ColumnIndex;
                    grid.Model[6, 2].CellValue = "R" + grid.Model[6, 2].RowIndex + "C" + grid.Model[6, 2].ColumnIndex;
                    grid.Model[7, 5].CellValue = "R" + grid.Model[7, 6].RowIndex + "C" + grid.Model[7, 6].ColumnIndex;
                    grid.Model[8, 3].CellValue = "R" + grid.Model[8, 3].RowIndex + "C" + grid.Model[8, 3].ColumnIndex;
                    grid.Model[9, 5].CellValue = "R" + grid.Model[9, 5].RowIndex + "C" + grid.Model[9, 5].ColumnIndex;
                }
            }
            grid.Model.Options.CopyPasteOption |= CopyPaste.CopyText;
            grid.Model.Options.CopyPasteOption |= CopyPaste.PasteText;
        }
    }
}

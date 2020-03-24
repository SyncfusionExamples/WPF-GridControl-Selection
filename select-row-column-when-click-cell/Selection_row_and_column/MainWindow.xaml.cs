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

namespace Selection_row_and_column
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
            grid.Model.ColumnCount = 8;
            grid.Model.RowStyles[0].Borders.All = new Pen(Brushes.LightGray, 1);
            grid.Model.ColStyles[0].Borders.All = new Pen(Brushes.LightGray, 1);

            //Looping throught the cells and assigning the value based on the row and column index
            for (int i=0;i<10;i++)
            {
                for(int j=0;j<8;j++)
                {
                    this.grid.Model[i, j].CellValue = "R" + i + "C" + j;
                }
            }

            //To select a row or column while clicking the cell
            grid.Model.Options.AllowSelection = Syncfusion.Windows.Controls.Grid.GridSelectionFlags.Row | Syncfusion.Windows.Controls.Grid.GridSelectionFlags.Column;

        }
    }
}

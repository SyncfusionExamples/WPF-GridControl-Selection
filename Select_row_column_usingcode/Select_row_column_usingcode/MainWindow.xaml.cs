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

namespace Select_row_column_usingcode
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

            for(int i=0;i<10;i++)
            {
                for(int j=0;j<8;j++)
                {
                    grid.Model[i, j].CellValue = "R" + i + "C" + j;
                }
            }

            //Programmatically select the row and column
            grid.Model.Selections.Add(GridRangeInfo.Row(4));
            grid.Model.Selections.Add(GridRangeInfo.Col(3));

        }
    }
}

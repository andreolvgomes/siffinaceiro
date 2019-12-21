using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SIF.PesquisaViewSP
{
    ///
    ///http://techiethings.blogspot.com.br/2010/05/get-wpf-datagrid-row-and-cell.html
    ///

    internal class GetDataGridCell : IDisposable
    {
        public T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        ///Existe um método simples para obter o atual (selecionado) linha do DataGrid:
        ///
        public DataGridRow GetSelectedRow(System.Windows.Controls.DataGrid grid)
        {
            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }

        ///Também pode obter uma linha por seus índices:
        ///
        public DataGridRow GetRow(System.Windows.Controls.DataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // Pode ser virtualizado, pôr em vista e tente novamente

                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        ///Agora podemos ter uma célula de um DataGrid por uma linha existente:
        ///
        public DataGridCell GetCell(System.Windows.Controls.DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        ///Ou podemos simplesmente selecionar uma linha por seus índices
        ///
        public DataGridCell GetCell(System.Windows.Controls.DataGrid grid, int indexRow, int indexColumn)
        {
            DataGridRow rowContainer = GetRow(grid, indexRow);
            return GetCell(grid, rowContainer, indexColumn);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

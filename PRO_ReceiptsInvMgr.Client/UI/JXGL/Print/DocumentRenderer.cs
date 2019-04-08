using PRO_ReceiptsInvMgr.Domain.DataObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace PRO_ReceiptsInvMgr.Client.UI.JXGL.Print
{
    class DocumentRenderer : IDocumentRenderer
    {
        public void Render(FlowDocument doc, object data)
        {
            TableRowGroup group = doc.FindName("rowsDetails") as TableRowGroup;
            Style styleCell = doc.Resources["BorderedCell"] as Style;

            foreach (ExportRzjg item in ((PrintRzjg)data).ExportRzjgList)
            {
                TableRow row = new TableRow();
                var paragraph = new Paragraph(new Run(item.Xh.ToString()));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                TableCell cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Fpdm));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Fphm));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Kprq.ToString("yyyy-MM-dd")));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Xsfmc));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Je.ToString()));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Se.ToString()));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Slv.ToString()));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Rzjg));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Fplx));
                paragraph.Margin = new Thickness(0, 2, 0, 2);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);
                group.Rows.Add(row);
            }

            TableRowGroup group2 = doc.FindName("rowsDetails2") as TableRowGroup;
            foreach (ExportSlType item in ((PrintRzjg)data).ExportSlTypeList)
            {
                TableRow row = new TableRow();
                var paragraph = new Paragraph(new Run(item.Slv.ToString()));
                paragraph.Margin = new Thickness(0, 5, 0, 5);
                if (item.Slv == "总计")
                {
                    paragraph.FontWeight = FontWeights.Bold;
                }
                TableCell cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.count.ToString()));
                paragraph.Margin = new Thickness(0, 5, 0, 5);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Je.ToString()));
                paragraph.Margin = new Thickness(0, 5, 0, 5);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                paragraph = new Paragraph(new Run(item.Se.ToString()));
                paragraph.Margin = new Thickness(0, 5, 0, 5);
                cell = new TableCell(paragraph);
                cell.Style = styleCell;
                row.Cells.Add(cell);

                group2.Rows.Add(row);
            }
        }
    }
}

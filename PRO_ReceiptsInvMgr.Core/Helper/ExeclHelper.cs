// ***********************************************************************
// Assembly         : SSI.Application.Common
// Author           : leif.lu
// Created          : 04-04-2014
//
// Last Modified By : leif.lu
// Last Modified On : 04-04-2014
// ***********************************************************************
// <copyright file="ExeclHelper.cs" company="SSI">
//     Copyright (c) SSI. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Excel = DocumentFormat.OpenXml.Spreadsheet;
using OpenXml = DocumentFormat.OpenXml.Packaging;
using System.Data;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace PRO_ReceiptsInvMgr.Core.Helper
{
    /// <summary>
    /// Class Open Xml Excel Helper.
    /// </summary>
    public static class ExcelHelper
    {
        #region reader
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <param name="cellReference">The cell reference.</param>
        /// <returns>System.String.</returns>
        private static string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        /// <summary>
        /// Converts the column name to number.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentException"></exception>
        private static int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) { throw new ArgumentException("My error message", "columnName"); }

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                int current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        /// <summary>
        /// Gets the excel cell enumerator.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns>IEnumerator&lt;Cell&gt;.</returns>
        private static IEnumerator<Excel.Cell> GetExcelCellEnumerator(Excel.Row row)
        {
            int currentCount = 0;
            foreach (Excel.Cell cell in row.Descendants<Excel.Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Excel.Cell() { DataType = null, CellValue = new Excel.CellValue(string.Empty) };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        /// <summary>
        /// Reads the excel cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="workbookPart">The workbook part.</param>
        /// <returns>System.String.</returns>
        private static string ReadExcelCell(Excel.Cell cell, OpenXml.WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == Excel.CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable.Elements<Excel.SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            return (text ?? string.Empty).Trim();
        }

        /// <summary>
        /// Reads the excel.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>SLExcelData.</returns>
        public static SlExcelData ReadExcel(HttpPostedFileBase file, int FirstLine = 0)
        {
            var data = new SlExcelData();

            // Check if the file is excel
            if (file.ContentLength <= 0)
            {
                data.Status.Message = "You uploaded an empty file";
                return data;
            }

            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                data.Status.Message = "Please upload a valid excel file of version 2007 and above";
                return data;
            }

            // Open the excel document
            OpenXml.WorkbookPart workbookPart; List<Excel.Row> rows;
            try
            {
                var document = OpenXml.SpreadsheetDocument.Open(file.InputStream, false);
                workbookPart = document.WorkbookPart;

                var sheets = workbookPart.Workbook.Descendants<Excel.Sheet>();
                var sheet = sheets.First();
                data.SheetName = sheet.Name;

                var workSheet = ((OpenXml.WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                var columns = workSheet.Descendants<Excel.Columns>().FirstOrDefault();
                data.ColumnConfigurations = columns;

                var sheetData = workSheet.Elements<Excel.SheetData>().First();
                rows = sheetData.Elements<Excel.Row>().ToList();
            }
            catch (Exception e)
            {
                data.Status.Message = "Unable to open the file Error:" + e.Message;
                return data;
            }

            // Read the header
            if (rows.Count >= FirstLine)
            {
                var row = rows[FirstLine];
                var cellEnumerator = GetExcelCellEnumerator(row);
                while (cellEnumerator.MoveNext())
                {
                    var cell = cellEnumerator.Current;
                    var text = ReadExcelCell(cell, workbookPart).Trim();
                    data.Headers.Add(text);
                }
            }

            // Read the sheet data
            if (rows.Count >= FirstLine + 1)
            {
                for (var i = FirstLine + 1; i < rows.Count; i++)
                {
                    var dataRow = new List<string>();
                    data.DataRows.Add(dataRow);
                    var row = rows[i];
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    while (cellEnumerator.MoveNext())
                    {
                        var cell = cellEnumerator.Current;
                        var text = ReadExcelCell(cell, workbookPart).Trim();
                        dataRow.Add(text);
                    }
                }
            }

            return data;
        }
        #endregion

        #region write
        /// <summary>
        /// Columns the letter.
        /// </summary>
        /// <param name="intCol">The int col.</param>
        /// <returns>System.String.</returns>
        private static string ColumnLetter(int intCol)
        {
            var intFirstLetter = ((intCol) / 676) + 64;
            var intSecondLetter = ((intCol % 676) / 26) + 64;
            var intThirdLetter = (intCol % 26) + 65;

            var firstLetter = (intFirstLetter > 64) ? (char)intFirstLetter : ' ';
            var secondLetter = (intSecondLetter > 64) ? (char)intSecondLetter : ' ';
            var thirdLetter = (char)intThirdLetter;

            return string.Concat(firstLetter, secondLetter, thirdLetter).Trim();
        }

        /// <summary>
        /// Creates the text cell.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="index">The index.</param>
        /// <param name="text">The text.</param>
        /// <returns>Cell.</returns>
        private static Excel.Cell CreateTextCell(string header, UInt32 index, string text)
        {
            var cell = new Excel.Cell
            {
                DataType = Excel.CellValues.InlineString,
                CellReference = header + index
            };

            var istring = new Excel.InlineString();
            var t = new Excel.Text { Text = text };
            istring.AppendChild(t);
            cell.AppendChild(istring);
            return cell;
        }

        /// <summary>
        /// Generates the excel.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GenerateExcel(SlExcelData data)
        {
            var stream = new MemoryStream();
            var document = OpenXml.SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);

            var workbookpart = document.AddWorkbookPart();
            workbookpart.Workbook = new Excel.Workbook();
            var worksheetPart = workbookpart.AddNewPart<OpenXml.WorksheetPart>();
            var sheetData = new Excel.SheetData();

            worksheetPart.Worksheet = new Excel.Worksheet(sheetData);

            var sheets = document.WorkbookPart.Workbook.
                AppendChild<Excel.Sheets>(new Excel.Sheets());

            var sheet = new Excel.Sheet()
            {
                Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = data.SheetName ?? "Sheet 1"
            };
            sheets.AppendChild(sheet);

            // Add header
            UInt32 rowIdex = 0;
            var row = new Excel.Row { RowIndex = ++rowIdex };
            sheetData.AppendChild(row);
            var cellIdex = 0;

            foreach (var header in data.Headers)
            {
                row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++), rowIdex, header ?? string.Empty));
            }
            // Add the column configuration if available
            if (data.Headers.Count > 0 && data.ColumnConfigurations != null)
            {
                var columns = (Excel.Columns)data.ColumnConfigurations.Clone();
                worksheetPart.Worksheet.InsertAfter(columns, worksheetPart.Worksheet.SheetFormatProperties);
            }

            // Add sheet data
            foreach (var rowData in data.DataRows)
            {
                cellIdex = 0;
                row = new Excel.Row { RowIndex = ++rowIdex };
                sheetData.AppendChild(row);
                foreach (var callData in rowData)
                {
                    var cell = CreateTextCell(ColumnLetter(cellIdex++), rowIdex, callData ?? string.Empty);
                    row.AppendChild(cell);
                }
            }

            workbookpart.Workbook.Save();
            document.Close();

            return stream.ToArray();
        }

        /// <summary>
        /// Exprots the excel
        /// </summary>
        /// <param name="tempfilename">Excel Module FilePath</param>
        /// <param name="tables">DataSource</param>
        /// <returns></returns>
        public static string ExportExcel(Stream tempStream, string destfilename, Dictionary<string, DataTable> tables)
        {
            #region 导出Excel 

            FileStream file = new FileStream(destfilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            tempStream.CopyTo(file);
            tempStream.Close();
            file.Position = 0;
            file.Close();
            bool isMerge = false;
            using (OpenXml.SpreadsheetDocument document = OpenXml.SpreadsheetDocument.Open(destfilename, true))
            {
                //1.去掉公式链
                var chain = document.WorkbookPart.CalculationChainPart;
                document.WorkbookPart.DeletePart(chain);

                var sheets = document.WorkbookPart.Workbook.Descendants<Excel.Sheet>();

                foreach (Excel.Sheet sheet in sheets.ToArray())
                {
                    WorksheetPart sourceWorksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);//查找目标模版Sheet页

                    SetSheetValue(sheet, sourceWorksheetPart.Worksheet, document, tables, ref isMerge);

                    sourceWorksheetPart.Worksheet.Save();

                    document.WorkbookPart.Workbook.Save();
                }

                if (isMerge)
                {
                    MergeCellsAfterSave(document);
                }
            }

            #endregion
            return destfilename;
        }

        public static void SetSheetValue(Excel.Sheet sheet, Worksheet worksheet, OpenXml.SpreadsheetDocument document, Dictionary<string, DataTable> tables, ref bool isMerge)
        {
            if (tables == null || tables.Count == 0)
            {
                return;
            }

            var defineNames = document.WorkbookPart.Workbook.DefinedNames.Elements<Excel.DefinedName>();

            var excelrows = worksheet.Descendants<Excel.Row>().ToList();

            //2.去掉所有的formula
            foreach (var excelrow in excelrows)
            {
                var excelcells = excelrow.Descendants<Excel.Cell>();
                foreach (var cell in excelcells)
                {
                    if (cell.CellFormula != null)
                    {
                        cell.CellFormula = null;
                        if (cell.CellValue != null) { cell.CellValue.InnerXml = ""; }
                    }

                }
            }

            //F_开头的单元格
            var fix = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("F_") == 0);

            foreach (var f in fix)
            {
                string k = f.Name.Value.Substring(2);
                string[] d = k.Split('_');
                if (tables[d[0]] != null && tables[d[0]].Rows.Count > 0)
                {
                    string val = FormatVal(tables[d[0]].Rows[0][d[1]], d);
                    if (d[0] == "SF")
                    {
                        val += "%";
                    }
                    if (d.Length > 2 && (d[2] == "Start" || d[2] == "End"))
                    {
                        isMerge = true;
                        val = "$" + d[2] + "$" + tables[d[0]].Rows[0][d[1]];
                    }

                    worksheet.UpdateCell(GetColumnLetter(f.InnerText), (uint)GetRowIndex(f.InnerText), val);
                }
            }

            var dynC = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("C_") == 0).OrderBy(o => (uint)GetRowIndex(o.InnerText));
            if (dynC.Any())
            {
                foreach (var item in dynC)
                {
                    var tb = item.Name.Value.Substring(2);

                    var dcells = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("D_") == 0 && o.Name.Value.Contains(tb));

                    int addColumn = 0;
                    for (int i = 0; i < tables[tb].Rows.Count; i++)
                    {
                        foreach (var c in dcells)
                        {
                            int columnIndex = GetColumnIndex(c.InnerText) + addColumn;

                            uint rowIndex = (uint)GetRowIndex(c.InnerText);

                            string t = c.Name.Value.Substring(2);
                            string[] d = t.Split('_');
                            string val = FormatVal(tables[d[0]].Rows[i][d[1]], d);
                            UpdateCell(worksheet, ToColumnLetter(columnIndex), rowIndex, val);
                        }
                        addColumn++;
                    }
                }
            }

            //判断是否有动态区域
            var dynA = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("A_") == 0);
            if (dynA.Any())
            {
                foreach (var item in dynA)
                {
                    var tb = item.Name.Value.Substring(2);
                    var row = worksheet.GetRow((uint)GetRowIndex(item.InnerText));
                    var dcells = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("X_") == 0 && o.Name.Value.Contains(tb));
                    var tbs = tables.Where(it => it.Key.Contains("A_dic_")).OrderBy(it => it.Key);

                    foreach (var tab in tbs)
                    {
                        for (int i = 0; i < tab.Value.Rows.Count; i++)
                        {
                            var newrow = worksheet.CreateRow(row); //新增行
                            if (tbs.ToList().IndexOf(tab) == 0 && item.Name == "A_Guarantor")
                            {
                                if (i == 0)
                                {
                                    newrow.UpdateCell("A", "担保人财务概况");
                                }
                                else if (i == 1)
                                {
                                    newrow.UpdateCell("A", "（万元）");
                                }
                            }

                            string firstMergeColumnName = string.Empty;
                            string lastMergeColumnName = string.Empty;
                            foreach (var c in dcells)
                            {
                                string t = c.Name.Value.Substring(2);
                                string[] d = t.Split('_');

                                if (d[1].ToLower() == "no")
                                {
                                    if (tab.Value.Columns.Contains("rowindex") && tab.Value.Rows[i]["rowindex"] != DBNull.Value)
                                    {
                                        newrow.UpdateCell(GetColumnLetter(c.InnerText), tab.Value.Rows[i]["rowindex"].ToString());
                                    }
                                    else
                                    {
                                        newrow.UpdateCell(GetColumnLetter(c.InnerText), (i + 1).ToString());
                                    }
                                    continue;
                                }

                                string val = FormatVal(tab.Value.Rows[i][d[1]], d);

                                newrow.UpdateCell(GetColumnLetter(c.InnerText), val);

                                if (dcells.ToList().IndexOf(c) == 0)
                                {
                                    firstMergeColumnName = GetColumnLetter(c.InnerText);
                                }
                                if (dcells.ToList().IndexOf(c) == dcells.Count() - 1)
                                {
                                    lastMergeColumnName = GetColumnLetter(c.InnerText);
                                }

                                if (i == 0 && !string.IsNullOrEmpty(firstMergeColumnName) && !string.IsNullOrEmpty(lastMergeColumnName))
                                {
                                    MergeTwoCells(worksheet, firstMergeColumnName + newrow.RowIndex, lastMergeColumnName + newrow.RowIndex);
                                }
                            }
                        }
                        row.Hidden = new DocumentFormat.OpenXml.BooleanValue(true); //原来的模板行隐藏  
                    }
                }
            }

            //判断是否有动态(列表)
            var dyn = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("R_") == 0).OrderByDescending(o => (uint)GetRowIndex(o.InnerText));
            if (dyn.Any())
            {
                foreach (var item in dyn)
                {
                    var tb = item.Name.Value.Substring(2);
                    var row = worksheet.GetRow((uint)GetRowIndex(item.InnerText));
                    var dcells = defineNames.Where(o => o.InnerText.IndexOf(sheet.Name) == 0 && o.Name.Value.IndexOf("D_") == 0 && o.Name.Value.Contains(tb));

                    if (tables.ContainsKey(tb))
                    {
                        for (int i = 0; i < tables[tb].Rows.Count; i++)
                        {
                            var newrow = worksheet.CreateRow(row); //新增行

                            foreach (var c in dcells)
                            {
                                string t = c.Name.Value.Substring(2);
                                string[] d = t.Split('_');
                                if (d[1].ToLower() == "no")
                                {
                                    if (tables[tb].Columns.Contains("rowindex") && tables[tb].Rows[i]["rowindex"] != DBNull.Value)
                                    {
                                        newrow.UpdateCell(GetColumnLetter(c.InnerText), tables[tb].Rows[i]["rowindex"].ToString());
                                    }
                                    else
                                    {
                                        newrow.UpdateCell(GetColumnLetter(c.InnerText), (i + 1).ToString());
                                    }
                                    continue;
                                }
                                string val = FormatVal(tables[d[0]].Rows[i][d[1]], d);
                                newrow.UpdateCell(GetColumnLetter(c.InnerText), val);
                            }
                        }
                        row.Hidden = new DocumentFormat.OpenXml.BooleanValue(true); //原来的模板行隐藏  
                    }
                }
            }
        }

        //添加一个工作表  
        public static void AddWorksheetPart(WorkbookPart workbookPart, string sheetName, WorksheetPart tempPart)
        {
            //WorksheetPart  
            Sheet sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().FirstOrDefault(s => s != null && s.Name != null && s.Name == sheetName);
            WorksheetPart worksheetPart = null;

            worksheetPart = workbookPart.AddPart<WorksheetPart>(tempPart);

            //Worksheet  
            if (worksheetPart.Worksheet == null)
            {
                worksheetPart.Worksheet = new Worksheet();
            }
            //SheetData  
            if (worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault() == null)
            {
                worksheetPart.Worksheet.AppendChild(new SheetData());
            }
            worksheetPart.Worksheet.Save();
            //Sheet  
            if (sheet == null)
            {
                sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().LastOrDefault();

                UInt32Value sheetId = 1;
                if (sheet != null)
                {
                    sheetId = sheet.SheetId + 1;
                }
                workbookPart.Workbook.Sheets.AppendChild(new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = sheetId,
                    Name = sheetName
                });
            }
            workbookPart.Workbook.Save();

        }

        private static void MergeCellsAfterSave(SpreadsheetDocument document)
        {

            //1.去掉公式链
            var chain = document.WorkbookPart.CalculationChainPart;
            document.WorkbookPart.DeletePart(chain);

            var sheets = document.WorkbookPart.Workbook.Descendants<Excel.Sheet>();

            foreach (Excel.Sheet sheet in sheets)
            {

                OpenXml.WorksheetPart part = (OpenXml.WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                var excelrows = part.Worksheet.Descendants<Excel.Row>().ToList();

                string mergeStart = string.Empty;
                string mergeEnd = string.Empty;
                foreach (var excelrow in excelrows)
                {

                    var excelcells = excelrow.Descendants<Excel.Cell>();

                    foreach (var cell in excelcells)
                    {
                        if (cell.CellValue != null && !string.IsNullOrEmpty(cell.CellValue.InnerText))
                        {
                            if (cell.CellValue.InnerText.Contains("$Start$"))
                            {
                                mergeStart = cell.CellReference;
                                cell.CellValue.Text = cell.CellValue.InnerText.Replace("$Start$", "");
                            }
                            else if (cell.CellValue.InnerText.Contains("$End$"))
                            {
                                mergeEnd = cell.CellReference;
                                cell.CellValue.Text = string.Empty;
                            }
                        }

                    }
                }

                if (!string.IsNullOrEmpty(mergeStart) && !string.IsNullOrEmpty(mergeEnd))
                {
                    MergeTwoCells(part.Worksheet, mergeStart, mergeEnd);
                }


            }
        }

        // Given a document name, a worksheet name, and the names of two adjacent cells, merges the two cells.
        // When two cells are merged, only the content from one cell is preserved:
        // the upper-left cell for left-to-right languages or the upper-right cell for right-to-left languages.
        private static void MergeTwoCells(Worksheet sheet, string cell1Name, string cell2Name)
        {
            // Open the document for editing.

            if (sheet == null || string.IsNullOrEmpty(cell1Name) || string.IsNullOrEmpty(cell2Name))
            {
                return;
            }

            // Verify if the specified cells exist, and if they do not exist, create them.
            CreateSpreadsheetCellIfNotExist(sheet, cell1Name);
            CreateSpreadsheetCellIfNotExist(sheet, cell2Name);

            MergeCells mergeCells;
            if (sheet.Elements<MergeCells>().Any())
            {
                mergeCells = sheet.Elements<MergeCells>().First();
            }
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.
                if (sheet.Elements<CustomSheetView>().Any())
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<CustomSheetView>().First());
                }
                else if (sheet.Elements<DataConsolidate>().Any())
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<DataConsolidate>().First());
                }
                else if (sheet.Elements<SortState>().Any())
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<SortState>().First());
                }
                else if (sheet.Elements<AutoFilter>().Any())
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<AutoFilter>().First());
                }
                else if (sheet.Elements<Scenarios>().Any())
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<Scenarios>().First());
                }
                else if (sheet.Elements<ProtectedRanges>().Any())
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<ProtectedRanges>().First());
                }
                else if (sheet.Elements<SheetProtection>().Any())
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<SheetProtection>().First());
                }
                else if (sheet.Elements<SheetCalculationProperties>().Any())
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<SheetCalculationProperties>().First());
                }
                else
                {
                    sheet.InsertAfter(mergeCells, sheet.Elements<SheetData>().First());
                }
            }

            // Create the merged cell and append it to the MergeCells collection.
            MergeCell mergeCell = new MergeCell() { Reference = new StringValue(cell1Name + ":" + cell2Name) };
            mergeCells.Append(mergeCell);

            sheet.Save();
        }

        // Given a Worksheet and a cell name, verifies that the specified cell exists.
        // If it does not exist, creates a new cell. 
        private static void CreateSpreadsheetCellIfNotExist(Worksheet worksheet, string cellName)
        {
            UInt32 rowIndex = (UInt32)GetRowIndex(cellName);

            IEnumerable<Row> rows = worksheet.Descendants<Row>().Where(r => r.RowIndex.Value == rowIndex);

            // If the Worksheet does not contain the specified row, create the specified row.
            // Create the specified cell in that row, and insert the row into the Worksheet.
            if (!rows.Any())
            {
                Row row = new Row() { RowIndex = new UInt32Value(rowIndex) };
                Cell cell = new Cell() { CellReference = new StringValue(cellName) };
                row.Append(cell);
                worksheet.Descendants<SheetData>().First().Append(row);
                worksheet.Save();
            }
            else
            {
                Row row = rows.First();

                IEnumerable<Cell> cells = row.Elements<Cell>().Where(c => c.CellReference.Value == cellName);

                // If the row does not contain the specified cell, create the specified cell.
                if (!cells.Any())
                {
                    Cell cell = new Cell() { CellReference = new StringValue(cellName) };
                    row.Append(cell);
                    worksheet.Save();
                }
            }
        }

        public static string FormatVal(object val, string[] d)
        {
            string value = val is DateTime ? ((DateTime)val).ToString("yyyy-MM-dd") : val.ToString();

            if (!string.IsNullOrEmpty(value))
            {
                switch (val.GetType().ToString())
                {
                    case "System.DateTime":
                        value = ((DateTime)val).ToString("yyyy-MM-dd");
                        break;
                    case "System.Boolean":
                        value = (Boolean)val ? "是" : "否";
                        break;
                    case "System.DBNull":
                        return string.Empty;
                    default:
                        break;
                }

                if (d.Length > 2)
                {
                    NumberFormatInfo nfi = new CultureInfo("zh-CN", false).NumberFormat;

                    if (d[2] == "Y")
                    {
                        value = string.Format("{0:0.##}", Convert.ToDouble(val).ToString("C", nfi));
                    }
                    else if (d[2] == "y")
                    {
                        value = string.Format("{0:0.##}", Convert.ToDouble(val).ToString("N"));
                    }
                    else if (d[2] == "M")
                    {
                        value = val is DateTime ? ((DateTime)val).ToString("yyyy-MM") : val.ToString();
                    }
                    else if (d[2] == "P")
                    {
                        value = Convert.ToDouble(val).ToString() + "%";
                    }
                    else if (d[2] == "Start" || d[2] == "End")
                    {
                        value = val.ToString();
                    }
                    else
                    {
                        value = Convert.ToDouble(val).ToString() + d[2];
                    }
                }
            }

            return value;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Given a cell name, parses the specified cell to get the row index.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Row Index (ie. 2)</returns>
        public static int GetRowIndex(string cellReference)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellReference);

            return int.Parse(match.Value);
        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column index.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. C2)</param>
        /// <returns>Row Index (ie. 3)</returns>
        public static int GetColumnIndex(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            string name = match.Value.ToUpper();
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column index.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. C2)</param>
        /// <returns>Row Letter (ie. C)</returns>
        public static string GetColumnLetter(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            string name = match.Value.ToUpper();

            return name;
        }

        /// <summary>
        /// 将column从12345 转换成 ABCDE
        /// </summary>
        /// <param name="colindex"></param>
        /// <returns></returns>
        public static string ToColumnLetter(int colindex)
        {
            int quotient = colindex / 26;
            if (quotient > 0)
            {
                return ToColumnLetter(quotient) + Char.ConvertFromUtf32((colindex % 26) + 64).ToString();
            }
            else
            {
                return char.ConvertFromUtf32(colindex + 64).ToString();
            }
        }

        /// <summary>
        /// 得到一个cell区域的开始 如sheet1!A1:B3,返回{A1,B3} 或者 Sheet3!$B$3:$F$3 返回{B3,F3}
        /// </summary>
        /// <param name="celladdr">一个单元格的地址</param>
        /// <returns></returns>
        public static string[] GetCellAddr(string celladdr)
        {
            string cellAddr = celladdr;
            if (string.IsNullOrEmpty(cellAddr))
            {
                return new string []{ };
            }
            int i = cellAddr.IndexOf("!");
            if (i > 0)
            {
                cellAddr = cellAddr.Substring(i + 1);
            }
            return cellAddr.Replace("$", "").Trim().Split(':');
        }

        public static string GetSheetName(string celladdr)
        {
            if (string.IsNullOrEmpty(celladdr)) { return null; }
            int i = celladdr.IndexOf("!");
            if (i > 0)
            {

                if (celladdr.StartsWith("'"))
                {
                    return celladdr.Substring(1, i - 2);
                }
                else
                {
                    return celladdr.Substring(0, i);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 增加新的行
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string AddRow(string address)
        {
            var ad = address.Split(':');

            string startAddress = ad[0];
            string endAddress = ad[1];
            int start = GetRowIndex(startAddress);
            int end = GetRowIndex(startAddress);
            return startAddress.Replace(start.ToString(), (start + 1).ToString()) + ":" + endAddress.Replace(end.ToString(), (end + 1).ToString());
        }

        #endregion

        // Given a worksheet, a column name, and a row index, 
        // gets the cell at the specified column and 
        private static Excel.Cell GetCell(this Excel.Worksheet worksheet, string columnName, uint rowIndex)
        {
            Excel.Row row = GetRow(worksheet, rowIndex);

            if (row == null)
            {
                return new Excel.Cell();
            }

            return row.Elements<Excel.Cell>().First(c => string.Compare
                   (c.CellReference.Value, columnName +
                   rowIndex, true) == 0);
        }

        // Given a worksheet and a row index, return the row.
        public static Excel.Row GetRow(this Excel.Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<Excel.SheetData>().
              Elements<Excel.Row>().First(r => r.RowIndex == rowIndex);
        }

        public static void UpdateCellNumber(this Excel.Worksheet worksheet, string col, uint row, string text)
        {
            Excel.Cell cell = GetCell(worksheet, col, row);
            cell.CellValue = new Excel.CellValue(text);
            cell.DataType = new EnumValue<Excel.CellValues>(Excel.CellValues.Number);
        }

        public static void UpdateCell(this Excel.Worksheet worksheet, string col, uint row, string text)
        {
            var cell = GetCell(worksheet, col, row);

            cell.CellValue = new Excel.CellValue(text);
            cell.DataType = new EnumValue<Excel.CellValues>(Excel.CellValues.String);
        }

        public static void UpdateCell(this Excel.Row row, string col, string text)
        {
            var cell = row.Elements<Excel.Cell>().First(c => string.Compare(c.CellReference.Value, col + row.RowIndex, true) == 0);
            cell.CellValue = new Excel.CellValue(text);
            cell.DataType = new EnumValue<Excel.CellValues>(Excel.CellValues.String);
        }

        public static void UpdateCellNumber(this Excel.Row row, string col, string text)
        {
            var cell = row.Elements<Excel.Cell>().First(c => string.Compare(c.CellReference.Value, col + row.RowIndex, true) == 0);
            cell.CellValue = new Excel.CellValue(text);
            cell.DataType = new EnumValue<Excel.CellValues>(Excel.CellValues.Number);
        }

        public static WorksheetPart GetWorksheetPartByName(this SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Excel.Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Excel.Sheets>().
               Elements<Excel.Sheet>().Where(s => s.Name == sheetName);

            if (!sheets.Any())
            {
                // The specified worksheet does not exist.

                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;

        }

        public static Excel.Row CreateRow(this Excel.Worksheet worksheet, Excel.Row refRow)
        {
            Excel.SheetData sheetData = worksheet.GetFirstChild<Excel.SheetData>();
            uint newRowIndex = 0;
            var newRow = new Excel.Row() { RowIndex = refRow.RowIndex.Value };
            var cells = refRow.Elements<Excel.Cell>();
            if (refRow.Height != null)
            {
                newRow.Height = new DoubleValue(refRow.Height);
            }
            if (refRow.CustomHeight != null)
            {
                newRow.CustomHeight = new BooleanValue(refRow.CustomHeight);
            }
            foreach (Excel.Cell cell in cells)
            {
                Excel.Cell newCell = (Excel.Cell)cell.CloneNode(true);
                newCell.StyleIndex = new UInt32Value(cell.StyleIndex);
                newRow.Append(newCell);
            }

            IEnumerable<Excel.Row> rows = sheetData.Descendants<Excel.Row>().Where(r => r.RowIndex.Value >= refRow.RowIndex.Value);
            foreach (Excel.Row row in rows)
            {
                newRowIndex = System.Convert.ToUInt32(row.RowIndex.Value + 1);

                foreach (var cell in row.Elements<Excel.Cell>())
                {
                    // Update the references for reserved cells.
                    string cellReference = cell.CellReference.Value;
                    cell.CellReference = new StringValue(cellReference.Replace(row.RowIndex.Value.ToString(), newRowIndex.ToString()));
                }

                row.RowIndex = new UInt32Value(newRowIndex);
            }
            sheetData.InsertBefore(newRow, refRow);

            // process merge cell in cloned rows
            var mcells = worksheet.GetFirstChild<Excel.MergeCells>();
            if (mcells != null)
            {
                //处理所有动态行以下的merg
                var clonedMergeCells = mcells.Elements<Excel.MergeCell>().
                     Where(m => GetRowIndex(m.Reference.Value.Split(':')[0]) >= newRow.RowIndex.Value).ToList<Excel.MergeCell>();
                foreach (var cmCell in clonedMergeCells)
                {
                    cmCell.Reference.Value = AddRow(cmCell.Reference.Value);
                }

                //增加新的merg
                var newMergeCells = new List<Excel.MergeCell>();
                var rowMergeCells = mcells.Elements<Excel.MergeCell>().
                    Where(m => GetRowIndex(m.Reference.Value.Split(':')[0]) == refRow.RowIndex).ToList<Excel.MergeCell>();
                foreach (var mc in rowMergeCells)
                {
                    newMergeCells.Add(new Excel.MergeCell() { Reference = mc.Reference.Value.Replace(refRow.RowIndex.Value.ToString(), (newRow.RowIndex.Value).ToString()) });
                }

                uint count = mcells.Count.Value;
                mcells.Count = new UInt32Value(count + (uint)newMergeCells.Count);
                mcells.Append(newMergeCells.ToArray());
            }

            return newRow;
        }

        public static WorksheetPart InsertWorksheet(SpreadsheetDocument doc, string sheetnameParm)
        {
            string sheetname = sheetnameParm;
            IEnumerable<Sheet> sheets = doc.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetname);

            if (!sheets.Any())
            {
                uint newId = (uint)(doc.Package.GetRelationships().Count() + 4);
                string rId = "relId" + newId; // do not set this to rId...
                if (string.IsNullOrEmpty(sheetname)) { sheetname = string.Format("Sheet{0}", newId); }
                int sheetnumber = doc.WorkbookPart.WorksheetParts.Count() + 1;
                WorksheetPart wsp = doc.WorkbookPart.AddNewPart<WorksheetPart>(rId);
                doc.WorkbookPart.Workbook.Sheets.AppendChild<Sheet>(new Sheet() { Id = rId, SheetId = newId, Name = sheetname });
                doc.Package.CreateRelationship(new Uri("worksheets/sheet" + sheetnumber + ".xml", UriKind.Relative),
                System.IO.Packaging.TargetMode.Internal,
                "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet", rId);
                doc.Package.Flush();
                return wsp;
            }
            return (WorksheetPart)doc.WorkbookPart.GetPartById(sheets.First().Id);
        }

        ///<summary>

        /// Sheet拷贝,需要先通过SpreadsheetWriter.InsertWorksheet()函数创建,创建WorkSheetPart

        ///</summary>

        ///<param name="document"></param>

        ///<param name="sheetId"></param>

        ///<param name="newWorkSheetPart"></param>

        public static void CopySheet(SpreadsheetDocument document, Worksheet oldWorkSheet, WorksheetPart newWorkSheetPart)
        {

            WorkbookPart workbookPart = document.WorkbookPart;

            // WorksheetPart sourceWorksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheetId);//查找目标模版Sheet页

            newWorkSheetPart.Worksheet = (Worksheet)oldWorkSheet.CloneNode(true);

            //通过深拷贝的方式直接拷贝目标模版的数据格式部分的XML，其他部分可以不需要。

            workbookPart.Workbook.Save();

        }

    }

    /// <summary>
    /// Class SLExcelStatus.
    /// </summary>
    public class SlExcelStatus
    {
        public string Message { get; set; }
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }

    /// <summary>
    /// Class SLExcelData.
    /// </summary>
    public class SlExcelData
    {
        public SlExcelStatus Status { get; set; }
        public Excel.Columns ColumnConfigurations { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> DataRows { get; set; }
        public string SheetName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlExcelData"/> class.
        /// </summary>
        public SlExcelData()
        {
            Status = new SlExcelStatus();
            Headers = new List<string>();
            DataRows = new List<List<string>>();
        }
    }

}

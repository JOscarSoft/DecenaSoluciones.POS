using ClosedXML.Excel;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    /// <summary>
    /// A utility class for applying common styling to Excel reports.
    /// </summary>
    internal static class ReportHelper
    {
        private const string CurrencyFormat = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";

        /// <summary>
        /// Applies a standard header style to the first row of a worksheet.
        /// </summary>
        public static void ApplyHeaderStyle(IXLWorksheet worksheet)
        {
            var headerRow = worksheet.Row(1);
            headerRow.CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Font.Shadow = true;
            headerRow.Style.Font.FontColor = XLColor.White;
        }

        /// <summary>
        /// Applies currency formatting to a specific column in a worksheet.
        /// </summary>
        public static void FormatColumnAsCurrency(IXLWorksheet worksheet, int column)
        {
            worksheet.Column(column).Style.NumberFormat.Format = CurrencyFormat;
        }

        /// <summary>
        /// Writes a value to a cell.
        /// </summary>
        public static void WriteCell(IXLWorksheet sheet, int row, int col, XLCellValue value)
        {
            sheet.Cell(row, col).Value = value;
        }

        /// <summary>
        /// Writes a row with a label and a value.
        /// </summary>
        public static void WriteLabeledRow(IXLWorksheet sheet, int row, int colLabel, string label, int colValue, XLCellValue value)
        {
            sheet.Cell(row, colLabel).Value = label;
            sheet.Cell(row, colValue).Value = value;
        }

        /// <summary>
        /// Writes a currency value to a cell and applies styling.
        /// </summary>
        public static IXLCell WriteCurrencyRow(IXLWorksheet sheet, int row, int colLabel, string label, int colValue, decimal amount, XLColor? fontColor = null)
        {
            sheet.Cell(row, colLabel).Value = label;

            var cell = sheet.Cell(row, colValue);
            cell.Value = amount;
            cell.Style.NumberFormat.Format = CurrencyFormat;
            cell.Style.Font.Bold = true;

            if (fontColor != null)
            {
                cell.Style.Font.FontColor = fontColor;
            }
            return cell;
        }

        /// <summary>
        /// Applies a standard style to a totals row.
        /// </summary>
        public static void ApplyTotalsRowStyle(IXLRow row)
        {
            row.CellsUsed().Style.Font.Bold = true;
            row.CellsUsed().Style.Font.Shadow = true;
        }
    }
}
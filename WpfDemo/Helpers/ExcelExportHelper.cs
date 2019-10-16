/************************************************************************************* 
* 类 名 称：ExcelExportHelper 
* 文 件 名：ExcelExportHelper
* 创建时间：2019/10/16 9:31:37     
* 作  者：hueEnergy     
* 说   明：     
* 修改时间：     
* 修 改 人：
*************************************************************************************/
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace WpfDemo.Helpers
{
    public class ExcelExportHelper
    {
        public static string Export(ReportViewModel viewModel)
        {
            bool exportToday = true;

            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("能效报表");

                // worksheet.DefaultRowHeight = 20;

                worksheet.Cells.Style.Numberformat.Format = "@";
                worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                #region 填充标题
                worksheet.Cells[1, 1].Value = "设备名称";
                worksheet.Cells[1, 2].Value = viewModel.ColumnsTitle;

                int totalExcelColumnCount = viewModel.Columns.Count;
                if (exportToday)
                {
                    totalExcelColumnCount *= 2;
                }
                int startColumnIndex = 1;
                for (int i = 0; i < viewModel.Columns.Count; i++)
                {
                    int excelColumn = i + 1;
                    if (exportToday)
                    {
                        excelColumn = 2 * i + 1;
                    }
                    worksheet.Cells[2, excelColumn + startColumnIndex].Value = viewModel.Columns[i].DateInfo;
                    if (exportToday)
                    {
                        excelColumn += 1;
                        worksheet.Cells[2, excelColumn + startColumnIndex].Value = viewModel.Columns[i].ValueInfo;
                    }
                }


                #endregion

                #region 填充内容
                int totalExcelRowCount = viewModel.Data.Count;
                for (int i = 0; i < totalExcelRowCount; i++)
                {
                    int excelRowIndex = i + 3;
                    var rowData = viewModel.Data[i];

                    //第一列
                    worksheet.Cells[excelRowIndex, 1].Value = rowData.Name;
                    worksheet.Row(excelRowIndex).Height = 20;

                    for (int j = 0; j < viewModel.Columns.Count; j++)
                    {
                        int excelColumn = j + 1;
                        if (exportToday)
                        {
                            excelColumn = 2 * j + 1;
                        }
                        worksheet.Cells[excelRowIndex, excelColumn + startColumnIndex].Value = rowData.DataValue[j].DateInfo;
                        if (exportToday)
                        {
                            excelColumn += 1;
                            worksheet.Cells[excelRowIndex, excelColumn + startColumnIndex].Value = rowData.DataValue[j].ValueInfo;
                        }
                    }
                }

                #endregion


                #region 设置样式

                //1、 列头样式
                worksheet.Cells[1, 1, 2, 1].Merge = true;//合并单元格
                //worksheet.Cells["A1:A2"].Merge = true;
                //worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center; 


                for (int i = 1; i < 3; i++)
                {
                    var rowInfo = worksheet.Row(i);
                    rowInfo.Height = 22;
                    rowInfo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rowInfo.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 240, 225));

                    var borderColor = Color.FromArgb(223, 223, 223);
                    //borderColor = Color.FromArgb(255, 0, 0);

                    rowInfo.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rowInfo.Style.Border.Left.Color.SetColor(borderColor);

                    rowInfo.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rowInfo.Style.Border.Top.Color.SetColor(borderColor);

                    rowInfo.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rowInfo.Style.Border.Right.Color.SetColor(borderColor);

                    rowInfo.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowInfo.Style.Border.Bottom.Color.SetColor(borderColor);


                    rowInfo.Style.Font.Bold = true;
                }

                worksheet.Cells[1, 2, 1, totalExcelColumnCount].Merge = true;
                //worksheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //
                // worksheet.Cells[1, 1, totalExcelRowCount, totalExcelColumnCount].Style.Numberformat.Format = "@"; 

                worksheet.Cells.AutoFitColumns(0);
                worksheet.Column(worksheet.Cells[1, 1].Columns).Width = 13;
                #endregion

                var excelFile = new FileInfo("reportDemo.xlsx");
                if (excelFile.Exists)
                {
                    excelFile.Delete();
                }
                package.SaveAs(excelFile);
                return excelFile.FullName;
            }
        }
    }
}

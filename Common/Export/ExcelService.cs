using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.Export
{
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> Write<T>(IList<T> registers, bool rightToLeft = false, bool addRowNumber = false)
        {
            var registersTotalRows = registers.Count;

            using (var excelPackage = new ExcelPackage())
            {
                var excelWorksheet = excelPackage.Workbook.Worksheets.Add("خروجی");

                if (rightToLeft)
                {
                    excelWorksheet.View.RightToLeft = true;
                }

                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();

                int totalColumns = properties.Length;
                if (addRowNumber)
                {
                    totalColumns += 1;
                }

                int colIndex = 1;

                if (addRowNumber)
                {
                    excelWorksheet.Cells[1, colIndex].Value = "ردیف";
                    colIndex++;
                }

                for (var i = 0; i < properties.Length; i++)
                {
                    string value = properties[i].Name;

                    var attribute = properties[i].GetCustomAttribute(typeof(DisplayAttribute));
                    if (attribute != null)
                    {
                        value = (attribute as DisplayAttribute).Name;
                    }

                    excelWorksheet.Cells[1, colIndex].Value = value;
                    colIndex++;
                }

                int index = 0;
                for (int row = 2; row <= registersTotalRows + 1; row++)
                {
                    colIndex = 1;

                    if (addRowNumber)
                    {
                        excelWorksheet.Cells[row, colIndex].Value = index + 1;
                        if (rightToLeft)
                        {
                            excelWorksheet.Cells[row, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        colIndex++;
                    }

                    for (var i = 0; i < properties.Length; i++)
                    {
                        var value = properties[i].GetValue(registers[index], null);
                        Type propertyType = properties[i].PropertyType;
                        TypeCode typeCode = Type.GetTypeCode(propertyType);

                        switch (typeCode)
                        {
                            case TypeCode.String:
                                excelWorksheet.Cells[row, colIndex].Value = value;
                                break;

                            case TypeCode.Int32:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                            case TypeCode.Single:
                                excelWorksheet.Cells[row, colIndex].Value = value?.ToString();
                                break;

                            case TypeCode.Boolean:
                                excelWorksheet.Cells[row, colIndex].Value = (bool)value ? "Yes" : "No";
                                break;

                            case TypeCode.DateTime:
                                excelWorksheet.Cells[row, colIndex].Value = ((DateTime)value).ToString("dd/MM/yyyy HH:mm:ss");
                                break;

                            default:
                                excelWorksheet.Cells[row, colIndex].Value = value?.ToString();
                                break;
                        }

                        if (rightToLeft)
                        {
                            excelWorksheet.Cells[row, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        colIndex++;
                    }
                    index++;
                }

                excelWorksheet.Cells.AutoFitColumns();


                var excelTable = excelWorksheet.Tables.Add(
                    new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: registersTotalRows + 1, toColumn: totalColumns),
                    "TestRegisters"
                );

                var excelRange = excelWorksheet.Cells[1, 1, excelWorksheet.Dimension.End.Row, excelWorksheet.Dimension.End.Column];
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                excelTable.TableStyle = TableStyles.Light21;
                excelTable.ShowTotal = false;

                return await excelPackage.GetAsByteArrayAsync();
            }
        }
    }
    public interface IExcelService
    {
        Task<byte[]> Write<T>(IList<T> registers, bool rightToLeft = false, bool addRowNumber = false);
    }
}

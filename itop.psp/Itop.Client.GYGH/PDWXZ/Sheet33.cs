using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet33
    {

        fpcommon fc = new fpcommon();
        //工作表行数
        int rowcount = 0;
        //工作表列数据
        int colcount = 0;
        //工作表第一行的表名
        string title = "";
        //工作表标签名
        string sheetname = "";
        public void Build(FarPoint.Win.Spread.SheetView obj_sheet,int year,string ProjID,List<string[]> SxXjName)
        {
            //表格共9 行6 列
            rowcount = 9;
            colcount = 6;
            //工作表第一行的标题
            title = "表3‑3 "+year+"年铜陵市负荷统计表";
            //工作表名
            sheetname = "表3-3";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[2].Width = 90;
            obj_sheet.Columns[3].Width = 110;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[5].Width = 90;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 40;
            //写标题行内容
            Sheet_AddItem(obj_sheet, SxXjName);
            //写入求合公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 4, 2, 4, 1, 3, 2, 4);
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 2, 2, 1, 8, 2, 4);

            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //限制格式
            CellType(obj_sheet);
           
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName)
        {
            //2行标题内容
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "网供最大负荷（MW）");
            obj_sheet.SetValue(1, 3, "全社会最大用电用负荷（MW）");
            obj_sheet.SetValue(1, 4, "网供电量（亿kWh）");
            obj_sheet.SetValue(1, 5, "全社会用电量（亿kWh）");
            //写标题列内容

            for (int row = 0; row < SxXjName.Count; row++)
            {
                obj_sheet.SetValue(2 + row, 0, SxXjName[row][0].ToString());
                obj_sheet.SetValue(2 + row, 1, SxXjName[row][1].ToString());
                
            }
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
          
        }
        public void CellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            for (int row = 2; row < 8; row++)
            {
                if (row!=3)
                {
                    for (int col = 2; col < 6; col++)
                    {
                        fc.Sheet_UnLockedandCellNumber(obj_sheet, row, col);
                    }
                }
            }
        }
    }
}

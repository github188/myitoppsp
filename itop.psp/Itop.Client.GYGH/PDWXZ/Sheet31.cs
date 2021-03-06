using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
   public class Sheet31
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
            //表格共4 行8 列
            rowcount = 4;
            colcount = 8;
            //工作表第一行的标题
            title = "表3‑1  "+year+"年铜陵市供电企业数量统计表";
            //工作表名
            sheetname = "表3-1";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 80;
            obj_sheet.Columns[2].Width = 80;
            obj_sheet.Columns[3].Width = 80;
            obj_sheet.Columns[4].Width = 80;
            obj_sheet.Columns[5].Width = 60;
            obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 60;
            //设定表格行高度
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 60;
            //写标题行内容
            Sheet_AddItem(obj_sheet);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, SxXjName);
            //写入求和公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 3, 4, 1, 4, 3);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //设定格式
            CellType(obj_sheet);
           
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "地市");
            obj_sheet.AddSpanCell(1, 1, 1, 2);
            obj_sheet.SetValue(1, 1, "市辖供电区");
            obj_sheet.AddSpanCell(1, 3, 1, 5);
            obj_sheet.SetValue(1, 3, "县级供电区");

            //3行标题内容
            obj_sheet.SetValue(2, 1, "地（市）级供电企业个数（个）");
            obj_sheet.SetValue(2, 2, "市辖供电区个数（个）");
            obj_sheet.SetValue(2, 3, "供电企业个数（个）");
            obj_sheet.SetValue(2, 4, "其中：直供直管");
            obj_sheet.SetValue(2, 5, "控股");
            obj_sheet.SetValue(2, 6, "参股");
            obj_sheet.SetValue(2, 7, "代管");
        }
        public void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName)
        {
          string tiaojian="";
            for (int i = 0; i < SxXjName.Count-1; i++)
			{
                if (SxXjName[i][2].ToString()=="合计")
                {
                    i++;
                }
                tiaojian = " CAST(col1 as int)<=" + year + "  and ProjectID='"+ProjID+"' and SType='" + SxXjName[i][2].ToString()+"'";
                int QYsum = 0;
                if (Services.BaseService.GetObject("SelectPs_Table_Enterprise_CountAll",tiaojian)!=null)
                {
                    QYsum = (int)Services.BaseService.GetObject("SelectPs_Table_Enterprise_CountAll", tiaojian);
                }
                obj_sheet.SetValue(3, 2 + i, QYsum);
			}
            
            
            
        }
        public void CellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            fc.Sheet_UnLockedandCellNumber(obj_sheet, 3, 1);
        }

    }
}

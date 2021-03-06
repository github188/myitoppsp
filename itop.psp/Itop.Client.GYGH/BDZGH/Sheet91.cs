using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.BDZGH
{
    class Sheet91
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
        public void Build(FarPoint.Win.Spread.SheetView obj_sheet, string ProjID, List<string[]> SxXjName)
        {
            //表格共59 行19 列
            rowcount = 59;
            colcount = 19;
            //工作表第一行的标题
            title = "表9‑1  新建变电站建设型式及用地需求 （单位：座、km2）";
            //工作表名
            sheetname = "表9‑1";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[2].Width = 60;
            obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[5].Width = 40;
            obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 40;
            obj_sheet.Columns[8].Width = 60;
            obj_sheet.Columns[9].Width = 40;
            obj_sheet.Columns[10].Width = 60;
            obj_sheet.Columns[11].Width = 40;
            obj_sheet.Columns[12].Width = 60;
            obj_sheet.Columns[13].Width = 40;
            obj_sheet.Columns[14].Width = 60;
            obj_sheet.Columns[15].Width = 40;
            obj_sheet.Columns[16].Width = 60;
            obj_sheet.Columns[17].Width = 50;
            obj_sheet.Columns[18].Width = 60;
            //设定表格行高度
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 20;
            
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SxXjName);
            //写入公式
            int startrow = 3;
            int dylength = 2;
            int length = 4;
            int countcol = 12;
            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 2 * dylength * length, 5, 4, dylength * length, startrow + 1 * dylength * length, 5, countcol);
            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 5, 2, dylength * length, startrow + 6 * dylength * length, 5, countcol);
            fc.Sheet_WriteFormula_MoreColsum_NoSpan(obj_sheet, startrow, 7, obj_sheet.RowCount - startrow, 5, startrow, 17, 2);
            fc.Sheet_WriteFormula_OneCol_Anotercol_nopercent(obj_sheet, startrow, 17, 18, 4, obj_sheet.RowCount - startrow,4);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //限定格式
            CellType(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName)
        {
           
            //写标题行内容

            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, " 类型");
            obj_sheet.AddSpanCell(1, 2, 2, 1);
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 2, 1);
            obj_sheet.SetValue(1, 3, "建设型式");
            obj_sheet.AddSpanCell(1, 4, 2, 1);
            obj_sheet.SetValue(1, 4, "平均用地需求");
            obj_sheet.AddSpanCell(1, 5, 1, 2);
            obj_sheet.SetValue(1, 5, "2010年");
            obj_sheet.AddSpanCell(1, 7, 1, 2);
            obj_sheet.SetValue(1, 7, "2011年");
            obj_sheet.AddSpanCell(1, 9, 1, 2);
            obj_sheet.SetValue(1, 9, "2012年");
            obj_sheet.AddSpanCell(1, 11, 1, 2);
            obj_sheet.SetValue(1, 11, "2013年");
            obj_sheet.AddSpanCell(1, 13, 1, 2);
            obj_sheet.SetValue(1, 13, "2014年");
            obj_sheet.AddSpanCell(1, 15, 1, 2);
            obj_sheet.SetValue(1, 15, "2015年");
            obj_sheet.AddSpanCell(1, 17, 1, 2);
            obj_sheet.SetValue(1, 17, "“十二五”合计");

            //3行标题内容
            obj_sheet.SetValue(2, 5, "座数");
            obj_sheet.SetValue(2, 6, "用地需求");
            obj_sheet.SetValue(2, 7, "座数");
            obj_sheet.SetValue(2, 8, "用地需求");
            obj_sheet.SetValue(2, 9, "座数");
            obj_sheet.SetValue(2, 10, "用地需求");
            obj_sheet.SetValue(2, 11, "座数");
            obj_sheet.SetValue(2, 12, "用地需求");
            obj_sheet.SetValue(2, 13, "座数");
            obj_sheet.SetValue(2, 14, "用地需求");
            obj_sheet.SetValue(2, 15, "座数");
            obj_sheet.SetValue(2, 16, "用地需求");
            obj_sheet.SetValue(2, 17, "座数");
            obj_sheet.SetValue(2, 18, "用地需求");
            //写标题列内容

           
           
            //4列标题内容
            obj_sheet.SetValue(3, 3, "户外");
            obj_sheet.SetValue(4, 3, "半户内");
            obj_sheet.SetValue(5, 3, "户内");
            obj_sheet.SetValue(6, 3, "其它");
            //写标题列内容
            List<string> obj_DY_List = new List<string>();
            obj_DY_List.Add("110（66）");
            obj_DY_List.Add("35");

            int startrow = 3;
            int dylength = obj_DY_List.Count;
            int length = 4;
            if (obj_DY_List.Count > 0)
            {
                for (int i = 0; i < SxXjName.Count; i++)
                {
                    for (int j = 0; j < obj_DY_List.Count; j++)
                    {

                        int row = startrow + i * dylength * length + j * length;
                        obj_sheet.SetValue(row, 3, "户外");
                        obj_sheet.SetValue(row + 1, 3, "半户内");
                        obj_sheet.SetValue(row + 2, 3, "户内");
                        obj_sheet.SetValue(row + 3, 3, "其它");
                        obj_sheet.AddSpanCell(startrow + i * dylength * length + j * length, 2, length, 1);
                        obj_sheet.SetValue(startrow + i * dylength * length + j * length, 2, obj_DY_List[j].ToString());
                    }
                    obj_sheet.AddSpanCell(startrow + i * dylength * length, 0, dylength * length, 1);
                    obj_sheet.SetValue(startrow + i * dylength * length, 0, SxXjName[i][0].ToString());
                    obj_sheet.AddSpanCell(startrow + i * dylength * length, 1, dylength * length, 1);
                    obj_sheet.SetValue(startrow + i * dylength * length, 1, SxXjName[i][1].ToString());
                }
            }
            

        }
        public void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet,string ProjID, List<string[]> SxXjName)
        {
           
            
        }
        public void CellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            int startrow = 3;
            int dylength = 2;
            int length = 4;
            for (int row = startrow; row < obj_sheet.RowCount - dylength * length; row++)
            {
                if (row==startrow+dylength*length)
                {
                    row = row + dylength * length;
                }
                for (int col = 5; col < 17; col++)
			    {
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, col);
			    }
                
            }
        }
       

    }
}

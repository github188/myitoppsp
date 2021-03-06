using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.DYGH
{
    class Sheet51
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
            //表格共9 行9 列
            rowcount = 9;
            colcount = 9;
            //工作表第一行的标题
            title = "表5‑1  铜陵市接入110kV及以下配电网的常规能源电厂装机容量情况（MW）";
            //工作表名
            sheetname = "表5‑1";
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
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[5].Width = 90;
            obj_sheet.Columns[6].Width = 90;
            obj_sheet.Columns[7].Width = 90;
            obj_sheet.Columns[8].Width = 90;
            //设定表格行高度
            obj_sheet.Rows[1].Height = 20;
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SxXjName);
            //写入数据
            Sheet_AddData(obj_sheet, ProjID, SxXjName);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName)
        {
            int startrow = 2;
            //2行标题内容
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "2010年");
            obj_sheet.SetValue(1, 3, "2011年");
            obj_sheet.SetValue(1, 4, "2012年");
            obj_sheet.SetValue(1, 5, "2013年");
            obj_sheet.SetValue(1, 6, "2014年");
            obj_sheet.SetValue(1, 7, "2015年");
            obj_sheet.SetValue(1, 8, "2020年");
            //写标题列内容

            for (int i = 0; i < SxXjName.Count; i++)
            {
                obj_sheet.SetValue(startrow + i, 0, SxXjName[i][0]);
                obj_sheet.SetValue(startrow + i, 1, SxXjName[i][1]);
            }
        }
        public void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet,string ProjID, List<string[]> SxXjName)
        {
            int startrow = 2;
            string tiaojian = "";
            string yeartiaojian = "";
            string GYorZB = "";
            for (int i = 0; i < SxXjName.Count; i++)
            {
                if (SxXjName[i][2] != "合计")
                {
                    for (int col = 2; col < 9; col++)
                    {
                        //2010年装机容量包括09年及以前的年份
                        if (col==2)
                        {
                            yeartiaojian = " CAST(S3 as int )<=2010 ";
                        }
                        //2020年装机容量包括2016到2020年
                        else if (col==8)
                        {
                            yeartiaojian = " CAST(S3 as int ) between 2016 and 2020 ";
                        }
                        //2011到2015每年单独统计
                        else
                        {
                            yeartiaojian = " CAST(S3 as int )=" + (2008 + col);
                        }
                        //计算装机容量
                        tiaojian = " AreaID='" + ProjID + "' and (S10='水电' or S10='煤电') and " + yeartiaojian + " and S5='" + SxXjName[i][2] + "' and CAST(S1 as int) between 10 and 110";
                        double ZJRLsum = 0;
                        if (Services.BaseService.GetObject("SelectPSP_PowerSubstation_Info_S2", tiaojian) != null)
                        {
                            ZJRLsum = (double)Services.BaseService.GetObject("SelectPSP_PowerSubstation_Info_S2", tiaojian);
                        }
                        obj_sheet.SetValue(startrow + i, col, ZJRLsum);
                    }
                                     
                }
                else
                {
                    if (i == 1)
                    {
                        //写入县级合计部分公式
                        fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 2, 2, 4, 1, startrow + 1, 2, 7);
                    }
                    else
                    {
                        //写入全地区合计公式
                        fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 2, 2, 1, startrow + i, 2, 7);
                    }
                }
            }
            
        }
       

    }
}

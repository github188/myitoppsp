using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet34
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
            //表格共10 行8 列
            rowcount = 10;
            colcount = 8;
            //工作表第一行的标题
            title = "表3‑4  "+year+"年铜陵市并网电厂装机容量表（MW）";
            //工作表名
            sheetname = "表3-4";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 110;
            obj_sheet.Columns[2].Width = 80;
            obj_sheet.Columns[3].Width = 80;
            obj_sheet.Columns[4].Width = 80;
            obj_sheet.Columns[5].Width = 80;
            obj_sheet.Columns[6].Width = 80;
            obj_sheet.Columns[7].Width = 80;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 20;
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SxXjName);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, SxXjName);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
           
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName)
        {
            int startrow = 3;
            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "110（66）kV");
            obj_sheet.AddSpanCell(1, 4, 1, 2);
            obj_sheet.SetValue(1, 4, "35kV");
            obj_sheet.AddSpanCell(1, 6, 1, 2);
            obj_sheet.SetValue(1, 6, "10（20）kV");

            //3行标题内容
            obj_sheet.SetValue(2, 2, "公用");
            obj_sheet.SetValue(2, 3, "自备");
            obj_sheet.SetValue(2, 4, "公用");
            obj_sheet.SetValue(2, 5, "自备");
            obj_sheet.SetValue(2, 6, "公用");
            obj_sheet.SetValue(2, 7, "自备");
            //写标题列内容
            for (int row = 0; row < SxXjName.Count; row++)
            {
                obj_sheet.SetValue(startrow + row, 0, SxXjName[row][0]);
                obj_sheet.SetValue(startrow + row, 1, SxXjName[row][1]);
            }
           

        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName)
        {
            int startrow = 3;
            string tiaojian = "";
            string dianya = "";
            string GYorZB = "";
            for (int i = 0; i < SxXjName.Count; i++)
            {
                if (SxXjName[i][2]!="合计")
                {
                    //固定循环两次，一次求公用，一次求自备
                    for (int j = 0; j < 2; j++)
                    {
                        if (j==0)
                        {
                            GYorZB = "公用";
                        }
                        else
                        {
                            GYorZB = "自备";
                        }
                        //求110（66）kV装机容量
                        dianya = " (S1='110' or S1='66')";
                        tiaojian = " AreaID='" + ProjID + "' and CAST(S3 as int ) <=" + year + " and S8='" + GYorZB + "' and  S5='"+SxXjName[i][2]+"' and " + dianya;
                        double ZJRLsum1 = 0;
                        if (Services.BaseService.GetObject("SelectPSP_PowerSubstation_Info_S2",tiaojian)!=null)
                        {
                            ZJRLsum1 = (double)Services.BaseService.GetObject("SelectPSP_PowerSubstation_Info_S2", tiaojian);
                        }
                        obj_sheet.SetValue(startrow + i, 2 + j, ZJRLsum1);
                        //求35kV装机容量
                        dianya = " S1='35'";
                        tiaojian = " AreaID='" + ProjID + "' and CAST(S3 as int ) <=" + year + " and S8='" + GYorZB + "' and  S5='" + SxXjName[i][2] + "' and " + dianya;
                        double ZJRLsum2 = 0;
                        if (Services.BaseService.GetObject("SelectPSP_PowerSubstation_Info_S2", tiaojian) != null)
                        {
                            ZJRLsum2 = (double)Services.BaseService.GetObject("SelectPSP_PowerSubstation_Info_S2", tiaojian);
                        }
                        obj_sheet.SetValue(startrow + i, 4 + j, ZJRLsum2);
                        //求10（20）kV装机容量
                        dianya = "  (S1='10' or S1='20')";
                        tiaojian = " AreaID='" + ProjID + "' and CAST(S3 as int ) <=" + year + " and S8='" + GYorZB + "' and  S5='" + SxXjName[i][2] + "' and " + dianya;
                        double ZJRLsum3 = 0;
                        if (Services.BaseService.GetObject("SelectPSP_PowerSubstation_Info_S2", tiaojian) != null)
                        {
                            ZJRLsum3 = (double)Services.BaseService.GetObject("SelectPSP_PowerSubstation_Info_S2", tiaojian);
                        }
                        obj_sheet.SetValue(startrow + i, 6 + j, ZJRLsum3);

                    }
                }
                else
                {
                    if (i==1)
                    {
                     //写入县级合计部分公式
                        fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 2, 2, 4, 1, startrow + 1, 2, 6);
                    }
                    else
                    {
                     //写入全地区合计公式
                        fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 2, 2, 1, startrow + i, 2, 6);
                    }
                }
            }
        }

    }
}

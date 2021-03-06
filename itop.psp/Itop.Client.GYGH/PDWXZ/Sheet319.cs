using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet319
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

            //线路电压等级根据实际情况查询
            //string XLDianYatiaojian = " (Type='05' or Type='50' or Type='51' or Type='52' or Type='55' or Type='56' or Type='57' or Type='58' ) ";
            //XLDianYatiaojian += " and (" + year + " -  year(cast(OperationYear as datetime)) )>=0 and ProjectID='" + ProjID + "' and RateVolt  between 6 and 20 group by RateVolt";

            string XLDianYatiaojian = " Type='05' and (" + year + " -  year(cast(OperationYear as datetime)) )>=0 and ProjectID='" + ProjID + "' and LineType2='公用' and RateVolt  between 6 and 20 group by RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //表标题行数
            int startrow = 3;
            //列标题每项行数
            int dylength = xlsum;
            int length = 5;

            //表格共 行10 列
            rowcount = startrow + 7 * dylength * length;
            colcount = 10;
            //工作表第一行的标题
            title = "表3‑19 "+year+"年铜陵市中压配电设备运行年限统计";
            //工作表名
            sheetname = "表3-19";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[2].Width = 80;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[4].Width = 80;
            obj_sheet.Columns[5].Width = 80;
            obj_sheet.Columns[6].Width = 80;
            obj_sheet.Columns[7].Width = 80;
            obj_sheet.Columns[8].Width = 80;
            obj_sheet.Columns[9].Width = 80;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 20;
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SxXjName, DY_list);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, SxXjName, DY_list);
            //写入求比例公式
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, startrow, 4, 7 * dylength, length, startrow, 5);
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, startrow, 6, 7 * dylength, length, startrow, 7);
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, startrow, 8, 7 * dylength, length, startrow, 9);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //写标题行内容

            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.AddSpanCell(1, 2, 2, 1);
            obj_sheet.SetValue(1, 2, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 3, 2, 1);
            obj_sheet.SetValue(1, 3, "年限");
            obj_sheet.AddSpanCell(1, 4, 1, 2);
            obj_sheet.SetValue(1, 4, "线路");
            obj_sheet.AddSpanCell(1, 6, 1, 2);
            obj_sheet.SetValue(1, 6, "配变");
            obj_sheet.AddSpanCell(1, 8, 1, 2);
            obj_sheet.SetValue(1, 8, "开关设备");

            //3行标题内容
            obj_sheet.SetValue(2, 4, "长度（km）");
            obj_sheet.SetValue(2, 5, "比例（%）");
            obj_sheet.SetValue(2, 6, "台数（台）");
            obj_sheet.SetValue(2, 7, "比例（%）");
            obj_sheet.SetValue(2, 8, "台数（台）");
            obj_sheet.SetValue(2, 9, "比例（%）");
            //写标题列内容
            int startrow = 3;
            int dylength = obj_DY_List.Count;
            int length = 5;
            if (obj_DY_List.Count > 0)
            {
                for (int i = 0; i < SxXjName.Count; i++)
                {
                    for (int j = 0; j < obj_DY_List.Count; j++)
                    {

                        int row = startrow + i * dylength * length + j * length;
                        obj_sheet.SetValue(row, 3, "0-5年");
                        obj_sheet.SetValue(row + 1, 3, "6-10年");
                        obj_sheet.SetValue(row + 2, 3, "11-15年");
                        obj_sheet.SetValue(row + 3, 3, "16-20年");
                        obj_sheet.SetValue(row + 4, 3, "20年以上");
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
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //添加数据
            //条件
            string tiaojian = "";
            int startrow = 3;
            int colcount = 5;
            int dylength = obj_DY_List.Count;
            int length = 5;
            string yeartiaojian = "";
            if (obj_DY_List.Count > 0)
            {
                for (int i = 0; i < SxXjName.Count; i++)
                {
                    //合计部分不用计算
                    if (SxXjName[i][2].ToString() != "合计")
                    {
                        for (int j = 0; j < obj_DY_List.Count; j++)
                        {
                            //固定循环5次，计算相应的年份
                            for (int k = 1; k <= 5; k++)
                            {
                                if (k == 1)
                                {
                                    yeartiaojian = " between 0 and 5  ";
                                }
                                if (k > 1 && k < 5)
                                {
                                    yeartiaojian = " between " + ((k - 1) * 5 + 1) + " and " + k * 5;
                                }
                                if (k == 5)
                                {
                                    yeartiaojian = " >20 ";
                                }
                                tiaojian = "(" + year + " -  year(cast(OperationYear as datetime)) )" + yeartiaojian + " and ProjectID='" + ProjID + "' and Type='05'  and LineType2='公用' and  DQ='" + SxXjName[i][2].ToString() + "' and RateVolt=" + obj_DY_List[j].ToString();
                                //线长为架空线+电缆线 
                                double Linelength = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUM_Length", tiaojian) != null)
                                {
                                    Linelength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUM_Length", tiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * dylength * length + j * length + k - 1, 4, Linelength);

                                //配变台数= 箱变51+柱上变52+配电室内配变50+开关站内配变55
                                //箱变台数 //柱上变台数
                                tiaojian = "(" + year + " -  year(cast(b.OperationYear as datetime)) )" + yeartiaojian + " and b.ProjectID='" + ProjID + "' and (b.Type='51' or b.Type='52')  and a.LineType2='公用' and  a.DQ='" + SxXjName[i][2].ToString() + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                                int XBsum = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                                {
                                    XBsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                                }
                                //配电室内配变台数 //开关站内配变台数
                                tiaojian = "(" + year + " -  year(cast(b.OperationYear as datetime)) )" + yeartiaojian + " and b.ProjectID='" + ProjID + "' and (b.Type='50' or b.Type='55') and a.LineType2='公用' and  a.DQ='" + SxXjName[i][2].ToString() + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                                int PDPBsum = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUMFlag", tiaojian) != null)
                                {
                                    PDPBsum = (int)Services.BaseService.GetObject("SelectPSPDEV_SUMFlag", tiaojian);
                                }
                                //写入配变台数
                                int PBDSSum = XBsum + PDPBsum;
                                obj_sheet.SetValue(startrow + i * dylength * length + j * length + k - 1, 6, PBDSSum);
                                //开关设备台数=配电室50+开关站55+环网柜56+柱上开关57+电缆分支箱58
                                tiaojian = "(" + year + " -  year(cast(b.OperationYear as datetime)) )" + yeartiaojian + " and b.ProjectID='" + ProjID + "' and (b.Type='50' or b.Type='55' or b.Type='56' or b.Type='57' or b.Type='58')  and a.LineType2='公用' and  a.DQ='" + SxXjName[i][2].ToString() + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                                int KGSBsum = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                                {
                                    KGSBsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * dylength * length + j * length + k - 1, 8, KGSBsum);
                            }
                        }
                    }
                    else
                    {
                        //县级合计部分公式
                        if (i == 1)
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * dylength * length, 4, 4, dylength * length, startrow + i * dylength * length, 4, colcount);

                        }
                        //全地区合计部分公式
                        else
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 4, 2, dylength * length, startrow + i * dylength * length, 4, colcount);
                        }
                    }

                }
            }
        }

    }
}

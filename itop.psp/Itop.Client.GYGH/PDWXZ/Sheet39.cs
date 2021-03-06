using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet39
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
            string XLDianYatiaojian = " Type='05' and OperationYear!='' and  year(cast(OperationYear as datetime))<="+year+" and  ProjectID='" + ProjID + "' and RateVolt  between 35 and 110 group by RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //表标题行数
            int startrow = 3;
            //列标题每项行数
            int length = xlsum;
            //表格共  行12 列
            rowcount = startrow + 7 * length;
            colcount = 12;
            //工作表第一行的标题
            title = "表3‑9  " + year + "年铜陵市110kV及以下高压配电网线路设备情况";
            //工作表名
            sheetname = "表3-9";
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
            obj_sheet.Columns[4].Width = 60;
            obj_sheet.Columns[5].Width = 60;
            obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 60;
            obj_sheet.Columns[8].Width = 60;
            obj_sheet.Columns[9].Width = 60;
            obj_sheet.Columns[10].Width = 60;
            obj_sheet.Columns[11].Width = 90;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 20;
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SxXjName, DY_list);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, SxXjName, DY_list);
            //写入求比例公式
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, startrow, 5, 7, 11, DY_list.Count * SxXjName.Count);
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
            obj_sheet.AddSpanCell(1, 3, 1, 2);
            obj_sheet.SetValue(1, 3, "线路条数（条）");
            obj_sheet.AddSpanCell(1, 5, 1, 2);
            obj_sheet.SetValue(1, 5, "线路总长度（km）");
            obj_sheet.AddSpanCell(1, 7, 1, 2);
            obj_sheet.SetValue(1, 7, "电缆线路长度（km）");
            obj_sheet.AddSpanCell(1, 9, 1, 2);
            obj_sheet.SetValue(1, 9, "架空线路长度（km）");
            obj_sheet.SetValue(1, 11, "电缆化率（%）");

            //3行标题内容
            obj_sheet.SetValue(2, 3, "公用");
            obj_sheet.SetValue(2, 4, "专用");
            obj_sheet.SetValue(2, 5, "公用");
            obj_sheet.SetValue(2, 6, "专用");
            obj_sheet.SetValue(2, 7, "公用");
            obj_sheet.SetValue(2, 8, "专用");
            obj_sheet.SetValue(2, 9, "公用");
            obj_sheet.SetValue(2, 10, "专用");
            obj_sheet.SetValue(2, 11, "公用");
            //写标题列内容
            fc.Sheet_AddItem_ZBonlyDY(obj_sheet, SxXjName, 3, obj_DY_List);
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //添加数据
            //公用or专用
            string GorZ = "";
            //条件
            string XLtiaojian = "";
            int startrow = 3;
            int length = obj_DY_List.Count;
            if (obj_DY_List.Count > 0)
            {
                for (int i = 0; i < SxXjName.Count; i++)
                {
                    //合计部分不用计算
                    if (SxXjName[i][2].ToString() != "合计")
                    {
                        for (int j = 0; j < obj_DY_List.Count; j++)
                        {
                            //obj_sheet.SetValue(startrow + i * length + j, 2, obj_DY_List[j].ToString());

                            //固定循环两次，计算公用和专用
                            for (int k = 0; k < 2; k++)
                            {
                                if (k == 0)
                                {
                                    GorZ = "公用";
                                }
                                else
                                {
                                    GorZ = "专用";
                                }
                                XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and ProjectID='" + ProjID + "' and Type='05' and LineType2='" + GorZ + "'  and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
                                //线路条数
                                int XLsum = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian) != null)
                                {
                                    XLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * length + j, 3 + k, XLsum);
                                //电缆线长
                                double DLXlength = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian) != null)
                                {
                                    DLXlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * length + j, 7 + k, DLXlength);
                                //架空线长
                                double JKXlength = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian) != null)
                                {
                                    JKXlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * length + j, 9 + k, JKXlength);
                                //写入全长公式
                                obj_sheet.Cells[startrow + i * length + j, 5 + k].Formula = "SUM(" + "R" + (startrow + i * length + j + 1) + "C" + (7 + k + 1) + "," + "R" + (startrow + i * length + j + 1) + "C" + (9 + k + 1) + ")";

                            }
                        }
                    }
                    else
                    {
                        //县级合计部分公式
                        if (i == 1)
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * length, 3, 4, length, startrow + i * length, 3, 8);
                        }
                        //全地区合计部分公式
                        else
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 3, 2, length, startrow + i * length, 3, 8);

                        }
                    }

                }
            }
        }

    }
}

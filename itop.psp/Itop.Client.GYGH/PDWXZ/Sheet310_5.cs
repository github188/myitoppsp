using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet310_5
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
        public void Build(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, Hashtable area_key_id, List<string[]> SxXjName)
        {
            //线路电压等级根据实际情况查询
            string XLDianYatiaojian = " Type='05' and (" + year + " -  year(cast(OperationYear as datetime)) )>=0 and ProjectID='" + ProjID + "' and LineType2='公用' and RateVolt  between 35 and 110 group by RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //计算市辖供电区条件 
            string SXtiaojianareaid = " Type='05' and (" + year + " -  year(cast(OperationYear as datetime)) )>=0  and AreaID!='' and  ProjectID='" + ProjID + "'  and LineType2='公用' and DQ='市辖供电区' and  RateVolt between 35 and 110  group by AreaID";
            //计算县级供电区条件
            string XJtiaojianareaid = " Type='05' and (" + year + " -  year(cast(OperationYear as datetime)) )>=0  and AreaID!='' and  ProjectID='" + ProjID + "' and LineType2='公用' and DQ!='市辖供电区'  and RateVolt between 35 and 110  group by AreaID";
            //存放市辖供电区分区名
            IList<string> SXareaid_List = Services.BaseService.GetList<string>("SelectPSPDEV_GroupAreaID", SXtiaojianareaid);
            //存放县级供电区分区名
            IList<string> XJareaid_List = Services.BaseService.GetList<string>("SelectPSPDEV_GroupAreaID", XJtiaojianareaid);

            //市辖供电区分区个数
            int SXsum = SXareaid_List.Count;
            //县级供电区分区个数
            int XJsum = XJareaid_List.Count;
            //表标题行数
            int startrow = 3;
            //列标题每项是压等级数
            int dylength = xlsum;
            //列标题每个电压等级行数
            int length = 5;

            //表格共 行8 列
            rowcount = startrow + (SXsum + XJsum + 2) * dylength * length;
            colcount = 8;
            //工作表第一行的标题
            title = "附表5  铜陵市110kV及以下高压配电网线路运行年限分布(" + year + "年)";
            //工作表名
            sheetname = "表3-10附表5";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 90;
            obj_sheet.Columns[1].Width = 90;
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
            Sheet_AddItem(obj_sheet, area_key_id, DY_list, SXareaid_List, XJareaid_List);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, DY_list, SXareaid_List, XJareaid_List);
            //写入求比例公式
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, startrow, 4, (SXareaid_List.Count + XJareaid_List.Count + 2) * dylength, length, startrow, 5);
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, startrow, 6, (SXareaid_List.Count + XJareaid_List.Count + 2) * dylength, length, startrow, 7);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet,Hashtable area_key_id, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            //写标题行内容

            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.AddSpanCell(1, 2, 2, 1);
            obj_sheet.SetValue(1, 2, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 3, 2, 1);
            obj_sheet.SetValue(1, 3, "年限");
            obj_sheet.AddSpanCell(1, 4, 1, 2);
            obj_sheet.SetValue(1, 4, "架空");
            obj_sheet.AddSpanCell(1, 6, 1, 2);
            obj_sheet.SetValue(1, 6, "电缆");

            //3行标题内容
            obj_sheet.SetValue(2, 4, "长度（km）");
            obj_sheet.SetValue(2, 5, "比例（%）");
            obj_sheet.SetValue(2, 6, "长度（km）");
            obj_sheet.SetValue(2, 7, "比例（%）");
            //写标题列内容
            int startrow = 3;
            int dylength = obj_DY_List.Count;
            int length = 5;
            if (obj_DY_List.Count > 0)
            {
                for (int i = 0; i < (2 + SXareaid_List.Count + XJareaid_List.Count); i++)
                {
                    string areaname = "";
                    if (i == 0 || i == (SXareaid_List.Count + 1))
                    {
                        areaname = "合计";
                    }
                    else
                    {
                        if (i < SXareaid_List.Count + 1)
                        {
                            if (area_key_id[SXareaid_List[i - 1].ToString()] != null)
                            {
                                areaname = area_key_id[SXareaid_List[i - 1].ToString()].ToString();
                            }
                            else
                            {
                                areaname = "";
                            }

                        }
                        else
                        {
                            if (area_key_id[XJareaid_List[i - SXareaid_List.Count - 2].ToString()] != null)
                            {
                                areaname = area_key_id[XJareaid_List[i - SXareaid_List.Count - 2].ToString()].ToString();
                            }
                            else
                            {
                                areaname = "";
                            }
                        }
                    }
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
                    obj_sheet.AddSpanCell(startrow + i * dylength * length, 1, dylength * length, 1);
                    obj_sheet.SetValue(startrow + i * dylength * length, 1, areaname);

                }
                //写第一列数据
                obj_sheet.AddSpanCell(startrow, 0, (SXareaid_List.Count + 1) * dylength * length, 1);
                obj_sheet.SetValue(startrow, 0, "市辖供电区");
                obj_sheet.AddSpanCell(startrow + (SXareaid_List.Count + 1) * dylength * length, 0, (XJareaid_List.Count + 1) * dylength * length, 1);
                obj_sheet.SetValue(startrow + (SXareaid_List.Count + 1) * dylength * length, 0, "县级供电区");
            }
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            //添加数据
            //条件
            string XLtiaojian = "";
            int startrow = 3;
            int dylength = obj_DY_List.Count;
            int length = 5;
            //DQ条件
            string DQtiaojain = "";
            string yeartiaojian = "";
            if (obj_DY_List.Count > 0)
            {
                for (int i = 0; i < (2 + SXareaid_List.Count + XJareaid_List.Count); i++)
                {
                    string areaid = "";
                    if (i != 0 && i != (SXareaid_List.Count + 1))
                    {
                        if (i < SXareaid_List.Count + 1)
                        {
                            DQtiaojain = " DQ='市辖供电区'";
                            areaid = SXareaid_List[i - 1].ToString();
                        }
                        else
                        {
                            DQtiaojain = " DQ!='市辖供电区'";
                            areaid = XJareaid_List[i - SXareaid_List.Count - 2].ToString();
                        }
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
                                XLtiaojian = "(" + year + " -  year(cast(OperationYear as datetime)) )" + yeartiaojian + " and ProjectID='" + ProjID + "' and Type='05' and AreaID='" + areaid + "' and LineType2='公用' and  " + DQtiaojain + "and RateVolt=" + obj_DY_List[j].ToString();
                                //架空线长
                                double JKXlength = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian) != null)
                                {
                                    JKXlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * dylength * length + j * length + k - 1, 4, JKXlength);
                                //电缆线长
                                double DLXlength = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian) != null)
                                {
                                    DLXlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * dylength * length + j * length + k - 1, 6, DLXlength);
                            }
                        }
                    }
                    else
                    {
                        //市辖合计部分公式
                        if (i == 0)
                        {
                            if (SXareaid_List.Count != 0)
                            {
                                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * dylength * length, 4, SXareaid_List.Count, dylength * length, startrow + i * dylength * length, 4, 4);
                            }
                            //无地区直接在合计部分写0
                            else
                            {
                                fc.Sheet_WriteZero(obj_sheet, startrow + i * dylength * length, 4, dylength * length, 4);
                            }

                        }
                        //县级合计部分公式
                        else
                        {
                            if (XJareaid_List.Count != 0)
                            {
                                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * dylength * length, 4, XJareaid_List.Count, dylength * length, startrow + i * dylength * length, 4, 4);

                            }
                            //无地区直接在合计部分写0
                            else
                            {
                                fc.Sheet_WriteZero(obj_sheet, startrow + i * dylength * length, 4, dylength * length, 4);
                            }
                        }
                    }
                }
            }
        }
    }
}

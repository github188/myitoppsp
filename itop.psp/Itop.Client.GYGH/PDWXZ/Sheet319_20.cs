using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet319_20
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
            //string XLDianYatiaojian = " (Type='05' or Type='50' or Type='51' or Type='52' or Type='55' or Type='56' or Type='57' or Type='58' ) ";
            //XLDianYatiaojian += " and (" + year + " -  year(cast(OperationYear as datetime)) )>=0 and ProjectID='" + ProjID + "' and RateVolt  between 6 and 20 group by RateVolt";
            //IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            //int xlsum = DY_list.Count;
            string XLDianYatiaojian = " Type='05' and (" + year + " -  year(cast(OperationYear as datetime)) )>=0 and ProjectID='" + ProjID + "' and LineType2='公用' and RateVolt  between 6 and 20 group by RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //这里的分区应包括两部分，一部分是线路条件查得的分区，另一部分是用设备查找所属线路所得分区
            //因为设备的OperationYear只会比线路晚，而ProjectID相同，RateVolt相同，所以用设备查所属线路
            //再查所得分区一定是线路分区的一个子集，所以查分区只用线路条件即可
            
            //计算市辖供电区条件 
            string SXtiaojianareaid = " Type='05' and (" + year + " -  year(cast(OperationYear as datetime)) )>=0  and AreaID!='' and  ProjectID='" + ProjID + "'  and LineType2='公用' and DQ='市辖供电区' and  RateVolt between 6 and 20  group by AreaID";
            //计算县级供电区条件
            string XJtiaojianareaid = " Type='05' and (" + year + " -  year(cast(OperationYear as datetime)) )>=0  and AreaID!='' and  ProjectID='" + ProjID + "' and LineType2='公用' and DQ!='市辖供电区'  and RateVolt between 6 and 20  group by AreaID";
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

            //表格共 行10 列
            rowcount = startrow + (SXsum + XJsum + 2) * dylength * length;
            colcount = 10;
            //工作表第一行的标题
            title = "附表20 铜陵市中压配电设备运行年限统计（" + year + "年）";
            //工作表名
            sheetname = "表3-19 附表20";
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
            Sheet_AddItem(obj_sheet, area_key_id, DY_list, SXareaid_List, XJareaid_List);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, DY_list, SXareaid_List, XJareaid_List);
            //写入求比例公式
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, startrow, 4, (SXsum + XJsum + 2) * dylength, length, startrow, 5);
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, startrow, 6, (SXsum + XJsum + 2) * dylength, length, startrow, 7);
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, startrow, 8, (SXsum + XJsum + 2) * dylength, length, startrow, 9);
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
                            if (area_key_id[SXareaid_List[i - 1].ToString()]!=null)
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
                            if (area_key_id[XJareaid_List[i - SXareaid_List.Count - 2].ToString()]!=null)
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
            string tiaojian = "";
            int startrow = 3;
            int dylength = obj_DY_List.Count;
            int colcount = 5;
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
                            DQtiaojain = "DQ='市辖供电区'";
                            areaid = SXareaid_List[i - 1].ToString();
                        }
                        else
                        {
                            DQtiaojain = "DQ!='市辖供电区'";
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
                                
                                tiaojian = "(" + year + " -  year(cast(OperationYear as datetime)) )" + yeartiaojian + " and ProjectID='" + ProjID + "' and Type='05'  and LineType2='公用' and  "+DQtiaojain+"  and  AreaID='"+areaid+"' and RateVolt=" + obj_DY_List[j].ToString();
                                //线长为架空线+电缆线 
                                double Linelength = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUM_Length", tiaojian) != null)
                                {
                                    Linelength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUM_Length", tiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * dylength * length + j * length + k - 1, 4, Linelength);

                                //配变台数= 箱变51+柱上变52+配电室内配变50+开关站内配变55
                                //箱变台数  //柱上变台数
                                tiaojian = "(" + year + " -  year(cast(b.OperationYear as datetime)) )" + yeartiaojian + " and b.ProjectID='" + ProjID + "' and (b.Type='51' or b.Type='52') and a.LineType2='公用' and  a." + DQtiaojain + " and a.AreaID='" + areaid + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                                int XBsum = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                                {
                                    XBsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                                }
                               
                                //配电室内配变台数  //开关站内配变台数
                                tiaojian = "(" + year + " -  year(cast(b.OperationYear as datetime)) )" + yeartiaojian + " and b.ProjectID='" + ProjID + "' and (b.Type='50' or b.Type='55')  and a.LineType2='公用' and  a." + DQtiaojain + " and a.AreaID='" + areaid + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                                int PDPBsum = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUMFlag", tiaojian) != null)
                                {
                                    PDPBsum = (int)Services.BaseService.GetObject("SelectPSPDEV_SUMFlag", tiaojian);
                                }
                                //写入配变台数
                                int PBDSSum = XBsum + PDPBsum;
                                obj_sheet.SetValue(startrow + i * dylength * length + j * length + k - 1, 6, PBDSSum);
                                //开关设备台数=配电室50+开关站55+环网柜56+柱上开关57+电缆分支箱58
                                tiaojian = "(" + year + " -  year(cast(b.OperationYear as datetime)) )" + yeartiaojian + " and b.ProjectID='" + ProjID + "' and (b.Type='50' or b.Type='55' or b.Type='56' or b.Type='57' or b.Type='58')  and a.LineType2='公用' and  a." + DQtiaojain + " and a.AreaID='" + areaid + "' and b.RateVolt=" + obj_DY_List[j].ToString();
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
                        //市辖合计部分公式
                        if (i == 0)
                        {
                            if (SXareaid_List.Count != 0)
                            {
                                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * dylength * length, 4, SXareaid_List.Count, dylength * length, startrow + i * dylength * length, 4, colcount);
                            }
                            //无地区直接在合计部分写0
                            else
                            {
                                fc.Sheet_WriteZero(obj_sheet, startrow + i * dylength * length, 4, dylength * length, colcount);
                            }

                        }
                        //县级合计部分公式
                        else
                        {
                            if (XJareaid_List.Count != 0)
                            {
                                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * dylength * length, 4, XJareaid_List.Count, dylength * length, startrow + i * dylength * length, 4, colcount);

                            }
                            //无地区直接在合计部分写0
                            else
                            {
                                fc.Sheet_WriteZero(obj_sheet, startrow + i * dylength * length, 4, dylength * length, colcount);
                            }
                        }
                    }
                }
            }
   
        }
       
    }
}

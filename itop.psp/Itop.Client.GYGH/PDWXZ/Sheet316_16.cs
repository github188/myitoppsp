using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet316_16
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
            //电压等级根据实际情况查询(中压 6 <= 电压<=20)
            string XLDianYatiaojian = " (b.Type='55' or b.Type='56' or b.Type='57' or b.Type='58') and  b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and a.LineType2='公用' and b.RateVolt  between 6 and 20 group by b.RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt_type50-59", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //计算市辖供电区条件 
            string SXtiaojianareaid = " (b.Type='55' or b.Type='56' or b.Type='57' or b.Type='58') and  b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and a.AreaID!='' and  b.ProjectID='" + ProjID + "' and a.LineType2='公用' and a.DQ='市辖供电区' and  b.RateVolt between 6 and 20  group by a.AreaID";
            //计算县级供电区条件
            string XJtiaojianareaid = " (b.Type='55' or b.Type='56' or b.Type='57' or b.Type='58') and  b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and a.AreaID!='' and  b.ProjectID='" + ProjID + "' and a.LineType2='公用' and a.DQ!='市辖供电区' and  b.RateVolt between 6 and 20  group by a.AreaID";
            //存放市辖供电区分区名
            IList<string> SXareaid_List = Services.BaseService.GetList<string>("SelectPSPDEV_AreaID_type50-59", SXtiaojianareaid);
            //存放县级供电区分区名
            IList<string> XJareaid_List = Services.BaseService.GetList<string>("SelectPSPDEV_AreaID_type50-59", XJtiaojianareaid);
            //市辖供电区分区个数
            int SXsum = SXareaid_List.Count;
            //县级供电区分区个数
            int XJsum = XJareaid_List.Count;
            //表标题行数
            int startrow = 2;
            //列标题每项是压等级数
            int dylength = xlsum;
            //表格共 行7 列
            rowcount = startrow + (SXsum + XJsum + 2) * dylength;
            colcount = 7;
            //工作表第一行的标题
            title = "附表16  铜陵市中压配电网开关站、环网柜等设备统计（ " + year + "年）";
            //工作表名
            sheetname = "表3-16 附表16";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 90;
            obj_sheet.Columns[1].Width = 70;
            obj_sheet.Columns[2].Width = 100;
            obj_sheet.Columns[3].Width = 100;
            obj_sheet.Columns[4].Width = 100;
            obj_sheet.Columns[5].Width = 100;
            obj_sheet.Columns[6].Width = 110;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 20;
            //写标题行列内容
            Sheet_AddItem(obj_sheet, area_key_id, DY_list, SXareaid_List, XJareaid_List);
            //写入数据
            Sheet_AddData(obj_sheet,year,ProjID, DY_list, SXareaid_List, XJareaid_List);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet,Hashtable area_key_id, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            //2行标题内容
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.SetValue(1, 2, "电压等级（kV）");
            obj_sheet.SetValue(1, 3, "开关站（座）");
            obj_sheet.SetValue(1, 4, "环网柜（座）");
            obj_sheet.SetValue(1, 5, "柱上开关（台）");
            obj_sheet.SetValue(1, 6, "电缆分支箱（座）");
            //写标题列内容
            fc.Sheet_AddItem_FBonlyDY(obj_sheet, area_key_id, 2, obj_DY_List, SXareaid_List, XJareaid_List);
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            //条件
            string tiaojian = "";
            //DQ条件
            string DQtiaojian = "";
            int startrow = 2;
            int length = obj_DY_List.Count;
            //要统计合计的列数
            int colcount = 4;
            for (int i = 0; i < (2 + SXareaid_List.Count + XJareaid_List.Count); i++)
            {
                string areaid = "";
                if (i != 0 && i != (SXareaid_List.Count + 1))
                {
                    if (i < SXareaid_List.Count + 1)
                    {
                        DQtiaojian = " a.DQ='市辖供电区'";
                        areaid = SXareaid_List[i - 1].ToString();
                    }
                    else
                    {
                        DQtiaojian = " a.DQ!='市辖供电区'";
                        areaid = XJareaid_List[i - SXareaid_List.Count - 2].ToString();
                    }
                    for (int j = 0; j < obj_DY_List.Count; j++)
                    {
                        //开关站（座）
                        tiaojian = " b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and b.Type='55' and a.LineType2='公用'  and " + DQtiaojian + " and a.AreaID='" + areaid + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                        int KGsum = 0;
                        if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                        {
                            KGsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                        }
                        obj_sheet.SetValue(startrow + i * length + j, 3, KGsum);
                        //环网柜（座）
                        tiaojian = " b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and b.Type='56' and a.LineType2='公用'  and " + DQtiaojian + " and a.AreaID='" + areaid + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                        int HWsum = 0;
                        if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                        {
                            HWsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                        }
                        obj_sheet.SetValue(startrow + i * length + j, 4, HWsum);
                        //柱上开关（台）
                        tiaojian = " b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and b.Type='57' and a.LineType2='公用'  and " + DQtiaojian + " and a.AreaID='" + areaid + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                        int ZSsum = 0;
                        if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                        {
                            ZSsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                        }
                        obj_sheet.SetValue(startrow + i * length + j, 5, ZSsum);
                        //电缆分支箱（座）
                        tiaojian = " b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and b.Type='58' and a.LineType2='公用'  and " + DQtiaojian + " and a.AreaID='" + areaid + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                        int DLFZsum = 0;
                        if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                        {
                            DLFZsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                        }
                        obj_sheet.SetValue(startrow + i * length + j, 6, DLFZsum);
                    }
                }

                else
                {
                    //市辖合计部分公式
                    if (i == 0)
                    {
                        if (SXareaid_List.Count != 0)
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * length, 3, SXareaid_List.Count, length, startrow + i * length, 3, colcount);
                        }
                        //无地区直接在合计部分写0
                        else
                        {
                            fc.Sheet_WriteZero(obj_sheet, startrow + i * length, 3, length, colcount);
                        }

                    }
                    //县级合计部分公式
                    else
                    {
                        if (XJareaid_List.Count != 0)
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * length, 3, XJareaid_List.Count, length, startrow + i * length, 3, colcount);

                        }
                        //无地区直接在合计部分写0
                        else
                        {
                            fc.Sheet_WriteZero(obj_sheet, startrow + i * length, 3, length, colcount);
                        }
                    }
                }
            }
        }
    }
}

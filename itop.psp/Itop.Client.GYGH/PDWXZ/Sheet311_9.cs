using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet311_9
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
            //线路电压等级根据实际情况查询(中压 6 <= 电压<=20)
            string XLDianYatiaojian = " Type='05' and OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2  and ProjectID='" + ProjID + "' and LineType2='公用' and RateVolt  between 6 and 20 group by RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //计算市辖供电区条件 
            string SXtiaojianareaid = " Type='05' and OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2   and AreaID!='' and  ProjectID='" + ProjID + "'  and LineType2='公用' and DQ='市辖供电区' and  RateVolt between 6 and 20  group by AreaID";
            //计算县级供电区条件
            string XJtiaojianareaid = " Type='05' and OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2  and AreaID!='' and  ProjectID='" + ProjID + "' and LineType2='公用' and DQ!='市辖供电区'  and RateVolt between 6 and 20  group by AreaID";
            //存放市辖供电区分区名
            IList<string> SXareaid_List = Services.BaseService.GetList<string>("SelectPSPDEV_GroupAreaID", SXtiaojianareaid);
            //存放县级供电区分区名
            IList<string> XJareaid_List = Services.BaseService.GetList<string>("SelectPSPDEV_GroupAreaID", XJtiaojianareaid);

            //市辖供电区分区个数
            int SXsum = SXareaid_List.Count;
            //县级供电区分区个数
            int XJsum = XJareaid_List.Count;
            //表标题行数
            int startrow = 2;
            //列标题每项是压等级数
            int dylength = xlsum;
            //表格共 行12 列
            rowcount = startrow + (SXsum + XJsum + 2) * dylength;
            colcount = 12;
            //工作表第一行的标题
            title = "附表9  铜陵市中压架空网络结构情况统计(" + year + "年)";
            //工作表名
            sheetname = "表3-11 附表9";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 90;
            obj_sheet.Columns[1].Width = 80;
            obj_sheet.Columns[2].Width = 60;
            obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 60;
            obj_sheet.Columns[5].Width = 80;
            obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 60;
            obj_sheet.Columns[8].Width = 60;
            obj_sheet.Columns[9].Width = 60;
            obj_sheet.Columns[10].Width = 60;
            obj_sheet.Columns[11].Width = 60;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 40;
           
            //写标题行列内容
            Sheet_AddItem(obj_sheet, area_key_id, DY_list, SXareaid_List, XJareaid_List);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, DY_list, SXareaid_List, XJareaid_List);
            //写入公式
            fc.Sheet_WriteFormula_OneCol_Anotercol_nopercent(obj_sheet, startrow, 3, 4, 5, dylength * (XJsum + SXsum + 2),2);
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, startrow, 3, 6, 9, dylength * (XJsum + SXsum + 2));
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, startrow, 3, 7, 10, dylength * (XJsum + SXsum + 2));
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, startrow, 3, 8, 11, dylength * (XJsum + SXsum + 2));
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet,Hashtable area_key_id, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            //写标题行内容

            //2行标题内容
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.SetValue(1, 2, "电压等级 （kV）");
            obj_sheet.SetValue(1, 3, "线路条数（条）");
            obj_sheet.SetValue(1, 4, "分段总数（段）");
            obj_sheet.SetValue(1, 5, "平均分段数（段/条）");
            obj_sheet.SetValue(1, 6, "辐射式线路条数");
            obj_sheet.SetValue(1, 7, "单联络线路条数");
            obj_sheet.SetValue(1, 8, "多联络线路条数");
            obj_sheet.SetValue(1, 9, "辐射式比例（%）");
            obj_sheet.SetValue(1, 10, "单联络比例（%）");
            obj_sheet.SetValue(1, 11, "多联络比例（%）");
            //写标题列内容
            fc.Sheet_AddItem_FBonlyDY(obj_sheet, area_key_id, 2, obj_DY_List, SXareaid_List, XJareaid_List);
      
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            //条件
            string XLtiaojian = "";
            //DQ条件
            string DQtiaojain = "";
            int startrow = 2;
            int length = obj_DY_List.Count;
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
                            XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2 and ProjectID='" + ProjID + "' and Type='05' and LineType2='公用' and " + DQtiaojain + " and AreaID='" + areaid + "' and RateVolt=" + obj_DY_List[j].ToString();
                            //线路条数
                            int XLsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian) != null)
                            {
                                XLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 3, XLsum);
                            //分段总数
                            int XLFDsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_SUMSwitchNum", XLtiaojian) != null)
                            {
                                XLFDsum = (int)Services.BaseService.GetObject("SelectPSPDEV_SUMSwitchNum", XLtiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 4, XLFDsum);
                            //辐射式线路条数
                            string fsxltiaojian = "OperationYear='" + year + "' and  LineLength>Length2 and LLFS='辐射式' and ProjectID='" + ProjID + "' and Type='05' and LineType2='公用'  and  " + DQtiaojain + " and AreaID='" + areaid + "' and RateVolt=" + obj_DY_List[j].ToString();
                            int FSXLsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", fsxltiaojian) != null)
                            {
                                FSXLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", fsxltiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 6, FSXLsum);

                            //单联络线路条数
                            string dllxltiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2 and LLFS='单联络' and ProjectID='" + ProjID + "' and Type='05' and LineType2='公用'  and  " + DQtiaojain + " and AreaID='" + areaid + "'  and RateVolt=" + obj_DY_List[j].ToString();
                            int DLLXLsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", dllxltiaojian) != null)
                            {
                                DLLXLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", dllxltiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 7, DLLXLsum);

                            //多联络线路条数
                            string dullxltiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2 and LLFS='多联络' and ProjectID='" + ProjID + "' and Type='05' and LineType2='公用' and  " + DQtiaojain + " and AreaID='" + areaid + "'  and RateVolt=" + obj_DY_List[j].ToString();
                            int DULLXLsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", dullxltiaojian) != null)
                            {
                                DULLXLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", dullxltiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 8, DULLXLsum);
                        }
                    }
                    else
                    {
                        //市辖合计部分公式
                        if (i == 0)
                        {
                            if (SXareaid_List.Count != 0)
                            {
                                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * length, 3, SXareaid_List.Count, length, startrow + i * length, 3, 9);
                            }
                            //无地区直接在合计部分写0
                            else
                            {
                                fc.Sheet_WriteZero(obj_sheet, startrow + i * length, 3, length, 9);
                            }

                        }
                        //县级合计部分公式
                        else
                        {
                            if (XJareaid_List.Count != 0)
                            {
                                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * length, 3, XJareaid_List.Count, length, startrow + i * length, 3, 9);

                            }
                            //无地区直接在合计部分写0
                            else
                            {
                                fc.Sheet_WriteZero(obj_sheet, startrow + i * length, 3, length, 9);
                            }
                        }
                    }
                }
            }
        }
    }
}

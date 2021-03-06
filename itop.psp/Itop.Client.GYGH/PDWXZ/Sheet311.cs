using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet311
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
        {//线路电压等级根据实际情况查询(中压 6 <= 电压<=20)
            string XLDianYatiaojian = " Type='05' and OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2  and ProjectID='" + ProjID + "' and LineType2='公用' and RateVolt  between 6 and 20 group by RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //表标题行数
            int startrow = 2;
            //列标题每项行数
            int dylength = xlsum;

            //表格共 行12 列
            rowcount = startrow + 7 * dylength;
            colcount = 12;
            //工作表第一行的标题
            title = "表3‑11  " + year + "年铜陵市中压架空网结构情况";
            //工作表名
            sheetname = "表3-11";
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
            Sheet_AddItem(obj_sheet, SxXjName, DY_list);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, SxXjName, DY_list);
            //写入公式
            fc.Sheet_WriteFormula_OneCol_Anotercol_nopercent(obj_sheet, startrow, 3, 4, 5, dylength * 7,2);
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, startrow, 3, 6, 9, dylength * 7);
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, startrow, 3, 7, 10, dylength * 7);
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, startrow, 3, 8, 11, dylength * 7);
          
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //写标题行内容

            //2行标题内容
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
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
            fc.Sheet_AddItem_ZBonlyDY(obj_sheet, SxXjName, 2, obj_DY_List);   
        
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //添加数据

            //条件
            string XLtiaojian = "";
            int startrow = 2;
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

                            XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2 and ProjectID='" + ProjID + "' and Type='05' and LineType2='公用'  and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
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
                            string fsxltiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2 and LLFS='辐射式' and ProjectID='" + ProjID + "' and Type='05' and LineType2='公用'  and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
                            int FSXLsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", fsxltiaojian) != null)
                            {
                                FSXLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", fsxltiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 6, FSXLsum);

                            //单联络线路条数
                            string dllxltiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2 and LLFS='单联络' and ProjectID='" + ProjID + "' and Type='05' and LineType2='公用'  and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
                            int DLLXLsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", dllxltiaojian) != null)
                            {
                                DLLXLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", dllxltiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 7, DLLXLsum);

                            //多联络线路条数
                            string dullxltiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + "  and  LineLength>Length2 and LLFS='多联络' and ProjectID='" + ProjID + "' and Type='05' and LineType2='公用' and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
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
                        //县级合计部分公式
                        if (i == 1)
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * length, 3, 4, length, startrow + i * length, 3, 9);
                        }
                        //全地区合计部分公式
                        else
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 3, 2, length, startrow + i * length, 3, 9);

                        }
                    }

                }
            }
        }

    }
}

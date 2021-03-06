using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet314
    {

        private class savedata
        {

            public string no = "";
            public string dy = "";
            public object data = null;
        }
        //存放表3-14数据
        List<savedata> SDL314 = new List<savedata>();
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
            //线路电压等级根据实际情况查询(中压 6 <= 电压<=20)
            string XLDianYatiaojian = " (b.Type='50' or b.Type='51' or b.Type='52') and  b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and a.LineType2!='' and b.RateVolt  between 6 and 20  group by b.RateVolt order by b.RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt_type50-59", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //表标题行数
            int startrow = 3;
            //表格共 行11 列
            rowcount = startrow + 7 * xlsum;
            colcount = 11;
            //工作表第一行的标题
            title = "表3‑14  "+year+"年铜陵市中压配电变压器统计情况";
            //工作表名
            sheetname = "表3-14";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 90;
            obj_sheet.Columns[2].Width = 60;
            obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 60;
            obj_sheet.Columns[5].Width = 60;
            obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 60;
            obj_sheet.Columns[8].Width = 60;
            obj_sheet.Columns[9].Width = 60;
            obj_sheet.Columns[10].Width = 60;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 20;
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SxXjName, DY_list);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, SxXjName, DY_list);
            //写入公式
            fc.Sheet_WriteFormula_OneCol_TwoCol_Threecol_sum(obj_sheet, startrow, 3, 5, 7, DY_list.Count);
            fc.Sheet_WriteFormula_OneCol_TwoCol_Threecol_sum(obj_sheet, startrow + 2 * DY_list.Count, 3, 5, 7, 4 * DY_list.Count);
            fc.Sheet_WriteFormula_OneCol_TwoCol_Threecol_sum(obj_sheet, startrow, 4, 6, 8, DY_list.Count);
            fc.Sheet_WriteFormula_OneCol_TwoCol_Threecol_sum(obj_sheet, startrow + 2 * DY_list.Count, 4, 6, 8, 4 * DY_list.Count);
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet,startrow,3,9,10,7*DY_list.Count);

            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //设定格式
            CellType(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.AddSpanCell(1, 2, 2, 1);
            obj_sheet.SetValue(1, 2, "电压等级(kV)");
            obj_sheet.AddSpanCell(1, 3, 2, 1);
            obj_sheet.SetValue(1, 3, "公变台数(台)");
            obj_sheet.AddSpanCell(1, 4, 2, 1);
            obj_sheet.SetValue(1, 4, "公变容量(MVA)");
            obj_sheet.AddSpanCell(1, 5, 2, 1);
            obj_sheet.SetValue(1, 5, "专变台数(台)");
            obj_sheet.AddSpanCell(1, 6, 2, 1);
            obj_sheet.SetValue(1, 6, "专变容量(MVA)");
            obj_sheet.AddSpanCell(1, 7, 2, 1);
            obj_sheet.SetValue(1, 7, "总台数(台)");
            obj_sheet.AddSpanCell(1, 8, 2, 1);
            obj_sheet.SetValue(1, 8, "总容量(MVA)");
            obj_sheet.AddSpanCell(1, 9, 1, 2);
            obj_sheet.SetValue(1, 9, "高损变");

            //3行标题内容
            obj_sheet.SetValue(2, 9, "台数(台)");
            obj_sheet.SetValue(2, 10, "比例(%)");

            //写标题列内容
            fc.Sheet_AddItem_ZBonlyDY(obj_sheet, SxXjName, 3, obj_DY_List);
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //添加数据
            //条件
            string GorZ = "";
            string tiaojian = "";
            int startrow = 3;
            int dylenth = obj_DY_List.Count;
            int colcount = 7;
            for (int i = 0; i < SxXjName.Count; i++)
            {
                //合计部分不用计算
                if (SxXjName[i][2].ToString() != "合计")
                {
                    for (int j = 0; j < obj_DY_List.Count; j++)
                    {
                        //一次算公用一次算专用
                        for (int k = 0; k < 2; k++)
                        {
                            if (k==0)
                            {
                                GorZ = "公用";
                            }
                            else
                            {
                                GorZ = "专用";
                            }
                            //配电室内主变台数
                            tiaojian = " b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and b.Type='50' and a.LineType2='" + GorZ + "' and a.DQ='" + SxXjName[i][2] + "' and b.RateVolt=" + obj_DY_List[j];

                            int PDsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_SUMFlag", tiaojian) != null)
                            {
                                PDsum = (int)Services.BaseService.GetObject("SelectPSPDEV_SUMFlag", tiaojian);
                            }
                            //箱变座数 柱上变（台）
                            tiaojian = " b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and (b.Type='51' or b.Type='52') and a.LineType2='" + GorZ + "' and a.DQ='" + SxXjName[i][2] + "' and b.RateVolt=" + obj_DY_List[j];
                            int XBsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                            {
                                XBsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                            }
                            //配变台数等于这三个数据和
                            obj_sheet.SetValue(startrow + i * dylenth + j, 3 + k * 2, PDsum + XBsum);


                            //配变容量
                            tiaojian = " b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and (b.Type='50' or b.Type='51' or b.Type='52') and a.LineType2='" + GorZ + "' and a.DQ='" + SxXjName[i][2] + "' and b.RateVolt=" + obj_DY_List[j];

                            double PBRLsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_SUMNum2_type50-59", tiaojian) != null)
                            {
                                PBRLsum = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMNum2_type50-59", tiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * dylenth + j, 4 + k * 2, PBRLsum);
                        }
                    }
                }
                else
                {
                    //县级合计部分公式
                    if (i == 1)
                    {
                        fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * dylenth, 3, 4, dylenth, startrow + i * dylenth, 3, colcount);
                    }
                    //全地区合计部分公式
                    else
                    {
                        fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 3, 2, dylenth, startrow + i * dylenth, 3, colcount);

                    }
                }

            }   
        }
        public void CellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //市辖部分的行号
            int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "1");
            //县级部分的行号
            int XJrow = fc.Sheet_Find_Value(obj_sheet, 0, "2");
            //其中： 直供直管部分的行号
            int ZGrow = fc.Sheet_Find_Value(obj_sheet, 0, "2.1");
            //全地区部分的行号
            int QDQrow = fc.Sheet_Find_Value(obj_sheet, 0, "3");
            //为-1时表示没找到，也就是电压等级为0个
            if (SXrow != -1)
            {
                for (int row = SXrow; row < XJrow; row++)
                {
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 9);
                }
                for (int row = ZGrow; row < QDQrow; row++)
                {
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 9);
                }
            }
        }
        public void SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //清空存放数据列表
            SDL314.Clear();

            //市辖部分的行号
            int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "1");
            //县级部分的行号
            int XJrow = fc.Sheet_Find_Value(obj_sheet, 0, "2");
            //差为电压等级数
            int DYnum = XJrow - SXrow;
            //存在电压等级
            if (DYnum != 0)
            {
                //市辖供电区加上县级供电区的四个分区，共有5个分区，加上县级合计有6个分区
                for (int row = SXrow; row < obj_sheet.RowCount - DYnum; row++)
                {
                    savedata tempdata = new savedata();
                    //县级合计部分跳过
                    if (row == XJrow)
                    {
                        row = row + DYnum;
                    }
                    string no = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                    string dy = obj_sheet.Cells[row, 2].Value.ToString();
                    tempdata.no = no;
                    tempdata.dy = dy;
                    tempdata.data = obj_sheet.GetValue(row, 9);
                    SDL314.Add(tempdata);
                }

            }
        }
        public void WriteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            if (SDL314.Count != 0)
            {
                //市辖部分的行号
                int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "1");
                //县级部分的行号
                int XJrow = fc.Sheet_Find_Value(obj_sheet, 0, "2");
                //差为电压等级数
                int newDYnum = XJrow - SXrow;
                for (int row = SXrow; row < obj_sheet.RowCount - newDYnum; row++)
                {
                    string sno = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                    string dy = obj_sheet.Cells[row, 2].Value.ToString();
                    for (int i = 0; i < SDL314.Count; i++)
                    {
                        if (sno == SDL314[i].no && dy == SDL314[i].dy)
                        {
                            obj_sheet.SetValue(row, 9, SDL314[i].data);
                            SDL314.Remove(SDL314[i]);
                            break;

                        }
                    }
                }
            }

        }
    }
}

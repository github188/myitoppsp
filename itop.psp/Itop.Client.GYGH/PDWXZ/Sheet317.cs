using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet317
    {

        fpcommon fc = new fpcommon();
        //用于保存数据
        private class savedata
        {
            
            public string no = "";
            public string dy = "";
            public object data = null;
        }
        //存放表3-17数据
        List<savedata> SDL317 = new List<savedata>();
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
            //电压等级根据实际情况查询(中压 6 <= 电压<=20)
            //此处电压等级分两部分情况，负荷开关中的电压等级可以直接查出
            //但断路器的电压等级是要查询其母线的电压等级
            //分别查出两部分电压等级后进行整合
            string FHKGDianYatiaojian = " b.Type='59' and  b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and a.LineType2='公用' and b.RateVolt  between 6 and 20 group by b.RateVolt";
            string DLQDianYatiaojian = " a.Type='06' and  a.OperationYear!='' and CAST(a.OperationYear as int)<=" + year + " and a.ProjectID='" + ProjID + "'  and b.RateVolt  between 6 and 20 group by b.RateVolt";
            IList<double> FHKGDY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt_type50-59", FHKGDianYatiaojian);
            IList<double> DLQDY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt_type06", DLQDianYatiaojian);
            //整合两部分电压
            for (int i = 0; i < FHKGDY_list.Count; i++)
            {
                for (int j = 0; j < DLQDY_list.Count; j++)
                {
                    if (FHKGDY_list[i].ToString()==DLQDY_list[j].ToString())
                    {
                        DLQDY_list.Remove(DLQDY_list[j]);
                        break;
                    }
                }
            }
            if (DLQDY_list.Count>0)
            {
                for (int i = 0; i < DLQDY_list.Count; i++)
			    {
                    FHKGDY_list.Add(DLQDY_list[i]);
			    }
                
            }
            List<double> DY_list=new List<double>() ;
            for (int i = 0; i < FHKGDY_list.Count; i++)
			{
                DY_list.Add(FHKGDY_list[i]);
			}
            DY_list.Sort();
            int xlsum = DY_list.Count;
            //表标题行数
            int startrow = 2;
            //列标题每项行数
            int dylength = xlsum;

            //表格共 行8 列
            rowcount = startrow + 7 * dylength;
            colcount = 8;
            //工作表第一行的标题
            title = "表3‑17  "+year+"年铜陵市中压配电网开关无油化率统计";
            //工作表名
            sheetname = "表3-17";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[2].Width = 100;
            obj_sheet.Columns[3].Width = 100;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[5].Width = 100;
            obj_sheet.Columns[6].Width = 110;
            obj_sheet.Columns[7].Width = 100;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 40;
            //写标题行内容
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SxXjName, DY_list);
            //写入数据
            Sheet_AddData(obj_sheet, year, ProjID, SxXjName, DY_list);
            //写入公式
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, 2, 3, 6, 7, dylength * 7);
            //设定格式
            CellType(obj_sheet);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //2行标题内容
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, " 类型");
            obj_sheet.SetValue(1, 2, "电压等级（kV）");
            obj_sheet.SetValue(1, 3, "总开关数（台）");
            obj_sheet.SetValue(1, 4, "断路器（台）");
            obj_sheet.SetValue(1, 5, "负荷开关（台）");
            obj_sheet.SetValue(1, 6, "其中：无油化开关（台）");
            obj_sheet.SetValue(1, 7, "开关无油化率（%）");
            //写标题列内容
            fc.Sheet_AddItem_ZBonlyDY(obj_sheet, SxXjName, 2, obj_DY_List);
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName, IList<double> obj_DY_List)
        {

            //添加数据

            //条件
            string tiaojian = "";
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

                            //断路器（台）
                            tiaojian = " a.OperationYear!='' and CAST(a.OperationYear as int)<=" + year + " and a.ProjectID='" + ProjID + "' and a.Type='06' and a.DQ='" + SxXjName[i][2] + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                            int DLQsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_CountDLQ", tiaojian) != null)
                            {
                                DLQsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountDLQ", tiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 4, DLQsum);
                            //负荷开关（台）
                            tiaojian = " b.OperationYear!='' and  year(cast(b.OperationYear as datetime))<=" + year + " and b.ProjectID='" + ProjID + "' and b.Type='59' and a.LineType2='公用'  and a.DQ='" + SxXjName[i][2] + "' and b.RateVolt=" + obj_DY_List[j].ToString();
                            int FHKGsum = 0;
                            if (Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian) != null)
                            {
                                FHKGsum = (int)Services.BaseService.GetObject("SelectPSPDEV_Count_type50-59", tiaojian);
                            }
                            obj_sheet.SetValue(startrow + i * length + j, 5, FHKGsum);
                            //总开关数（台）(断路器（台）+负荷开关（台）)
                            int ALLKG = DLQsum + FHKGsum;

                            obj_sheet.SetValue(startrow + i * length + j, 3, ALLKG);
                            
                        }
                    }
                    else
                    {
                        //县级合计部分公式
                        if (i == 1)
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * length, 3, 4, length, startrow + i * length, 3, 4);
                        }
                        //全地区合计部分公式
                        else
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 3, 2, length, startrow + i * length, 3, 4);

                        }
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
            if (SXrow!=-1)
            {
                for (int row = SXrow; row < XJrow; row++)
                {
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 6);
                }
                for (int row = ZGrow; row < QDQrow; row++)
                {
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 6);
                }
            }
        }
        public void SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //清空存放数据列表
            SDL317.Clear();
            
            //市辖部分的行号
            int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "1");
            //县级部分的行号
            int XJrow = fc.Sheet_Find_Value(obj_sheet, 0, "2");
            //差为电压等级数
            int DYnum = XJrow - SXrow;
            //存在电压等级
            if (DYnum!=0)
            {
                //市辖供电区加上县级供电区的四个分区，共有5个分区，加上县级合计有6个分区
                for (int row = SXrow; row < obj_sheet.RowCount-DYnum; row++)
                {
                    savedata tempdata = new savedata();
                    //县级合计部分跳过
                    if (row==XJrow)
                    {
                        row = row + DYnum ;
                    }
                    string no = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                    string dy = obj_sheet.Cells[row, 2].Value.ToString();
                    tempdata.no = no;
                    tempdata.dy = dy;
                    tempdata.data = obj_sheet.GetValue(row, 6);
                    SDL317.Add(tempdata);
                }

            }
        }
        public void WriteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            if (SDL317.Count!=0)
            {
                //市辖部分的行号
                int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "1");
                //县级部分的行号
                int XJrow = fc.Sheet_Find_Value(obj_sheet, 0, "2");
                //差为电压等级数
                int newDYnum = XJrow - SXrow;
                for (int row = SXrow; row < obj_sheet.RowCount-newDYnum; row++)
                {
                    string sno = fc.Sheet_find_Rownotemptycell(obj_sheet,row, 0);
                    string dy = obj_sheet.Cells[row, 2].Value.ToString();
                    for (int i = 0; i < SDL317.Count; i++)
                    {
                        if (sno == SDL317[i].no && dy == SDL317[i].dy)
                        {
                            obj_sheet.SetValue(row, 6, SDL317[i].data);
                            SDL317.Remove(SDL317[i]);
                            break;

                        }
                    }
                }
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet318_19
    {
      
        private class savedata
        {
            public string DQ = "";
            public string areaname = "";
            public object[] data = new object[4];
            public savedata()
            {
                for (int i = 0; i < 4; i++)
                {
                    data[i] = null;
                }
            }
        }
        //存放表3-18附表19数据
        List<savedata> SDL318_19 = new List<savedata>();
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
            ////电压等级根据实际情况查询(中压 6 <= 电压<=20)
            //string XLDianYatiaojian = " (Type='55' or Type='56' or Type='57' or Type='58') and OperationYear='" + year + "' and ProjectID='" + ProjID + "' and LineType2='公用' and RateVolt  between 6 and 20 group by RateVolt";
            //IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            //int xlsum = DY_list.Count;
            //计算市辖供电区条件 
            List<double> DY_list = new List<double>();

            //条件中要加入对AreaID的限制，因为设备使用了AreaID存放其所属线路的UID,所以查询时要避免这种情况,用like +ProjectID来限制
            string SXtiaojianareaid = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and AreaID like '%" + ProjID + "' and  ProjectID='" + ProjID + "' and LineType2='公用' and DQ='市辖供电区'  group by AreaID";
            //计算县级供电区条件
            string XJtiaojianareaid = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and AreaID like '%" + ProjID + "'  and  ProjectID='" + ProjID + "' and LineType2='公用' and DQ!='市辖供电区'  group by AreaID";
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
            int dylength = 1;
            //表格共 行7 列
            rowcount = startrow + (SXsum + XJsum + 2) * dylength;
            colcount = 7;
            //工作表第一行的标题
            title = "附表19  铜陵市中低压配电网无功补偿统计（" + year + "年）";
            //工作表名
            sheetname = "表3-18 附表19";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 100;
            obj_sheet.Columns[1].Width = 90;
            obj_sheet.Columns[2].Width = 110;
            obj_sheet.Columns[3].Width = 110;
            obj_sheet.Columns[4].Width = 110;
            obj_sheet.Columns[5].Width = 110;
            obj_sheet.Columns[6].Width = 110;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 40;
           
           
            //写标题行列内容
            Sheet_AddItem(obj_sheet, area_key_id, DY_list, SXareaid_List, XJareaid_List);
            //写入数据
            //Sheet_AddData(obj_sheet,year,ProjID, DY_list, SXareaid_List, XJareaid_List);
            //写入公式
            if (SXareaid_List.Count>0)
            {
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1, 2, SXareaid_List.Count, 1, startrow, 2, 4);
            }
            if (XJareaid_List.Count>0)
            {
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + SXareaid_List.Count + 2, 2, XJareaid_List.Count, 1, startrow + SXareaid_List.Count + 1, 2, 4);
            }
            fc.Sheet_WriteFormula_OneCol_TwoCol_Threecol_sum(obj_sheet,3,3,5,6,SXareaid_List.Count+XJareaid_List.Count+2);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //限制格式
            CellType(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet,Hashtable area_key_id, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            //写标题行内容
            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "配变低压侧补偿");
            obj_sheet.AddSpanCell(1, 4, 1, 2);
            obj_sheet.SetValue(1, 4, "中压线路补偿");
            obj_sheet.AddSpanCell(1, 6, 2, 1);
            obj_sheet.SetValue(1, 6, "无功补偿总容量（Mvar）");

            //3行标题内容
            obj_sheet.SetValue(2, 2, "配变总台数（台）");
            obj_sheet.SetValue(2, 3, "无功补偿容量（Mvar）");
            obj_sheet.SetValue(2, 4, "线路条数（条）");
            obj_sheet.SetValue(2, 5, "无功补偿容量（Mvar）");
            //写标题列内容
            int startrow = 3;
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
                    obj_sheet.SetValue(startrow + i , 1, areaname);

                }
                //写第一列数据
                obj_sheet.AddSpanCell(startrow, 0, (SXareaid_List.Count + 1) , 1);
                obj_sheet.SetValue(startrow, 0, "市辖供电区");
                obj_sheet.AddSpanCell(startrow + (SXareaid_List.Count + 1) , 0, (XJareaid_List.Count + 1) , 1);
                obj_sheet.SetValue(startrow + (SXareaid_List.Count + 1) , 0, "县级供电区");
     
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
   
        }
        public void CellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //市辖部分的行号
            int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "市辖供电区");
            //县级部分的行号
            int XJrow = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            //为-1时表示没找到，也就是电压等级为0个
            if (SXrow != -1)
            {
                //市辖供电区中第一行第二列的“合计”部分合并的行数就是电压等级数
                //int dysum = obj_sheet.Cells[SXrow, 1].RowSpan;
                int dysum = 1;
                for (int row = SXrow + dysum; row < obj_sheet.RowCount; row++)
                {
                    //县级合计部分跳过
                    if (row==XJrow)
                    {
                        row = row + dysum;
                        if (!(row < obj_sheet.RowCount))
                        {
                            break;
                        }
                    }
                    for (int col = 2; col < 6; col++)
                    {
                        fc.Sheet_UnLockedandCellNumber(obj_sheet, row, col);
                    }
                }

            }
        }
        public void SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //清空存放数据列表
            SDL318_19.Clear();

            //市辖部分的行号
            int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "市辖供电区");
            //县级部分的行号
            int XJrow = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            //为-1时表示没找到，也就是电压等级为0个
            if (SXrow != -1)
            {
                //市辖供电区中第一行第二列的“合计”部分合并的行数就是电压等级数
                //int dysum = obj_sheet.Cells[SXrow, 1].RowSpan;
                int dysum = 1;
                //存储市辖部分除合计以外的数据
                for (int row = SXrow + dysum; row < XJrow; row++)
                {
                    savedata tempdata = new savedata();
                    tempdata.DQ = "市辖供电区";
                    tempdata.areaname = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1);
                    for (int col = 2; col < 6; col++)
                    {
                        tempdata.data[col-2] = obj_sheet.GetValue(row, col);
                    }
                   
                    SDL318_19.Add(tempdata);
                }
                //存储县级部分除合计以外的数据
                for (int row = XJrow + dysum; row < obj_sheet.RowCount; row++)
                {
                    savedata tempdata = new savedata();
                    tempdata.DQ = "县级供电区";
                    tempdata.areaname = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1);
                    for (int col = 2; col < 6; col++)
                    {
                        tempdata.data[col - 2] = obj_sheet.GetValue(row, col);
                    }

                    SDL318_19.Add(tempdata);
                }
            }
        }
        public void WriteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            if (SDL318_19.Count != 0)
            {
                //市辖部分的行号
                int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "市辖供电区");
                ////市辖供电区中第一行第二列的“合计”部分合并的行数就是电压等级数
                //int newdysum = obj_sheet.Cells[SXrow, 1].RowSpan;
                int newdysum = 1;
                for (int row = SXrow + newdysum; row < obj_sheet.RowCount; row++)
                {
                    string dq = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                    string areaname = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1);
                    for (int i = 0; i < SDL318_19.Count; i++)
                    {
                        if (dq == SDL318_19[i].DQ && areaname == SDL318_19[i].areaname)
                        {
                            for (int col = 2; col < 6; col++)
                            {
                                obj_sheet.SetValue(row, col, SDL318_19[i].data[col-2]);
                            }
                            
                            SDL318_19.Remove(SDL318_19[i]);
                            break;
                        }
                    }
                }
            }

        }
    }
}

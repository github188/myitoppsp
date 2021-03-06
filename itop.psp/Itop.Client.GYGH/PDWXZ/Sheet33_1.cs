using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet33_1
    {


        private class savedata
        {
            public string DQ = "";
            public string areaname = "";
            public object[] data = new object[5];
        }
        //存放表3-3附表1数据
        List<savedata> SDL33_1 = new List<savedata>();
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
            //计算市辖供电区条件 
            string SXtiaojianareaid = " ProjectID='" + ProjID + "' and Col1='市区'  group by ID";
            //计算县级供电区条件
            string XJtiaojianareaid = " ProjectID='" + ProjID + "' and Col1='县级'  group by ID";
            //存放市辖供电区分区名
            IList<string> SXareaid_List = Services.BaseService.GetList<string>("SelectPS_Table_AreaWH_ID", SXtiaojianareaid);
            //存放县级供电区分区名
            IList<string> XJareaid_List = Services.BaseService.GetList<string>("SelectPS_Table_AreaWH_ID", XJtiaojianareaid);

            //市辖供电区分区个数
            int SXsum = SXareaid_List.Count;
            //县级供电区分区个数
            int XJsum = XJareaid_List.Count;
            //表标题行数
            int startrow = 2;
           
           

            //表格共 行7 列
            rowcount = startrow + (SXsum + XJsum + 2) ;
            colcount = 7;
            //工作表第一行的标题
            title = "附表1  铜陵市最大负荷和电量情况（"+year+"年）";
            //工作表名
            sheetname = "表3-3 附表1";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 90;
            obj_sheet.Columns[1].Width = 80;
            obj_sheet.Columns[2].Width = 80;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[4].Width = 110;
            obj_sheet.Columns[5].Width = 90;
            obj_sheet.Columns[6].Width = 90;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 40;
            //写标题行内容
            Sheet_AddItem(obj_sheet,area_key_id,SXareaid_List,XJareaid_List);
            //写入求合公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet,startrow+1,2,SXareaid_List.Count,1,startrow,2,5);
            fc.Sheet_WriteFormula_RowSum(obj_sheet,startrow+SXareaid_List.Count+2,2,XJareaid_List.Count,1,startrow+SXareaid_List.Count+1,2,5);

            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //限制格式
            CellType(obj_sheet);
          
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet,Hashtable area_key_id, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            //2行标题内容
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.SetValue(1, 2, "面积(km2)");
            obj_sheet.SetValue(1, 3, "网供最大负荷（MW）");
            obj_sheet.SetValue(1, 4, "全社会最大用电用负荷（MW）");
            obj_sheet.SetValue(1, 5, "网供电量（亿kWh）");
            obj_sheet.SetValue(1, 6, "全社会用电量（亿kWh）");
            //写标题列内容
            //写标题列内容
            int startrow = 2;
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
                obj_sheet.SetValue(startrow + i, 1, areaname);

            }
            //写第一列数据
            obj_sheet.AddSpanCell(startrow, 0, (SXareaid_List.Count + 1), 1);
            obj_sheet.SetValue(startrow, 0, "市辖供电区");
            obj_sheet.AddSpanCell(startrow + (SXareaid_List.Count + 1), 0, (XJareaid_List.Count + 1), 1);
            obj_sheet.SetValue(startrow + (SXareaid_List.Count + 1), 0, "县级供电区");
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, IList<double> obj_DY_List, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
           
        }
        public void CellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //返回县级供电区开始行号
            int xjrow = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            for (int row = 3; row < obj_sheet.RowCount; row++)
            {
                if (row != xjrow)
                {
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 2);
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 3);
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 4);
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 5);
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 6);
                }
            }
        }
        public void SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            SDL33_1.Clear();
            //返回县级供电区开始行号
            int xjrow = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            for (int row = 3; row < obj_sheet.RowCount; row++)
            {
                if (row != xjrow)
                {
                    savedata tempdata = new savedata();
                    tempdata.DQ = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                    tempdata.areaname = obj_sheet.Cells[row, 1].Value.ToString();
                    tempdata.data[0] = obj_sheet.GetValue(row, 2);
                    tempdata.data[1] = obj_sheet.GetValue(row, 3);
                    tempdata.data[2] = obj_sheet.GetValue(row, 4);
                    tempdata.data[3] = obj_sheet.GetValue(row, 5);
                    tempdata.data[4] = obj_sheet.GetValue(row, 6);
                    SDL33_1.Add(tempdata);
               }
            }
        }
        public void WriteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            int SXrow = fc.Sheet_Find_Value(obj_sheet, 0, "市辖供电区");
            for (int row = SXrow; row < obj_sheet.RowCount; row++)
            {
                string DQ = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                string areaname = obj_sheet.Cells[row, 1].Value.ToString();
                for (int i = 0; i < SDL33_1.Count; i++)
                {
                    if (DQ==SDL33_1[i].DQ && areaname==SDL33_1[i].areaname)
                    {
                        obj_sheet.SetValue(row, 2, SDL33_1[i].data[0]);
                        obj_sheet.SetValue(row, 3, SDL33_1[i].data[1]);
                        obj_sheet.SetValue(row, 4, SDL33_1[i].data[2]);
                        obj_sheet.SetValue(row, 5, SDL33_1[i].data[3]);
                        obj_sheet.SetValue(row, 6, SDL33_1[i].data[4]);
                        SDL33_1.Remove(SDL33_1[i]);
                        break;
                    }
                }
            }
        }
    }
}

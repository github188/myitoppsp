using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
//using Itop.Domain.Graphics;
namespace Itop.Client.GYGH.BDZGH
{
    class Sheet91_52
    {

        class SaveData9152
        {
            public string DQ;
            public string AreaName;
            public object[,] data = new object[8, 12];
            public SaveData9152()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        data[i,j] = null; 
                    }
                    

                }
            }
        }
        List<SaveData9152> save91_52list = new List<SaveData9152>();
        fpcommon fc = new fpcommon();
        //工作表行数
        int rowcount = 0;
        //工作表列数据
        int colcount = 0;
        //工作表第一行的表名
        string title = "";
        //工作表标签名
        string sheetname = "";
        public void Build(FarPoint.Win.Spread.SheetView obj_sheet, string ProjID)
        {
            //计算市辖供电区条件 
            string SXtiaojianareaid = " ProjectID='" + ProjID + "' and Col1='市区'  group by Title";
            //计算县级供电区条件
            string XJtiaojianareaid = " ProjectID='" + ProjID + "' and Col1='县级'  group by Title";
            //存放市辖供电区分区名
            IList<string> SXareaid_List = Services.BaseService.GetList<string>("SelectPS_Table_AreaWH_Title", SXtiaojianareaid);
            //存放县级供电区分区名
            IList<string> XJareaid_List = Services.BaseService.GetList<string>("SelectPS_Table_AreaWH_Title", XJtiaojianareaid);

            //市辖供电区分区个数
            int SXsum = SXareaid_List.Count;
            //县级供电区分区个数
            int XJsum = XJareaid_List.Count;
            //表标题行数
            int startrow = 3;
            int dylength = 2;
            int length = 4;


            //表格共 行19列
            rowcount = startrow + (SXsum + XJsum + 2)*dylength*length;
            colcount = 19;
            //工作表第一行的标题
            title = "附表52  铜陵市规划新建不同型式变电站的座数和用地需求统计";
            //工作表名
            sheetname = "表9‑1 附表52 ";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 100;
            obj_sheet.Columns[1].Width = 80;
            obj_sheet.Columns[2].Width = 60;
            obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[5].Width = 40;
            obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 40;
            obj_sheet.Columns[8].Width = 60;
            obj_sheet.Columns[9].Width = 40;
            obj_sheet.Columns[10].Width = 60;
            obj_sheet.Columns[11].Width = 40;
            obj_sheet.Columns[12].Width = 60;
            obj_sheet.Columns[13].Width = 40;
            obj_sheet.Columns[14].Width = 60;
            obj_sheet.Columns[15].Width = 40;
            obj_sheet.Columns[16].Width = 60;
            obj_sheet.Columns[17].Width = 50;
            obj_sheet.Columns[18].Width = 60;
            //设定表格行高度
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 20;

            //写标题行列内容
            Sheet_AddItem(obj_sheet,SXareaid_List,XJareaid_List);
            //写入公式
            int countcol = 12;
            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1* dylength * length, 5, SXareaid_List.Count, dylength * length, startrow, 5, countcol);
            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow+(2+SXareaid_List.Count)*dylength*length, 5, XJareaid_List.Count, dylength * length, startrow + (SXareaid_List.Count+1) * dylength * length, 5, countcol);
            
            fc.Sheet_WriteFormula_MoreColsum_NoSpan(obj_sheet, startrow, 7, obj_sheet.RowCount - startrow, 5, startrow, 17, 2);
            fc.Sheet_WriteFormula_OneCol_Anotercol_nopercent(obj_sheet, startrow, 17, 18, 4, obj_sheet.RowCount - startrow,4);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //限定格式
            CellType(obj_sheet);

        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {

            //写标题行内容

            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.AddSpanCell(1, 2, 2, 1);
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 2, 1);
            obj_sheet.SetValue(1, 3, "建设型式");
            obj_sheet.AddSpanCell(1, 4, 2, 1);
            obj_sheet.SetValue(1, 4, "平均用地需求");
            obj_sheet.AddSpanCell(1, 5, 1, 2);
            obj_sheet.SetValue(1, 5, "2010年");
            obj_sheet.AddSpanCell(1, 7, 1, 2);
            obj_sheet.SetValue(1, 7, "2011年");
            obj_sheet.AddSpanCell(1, 9, 1, 2);
            obj_sheet.SetValue(1, 9, "2012年");
            obj_sheet.AddSpanCell(1, 11, 1, 2);
            obj_sheet.SetValue(1, 11, "2013年");
            obj_sheet.AddSpanCell(1, 13, 1, 2);
            obj_sheet.SetValue(1, 13, "2014年");
            obj_sheet.AddSpanCell(1, 15, 1, 2);
            obj_sheet.SetValue(1, 15, "2015年");
            obj_sheet.AddSpanCell(1, 17, 1, 2);
            obj_sheet.SetValue(1, 17, "“十二五”合计");

            //3行标题内容
            obj_sheet.SetValue(2, 5, "座数");
            obj_sheet.SetValue(2, 6, "用地需求");
            obj_sheet.SetValue(2, 7, "座数");
            obj_sheet.SetValue(2, 8, "用地需求");
            obj_sheet.SetValue(2, 9, "座数");
            obj_sheet.SetValue(2, 10, "用地需求");
            obj_sheet.SetValue(2, 11, "座数");
            obj_sheet.SetValue(2, 12, "用地需求");
            obj_sheet.SetValue(2, 13, "座数");
            obj_sheet.SetValue(2, 14, "用地需求");
            obj_sheet.SetValue(2, 15, "座数");
            obj_sheet.SetValue(2, 16, "用地需求");
            obj_sheet.SetValue(2, 17, "座数");
            obj_sheet.SetValue(2, 18, "用地需求");
            //写标题列内容



            //4列标题内容
            obj_sheet.SetValue(3, 3, "户外");
            obj_sheet.SetValue(4, 3, "半户内");
            obj_sheet.SetValue(5, 3, "户内");
            obj_sheet.SetValue(6, 3, "其它");
            //写标题列内容
            List<string> obj_DY_List = new List<string>();
            obj_DY_List.Add("110（66）");
            obj_DY_List.Add("35");

            int startrow = 3;
            int dylength = obj_DY_List.Count;
            int length = 4;
            
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
                            areaname = SXareaid_List[i - 1].ToString();
                        }
                        else
                        {
                            areaname = XJareaid_List[i - SXareaid_List.Count - 2].ToString();
                        }
                    }
                    for (int j = 0; j < obj_DY_List.Count; j++)
                    {
                        int row = startrow + i * dylength * length + j * length;
                        obj_sheet.SetValue(row, 3, "户外");
                        obj_sheet.SetValue(row + 1, 3, "半户内");
                        obj_sheet.SetValue(row + 2, 3, "户内");
                        obj_sheet.SetValue(row + 3, 3, "其它");
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
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, string ProjID, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            
        }
        public void SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            save91_52list.Clear();
            int startrow = 3+8;
            int xjrow = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            for (int row = startrow; row < xjrow; row++)
            {
                SaveData9152 tempdata = new SaveData9152();
                tempdata.DQ = "市辖供电区";
                tempdata.AreaName = obj_sheet.Cells[row, 1].Value.ToString();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        tempdata.data[i,j] = obj_sheet.GetValue(row + i,5 + j);
                    }
                }
                save91_52list.Add(tempdata);
                row = row + 7;
            }
            for (int row = xjrow+8; row < obj_sheet.RowCount; row++)
            {
                SaveData9152 tempdata = new SaveData9152();
                tempdata.DQ = "县级供电区";
                tempdata.AreaName = obj_sheet.Cells[row, 1].Value.ToString();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        tempdata.data[i,j] = obj_sheet.GetValue(row + i,5 + j);
                    }
                }
                save91_52list.Add(tempdata);
                row = row + 7;
            }
        }
        public void WriteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            int startrow = 3 + 8;
            int xjrow = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            for (int row = startrow; row < xjrow; row++)
            {
                for (int k = 0; k < save91_52list.Count; k++)
                {
                    if (save91_52list[k].DQ=="市辖供电区"&&save91_52list[k].AreaName==fc.Sheet_find_Rownotemptycell(obj_sheet,row,1))
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 12; j++)
                            {
                              obj_sheet.SetValue(row + i, 5 + j,save91_52list[k].data[i,j]);
                            }
                        }
                        save91_52list.Remove(save91_52list[k]);
                        row = row + 7;
                        break;
                    }
                }
                
            }
            for (int row = xjrow + 8; row < obj_sheet.RowCount; row++)
            {
                for (int k = 0; k < save91_52list.Count; k++)
                {
                    if (save91_52list[k].DQ == "县级供电区" && save91_52list[k].AreaName == fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1))
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                obj_sheet.SetValue(row + i, 5 + j, save91_52list[k].data[i, j]);
                            }
                        }
                        save91_52list.Remove(save91_52list[k]);
                        row = row + 7;
                        break;
                    }
                }
            }
        }
        public void CellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            int startrow = 3;
            int dylength = 2;
            int length = 4;
            int xjrow=fc.Sheet_Find_Value(obj_sheet,0,"县级供电区");
            if (xjrow!=-1)
            {
                for (int row = startrow + dylength * length; row < obj_sheet.RowCount; row++)
                {
                    if (row == xjrow)
                    {
                        row = row + dylength * length;
                    }
                    for (int col = 5; col < 17; col++)
                    {
                        fc.Sheet_UnLockedandCellNumber(obj_sheet, row, col);
                    }

                }
            }
            
        }
    }
}

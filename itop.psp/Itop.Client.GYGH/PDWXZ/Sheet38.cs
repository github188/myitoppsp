using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet38
    {

        /// <summary>
        /// 本表中断路器的统计比较复杂，其电压值要取所在母线的电压，公用性要取母线所在变电站的公用性
        /// </summary>
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
            //表格共73 行8 列
            rowcount = 73;
            colcount = 8;
            //工作表第一行的标题
            title = "表3‑8  " + year + "年铜陵市110kV及以下高压配电网主要设备运行年限分布";
            //工作表名
            sheetname = "表3-8";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 110;
            obj_sheet.Columns[2].Width = 90;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[5].Width = 90;
            obj_sheet.Columns[6].Width = 90;
            obj_sheet.Columns[7].Width = 90;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 20;
            //写入标题行和标题列
            Sheet_AddItem(obj_sheet);
            //写入数据
            Sheet_AddData(obj_sheet,year,ProjID);
            //写入公式
            //县级合计部分的求合公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 23, 4, 4, 10, 13, 4, 1);
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 23, 6, 4, 10, 13, 6, 1);
            //全地区合计部分的求合公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 3, 4, 2, 10, 63, 4, 1);
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 3, 6, 2, 10, 63, 6, 1);
            //求百分比公式
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, 3, 4, 14, 5, 3, 5);
            fc.Sheet_WriteFormula_TwoCol_Percent(obj_sheet, 3, 6, 14, 5, 3, 7);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);   
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //写标题行内容

            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.AddSpanCell(1, 2, 2, 1);
            obj_sheet.SetValue(1, 2, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 3, 2, 1);
            obj_sheet.SetValue(1, 3, "年限");
            obj_sheet.AddSpanCell(1, 4, 1, 2);
            obj_sheet.SetValue(1, 4, "主变台数");
            obj_sheet.AddSpanCell(1, 6, 1, 2);
            obj_sheet.SetValue(1, 6, "断路器");

            //3行标题内容
            obj_sheet.SetValue(2, 4, "数量（台）");
            obj_sheet.SetValue(2, 5, "比例（%）");
            obj_sheet.SetValue(2, 6, "数量（台）");
            obj_sheet.SetValue(2, 7, "比例（%）");
            //写标题列内容

            //1列标题内容
            obj_sheet.AddSpanCell(3, 0, 10, 1);
            obj_sheet.SetValue(3, 0, "1");
            obj_sheet.AddSpanCell(13, 0, 10, 1);
            obj_sheet.SetValue(13, 0, "2");
            obj_sheet.AddSpanCell(23, 0, 10, 1);
            obj_sheet.SetValue(23, 0, "2.1");
            obj_sheet.AddSpanCell(33, 0, 10, 1);
            obj_sheet.SetValue(33, 0, "2.2");
            obj_sheet.AddSpanCell(43, 0, 10, 1);
            obj_sheet.SetValue(43, 0, "2.3");
            obj_sheet.AddSpanCell(53, 0, 10, 1);
            obj_sheet.SetValue(53, 0, "2.4");
            obj_sheet.AddSpanCell(63, 0, 10, 1);
            obj_sheet.SetValue(63, 0, "3");

            //2列标题内容
            obj_sheet.AddSpanCell(3, 1, 10, 1);
            obj_sheet.SetValue(3, 1, "市辖供电区");
            obj_sheet.AddSpanCell(13, 1, 10, 1);
            obj_sheet.SetValue(13, 1, "县级供电区");
            obj_sheet.AddSpanCell(23, 1, 10, 1);
            obj_sheet.SetValue(23, 1, "其中：直供直管");
            obj_sheet.AddSpanCell(33, 1, 10, 1);
            obj_sheet.SetValue(33, 1, "控股");
            obj_sheet.AddSpanCell(43, 1, 10, 1);
            obj_sheet.SetValue(43, 1, "参股");
            obj_sheet.AddSpanCell(53, 1, 10, 1);
            obj_sheet.SetValue(53, 1, "代管");
            obj_sheet.AddSpanCell(63, 1, 10, 1);
            obj_sheet.SetValue(63, 1, "全地区");

            //偱环添加列标题
            for (int row = 3; row <= 72; row++)
            {
                obj_sheet.SetValue(row + 0, 3, "0-5年");
                obj_sheet.SetValue(row + 1, 3, "6-10年");
                obj_sheet.SetValue(row + 2, 3, "11-15年");
                obj_sheet.SetValue(row + 3, 3, "16-20年");
                obj_sheet.SetValue(row + 4, 3, "20年以上");
                obj_sheet.AddSpanCell(row + 0, 2, 5, 1);
                obj_sheet.SetValue(row + 0, 2, "110（66）");

                obj_sheet.SetValue(row + 5, 3, "0-5年");
                obj_sheet.SetValue(row + 6, 3, "6-10年");
                obj_sheet.SetValue(row + 7, 3, "11-15年");
                obj_sheet.SetValue(row + 8, 3, "16-20年");
                obj_sheet.SetValue(row + 9, 3, "20年以上");
                obj_sheet.AddSpanCell(row + 5, 2, 5, 1);
                obj_sheet.SetValue(row + 5, 2, "35");

                row = row + 9;
            }
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID)
        {
            //断路器的电压取它所链接的母线的电压,公用或专用取母线所属变电站的公用或专用

            //将类型存入数组，便于读取
            string[] dq ={ "市辖供电区", "县级直供直管", "县级控股", "县级参股", "县级代管" };
            //存放读出的类型
            string dqstr = "";
            //存便电站电压条件
            string BDdianya = "";
            //存断路器电压条件
            string DLQdianya = "";
            //存放变电站查询条件
            string BDtiaojian = "";
            //存放断路器查询条件
            string DLQtiaojian = "";
            string yeartiaojian="";
            //存放公用还是专用
            //string GorZ = "";
            //定义初始行数
            int startrow = 3;
            int row = 0;
            int length = 10;
            for (int i = 0; i < 5; i++)
            {
                if (i > 0)
                {
                    row = startrow + (i + 1) * length;
                }
                else
                {
                    row = startrow + i * length;
                }
                dqstr = dq[i];
                //固定循环2次，一次求电压110（66），一次求电压为35
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {
                        BDdianya = " and (L1=110 or L1=66) ";
                        DLQdianya = " and (b.RateVolt=110 or b.RateVolt=66)";
                    }
                    else
                    {
                        BDdianya = " and L1=35 ";
                        DLQdianya = " and b.RateVolt=35 ";
                    }
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
                         //各年主变台数
                        BDtiaojian = " S2!='' and (" + year + "-cast(substring(S2,1,4) as int) )"+yeartiaojian+" and AreaID='" + ProjID + "' and DQ='" + dqstr + "' and  S4='公用' " + BDdianya;
                        int ZBsum = 0;
                        if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML3", BDtiaojian) != null)
                        {
                            ZBsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML3", BDtiaojian);
                        }
                        obj_sheet.SetValue(row+k-1, 4, ZBsum);
                        //各年 断路器台数
                        DLQtiaojian = " a.OperationYear!='' and (" + year + "-cast(a.OperationYear as int) )"+yeartiaojian+" and a.ProjectID='" + ProjID + "' and a.DQ='" + dqstr + "' and a.Type='06' " + DLQdianya;
                        int DLQsum1 = 0;
                        if (Services.BaseService.GetObject("SelectPSPDEV_CountDLQ", DLQtiaojian) != null)
                        {
                            DLQsum1 = (int)Services.BaseService.GetObject("SelectPSPDEV_CountDLQ", DLQtiaojian);
                        }
                        obj_sheet.SetValue(row+k-1, 6, DLQsum1);
                    }

                    row = row + 5;
                }
            }
            
        }

    }
}

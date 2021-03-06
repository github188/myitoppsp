using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet37_3
    {

        fpcommon fc = new fpcommon();
        //用于保存数据
        private class savedata
        {
            //记录数据的行数，也是二维数组X
            private static int m;
            //记录数据的列数，也是二维数组Y
            private static int n;
            public string DQ = "";
            public string AreaName = "";
            public object[,] data;

            //构造函数
            public savedata(int row, int col)
            {
                m = row;
                n = col;
                data = new object[m, n];
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        data[i, j] = null;
                    }
                }
            }
        }
        //存放表3-7-3数据
        List<savedata> SDL3_7_3 = new List<savedata>();
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
            string SXtiaojian = " S2!='' and AreaName!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and DQ='市辖供电区'  and (L1=110 or L1=66 or L1=35)  group by AreaName";
            //计算县级供电区条件
            string XJtiaojian = " S2!='' and AreaName!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and DQ!='市辖供电区'  and (L1=110 or L1=66 or L1=35) group by AreaName";
            //存放市辖供电区分区名
            IList<string> SXarea = Services.BaseService.GetList<string>("SelectPSP_Substation_InfoGroupAreaName", SXtiaojian);
            //存放县级供电区分区名
            IList<string> XJarea = Services.BaseService.GetList<string>("SelectPSP_Substation_InfoGroupAreaName", XJtiaojian);
            //市辖供电区分区个数
            int SXsum = SXarea.Count;
            //县级供电区分区个数
            int XJsum = XJarea.Count;
            //表标题行数
            int startrow = 3;
            //列标题每项行数
            int length = 2;
            //表格共   行15 列
            rowcount = startrow + (SXsum + XJsum + 2) * length;
            colcount = 15;
            //工作表第一行的标题
            title = "附表3 铜陵市110kV及以下高压配电网变电设备情况(" + year + "年)";
            sheetname = "表3-7附表3 ";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            //obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 80;
            obj_sheet.Columns[2].Width = 80;
            //obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 80;
            //obj_sheet.Columns[5].Width = 60;
            //obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 80;
            //obj_sheet.Columns[8].Width = 60;
            //obj_sheet.Columns[9].Width = 60;
            //obj_sheet.Columns[10].Width = 60;
            obj_sheet.Columns[11].Width = 100;
            obj_sheet.Columns[12].Width = 100;
            obj_sheet.Columns[13].Width = 80;
            obj_sheet.Columns[14].Width = 90;
            //设定行高
            obj_sheet.Rows[1].Height = 40;
            obj_sheet.Rows[2].Height = 40;

            //写标题行内容

            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.AddSpanCell(1, 2, 2, 1);
            obj_sheet.SetValue(1, 2, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 3, 1, 3);
            obj_sheet.SetValue(1, 3, "变电站座数（座）");
            obj_sheet.AddSpanCell(1, 6, 1, 3);
            obj_sheet.SetValue(1, 6, "主变台数（台）");
            obj_sheet.AddSpanCell(1, 9, 1, 2);
            obj_sheet.SetValue(1, 9, "变电容量（MVA）");
            obj_sheet.SetValue(1, 11, "10（20）kV出线间隔总数（回）");
            obj_sheet.SetValue(1, 12, "10（20）已出线间隔总数（回）");
            obj_sheet.SetValue(1, 13, "无功补偿容量（Mvar）");
            obj_sheet.SetValue(1, 14, "10kV平均供电半径（km）");

            //3行标题内容
            obj_sheet.SetValue(2, 3, "公用");
            obj_sheet.SetValue(2, 4, "其中：单线单变座数");
            obj_sheet.SetValue(2, 5, "专用");
            obj_sheet.SetValue(2, 6, "公用");
            obj_sheet.SetValue(2, 7, "其中：有载调压主变");
            obj_sheet.SetValue(2, 8, "专用");
            obj_sheet.SetValue(2, 9, "公用");
            obj_sheet.SetValue(2, 10, "专用");
            obj_sheet.SetValue(2, 11, "公用");
            obj_sheet.SetValue(2, 12, "公用");
            obj_sheet.SetValue(2, 13, "公用");
            obj_sheet.SetValue(2, 14, "公用");
            //写入市辖供电区合计部分
            Sheet_AddItem(obj_sheet, startrow, "合计");
            //写入市辖供电区部分
            if (SXsum > 0)
            {
                for (int i = 0; i < SXsum; i++)
                {
                    //写入市辖部分列项目
                    Sheet_AddItem(obj_sheet, startrow + (i + 1) * length, SXarea[i]);
                    //写入市辖部分数据
                    Sheet_AddData(obj_sheet,year,ProjID,startrow + (i + 1) * length, "市辖供电区", SXarea[i]);
                }
                //写入市辖合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + length, 3, SXsum, length, startrow, 3, 12);
            }
            else
            {
                //没有市辖分区，直接在合计部分写0
                fc.Sheet_WriteZero(obj_sheet, startrow, 3, length, 12);
            }
            //合并第一列写入市辖供电区
            obj_sheet.AddSpanCell(startrow, 0, (SXsum + 1) * length, 1);
            obj_sheet.SetValue(startrow, 0, "市辖供电区");

            //写入县级供电区合计部分
            Sheet_AddItem(obj_sheet, startrow + (1 + SXsum) * length, "合计");
            //写入县级供电区部分
            if (XJsum > 0)
            {
                for (int i = 0; i < XJsum; i++)
                {
                    //写入县级部分列项目
                    Sheet_AddItem(obj_sheet, startrow + (i + 2 + SXsum) * length, XJarea[i]);
                    //写入县级部分数据
                    Sheet_AddData(obj_sheet,year,ProjID, startrow + (i + 2 + SXsum) * length, "县级供电区", XJarea[i]);
                }
                //写入县级合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (2 + SXsum) * length, 3, XJsum, length, startrow + (1 + SXsum) * length, 3, 12);

            }
            else
            {
                //没有县级分区，直接在合计部分写0
                fc.Sheet_WriteZero(obj_sheet, startrow + (1 + SXsum) * length, 3, length, 12);
            }
            //合并第一列写入县级供电区
            obj_sheet.AddSpanCell(startrow + (1 + SXsum) * length, 0, (XJsum + 1) * length, 1);
            obj_sheet.SetValue(startrow + (1 + SXsum) * length, 0, "县级供电区");



            //设定用户可输入数据单元格格式
            CellType(obj_sheet);
            //锁定表格
            fc.Sheet_Locked(obj_sheet); 
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, int row, string DQname)
        {
            //写标题列内容
            obj_sheet.SetValue(row, 2, "110（66）");
            obj_sheet.SetValue(row + 1, 2, "35");
            obj_sheet.AddSpanCell(row, 1, 2, 1);
            obj_sheet.SetValue(row, 1, DQname);
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID,  int row, string DQ, string areaname)
        {

            //存放DQ条件
            string dqstr = "";
            //存电压条件
            string dianya = "";
            //存放查询条件
            string BDtiaojian = "";
            //存放公用还是专用
            string GorZ = "";
            if (DQ == "市辖供电区")
            {
                dqstr = "  DQ='市辖供电区' ";
            }
            else
            {
                dqstr = "  DQ!='市辖供电区' ";
            }

            //固定循环二次
            for (int i = 0; i < 2; i++)
            {
                if (row % 2 == 0)
                {
                    dianya = " and L1=35 ";
                }
                else
                {
                    dianya = " and (L1=110 or L1=66) ";
                }
                //计算公用变电站座数
                GorZ = "公用";
                BDtiaojian = " S2!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                int GBDsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoCountall", BDtiaojian);
                obj_sheet.SetValue(row, 3, GBDsum);

                //计算专用变电站座数
                GorZ = "专用";
                BDtiaojian = " S2!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and  " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                int ZBDsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoCountall", BDtiaojian);
                obj_sheet.SetValue(row, 5, ZBDsum);

                //计算公用主变台数（台）
                GorZ = "公用";
                BDtiaojian = " S2!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and  " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                int GZBsum = 0;
                if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML3", BDtiaojian) != null)
                {
                    GZBsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML3", BDtiaojian);
                }
                obj_sheet.SetValue(row, 6, GZBsum);

                //计算专用主变台数（台）
                GorZ = "专用";
                BDtiaojian = " S2!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and  " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                int ZZBsum = 0;
                if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML3", BDtiaojian) != null)
                {
                    ZZBsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML3", BDtiaojian);
                }
                obj_sheet.SetValue(row, 8, ZZBsum);

                //计算公用变电容量
                GorZ = "公用";
                BDtiaojian = " S2!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and  " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                double GBDRLsum = 0;
                if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML2", BDtiaojian) != null)
                {
                    GBDRLsum = (double)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML2", BDtiaojian);
                }
                obj_sheet.SetValue(row, 9, GBDRLsum);
                //计算专用变电容量
                GorZ = "专用";
                BDtiaojian = " S2!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and  " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                double ZBDRLsum = 0;
                if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML2", BDtiaojian) != null)
                {
                    ZBDRLsum = (double)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML2", BDtiaojian);
                }
                obj_sheet.SetValue(row, 10, ZBDRLsum);

                //计算公用10（20）kV出线间隔总数（回）
                //GorZ = "公用";
                //BDtiaojian = " S2!='' and L13!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and  " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                //int GCXJGsum = 0;
                //if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML13", BDtiaojian) != null)
                //{
                //    GCXJGsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML13", BDtiaojian);
                //}
                //obj_sheet.SetValue(row, 11, GCXJGsum);

                //计算公用10（20）已出线间隔总数（回）
                //BDtiaojian = " S2!='' and L14!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and  " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                //int GYCXJGsum = 0;
                //if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML14", BDtiaojian) != null)
                //{
                //    GYCXJGsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML14", BDtiaojian);
                //}
                //obj_sheet.SetValue(row, 12, GYCXJGsum);

                //计算公用无功补偿容量（Mvar）
                BDtiaojian = " S2!='' and L5!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and  " + dqstr + " and AreaName='" + areaname + "'and S4='" + GorZ + "' " + dianya;
                double GWGBCsum = 0;
                if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML5", BDtiaojian) != null)
                {
                    GWGBCsum = (double)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML5", BDtiaojian);
                }
                obj_sheet.SetValue(row, 13, GWGBCsum);
                //行数加一
                row++;
            }
        }
        public void CellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //反回县级部分的行号
            int m = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            for (int row = 5; row < obj_sheet.RowCount; row++)
            {
                //如果是县级合计部分则跳过
                if (row != m && row != (m + 1))
                {

                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 4);
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 7);
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 11);
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 12);
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, row, 14);
                }
            }
        }
        public void SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            SDL3_7_3.Clear();
            //用于更新时保存用户数据
            for (int row = 5; row < obj_sheet.Rows.Count; row++)
            {
                if (obj_sheet.Cells[row, 1].Value.ToString() != "合计")
                {
                    savedata tempsd = new savedata(2, 3);
                    tempsd.DQ = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                    tempsd.AreaName = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1);
                    if (obj_sheet.Cells[row, 4].Value != null)
                    {
                        tempsd.data[0, 0] = obj_sheet.GetValue(row, 4);
                    }
                    if (obj_sheet.Cells[row, 7].Value != null)
                    {
                        tempsd.data[0, 1] = obj_sheet.GetValue(row, 7);
                    }
                    if (obj_sheet.Cells[row, 14].Value != null)
                    {
                        tempsd.data[0, 2] = obj_sheet.GetValue(row, 14);
                    }

                    if (obj_sheet.Cells[row + 1, 4].Value != null)
                    {
                        tempsd.data[1, 0] = obj_sheet.GetValue(row + 1, 4);
                    }
                    if (obj_sheet.Cells[row + 1, 7].Value != null)
                    {
                        tempsd.data[1, 1] = obj_sheet.GetValue(row + 1, 7);
                    }
                    if (obj_sheet.Cells[row + 1, 14].Value != null)
                    {
                        tempsd.data[1, 2] = obj_sheet.GetValue(row + 1, 14);
                    }
                    SDL3_7_3.Add(tempsd);

                }
                //行号加一
                row++;
            }
        }
        public void WriteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //用于更新时回写用户数据
            for (int row = 5; row < obj_sheet.Rows.Count; row++)
            {
                for (int i = 0; i < SDL3_7_3.Count; i++)
                {
                    if (fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0) == SDL3_7_3[i].DQ && fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1) == SDL3_7_3[i].AreaName)
                    {
                        obj_sheet.SetValue(row, 4, SDL3_7_3[i].data[0, 0]);
                        obj_sheet.SetValue(row, 7, SDL3_7_3[i].data[0, 1]);
                        obj_sheet.SetValue(row, 14, SDL3_7_3[i].data[0, 2]);
                        obj_sheet.SetValue(row + 1, 4, SDL3_7_3[i].data[1, 0]);
                        obj_sheet.SetValue(row + 1, 7, SDL3_7_3[i].data[1, 1]);
                        obj_sheet.SetValue(row + 1, 14, SDL3_7_3[i].data[1, 2]);
                        SDL3_7_3.Remove(SDL3_7_3[i]);
                        break;
                    }


                }
                //行号加一
                row++;
            }
        }
    }
}

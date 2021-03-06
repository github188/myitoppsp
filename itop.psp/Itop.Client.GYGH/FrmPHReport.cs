using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Itop.Client.Common;
using Itop.Domain.Layouts;
using Itop.Domain.Table;
using Itop.Common;
using Itop.Domain.Graphics;
using Itop.Domain.Stutistic;

using System.Reflection;
using System.Diagnostics;
using DevExpress.Utils;
using Itop.Domain.RightManager;
using Itop.Client.Base;
using Itop.Client.Table;
using FarPoint.Win;
using Itop.Domain.Forecast;
using Microsoft.Office.Interop.Excel;

namespace Itop.Client.GYGH
{
    public partial class FrmPHReport : FormBase
    {
        

        fpcommon fc = new fpcommon();
        
        //定义一个边框线
        LineBorder border = new LineBorder(Color.Black);
        //工程号
        string ProjID = Itop.Client.MIS.ProgUID;
        public FrmPHReport()
        {
            InitializeComponent();
        }
       // public Ps_forecast_list forecastReport = new Ps_forecast_list();
        private void FrmGYDWGH_Load(object sender, EventArgs e)
        {

            //根据窗口变化全部添满
            fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            //选择预测方案
            //FrmSelforcast fs = new FrmSelforcast();
            //if (fs.ShowDialog()==DialogResult.OK)
            //{
            //    forecastReport = fs.report;
            //}
            //添加表
            fpSpread1_addsheet();
            
            
        }
        private void SpreadRemoveEmptyCells(FarPoint.Win.Spread.FpSpread tempspread)
        {
            //本方法用于去掉多表格中多余的行和列（空）
            //定义无空单元格模式
           FarPoint.Win.Spread.Model.INonEmptyCells nec;
            //计算spread有多少个表格
           int sheetscount = tempspread.Sheets.Count;
            //定义行数
           int rowcount = 0;
            //定义列数
           int colcount = 0;
           for (int m = 0; m < sheetscount;m++ )
           {
               nec = (FarPoint.Win.Spread.Model.INonEmptyCells)tempspread.Sheets[m].Models.Data;
               //计算无空单元格列数
               colcount= nec.NonEmptyColumnCount;
               //计算无空单元格行数
               rowcount = nec.NonEmptyRowCount;
               tempspread.Sheets[m].RowCount = rowcount;
               tempspread.Sheets[m].ColumnCount = colcount;
           }
        }
        private void fpSpread1_addsheet()
        {
            WaitDialogForm wait = null;
            wait = new WaitDialogForm("", "正在加载数据, 请稍候...");
            try
            {
                //打开Excel表格
                fpSpread1.Sheets.Clear();
                fpSpread1.OpenExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\dlphtj.xls");
                SpreadRemoveEmptyCells(fpSpread1);
            }
            catch (System.Exception e)
            {
                //如果打开出错则重新生成并保存
                fpSpread1.Sheets.Clear();
                Firstadddata();
                //判断文件夹是否存在，不存在则创建
                if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\xls"))
                {
                    Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\xls");
                }
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\dlphtj.xls");
            }
            wait.Close();
        }   
        private void  recreatsheetbydt(string title, System.Data.DataTable dt, Dictionary<string, Columqk> viscol, int sheetindex)
        {
            FarPoint.Win.Spread.SheetView Sheet1 = new FarPoint.Win.Spread.SheetView();
            if (fpSpread1.Sheets.Count-1>=sheetindex)
            {
                Sheet1 = fpSpread1.Sheets[sheetindex];
            }
            else
            {
                Sheet1.SheetName = title;
                fpSpread1.Sheets.Add(Sheet1);
            }
            if (Sheet1.Rows.Count>0)
            {
                Sheet1.RowCount = 0;
                Sheet1.ColumnCount = 0;
            }
            Sheet1.Columns.Count = viscol.Count;
            Sheet1.Rows.Count = dt.Rows.Count + 1;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet1);
            //设表格模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet1);
            int visrowcount = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (KeyValuePair<string, Columqk> kp in viscol)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (kp.Key == dt.Columns[j].ColumnName)
                        {
                            Sheet1.SetValue(0, visrowcount, viscol[dt.Columns[j].ColumnName].colname);
                            Sheet1.Columns[visrowcount].Width = 70;
                            if (viscol[dt.Columns[j].ColumnName].CellType == 1)
                            {
                                Sheet1.Columns[visrowcount].CellType = new FarPoint.Win.Spread.CellType.PercentCellType();
                            }
                            else if (viscol[dt.Columns[j].ColumnName].CellType == 2)
                            {
                                FarPoint.Win.Spread.CellType.NumberCellType newcelltype = new FarPoint.Win.Spread.CellType.NumberCellType();
                                newcelltype.DecimalPlaces = viscol[dt.Columns[j].ColumnName].weishu;
                                Sheet1.Columns[visrowcount].CellType = newcelltype;

                            }

                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                Sheet1.SetValue(k + 1, visrowcount, dt.Rows[k][j].ToString());
                            }
                            visrowcount++;
                        }
                    }
                }
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    if (viscol.ContainsKey(dt.Columns[i].ColumnName))
                //    {
                //        Sheet1.SetValue(0, visrowcount, viscol[dt.Columns[i].ColumnName].colname);
                //        Sheet1.Columns[visrowcount].Width = 70;
                //        if (viscol[dt.Columns[i].ColumnName].CellType == 1)
                //        {
                //            Sheet1.Columns[visrowcount].CellType = new FarPoint.Win.Spread.CellType.PercentCellType();
                //        }
                //        else if (viscol[dt.Columns[i].ColumnName].CellType == 2)
                //        {
                //            FarPoint.Win.Spread.CellType.NumberCellType newcelltype = new FarPoint.Win.Spread.CellType.NumberCellType();
                //            newcelltype.DecimalPlaces = viscol[dt.Columns[i].ColumnName].weishu;
                //            Sheet1.Columns[visrowcount].CellType = newcelltype;

                //        }

                //        for (int j = 0; j < dt.Rows.Count; j++)
                //        {
                //            Sheet1.SetValue(j + 1, visrowcount, dt.Rows[j][i].ToString());
                //        }
                //        visrowcount++;
                //    }

                //}
                Sheet1.Rows[0].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            }

            //使表格只读
            fc.Sheet_Locked(Sheet1);
        }
        /// <summary>
        /// 根据datatable创建一个工作表
        /// </summary>
        /// <param name="title"> 报表标题</param>
        /// <param name="dt">数据源</param>
        /// <param name="viscol">控制显示的列 列明为键 新名称为值</param>
        /// <param name="bl">图表是否带有图例，是true，否false</param>
        private void creatsheetbydt(string title, System.Data.DataTable dt, Dictionary<string, Columqk> viscol)
        {
            FarPoint.Win.Spread.SheetView Sheet1 = new FarPoint.Win.Spread.SheetView();
            Sheet1.SheetName = title;
            fpSpread1.Sheets.Add(Sheet1);
            Sheet1.Columns.Count = viscol.Count;
            Sheet1.Rows.Count = dt.Rows.Count + 1;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet1);
            //设表格模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet1);
            int visrowcount = 0;
            if (dt.Rows.Count>0)
            { 
               
                foreach ( KeyValuePair<string,Columqk> kp in viscol)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (kp.Key == dt.Columns[j].ColumnName)
                        {
                            Sheet1.SetValue(0, visrowcount, viscol[dt.Columns[j].ColumnName].colname);
                            Sheet1.Columns[visrowcount].Width = 70;
                            if (viscol[dt.Columns[j].ColumnName].CellType == 1)
                            {
                                Sheet1.Columns[visrowcount].CellType = new FarPoint.Win.Spread.CellType.PercentCellType();
                            }
                            else if (viscol[dt.Columns[j].ColumnName].CellType == 2)
                            {
                                FarPoint.Win.Spread.CellType.NumberCellType newcelltype = new FarPoint.Win.Spread.CellType.NumberCellType();
                                newcelltype.DecimalPlaces = viscol[dt.Columns[j].ColumnName].weishu;
                                Sheet1.Columns[visrowcount].CellType = newcelltype;

                            }

                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                Sheet1.SetValue(k + 1, visrowcount, dt.Rows[k][j].ToString());
                            }
                            visrowcount++;
                        }
                    }
                }
             
            //    for (int i = 0; i < dt.Columns.Count;i++ )
            //   {
            //    if (viscol.ContainsKey(dt.Columns[i].ColumnName))
            //    {
            //        Sheet1.SetValue(0, visrowcount, viscol[dt.Columns[i].ColumnName].colname);
            //        Sheet1.Columns[visrowcount].Width = 70;
            //        if (viscol[dt.Columns[i].ColumnName].CellType == 1)
            //        {
            //            Sheet1.Columns[visrowcount].CellType = new FarPoint.Win.Spread.CellType.PercentCellType();
            //        }
            //        else if (viscol[dt.Columns[i].ColumnName].CellType == 2)
            //        {
            //            FarPoint.Win.Spread.CellType.NumberCellType newcelltype = new FarPoint.Win.Spread.CellType.NumberCellType();
            //            newcelltype.DecimalPlaces = viscol[dt.Columns[i].ColumnName].weishu;
            //            Sheet1.Columns[visrowcount].CellType = newcelltype;

            //        }

            //        for (int j = 0; j < dt.Rows.Count; j++)
            //        {
            //            Sheet1.SetValue(j + 1, visrowcount, dt.Rows[j][i].ToString());
            //        }
            //        visrowcount++;
            //    }
                                 
            //}
             Sheet1.Rows[0].CellType=new FarPoint.Win.Spread.CellType.TextCellType();
            }
            
            //使表格只读
            fc.Sheet_Locked(Sheet1);

        }
        private  void Firstadddata()
        {
            
            //逐步获取各个数据的datatable;

            //形成基础数据电力发展实绩历年地区生产
          
            //500千伏分区分年电容量计算表

            build_500rltj();
             //500千伏分区分年容载比计算表
            build_500rzbtj();

        }

#region 电量平衡
        private string GetProjectID=Itop.Client.MIS.ProgUID;
#region 500KV
        private void build_500rltj()
        {
            Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "500千伏分区分年电容量计算表";

            OperTable oper = new OperTable();
            yAnge = oper.GetYearRange("Col5='" + GetProjectID + "' and Col4='" + OperTable.ph500 + "'");
            System.Data.DataTable datatable = get500phtable(yAnge);
            System.Data.DataTable dt = ResultDataTable(ConvertTreeListToDataTable(datatable, false, yAnge), ref viscol, yAnge);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 0);
            }

        }
        //500千伏分区分年容载比计算表
        private void build_500rzbtj()
        {
            Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "500千伏分区分年容载比计算表";
            OperTable oper = new OperTable();
            yAnge = oper.GetYearRange("Col5='" + GetProjectID + "' and Col4='" + OperTable.ph500 + "'");
            System.Data.DataTable datatable = get500phtable(yAnge);
            System.Data.DataTable dt = ResultDataTable1(ConvertTreeListToDataTable(datatable, false, yAnge), ref viscol, yAnge);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 1);
            }

        }
        private System.Data.DataTable get500phtable(Ps_YearRange yange)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            if (dataTable != null)
            {
                dataTable.Columns.Clear();

            }
            string conn = "ProjectID='" + GetProjectID + "' and ParentID='0'";
            IList pList = Common.Services.BaseService.GetList("SelectPs_Table_500PHListByConn", conn);
            for (int i = 0; i < pList.Count; i++)
            {
                UpdateFuHe(((Ps_Table_500PH)pList[i]).Title, ((Ps_Table_500PH)pList[i]).ID, "yf");
            }
            string con = "ProjectID='" + GetProjectID + "'";
            IList listTypes = Common.Services.BaseService.GetList("SelectPs_Table_500PHListByConn", con);
            CaleHeTable(ref listTypes);
            AddRows(ref listTypes, "ParentID='0' and ProjectID='" + GetProjectID + "'");
            //  CalcTotal(ref listTypes);
            dataTable = Itop.Common.DataConverter.ToDataTable(listTypes, typeof(Ps_Table_500PH));
            return dataTable;
        }

        private System.Data.DataTable ResultDataTable1(System.Data.DataTable sourceDataTable, ref Dictionary<string, Columqk> viscol, Ps_YearRange yAnge)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();
            dtReturn.Columns.Add("Title", typeof(string));
            viscol["Title"] = new Columqk("名称", 3, 2);
            for (int i = yAnge.StartYear; i <= yAnge.FinishYear; i++)
            {

                dtReturn.Columns.Add(i.ToString() + "年", typeof(double));
                viscol[i.ToString() + "年"] = new Columqk(i.ToString() + "年", 2, 2);
            }

            int nRowMaxLoad = 0;//地区最高负荷所在行
            int nRowMaxPower = 0;//地区电网供电能力所在行
            int nRowMaxPowerLow = 0;//枯水期地区电网供电能力所在行

            #region 填充数据
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                if (sourceDataTable.Rows[i]["Title"].ToString().IndexOf("500千伏需要容量") != -1)
                    continue;
                DataRow newRow = dtReturn.NewRow();
                DataRow sourceRow = sourceDataTable.Rows[i];
                foreach (DataColumn column in dtReturn.Columns)
                {
                    newRow[column.ColumnName] = sourceRow[ConvertYear(column.ColumnName)];
                }
                dtReturn.Rows.Add(newRow);


            }
            #endregion


            return dtReturn;
        }
        //根据选择的统计年份，生成统计结果数据表
        private System.Data.DataTable ResultDataTable(System.Data.DataTable sourceDataTable, ref Dictionary<string, Columqk> viscol, Ps_YearRange yAnge)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();
            dtReturn.Columns.Add("Title", typeof(string));
            viscol["Title"] = new Columqk("名称", 3, 2);

            for (int i = yAnge.StartYear; i <= yAnge.FinishYear; i++)
            {

                dtReturn.Columns.Add(i.ToString() + "年", typeof(double));
                viscol[i.ToString() + "年"] = new Columqk(i.ToString() + "年", 2, 2);
            }


            int nRowMaxLoad = 0;//地区最高负荷所在行
            int nRowMaxPower = 0;//地区电网供电能力所在行
            int nRowMaxPowerLow = 0;//枯水期地区电网供电能力所在行

            #region 填充数据
            for (int i = 0; i < sourceDataTable.Rows.Count; i++)
            {
                DataRow newRow = dtReturn.NewRow();
                DataRow sourceRow = sourceDataTable.Rows[i];
                foreach (DataColumn column in dtReturn.Columns)
                {
                    newRow[column.ColumnName] = sourceRow[ConvertYear(column.ColumnName)];
                }
                dtReturn.Rows.Add(newRow);


            }
            #endregion

            //#region 计算电力盈亏和枯水期地区电力盈亏
            //foreach (ChoosedYears choosedYear in listChoosedYears)
            //{
            //    object maxLoad = dtReturn.Rows[nRowMaxLoad][choosedYear.Year + "年"];
            //    if (maxLoad != DBNull.Value)
            //    {
            //        object maxPower = dtReturn.Rows[nRowMaxPower][choosedYear.Year + "年"];
            //        if (maxPower != DBNull.Value)
            //        {
            //            dtReturn.Rows[nRowMaxPower + 1][choosedYear.Year + "年"] = (double)maxPower - (double)maxLoad;
            //        }

            //        maxPower = dtReturn.Rows[nRowMaxPowerLow][choosedYear.Year + "年"];
            //        if (maxPower != DBNull.Value)
            //        {
            //            dtReturn.Rows[nRowMaxPowerLow + 1][choosedYear.Year + "年"] = (double)maxPower - (double)maxLoad;
            //        }
            //    }
            //}
            //#endregion

            return dtReturn;
        }
        public string ConvertYear(string year)
        {
            if (year.IndexOf("年") == -1)
                return year;
            return "y" + year.Substring(0, 4);
        }
        //把树控件内容按显示顺序生成到DataTable中
        private System.Data.DataTable ConvertTreeListToDataTable(System.Data.DataTable xTreeList, bool bRemove, Ps_YearRange yAnge)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            List<string> listColID = new List<string>();
            listColID.Add("BuildIng");
            dt.Columns.Add("BuildIng", typeof(string));
            listColID.Add("BuildYear");
            dt.Columns.Add("BuildYear", typeof(string));

            listColID.Add("BuildEd");
            dt.Columns.Add("BuildEd", typeof(string));
            listColID.Add("Sort");
            dt.Columns.Add("Sort", typeof(int));
            listColID.Add("ProjectID");
            dt.Columns.Add("ProjectID", typeof(string));

            listColID.Add("ParentID");
            dt.Columns.Add("ParentID", typeof(string));

            listColID.Add("Title");
            dt.Columns.Add("Title", typeof(string));
            dt.Columns["Title"].Caption = "分类";
            for (int i = yAnge.StartYear; i <= yAnge.FinishYear; i++)
            {
                listColID.Add("y" + i.ToString());
                dt.Columns.Add("y" + i.ToString(), typeof(double));
            }
            for (int i = 1; i < 5; i++)
            {
                listColID.Add("Col" + i.ToString());
                dt.Columns.Add("Col" + i.ToString(), typeof(string));
            }
            foreach (DataRow node in xTreeList.Rows)
            {
                AddNodeDataToDataTable(dt, xTreeList, node, listColID, bRemove);
            }

            return dt;
        }
        private void AddNodeDataToDataTable(System.Data.DataTable dt, System.Data.DataTable datasource, DataRow node, List<string> listColID, bool bRemove)
        {
            DataRow newRow = dt.NewRow();
            DataRow[] dv = datasource.Select("ID='" + node["ParentID"] + "'");
            foreach (string colID in listColID)
            {
                //分类名，第二层及以后在前面加空格
                if (colID == "Title" && dv.Length > 0)
                {
                    newRow[colID] = "　　" + node[colID];
                }
                else
                {
                    newRow[colID] = node[colID];
                }
            }
            if (bRemove)
            {
                if (newRow["Col1"].ToString() != "1" && newRow["BuildEd"].ToString() != "total")
                    dt.Rows.Add(newRow);
            }
            else
                dt.Rows.Add(newRow);
            dv = datasource.Select("ParentID='" + node["ID"] + "'");
            foreach (DataRow nd in dv)
            {
                AddNodeDataToDataTable(dt, datasource, nd, listColID, bRemove);
            }
        }
        public int[] GetYears()
        {
            Ps_YearRange yr = yAnge;
            int[] year = new int[4] { yr.BeginYear, yr.StartYear, yr.FinishYear, yr.EndYear };
            return year;
        }
        private string rongZai220 = "1.5";
        public string RongZai(Ps_Table_500PH cur)
        {
            if (cur == null || cur.BuildYear == null || cur.BuildYear == "")
                return rongZai220;
            return cur.BuildYear;
        }
        public string RongZai1(Ps_Table_200PH cur)
        {
            if (cur == null || cur.BuildYear == null || cur.BuildYear == "")
                return rongZai220;
            return cur.BuildYear;
        }
        public void AddRows(ref IList list, string con)
        {
            //IList list = new List<Ps_Table_500PH>();
            string conn = "ParentID='0' and ProjectID='" + GetProjectID + "'";

            int[] year = GetYears();
            IList pareList = Common.Services.BaseService.GetList("SelectPs_Table_500PHListByConn", con);
            try
            {
                for (int i = 0; i < pareList.Count; i++)
                {
                    Ps_Table_500PH ps_table1 = new Ps_Table_500PH();
                    //Ps_Table_500PH con = new Ps_Table_500PH();
                    //con.Col4 = rongZai220;
                    //con.Title = "ParentID='" + ((Ps_Table_500PH)pareList[i]).ID + "' and Col1='0'";
                    //IList childList1 = Common.Services.BaseService.GetList("SelectPs_Table_500PHJiByConn", con);
                    //CaleHeTable(ref childList1);
                    //ps_table1 = (Ps_Table_500PH)childList1[0];
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (((Ps_Table_500PH)list[j]).ParentID == ((Ps_Table_500PH)pareList[i]).ID && ((Ps_Table_500PH)list[j]).Col1 == "0")
                        {
                            for (int k = year[1]; k <= year[2]; k++)
                            {
                                ps_table1.GetType().GetProperty("y" + k).SetValue(ps_table1, Math.Round(double.Parse(((Ps_Table_500PH)list[j]).GetType().GetProperty("y" + k).GetValue(((Ps_Table_500PH)list[j]), null).ToString()) * double.Parse(RongZai((Ps_Table_500PH)pareList[i])), 1), null);
                            }
                        }
                    }
                    ps_table1.ParentID = ((Ps_Table_500PH)pareList[i]).ID;
                    ps_table1.ID = Guid.NewGuid().ToString();
                    ps_table1.ID += "|" + GetProjectID;
                    ps_table1.Sort = 2;
                    ps_table1.Col1 = "no";
                    ps_table1.Title = "500千伏需要容量(" + RongZai((Ps_Table_500PH)pareList[i]) + ")";
                    list.Add(ps_table1);

                    Ps_Table_500PH ps_table = new Ps_Table_500PH();

                    //conn = "ParentID='" + ((Ps_Table_500PH)pareList[i]).ID + "' and Col1='1'";
                    //IList childList = Common.Services.BaseService.GetList("SelectPs_Table_500PHSumByConn", conn);
                    //CaleHeTable(ref childList);
                    //ps_table = (Ps_Table_500PH)childList[0];
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (((Ps_Table_500PH)list[j]).ParentID == ((Ps_Table_500PH)pareList[i]).ID && ((Ps_Table_500PH)list[j]).Col1 == "1")
                        {
                            for (int k = year[1]; k <= year[2]; k++)
                            {
                                ps_table.GetType().GetProperty("y" + k).SetValue(ps_table, double.Parse(ps_table.GetType().GetProperty("y" + k).GetValue(ps_table, null).ToString()) + double.Parse(((Ps_Table_500PH)list[j]).GetType().GetProperty("y" + k).GetValue(((Ps_Table_500PH)list[j]), null).ToString()), null);
                            }
                        }
                    }
                    ps_table.ParentID = ((Ps_Table_500PH)pareList[i]).ID;
                    ps_table.ID = Guid.NewGuid().ToString();
                    ps_table.ID += "|" + GetProjectID;
                    ps_table.Sort = 3;
                    ps_table.Col1 = "no";
                    ps_table.Title = "500千伏公用变电站总容量";
                    list.Add(ps_table);

                    // conn = "ParentID='" + ((Ps_Table_500PH)pareList[i]).ID + "' and Col1='0'";
                    Ps_Table_500PH temp = new Ps_Table_500PH();// (Ps_Table_500PH)Common.Services.BaseService.GetList("SelectPs_Table_500PHListByConn", conn)[0];
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (((Ps_Table_500PH)list[j]).ParentID == ((Ps_Table_500PH)pareList[i]).ID && ((Ps_Table_500PH)list[j]).Col1 == "0")
                        {
                            temp = (Ps_Table_500PH)list[j];
                        }
                    }
                    Ps_Table_500PH ps_table2 = new Ps_Table_500PH();
                    for (int j = year[1]; j <= year[2]; j++)
                    {
                        double d = (double)temp.GetType().GetProperty("y" + j).GetValue(temp, null);
                        if (d != 0.0)
                        {
                            double chu = (double)ps_table.GetType().GetProperty("y" + j).GetValue(ps_table, null);
                            ps_table2.GetType().GetProperty("y" + j).SetValue(ps_table2, Math.Round(chu / d, 1), null);
                        }
                    }
                    ps_table2.ParentID = ((Ps_Table_500PH)pareList[i]).ID;
                    ps_table2.ID = Guid.NewGuid().ToString();
                    ps_table2.ID += "|" + GetProjectID;
                    ps_table2.Sort = 1000;
                    ps_table2.Col1 = "no";
                    ps_table2.Title = "500千伏容载比";
                    list.Add(ps_table2);
                }

                //  CalcTotal(ref list);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            //return list;
        }

        public void UpdateFuHe(string title, string id, string str)
        {
            string title1 = title;
            if (title1.IndexOf('（') != -1)
            {
                title1 = title1.Substring(0, title1.IndexOf('（'));
            }
            string conn = "ProjectID='" + GetProjectID + "' and Title='" + title1 + "' and parentID='0'";
            IList listf = Common.Services.BaseService.GetList("SelectPs_Table_500ResultByConn", conn);
            if (listf.Count > 0)
            {
                conn = "ProjectID='" + GetProjectID + "' and ParentID='" + id + "' and Col1='0'";
                IList myList = Common.Services.BaseService.GetList("SelectPs_Table_500PHListByConn", conn);
                if (myList.Count > 0)
                {
                    conn = "ProjectID='" + GetProjectID + "' and Col1='" + ((Ps_Table_500PH)myList[0]).Col2 + "' and ParentID='" + ((Ps_Table_500Result)listf[0]).ID + "'";
                    IList listt = Common.Services.BaseService.GetList("SelectPs_Table_500ResultByConn", conn);
                    if (listt.Count == 0)
                    {
                        conn = "ProjectID='" + GetProjectID + "' and Col1='" + "no1"/*((Ps_Table_500PH)myList[0]).Col2*/ + "' and ParentID='" + ((Ps_Table_500Result)listf[0]).ID + "'";
                        listt = Common.Services.BaseService.GetList("SelectPs_Table_500ResultByConn", conn);
                    }
                    if (listt.Count > 0)
                    {
                        for (int i = yAnge.BeginYear; i <= yAnge.EndYear; i++)
                        {
                            double t1 = double.Parse(listt[0].GetType().GetProperty(str + i.ToString()).GetValue(listt[0], null).ToString());
                            myList[0].GetType().GetProperty("y" + i.ToString()).SetValue(myList[0], t1, null);
                        }
                        Common.Services.BaseService.Update<Ps_Table_500PH>((Ps_Table_500PH)myList[0]);
                    }
                }
            }
        }

        public void CaleHeTable(ref IList heList)
        {
            Ps_YearRange range = yAnge;
            for (int i = 0; i < heList.Count; i++)
            {
                if (((Ps_Table_500PH)heList[i]).Col1 == "1")
                {
                    string conn = "ParentID='" + ((Ps_Table_500PH)heList[i]).ID + "'";
                    IList tList = Common.Services.BaseService.GetList("SelectPs_Table_EditListByConn", conn);
                    for (int j = 0; j < tList.Count; j++)
                    {
                        if (((Ps_Table_Edit)tList[j]).Status == "扩建")
                        {
                            for (int k = int.Parse(((Ps_Table_Edit)tList[j]).FinishYear); k <= range.EndYear; k++)
                            {
                                double old = (double)((Ps_Table_500PH)heList[i]).GetType().GetProperty("y" + k.ToString()).GetValue(((Ps_Table_500PH)heList[i]), null);
                                ((Ps_Table_500PH)heList[i]).GetType().GetProperty("y" + k.ToString()).SetValue(((Ps_Table_500PH)heList[i]), double.Parse(((Ps_Table_Edit)tList[j]).Volume) + old, null);
                            }
                        }
                        else if (((Ps_Table_Edit)tList[j]).Status == "改造")
                        {
                            for (int k = int.Parse(((Ps_Table_Edit)tList[j]).FinishYear); k <= range.EndYear; k++)
                            {
                                double old = (double)((Ps_Table_500PH)heList[i]).GetType().GetProperty("y" + k.ToString()).GetValue(((Ps_Table_500PH)heList[i]), null);
                                ((Ps_Table_500PH)heList[i]).GetType().GetProperty("y" + k.ToString()).SetValue(((Ps_Table_500PH)heList[i]), double.Parse(((Ps_Table_Edit)tList[j]).Volume), null);
                            }
                        }
                        else if (((Ps_Table_Edit)tList[j]).Status == "拆除")
                        {
                            for (int k = int.Parse(((Ps_Table_Edit)tList[j]).FinishYear); k <= range.EndYear; k++)
                            {
                                ((Ps_Table_500PH)heList[i]).GetType().GetProperty("y" + k.ToString()).SetValue(((Ps_Table_500PH)heList[i]), 0.0, null);
                            }
                        }
                    }
                }
            }
        }
#endregion
        //500千伏分区分年电容量计算表
      private  Ps_YearRange yAnge = new Ps_YearRange();
#region 200KV
      //"220千伏分区分年电容量计算表"
        private void build_200rltj()
        {
            Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "200千伏分区分年电容量计算表";

            OperTable oper = new OperTable();
            yAnge = oper.GetYearRange("Col5='" + GetProjectID + "' and Col4='" + OperTable.ph220 + "'");
            System.Data.DataTable datatable = get200phtable(yAnge);
            System.Data.DataTable dt = ResultDataTable(ConvertTreeListToDataTable(datatable, false, yAnge), ref viscol, yAnge);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol,2);
            }
        }
        //"220千伏分区分年容载比计算表"
        private void build_200rzbtj()
        {
            Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "200千伏分区分年容载比计算表";
            OperTable oper = new OperTable();
            yAnge = oper.GetYearRange("Col5='" + GetProjectID + "' and Col4='" + OperTable.ph220 + "'");
            System.Data.DataTable datatable = get200phtable(yAnge);
            System.Data.DataTable dt = ResultDataTable1(ConvertTreeListToDataTable(datatable, false, yAnge), ref viscol, yAnge);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 3);
            }

        }
        private System.Data.DataTable get200phtable(Ps_YearRange yange)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            if (dataTable != null)
            {
                dataTable.Columns.Clear();

            }
            string conn = "ProjectID='" + GetProjectID + "' and ParentID='0'";
            IList pList = Common.Services.BaseService.GetList("SelectPs_Table_200PHListByConn", conn);
            for (int i = 0; i < pList.Count; i++)
            {
                UpdateFuHe1(((Ps_Table_200PH)pList[i]).Title, ((Ps_Table_200PH)pList[i]).ID, "yf");
            }
            string con = "ProjectID='" + GetProjectID + "'";
            IList listTypes = Common.Services.BaseService.GetList("SelectPs_Table_200PHListByConn", con);
            CaleHeTable1(ref listTypes);
            AddRows1(ref listTypes, "ParentID='0' and ProjectID='" + GetProjectID + "'");
            //  CalcTotal(ref listTypes);
            dataTable = Itop.Common.DataConverter.ToDataTable(listTypes, typeof(Ps_Table_200PH));
            return dataTable;
        }
        public void AddRows1(ref IList list, string con)
        {
            //IList list = new List<Ps_Table_500PH>();
            string conn = "ParentID='0' and ProjectID='" + GetProjectID + "'";

            int[] year = GetYears();
            IList pareList = Common.Services.BaseService.GetList("SelectPs_Table_200PHListByConn", con);
            try
            {
                for (int i = 0; i < pareList.Count; i++)
                {
                    Ps_Table_200PH ps_table1 = new Ps_Table_200PH();
                    //Ps_Table_500PH con = new Ps_Table_500PH();
                    //con.Col4 = rongZai220;
                    //con.Title = "ParentID='" + ((Ps_Table_500PH)pareList[i]).ID + "' and Col1='0'";
                    //IList childList1 = Common.Services.BaseService.GetList("SelectPs_Table_500PHJiByConn", con);
                    //CaleHeTable(ref childList1);
                    //ps_table1 = (Ps_Table_500PH)childList1[0];
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (((Ps_Table_200PH)list[j]).ParentID == ((Ps_Table_200PH)pareList[i]).ID && ((Ps_Table_200PH)list[j]).Col1 == "0")
                        {
                            for (int k = year[1]; k <= year[2]; k++)
                            {
                                ps_table1.GetType().GetProperty("y" + k).SetValue(ps_table1, Math.Round(double.Parse(((Ps_Table_200PH)list[j]).GetType().GetProperty("y" + k).GetValue(((Ps_Table_200PH)list[j]), null).ToString()) * double.Parse(RongZai1((Ps_Table_200PH)pareList[i])), 1), null);
                            }
                        }
                    }
                    ps_table1.ParentID = ((Ps_Table_200PH)pareList[i]).ID;
                    ps_table1.ID = Guid.NewGuid().ToString();
                    ps_table1.ID += "|" + GetProjectID;
                    ps_table1.Sort = 2;
                    ps_table1.Col1 = "no";
                    ps_table1.Title = "200千伏需要容量(" + RongZai1((Ps_Table_200PH)pareList[i]) + ")";
                    list.Add(ps_table1);

                    Ps_Table_200PH ps_table = new Ps_Table_200PH();

                    //conn = "ParentID='" + ((Ps_Table_500PH)pareList[i]).ID + "' and Col1='1'";
                    //IList childList = Common.Services.BaseService.GetList("SelectPs_Table_500PHSumByConn", conn);
                    //CaleHeTable(ref childList);
                    //ps_table = (Ps_Table_500PH)childList[0];
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (((Ps_Table_200PH)list[j]).ParentID == ((Ps_Table_200PH)pareList[i]).ID && ((Ps_Table_200PH)list[j]).Col1 == "1")
                        {
                            for (int k = year[1]; k <= year[2]; k++)
                            {
                                ps_table.GetType().GetProperty("y" + k).SetValue(ps_table, double.Parse(ps_table.GetType().GetProperty("y" + k).GetValue(ps_table, null).ToString()) + double.Parse(((Ps_Table_200PH)list[j]).GetType().GetProperty("y" + k).GetValue(((Ps_Table_200PH)list[j]), null).ToString()), null);
                            }
                        }
                    }
                    ps_table.ParentID = ((Ps_Table_200PH)pareList[i]).ID;
                    ps_table.ID = Guid.NewGuid().ToString();
                    ps_table.ID += "|" + GetProjectID;
                    ps_table.Sort = 3;
                    ps_table.Col1 = "no";
                    ps_table.Title = "200千伏公用变电站总容量";
                    list.Add(ps_table);

                    // conn = "ParentID='" + ((Ps_Table_500PH)pareList[i]).ID + "' and Col1='0'";
                    Ps_Table_200PH temp = new Ps_Table_200PH();// (Ps_Table_500PH)Common.Services.BaseService.GetList("SelectPs_Table_500PHListByConn", conn)[0];
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (((Ps_Table_200PH)list[j]).ParentID == ((Ps_Table_200PH)pareList[i]).ID && ((Ps_Table_200PH)list[j]).Col1 == "0")
                        {
                            temp = (Ps_Table_200PH)list[j];
                        }
                    }
                    Ps_Table_200PH ps_table2 = new Ps_Table_200PH();
                    for (int j = year[1]; j <= year[2]; j++)
                    {
                        double d = (double)temp.GetType().GetProperty("y" + j).GetValue(temp, null);
                        if (d != 0.0)
                        {
                            double chu = (double)ps_table.GetType().GetProperty("y" + j).GetValue(ps_table, null);
                            ps_table2.GetType().GetProperty("y" + j).SetValue(ps_table2, Math.Round(chu / d, 1), null);
                        }
                    }
                    ps_table2.ParentID = ((Ps_Table_200PH)pareList[i]).ID;
                    ps_table2.ID = Guid.NewGuid().ToString();
                    ps_table2.ID += "|" + GetProjectID;
                    ps_table2.Sort = 1000;
                    ps_table2.Col1 = "no";
                    ps_table2.Title = "200千伏容载比";
                    list.Add(ps_table2);
                }

                //  CalcTotal(ref list);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            //return list;
        }
        public void UpdateFuHe1(string title, string id, string str)
        {
            string title1 = title;
            if (title1.IndexOf('（') != -1)
            {
                title1 = title1.Substring(0, title1.IndexOf('（'));
            }
            string conn = "ProjectID='" + GetProjectID + "' and Title='" + title1 + "' and parentID='0'";
            IList listf = Common.Services.BaseService.GetList("SelectPs_Table_200ResultByConn", conn);
            if (listf.Count > 0)
            {
                conn = "ProjectID='" + GetProjectID + "' and ParentID='" + id + "' and Col1='0'";
                IList myList = Common.Services.BaseService.GetList("SelectPs_Table_200PHListByConn", conn);
                if (myList.Count > 0)
                {
                    conn = "ProjectID='" + GetProjectID + "' and Col1='" + ((Ps_Table_200PH)myList[0]).Col2 + "' and ParentID='" + ((Ps_Table_220Result)listf[0]).ID + "'";
                    IList listt = Common.Services.BaseService.GetList("SelectPs_Table_200ResultByConn", conn);
                    if (listt.Count == 0)
                    {
                        conn = "ProjectID='" + GetProjectID + "' and Col1='" + "no1"/*((Ps_Table_500PH)myList[0]).Col2*/ + "' and ParentID='" + ((Ps_Table_220Result)listf[0]).ID + "'";
                        listt = Common.Services.BaseService.GetList("SelectPs_Table_200ResultByConn", conn);
                    }
                    if (listt.Count > 0)
                    {
                        for (int i = yAnge.BeginYear; i <= yAnge.EndYear; i++)
                        {
                            double t1 = double.Parse(listt[0].GetType().GetProperty(str + i.ToString()).GetValue(listt[0], null).ToString());
                            myList[0].GetType().GetProperty("y" + i.ToString()).SetValue(myList[0], t1, null);
                        }
                        Common.Services.BaseService.Update<Ps_Table_200PH>((Ps_Table_200PH)myList[0]);
                    }
                }
            }
        }

        public void CaleHeTable1(ref IList heList)
        {
            Ps_YearRange range = yAnge;
            for (int i = 0; i < heList.Count; i++)
            {
                if (((Ps_Table_200PH)heList[i]).Col1 == "1")
                {
                    string conn = "ParentID='" + ((Ps_Table_200PH)heList[i]).ID + "'";
                    IList tList = Common.Services.BaseService.GetList("SelectPs_Table_EditListByConn", conn);
                    for (int j = 0; j < tList.Count; j++)
                    {
                        if (((Ps_Table_Edit)tList[j]).Status == "扩建")
                        {
                            for (int k = int.Parse(((Ps_Table_Edit)tList[j]).FinishYear); k <= range.EndYear; k++)
                            {
                                double old = (double)((Ps_Table_200PH)heList[i]).GetType().GetProperty("y" + k.ToString()).GetValue(((Ps_Table_200PH)heList[i]), null);
                                ((Ps_Table_200PH)heList[i]).GetType().GetProperty("y" + k.ToString()).SetValue(((Ps_Table_200PH)heList[i]), double.Parse(((Ps_Table_Edit)tList[j]).Volume) + old, null);
                            }
                        }
                        else if (((Ps_Table_Edit)tList[j]).Status == "改造")
                        {
                            for (int k = int.Parse(((Ps_Table_Edit)tList[j]).FinishYear); k <= range.EndYear; k++)
                            {
                                double old = (double)((Ps_Table_200PH)heList[i]).GetType().GetProperty("y" + k.ToString()).GetValue(((Ps_Table_200PH)heList[i]), null);
                                ((Ps_Table_200PH)heList[i]).GetType().GetProperty("y" + k.ToString()).SetValue(((Ps_Table_200PH)heList[i]), double.Parse(((Ps_Table_Edit)tList[j]).Volume), null);
                            }
                        }
                        else if (((Ps_Table_Edit)tList[j]).Status == "拆除")
                        {
                            for (int k = int.Parse(((Ps_Table_Edit)tList[j]).FinishYear); k <= range.EndYear; k++)
                            {
                                ((Ps_Table_200PH)heList[i]).GetType().GetProperty("y" + k.ToString()).SetValue(((Ps_Table_200PH)heList[i]), 0.0, null);
                            }
                        }
                    }
                }
            }
        }
#endregion

      #endregion
      private double detel_jd(double tempdouble, int tempint)
        {
            return Math.Round(tempdouble, tempint);
        }
        private void Sheet_WriteZero(FarPoint.Win.Spread.SheetView obj_sheet, int startrow, int startcol, int rowcount, int colcount)
        {
            for (int row = 0; row < rowcount; row++)
            {
                for (int col = 0; col < colcount; col++)
                {
                    obj_sheet.SetValue(startrow + row, startcol + col, 0);
                }
            }
        }

        private void barBtnDiaoChu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string fname = "";
            saveFileDialog1.Filter = "Microsoft Excel (*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fname = saveFileDialog1.FileName;

                try
                {
                    fpSpread1.SaveExcel(fname);
                    //fps.SaveExcel(fname);
                    // 定义要使用的Excel 组件接口
                    // 定义Application 对象,此对象表示整个Excel 程序
                    Microsoft.Office.Interop.Excel.Application excelApp = null;
                    // 定义Workbook对象,此对象代表工作薄
                    Microsoft.Office.Interop.Excel.Workbook workBook;
                    // 定义Worksheet 对象,此对象表示Execel 中的一张工作表
                    Microsoft.Office.Interop.Excel.Worksheet ws = null;
                    Microsoft.Office.Interop.Excel.Range range = null;
                    excelApp = new Microsoft.Office.Interop.Excel.Application();
                    workBook = excelApp.Workbooks.Open(fname, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    for (int i = 1; i <= workBook.Worksheets.Count; i++)
                    {

                        ws = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets[i];
                        //取消保护工作表
                        ws.Unprotect(Missing.Value);
                        //有数据的行数
                        int row = ws.UsedRange.Rows.Count;
                        //有数据的列数
                        int col = ws.UsedRange.Columns.Count;
                        //创建一个区域
                        range = ws.get_Range(ws.Cells[1, 1], ws.Cells[row, col]);
                        //设区域内的单元格自动换行
                        range.WrapText = true;
                        //自动行高
                        //range.Cells.Rows.AutoFit();
                        //保护工作表
                        //ws.Protect(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    }
                    //保存工作簿
                    workBook.Save();
                    //关闭工作簿
                    excelApp.Workbooks.Close();
                    if (MsgBox.ShowYesNo("导出成功，是否打开该文档？") != DialogResult.Yes)
                        return;

                    System.Diagnostics.Process.Start(fname);
                }
                catch
                {
                    MsgBox.Show("无法保存" + fname + "。请用其他文件名保存文件，或将文件存至其他位置。");
                    return;
                }
            }
        }
        private void barBtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void barBtnRefrehData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            WaitDialogForm newwait=new WaitDialogForm("", "正在更新数据, 请稍候...");
            //清空原有sheet的数据，但保留sheetname
            fc.SpreadClearSheet(fpSpread1);
            //生成一个空表用来保存当前表
            FarPoint.Win.Spread.SheetView obj_sheet = null;
            //生成一个空表，行列值都设为0用来做为程序处理时的当前表，这样可以提高处理速度
            FarPoint.Win.Spread.SheetView activesheet = new FarPoint.Win.Spread.SheetView();
            activesheet.RowCount = 0;
            activesheet.ColumnCount = 0;
            //添加空表
            fpSpread1.Sheets.Add(activesheet);
            //保留当前表，以备程序结束后还原当前表
            obj_sheet = fpSpread1.ActiveSheet;
            //将空表设为当前表
            fpSpread1.ActiveSheet = activesheet;
            shjjbyyear = true;
           
           //500千伏分区分年电容量计算表

            build_500rltj();
            //500千伏分区分年容载比计算表
            build_500rzbtj();

            shjjbyyear = false;
            fpSpread1.Sheets.Remove(activesheet);

            //移除空表
         
            //还原当前表
            fpSpread1.ActiveSheet = obj_sheet;
            newwait.Close();
            MessageBox.Show("更新数据完成！");
        }
        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            WaitDialogForm wait = null;
            wait = new WaitDialogForm("", "正在保存数据, 请稍候...");
            //判断文件夹xls是否存在，不存在则创建
            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\xls"))
            {
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\xls");
            }
            fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\dlphtj.xls");
            wait.Close();
            MsgBox.Show("保存成功");


        }

        private void fpSpread1_SheetTabClick(object sender, FarPoint.Win.Spread.SheetTabClickEventArgs e)
        {
            switch (e.SheetTabIndex)
            {
               case 2:
                   barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                   break;
                default:
                    barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    break;

            }
        }
        bool shjjbyyear = false;
        Hashtable ht = new Hashtable();
        Hashtable ht1 = new Hashtable();
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<int> li = new List<int>();
            Ps_YearRange py = new Ps_YearRange();
            py.Col4 = "电力发展实绩";
            py.Col5 = Itop.Client.MIS.ProgUID;
            int firstyear, endyear;
            IList<Ps_YearRange> list = Itop.Client.Common.Services.BaseService.GetList<Ps_YearRange>("SelectPs_YearRangeByCol5andCol4", py);
            if (list.Count > 0)
            {
                firstyear = list[0].StartYear;
                endyear = list[0].FinishYear;
            }
            else
            {
                firstyear = 1990;
                endyear = 2020;
                py.BeginYear = 1990;
                py.FinishYear = endyear;
                py.StartYear = firstyear;
                py.EndYear = 2060;
                py.ID = Guid.NewGuid().ToString();
                Itop.Client.Common.Services.BaseService.Create<Ps_YearRange>(py);
            }
            for (int i = firstyear; i <= endyear; i++)
            {
                li.Add(i);
            }

            FormChooseYears1 cy = new FormChooseYears1();
            cy.ListYearsForChoose = li;
            if (cy.ShowDialog() != DialogResult.OK)
                return;
   
            foreach (DataRow a in cy.DT.Rows)
            {
                if (a["B"].ToString() == "True")
                    ht.Add(Guid.NewGuid().ToString(), Convert.ToInt32(a["A"].ToString().Replace("年", "")));

                if (a["C"].ToString() == "True")
                    ht1.Add(Guid.NewGuid().ToString(), Convert.ToInt32(a["A"].ToString().Replace("年", "")));
            }
            shjjbyyear = true;
            //电力发展实绩社会经济用电情况
          //  build_dlhistoryjjyd();
            //再回到原始状态
            shjjbyyear = false;
        }
    }
}
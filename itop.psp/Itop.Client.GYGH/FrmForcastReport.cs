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
    public partial class FrmForcastReport : FormBase
    {
        

        fpcommon fc = new fpcommon();
        
        //定义一个边框线
        LineBorder border = new LineBorder(Color.Black);
        //工程号
        string ProjID = Itop.Client.MIS.ProgUID;
        public FrmForcastReport()
        {
            InitializeComponent();
        }
        public Ps_forecast_list forecastReport = new Ps_forecast_list();
        private void FrmGYDWGH_Load(object sender, EventArgs e)
        {

            //根据窗口变化全部添满
            fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            //选择预测方案
            FrmSelforcast fs = new FrmSelforcast();
            if (fs.ShowDialog()==DialogResult.OK)
            {
                forecastReport = fs.report;
            }
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
                fpSpread1.OpenExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\fhycztj.xls");
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
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\fhycztj.xls");
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
           //年增长率法
            build_ArverageIncreasing();
            //外推法
            build_Extrapolation();
              //指数平滑
            build_ExponentialSmoothing();
            //弹性系数
            build_CoefficientOfElasticity();
            //相关法
           
           build_CorrelationMethod();
            //灰色模型
            build_GrayModel();

      
        }
#region 负荷预测
        //年增长率法
        private void build_ArverageIncreasing()
        {
            Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "年增长率法";
            int type = 1;
            System.Data.DataTable datatable = new System.Data.DataTable();
            datatable = getavtable(1);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = OutTable(datatable, ref viscol);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 0);
            }           
            

        }
        //外推法
        private void build_Extrapolation()
        {
            Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "外推法";
            int type = 2;
            System.Data.DataTable datatable = new System.Data.DataTable();
            datatable = getavtable(type);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = OutTable(datatable, ref viscol);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 1);
            }           
        }
        //指数平滑
        private void build_ExponentialSmoothing()
        {
             Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "指数平滑";
            int type = 5;
            System.Data.DataTable datatable = new System.Data.DataTable();
            datatable = getavtable(type);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = OutTable(datatable, ref viscol);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 2);
            }           
        }
        //弹性系数
        private void build_CoefficientOfElasticity()
        {
             Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "弹性系数";
            int type = 4;
            System.Data.DataTable datatable = new System.Data.DataTable();
            datatable = getavtable(type);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = OutTable(datatable, ref viscol);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 3);
            }           
        }
        //相关法
        private void build_CorrelationMethod()
        {
               Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "相关法";
            int type = 3;
            System.Data.DataTable datatable = new System.Data.DataTable();
            datatable = getavtable(type);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = OutTable(datatable, ref viscol);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 4);
            }           
        }
        //灰色模型
      private void build_GrayModel()
      {
             Dictionary<string, Columqk> viscol = new Dictionary<string, Columqk>();
            string title = "灰色模型";
            int type = 6;
            System.Data.DataTable datatable = new System.Data.DataTable();
            datatable = getavtable(type);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = OutTable(datatable, ref viscol);
            if (!shjjbyyear)
            {
                creatsheetbydt(title, dt, viscol);
            }

            else
            {
                recreatsheetbydt(title, dt, viscol, 5);
            }           
      }
        //
        //获得datatable
        private System.Data.DataTable getavtable(int type)
        {
            Ps_Forecast_Math psp_Type = new Ps_Forecast_Math();
            psp_Type.ForecastID = forecastReport.ID;
            psp_Type.Forecast = type;
            IList listTypes = Common.Services.BaseService.GetList("SelectPs_Forecast_MathByForecastIDAndForecast", psp_Type);

          System.Data.DataTable  dataTable = Itop.Common.DataConverter.ToDataTable(listTypes, typeof(Ps_Forecast_Math));
          return dataTable;

        }
        //形成输出的datatable
        private System.Data.DataTable OutTable( System.Data.DataTable datatable,ref Dictionary<string, Columqk> viscol)
        {
          System.Data.DataTable dt = new  System.Data.DataTable();
            List<string> listColID = new List<string>();

            listColID.Add("Title");
            dt.Columns.Add("Title", typeof(string));
            dt.Columns["Title"].Caption = "项目";
            viscol["Title"] = new Columqk("项目", 3, 2);
            dt.Columns.Add("ParentID", typeof(string));

            foreach (DataColumn column in datatable.Columns)
            {
                if (column.ColumnName.IndexOf("y") >= 0)
                {
                    int year=Convert.ToInt32(column.ColumnName.Substring(1));
                    
                     if (year >= forecastReport.StartYear && year <= forecastReport.EndYear)
                    {
                        listColID.Add(column.ColumnName);
                        viscol[column.ColumnName] = new Columqk(year + "年", 2, 2);
                        dt.Columns.Add(column.ColumnName, typeof(double));
                    }
                   
                }
                //else
                //    if (column.FieldName == "ParentID")
                //    {
                //        dt.Columns.Add("ParentID", typeof(string));
                //        listColID.Add("ParentID");
                //        dt.Columns["ParentID"].Caption = "父ID";
                //    }
            }
            listColID.Add("ParentID");
            dt.Columns["ParentID"].Caption = "父ID";

            int itemp = -4;
            int jtemp = -4;
            foreach (DataRow node in datatable.Rows)
            {
                jtemp = itemp;
                AddNodeDataToDataTable(dt,datatable, node, listColID, ref itemp, jtemp);
                // itemp++;
            }
            return dt;
        }

        private static void AddNodeDataToDataTable(System.Data.DataTable dt, System.Data.DataTable datasource, DataRow node, List<string> listColID, ref int i, int j)
        {
            DataRow newRow = dt.NewRow();
            DataRow[] dv = datasource.Select("ParentID='" + node["ID"] + "'");
            foreach (string colID in listColID)
            {
               
                //分类名，第二层及以后在前面加空格
                if (colID == "Title" && dv.Length>0)
                {
                    newRow[colID] = "　　" + node[colID];
                }
                else if (colID == "ParentID" && dv.Length>0)
                {
                    newRow[colID] = j;
                }
                else
                {
                    newRow[colID] = node[colID];
                }
            }

            ////根分类结束后加空行

            //if (node.ParentNode == null && dt.Rows.Count > 0)
            //{
            //    dt.Rows.Add(new object[] { });
            //}

            dt.Rows.Add(newRow);
            j = i;
            i--;
            dv = datasource.Select("ID='" + node["ParentID"] + "'");
            foreach (DataRow nd in dv)
            {


                AddNodeDataToDataTable(dt,datasource, nd, listColID, ref  i, j);
                // i++;
            }

        }
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
            //年增长率法
            build_ArverageIncreasing();
             //外推法
            build_Extrapolation();
             //指数平滑
            build_ExponentialSmoothing();
             //弹性系数
           build_CoefficientOfElasticity();
              //相关法       
           build_CorrelationMethod();
             //灰色模型
           build_GrayModel();

 
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
            fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\fhycztj.xls");
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
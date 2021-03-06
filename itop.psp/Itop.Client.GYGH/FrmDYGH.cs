using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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
using FarPoint.Win;
using Itop.Domain.Forecast;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace Itop.Client.GYGH
{
    public partial class FrmDYGH : FormBase
    {

        //创建表对象
        DYGH.Sheet51 sh51 = new Itop.Client.GYGH.DYGH.Sheet51();
        DYGH.Sheet51_32 sh51_32 = new Itop.Client.GYGH.DYGH.Sheet51_32();
        DYGH.Sheet52 sh52 = new Itop.Client.GYGH.DYGH.Sheet52();

        //工程号
        string ProjID = Itop.Client.MIS.ProgUID;

        //用自定义类创建对象
        fpcommon fc = new fpcommon();
        //存放市县固定名称编号及相应值
        List<string[]> SxXjName = new List<string[]>();
      
        private void Add_SxXJname()
        {
            //初始化市县地区名编号等
            string[] str1 ={ "1", "市辖供电区", "市辖供电区" };
            string[] str2 ={ "2", "县级供电区", "合计" };
            string[] str3 ={ "2.1", "其中：直供直管", "县级直供直管" };
            string[] str4 ={ "2.2", "控股", "县级控股" };
            string[] str5 ={ "2.3", "参股", "县级参股" };
            string[] str6 ={ "2.4", "代管", "县级代管" };
            string[] str7 ={ "3", "全地区", "合计" };
            SxXjName.Add(str1);
            SxXjName.Add(str2);
            SxXjName.Add(str3);
            SxXjName.Add(str4);
            SxXjName.Add(str5);
            SxXjName.Add(str6);
            SxXjName.Add(str7);
        }

        public FrmDYGH()
        {
            InitializeComponent();
        }

        private void FrmPDWXZ_Load(object sender, EventArgs e)
        {
            Add_SxXJname();
          
            //根据窗口变化全部添满
            fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            fpSpread_addsheet();
            //Firstadddata();
        }
        private void fpSpread_addsheet()
        {
            WaitDialogForm wait = null;
            wait = new WaitDialogForm("", "正在加载数据, 请稍候...");
            try
            {
                //打开Excel表格
                //清空工作表
                fpSpread1.Sheets.Clear();
                fpSpread1.OpenExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\DYGH.xls");
                fc.SpreadRemoveEmptyCells(fpSpread1);
               
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
                //保存excel文件
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\DYGH.xls");
                //以下是打开文件设表格自动换行

                // 定义要使用的Excel 组件接口
                // 定义Application 对象,此对象表示整个Excel 程序
                Microsoft.Office.Interop.Excel.Application excelApp = null;
                // 定义Workbook对象,此对象代表工作薄
                Microsoft.Office.Interop.Excel.Workbook workBook;
                // 定义Worksheet 对象,此对象表示Execel 中的一张工作表
                Microsoft.Office.Interop.Excel.Worksheet ws = null;
                Microsoft.Office.Interop.Excel.Range range = null;
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                string filename = System.Windows.Forms.Application.StartupPath + "\\xls\\DYGH.xls";
                workBook = excelApp.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
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
                    //保护工作表
                    ws.Protect(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                }
                //保存工作簿
                workBook.Save();
                //关闭工作簿
                excelApp.Workbooks.Close();

            }
            wait.Close();

        }
        private void Firstadddata()
        {
            //生成表5-1
            FarPoint.Win.Spread.SheetView Sheet51 = new FarPoint.Win.Spread.SheetView();
            //生成表5-1附表32
            FarPoint.Win.Spread.SheetView Sheet51_32 = new FarPoint.Win.Spread.SheetView();
            //生成表5-2
            FarPoint.Win.Spread.SheetView Sheet52 = new FarPoint.Win.Spread.SheetView();


            //添加表5-1
            fpSpread1.Sheets.Add(Sheet51);
            //添加表5-1附表32
            fpSpread1.Sheets.Add(Sheet51_32);
            //添加5-2
            fpSpread1.Sheets.Add(Sheet52);


            //创建表5-1
            sh51.Build(Sheet51,ProjID, SxXjName);
            //创建表5-1附表32
            sh51_32.Build(Sheet51_32, ProjID);
            //创建表5-2
            sh52.Build(Sheet52, ProjID, SxXjName);
             
       

           fc.Sheet_Colautoenter(fpSpread1);
        }
        private void barBtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void barBtnDaochuExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                    //以下是打开文件设表格自动换行

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
        private void barBtnRefreshData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
          
            WaitDialogForm newwait = new WaitDialogForm("", "正在更新数据, 请稍候...");
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


            //更新表5-1
            fpSpread1.Sheets[0].RowCount = 0;
            fpSpread1.Sheets[0].ColumnCount = 0;
            sh51.Build(fpSpread1.Sheets[0], ProjID, SxXjName);
            //更新表5-1附表32
            fpSpread1.Sheets[1].RowCount = 0;
            fpSpread1.Sheets[1].ColumnCount = 0;
            sh51_32.Build(fpSpread1.Sheets[1], ProjID);
            //更新表5-2
            fpSpread1.Sheets[2].RowCount = 0;
            fpSpread1.Sheets[2].ColumnCount = 0;
            sh52.Build(fpSpread1.Sheets[2], ProjID, SxXjName);

            //移除空表
            fpSpread1.Sheets.Remove(activesheet);
            //还原当前表
            fpSpread1.ActiveSheet = obj_sheet;
            //设文本自动换行
            fc.Sheet_Colautoenter(fpSpread1);
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
            try
            {
                //保存excel文件
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\DYGH.xls");
                //以下是打开文件设表格自动换行

                // 定义要使用的Excel 组件接口
                // 定义Application 对象,此对象表示整个Excel 程序
                Microsoft.Office.Interop.Excel.Application excelApp = null;
                // 定义Workbook对象,此对象代表工作薄
                Microsoft.Office.Interop.Excel.Workbook workBook;
                // 定义Worksheet 对象,此对象表示Execel 中的一张工作表
                Microsoft.Office.Interop.Excel.Worksheet ws = null;
                Microsoft.Office.Interop.Excel.Range range = null;
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                string filename = System.Windows.Forms.Application.StartupPath + "\\xls\\DYGH.xls";
                workBook = excelApp.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
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
                    //保护工作表
                    ws.Protect(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                }
                //保存工作簿
                workBook.Save();
                //关闭工作簿
                excelApp.Workbooks.Close();
                wait.Close();
                MsgBox.Show("保存成功");
            }
            catch (System.Exception ee)
            {
                wait.Close();
                MsgBox.Show("保存错误！确定您安装有Office Excel,或者关闭所有Excel文件重试");
            }
            

        }

    }
}
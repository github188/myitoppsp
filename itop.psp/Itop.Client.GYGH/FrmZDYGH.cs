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

namespace Itop.Client.GYGH
{
    public partial class FrmZDYGH : FormBase
    {

        //工程号
        string ProjID = Itop.Client.MIS.ProgUID;
        //记录县级供电区合计部分开始行号，以便复制数据时使用
        int XJDQHEJI8346Row = 0;
        int XJDQHEJI8750Row = 0;
        public int flag83 = 0;
        public int flag87 = 0;
        private class SaveData8346
        {
            //用于存放表8-3附表46中的数据
            public string dq = "";
            public string AreaName = "";
            public object[,] data=new object[4,6];
          
            public SaveData8346()
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        data[i, j] =null;
                    }
                }
            }
            
        }
        //在更新时存放8-3附表46中的用户手写数据
        List<SaveData8346> templist8346=new List<SaveData8346>() ;
        //在更新时存放8-7附表50中的用户手写数据（借用8346的类）
        List<SaveData8346> templist8750 = new List<SaveData8346>();
        //用自定义类创建对象
        fpcommon fc=new fpcommon() ;
        //工作表行数
        int rowcount = 0;
        //工作表列数据
        int colcount = 0;
        //工作表第一行的表名
        string title = "";
        //工作表标签名
        string sheetname = "";
       
        public FrmZDYGH()
        {
            InitializeComponent();
        }

        private void FrmZDYGH_Load(object sender, EventArgs e)
        {
            //根据窗口变化全部添满
            fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            fpSpread_addsheet();
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
                fpSpread1.OpenExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\ZDYGH.xls");
                fc.SpreadRemoveEmptyCells(fpSpread1);
                //生成动态事件
                fpSpread1.SheetTabClick += new FarPoint.Win.Spread.SheetTabClickEventHandler(fpSpread1_SheetTabClick);

                //设定表8-3中的手工输入部分的格式
                Sheet83_SetCellType(fpSpread1.Sheets[3]);
                fpSpread1.Sheets[3].CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(FrmZDYGH_83CellChanged);
                //设定表8-3附表46中的手工输入部分的格式
                Sheet83_46SetCellType(fpSpread1.Sheets[4]);
                //查找并重设县级供电区合计部分的行号,以便复制数据
                XJDQHEJI8346Row = fc.Sheet_Find_Value(fpSpread1.Sheets[4], 0, "县级供电区");
                //设定表8-7中的手工输入部分的格式
                Sheet87_SetCellType(fpSpread1.Sheets[11]);
                fpSpread1.Sheets[11].CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(FrmZDYGH_87CellChanged);
                //设定表8-7附表50中的手工输入部分的格式
                Sheet87_50SetCellType(fpSpread1.Sheets[12]);
                XJDQHEJI8750Row = fc.Sheet_Find_Value(fpSpread1.Sheets[12], 0, "县级供电区");
                //设定表8-9中的手工输入部分的格式
                Sheet89_SetCellType(fpSpread1.Sheets[15]);
                //设定表8-10中的手工输入部分的格式
                Sheet810_SetCellType(fpSpread1.Sheets[16]);
                //设定表8-11中的手工输入部分的格式
                Sheet811_SetCellType(fpSpread1.Sheets[17]);

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
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\ZDYGH.xls");
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
                string filename = System.Windows.Forms.Application.StartupPath + "\\xls\\ZDYGH.xls";
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
            //生成表8-1
            FarPoint.Win.Spread.SheetView Sheet81 = new FarPoint.Win.Spread.SheetView();
            //生成表8-2
            FarPoint.Win.Spread.SheetView Sheet82 = new FarPoint.Win.Spread.SheetView();
            //生成表8-2附表45
            FarPoint.Win.Spread.SheetView Sheet82_45 = new FarPoint.Win.Spread.SheetView();
            //生成表8-3
            FarPoint.Win.Spread.SheetView Sheet83 = new FarPoint.Win.Spread.SheetView();
            //生成表8-3附表46
            FarPoint.Win.Spread.SheetView Sheet83_46 = new FarPoint.Win.Spread.SheetView();
            //生成表8-4
            FarPoint.Win.Spread.SheetView Sheet84 = new FarPoint.Win.Spread.SheetView();
            //生成表8-4附表47
            FarPoint.Win.Spread.SheetView Sheet84_47 = new FarPoint.Win.Spread.SheetView();
            //生成表8-5
            FarPoint.Win.Spread.SheetView Sheet85 = new FarPoint.Win.Spread.SheetView();
            //生成表8-5附表48
            FarPoint.Win.Spread.SheetView Sheet85_48 = new FarPoint.Win.Spread.SheetView();
            //生成表8-6
            FarPoint.Win.Spread.SheetView Sheet86 = new FarPoint.Win.Spread.SheetView();
            //生成表8-6附表49
            FarPoint.Win.Spread.SheetView Sheet86_49 = new FarPoint.Win.Spread.SheetView();
            //生成表8-7
            FarPoint.Win.Spread.SheetView Sheet87 = new FarPoint.Win.Spread.SheetView();
            //生成表8-7附表50
            FarPoint.Win.Spread.SheetView Sheet87_50 = new FarPoint.Win.Spread.SheetView();
            //生成表8-8
            FarPoint.Win.Spread.SheetView Sheet88 = new FarPoint.Win.Spread.SheetView();
            //生成表8-8附表51
            FarPoint.Win.Spread.SheetView Sheet88_51 = new FarPoint.Win.Spread.SheetView();
            //生成表8-9
            FarPoint.Win.Spread.SheetView Sheet89 = new FarPoint.Win.Spread.SheetView();
            //生成表8-10
            FarPoint.Win.Spread.SheetView Sheet810 = new FarPoint.Win.Spread.SheetView();
            //生成表8-11
            FarPoint.Win.Spread.SheetView Sheet811 = new FarPoint.Win.Spread.SheetView();


            //创建表8-1
            Build_Sheet81(Sheet81);
            //创建表8-2
            Build_Sheet82(Sheet82);
            //创建表8-2附表45
            Build_Sheet82_45(Sheet82_45);
            //创建表8-3
            Build_Sheet83(Sheet83);
            //创建表8-3附表46
            Build_Sheet83_46(Sheet83_46);
            //创建表8-4
            Build_Sheet84(Sheet84);
            //创建表8-4附表47
            Build_Sheet84_47(Sheet84_47);
            //创建表8-5
            Build_Sheet85(Sheet85);
            //创建表8-5附表48
            Build_Sheet85_48(Sheet85_48);
            //创建表8-6
            Build_Sheet86(Sheet86);
            //创建表8-6附表49
            Build_Sheet86_49(Sheet86_49);
            //创建表8-7
            Build_Sheet87(Sheet87);
            //创建表8-7附表50
            Build_Sheet87_50(Sheet87_50);
            //创建表8-8
            Build_Sheet88(Sheet88);
            //创建表8-6附表49
            Build_Sheet88_51(Sheet88_51);
            //创建表8-9
            Build_Sheet89(Sheet89);
            //创建表8-10
            Build_Sheet810(Sheet810);
            //创建表8-11
            Build_Sheet811(Sheet811);

            //添加表8-1
            fpSpread1.Sheets.Add(Sheet81);
            //添加表8-2
            fpSpread1.Sheets.Add(Sheet82);
            //添加8-2附表45
            fpSpread1.Sheets.Add(Sheet82_45);
            //添加表8-3
            fpSpread1.Sheets.Add(Sheet83);
            //
            //动态方法
            //
            fpSpread1.Sheets[3].CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(FrmZDYGH_83CellChanged);

            //添加表8-46
            fpSpread1.Sheets.Add(Sheet83_46);
            //添加表8-4
            fpSpread1.Sheets.Add(Sheet84);
            //添加表8-4附表47
            fpSpread1.Sheets.Add(Sheet84_47);
            //添加表8-5
            fpSpread1.Sheets.Add(Sheet85);
            //添加表8-5附表48
            fpSpread1.Sheets.Add(Sheet85_48);
            //添加表8-6
            fpSpread1.Sheets.Add(Sheet86);
            //添加表8-6附表49
            fpSpread1.Sheets.Add(Sheet86_49);
            //添加表8-7
            fpSpread1.Sheets.Add(Sheet87);
            fpSpread1.Sheets[11].CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(FrmZDYGH_87CellChanged);
            //添加表8-7附表50
            fpSpread1.Sheets.Add(Sheet87_50);
            //添加表8-8
            fpSpread1.Sheets.Add(Sheet88);
            //添加表8-8附表51
            fpSpread1.Sheets.Add(Sheet88_51);
            //添加表8-9
            fpSpread1.Sheets.Add(Sheet89);
            //添加表8-10
            fpSpread1.Sheets.Add(Sheet810);
            //添加表8-11
            fpSpread1.Sheets.Add(Sheet811);

            fc.Sheet_Colautoenter(fpSpread1);
            fpSpread1.SheetTabClick += new FarPoint.Win.Spread.SheetTabClickEventHandler(fpSpread1_SheetTabClick);


        }
        //动态事件用来更新表8-3,8-7,8-11的数据（从8-3,8-7附表获取）
        void fpSpread1_SheetTabClick(object sender, FarPoint.Win.Spread.SheetTabClickEventArgs e)
        {
            if (e.SheetTabIndex==3)
            {
                Sheet83_GetDatafrom8346(fpSpread1.Sheets[3]);
                return;
            }
            if (e.SheetTabIndex == 11)
            {
                Sheet87_GetDatafrom8750(fpSpread1.Sheets[11]);
                return;
            }
            if (e.SheetTabIndex==17)
            {
                Sheet811_GetDatafrom8346(fpSpread1.Sheets[17]);
                return;
            }

        }

        //动态事件用来判断8-3中分项的和值是否与总计相符合
        void FrmZDYGH_83CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
            if (flag83 == 1)
            {
                flag83 = 0;
                return;
            }
            if (9 < e.Row && e.Row < 26 && 4 < e.Column && e.Column < 11)
            {

                double a = 0;
                double b = 0;
                double c = 0;
                double d = 0;
                double sum = 0;
                int row = (e.Row - 10) % 4;

               

                if (fpSpread1.Sheets[3].Cells[10 + row - 4, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[3].Cells[10 + row - 4, e.Column].Value.ToString(), out  sum);
                }

                if (fpSpread1.Sheets[3].Cells[10 + row + 0, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[3].Cells[10 + row + 0, e.Column].Value.ToString(), out a);
                }
                if (fpSpread1.Sheets[3].Cells[10 + row + 4, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[3].Cells[10 + row + 4, e.Column].Value.ToString(), out b);
                }
                if (fpSpread1.Sheets[3].Cells[10 + row + 8, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[3].Cells[10 + row + 8, e.Column].Value.ToString(), out c);
                }
                if (fpSpread1.Sheets[3].Cells[10 + row + 12, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[3].Cells[10 + row + 12, e.Column].Value.ToString(), out d);
                }


                if (a + b + c + d == sum)
                {
                    flag83 = 1;
                    fpSpread1.Sheets[3].Cells[10 + row - 4, e.Column].BackColor = Color.White;

                }
                else
                {
                    flag83 = 1;
                    fpSpread1.Sheets[3].Cells[10 + row - 4, e.Column].BackColor = Color.Red;

                }
            }

        }
        //动态事件用来判断8-7中分项的和值是否与总计相符合
        void FrmZDYGH_87CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
            if (flag83 == 1)
            {
                flag83 = 0;
                return;
            }
            if (5 < e.Row && e.Row < 14 && 3 < e.Column && e.Column < 10)
            {

                double a = 0;
                double b = 0;
                double c = 0;
                double d = 0;
                double sum = 0;
                int row = (e.Row - 6) % 2;

                if (fpSpread1.Sheets[11].Cells[4 + row, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[11].Cells[4 + row, e.Column].Value.ToString(),out sum);
                }

                if (fpSpread1.Sheets[11].Cells[6 + row, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[11].Cells[6 + row, e.Column].Value.ToString(),out a);
                }
                if (fpSpread1.Sheets[11].Cells[8 + row, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[11].Cells[8 + row, e.Column].Value.ToString(),out b);
                }
                if (fpSpread1.Sheets[11].Cells[10 + row, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[11].Cells[10 + row, e.Column].Value.ToString(),out c);
                }
                if (fpSpread1.Sheets[11].Cells[12 + row, e.Column].Value != null)
                {
                    double.TryParse(fpSpread1.Sheets[11].Cells[12 + row, e.Column].Value.ToString(),out d);
                }

                if (a + b + c + d == sum)
                {
                    flag87 = 1;
                    fpSpread1.Sheets[11].Cells[4 + row, e.Column].BackColor= Color.White;

                }
                else
                {
                    flag87 = 1;
                    fpSpread1.Sheets[11].Cells[4 + row, e.Column].BackColor = Color.Red;

                }
            }

               
        }
        private void Build_Sheet81(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //16行11列
            rowcount = 16;
            colcount = 11;
            title = "表8-1  铜陵市中压配变新建工程量";
            sheetname = "表8-1";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[2].Width = 100;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "电压等级（kV）");
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;
            int j = 0;
            int length = 2;
            //写入市辖供电区部分
            Sheet81_AddItems(obj_sheet, startrow + j * length, "市辖供电区", "1");
            Sheet81_AddData(obj_sheet, startrow + j * length, "市辖供电区");
            j++;
            //写入县级供电区部分
            Sheet81_AddItems(obj_sheet, startrow + j * length, "县级供电区", "2");
            j++;
            //写入直供直管部分
            Sheet81_AddItems(obj_sheet, startrow + j * length, "其中：直供直管", "2.1");
            Sheet81_AddData(obj_sheet, startrow + j * length, "县级直供直管");
            j++;
            //写入县级控股部分
            Sheet81_AddItems(obj_sheet, startrow + j * length, "控股", "2.2");
            Sheet81_AddData(obj_sheet, startrow + j * length, "县级控股");
            j++;
            //写入县级参股部分
            Sheet81_AddItems(obj_sheet, startrow + j * length, "参股", "2.3");
            Sheet81_AddData(obj_sheet, startrow + j * length, "县级参股");
            j++;
            //写入县级代管部分
            Sheet81_AddItems(obj_sheet, startrow + j * length, "代管", "2.4");
            Sheet81_AddData(obj_sheet, startrow + j * length, "县级代管");
            j++;
            //写入全地区合计部分
            Sheet81_AddItems(obj_sheet, startrow + j * length, "全地区合计", "3");

            //写入计算县级供电区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 6, 4, 4, 2, 4, 4, 6);
            //写入计算全地区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 4, 2, 2, 14, 4, 6);
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 14, 5, 10);
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet81_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname, string bianhao)
        {
            //添加表8-1的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 3, "台数（台）");
            obj_sheet.SetValue(row + 1, 3, "容量（MVA）");
            obj_sheet.AddSpanCell(row, 2, 2, 1);
            obj_sheet.SetValue(row, 2, "10");
            obj_sheet.AddSpanCell(row, 1, 2, 1);
            obj_sheet.SetValue(row, 1, itemname);
            obj_sheet.AddSpanCell(row, 0, 2, 1);
            obj_sheet.SetValue(row, 0, bianhao);
        }
        private void Sheet81_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 4; i < 10; i++)
            {
                string tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and DQ='" + DQ + "' and Col3='新建' and Col4='pw-pb' and BianInfo like '10@%'";
                //配电室台数
                double PDnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                {
                    PDnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                }
                //箱式变台数
                double XSnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                {
                    XSnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                }
                //柱上变台数
                double ZSnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                {
                    ZSnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                }
                //写入台数，是上述三个之和
                obj_sheet.SetValue(row, i, PDnum1 + XSnum1 + ZSnum1);

                //配电室容量
                double PDRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) != null)
                {
                    PDRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                }
                //箱式变容量
                double XSRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian) != null)
                {
                    XSRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian);
                }
                //柱上变容量
                double ZSRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojian) != null)
                {
                    ZSRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojian);
                }
                //写入容量，是上述三个之和
                obj_sheet.SetValue(row + 1, i, PDRLnum1 + XSRLnum1           + ZSRLnum1);
            }
        }
        private void Build_Sheet82(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //44行12列
            rowcount = 44;
            colcount = 12;
            title = "表8-2 铜陵市中压配电室新建规模";
            sheetname = "表8-2";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[11].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 1, 2);
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 5; i < 11; i++)
            {
                obj_sheet.SetValue(1, i, i + 2005 + "年");
            }
            obj_sheet.SetValue(1, 11, "“十二五”合计");
            int startrow         = 2;
            int j         = 0;
            //写入市辖供电区部分
            Sheet82_AddItems(obj_sheet, startrow + j * 6, "市辖供电区", "1");
            Sheet82_AddData(obj_sheet, startrow + j * 6, "市辖供电区");
            j++;
            //写入县级供电区部分
            Sheet82_AddItems(obj_sheet, startrow + j * 6, "县级供电区", "2");
            j++;
            //写入直供直管部分
            Sheet82_AddItems(obj_sheet, startrow + j * 6, "其中：直供直管", "2.1");
            Sheet82_AddData(obj_sheet, startrow + j * 6, "县级直供直管");
            j++;
            //写入县级控股部分
            Sheet82_AddItems(obj_sheet, startrow + j * 6, "控股", "2.2");
            Sheet82_AddData(obj_sheet, startrow + j * 6, "县级控股");
            j++;
            //写入县级参股部分
            Sheet82_AddItems(obj_sheet, startrow + j * 6, "参股", "2.3");
            Sheet82_AddData(obj_sheet, startrow + j * 6, "县级参股");
            j++;
            //写入县级代管部分
            Sheet82_AddItems(obj_sheet, startrow + j * 6, "代管", "2.4");
            Sheet82_AddData(obj_sheet, startrow + j * 6, "县级代管");
            j++;
            //写入全地区合计部分
            Sheet82_AddItems(obj_sheet, startrow + j * 6, "全地区合计", "3");

            //写入计算县级供电区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 14, 5, 4, 6, 8, 5, 6);
            //写入计算全地区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 5, 2, 6, 38, 5, 6);
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 6, 42, 5, 11);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet82_AddItems(FarPoint.Win.Spread.SheetView obj_sheet,int row, string itemname, string bianhao)
        {
            //添加表8-2的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 4, "台数（台）");
            obj_sheet.SetValue(row + 1, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row, 3, 2, 1);
            obj_sheet.SetValue(row, 3, "配电室");

            obj_sheet.SetValue(row + 2, 4, "台数（台）");
            obj_sheet.SetValue(row + 3, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row + 2, 3, 2, 1);
            obj_sheet.SetValue(row + 2, 3, "箱式变");

            obj_sheet.SetValue(row + 4, 4, "台数（台）");
            obj_sheet.SetValue(row + 5, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row + 4, 3, 2, 1);
            obj_sheet.SetValue(row + 4, 3, "柱上变");

            obj_sheet.AddSpanCell(row, 2, 6, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 6, 1);
            obj_sheet.SetValue(row, 1, itemname);
            obj_sheet.AddSpanCell(row, 0, 6, 1);
            obj_sheet.SetValue(row, 0, bianhao);
        }
        private void Sheet82_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 5; i < 11; i++)
            {
                string tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and DQ='" + DQ + "' and Col3='新建' and Col4='pw-pb' and BianInfo like '10@%'";
                //写入配电室台数
                double PDnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian)!= null)
                {
                    PDnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                }
                obj_sheet.SetValue(row, i, PDnum1);
                //写入配电室容量
                double PDRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) != null)
                {
                    PDRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                }
                obj_sheet.SetValue(row + 1, i, PDRLnum1);

                //写入箱式变台数
                double XSnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                {
                    XSnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                }
                obj_sheet.SetValue(row + 2, i, XSnum1);
                //写入箱式变容量
                double XSRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian) != null)
                {
                    XSRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian);
                }
                obj_sheet.SetValue(row + 3, i, XSRLnum1);

                //写入柱上变台数
                double ZSnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                {
                    ZSnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                }
                obj_sheet.SetValue(row + 4, i, ZSnum1);
                //写入柱上变容量
                double ZSRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojian) != null)
                {
                    ZSRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojian);
                }
                obj_sheet.SetValue(row + 5, i, ZSRLnum1);
            }
        }
        private void Build_Sheet82_45(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //查询AreaName的市辖供电区条件
            string SXtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ='市辖供电区' " + " and Col3='新建' and Col4='pw-pb' and BianInfo like '10@%' Group by AreaName";
            //查询AreaName的县级供电区条件
            string XJtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ!='市辖供电区' " + " and Col3='新建' and Col4='pw-pb' and BianInfo like '10@%' Group by AreaName";
            IList SXarea = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            IList XJarea = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录市辖分区数
            int SXsum = SXarea.Count;
            //记录县级分区数
            int XJsum = XJarea.Count;
            //12列
            //每增加一个分区相应的增加6行
            rowcount = 2 + (SXsum + XJsum + 2) * 6;
            colcount = 12;
            title = "附表45  铜陵市中压配电室新建规模统计";
            sheetname = "表8-2 附表45 ";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 80;
            obj_sheet.Columns[1].Width = 85;
            obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[11].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 1, 2);
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 5; i < 11; i++)
            {
                obj_sheet.SetValue(1, i, i + 2005 + "年");
            }
            obj_sheet.SetValue(1, 11, "“十二五”合计");
            int startrow = 2;
            //写入市辖供电区合计部分
            Sheet82_45AddItems(obj_sheet, startrow,"合计");
            if (SXsum != 0)
            {
                //循环写入市辖供电区分区名和相应的项目部分
                for (int m     = 0; m < SXsum;m++)
                {
                    Sheet82_45AddItems(obj_sheet, startrow + (m + 1) * 6, SXarea[m].ToString());
                    Sheet82_45AddData(obj_sheet, startrow + (m + 1) * 6, SXarea[m].ToString(), "市辖供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1 * 6, 5, SXsum, 6, 2, 5, 6);
            }
            else
            {
                //直接在合计部分写入0值
               fc.Sheet_WriteZero(obj_sheet,2,5,6,6);
            }
            //合并市辖部分，写入分区类型
            obj_sheet.AddSpanCell(2, 0, (1 + SXsum) * 6, 1);
            obj_sheet.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计部分
            Sheet82_45AddItems(obj_sheet, startrow + (1 + SXsum) * 6, "合计");
            if (XJsum != 0)
            {
                //循环写入县级供电区分区名和相应的项目部分
                for (int m     = 0; m < XJsum; m++)
                {
                    Sheet82_45AddItems(obj_sheet, startrow + (SXsum +         2) * 6, XJarea[m].ToString());
                    Sheet82_45AddData(obj_sheet, startrow + (SXsum + m + 2) * 6, XJarea[m].ToString(), "县级供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (SXsum + 2) * 6, 5, XJsum, 6, startrow + (1 + SXsum) * 6, 5, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, startrow      +   ( 1  +  SXsum) * 6, 5, 6, 6);
            }
            //合并县级部分，写入分区类型
            obj_sheet.AddSpanCell(startrow      +   ( 1  +  SXsum) * 6, 0, (1 + XJsum) * 6, 1);
            obj_sheet.SetValue(startrow + (1 + SXsum) * 6, 0, "县级供电区");
            //写入十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 6, (2 + XJsum + SXsum) * 6, 5, 11);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet82_45AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname)
        {
            //添加表8-2附45的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 4, "台数（台）");
            obj_sheet.SetValue(row + 1, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row, 3, 2, 1);
            obj_sheet.SetValue(row, 3, "配电室");

            obj_sheet.SetValue(row + 2, 4, "台数（台）");
            obj_sheet.SetValue(row + 3, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row + 2, 3, 2, 1);
            obj_sheet.SetValue(row + 2, 3, "箱式变");

            obj_sheet.SetValue(row + 4, 4, "台数（台）");
            obj_sheet.SetValue(row + 5, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row + 4, 3, 2, 1);
            obj_sheet.SetValue(row + 4, 3, "柱上变");

            obj_sheet.AddSpanCell(row, 2, 6, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 6, 1);
            obj_sheet.SetValue(row, 1, itemname);
        }
        private void Sheet82_45AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string areaname,   string DQ)
        {
            //用年份部分列数做外循环控制
            string tiaojian = "";
            for (int i = 5; i < 11; i++)
            {
                if (DQ == "市辖供电区")
                {
                    tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and DQ='市辖供电区' and AreaName='" + areaname + "' and Col3='新建' and Col4='pw-pb' and BianInfo like '10@%'";
                }
                else
                {
                    tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and DQ!='市辖供电区' and AreaName='" + areaname + "' and Col3='新建' and Col4='pw-pb' and BianInfo like '10@%'";
                }
                //写入配电室台数
                double PDnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                {
                    PDnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                }
                obj_sheet.SetValue(row, i, PDnum1);
                //写入配电室容量
                double PDRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) != null)
                {
                    PDRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                }
                obj_sheet.SetValue(row + 1, i, PDRLnum1);

                //写入箱式变台数
                double XSnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                {
                    XSnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                }
                obj_sheet.SetValue(row + 2, i, XSnum1);
                //写入箱式变容量
                double XSRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian) != null)
                {
                    XSRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian);
                }
                obj_sheet.SetValue(row + 3, i, XSRLnum1);

                //写入柱上变台数
                double ZSnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                {
                    ZSnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                }
                obj_sheet.SetValue(row + 4, i, ZSnum1);
                //写入柱上变容量
                double ZSRLnum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojian) != null)
                {
                    ZSRLnum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojian);
                }
                obj_sheet.SetValue(row + 5, i, ZSRLnum1);
            }
        }
        private void Build_Sheet83(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //30行12列
            rowcount = 30;
            colcount = 12;
            title = "表8-3  铜陵市中压配变设备改造规模";
            sheetname = "表8-3";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[3].Width = 65;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[11].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 1, 2);
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 5; i < 11; i++)
            {
                obj_sheet.SetValue(1, i, i + 2005 + "年");
            }
            obj_sheet.SetValue(1, 11, "“十二五”合计");
            int startrow = 2;
            int j = 0;
            int length = 4;
            //写入市辖供电区部分
            Sheet83_AddItems(obj_sheet, startrow + j * length, "市辖供电区", "1");

            j++;
            //写入县级供电区部分
            Sheet83_AddItems(obj_sheet, startrow + j * length, "县级供电区", "2");
            j++;
            //写入直供直管部分
            Sheet83_AddItems(obj_sheet, startrow + j * length, "其中：直供直管", "2.1");

            j++;
            //写入县级控股部分
            Sheet83_AddItems(obj_sheet, startrow + j * length, "控股", "2.2");

            j++;
            //写入县级参股部分
            Sheet83_AddItems(obj_sheet, startrow + j * length, "参股", "2.3");
            j++;
            //写入县级代管部分
            Sheet83_AddItems(obj_sheet, startrow + j * length, "代管", "2.4");
            j++;
            //写入全地区合计部分
            Sheet83_AddItems(obj_sheet, startrow + j * length, "全地区合计", "3");

            //计算全地区合计

            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 5, 2, 4, 26, 5, 6);
            //计算十二五合计

            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 6, 28, 5, 11);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            Sheet83_SetCellType(obj_sheet);

        }
        private void Sheet83_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname, string bianhao)
        {
            //添加表8-3的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 4, "台数（台）");
            obj_sheet.SetValue(row + 1, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row, 3, 2, 1);
            obj_sheet.SetValue(row, 3, "高损配变");

            obj_sheet.SetValue(row + 2, 4, "组数（组）");
            obj_sheet.SetValue(row + 3, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row + 2, 3, 2, 1);
            obj_sheet.SetValue(row + 2, 3, "无功补偿");

            obj_sheet.AddSpanCell(row, 2, 4, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 4, 1);
            obj_sheet.SetValue(row, 1, itemname);
            obj_sheet.AddSpanCell(row, 0, 4, 1);
            obj_sheet.SetValue(row, 0, bianhao);
        }
        private void Sheet83_GetDatafrom8346(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //从8-3附表46中获取添入表8-3
            for (int col = 5; col < 11; col++)
            {
                //市辖部分合计
                obj_sheet.SetValue(2, col, fpSpread1.Sheets[4].Cells[2, col].Value);
                obj_sheet.SetValue(3, col, fpSpread1.Sheets[4].Cells[3, col].Value);
                obj_sheet.SetValue(4, col, fpSpread1.Sheets[4].Cells[4, col].Value);
                obj_sheet.SetValue(5, col, fpSpread1.Sheets[4].Cells[5, col].Value);
                //县级部分合计
                obj_sheet.SetValue(6, col, fpSpread1.Sheets[4].Cells[XJDQHEJI8346Row, col].Value);
                obj_sheet.SetValue(7, col, fpSpread1.Sheets[4].Cells[XJDQHEJI8346Row + 1, col].Value);
                obj_sheet.SetValue(8, col, fpSpread1.Sheets[4].Cells[XJDQHEJI8346Row + 2, col].Value);
                obj_sheet.SetValue(9, col, fpSpread1.Sheets[4].Cells[XJDQHEJI8346Row + 3, col].Value);

                //判断合计和分项值是否相等来标颜色
                for (int i = 0; i < 4; i++)
                {
                    double a = 0;
                    double b = 0;
                    double c = 0;
                    double d = 0;
                    double sum = 0;
                    if (obj_sheet.Cells[6 + i, col].Value != null)
                    {
                         double.TryParse(obj_sheet.Cells[6 + i, col].Value.ToString() , out sum);
                    }

                    if (obj_sheet.Cells[10 + i, col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[10 + i, col].Value.ToString(),  out a);
                    }
                    if (obj_sheet.Cells[10 + i + 4, col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[10 + i + 4, col].Value.ToString(),  out b);
                    }
                    if (obj_sheet.Cells[10 + i + 8, col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[10 + i + 8, col].Value.ToString(),  out c);
                    }
                    if (obj_sheet.Cells[10 + i + 12, col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[10 + i + 12, col].Value.ToString(),  out d);
                    }

                    if (a + b + c + d == sum)
                    {
                        obj_sheet.Cells[10 + i - 4, col].BackColor = Color.White;
                    }
                    else
                    {
                        obj_sheet.Cells[10 + i - 4, col].BackColor = Color.Red;
                    }

                }
            }


        }
        private void Sheet83_SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //解开用户手工输入部分并设定格式
            for (int i = 5; i < 11; i++)
            {
                for (int j = 10; j < 26;  j++)
                {
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, j, i);
                }

            }
        }
        private void Build_Sheet83_46(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //查询AreaName的市辖供电区条件
            string SXtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ='市辖供电区' " + " Group by AreaName";
            //查询AreaName的县级供电区条件
            string XJtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ!='市辖供电区' " + " Group by AreaName";
            IList SXarea = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            IList XJarea = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录市辖分区数
            int SXsum = SXarea.Count;
            //记录县级分区数
            int XJsum = XJarea.Count;
            //12列
            //每增加一个分区相应的增加6行
            rowcount = 2 + (SXsum + XJsum + 2) * 4;
            colcount = 12;
            title = "附表46  铜陵市中压配变设备改造规模";
            sheetname = "表8-3 附表46";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 85;
            obj_sheet.Columns[1].Width = 85;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[4].Width = 90;
            obj_sheet.Columns[11].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 1, 2);
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 5; i < 11; i++)
            {
                obj_sheet.SetValue(1, i, i + 2005 + "年");
            }
            obj_sheet.SetValue(1, 11, "“十二五”合计");
            int startrow = 2;

            int length = 4;
            //写入市辖供电区合计部分
            Sheet83_46AddItems(obj_sheet, startrow, "合计");
            if (SXsum != 0)
            {
                //循环写入市辖供电区分区名和相应的项目部分
                for (int m = 0; m < SXsum; m++)
                {
                    Sheet83_46AddItems(obj_sheet, startrow + (m + 1) * length, SXarea[m].ToString());
                    //Sheet83_46AddData(obj_sheet, startrow + (m + 1) * length, SXarea[m].ToString(), "市辖供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1 * length, 5, SXsum, length, 2, 5, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, 2, 5, length, 6);
            }
            //合并市辖部分，写入分区类型
            obj_sheet.AddSpanCell(2, 0, (1 + SXsum) * length, 1);
            obj_sheet.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计部分
            Sheet83_46AddItems(obj_sheet, startrow + (1 + SXsum) * length, "合计");
            XJDQHEJI8346Row = startrow + (1 + SXsum) * length;
            if (XJsum != 0)
            {
                
                //循环写入县级供电区分区名和相应的项目部分
                for (int m = 0; m < XJsum; m++)
                {
                    Sheet83_46AddItems(obj_sheet, startrow + (SXsum + m + 2) * length, XJarea[m].ToString());
                   
                    //Sheet83_46AddData(obj_sheet, startrow + (SXsum + m + 2) * length, XJarea[m].ToString(), "县级供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (SXsum + 2) * length, 5, XJsum, length, startrow + (1 + SXsum) * length, 5, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, startrow + (1 + SXsum) * length, 5, length, 6);
            }
            //合并县级部分，写入分区类型
            obj_sheet.AddSpanCell(startrow + (1 + SXsum) * length, 0, (1 + XJsum) * length, 1);
            obj_sheet.SetValue(startrow + (1 + SXsum) * length, 0, "县级供电区");
            //写入十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 6, (2 + XJsum + SXsum) * length, 5, 11);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //解开手工输入部分并设格式
            Sheet83_46SetCellType(obj_sheet);
            

        }
        private void Sheet83_46SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //反回县级部分的行号
            int m = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            for (int row = 6; row < obj_sheet.RowCount; row++)
            {
                for (int col = 5; col < 11; col++)
                {
                    //如果是县级合计部分则跳过
                    if (row != m && row != (m+ 1) && row != (m+ 2) && row != (m + 3))
                    {
                        fc.Sheet_UnLockedandCellNumber(obj_sheet, row, col);
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
        private void Sheet83_46AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname)
        {
            //添加表8-3附表46的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 4, "台数（台）");
            obj_sheet.SetValue(row + 1, 4, "容量（MVA）");
            obj_sheet.AddSpanCell(row, 3, 2, 1);
            obj_sheet.SetValue(row, 3, "高损配变");

            obj_sheet.SetValue(row + 2, 4, "组数（组）");
            obj_sheet.SetValue(row + 3, 4, "容量（Mvar）");
            obj_sheet.AddSpanCell(row + 2, 3, 2, 1);
            obj_sheet.SetValue(row + 2, 3, "无功补偿");

            obj_sheet.AddSpanCell(row, 2, 4, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 4, 1);
            obj_sheet.SetValue(row, 1, itemname);

        }
        private void Sheet83_46SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            /*
            templist8346.Clear();
           //用于更新时保存用户数据
            for (int row=6;row<obj_sheet.RowCount;row++)
            {
                if (obj_sheet.Cells[row,1].ToString()!="合计")
                {
                    SaveData8346 temp8346=new SaveData8346 ();
                    temp8346.dq=fc.Sheet_find_Rownotemptycell(obj_sheet,row,0);
                    temp8346.AreaName=fc.Sheet_find_Rownotemptycell(obj_sheet,row,1);
                    for (int col=5;col<11;col++)
                    {
                        if (obj_sheet.Cells[row,col].Value!=null)
                        {
                            temp8346.data[0, col - 5] = obj_sheet.Cells[row, col].Value.ToString();
                        }
                        if (obj_sheet.Cells[row+1, col].Value != null)
                        {
                            temp8346.data[1, col - 5] = obj_sheet.Cells[row+1, col].Value.ToString();
                        }
                        if (obj_sheet.Cells[row + 2, col].Value != null)
                        {
                            temp8346.data[2, col - 5] = obj_sheet.Cells[row+2, col].Value.ToString();
                        }
                        if (obj_sheet.Cells[row + 3, col].Value != null)
                        {
                            temp8346.data[3, col - 5] = obj_sheet.Cells[row+3, col].Value.ToString();
                        }
                      
                    }
                    templist8346.Add(temp8346);
                }
                row = row + 3;
            }
            */
             templist8346.Clear();
           //用于更新时保存用户数据
            for (int row=6;row<obj_sheet.RowCount;row++)
            {
                if (obj_sheet.Cells[row,1].ToString()!="合计")
                {
                    SaveData8346 temp8346=new SaveData8346 ();
                    temp8346.dq=fc.Sheet_find_Rownotemptycell(obj_sheet,row,0);
                    temp8346.AreaName=fc.Sheet_find_Rownotemptycell(obj_sheet,row,1);
                    for (int col=5;col<11;col++)
                    {
                        if (obj_sheet.Cells[row,col].Value!=null)
                        {
                            //temp8346.data[0, col - 5] = obj_sheetg.Cells[row ,col].Value;
                            temp8346.data[0, col - 5] = obj_sheet.GetValue(row,col);                        }
                        if (obj_sheet.Cells[row+1, col].Value != null)
                        {
                            temp8346.data[1, col - 5] = obj_sheet.GetValue(row + 1, col);
                        }
                        if (obj_sheet.Cells[row + 2, col].Value != null)
                        {
                            temp8346.data[2, col - 5] = obj_sheet.GetValue(row+2, col);
                        }
                        if (obj_sheet.Cells[row + 3, col].Value != null)
                        {
                            temp8346.data[3, col - 5] = obj_sheet.GetValue(row+3, col);
                        }
                      
                    }
                    templist8346.Add(temp8346);
                }
                row = row + 3;
            }
        }
        private void Sheet83_46WriteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            ////用于更新时回写用户数据
            //for (int row = 6; row < obj_sheet.RowCount; row++)
            //{
            //    for (int j = 0; j < templist8346.Count; j++)
            //    {
            //        if (fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0) == templist8346[j].dq && fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1) == templist8346[j].AreaName)
            //        {
            //            for (int col = 5; col < 11; col++)
            //            {
            //                if (templist8346[j].data[0, col - 5].ToString() != "")
            //                {
            //                    obj_sheet.SetValue(row, col, templist8346[j].data[0, col - 5].ToString());
            //                }

            //                if (templist8346[j].data[1, col - 5].ToString() != "")
            //                {
            //                    obj_sheet.SetValue(row + 1, col, templist8346[j].data[1, col - 5].ToString());
            //                }
            //                if (templist8346[j].data[2, col - 5].ToString() != "")
            //                {
            //                    obj_sheet.SetValue(row + 2, col, templist8346[j].data[2, col - 5].ToString());
            //                }
            //                if (templist8346[j].data[3, col - 5].ToString() != "")
            //                {
            //                    obj_sheet.SetValue(row + 3, col, templist8346[j].data[3, col - 5].ToString());
            //                }

            //            }
            //            templist8346.Remove(templist8346[j]);
            //            break;
            //        }
            //    }
            //    row = row + 3;

            //}
            //用于更新时回写用户数据
            for (int row = 6; row < obj_sheet.RowCount; row++)
            {
                for (int j = 0; j < templist8346.Count; j++)
                {
                    if (fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0) == templist8346[j].dq && fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1) == templist8346[j].AreaName)
                    {
                        for (int col = 5; col < 11; col++)
                        {
                            if (templist8346[j].data[0, col - 5]!=null)
                            {
                                //obj_sheet.SetValue(row, col, templist8346[j].data[0, col - 5].ToString());
                                obj_sheet.SetValue(row, col, templist8346[j].data[0, col - 5]);
                            }

                            if (templist8346[j].data[1, col - 5] != null)
                            {
                                obj_sheet.SetValue(row + 1, col, templist8346[j].data[1, col - 5]);
                            }
                            if (templist8346[j].data[2, col - 5] != null)
                            {
                                obj_sheet.SetValue(row + 2, col, templist8346[j].data[2, col - 5]);
                            }
                            if (templist8346[j].data[3, col - 5] != null)
                            {
                                obj_sheet.SetValue(row + 3, col, templist8346[j].data[3, col - 5]);
                            }

                        }
                        templist8346.Remove(templist8346[j]);
                        break;
                    }
                }
                row = row + 3;

            }
        }
        private void Build_Sheet84(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //30行11列
            rowcount = 30;
            colcount = 11;
            title = "表8-4  铜陵市中压配电线路新建工程量";
            sheetname = "表8-4";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[3].Width = 130;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;
            int j = 0;
            int length = 4;
            //写入市辖供电区部分
            Sheet84_AddItems(obj_sheet, startrow + j * length, "市辖供电区", "1");
            Sheet84_AddData(obj_sheet, startrow + j * length, "市辖供电区");
            j++;
            //写入县级供电区部分
            Sheet84_AddItems(obj_sheet, startrow + j * length, "县级供电区", "2");
            j++;
            //写入直供直管部分
            Sheet84_AddItems(obj_sheet, startrow + j * length, "其中：直供直管", "2.1");
            Sheet84_AddData(obj_sheet, startrow + j * length, "县级直供直管");
            j++;
            //写入县级控股部分
            Sheet84_AddItems(obj_sheet, startrow + j * length, "控股", "2.2");
            Sheet84_AddData(obj_sheet, startrow + j * length, "县级控股");
            j++;
            //写入县级参股部分
            Sheet84_AddItems(obj_sheet, startrow + j * length, "参股", "2.3");
            Sheet84_AddData(obj_sheet, startrow + j * length, "县级参股");
            j++;
            //写入县级代管部分
            Sheet84_AddItems(obj_sheet, startrow + j * length, "代管", "2.4");
            Sheet84_AddData(obj_sheet, startrow + j * length, "县级代管");
            j++;
            //写入全地区合计部分
            Sheet84_AddItems(obj_sheet, startrow + j * length, "全地区合计", "3");

            //写入计算县级供电区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 10, 4, 4, 4, 6, 4, 6);
            //写入计算全地区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 4, 2, 4, 26, 4, 6);
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 28, 5, 10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }  
        private void Sheet84_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname, string bianhao)
        {
            //添加表8-4的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 3, "线路总条数（条）");
            obj_sheet.SetValue(row + 1, 3, "架空线路（km）");
            obj_sheet.SetValue(row + 2, 3, "电缆线路（km）");
            obj_sheet.SetValue(row + 3, 3, "主干线路（km）");
            obj_sheet.AddSpanCell(row, 2, 4, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 4, 1);
            obj_sheet.SetValue(row, 1, itemname);
            obj_sheet.AddSpanCell(row, 0, 4, 1);
            obj_sheet.SetValue(row, 0, bianhao);
        }
        private void Sheet84_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string DQ)
        {
             //用年份部分列数做外循环控制
              for (int i = 4; i < 10; i++)
              {
                  //写入线路条数
                  string tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-line' and  DQ='" + DQ + "' and LineInfo like '10@%' ";
                  double XLsum = 0;
                  if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                  {
                      XLsum =(double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                  }
                  obj_sheet.SetValue(row , i, XLsum);
                  //写入架空线长
                  double JKlenth = 0;
                  if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian) != null)
                  {
                      JKlenth = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian);
                  }
                  obj_sheet.SetValue(row + 1, i, JKlenth);
                  //写入电缆线长度
                  double DLXlenth = 0;
                  if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian) != null)
                  {
                      DLXlenth = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian);
                  }
                  obj_sheet.SetValue(row + 2, i, DLXlenth);
                  //写入主干线长度
                  double ZGXlenth = JKlenth+DLXlenth;
                  obj_sheet.SetValue(row + 3, i, ZGXlenth);
              }
        }
        private void Build_Sheet84_47(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //查询AreaName的市辖供电区条件
            string SXtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ='市辖供电区' " + " and Col3='新建' and Col4='pw-line' and LineInfo like '10@%' Group by AreaName";
            //查询AreaName的县级供电区条件
            string XJtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ!='市辖供电区' " + " and Col3='新建' and Col4='pw-line' and LineInfo like '10@%' Group by AreaName";
            IList SXarea = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            IList XJarea = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录市辖分区数
            int SXsum = SXarea.Count;
            //记录县级分区数
            int XJsum = XJarea.Count;
            //12列
            //每增加一个分区相应的增加6行
            rowcount = 2 + (SXsum + XJsum + 2) * 4;
            colcount = 11;
            title = "附表47  铜陵市中压配电线路新建工程量统计";
            sheetname = "表8-4 附表47 ";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 85;
            obj_sheet.Columns[1].Width = 85;
            obj_sheet.Columns[3].Width = 120;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;

            int length = 4;
            //写入市辖供电区合计部分
            Sheet84_47AddItems(obj_sheet, startrow, "合计");
            if (SXsum != 0)
            {
                //循环写入市辖供电区分区名和相应的项目部分
                for (int m = 0; m < SXsum; m++)
                {
                    Sheet84_47AddItems(obj_sheet, startrow + (m + 1) *length, SXarea[m].ToString());
                    Sheet84_47AddData(obj_sheet, startrow + (m + 1) *length, SXarea[m].ToString(), "市辖供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1 * length, 4, SXsum, length, 2, 4, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, 2, 4, length, 6);
            }
            //合并市辖部分，写入分区类型
            obj_sheet.AddSpanCell(2, 0, (1 + SXsum) *length, 1);
            obj_sheet.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计部分
            Sheet84_47AddItems(obj_sheet, startrow + (1 + SXsum) *length, "合计");
            if (XJsum != 0)
            {
                //循环写入县级供电区分区名和相应的项目部分
                for (int m = 0; m < XJsum; m++)
                {
                    Sheet84_47AddItems(obj_sheet, startrow + (SXsum + m + 2) *length, XJarea[m].ToString());
                    Sheet84_47AddData(obj_sheet, startrow + (SXsum + m + 2) *length, XJarea[m].ToString(), "县级供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (SXsum + 2) * length, 4, XJsum, length, startrow + (1 + SXsum) * length, 4, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, startrow + (1 + SXsum) *length,4,length,6);
            }
            //合并县级部分，写入分区类型
            obj_sheet.AddSpanCell(startrow + (1 + SXsum) *length, 0, (1 + XJsum) *length, 1);
            obj_sheet.SetValue(startrow + (1 + SXsum) *length, 0, "县级供电区");
            //写入十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, (2 + XJsum + SXsum) * length, 5, 10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet84_47AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname)
        {
            //添加表8-4附表面7的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 3, "线路总条数（条）");
            obj_sheet.SetValue(row + 1, 3, "架空线路（km）");
            obj_sheet.SetValue(row + 2, 3, "电缆线路（km）");
            obj_sheet.SetValue(row + 3, 3, "主干线路（km）");
            obj_sheet.AddSpanCell(row, 2, 4, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 4, 1);
            obj_sheet.SetValue(row, 1, itemname);
            
        }
        private void Sheet84_47AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row,string areaname, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 4; i < 10; i++)
            {
                string tiaojian = "";
                if (DQ=="市辖供电区")
                {
                    //市辖供电区
                    tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-line' and  DQ='市辖供电区' and  AreaName='" + areaname + "' and LineInfo like '10@%' ";
                }
                else
                {
                    //县级供电区分多种所以 DQ!='市辖供电区'
                    tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-line' and  DQ!='市辖供电区' and  AreaName='" + areaname + "' and LineInfo like '10@%' ";
                }
                 //写入线路条数
                double XLsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                {
                    XLsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                }
                obj_sheet.SetValue(row, i, XLsum);
                //写入架空线长
                double JKlenth = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian) != null)
                {
                    JKlenth = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian);
                }
                obj_sheet.SetValue(row + 1, i, JKlenth);
                //写入电缆线长度
                double DLXlenth = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian) != null)
                {
                    DLXlenth = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian);
                }
                obj_sheet.SetValue(row + 2, i, DLXlenth);
                //写入主干线长度
                double ZGXlenth = JKlenth + DLXlenth;
                obj_sheet.SetValue(row + 3, i, ZGXlenth);
            }
        }
        private void Build_Sheet85(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //30行11列
            rowcount = 30;
            colcount = 11;
            title = "表8-5 铜陵市中压配电网新增开关设备规模";
            sheetname = "表8-5";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[3].Width = 120;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.SetValue(1, 3, "类 型");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;
            int j = 0;
            int length = 4;
            //写入市辖供电区部分
            Sheet85_AddItems(obj_sheet, startrow + j * length, "市辖供电区", "1");
            Sheet85_AddData(obj_sheet, startrow + j * length, "市辖供电区");
            j++;
            //写入县级供电区部分
            Sheet85_AddItems(obj_sheet, startrow + j * length, "县级供电区", "2");
            j++;
            //写入直供直管部分
            Sheet85_AddItems(obj_sheet, startrow + j * length, "其中：直供直管", "2.1");
            Sheet85_AddData(obj_sheet, startrow + j * length, "县级直供直管");
            j++;
            //写入县级控股部分
            Sheet85_AddItems(obj_sheet, startrow + j * length, "控股", "2.2");
            Sheet85_AddData(obj_sheet, startrow + j * length, "县级控股");
            j++;
            //写入县级参股部分
            Sheet85_AddItems(obj_sheet, startrow + j * length, "参股", "2.3");
            Sheet85_AddData(obj_sheet, startrow + j * length, "县级参股");
            j++;
            //写入县级代管部分
            Sheet85_AddItems(obj_sheet, startrow + j * length, "代管", "2.4");
            Sheet85_AddData(obj_sheet, startrow + j * length, "县级代管");
            j++;
            //写入全地区合计部分
            Sheet85_AddItems(obj_sheet, startrow + j * length, "全地区合计", "3");

            //写入计算县级供电区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 10, 4, 4, 4, 6, 4, 6);
            //写入计算全地区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 4, 2, 4, 26, 4, 6);
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 28, 5, 10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet85_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname, string bianhao)
        {
            //添加表8-5的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 3, "开关站（座）");
            obj_sheet.SetValue(row + 1, 3, "环网柜（座）");
            obj_sheet.SetValue(row + 2, 3, "柱上开关（台）");
            obj_sheet.SetValue(row + 3, 3, "电缆分支箱（座）");
            obj_sheet.AddSpanCell(row, 2, 4, 1);
            obj_sheet.SetValue(row, 2, "10");
            obj_sheet.AddSpanCell(row, 1, 4, 1);
            obj_sheet.SetValue(row, 1, itemname);
            obj_sheet.AddSpanCell(row, 0, 4, 1);
            obj_sheet.SetValue(row, 0, bianhao);
        }
        private void Sheet85_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 4; i < 10; i++)
            {
                //写入开关站数量
                string tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-kg' and  DQ='" + DQ + "' and BianInfo like '10@%' ";
                double KGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                {
                    KGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                }
                obj_sheet.SetValue(row, i, KGsum);
                //写入环网柜座数
                double HWGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) != null)
                {
                    HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                }
                obj_sheet.SetValue(row + 1, i, HWGsum);
                //写入柱上开关台数
                double ZSKGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                {
                    ZSKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                }
                obj_sheet.SetValue(row + 2, i, ZSKGsum);
                //写入电缆分支箱数
                double DLFZsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian) != null)
                {
                    DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian);
                }
                obj_sheet.SetValue(row + 3, i, DLFZsum);
                
            }
        }
        private void Build_Sheet85_48(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //查询AreaName的市辖供电区条件
            string SXtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ='市辖供电区' " + " and Col3='新建' and Col4='pw-kg' and BianInfo like '10@%' Group by AreaName";
            //查询AreaName的县级供电区条件
            string XJtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ!='市辖供电区' " + " and Col3='新建' and Col4='pw-kg' and BianInfo like '10@%' Group by AreaName";
            IList SXarea = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            IList XJarea = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录市辖分区数
            int SXsum = SXarea.Count;
            //记录县级分区数
            int XJsum = XJarea.Count;
            //11列
            //每增加一个分区相应的增加4行
            rowcount = 2 + (SXsum + XJsum + 2) * 4;
            colcount = 11;
            title = "附表48  铜陵市中压配电网新增开关设备规模估算";
            sheetname = "表8-5 附表48";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 85;
            obj_sheet.Columns[1].Width = 85;
            obj_sheet.Columns[3].Width = 120;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.SetValue(1, 3, "类 型");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;

            int length = 4;
            //写入市辖供电区合计部分
            Sheet85_48AddItems(obj_sheet, startrow, "合计");
            if (SXsum != 0)
            {
                //循环写入市辖供电区分区名和相应的项目部分
                for (int m = 0; m < SXsum; m++)
                {
                    Sheet85_48AddItems(obj_sheet, startrow + (m + 1) *length, SXarea[m].ToString());
                    Sheet85_48AddData(obj_sheet, startrow + (m + 1) *length, SXarea[m].ToString(), "市辖供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1 * length, 4, SXsum, length, 2, 4, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, 2, 4, length, 6);
            }
            //合并市辖部分，写入分区类型
            obj_sheet.AddSpanCell(2, 0, (1 + SXsum) *length, 1);
            obj_sheet.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计部分
            Sheet85_48AddItems(obj_sheet, startrow + (1 + SXsum) *length, "合计");
            if (XJsum != 0)
            {
                //循环写入县级供电区分区名和相应的项目部分
                for (int m = 0; m < XJsum; m++)
                {
                    Sheet85_48AddItems(obj_sheet, startrow + (SXsum + m + 2) *length, XJarea[m].ToString());
                    Sheet85_48AddData(obj_sheet, startrow + (SXsum + m + 2) *length, XJarea[m].ToString(), "县级供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (SXsum + 2) * length, 4, XJsum, length, startrow + (1 + SXsum) * length, 4, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, startrow + (1 + SXsum) *length,4,length,6);
            }
            //合并县级部分，写入分区类型
            obj_sheet.AddSpanCell(startrow + (1 + SXsum) *length, 0, (1 + XJsum) *length, 1);
            obj_sheet.SetValue(startrow + (1 + SXsum) *length, 0, "县级供电区");
            //写入十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, (2 + XJsum + SXsum) * length, 5, 10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet85_48AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname)
        {
            //添加表8-4附表面7的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 3, "开关站（座）");
            obj_sheet.SetValue(row + 1, 3, "环网柜（座）");
            obj_sheet.SetValue(row + 2, 3, "柱上开关（台）");
            obj_sheet.SetValue(row + 3, 3, "电缆分支箱（座）");
            obj_sheet.AddSpanCell(row, 2, 4, 1);
            obj_sheet.SetValue(row, 2, "10");
            obj_sheet.AddSpanCell(row, 1, 4, 1);
            obj_sheet.SetValue(row, 1, itemname);
            
        }
        private void Sheet85_48AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row,string areaname, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 4; i < 10; i++)
            {
                string tiaojian = "";
                if (DQ=="市辖供电区")
                {
                    //市辖供电区
                    tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-kg' and  DQ='市辖供电区' and  AreaName='" + areaname + "' and BianInfo like '10@%' ";
                }
                else
                {
                    //县级供电区分多种所以 DQ!='市辖供电区'
                    tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-kg' and  DQ!='市辖供电区' and  AreaName='" + areaname + "' and BianInfo like '10@%' ";
                }
                double KGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                {
                    KGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                }
                obj_sheet.SetValue(row, i, KGsum);
                //写入环网柜座数
                double HWGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) != null)
                {
                    HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                }
                obj_sheet.SetValue(row + 1, i, HWGsum);
                //写入柱上开关台数
                double ZSKGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                {
                    ZSKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                }
                obj_sheet.SetValue(row + 2, i, ZSKGsum);
                //写入电缆分支箱数
                double DLFZsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian) != null)
                {
                    DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian);
                }
                obj_sheet.SetValue(row + 3, i, DLFZsum);
            }
        }
        private void Build_Sheet86(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //58行12列
            rowcount = 58;
            colcount = 12;
            title = "表8-6  铜陵市中压配电网络改造规模";
            sheetname = "表8-6";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[3].Width = 65;
            obj_sheet.Columns[4].Width = 110;
            obj_sheet.Columns[11].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 1, 2);
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 5; i < 11; i++)
            {
                obj_sheet.SetValue(1, i, i + 2005 + "年");
            }
            obj_sheet.SetValue(1, 11, "“十二五”合计");
            int startrow = 2;
            int j = 0;
            int length = 8;
            //写入市辖供电区部分
            Sheet86_AddItems(obj_sheet, startrow + j * length, "市辖供电区", "1");
            Sheet86_AddData(obj_sheet, startrow + j * length, "市辖供电区");
            j++;
            //写入县级供电区部分
            Sheet86_AddItems(obj_sheet, startrow + j * length, "县级供电区", "2");
            j++;
            //写入直供直管部分
            Sheet86_AddItems(obj_sheet, startrow + j * length, "其中：直供直管", "2.1");
            Sheet86_AddData(obj_sheet, startrow + j * length, "县级直供直管");
            j++;
            //写入县级控股部分
            Sheet86_AddItems(obj_sheet, startrow + j * length, "控股", "2.2");
            Sheet86_AddData(obj_sheet, startrow + j * length, "县级控股");
            j++;
            //写入县级参股部分
            Sheet86_AddItems(obj_sheet, startrow + j * length, "参股", "2.3");
            Sheet86_AddData(obj_sheet, startrow + j * length, "县级参股");
            j++;
            //写入县级代管部分
            Sheet86_AddItems(obj_sheet, startrow + j * length, "代管", "2.4");
            Sheet86_AddData(obj_sheet, startrow + j * length, "县级代管");
            j++;
            //写入全地区合计部分
            Sheet86_AddItems(obj_sheet, startrow + j * length, "全地区合计", "3");

            //写入计算县级供电区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 18, 5, 4, 8, 10, 5, 6);
            //写入计算全地区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 5, 2, 8, 50, 5, 6);
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 6, 56, 5, 11);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet86_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname, string bianhao)
        {
            //添加表8-6的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 4, "断路器（台）");
            obj_sheet.SetValue(row + 1, 4, "负荷开关（台）");
            obj_sheet.SetValue(row + 2, 4, "环网柜（座）");
            obj_sheet.SetValue(row + 3, 4, "电缆分支箱（座）");
            obj_sheet.AddSpanCell(row, 3, 4, 1);
            obj_sheet.SetValue(row, 3, "开关类");

            obj_sheet.SetValue(row + 4, 4, "线路长度（km）");
            obj_sheet.SetValue(row + 5, 4, "杆塔（基）");
            obj_sheet.AddSpanCell(row + 4, 3, 2, 1);
            obj_sheet.SetValue(row + 4, 3, "架空线");

            obj_sheet.SetValue(row + 6, 4, "长度（km）");
            obj_sheet.SetValue(row + 7, 4, "沟道（km）");
            obj_sheet.AddSpanCell(row + 6, 3, 2, 1);
            obj_sheet.SetValue(row + 6, 3, "电缆");

            obj_sheet.AddSpanCell(row, 2, 8, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 8, 1);
            obj_sheet.SetValue(row, 1, itemname);
            obj_sheet.AddSpanCell(row, 0, 8, 1);
            obj_sheet.SetValue(row, 0, bianhao);
        }
        private void Sheet86_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 5; i < 11; i++)
            {
                //写入断路器数
                string kgtiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and Col3='改造' and Col4='pw-kg' and  DQ='" + DQ + "' and BianInfo like '10@%' ";
                string xltiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and Col3='改造' and Col4='pw-line' and  DQ='" + DQ + "' and BianInfo like '10@%' ";
                double DLQsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", kgtiaojian) != null)
                {
                    DLQsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", kgtiaojian);
                }
                obj_sheet.SetValue(row, i, DLQsum);
                //写入负荷开关（台）
                double FHKGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", kgtiaojian) != null)
                {
                    FHKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", kgtiaojian);
                }
                obj_sheet.SetValue(row + 1, i, FHKGsum);
                //写入环网柜座数
                double HWGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", kgtiaojian) != null)
                {
                    HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", kgtiaojian);
                }
                obj_sheet.SetValue(row + 2, i, HWGsum);
               
                //写入电缆分支箱数
                double DLFZsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", kgtiaojian) != null)
                {
                    DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", kgtiaojian);
                }
                obj_sheet.SetValue(row + 3, i, DLFZsum);

                //写入架空线路长度（km）
                double JKXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", xltiaojian) != null)
                {
                    JKXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", xltiaojian);
                }
                obj_sheet.SetValue(row + 4, i, JKXlength);
                //写入架空线杆塔（基）
                double JKGTJsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", xltiaojian) != null)
                {
                    JKGTJsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", xltiaojian);
                }
                obj_sheet.SetValue(row + 5, i, JKGTJsum);
                //写入电缆线路长度（km）
                double DLXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", xltiaojian) != null)
                {
                    DLXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", xltiaojian);
                }
                obj_sheet.SetValue(row + 6, i, DLXlength);
                //写入电缆沟道（km）
                double DLGDlength= 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", xltiaojian) != null)
                {
                    DLGDlength = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", xltiaojian);
                }
                obj_sheet.SetValue(row + 7, i, DLXlength);
            }
        }
        private void Build_Sheet86_49(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //查询AreaName的市辖供电区条件
            string SXtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ='市辖供电区' " + " and Col3='改造' and (Col4='pw-kg' or Col4='pw-line') and BianInfo like '10@%' Group by AreaName";
            //查询AreaName的县级供电区条件
            string XJtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ!='市辖供电区' " + " and Col3='改造' and (Col4='pw-kg' or Col4='pw-line') and BianInfo like '10@%' Group by AreaName";
            IList SXarea = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            IList XJarea = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录市辖分区数
            int SXsum = SXarea.Count;
            //记录县级分区数
            int XJsum = XJarea.Count;
            //12列
            //每增加一个分区相应的增加8行
            rowcount = 2 + (SXsum + XJsum + 2) * 8;
            colcount = 12;
            title = "附表49  铜陵市中压配电网络改造规模";
            sheetname = "表8-6 附表49 ";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 85;
            obj_sheet.Columns[1].Width = 85;
            obj_sheet.Columns[3].Width = 75;
            obj_sheet.Columns[4].Width = 120;
            obj_sheet.Columns[11].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 1, 2);
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 5; i < 11; i++)
            {
                obj_sheet.SetValue(1, i, i + 2005 + "年");
            }
            obj_sheet.SetValue(1, 11, "“十二五”合计");
            int startrow = 2;
            //int j = 0;
            int length = 8;
            //写入市辖供电区合计部分
            Sheet86_49AddItems(obj_sheet, startrow, "合计");
            if (SXsum != 0)
            {
                //循环写入市辖供电区分区名和相应的项目部分
                for (int m = 0; m < SXsum; m++)
                {
                    Sheet86_49AddItems(obj_sheet, startrow + (m + 1) * length, SXarea[m].ToString());
                    Sheet86_49AddData(obj_sheet, startrow + (m + 1) * length, SXarea[m].ToString(), "市辖供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1 * length, 5, SXsum, length, 2, 5, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, 2, 5, length, 6);
            }
            //合并市辖部分，写入分区类型
            obj_sheet.AddSpanCell(2, 0, (1 + SXsum) * length, 1);
            obj_sheet.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计部分
            Sheet86_49AddItems(obj_sheet, startrow + (1 + SXsum) * length, "合计");
            if (XJsum != 0)
            {
                //循环写入县级供电区分区名和相应的项目部分
                for (int m = 0; m < XJsum; m++)
                {
                    Sheet86_49AddItems(obj_sheet, startrow + (SXsum + m + 2) * length, XJarea[m].ToString());
                    Sheet86_49AddData(obj_sheet, startrow + (SXsum + m + 2) * length, XJarea[m].ToString(), "县级供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (SXsum + 2) * length, 5, XJsum, length, startrow + (1 + SXsum) * length, 5, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, startrow + (1 + SXsum) * length, 5, length, 6);
            }
            //合并县级部分，写入分区类型
            obj_sheet.AddSpanCell(startrow + (1 + SXsum) * length, 0, (1 + XJsum) * length, 1);
            obj_sheet.SetValue(startrow + (1 + SXsum) * length, 0, "县级供电区");
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 6, (2 + XJsum + SXsum) * length, 5, 11);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet86_49AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname)
        {
            //添加表8-6附表49的项目部分，其中分区名称做为参数传入
           
            obj_sheet.SetValue(row, 4, "断路器（台）");
            obj_sheet.SetValue(row + 1, 4, "负荷开关（台）");
            obj_sheet.SetValue(row + 2, 4, "环网柜（座）");
            obj_sheet.SetValue(row + 3, 4, "电缆分支箱（座）");
            obj_sheet.AddSpanCell(row, 3, 4, 1);
            obj_sheet.SetValue(row, 3, "开关类");

            obj_sheet.SetValue(row + 4, 4, "线路长度（km）");
            obj_sheet.SetValue(row + 5, 4, "杆塔（基）");
            obj_sheet.AddSpanCell(row + 4, 3, 2, 1);
            obj_sheet.SetValue(row + 4, 3, "架空线");

            obj_sheet.SetValue(row + 6, 4, "长度（km）");
            obj_sheet.SetValue(row + 7, 4, "沟道（km）");
            obj_sheet.AddSpanCell(row + 6, 3, 2, 1);
            obj_sheet.SetValue(row + 6, 3, "电缆");

            obj_sheet.AddSpanCell(row, 2, 8, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 8, 1);
            obj_sheet.SetValue(row, 1, itemname);
         

        }
        private void Sheet86_49AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string areaname, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 5; i < 11; i++)
            {
                //开关类条件
                string kgtiaojian  = "";
                //线路类条件
                string xltiaojian = "";
                if (DQ == "市辖供电区")
                {
                    //市辖供电区
                    kgtiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and Col3='改造' and Col4='pw-kg' and  DQ='市辖供电区' and  AreaName='" + areaname + "' and BianInfo like '10@%' ";
                    xltiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and Col3='改造' and Col4='pw-line' and  DQ='市辖供电区' and  AreaName='" + areaname + "' and BianInfo like '10@%' ";
                }
                else
                {
                    //县级供电区分多种所以 DQ!='市辖供电区'
                    kgtiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and Col3='改造' and Col4='pw-kg' and  DQ!='市辖供电区' and  AreaName='" + areaname + "' and BianInfo like '10@%' ";
                    xltiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2005) + "' and Col3='改造' and Col4='pw-line' and  DQ!='市辖供电区' and  AreaName='" + areaname + "' and BianInfo like '10@%' ";
                }
                //写入断路器数
                double DLQsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", kgtiaojian) != null)
                {
                    DLQsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", kgtiaojian);
                }
                obj_sheet.SetValue(row, i, DLQsum);
                //写入负荷开关（台）
                double FHKGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", kgtiaojian) != null)
                {
                    FHKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", kgtiaojian);
                }
                obj_sheet.SetValue(row + 1, i, FHKGsum);
                //写入环网柜座数
                double HWGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", kgtiaojian) != null)
                {
                    HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", kgtiaojian);
                }
                obj_sheet.SetValue(row + 2, i, HWGsum);

                //写入电缆分支箱数
                double DLFZsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", kgtiaojian) != null)
                {
                    DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", kgtiaojian);
                }
                obj_sheet.SetValue(row + 3, i, DLFZsum);

                //写入架空线路长度（km）
                double JKXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", xltiaojian) != null)
                {
                    JKXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", xltiaojian);
                }
                obj_sheet.SetValue(row + 4, i, JKXlength);
                //写入架空线杆塔（基）
                double JKGTJsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", xltiaojian) != null)
                {
                    JKGTJsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", xltiaojian);
                }
                obj_sheet.SetValue(row + 5, i, JKGTJsum);
                //写入电缆线路长度（km）
                double DLXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", xltiaojian) != null)
                {
                    DLXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", xltiaojian);
                }
                obj_sheet.SetValue(row + 6, i, DLXlength);
                //写入电缆沟道（km）
                double DLGDlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", xltiaojian) != null)
                {
                    DLGDlength = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", xltiaojian);
                }
                obj_sheet.SetValue(row + 7, i, DLXlength);

            }
        }
        private void Build_Sheet87(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //16行12列
            rowcount = 16;
            colcount = 11;
            title = "表8-7  铜陵市中低压配电网无功补偿设备规模";
            sheetname = "表8-7";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[3].Width = 110;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;
            int j = 0;
            int length = 2;
            //写入市辖供电区部分
            Sheet87_AddItems(obj_sheet, startrow + j * length, "市辖供电区", "1");

            j++;
            //写入县级供电区部分
            Sheet87_AddItems(obj_sheet, startrow + j * length, "县级供电区", "2");
            j++;
            //写入直供直管部分
            Sheet87_AddItems(obj_sheet, startrow + j * length, "其中：直供直管", "2.1");

            j++;
            //写入县级控股部分
            Sheet87_AddItems(obj_sheet, startrow + j * length, "控股", "2.2");

            j++;
            //写入县级参股部分
            Sheet87_AddItems(obj_sheet, startrow + j * length, "参股", "2.3");

            j++;
            //写入县级代管部分
            Sheet87_AddItems(obj_sheet, startrow + j * length, "代管", "2.4");

            j++;
            //写入全地区合计部分
            Sheet87_AddItems(obj_sheet, startrow + j * length, "全地区合计", "3");

           

            //计算全地区合计
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 4, 2, 2, 14,4,6);
            //计算十二五合计
            
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet,2,5,14,5,10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            Sheet87_SetCellType(obj_sheet);

        }
        private void Sheet87_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname, string bianhao)
        {
            //添加表8-7的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 3, "配变低压侧补偿");
            obj_sheet.SetValue(row + 1, 3, "中压线路补偿");
            obj_sheet.AddSpanCell(row, 2, 2, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 2, 1);
            obj_sheet.SetValue(row, 1, itemname);
            obj_sheet.AddSpanCell(row, 0, 2, 1);
            obj_sheet.SetValue(row, 0, bianhao);
        }
        private void Sheet87_GetDatafrom8750(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //从8-7附表50中获取添入表8-7
            for (int col = 4; col < 10; col++)
            {
                //市辖部分合计
                obj_sheet.SetValue(2, col, fpSpread1.Sheets[12].Cells[2, col].Value);
                obj_sheet.SetValue(3, col, fpSpread1.Sheets[12].Cells[3, col].Value);
              
                //县级部分合计
                obj_sheet.SetValue(4, col, fpSpread1.Sheets[12].Cells[XJDQHEJI8750Row, col].Value);
                obj_sheet.SetValue(5, col, fpSpread1.Sheets[12].Cells[XJDQHEJI8750Row + 1, col].Value);
               

                //判断合计和分项值是否相等来标颜色
                for (int i = 0; i < 2; i++)
                {
                    double a = 0;
                    double b = 0;
                    double c = 0;
                    double d = 0;
                    double sum = 0;
                    if (obj_sheet.Cells[4 + i, col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[4 + i, col].Value.ToString(),out sum);
                    }

                    if (obj_sheet.Cells[6 + i, col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[6 + i, col].Value.ToString(),out a);
                    }
                    if (obj_sheet.Cells[8 + i , col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[8 + i , col].Value.ToString(),out b);
                    }
                    if (obj_sheet.Cells[10 + i , col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[10 + i, col].Value.ToString(),out c);
                    }
                    if (obj_sheet.Cells[12 + i , col].Value != null)
                    {
                        double.TryParse(obj_sheet.Cells[12 + i , col].Value.ToString(),out d);
                    }

                    if (a + b + c + d == sum)
                    {
                        obj_sheet.Cells[4 + i , col].BackColor = Color.White;

                    }
                    else
                    {
                        obj_sheet.Cells[4 + i , col].BackColor = Color.Red;

                    }

                }
            }


        }
        private void Sheet87_SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //解开用户手工输入部分并设定格式
            for (int i = 4; i < 10; i++)
            {
                for (int j = 6; j < 14; j++)
                {
                    fc.Sheet_UnLockedandCellNumber(obj_sheet, j, i);
                }

            }
        }
        private void Build_Sheet87_50(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //查询AreaName的市辖供电区条件
            string SXtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ='市辖供电区' " + " Group by AreaName";
            //查询AreaName的县级供电区条件
            string XJtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ!='市辖供电区' " + " Group by AreaName";
            IList SXarea = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            IList XJarea = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录市辖分区数
            int SXsum = SXarea.Count;
            //记录县级分区数
            int XJsum = XJarea.Count;
            //12列
            //每增加一个分区相应的增加6行
            rowcount = 2 + (SXsum + XJsum + 2) * 2;
            colcount = 11;
            title = "附表50  铜陵市中低压配电网无功补偿设备规模";
            sheetname = "表8-7 附表50 ";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 85;
            obj_sheet.Columns[1].Width = 85;
            obj_sheet.Columns[3].Width = 110;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "分区类型");
            obj_sheet.SetValue(1, 1, "分区名称");
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.SetValue(1, 3, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;

            int length = 2;
            //写入市辖供电区合计部分
            Sheet87_50AddItems(obj_sheet, startrow, "合计");
            if (SXsum != 0)
            {
                //循环写入市辖供电区分区名和相应的项目部分
                for (int m = 0; m < SXsum; m++)
                {
                    Sheet87_50AddItems(obj_sheet, startrow + (m + 1) * length, SXarea[m].ToString());
                    //Sheet87_50AddData(obj_sheet, startrow + (m + 1) * length, SXarea[m].ToString(), "市辖供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1 * length, 4, SXsum, length, 2, 4, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, 2, 4, length, 6);
            }
            //合并市辖部分，写入分区类型
            obj_sheet.AddSpanCell(2, 0, (1 + SXsum) * length, 1);
            obj_sheet.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计部分
            Sheet87_50AddItems(obj_sheet, startrow + (1 + SXsum) * length, "合计");
            //记录县级供电区合计部分开始行号,以便复制数据到8-7时使用
            XJDQHEJI8750Row = startrow + (1 + SXsum) * length;
            if (XJsum != 0)
            {

                //循环写入县级供电区分区名和相应的项目部分
                for (int m = 0; m < XJsum; m++)
                {
                    Sheet87_50AddItems(obj_sheet, startrow + (SXsum + m + 2) * length, XJarea[m].ToString());

                    //Sheet87_50AddData(obj_sheet, startrow + (SXsum + m + 2) * length, XJarea[m].ToString(), "县级供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (SXsum + 2) * length, 4, XJsum, length, startrow + (1 + SXsum) * length, 4, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, startrow + (1 + SXsum) * length, 4, length, 6);
            }
            //合并县级部分，写入分区类型
            obj_sheet.AddSpanCell(startrow + (1 + SXsum) * length, 0, (1 + XJsum) * length, 1);
            obj_sheet.SetValue(startrow + (1 + SXsum) * length, 0, "县级供电区");
            //写入十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, (2 + XJsum + SXsum) * length, 5, 10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //解开手工输入部分并设格式
            Sheet87_50SetCellType(obj_sheet);
        }
        private void Sheet87_50SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //反回县级部分的行号
            int m = fc.Sheet_Find_Value(obj_sheet, 0, "县级供电区");
            for (int row = 4; row < obj_sheet.RowCount; row++)
            {
                for (int col = 4; col < 10; col++)
                {
                    //如果是县级合计部分则跳过
                    if (row != m && row != (m + 1))
                    {
                        fc.Sheet_UnLockedandCellNumber(obj_sheet, row, col);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        private void Sheet87_50AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname)
        {
            //添加表8-3附表46的项目部分，其中编号和类型做为参数传入
            //添加表8-7的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 3, "配变低压侧补偿");
            obj_sheet.SetValue(row + 1, 3, "中压线路补偿");
            obj_sheet.AddSpanCell(row, 2, 2, 1);
            obj_sheet.SetValue(row, 2, "10kV");
            obj_sheet.AddSpanCell(row, 1, 2, 1);
            obj_sheet.SetValue(row, 1, itemname);
        }
        private void Sheet87_50SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            
            templist8750.Clear();
            //用于更新时保存用户数据
            for (int row = 4; row < obj_sheet.RowCount; row++)
            {
                if (obj_sheet.Cells[row, 1].ToString() != "合计")
                {
                    SaveData8346 temp8750 = new SaveData8346();
                    temp8750.dq = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                    temp8750.AreaName = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1);
                    for (int col = 4; col < 10; col++)
                    {
                        if (obj_sheet.Cells[row, col].Value != null)
                        {
                            temp8750.data[0, col - 4] = obj_sheet.GetValue(row, col);
                        }
                        if (obj_sheet.Cells[row + 1, col].Value != null)
                        {
                            temp8750.data[1, col - 4] = obj_sheet.GetValue(row + 1, col);
                        }

                    }
                    templist8750.Add(temp8750);
                }
                row = row + 1;
            }
        }
        private void Sheet87_50WriteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            for (int row = 4; row < obj_sheet.RowCount; row++)
            {
                for (int j = 0; j < templist8750.Count; j++)
                {
                    if (fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0) == templist8750[j].dq && fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1) == templist8750[j].AreaName)
                    {
                        for (int col = 4; col < 10; col++)
                        {
                            if (templist8750[j].data[0, col - 4]!=null)
                            {
                                obj_sheet.SetValue(row, col, templist8750[j].data[0, col - 4]);
                            }

                            if (templist8750[j].data[1, col - 4] != null)
                            {
                                obj_sheet.SetValue(row + 1, col, templist8750[j].data[1, col - 4]);
                            }
                        }
                        templist8750.Remove(templist8750[j]);
                        break;
                    }
                }
                row = row + 1;
            }


        }
        private void Build_Sheet88(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //30行11列
            rowcount = 30;
            colcount = 11;
            title = "表8-8  铜陵市低压配电网新增线路工程量";
            sheetname = "表8-8";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[2].Width = 90;
            obj_sheet.Columns[3].Width = 90;
            //obj_sheet.Columns[3].Width = 60;
            //obj_sheet.Columns[4].Width = 110;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;
            int j = 0;
            int length = 4;
            //写入市辖供电区部分
            Sheet88_AddItems(obj_sheet, startrow + j * length, "市辖供电区", "1");
            Sheet88_AddData(obj_sheet, startrow + j * length, "市辖供电区");
            j++;
            //写入县级供电区部分
            Sheet88_AddItems(obj_sheet, startrow + j * length, "县级供电区", "2");
            j++;
            //写入直供直管部分
            Sheet88_AddItems(obj_sheet, startrow + j * length, "其中：直供直管", "2.1");
            Sheet88_AddData(obj_sheet, startrow + j * length, "县级直供直管");
            j++;
            //写入县级控股部分
            Sheet88_AddItems(obj_sheet, startrow + j * length, "控股", "2.2");
            Sheet88_AddData(obj_sheet, startrow + j * length, "县级控股");
            j++;
            //写入县级参股部分
            Sheet88_AddItems(obj_sheet, startrow + j * length, "参股", "2.3");
            Sheet88_AddData(obj_sheet, startrow + j * length, "县级参股");
            j++;
            //写入县级代管部分
            Sheet88_AddItems(obj_sheet, startrow + j * length, "代管", "2.4");
            Sheet88_AddData(obj_sheet, startrow + j * length, "县级代管");
            j++;
            //写入全地区合计部分
            Sheet88_AddItems(obj_sheet, startrow + j * length, "全地区合计", "3");

            //写入计算县级供电区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet,10,4,4,4,6,4,6);
            //写入计算全地区合计公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet,2,4,2,4,26,4,6);
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 28, 5, 10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet88_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname, string bianhao)
        {
            //添加表8-8的项目部分，其中编号和类型做为参数传入
            obj_sheet.SetValue(row, 3, "条数（条）");
            obj_sheet.SetValue(row + 1, 3, "长度（km）");
            obj_sheet.AddSpanCell(row, 2, 2, 1);
            obj_sheet.SetValue(row, 2, "架空线路");

            obj_sheet.SetValue(row + 2, 3, "条数（条）");
            obj_sheet.SetValue(row + 3, 3, "长度（km）");
            obj_sheet.AddSpanCell(row + 2, 2, 2, 1);
            obj_sheet.SetValue(row + 2, 2, "电缆线路");       
            obj_sheet.AddSpanCell(row, 1, 4, 1);
            obj_sheet.SetValue(row, 1, itemname);
            obj_sheet.AddSpanCell(row, 0, 4, 1);
            obj_sheet.SetValue(row, 0, bianhao);
        }
        private void Sheet88_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 4; i < 10; i++)
            {
                //写入架空线条数
                string xltiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-line' and  DQ='" + DQ + "' and (LineInfo  like '10@%'  or LineInfo  like '20@%' or LineInfo like '6@%')";
                double JKXsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum3", xltiaojian) != null)
                {
                    JKXsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", xltiaojian);
                }
                obj_sheet.SetValue(row, i, JKXsum);
                //写入架空线长度
                double JKXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", xltiaojian) != null)
                {
                    JKXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", xltiaojian);
                }
                obj_sheet.SetValue(row + 1, i, JKXlength);
                //写入电缆线条数
                double DLXsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", xltiaojian) != null)
                {
                    DLXsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", xltiaojian);
                }
                obj_sheet.SetValue(row + 2, i, DLXsum);
                //写入电缆线长度
                double DLXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", xltiaojian) != null)
                {
                    DLXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", xltiaojian);
                }
                obj_sheet.SetValue(row + 3, i, DLXlength);
            }
        }
        private void Build_Sheet88_51(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //查询AreaName的市辖供电区条件
            string SXtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ='市辖供电区' " + " and Col3='新建' and Col4='pw-line' and (LineInfo  like '10@%'  or LineInfo  like '20@%' or LineInfo like '6@%') Group by AreaName";
            //查询AreaName的县级供电区条件
            string XJtiaojian = " ProjectID='"+ProjID+"' and  BuildEd between 2010 and 2015 and AreaName!='' and DQ!='市辖供电区' " + " and Col3='新建' and Col4='pw-line' and (LineInfo  like '10@%'  or LineInfo  like '20@%' or LineInfo like '6@%') Group by AreaName";
            IList SXarea = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            IList XJarea = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录市辖分区数
            int SXsum = SXarea.Count;
            //记录县级分区数
            int XJsum = XJarea.Count;
            //11列
            //每增加一个分区相应的增加4行
            rowcount = 2 + (SXsum + XJsum + 2) * 4;
            colcount = 11;
            title = "附表51  铜陵市低压配电网新增线路工程量估算";
            sheetname = "表8-8 附表51";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 85;
            obj_sheet.Columns[1].Width = 85;
            obj_sheet.Columns[2].Width = 85;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[10].Width = 95;
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, i + 2006 + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            int startrow = 2;
            //int j = 0;
            int length = 4;
           
            //写入市辖供电区合计部分
            Sheet88_51AddItems(obj_sheet, startrow, "合计");
            if (SXsum != 0)
            {
                //循环写入市辖供电区分区名和相应的项目部分
                for (int m = 0; m < SXsum; m++)
                {
                    Sheet88_51AddItems(obj_sheet, startrow + (m + 1) * length, SXarea[m].ToString());
                    Sheet88_51AddData(obj_sheet, startrow + (m + 1) * length, SXarea[m].ToString(), "市辖供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + 1 * length, 4, SXsum, length, 2, 4, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, 2, 4, length, 6);
            }
            //合并市辖部分，写入分区类型
            obj_sheet.AddSpanCell(2, 0, (1 + SXsum) * length, 1);
            obj_sheet.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计部分
            Sheet88_51AddItems(obj_sheet, startrow + (1 + SXsum) * length, "合计");
            if (XJsum != 0)
            {
                //循环写入县级供电区分区名和相应的项目部分
                for (int m = 0; m < XJsum; m++)
                {
                    Sheet88_51AddItems(obj_sheet, startrow + (SXsum + m + 2) * length, XJarea[m].ToString());
                    Sheet88_51AddData(obj_sheet, startrow + (SXsum + m + 2) * length, XJarea[m].ToString(), "县级供电区");
                }

                //写入计算合计部分公式
                fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (SXsum + 2) * length, 4, XJsum, length, startrow + (1 + SXsum) * length, 4, 6);
            }
            else
            {
                //直接在合计部分写入0值
                fc.Sheet_WriteZero(obj_sheet, startrow + (1 + SXsum) * length, 4, length, 6);
            }
            //合并县级部分，写入分区类型
            obj_sheet.AddSpanCell(startrow + (1 + SXsum) * length, 0, (1 + XJsum) * length, 1);
            obj_sheet.SetValue(startrow + (1 + SXsum) * length, 0, "县级供电区");
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, (2 + XJsum + SXsum) * length, 5, 10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet88_51AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string itemname)
        {
            //添加表8-8附表51的项目部分，其中分区名称做为参数传入
            obj_sheet.SetValue(row, 3, "条数（条）");
            obj_sheet.SetValue(row + 1, 3, "长度（km）");
            obj_sheet.AddSpanCell(row, 2, 2, 1);
            obj_sheet.SetValue(row, 2, "架空线路");

            obj_sheet.SetValue(row + 2, 3, "条数（条）");
            obj_sheet.SetValue(row + 3, 3, "长度（km）");
            obj_sheet.AddSpanCell(row + 2, 2, 2, 1);
            obj_sheet.SetValue(row + 2, 2, "电缆线路");
            obj_sheet.AddSpanCell(row, 1, 4, 1);
            obj_sheet.SetValue(row, 1, itemname);

        }
        private void Sheet88_51AddData(FarPoint.Win.Spread.SheetView obj_sheet, int row, string areaname, string DQ)
        {
            //用年份部分列数做外循环控制
            for (int i = 4; i < 10; i++)
            {
                //线路类条件
                string xltiaojian = "";
                if (DQ == "市辖供电区")
                {
                    //市辖供电区
                    xltiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-line' and  DQ='市辖供电区' and  AreaName='" + areaname + "' and (LineInfo  like '10@%'  or LineInfo  like '20@%' or LineInfo like '6@%')";
                }
                else
                {
                    //县级供电区分多种所以 DQ!='市辖供电区'
                    xltiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='pw-line' and  DQ!='市辖供电区' and  AreaName='" + areaname + "' and (LineInfo  like '10@%'  or LineInfo  like '20@%' or LineInfo like '6@%')";
                }
                //写入架空线条数
                double JKXsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum3", xltiaojian) != null)
                {
                    JKXsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", xltiaojian);
                }
                obj_sheet.SetValue(row, i, JKXsum);
                //写入架空线长度
                double JKXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", xltiaojian) != null)
                {
                    JKXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", xltiaojian);
                }
                obj_sheet.SetValue(row + 1, i, JKXlength);
                //写入电缆线条数
                double DLXsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", xltiaojian) != null)
                {
                    DLXsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", xltiaojian);
                }
                obj_sheet.SetValue(row + 2, i, DLXsum);
                //写入电缆线长度
                double DLXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", xltiaojian) != null)
                {
                    DLXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", xltiaojian);
                }
                obj_sheet.SetValue(row + 3, i, DLXlength);

            }
        }
        private void Build_Sheet89(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //42行11列
            rowcount = 42;
            colcount = 11;
            title = "表8-9  铜陵市市辖供电区中低压配电网新扩建工程分类统计";
            sheetname = "表8-9";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 110;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[2].Width = 65;
            obj_sheet.Columns[3].Width = 135;
            
            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "工程类型");
            obj_sheet.SetValue(1, 1, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, (i+2006) + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            obj_sheet.Columns[10].Width = 95;
            //除表头部分开始行号
            int startrow = 2;
            //中压线路 或者 低压网配套 所占行数
            int length1 = 2;
            int j1 = 0;
            //中压配电 或者 中压开关 所占行数
            int length2 = 4;
            int j2 = 0;
            int oldstartrow = 0;
            //写入变电站配套送出部分
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "低压网配套");
            j1++;
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "变电站配套送出");

            //配电网切改
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "配电网切改");
            //架空线入地
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "架空线入地");

            //其他类别
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            Sheet89_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "低压网配套");
            j1++;
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "其他类别");

            Sheet89_RefreshData(obj_sheet);
            //写入求和公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 40, 5, 10);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            //解锁并设定格式
            Sheet89_SetCellType(obj_sheet);
        }
        private void Sheet89_SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //解开用户手工输入部分并设定格式
            for (int i = 4; i < 10; i++)
            {
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 12, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 13, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 40, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 41, i);
            }
        }
        private void Sheet89_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string liebie)
        {
            //类别有四种，分别是“中压线路”，“中压配电”，“中压开关”和“低压网配套”
            if (liebie == "中压线路")
            {

                obj_sheet.SetValue(row, 3, "架空长度（km）");
                obj_sheet.SetValue(row + 1, 3, "电缆长度（km）");
                obj_sheet.AddSpanCell(row, 2, 2, 1);
                obj_sheet.SetValue(row, 2, liebie);
              
            }
            if (liebie == "中压配电")
            {

                obj_sheet.SetValue(row, 3, " 配电室（座）");
                obj_sheet.SetValue(row + 1, 3, "箱变（座）");
                obj_sheet.SetValue(row + 2, 3, "柱上变（座）");
                obj_sheet.SetValue(row + 3, 3, "无功补偿容量（Mvar）");
                obj_sheet.AddSpanCell(row, 2, 4, 1);
                obj_sheet.SetValue(row, 2, "中压配电");
                
            }
            if (liebie == "中压开关")
            {

                obj_sheet.SetValue(row, 3, " 开闭站（座）");
                obj_sheet.SetValue(row + 1, 3, "环网柜（座）");
                obj_sheet.SetValue(row + 2, 3, "柱上开关（台）");
                obj_sheet.SetValue(row + 3, 3, "电缆分支箱（座）");
                obj_sheet.AddSpanCell(row, 2, 4, 1);
                obj_sheet.SetValue(row, 2, "中压开关");
              
            }
            if (liebie == "低压网配套")
            {

                obj_sheet.SetValue(row, 3, "架空线路长度（km）");
                obj_sheet.SetValue(row + 1, 3, "电缆线路长度（km）");
                obj_sheet.AddSpanCell(row, 1, 2, 2);
                obj_sheet.SetValue(row, 1, liebie);
               
            }

        }
        private void Sheet89_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int rowstart, int countrow)
        {
            //根据表8-9特点，把项目部分做为一个突破口，对应“中压线路”“中压配电”和“中压开关”三个基本部分
            //低压网配套部分不用记算
            //用行做外循环控制，列（年份）做内循环控制，取工程类型（合并单元格有空能值为空，调用Sheet_find_notnull方法），
            for (int row = rowstart; row < countrow; row++)
            {
                string tiaojian = "";
                //记录工程序类别
                string type = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                for (int i = 4; i < 10; i++)
                {
                    //判断是否有数据，如果没有那么对应低压网配套
                    if (obj_sheet.Cells[row, 2].Value != null)
                    {
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压线路")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2006 + i) + "' and (Col3='新建' or Col3='扩建') and Col4='pw-line' and  ProgType='" + type + "' and LineInfo  like '10@%'";
                            //向表写入架空线长
                            double JKlenth1 = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian) != null)
                            {
                                JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, JKlenth1);
                            //向表写入电缆线长度
                            double DLXlenth1 = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian) != null)
                            {
                                DLXlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, DLXlenth1);
                        }
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压配电")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='pw-pb' and  ProgType='" + type + "' and BianInfo  like '10@%'";
                            //向表写入配电室（座）
                            double PDSsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                            {
                                PDSsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, PDSsum);
                            //向表写入箱变（座）
                            double XBsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                            {
                                XBsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, XBsum);
                            //向表写入柱上变（座）
                            double ZSBsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                            {
                                ZSBsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                            }
                            obj_sheet.SetValue(row + 2, i, ZSBsum);
                            //向表写入无功补偿容量（Mvar）
                            double WGBCsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", tiaojian) != null)
                            {
                                WGBCsum = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", tiaojian);
                            }
                            obj_sheet.SetValue(row + 3, i, WGBCsum);
                        }
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压开关")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='pw-kg' and  ProgType='" + type + "' and BianInfo  like '10@%'";
                            //向表写入开闭站（座）
                            double KBZsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                            {
                                KBZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, KBZsum);
                            //向表写入环网柜（座）
                            double HWGsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) != null)
                            {
                                HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, HWGsum);
                            //向表写入柱上开关（台）
                            double ZSKGsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                            {
                                ZSKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                            }
                            obj_sheet.SetValue(row + 2, i, ZSKGsum);
                            //向表写入电缆分支箱（座）
                            double DLFZsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian) != null)
                            {
                                DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian);
                            }
                            obj_sheet.SetValue(row + 3, i, DLFZsum);
                        }

                    }
                }
                //如果是“中压线路”每个对应二行内容，如果是“中压配电”每个对应四行内容，如果是”中压开关“每个对应四行内容，如果是”null"表示低压网配套，每个对应二行内容，
                //循环本身再加1，指向下一个内容
                if (obj_sheet.Cells[row, 2].Value != null)
                {
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "中压配电" || obj_sheet.Cells[row, 2].Value.ToString() == "中压开关")
                    {
                        row = row + 3;
                    }
                    else
                    {
                        row = row + 1;
                    }
                }
                else
                {
                    row = row + 1;
                }

            }
        }
        private void Sheet89_RefreshData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //表89为固定格式表，其中有一部分是人工输入部分
            //更新数据时保留人工输入部分，只用此方法更新其它的数据部分
            //写入数据
            Sheet89_AddData(obj_sheet, 2, 42);
            
        }
        private void Build_Sheet810(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //66行11列
            rowcount = 66;
            colcount = 11;
            title = "表8-10  铜陵市县级供电区中低压配电网新扩建工程分类统计";
            sheetname = "表8-10";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 110;
            obj_sheet.Columns[1].Width = 100;
            obj_sheet.Columns[2].Width = 65;
            obj_sheet.Columns[3].Width = 135;

            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "工程类型");
            obj_sheet.SetValue(1, 1, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, (i + 2006) + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            obj_sheet.Columns[10].Width = 95;
            //除表头部分开始行号
            int startrow = 2;
            //中压线路 或者 低压网配套 所占行数
            int length1 = 2;
            int j1 = 0;
            //中压配电 或者 中压开关 所占行数
            int length2 = 4;
            int j2 = 0;
            int oldstartrow = 0;
            //写入变电站配套送出部分
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "低压网配套");
            j1++;
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "变电站配套送出");

            //配电网切改
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "配电网切改");
            //架空线入地
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "架空线入地");
            //新农村电气化
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "低压网配套");
            j1++;
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "新农村电气化");

            //无电地区电力建设
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "低压网配套");
            j1++;
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "无电地区电力建设");

            //其他类别
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            Sheet810_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "低压网配套");
            j1++;
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "其他类别");

            Sheet810_RefreshData(obj_sheet);
            //写入十二合计求和公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 64, 5, 10);

            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            Sheet810_SetCellType(obj_sheet);
        }
        private void Sheet810_SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //解开用户手工输入部分并设定格式
            for (int i = 4; i < 10; i++)
            {
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 12, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 13, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 40, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 41, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 52, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 53, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 64, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 65, i);
            }
        }
        private void Sheet810_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string liebie)
        {
            //类别有四种，分别是“中压线路”，“中压配电”，“中压开关”和“低压网配套”
            if (liebie == "中压线路")
            {

                obj_sheet.SetValue(row, 3, "架空长度（km）");
                obj_sheet.SetValue(row + 1, 3, "电缆长度（km）");
                obj_sheet.AddSpanCell(row, 2, 2, 1);
                obj_sheet.SetValue(row, 2, liebie);

            }
            if (liebie == "中压配电")
            {

                obj_sheet.SetValue(row, 3, " 配电室（座）");
                obj_sheet.SetValue(row + 1, 3, "箱变（座）");
                obj_sheet.SetValue(row + 2, 3, "柱上变（座）");
                obj_sheet.SetValue(row + 3, 3, "无功补偿容量（Mvar）");
                obj_sheet.AddSpanCell(row, 2, 4, 1);
                obj_sheet.SetValue(row, 2, "中压配电");

            }
            if (liebie == "中压开关")
            {

                obj_sheet.SetValue(row, 3, " 开闭站（座）");
                obj_sheet.SetValue(row + 1, 3, "环网柜（座）");
                obj_sheet.SetValue(row + 2, 3, "柱上开关（台）");
                obj_sheet.SetValue(row + 3, 3, "电缆分支箱（座）");
                obj_sheet.AddSpanCell(row, 2, 4, 1);
                obj_sheet.SetValue(row, 2, "中压开关");

            }
            if (liebie == "低压网配套")
            {

                obj_sheet.SetValue(row, 3, "架空线路长度（km）");
                obj_sheet.SetValue(row + 1, 3, "电缆线路长度（km）");
                obj_sheet.AddSpanCell(row, 1, 2, 2);
                obj_sheet.SetValue(row, 1, liebie);

            }

        }
        private void Sheet810_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int rowstart, int countrow)
        {
            //根据表8-10特点，把项目部分做为一个突破口，对应“中压线路”“中压配电”和“中压开关”三个基本部分
            //低压网配套部分不用记算
            //用行做外循环控制，列（年份）做内循环控制，取工程类型（合并单元格有空能值为空，调用Sheet_find_notnull方法），
            for (int row = rowstart; row < countrow; row++)
            {
                string tiaojian = "";
                //记录工程序类别
                string type = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                for (int i = 4; i < 10; i++)
                {
                    //判断是否有数据，如果没有那么对应低压网配套
                    if (obj_sheet.Cells[row, 2].Value != null)
                    {
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压线路")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2006 + i) + "' and (Col3='新建' or Col3='扩建') and Col4='pw-line' and  ProgType='" + type + "' and LineInfo  like '10@%'";
                            //向表写入架空线长
                            double JKlenth1 = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian) != null)
                            {
                                JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, JKlenth1);
                            //向表写入电缆线长度
                            double DLXlenth1 = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian) != null)
                            {
                                DLXlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, DLXlenth1);
                        }
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压配电")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='pw-pb' and  ProgType='" + type + "' and BianInfo  like '10@%'";
                            //向表写入配电室（座）
                            double PDSsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                            {
                                PDSsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, PDSsum);
                            //向表写入箱变（座）
                            double XBsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                            {
                                XBsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, XBsum);
                            //向表写入柱上变（座）
                            double ZSBsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                            {
                                ZSBsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                            }
                            obj_sheet.SetValue(row + 2, i, ZSBsum);
                            //向表写入无功补偿容量（Mvar）
                            double WGBCsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", tiaojian) != null)
                            {
                                WGBCsum = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", tiaojian);
                            }
                            obj_sheet.SetValue(row + 3, i, WGBCsum);
                        }
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压开关")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='pw-kg' and  ProgType='" + type + "' and BianInfo  like '10@%'";
                            //向表写入开闭站（座）
                            double KBZsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                            {
                                KBZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, KBZsum);
                            //向表写入环网柜（座）
                            double HWGsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) != null)
                            {
                                HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, HWGsum);
                            //向表写入柱上开关（台）
                            double ZSKGsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                            {
                                ZSKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                            }
                            obj_sheet.SetValue(row + 2, i, ZSKGsum);
                            //向表写入电缆分支箱（座）
                            double DLFZsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian) != null)
                            {
                                DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian);
                            }
                            obj_sheet.SetValue(row + 3, i, DLFZsum);
                        }

                    }
                }
                //如果是“中压线路”每个对应二行内容，如果是“中压配电”每个对应四行内容，如果是”中压开关“每个对应四行内容，如果是”null"表示低压网配套，每个对应二行内容，
                //循环本身再加1，指向下一个内容
                if (obj_sheet.Cells[row, 2].Value != null)
                {
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "中压配电" || obj_sheet.Cells[row, 2].Value.ToString() == "中压开关")
                    {
                        row = row + 3;
                    }
                    else
                    {
                        row = row + 1;
                    }
                }
                else
                {
                    row = row + 1;
                }

            }
        }
        private void Sheet810_RefreshData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //表89为固定格式表，其中有一部分是人工输入部分
            //更新数据时保留人工输入部分，只用此方法更新其它的数据部分
            //写入数据
            Sheet89_AddData(obj_sheet, 2, 66);
            
        }
        private void Build_Sheet811(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //30行11列
            rowcount = 30;
            colcount = 11;
            title = "表8-11 铜陵市中低压配电网改造工程分类规模";
            sheetname = "表8-11";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定有格线和文本居中
            fc.Sheet_GridandCenter(obj_sheet);
            //设行列模式为R1C1
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 110;
            obj_sheet.Columns[1].Width = 95;
            obj_sheet.Columns[2].Width = 80;
            obj_sheet.Columns[3].Width = 135;

            //向表中写入相关的标题
            obj_sheet.SetValue(1, 0, "区域");
            obj_sheet.SetValue(1, 1, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "项   目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                obj_sheet.SetValue(1, i, (i + 2006) + "年");
            }
            obj_sheet.SetValue(1, 10, "“十二五”合计");
            obj_sheet.Columns[10].Width = 95;

            //写入市辖供电区部分
            Sheet811_AddItems(obj_sheet, 2, "市辖供电区");

            //写入县级供电区部分
            Sheet811_AddItems(obj_sheet, 16, "县级供电区");

            Sheet811_RefreshData(obj_sheet);

            //写入十二五合计求和公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 28, 5, 10);

            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            Sheet811_SetCellType(obj_sheet);
           
        }
        private void Sheet811_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string quyu)
        {
            obj_sheet.SetValue(row, 3, "台数（台）");
            obj_sheet.SetValue(row + 1, 3, "容量（MVA）");
            obj_sheet.AddSpanCell(row, 2, 2, 1);
            obj_sheet.SetValue(row, 2, "高损配变");

            obj_sheet.SetValue(row + 2, 3, "台数（台）");
            obj_sheet.SetValue(row + 3, 3, "容量（MVA）");
            obj_sheet.AddSpanCell(row + 2, 2, 2, 1);
            obj_sheet.SetValue(row + 2, 2, "无功补偿装置");

            obj_sheet.SetValue(row + 4, 3, "断路器（台）");
            obj_sheet.SetValue(row + 5, 3, "负荷开关（台）");
            obj_sheet.SetValue(row + 6, 3, "环网柜（座）");
            obj_sheet.SetValue(row + 7, 3, "电缆分支箱（座）");
            obj_sheet.AddSpanCell(row + 4, 2, 4, 1);
            obj_sheet.SetValue(row + 4, 2, "开关类");

            obj_sheet.SetValue(row + 8, 3, "长度（km）");
            obj_sheet.SetValue(row + 9, 3, "杆塔（基）");
            obj_sheet.AddSpanCell(row + 8, 2, 2, 1);
            obj_sheet.SetValue(row + 8, 2, "架空线路");

            obj_sheet.SetValue(row + 10, 3, "长度（km）");
            obj_sheet.SetValue(row + 11, 3, "沟道（km）");
            obj_sheet.AddSpanCell(row + 10, 2, 2, 1);
            obj_sheet.SetValue(row + 10, 2, "电缆线路");

            obj_sheet.AddSpanCell(row,1, 12, 1);
            obj_sheet.SetValue(row, 1, "10");



            obj_sheet.SetValue(row + 12, 3, "架空线路长度（km）");
            obj_sheet.SetValue(row + 13, 3, "电缆线路长度（km）");
            obj_sheet.AddSpanCell(row + 12, 1, 2, 2);
            obj_sheet.SetValue(row + 12, 1, "低压线路");

            obj_sheet.AddSpanCell(row, 0, 14, 1);
            obj_sheet.SetValue(row,0, quyu);



        }
        private void Sheet811_AddData(FarPoint.Win.Spread.SheetView obj_sheet,int row,string quyu)
        {
            string kgtiaojian = "";
            string linetiaojian = "";
            string dqtiaojian = "";
            if (quyu == "市辖供电区")
            {
                dqtiaojian = " and DQ='市辖供电区'";
            }
            else
            {
                dqtiaojian = " and DQ!='市辖供电区'";
            }
            for (int col = 4; col < 10; col++)
            {
                kgtiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2006+col) + "' and Col3='改造'  and Col4='pw-kg' and BianInfo like '10@%'"+dqtiaojian;
                //写入断路器台数
                double DLQsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", kgtiaojian) != null)
                {
                    DLQsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", kgtiaojian);
                }
                obj_sheet.SetValue(row+4, col, DLQsum);
                //写入负荷开关（台）
                double FHKGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", kgtiaojian) != null)
                {
                    FHKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", kgtiaojian);
                }
                obj_sheet.SetValue(row + 5, col, FHKGsum);
                //写入环网柜（座）
                double HWGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", kgtiaojian) != null)
                {
                    HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", kgtiaojian);
                }
                obj_sheet.SetValue(row + 6, col, HWGsum);
                //写入电缆分支箱（座）
                double DLFZsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", kgtiaojian) != null)
                {
                    DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", kgtiaojian);
                }
                obj_sheet.SetValue(row + 7, col, DLFZsum);

                linetiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2006 + col) + "' and Col3='改造' and Col4='pw-line' and LineInfo like '10@%'" + dqtiaojian;
                //写入架空线路 长度
                double JKXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", linetiaojian) != null)
                {
                    JKXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", linetiaojian);
                }
                obj_sheet.SetValue(row + 8, col, JKXlength);
                //写入架空线路 杆塔（基）
                double GTJlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", linetiaojian) != null)
                {
                    GTJlength = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", linetiaojian);
                }
                obj_sheet.SetValue(row + 9, col, GTJlength);
                //写入电缆线路 长度
                double DLXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", linetiaojian) != null)
                {
                    DLXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", linetiaojian);
                }
                obj_sheet.SetValue(row + 10, col, DLXlength);
                //写入电缆线路 沟道（km）
                double DLGDlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", linetiaojian) != null)
                {
                    DLGDlength = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", linetiaojian);
                }
                obj_sheet.SetValue(row + 11, col, DLGDlength);

            }
        }
        private void Sheet811_RefreshData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //表8-11为固定格式表，其中有一部分是人工输入部分
            //更新数据时保留人工输入部分，只用此方法更新其它的数据部分
            //写入数据
            //写入市辖供电区部分数据
            Sheet811_AddData(obj_sheet, 2, "市辖供电区");
            //写入县级供电区部分数据
            Sheet811_AddData(obj_sheet, 16, "县级供电区");


        }
        private void Sheet811_GetDatafrom8346(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //从8-3附表46中获取添入表8-11的高损配变和无功补偿
            for (int col = 5; col < 11; col++)
            {
                //市辖部分合计
                obj_sheet.SetValue(2, col - 1, fpSpread1.Sheets[4].Cells[2, col].Value);
                obj_sheet.SetValue(3, col - 1, fpSpread1.Sheets[4].Cells[3, col].Value);
                obj_sheet.SetValue(4, col - 1, fpSpread1.Sheets[4].Cells[4, col].Value);
                obj_sheet.SetValue(5, col - 1, fpSpread1.Sheets[4].Cells[5, col].Value);
                //县级部分合计
                obj_sheet.SetValue(16, col - 1, fpSpread1.Sheets[4].Cells[XJDQHEJI8346Row, col].Value);
                obj_sheet.SetValue(17, col - 1, fpSpread1.Sheets[4].Cells[XJDQHEJI8346Row + 1, col].Value);
                obj_sheet.SetValue(18, col - 1, fpSpread1.Sheets[4].Cells[XJDQHEJI8346Row + 2, col].Value);
                obj_sheet.SetValue(19, col - 1, fpSpread1.Sheets[4].Cells[XJDQHEJI8346Row + 3, col].Value);
            }

        }
        private void Sheet811_SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //解开用户手工输入部分并设定格式
            for (int i = 4; i < 10; i++)
            {
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 14, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 15, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 28, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 29, i);
            }
        }
       
        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            WaitDialogForm wait = null;
            wait = new WaitDialogForm("", "正在保存数据, 请稍候...");
            //先从8-3附表中获得数据填入8-3和8-11
            Sheet83_GetDatafrom8346(fpSpread1.Sheets[3]);
            Sheet811_GetDatafrom8346(fpSpread1.Sheets[17]);
            //先从8-7附表中获得数据填入8-7
            Sheet87_GetDatafrom8750(fpSpread1.Sheets[11]);
           

             //先从8-3附表中获得数据填入8-3
            //判断文件夹xls是否存在，不存在则创建
            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\xls"))
            {
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\xls");
            }
            try
            {
                //保存excel文件
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\ZDYGH.xls");
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
                string filename = System.Windows.Forms.Application.StartupPath + "\\xls\\ZDYGH.xls";
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

            //清空表8-1
            fpSpread1.Sheets[0].RowCount = 0;
            fpSpread1.Sheets[0].ColumnCount = 0;
            //重建表8-1
            Build_Sheet81(fpSpread1.Sheets[0]);
            //清空表8-2
            fpSpread1.Sheets[1].RowCount = 0;
            fpSpread1.Sheets[1].ColumnCount = 0;
            //重建表8-2
            Build_Sheet82(fpSpread1.Sheets[1]);
            //清空表8-2附表45
            fpSpread1.Sheets[2].RowCount = 0;
            fpSpread1.Sheets[2].ColumnCount = 0;
            //重建表8-2附表45
            Build_Sheet82_45(fpSpread1.Sheets[2]);
            //重新获取8-3数据 在8-3附表46完成后再更新
            //保存8-3附表中的手写数据
            Sheet83_46SaveData(fpSpread1.Sheets[4]);
            //行列数据清0
            fpSpread1.Sheets[4].RowCount = 0;
            fpSpread1.Sheets[4].ColumnCount = 0;
            //重新生成8-3附表
            Build_Sheet83_46(fpSpread1.Sheets[4]);
            //写回原有的手写数据
            Sheet83_46WriteData(fpSpread1.Sheets[4]);
            //从8-3附表中获得数据填入8-3
            Sheet83_GetDatafrom8346(fpSpread1.Sheets[3]);

            //清空表8-4
            fpSpread1.Sheets[5].RowCount = 0;
            fpSpread1.Sheets[5].ColumnCount = 0;
            //重建表8-4
            Build_Sheet84(fpSpread1.Sheets[5]);
            //清空表8-4附表47
            fpSpread1.Sheets[6].RowCount = 0;
            fpSpread1.Sheets[6].ColumnCount = 0;
            //重建表8-4附表47
            Build_Sheet84_47(fpSpread1.Sheets[6]);
            //清空表8-5
            fpSpread1.Sheets[7].RowCount = 0;
            fpSpread1.Sheets[7].ColumnCount = 0;
            //重建表8-5
            Build_Sheet85(fpSpread1.Sheets[7]);    
            //清空表8-5附表48
            fpSpread1.Sheets[8].RowCount = 0;
            fpSpread1.Sheets[8].ColumnCount = 0;
            //重建表8-5附表48
            Build_Sheet85_48(fpSpread1.Sheets[8]);
            //清空表8-6
            fpSpread1.Sheets[9].RowCount = 0;
            fpSpread1.Sheets[9].ColumnCount = 0;
            //重建表8-6
            Build_Sheet86(fpSpread1.Sheets[9]);
            //清空表8-6附表49
            fpSpread1.Sheets[10].RowCount = 0;
            fpSpread1.Sheets[10].ColumnCount = 0;
            //重建表8-6附表49
            Build_Sheet86_49(fpSpread1.Sheets[10]);

            //保存8-7附表中的手写数据
            Sheet87_50SaveData(fpSpread1.Sheets[12]);
            //行列数据清0
            fpSpread1.Sheets[12].RowCount = 0;
            fpSpread1.Sheets[12].ColumnCount = 0;
            //重新生成8-7附表
            Build_Sheet87_50(fpSpread1.Sheets[12]);
            //写回原有的手写数据
            Sheet87_50WriteData(fpSpread1.Sheets[12]);
            //从8-7附表中获得数据填入8-7
            Sheet87_GetDatafrom8750(fpSpread1.Sheets[11]);

            //清空表8-8
            fpSpread1.Sheets[13].RowCount = 0;
            fpSpread1.Sheets[13].ColumnCount = 0;
            //重建表8-8
            Build_Sheet88(fpSpread1.Sheets[13]);
            //清空表8-8附表51
            fpSpread1.Sheets[14].RowCount = 0;
            fpSpread1.Sheets[14].ColumnCount = 0;
            //重建表8-8附表51
            Build_Sheet88_51(fpSpread1.Sheets[14]);
            //更新8-9
            Sheet89_RefreshData(fpSpread1.Sheets[15]);
            //更新8-10
            Sheet810_RefreshData(fpSpread1.Sheets[16]);

            //先从8-3附表中获得数据填入8-11
            Sheet811_GetDatafrom8346(fpSpread1.Sheets[17]);
            Sheet811_RefreshData(fpSpread1.Sheets[17]);

            //移除空表
            fpSpread1.Sheets.Remove(activesheet);
            //还原当前表
            fpSpread1.ActiveSheet = obj_sheet;

            //设文本自动换行
            fc.Sheet_Colautoenter(fpSpread1);
            newwait.Close();
            MessageBox.Show("更新数据完成！");

        }
        private void barBtnDaochuExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            //先从8-3附表中获得数据填入8-3  和8-11
            Sheet83_GetDatafrom8346(fpSpread1.Sheets[3]);
            Sheet811_GetDatafrom8346(fpSpread1.Sheets[17]);
            //先从8-7附表中获得数据填入8-7
            Sheet87_GetDatafrom8750(fpSpread1.Sheets[11]);
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
        private void barBtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

    
    
    }
}
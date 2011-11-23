using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet315
    {

        fpcommon fc = new fpcommon();
        //工作表行数
        int rowcount = 0;
        //工作表列数据
        int colcount = 0;
        //工作表第一行的表名
        string title = "";
        //工作表标签名
        string sheetname = "";
        public void Build(FarPoint.Win.Spread.FpSpread fpspread, FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName)
        {
            //线路电压等级根据实际情况查询(中压 6 <= 电压<=20)
            string XLDianYatiaojian = " Type='05' and OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and ProjectID='" + ProjID + "' and (LineType2='公用' or LineType2='专用') and RateVolt  between 6 and 20 group by RateVolt order by RateVolt";
            IList<double> DY_list = Services.BaseService.GetList<double>("SelectPSPDEV_RateVolt", XLDianYatiaojian);
            int xlsum = DY_list.Count;
            //表标题行数
            int startrow = 3;
            //列标题每项行数
            int dylength = xlsum;

            //表格共 行21 列
            rowcount = startrow + 7 * dylength;
            colcount = 21;
            //工作表第一行的标题
            title = "表3‑15  " + year + "年铜陵市中压配电线路统计";
            //工作表名
            sheetname = "表3-15";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 60;
            obj_sheet.Columns[2].Width = 60;
            obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 60;
            obj_sheet.Columns[5].Width = 60;
            obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 60;
            obj_sheet.Columns[8].Width = 60;
            obj_sheet.Columns[9].Width = 120;
            obj_sheet.Columns[10].Width = 60;
            obj_sheet.Columns[11].Width = 120;
            obj_sheet.Columns[12].Width = 90;
            obj_sheet.Columns[13].Width = 90;
            obj_sheet.Columns[14].Width = 60;
            obj_sheet.Columns[15].Width = 60;
            obj_sheet.Columns[16].Width = 60;
            obj_sheet.Columns[17].Width = 60;
            obj_sheet.Columns[18].Width = 90;
            obj_sheet.Columns[19].Width = 80;
            obj_sheet.Columns[20].Width = 60;
            //设定表格行高度
            
            obj_sheet.Rows[1].Height = 40;
            obj_sheet.Rows[2].Height = 20;
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SxXjName, DY_list);
            //写入数据
            Sheet_AddData(fpspread,obj_sheet, year, ProjID, SxXjName, DY_list);
            //写入公式
            fc.Sheet_WriteFormula_OneCol_Anotercol_nopercent(obj_sheet, 3, 3, 5, 9, 7 * dylength,2);
            fc.Sheet_WriteFormula_OneCol_Anotercol_nopercent(obj_sheet, 3, 3, 10, 11, 7 * dylength,2);
            fc.Sheet_WriteFormula_OneCol_Twocol_Threecol_percent(obj_sheet, 3, 18, 5, 7, 19, 7 * dylength);
            fc.Sheet_WriteFormula_OneCol_Anotercol_percent(obj_sheet, 3, 5, 7, 20, 7 * dylength);
            //锁定表格
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
             //写标题行内容
            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "编号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "类型");
            obj_sheet.AddSpanCell(1, 2, 2, 1);
            obj_sheet.SetValue(1, 2, "电压等级");
            obj_sheet.AddSpanCell(1, 3, 1, 2);
            obj_sheet.SetValue(1, 3, "线路条数(条)");
            obj_sheet.AddSpanCell(1, 5, 1, 2);
            obj_sheet.SetValue(1, 5, "线路总长度(km)");
            obj_sheet.AddSpanCell(1, 7, 1, 2);
            obj_sheet.SetValue(1, 7, "其中：电缆长度(km)");
            obj_sheet.SetValue(1, 9, "平均线路长度(km)");
            obj_sheet.SetValue(1, 10, "主干线长度(km)");
            obj_sheet.SetValue(1, 11, "主干平均长度(km)");
            obj_sheet.SetValue(1, 12, "主干长度>4km条数(条)");
            obj_sheet.SetValue(1, 13, "主干长度>10km条数(条)");
            obj_sheet.AddSpanCell(1, 14, 1, 2);
            obj_sheet.SetValue(1, 14, "平均单条线路配变装接容量(MVA/条)");
            obj_sheet.AddSpanCell(1, 16, 1, 2);
            obj_sheet.SetValue(1, 16, "线路配变装接容量>12MVA线路条数(条)");
            obj_sheet.SetValue(1, 18, "架空线绝缘导线长度（km）");
            obj_sheet.SetValue(1, 19, "架空线路绝缘化率(%)");
            obj_sheet.SetValue(1, 20, "电缆化率(%)");

            //3行标题内容
            obj_sheet.SetValue(2, 3, "公用");
            obj_sheet.SetValue(2, 4, "专用");
            obj_sheet.SetValue(2, 5, "公用");
            obj_sheet.SetValue(2, 6, "专用");
            obj_sheet.SetValue(2, 7, "公用");
            obj_sheet.SetValue(2, 8, "专用");
            obj_sheet.SetValue(2, 9, "公用");
            obj_sheet.SetValue(2, 10, "公用");
            obj_sheet.SetValue(2, 11, "公用");
            obj_sheet.SetValue(2, 12, "公用");
            obj_sheet.SetValue(2, 13, "公用");
            obj_sheet.SetValue(2, 14, "公用线路");
            obj_sheet.SetValue(2, 15, "专用线路");
            obj_sheet.SetValue(2, 16, "公用线路");
            obj_sheet.SetValue(2, 17, "专用线路");
            obj_sheet.SetValue(2, 18, "公用");
            obj_sheet.SetValue(2, 19, "公用");
            obj_sheet.SetValue(2, 20, "公用");

            //写标题列内容
            fc.Sheet_AddItem_ZBonlyDY(obj_sheet, SxXjName, 3, obj_DY_List);
        }
        private void Sheet_AddData(FarPoint.Win.Spread.FpSpread fpspread,FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID, List<string[]> SxXjName, IList<double> obj_DY_List)
        {
            //其中平均单条线路配变装接容量(MVA/条) 取表3-14的数据计算，所以要选判断电压等级是否一致
            string dyflag = "";
            //查找表3-14市辖供电区的开始行号
            int SXrow = fc.Sheet_Find_Value(fpspread.Sheets[22], 0, "1");
            if (SXrow != -1)
            {
                //查3-14的电压等级数
                int sheet314dysum = fpspread.Sheets[22].Cells[SXrow, 0].RowSpan;
                if (sheet314dysum == obj_DY_List.Count)
                {
                    for (int i = 0; i < sheet314dysum; i++)
                    {
                        //看电压内容是否相同
                        if (fpspread.Sheets[22].Cells[SXrow + i, 2].Value.ToString() != obj_DY_List[i].ToString())
                        {
                            dyflag = "不同";
                        }
                    }
                }
                else
                {
                    //如果电压等级不相同，则标识不同
                    dyflag = "不同";
                }
            }


            //添加数据
            //公用还是专用
            string GorZ = "";
            //条件
            string XLtiaojian = "";
            int startrow = 3;
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
                            //固定两次循环，一次计算公用，一次计算专用
                            for (int k = 0; k <= 1; k++)
                            {
                                if (k == 0)
                                {
                                    GorZ = "公用";
                                    //主干公用线路长度
                                    //电缆线路长度（此处用来计算线路总长度）
                                    XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and ProjectID='" + ProjID + "' and Type='05' and LineType2='" + GorZ + "' and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
                                    double ZGDLlength = 0;
                                    if (Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian) != null)
                                    {
                                        ZGDLlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian);
                                    }

                                    //架空线路长度（此处用来计算线路总长度）
                                    double ZGJKXLlength = 0;
                                    if (Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian) != null)
                                    {
                                        ZGJKXLlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian);
                                    }
                                    obj_sheet.SetValue(startrow + i * length + j, 10, ZGDLlength + ZGJKXLlength);
                                    //主干长度>4km条数(条)
                                    XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and ProjectID='" + ProjID + "' and Type='05' and LineType2='" + GorZ + "' and (LineLength+Length2)>4 and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
                                    int ZGXLsum1 = 0;
                                    if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian) != null)
                                    {
                                        ZGXLsum1 = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian);
                                    }
                                    obj_sheet.SetValue(startrow + i * length + j, 12, ZGXLsum1);
                                    //主干长度>10km条数(条)
                                    XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and ProjectID='" + ProjID + "' and Type='05' and LineType2='" + GorZ + "' and (LineLength+Length2)>10 and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
                                    int ZGXLsum2 = 0;
                                    if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian) != null)
                                    {
                                        ZGXLsum2 = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian);
                                    }
                                    obj_sheet.SetValue(startrow + i * length + j, 13, ZGXLsum2);
                                    //架空线绝缘导线长度（km）
                                    XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and ProjectID='" + ProjID + "' and Type='05' and LineType2='" + GorZ + "' and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
                                    double JKJYlength = 0;
                                    if (Services.BaseService.GetObject("SelectPSPDEV_SUMNum1", XLtiaojian) != null)
                                    {
                                        JKJYlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMNum1", XLtiaojian);
                                    }
                                    obj_sheet.SetValue(startrow + i * length + j, 18, JKJYlength);

                                }
                                else
                                {
                                    GorZ = "专用";
                                }
                                //线路条数
                                XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and ProjectID='" + ProjID + "' and Type='05' and LineType2='" + GorZ + "' and DQ='" + SxXjName[i][2] + "' and RateVolt=" + obj_DY_List[j].ToString();
                                int XLsum = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian) != null)
                                {
                                    XLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * length + j, 3 + k, XLsum);
                                //电缆线路长度
                                double DLXLlength = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian) != null)
                                {
                                    DLXLlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * length + j, 7 + k, DLXLlength);
                                //架空线路长度（此处用来计算线路总长度）
                                double JKXLlength = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian) != null)
                                {
                                    JKXLlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * length + j, 5 + k, DLXLlength + JKXLlength);

                                //平均单条线路配变装接容量(MVA/条) 取表3-14的数据计算,先查看 dyflag的值;
                                //如果电压不同，提示出错，否则写入公式
                                if (dyflag=="不同")
                                {
                                    obj_sheet.SetValue(startrow + i * length + j, 14 + k, "公式错误");
                                }
                                else
                                {
                                    string sheet314name=fpspread.Sheets[22].SheetName;
                                    FarPoint.Win.Spread.CellType.NumberCellType newcelltype = new FarPoint.Win.Spread.CellType.NumberCellType();
                                    //结果保留两位小数
                                    newcelltype.DecimalPlaces = 2;
                                    obj_sheet.Cells[startrow + i * length + j, 14 + k].CellType = newcelltype;
                                    obj_sheet.Cells[startrow + i * length + j, 14 + k].Formula = "'" + sheet314name + "'!R" + (startrow + i * length + j + 1) + "C" + (5 + 2 * k) + "/R" + (startrow + i * length + j + 1) + "C" + (4 + k);
                                }
                               
                                //线路配变装接容量>12MVA线路条数(条)
                                XLtiaojian = " OperationYear!='' and  year(cast(OperationYear as datetime))<=" + year + " and ProjectID='" + ProjID + "' and Type='05' and LineType2='" + GorZ + "' and DQ='" + SxXjName[i][2] + "' and  Burthen*RateVolt>12 and RateVolt=" + obj_DY_List[j].ToString();
                                int RLXLsum = 0;
                                if (Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian) != null)
                                {
                                    RLXLsum = (int)Services.BaseService.GetObject("SelectPSPDEV_CountAll", XLtiaojian);
                                }
                                obj_sheet.SetValue(startrow + i * length + j, 16 + k, RLXLsum);
                            }
                        }
                    }
                    else
                    {
                        //县级合计部分公式
                        if (i == 1)
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow + (i + 1) * length, 3, 4, length, startrow + i * length, 3, 18);
                        }
                        //全地区合计部分公式
                        else
                        {
                            fc.Sheet_WriteFormula_RowSum(obj_sheet, startrow, 3, 2, length, startrow + i * length, 3, 18);

                        }
                    }

                }
            }
        }

    }
}

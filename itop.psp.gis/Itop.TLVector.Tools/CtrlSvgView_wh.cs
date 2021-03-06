using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ItopVector.Core;
using ItopVector.Core.Figure;
using ItopVector.Core.Document;
using DevComponents.DotNetBar;
using System.Drawing.Imaging;
using Itop.Client;
using System.Xml;
using Itop.Domain.Graphics;
using Itop.Client.Common;
using Itop.MapView;
using System.Configuration;
using System.IO;
using ItopVector.Core.Interface.Figure;
using Itop.TLPSP.DEVICE;
using Itop.Domain.RightManager;

namespace ItopVector.Tools {
    public partial class CtrlSvgView_wh : UserControl {

        private DevComponents.DotNetBar.ComboBoxItem LayerBox;
        private DevComponents.DotNetBar.ComboBoxItem scaleBox;
        private string SvgUID = "";
        private string selLar = "";
        private DevComponents.DotNetBar.ToolTip tip;
        private string SelUseArea = "";
        private string LineLen = "";
        ArrayList Layerlist = new ArrayList();
        private string rzb = "1";
        public static string MapType = "接线图";
        frmInfo fInfo = new frmInfo();
        XmlNode img = null;
        string pid = "";
        public bool EditRight = false;
        public string progtype = "";
        private bool LoadImage = true;
        public Itop.MapView.IMapViewObj mapview;
        //public Itop.MapView.IMapViewObj mapview1;
        //public Itop.MapView.IMapViewObj mapview2;
        //public frmLayerManager frmlar = new frmLayerManager();
        public frmLayerTreeManager frmlar = new frmLayerTreeManager();
        int chose = 1;
        double jd = 0;
        double wd = 0;
        int show3d = 0;
        string str_selID = "";
        public CtrlSvgView_wh() {
            //fInfo.Owner = this.FindForm();
            tip = new DevComponents.DotNetBar.ToolTip();
            Pen pen1 = new Pen(Brushes.Cyan, 3);
            InitializeComponent();
            this.dotNetBarManager1.Images = ItopVector.Resource.ResourceHelper.LoadBitmapStrip(base.GetType(), "ItopVector.Tools.ToolbarImages.bmp", new Size(16, 16), new Point(0, 0));
            tlVectorControl1.ScaleChanged += new EventHandler(tlVectorControl1_ScaleChanged);
            tlVectorControl1.RightClick += new ItopVector.DrawArea.SvgElementEventHandler(tlVectorControl1_RightClick);
            tlVectorControl1.LeftClick += new ItopVector.DrawArea.SvgElementEventHandler(tlVectorControl1_LeftClick);
            tlVectorControl1.DoubleLeftClick += new ItopVector.DrawArea.SvgElementEventHandler(tlVectorControl1_DoubleLeftClick);
            //tlVectorControl1.AfterPaintPage += new ItopVector.DrawArea.PaintMapEventHandler(tlVectorControl1_AfterPaintPage);
            ItopVector.SpecialCursors.LoadCursors();
            tlVectorControl1.DrawMode = DrawModeType.MemoryImage;
            tlVectorControl1.TempPen = pen1;
            popupContainerEdit1.Text = selLar;
            tlVectorControl1.CanEdit = true;
            contextMenuStrip1.Enabled = false;
            ButtonEnb(false);
            tlVectorControl1.DrawArea.ViewMargin = new Size(20000, 20000);
            //mapview.ZeroLongLat = new LongLat(117.6787m, 31.0568m);
            tlVectorControl1.ContextMenuStrip = null;
            //lgm 修改
            //jd = Convert.ToDouble(ConfigurationSettings.AppSettings.Get("jd"));
            //wd = Convert.ToDouble(ConfigurationSettings.AppSettings.Get("wd"));
            jd = MIS.JD;
            wd = MIS.WD;

            chose = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("chose"));
            show3d = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("show3d"));
            //mapview.ZeroLongLat = new LongLat(117.6787m, 31.0568m);
            if (show3d == 1) {
                checkEdit1.Visible = true;
            } else if (show3d == 0) {
                checkEdit1.Visible = false;
            }
            //lgm chose取不到值，暂时修改值为1
            if (chose == 0) {
                chose = 1;
            }
            if (chose == 1) {

                //mapview2 = new Itop.MapView.MapViewObj("MapData3d.yap");
                mapview = new Itop.MapView.MapViewObj();
                // mapview = mapview1;
            } else if (chose == 2) {
                mapview = new Itop.MapView.MapViewGoogle();
                (mapview as MapViewGoogle).IsDownMap = true;
            }
            mapview.ZeroLongLat = new LongLat(jd, wd);

        }
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            //tlVectorControl1.ScaleRatio = .4f;
        }
        bool isIn = false;
        System.Drawing.Image backbmp = null;
        void tlVectorControl1_AfterPaintPage(object sender, PaintMapEventArgs e) {
            try {
                if (LoadImage) {
                    int nScale = mapview.Getlevel(tlVectorControl1.ScaleRatio);
                    LongLat longlat = LongLat.Empty;
                    float nn = 1;
                    if (mapview is MapViewObj) {
                        if (isIn)
                            nScale = mapview.Getlevel(tlVectorControl1.ScaleRatio, out nn);
                        else
                            nScale = mapview.Getlevel2(tlVectorControl1.ScaleRatio, out nn);
                    }
                    if (nScale == -1) return;
                    //LongLat longlat = LongLat.Empty;
                    //计算中心点经纬度

                    //int offsetY = (nScale - 10) * 25;

                    longlat = mapview.OffSet(mapview.ZeroLongLat, nScale, (int)(e.CenterPoint.X), (int)(e.CenterPoint.Y));
                    //Color color3 = ColorTranslator.FromHtml("#FFFFFF");
                    //e.G.Clear(color3);
                    //创建地图

                    System.Drawing.Image image = backbmp;
                    if (nn >= 1)
                        image = mapview.CreateMap(e.Bounds.Width, e.Bounds.Height, nScale, longlat.Longitude, longlat.Latitude);
                    else
                        image = mapview.CreateMap((int)(e.Bounds.Width / nn), (int)(e.Bounds.Height / nn), nScale, longlat.Longitude, longlat.Latitude);
                    //backbmp = image

                    //System.Drawing.Image image = mapview.CreateMap(e.Bounds.Width, e.Bounds.Height, nScale, longlat.Longitude, longlat.Latitude);
                    string newnScale = mapview.GetMiles(nScale);
                    ImageAttributes imageAttributes = new ImageAttributes();
                    ColorMatrix matrix1 = new ColorMatrix();
                    matrix1.Matrix00 = 1f;
                    matrix1.Matrix11 = 1f;
                    matrix1.Matrix22 = 1f;
                    matrix1.Matrix33 = 0.9f;//地图透明度
                    matrix1.Matrix44 = 1f;
                    //设置地图透明度
                    //imageAttributes.SetColorMatrix(matrix1, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    Color color = ColorTranslator.FromHtml("#F4F4FB");
                    Color color2 = ColorTranslator.FromHtml("#EFF0F1");//EFF0F1
                    //imageAttributes2.SetColorKey(color, color);
                    //imageAttributes2.SetColorKey(color2, color2);
                    imageAttributes.SetColorKey(color2, color);
                    //绘制地图

                    // e.G.DrawImage((Bitmap)image, e.Bounds, 0f, 0f, (float)image.Width, (float)image.Height, GraphicsUnit.Pixel, imageAttributes);
                    if (nn > 1) {
                        int w1 = (int)(e.Bounds.Width * ((nn - 1) / 2));
                        int h1 = (int)(e.Bounds.Height * ((nn - 1) / 2));

                        Rectangle rt1 = e.Bounds;
                        rt1.Inflate(w1, h1);
                        e.G.CompositingQuality = CompositingQuality.HighQuality;
                        e.G.DrawImage((Bitmap)image, rt1, 0f, 0f, (float)image.Width, (float)image.Height, GraphicsUnit.Pixel, imageAttributes);
                    } else
                        e.G.DrawImage((Bitmap)image, e.Bounds, 0f, 0f, (float)image.Width, (float)image.Height, GraphicsUnit.Pixel, imageAttributes);
                    //  SolidBrush brush = new SolidBrush(Color.FromArgb(220, 75, 75, 75));
                    //绘制中心点
                    //绘制中心点
                    e.G.DrawEllipse(Pens.Red, e.Bounds.Width / 2 - 2, e.Bounds.Height / 2 - 2, 4, 4);
                    e.G.DrawEllipse(Pens.Red, e.Bounds.Width / 2 - 1, e.Bounds.Height / 2 - 1, 2, 2);

                    //绘制比例尺
                    Point p1 = new Point(20, e.Bounds.Height - 30);
                    Point p2 = new Point(20, e.Bounds.Height - 20);
                    Point p3 = new Point(80, e.Bounds.Height - 20);
                    Point p4 = new Point(80, e.Bounds.Height - 30);
                    e.G.DrawLines(new Pen(Color.Black, 2), new Point[4] { p1, p2, p3, p4 });
                    string str1 = string.Format("{0}公里", newnScale);
                    e.G.DrawString(str1, new Font("宋体", 10), Brushes.Black, 30, e.Bounds.Height - 40);

                }
            } catch (Exception e1) {
            }
            //string s = string.Format("{0}行{1}列", nRows, nCols);
            //string s = string.Format("经{0}：纬{1}", longlat.Longitude, longlat.Latitude);
            //显示中心点经纬度
            //e.G.DrawString(s, new Font("宋体", 10), Brushes.Red, 20, 40);
        }
        public void CloseInfo() {
            fInfo.Close();
        }
        public void Init() {
            // progtype = stype;
            ArrayList layerlist = tlVectorControl1.SVGDocument.getLayerList();
            ArrayList tmplaylist = new ArrayList();
            DevExpress.XtraEditors.Controls.CheckedListBoxItem[] chkItems = null;
            this.checkedListBoxControl1.Items.Clear();

            if (progtype == "地理信息层") {
                for (int i = 0; i < layerlist.Count; i++) {
                    Layer lar = (Layer)layerlist[i];
                    if (lar.GetAttribute("layerType") == progtype) {
                        tmplaylist.Add(layerlist[i]);
                    } else {
                        lar.Visible = false;
                    }
                }
            }
            if (progtype == "城市规划层") {
                for (int i = 0; i < layerlist.Count; i++) {
                    Layer lar = (Layer)layerlist[i];
                    if (lar.GetAttribute("layerType") == "城市规划层" || lar.GetAttribute("layerType") == "地理信息层") {
                        tmplaylist.Add(layerlist[i]);
                    } else {
                        lar.Visible = false;
                    }
                }
            }
            if (progtype == "电网规划层") {
                for (int i = 0; i < layerlist.Count; i++) {
                    Layer lar = (Layer)layerlist[i];
                    tmplaylist.Add(layerlist[i]);
                }
            }
            if (MapType == "所内接线图") {
                CreateComboBox();
                ButtonEnb(true);
                LoadImage = false;
                bk1.Visible = false;
                selLar = "";
            }
            chkItems = new DevExpress.XtraEditors.Controls.CheckedListBoxItem[tmplaylist.Count];
            for (int j = 0; j < tmplaylist.Count; j++) {
                chkItems.SetValue(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(((Layer)tmplaylist[j]).Label), j);
                if (((Layer)tmplaylist[j]).Visible) {
                    chkItems[j].CheckState = CheckState.Checked;
                }
            }
            this.checkedListBoxControl1.Items.AddRange(chkItems);

            if (tmplaylist.Count > 0) {
                Layer lar = (Layer)tmplaylist[0];
                SvgDocument.currentLayer = lar.ID;
                popupContainerEdit1.Text = lar.Label;
                selLar = lar.Label;
            }
            tlVectorControl1.FullDrawMode = true;
            int chose = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("chose"));
            if (chose == 1) {
                tlVectorControl1.ScaleRatio = 0.1f;

            }
            if (chose == 2) {
                tlVectorControl1.ScaleRatio = 0.015625f;

            }
            //tlVectorControl1.ScaleRatio = 0.01f;
            tlVectorControl1.CurrentOperation = ToolOperation.Roam;

            //tlVectorControl1.Refresh();
        }
        void tlVectorControl1_DoubleLeftClick(object sender, ItopVector.DrawArea.SvgElementSelectedEventArgs e) {
            XmlElement xml1 = ((XmlElement)(e.Elements[0]));
            string str_name = xml1.GetAttribute("xlink:href");
            if (MapType == "接线图") {
                SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;
                if (str_name.Contains("Substation")) {
                    pid = tlVectorControl1.SVGDocument.SvgdataUid;
                    SVGFILE svg_temp = new SVGFILE();
                    svg_temp.SUID = xml1.GetAttribute("id");
                    IList svglist = Services.BaseService.GetList("SelectSVGFILEByKey", svg_temp);
                    if (svglist.Count > 0) {
                        svg_temp = (SVGFILE)svglist[0];
                        SvgDocument sdoc = new SvgDocument();
                        sdoc.LoadXml(svg_temp.SVGDATA);
                        tlVectorControl1.SVGDocument = sdoc;
                        tlVectorControl1.SVGDocument.SvgdataUid = svg_temp.SUID;
                        MapType = "所内接线图";
                    } else {
                        tlVectorControl1.NewFile();
                        tlVectorControl1.IsPasteGrid = false;
                        tlVectorControl1.IsShowGrid = false;
                        tlVectorControl1.IsShowRule = false;
                        tlVectorControl1.IsShowTip = false;
                        tlVectorControl1.SVGDocument.SvgdataUid = svg_temp.SUID;
                        MapType = "所内接线图";
                    }
                    if (tlVectorControl1.SVGDocument.getLayerList().Count == 0) {
                        Layer _lar = ItopVector.Core.Figure.Layer.CreateNew("接线图", tlVectorControl1.SVGDocument);
                        _lar.SetAttribute("layerType", "所内接线图");
                        _lar.Visible = true;
                        SvgDocument.currentLayer = ((Layer)tlVectorControl1.SVGDocument.getLayerList()[0]).ID;
                    }

                    CreateComboBox();

                    ButtonEnb(true);
                    LoadImage = false;
                    bk1.Visible = false;
                    selLar = "";
                    tlVectorControl1.ScaleRatio = 1f;
                    tlVectorControl1.Refresh();
                }
            }
        }

        void tlVectorControl1_LeftClick(object sender, ItopVector.DrawArea.SvgElementSelectedEventArgs e) {
            //if (tlVectorControl1.ScaleRatio < 0.1f)
            //{
            //    tlVectorControl1.ScaleRatio = 0.01f;
            //    scaleBox.ComboBoxEx.Text = "1%";
            //    //scaleBox.SelectedText = "10%";
            //}
            //SvgElement e1 = null;
            //if (tlVectorControl1.SVGDocument.CurrentElement != null)
            //{
            //     e1 = (SvgElement)tlVectorControl1.SVGDocument.CurrentElement.Clone();

            //}
            ////tlVectorControl1.SVGDocument.CurrentElement = null;
            //tlVectorControl1.SVGDocument.SelectCollection.Clear();

            //tip.Hide();
            fInfo.Hide();
            //tlVectorControl1.SVGDocument.CurrentElement =(SvgElement) e.Elements[0];
            decimal ViewScale = 1;
            string str_Scale = tlVectorControl1.SVGDocument.getViewScale();
            if (str_Scale != "") {
                ViewScale = Convert.ToDecimal(str_Scale);
            }
            if (e.SvgElement.GetType().ToString() == "ItopVector.Core.Figure.Polygon") {
                string IsArea = ((XmlElement)e.SvgElement).GetAttribute("IsArea");
                if (IsArea != "") {

                    PointF[] pnts = ((Polygon)e.SvgElement).Points.Clone() as PointF[];
                    ((Polygon)e.SvgElement).Transform.Matrix.TransformPoints(pnts);
                    decimal temp1 = TLMath.getPolygonArea(pnts, 1);
                    temp1 = TLMath.getNumber2(temp1, tlVectorControl1.ScaleRatio) / Convert.ToDecimal(4.2);
                    SelUseArea = temp1.ToString("#####.####");
                    if (SelUseArea != "") {
                        if (Convert.ToDecimal(SelUseArea) >= 1) {
                            fInfo.Info = "区域面积：" + SelUseArea + "（KM²）";
                        } else {
                            fInfo.Info = "区域面积： 0" + SelUseArea + "（KM²）";
                        }
                        fInfo.Top = e.Mouse.Y;
                        fInfo.Left = e.Mouse.X;
                        fInfo.Show();
                    }
                    //tip.Text = "区域面积：" + SelUseArea;
                    //tip.ShowToolTip();
                }
            }
            if (e.SvgElement.GetType().ToString() == "ItopVector.Core.Figure.Line") {
                string IsLead = ((XmlElement)e.SvgElement).GetAttribute("IsLead");
                if (IsLead != "") {
                    Line line = (Line)e.SvgElement;
                    decimal temp1 = TLMath.getLineLength(line, 1);
                    temp1 = TLMath.getNumber(temp1, tlVectorControl1.ScaleRatio);
                    string len = temp1.ToString("#####.####");
                    LineLen = len;
                    LineInfo lineInfo = new LineInfo();
                    lineInfo.EleID = e.SvgElement.ID;
                    lineInfo.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;
                    LineInfo _lineTemp = (LineInfo)Services.BaseService.GetObject("SelectLineInfoByEleID", lineInfo);

                    if ((len != "") && (_lineTemp != null)) {
                        if (Convert.ToDecimal(len) >= 1) {
                            fInfo.Info = "线路名称：" + _lineTemp.LineName + " 线路长度：" + len + "（KM）\r\n" + "导线型号：" + _lineTemp.LineType + " 电压等级：" + _lineTemp.Voltage + "kV 投运时间：" + _lineTemp.ObligateField3;
                        } else {
                            fInfo.Info = "线路名称：" + _lineTemp.LineName + " 线路长度： 0" + len + "（KM）\r\n" + "导线型号：" + _lineTemp.LineType + " 电压等级：" + _lineTemp.Voltage + "kV 投运时间：" + _lineTemp.ObligateField3;
                        }
                    } else if (len != "") {
                        if (Convert.ToDecimal(len) >= 1) {
                            fInfo.Info = "线路名称：" + " " + "线路长度：" + len + "（KM）\r\n" + "导线型号：" + " " + " 电压等级：" + _lineTemp.Voltage + " 投运时间：" + _lineTemp.ObligateField3;
                        } else {
                            fInfo.Info = "线路名称：" + " " + "线路长度： 0" + len + "（KM）\r\n" + "导线型号：" + " " + " 电压等级：" + _lineTemp.Voltage + " 投运时间：" + _lineTemp.ObligateField3;
                        }
                    }
                    fInfo.Top = e.Mouse.Y;
                    fInfo.Left = e.Mouse.X;
                    fInfo.Width = (fInfo.Info.Length) * 7;
                    fInfo.Height = 50;
                    if (len != "") {
                        fInfo.Show();

                    }

                }
            }
            if (e.SvgElement.GetType().ToString() == "ItopVector.Core.Figure.Polyline") {
                string IsLead = ((XmlElement)e.SvgElement).GetAttribute("IsLead");
                if (IsLead != "") {
                    Polyline polyline = (Polyline)e.SvgElement;
                    double temp1 = 0;
                    for (int i = 1; i < polyline.Points.Length; i++) {
                        temp1 += this.mapview.CountLength(polyline.Points[i - 1], polyline.Points[i]);
                    }
                    string len = temp1.ToString("#####.####");
                    LineLen = len;
                    PSPDEV lineInfo = new PSPDEV();
                    lineInfo.SUID = ((XmlElement)e.SvgElement).GetAttribute("Deviceid");
                    // lineInfo.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;
                    PSPDEV _lineTemp = (PSPDEV)Services.BaseService.GetObject("SelectPSPDEVByKey", lineInfo);

                    if ((len != "") && (_lineTemp != null)) {
                        if (Convert.ToDecimal(len) >= 1) {
                            fInfo.Info = "线路名称：" + _lineTemp.Name + " 线路长度：" + len + "（KM）\r\n" + "导线型号：" + _lineTemp.LineType + " 电压等级：" + _lineTemp.RateVolt.ToString() + "kV 投产年限：" + _lineTemp.OperationYear;
                        } else {
                            fInfo.Info = "线路名称：" + _lineTemp.Name + " 线路长度： 0" + len + "（KM）\r\n" + "导线型号：" + _lineTemp.LineType + " 电压等级：" + _lineTemp.RateVolt.ToString() + "kV 投产年限：" + _lineTemp.OperationYear;
                        }
                    } else if (len != "") {
                        if (Convert.ToDecimal(len) >= 1) {
                            fInfo.Info = "线路名称：" + " " + "线路长度：" + len + "（KM）\r\n" + "导线型号：" + " " + " 电压等级：" + " " + " 投运时间：" + " ";
                        } else {
                            fInfo.Info = "线路名称：" + " " + "线路长度： 0" + len + "（KM）\r\n" + "导线型号：" + " " + " 电压等级：" + " " + " 投运时间：" + " ";
                        }
                    }
                    fInfo.Top = e.Mouse.Y;
                    fInfo.Left = e.Mouse.X;
                    fInfo.Width = (fInfo.Info.Length) * 7;
                    fInfo.Height = 50;
                    //fInfo.Right = fInfo.Left+fInfo.Info.Length*10;
                    if (len != "") {
                        fInfo.Show();
                    }
                }
            }
            if (e.SvgElement.GetType().ToString() == "ItopVector.Core.Figure.Use") {
                string aaa = ((Use)e.SvgElement).RefElement.ID;
                //if (!aaa.Contains("Substation"))
                //{
                //    return;
                //}

                string IsLead = ((XmlElement)e.SvgElement).GetAttribute("IsLead");

                if (aaa.Contains("Substation")) {
                    PSP_Substation_Info sub = new PSP_Substation_Info();

                    string deviceid = ((XmlElement)e.SvgElement).GetAttribute("Deviceid");
                    //sub.AreaID = tlVectorControl1.SVGDocument.SvgdataUid;
                    PSP_Substation_Info _subTemp = DeviceHelper.GetDevice<PSP_Substation_Info>(deviceid);
                    // PSP_Substation_Info _subTemp = (PSP_Substation_Info)Services.BaseService.GetObject("SelectPSP_Substation_InfoListByEleID", sub);
                    if (_subTemp != null) {
                        fInfo.Info = "变电站名称：" + _subTemp.Title + " 容量：" + _subTemp.L2.ToString("##.##") + "MVA\r\n" + " 电压等级：" + _subTemp.L1.ToString("##.##") + "kV 最大负荷：" + _subTemp.L9.ToString("##.##") + "MW \r\n 投产年限：" + _subTemp.S2;
                    } else {
                        fInfo.Info = "变电站名称：" + " " + " 容量：0" + "MVA" + "\r\n 电压等级： 最大负荷： \r\n 负荷率： 投产年限：";
                    }
                    fInfo.Top = e.Mouse.Y;
                    fInfo.Left = e.Mouse.X;
                    fInfo.Width = (fInfo.Info.Length) * 5;
                    fInfo.Height = 60;
                    fInfo.Show();
                }
                if (aaa.Contains("kbs") || aaa.Contains("fjx") || aaa.Contains("byq") || aaa.Contains("hwg")) {
                    string deviceid = ((XmlElement)e.SvgElement).GetAttribute("Deviceid");
                    string s_name = "";
                    if (aaa.Contains("kbs")) {
                        s_name = "开闭所";
                    }
                    if (aaa.Contains("fjx")) {
                        s_name = "分接箱";
                    }
                    if (aaa.Contains("byq")) {
                        s_name = "变压器";
                    }
                    if (aaa.Contains("hwg")) {
                        s_name = "环网柜";
                    }
                    PSPDEV _subTemp = new PSPDEV();
                    _subTemp.SUID = deviceid;
                    _subTemp = (PSPDEV)Services.BaseService.GetObject("SelectPSPDEVByKey", _subTemp);
                    //PSP_Gra_item sub = new PSP_Gra_item();
                    //sub.EleID = e.SvgElement.ID;
                    //sub.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;
                    //sub.LayerID = tlVectorControl1.SVGDocument.CurrentLayer.ID;
                    //PSP_Gra_item _subTemp = (PSP_Gra_item)Services.BaseService.GetObject("SelectPSP_Gra_itemByEleIDKey", sub);
                    if (_subTemp != null) {
                        fInfo.Info = s_name + " 名称：" + _subTemp.Name;
                    } else {
                        fInfo.Info = s_name; // +"编号：   \r\n 名称： ";
                    }
                    fInfo.Top = e.Mouse.Y;
                    fInfo.Left = e.Mouse.X;
                    fInfo.Width = (fInfo.Info.Length) * 15;
                    fInfo.Height = 60;
                    fInfo.Show();
                }
            }

            //if(e1!=null){
            //    tlVectorControl1.SVGDocument.CurrentElement = e1;
            //}
        }

        void tlVectorControl1_ScaleChanged(object sender, EventArgs e) {
            string text1 = (((ItopVector.ItopVectorControl)sender).ScaleRatio * 100) + "%";
            try {
                scaleBox.ComboBoxEx.SelectedIndexChanged -= new EventHandler(ComboBoxScaleBox_SelectedIndexChanged);

                this.scaleBox.ComboBoxEx.Text = text1;
                scaleBox.ComboBoxEx.SelectedIndexChanged += new EventHandler(ComboBoxScaleBox_SelectedIndexChanged);
            } catch { }
            //if (tlVectorControl1.ScaleRatio < 0.1f) {
            //    tlVectorControl1.ScaleRatio = 0.01f;
            //    scaleBox.ComboBoxEx.Text = "1%";
            //    return;
            //} else
            //    if (tlVectorControl1.ScaleRatio < 0.2f) {
            //        tlVectorControl1.ScaleRatio = 0.1f;
            //        scaleBox.ComboBoxEx.Text = "";
            //        scaleBox.SelectedText = "10%";
            //    }
        }

        void tlVectorControl1_RightClick(object sender, ItopVector.DrawArea.SvgElementSelectedEventArgs e) {
            if (MapType == "接线图") {
                tip.Hide();
                contextMenuStrip1.Show(e.Mouse.X, e.Mouse.Y);
                if (getlayer(SvgDocument.currentLayer, "背景层", tlVectorControl1.SVGDocument.getLayerList())) {
                    contextMenuStrip1.Enabled = false;
                } else {
                    contextMenuStrip1.Enabled = true;
                }

                if (tlVectorControl1.SVGDocument.CurrentElement == null ||
                   tlVectorControl1.SVGDocument.CurrentElement.GetType().ToString() != "ItopVector.Core.Figure.Use") {

                    jxtMenuItem.Visible = false;
                } else {
                    if (tlVectorControl1.SVGDocument.CurrentElement.GetAttribute("xlink:href").Contains("Substation")) {

                        jxtMenuItem.Visible = true;
                    }
                }
                if (tlVectorControl1.Operation == ToolOperation.InterEnclosure) {
                    System.Collections.SortedList list = new SortedList();
                    glebeProperty gPro = new glebeProperty();
                    decimal s = 0;
                    ItopVector.Core.SvgElementCollection selCol = tlVectorControl1.SVGDocument.SelectCollection;
                    if (selCol.Count > 1) {
                        decimal ViewScale = 1;
                        string str_Scale = tlVectorControl1.SVGDocument.getViewScale();
                        if (str_Scale != "") {
                            ViewScale = Convert.ToDecimal(str_Scale);
                        }
                        string str_remark = "";
                        string str_selArea = "";
                        //string Elements = "";
                        Hashtable SelAreaCol = new Hashtable();
                        this.Cursor = Cursors.WaitCursor;
                        int t = 0;
                    Lab001:
                        t = t + 1;
                        XmlElement poly1 = (XmlElement)selCol[selCol.Count - t];
                        if (poly1.GetType().FullName != "ItopVector.Core.Figure.Polygon") {
                            // selCol.Remove(selCol[selCol.Count-1]);
                            goto Lab001;
                        }
                        frmWaiting wait = new frmWaiting();
                        wait.Show(this);
                        wait.Refresh();

                        GraphicsPath gr1 = new GraphicsPath();
                        //gr1.AddRectangle(TLMath.getRectangle(poly1));
                        gr1.AddPolygon(TLMath.getPolygonPoints(poly1));
                        gr1.CloseFigure();

                        for (int i = 0; i < selCol.Count - 1; i++) {
                            if (selCol[i].GetType().FullName == "ItopVector.Core.Figure.Polygon") {

                                string IsArea = ((XmlElement)selCol[i]).GetAttribute("IsArea");
                                if (IsArea != "") {
                                    XmlElement polyn = (XmlElement)selCol[i];
                                    GraphicsPath gr2 = new GraphicsPath();
                                    //gr2.AddRectangle(TLMath.getRectangle(polyn));
                                    gr2.AddPolygon(TLMath.getPolygonPoints(polyn));
                                    gr2.CloseFigure();
                                    Region region = new Region(gr1);
                                    region.Intersect(gr2);

                                    RectangleF rect = new RectangleF();
                                    rect = tlVectorControl1.SelectedRectangle(region);

                                    decimal sub_s = TLMath.getInterPolygonArea(region, rect, ViewScale);
                                    sub_s = TLMath.getNumber2(sub_s, tlVectorControl1.ScaleRatio);
                                    SelAreaCol.Add(polyn.GetAttribute("id"), sub_s);
                                    glebeProperty _gleProp = new glebeProperty();
                                    _gleProp.EleID = polyn.GetAttribute("id");
                                    _gleProp.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;
                                    IList gList = Services.BaseService.GetList("SelectglebePropertyByEleID", _gleProp);

                                    if (gList.Count > 0) {
                                        _gleProp = (glebeProperty)gList[0];
                                        list.Add(_gleProp.UseID, sub_s.ToString("#####.####"));
                                        str_selArea = str_selArea + _gleProp.EleID + "," + sub_s.ToString("#####.####") + ";";
                                        //str_remark = str_remark + "地块" + _gleProp.UseID + "选中面积为：" + sub_s.ToString() + "\r\n";
                                        s += sub_s;
                                    }
                                }
                            }
                            //if (selCol[i].GetType().FullName == "ItopVector.Core.Figure.Use")
                            //{
                            //    XmlElement e1 = (XmlElement)selCol[i];
                            //    string str_id = e1.GetAttribute("id");

                            //    substation _sub1 = new substation();
                            //    _sub1.EleID = str_id;
                            //    _sub1.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;
                            //    _sub1 = (substation)Services.BaseService.GetObject("SelectsubstationByEleID", _sub1);
                            //    if (_sub1 != null)
                            //    {
                            //        _sub1.glebeEleID = guid;
                            //        Services.BaseService.Update("Updatesubstation", _sub1);
                            //    }

                            //}

                        }
                        decimal nullpoly = TLMath.getNumber2(TLMath.getPolygonArea(TLMath.getPolygonPoints(poly1), 1), tlVectorControl1.ScaleRatio) - s;

                        for (int j = 0; j < list.Count; j++) {
                            if (Convert.ToString(list.GetByIndex(j)) != "") {
                                if (Convert.ToDecimal(list.GetByIndex(j)) < 1) {
                                    str_remark = str_remark + "地块" + list.GetKey(j).ToString() + "选中面积为：" + "0" + list.GetByIndex(j).ToString() + "（KM²）\r\n";
                                } else {
                                    str_remark = str_remark + "地块" + list.GetKey(j).ToString() + "选中面积为：" + list.GetByIndex(j).ToString() + "（KM²）\r\n";
                                }
                            }
                        }
                        XmlElement x1 = poly1;// (XmlElement)selCol[selCol.Count - 1];

                        gPro.UID = Guid.NewGuid().ToString();
                        gPro.EleID = x1.GetAttribute("id");
                        gPro.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;
                        gPro.ParentEleID = "0";
                        if (s != 0) {
                            gPro.Area = Convert.ToDecimal(s.ToString("#####.####"));
                        } else {
                            gPro.Area = 0;
                        }
                        gPro.SelSonArea = str_selArea;

                        if (nullpoly < 1) {
                            gPro.ObligateField10 = "0" + nullpoly.ToString("#####.####");
                        } else {
                            gPro.ObligateField10 = nullpoly.ToString("#####.####");
                        }

                        str_remark = str_remark + "\r\n 空白区域面积" + gPro.ObligateField10 + "（KM²）\r\n";
                        if (str_remark != "") {
                            str_remark = str_remark.Substring(0, str_remark.Length - 2);
                        }

                        gPro.Remark = str_remark;
                        wait.Close();
                        this.Cursor = Cursors.Default;
                        if (s < 1) {
                            MessageBox.Show("选中区域面积:" + "0" + s.ToString("#####.####") + "（KM²）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        } else {
                            MessageBox.Show("选中区域面积:" + s.ToString("#####.####") + "（KM²）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                        Services.BaseService.Create<glebeProperty>(gPro);

                        IDictionaryEnumerator ISelList = SelAreaCol.GetEnumerator();
                        while (ISelList.MoveNext()) {
                            glebeProperty sub_gle = new glebeProperty();
                            sub_gle.EleID = ISelList.Key.ToString();
                            sub_gle.ParentEleID = gPro.EleID;
                            sub_gle.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;
                            Services.BaseService.Update("UpdateglebePropertySelArea", sub_gle);
                        }

                        tlVectorControl1.SVGDocument.SelectCollection.Clear();
                        tlVectorControl1.SVGDocument.CurrentElement = (SvgElement)x1;
                    }
                    // SubPrint = false;
                }
            } else {
                contextMenuStrip1.Enabled = false;
            }
        }
        public void LayerManagerShow() {
            //frmlar.Key = "ALL";
            //frmlar.SymbolDoc = tlVectorControl1.SVGDocument;
            //if (MapType == "所内接线图") {
            //    frmlar.Progtype = MapType;
            //} else {
            //    frmlar.Progtype = progtype;
            //}
            //frmlar.Owner = this.ParentForm;
            //frmlar.OnClickLayer += new OnClickLayerhandler(frmlar_OnClickLayer);
            ////frmlar.OnDeleteLayer += new OnDeleteLayerhandler(frmlar_OnDeleteLayer);
            //// Init(progtype);
            //frmlar.Readonly();
            //frmlar.ShowInTaskbar = true;
            //frmlar.Top = 100;//Screen.PrimaryScreen.WorkingArea.Height - 500;
            ////frmlar.Left = Screen.PrimaryScreen.WorkingArea.Width - frmlar.Width;
            //frmlar.Show();


            frmlar.SymbolDoc = tlVectorControl1.SVGDocument;
           
            if (MapType == "所内接线图")
            {
                frmlar.Progtype = MapType;
            }
            else
            {
                frmlar.Progtype = progtype;
            }
            frmlar.Owner = this.ParentForm; ;
            frmlar.OnClickLayer += new OnClickLayerhandler(frmlar_OnClickLayer);
            frmlar.Readonly();
            // Init(progtype);
            frmlar.ShowInTaskbar = false;
            frmlar.Top = 25;//Screen.PrimaryScreen.WorkingArea.Height - 500;
            frmlar.Left = Screen.PrimaryScreen.WorkingArea.Width - frmlar.Width;
            frmlar.Show();
        }
        void frmlar_OnClickLayer(object sender, Layer lar) {
            tlVectorControl1.SVGDocument.SelectCollection.Clear();
            ArrayList a = tlVectorControl1.SVGDocument.getLayerList();
            SvgDocument.currentLayer = lar.ID;
        }
        private void dotNetBarManager1_ItemClick(object sender, EventArgs e) {
            DevComponents.DotNetBar.ButtonItem btItem = sender as DevComponents.DotNetBar.ButtonItem;
            if (btItem != null) {
                switch (btItem.Name) {
                    case "mOpen":
                        frmFileSelect f = new frmFileSelect();
                        f.InitData("", "", false);
                        if (f.ShowDialog() == DialogResult.OK) {
                            OpenFromDatabase(f.svgDataUid);
                        }
                        break;
                    case "btLar":
                        frmlar.Show();
                        break;
                    case "mSelect":
                        tlVectorControl1.Operation = ToolOperation.Select;
                        break;
                    case "m_msel":
                        tlVectorControl1.Operation = ToolOperation.Select;
                        tlVectorControl1.Operation = ToolOperation.InterEnclosure;
                        break;
                    case "mDecreaseView":
                        //string a = tlVectorControl1.SVGDocument.SvgdataUid;


                        //tlVectorControl1.OpenFile("C:\\tl.svg");
                        //tlVectorControl1.SVGDocument.SvgdataUid = a;
                        //tlVectorControl1.Refresh();
                        //SVGFILE svg = new SVGFILE();
                        //svg.SUID = tlVectorControl1.SVGDocument.SvgdataUid;
                        //svg.SVGDATA = tlVectorControl1.SVGDocument.OuterXml;
                        //svg.FILENAME = "aaaaaaaaaa";
                        //svg.PARENTID = "1";
                        //Services.BaseService.Update<SVGFILE>(svg);
                        tlVectorControl1.Operation = ToolOperation.DecreaseView;

                        break;
                    case "mIncreaseView":
                        tlVectorControl1.Operation = ToolOperation.IncreaseView;

                        break;
                    case "mRoam":
                        tlVectorControl1.Operation = ToolOperation.Roam;
                        break;
                    case "mBack":
                        //if (MapType == "所内接线图")
                        if (tlVectorControl1.SVGDocument.SvgdataUid.Length < 20) {
                            if (pid != "") {
                                LoadImage = true;
                                OpenFromDatabase(pid);
                            }
                            //else{
                            //    OpenFromDatabase(SvgUID);
                            //}
                            ButtonEnb(false);
                            bk1.Visible = true;
                        }
                        MapType = "接线图";
                        break;
                    case "mEdit":

                        if (progtype == "电网规划层") {
                            string yearid = str_selID;
                            //frmLayerGrade fgrade = new frmLayerGrade();                           
                            //fgrade.SymbolDoc = tlVectorControl1.SVGDocument;
                            //fgrade.InitData(tlVectorControl1.SVGDocument.SvgdataUid);
                            //if (fgrade.ShowDialog() == DialogResult.OK)
                            //{
                            //yearid=fgrade.GetSelectNode();

                            if (!EditRight) {
                                MessageBox.Show("您没有此权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            if (tlVectorControl1.SVGDocument.RootElement == null) {
                                return;
                            }
                            //frmMain_wh fmain = new frmMain_wh();        //临时暂停 为网架优化用
                            //frmMain_wh.MapType = MapType;
                            //frmMain_wh.progtype = progtype;
                            frmMain_wj fmain = new frmMain_wj();
                            frmMain_wj.MapType = MapType;
                            frmMain_wj.progtype = progtype;
                            fmain.Modifier = true;
                            fmain.yearID = yearid;
                            fmain.SvgName = tlVectorControl1.SVGDocument.FileName;
                            fmain.Show();

                            if (progtype == "地理信息层") {
                                fmain.ViewMenu();
                            }
                            if (pid == "") {
                                fmain.Open(tlVectorControl1.SVGDocument.SvgdataUid);
                            } else {
                                fmain.Open(tlVectorControl1.SVGDocument.SvgdataUid, pid);
                            }
                            LoadImage = true;
                            fmain.InitShape();
                            fmain.Init(progtype);
                            fmain.InitScaleRatio();
                            fmain.LayerManagerShow();
                            //fmain.OnCloseSvgDocument += new OnCloseDocumenthandler(fmain_OnCloseSvgDocument);


                            //fmain.Open(tlVectorControl1.SVGDocument);
                            //}
                        } else if (progtype == "变电站选址") {
                            string yearid = str_selID;
                            //frmLayerGrade fgrade = new frmLayerGrade();                           
                            //fgrade.SymbolDoc = tlVectorControl1.SVGDocument;
                            //fgrade.InitData(tlVectorControl1.SVGDocument.SvgdataUid);
                            //if (fgrade.ShowDialog() == DialogResult.OK)
                            //{
                            //yearid=fgrade.GetSelectNode();

                            if (!EditRight) {
                                MessageBox.Show("您没有此权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            if (tlVectorControl1.SVGDocument.RootElement == null) {
                                return;
                            }
                            //frmMain_wh fmain = new frmMain_wh();        //临时暂停 为网架优化用
                            //frmMain_wh.MapType = MapType;
                            //frmMain_wh.progtype = progtype;
                            frmMain_wj fmain = new frmMain_wj();
                            frmMain_wj.MapType = MapType;
                            frmMain_wj.progtype = progtype;
                            fmain.Modifier = true;
                            fmain.yearID = yearid;
                            fmain.SvgName = tlVectorControl1.SVGDocument.FileName;
                            fmain.Show();

                            if (progtype == "地理信息层") {
                                fmain.ViewMenu();
                            }
                            if (pid == "") {
                                fmain.Open(tlVectorControl1.SVGDocument.SvgdataUid);
                            } else {
                                fmain.Open(tlVectorControl1.SVGDocument.SvgdataUid, pid);
                            }
                            LoadImage = true;
                            fmain.InitShape();
                            fmain.Init(progtype);
                            fmain.InitScaleRatio();
                            fmain.LayerManagerShow();
                            //fmain.OnCloseSvgDocument += new OnCloseDocumenthandler(fmain_OnCloseSvgDocument);


                            //fmain.Open(tlVectorControl1.SVGDocument);
                            //}
                        } else {
                            if (!EditRight) {
                                MessageBox.Show("您没有此权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            if (tlVectorControl1.SVGDocument.RootElement == null) {
                                return;
                            }
                            //frmMain_wh fmain = new frmMain_wh();            //临时暂停 为网架优化用
                            //frmMain_wh.MapType = MapType;
                            //frmMain_wh.progtype = progtype;
                            frmMain_wj fmain = new frmMain_wj();
                            frmMain_wj.MapType = MapType;
                            frmMain_wj.progtype = progtype;

                            //frmMain_Spatial fmain = new frmMain_Spatial();
                            //frmMain_Spatial.MapType = MapType;
                            //frmMain_Spatial.progtype = progtype;
                            fmain.Show();

                            if (progtype == "地理信息层") {
                                fmain.ViewMenu();
                            }
                            if (pid == "") {
                                fmain.Open(tlVectorControl1.SVGDocument.SvgdataUid, "");
                            } else {
                                fmain.Open(tlVectorControl1.SVGDocument.SvgdataUid, pid);
                            }
                            LoadImage = true;
                            fmain.InitShape();
                            fmain.Init(progtype);
                            fmain.InitScaleRatio();
                            //fmain.OpenGHQYpropetty("青山");
                            fmain.LayerManagerShow();

                            // fmain.ShowDialog();
                        }
                        this.ParentForm.Close();
                        break;
                    case "mAirscape":
                        frmAirscape fAir = new frmAirscape();
                        fAir.ShowInTaskbar = false;
                        fAir.InitData(tlVectorControl1);
                        fAir.Owner = this.ParentForm;
                        fAir.Top = Screen.PrimaryScreen.WorkingArea.Height - 250;
                        fAir.Left = Screen.PrimaryScreen.WorkingArea.Width - 300;
                        fAir.Show();
                        break;
                }
            }
        }

        void fmain_OnCloseSvgDocument(object sender, string _SvgUid, string _pid) {
            OpenFromDatabase(_SvgUid);
            pid = _pid;
            //Init();
        }

        public void OpenFromDatabase(string SvgDataUID) {
            try {
                pid = "";
                SvgUID = SvgDataUID;

                if (progtype == "电网规划层" || progtype == "变电站选址") {
                    Open(SvgDataUID);
                } else {
                    Open2(SvgDataUID);
                }
            } catch (Exception e) {
                MessageBox.Show("数据格式错误：" + e.Message);
            }
        }
        public void OpenFromFile(string filename) {

        }
        public void OpenMenu(bool b) {
            打开ToolStripMenuItem.Visible = b;
        }
        public void NullFile(string _SvgUID) {
            try {
                tlVectorControl1.NewFile();
                // Layer.AddNode("背景层", tlVectorControl1.SVGDocument);
                Layer lar1 = Layer.CreateNew("城市规划层", tlVectorControl1.SVGDocument);
                lar1.SetAttribute("layerType", "城市规划层");
                Layer lar2 = Layer.CreateNew("供电区域层", tlVectorControl1.SVGDocument);
                lar2.SetAttribute("layerType", "电网规划层");
                tlVectorControl1.SVGDocument.SvgdataUid = _SvgUID;
                tlVectorControl1.SVGDocument.FileName = this.Text;
                CreateComboBox();
                AddCombolScale();
                this.tlVectorControl1.IsPasteGrid = false;
                this.tlVectorControl1.IsShowGrid = false;
                this.tlVectorControl1.IsShowRule = false;
                this.tlVectorControl1.IsShowTip = false;
                contextMenuStrip1.Enabled = false;
                tlVectorControl1.Operation = ToolOperation.Roam;
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }
        public void Open(SvgDocument _doc) {
            tlVectorControl1.SVGDocument = _doc;
            CreateComboBox();
            AddCombolScale();
            Init();
            this.tlVectorControl1.IsPasteGrid = false;
            this.tlVectorControl1.IsShowGrid = false;
            this.tlVectorControl1.IsShowRule = false;
            this.tlVectorControl1.IsShowTip = false;
            contextMenuStrip1.Enabled = false;
            tlVectorControl1.Operation = ToolOperation.Select;
            //LayerManagerShow();
        }
        public static SvgDocument CashSvgDocument;

        public void Open(string _SvgUID) {
            string id = "''";
            frmLayerGrade fgrade = new frmLayerGrade();
            fgrade.SymbolDoc = tlVectorControl1.SVGDocument;
            fgrade.InitData(_SvgUID);

            if (fgrade.ShowDialog() == DialogResult.OK) {
                id = fgrade.GetSelectNode();
                str_selID = id;
            } else {
                id = fgrade.GetSelectNode();
                str_selID = id;
            }
            frmlar.YearID = id;
            StringBuilder txt = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?><svg id=\"svg\" width=\"1500\" height=\"1000\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:itop=\"http://www.Itop.com/itop\" transform=\"matrix(1 0 0 1 0 1)\"><defs>");
            string svgdefs = "";
            string layertxt = "";
            StringBuilder content = new StringBuilder();

            if (string.IsNullOrEmpty(_SvgUID)) return;
            try {
                SVGFILE svgFile = new SVGFILE();
                svgFile.SUID = _SvgUID;
                svgFile = (SVGFILE)Services.BaseService.GetObject("SelectSVGFILEByKey", svgFile);
                //SvgDocument document = CashSvgDocument;
                //if (document == null) {
                SVG_LAYER lar = new SVG_LAYER();
                lar.svgID = _SvgUID;
                lar.YearID = "Yearid in(" + id + ") or layerType='城市规划层'";
                IList<SVG_LAYER> larlist = Services.BaseService.GetList<SVG_LAYER>("SelectSVG_LAYERByWhere", lar);
                foreach (SVG_LAYER _lar in larlist) {
                    layertxt = layertxt + "<layer id=\"" + _lar.SUID + "\" label=\"" + _lar.NAME + "\" layerType=\"" + _lar.layerType + "\" visibility=\"" + _lar.visibility + "\" ParentID=\"" + _lar.YearID + "\" IsSelect=\"" + _lar.IsSelect + "\" />";
                    content.Append(_lar.XML);
                }
                txt.Append(layertxt);


                SVG_SYMBOL sym = new SVG_SYMBOL();
                sym.svgID = _SvgUID;
                IList<SVG_SYMBOL> symlist = Services.BaseService.GetList<SVG_SYMBOL>("SelectSVG_SYMBOLBySvgID", sym);
                foreach (SVG_SYMBOL _sym in symlist) {
                    svgdefs = svgdefs + _sym.XML;
                }

                txt.Append(svgdefs + "</defs>");
                txt.Append(content.ToString() + "</svg>");


                //IList svgList = Services.BaseService.GetList("SelectSVGFILEByKey", svgFile);
                //svgFile = (SVGFILE)svgList[0];

                SvgDocument document = SvgDocumentFactory.CreateDocument();
                if (txt.ToString() != "1") {
                    string filename = Path.GetTempFileName();
                    if (File.Exists("tmp080321.temp")) {
                        filename = "tmp080321.temp";
                    } else {
                        StreamWriter sw = new StreamWriter(filename);
                        sw.Write(txt.ToString());
                        sw.Close();
                    }
                    tlVectorControl1.OpenFile(filename);
                    document = tlVectorControl1.SVGDocument;
                    int chose = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("chose"));
                    //if (chose == 2)
                    //{ convertDoc(document); }
                } else {
                    NullFile(_SvgUID);
                    return;
                }
                document.FileName = svgFile.FILENAME;
                document.SvgdataUid = svgFile.SUID;
                //CashSvgDocument = document;
                //}

                SvgUID = document.SvgdataUid;
                //this.Text = document.FileName;
                string title = " ";
                string t = "";
                getProjName(MIS.ProgUID, ref title);

                string[] str = str_selID.Split(",".ToCharArray());
                if (str.Length > 1) {
                    for (int i = 1; i < str.Length; i++) {
                        LayerGrade ll = Services.BaseService.GetOneByKey<LayerGrade>(str[i].Replace("'", ""));
                        if (ll != null) {
                            if (ll.ParentID != "SUID") {
                                LayerGrade ll2 = Services.BaseService.GetOneByKey<LayerGrade>(ll.ParentID);
                                t = t + ll2.Name + " " + ll.Name + "， ";
                            }
                        }
                    }
                }

                this.ParentForm.Text = title + " " + t + " 浏览状态";
                if (document.RootElement == null) {
                    tlVectorControl1.NewFile();
                    Layer.CreateNew("背景层", tlVectorControl1.SVGDocument);
                    Layer.CreateNew("城市规划层", tlVectorControl1.SVGDocument);
                    Layer.CreateNew("供电区域层", tlVectorControl1.SVGDocument);
                } else {
                    tlVectorControl1.SVGDocument = document;
                }
                tlVectorControl1.SVGDocument.SvgdataUid = SvgUID;
                tlVectorControl1.SVGDocument.FileName = this.Text;
                if (svgFile.SUID.Length > 20) {
                    MapType = "接线图";
                }

                CreateComboBox();
                AddCombolScale();
                Init();
                this.tlVectorControl1.IsPasteGrid = false;
                this.tlVectorControl1.IsShowGrid = false;
                this.tlVectorControl1.IsShowRule = false;
                this.tlVectorControl1.IsShowTip = false;
                contextMenuStrip1.Enabled = false;
                //tlVectorControl1.Operation = ToolOperation.Roam;
                //tlVectorControl1.ScaleRatio = 0.1f;
                //LayerManagerShow();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }
        public void getProjName(string uid, ref string title) {
            Project sm = Services.BaseService.GetOneByKey<Project>(uid);
            if (sm != null) {
                title = sm.ProjectName + " " + title;
                if (sm.ProjectManager == sm.UID) { return; }
                getProjName(sm.ProjectManager, ref title);
            }
            //return title;
        }
        public void Open2(string _SvgUID) {

            StringBuilder txt = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?><svg id=\"svg\" width=\"1500\" height=\"1000\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:itop=\"http://www.Itop.com/itop\" transform=\"matrix(1 0 0 1 0 1)\"><defs>");
            string svgdefs = "";
            string layertxt = "";
            StringBuilder content = new StringBuilder();
            string where = "";
            if (string.IsNullOrEmpty(_SvgUID)) return;
            try {
                if (progtype == "城市规划层") {
                    where = " (layerType = '城市规划层' OR layerType = '地理信息层' OR YearID = '') ";
                } else {
                    where = " (layerType = '地理信息层') ";
                }
                SVGFILE svgFile = new SVGFILE();
                svgFile.SUID = _SvgUID;
                svgFile = (SVGFILE)Services.BaseService.GetObject("SelectSVGFILEByKey", svgFile);
                //SvgDocument document = CashSvgDocument;
                //if (document == null) {
                SVG_LAYER lar = new SVG_LAYER();
                lar.svgID = _SvgUID;
                lar.YearID = where;
                IList<SVG_LAYER> larlist = Services.BaseService.GetList<SVG_LAYER>("SelectSVG_LAYERByWhere", lar);
                foreach (SVG_LAYER _lar in larlist) {
                    layertxt = layertxt + "<layer id=\"" + _lar.SUID + "\" label=\"" + _lar.NAME + "\" layerType=\"" + _lar.layerType + "\" visibility=\"" + _lar.visibility + "\" ParentID=\"" + _lar.YearID + "\" IsSelect=\"" + _lar.IsSelect + "\" />";
                    content.Append(_lar.XML);
                }
                txt.Append(layertxt);


                SVG_SYMBOL sym = new SVG_SYMBOL();
                sym.svgID = _SvgUID;
                IList<SVG_SYMBOL> symlist = Services.BaseService.GetList<SVG_SYMBOL>("SelectSVG_SYMBOLBySvgID", sym);
                foreach (SVG_SYMBOL _sym in symlist) {
                    svgdefs = svgdefs + _sym.XML;
                }

                txt.Append(svgdefs + "</defs>");
                txt.Append(content.ToString() + "</svg>");

                SvgDocument document = SvgDocumentFactory.CreateDocument();
                if (txt.ToString() != "1") {
                    string filename = Path.GetTempFileName();
                    StreamWriter sw = new StreamWriter(filename);
                    sw.Write(txt.ToString());
                    sw.Close();
                    tlVectorControl1.OpenFile(filename);
                    document = tlVectorControl1.SVGDocument;
                    int chose = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("chose"));
                    //if (chose == 2)
                    //{ convertDoc(document); }
                } else {
                    NullFile(_SvgUID);
                    return;
                }
                document.FileName = svgFile.FILENAME;
                document.SvgdataUid = svgFile.SUID;
                SvgUID = document.SvgdataUid;
                this.Text = document.FileName;
                if (document.RootElement == null) {
                    tlVectorControl1.NewFile();
                    Layer.CreateNew("背景层", tlVectorControl1.SVGDocument);
                    Layer.CreateNew("城市规划层", tlVectorControl1.SVGDocument);
                    Layer.CreateNew("供电区域层", tlVectorControl1.SVGDocument);
                } else {
                    tlVectorControl1.SVGDocument = document;
                }
                tlVectorControl1.SVGDocument.SvgdataUid = SvgUID;
                tlVectorControl1.SVGDocument.FileName = this.Text;
                if (svgFile.SUID.Length > 20) {
                    MapType = "接线图";
                }
                NotDKbutProperty();
                CreateComboBox();
                AddCombolScale();
                Init();
                this.tlVectorControl1.IsPasteGrid = false;
                this.tlVectorControl1.IsShowGrid = false;
                this.tlVectorControl1.IsShowRule = false;
                this.tlVectorControl1.IsShowTip = false;
                contextMenuStrip1.Enabled = false;
                //tlVectorControl1.Operation = ToolOperation.Roam;
                //tlVectorControl1.ScaleRatio = 0.1f;
                //LayerManagerShow();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }
        private void NotDKbutProperty()
        {
            ArrayList list = tlVectorControl1.SVGDocument.getLayerList();
            for (int i = 0; i < list.Count; i++)
            {
                string str_sed = "";
                Layer layer = (Layer)list[i];
                if (layer.GetAttribute("layerType") == "城市规划层")
                {

                    XmlNodeList nd = layer.SelectNodes("//*[@IsArea=\"1\" and @layer='" + layer.ID + "']");
                    foreach (XmlNode xn in nd)
                    {
                        str_sed = str_sed + "'" + ((XmlElement)xn).GetAttribute("id") + "',";
                    }
                    if (str_sed.Length > 0)
                    {
                        str_sed = str_sed.Substring(0, str_sed.Length - 1);
                        IList<glebeProperty> list1 = Services.BaseService.GetList<glebeProperty>("SelectglebePropertyByWhere","LayerID='" + layer.ID + "'and EleID NOT IN(" + str_sed + ")");
                        if (list1.Count > 0)
                            Services.BaseService.Update("DeleteglebePropertyByCon", "LayerID='" + layer.ID + "'and EleID NOT IN(" + str_sed + ")");
                    }

                }
            }
        }
        public SvgDocument convertDoc(SvgDocument doc) {
            SvgElementCollection list = (doc.RootElement as SVG).GraphList;
            Matrix matrix1 = new Matrix();
            Matrix matrix11 = new Matrix();
            matrix1.Scale(1.30208f, 1.04167f, MatrixOrder.Prepend);//1.04167f, MatrixOrder.Prepend);//);
            matrix11.Scale(1.30208f, 1.30208f, MatrixOrder.Prepend);//1.04167f, MatrixOrder.Prepend);//);

            matrix1.Translate(113.67187f, -40.03906f, MatrixOrder.Append);
            matrix11.Translate(113.67187f, -40.03906f, MatrixOrder.Append);

            foreach (IGraph graph in list) {
                //graph.Transform.setMatrix(matrix1);
                Matrix matrix2 = graph.Transform.Matrix.Clone();

                if (graph is Use) {
                    //matrix2.Multiply(matrix11, MatrixOrder.Append);
                    //graph.Transform = new ItopVector.Core.Types.Transf(matrix2);
                    Use use = graph as Use;
                    float x = (use.CenterPoint.X) * 0.30208f + 111.08317f;
                    float y = (use.CenterPoint.Y) * 0.04167f - 45.03906f;
                    matrix2.Translate(x, y, MatrixOrder.Append);
                    graph.Transform = new ItopVector.Core.Types.Transf(matrix2);
                } else {
                    matrix2.Multiply(matrix1, MatrixOrder.Append);
                    graph.Transform = new ItopVector.Core.Types.Transf(matrix2);
                }
            }
            return doc;
        }
        void ComboBoxEx_SelectedIndexChanged(object sender, EventArgs e) {
            Layer lar = (Layer)LayerBox.ComboBoxEx.SelectedItem;
            SvgDocument.currentLayer = lar.ID;
            lar.Visible = true;
        }
        void ComboBoxScaleBox_SelectedIndexChanged(object sender, EventArgs e) {
            string text1 = this.scaleBox.SelectedItem.ToString();
            tlVectorControl1.ScaleRatio = ItopVector.Core.Func.Number.ParseFloatStr(text1);
            //tlVectorControl1.Operation = ToolOperation.Select;

        }
        private void CreateComboBox() {
            //字体
            popupContainerEdit1.Text = "";
            Layerlist = tlVectorControl1.SVGDocument.getLayerList();
            ArrayList tmplaylist = new ArrayList();
            DevExpress.XtraEditors.Controls.CheckedListBoxItem[] chkItems = null;
            this.checkedListBoxControl1.Items.Clear();

            for (int i = 0; i < Layerlist.Count; i++) {
                Layer lar = (Layer)Layerlist[i];
                if (lar.GetAttribute("layerType") == progtype || MapType == "所内接线图") {
                    tmplaylist.Add(Layerlist[i]);
                } else {
                    lar.Visible = false;
                }
            }
            //if(MapType=="所内接线图"){

            //    for (int i = 0; i < Layerlist.Count; i++)
            //    {
            //        Layer lar = (Layer)Layerlist[i];
            //        if (lar.GetAttribute("layerType") == MapType)
            //        {
            //            tmplaylist.Add(Layerlist[i]);
            //        }
            //        else
            //        {
            //            lar.Visible = false;
            //        }
            //    }
            //}
            chkItems = new DevExpress.XtraEditors.Controls.CheckedListBoxItem[tmplaylist.Count];
            for (int i = 0; i < tmplaylist.Count; i++) {
                chkItems.SetValue(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(((Layer)tmplaylist[i]).Label), i);
                if (((Layer)tmplaylist[i]).Visible) {
                    chkItems[i].CheckState = CheckState.Checked;
                }
            }
            if (tmplaylist.Count > 0) {
                Layer lar1 = (Layer)tmplaylist[0];
                SvgDocument.currentLayer = lar1.ID;
                popupContainerEdit1.Text = lar1.Label;
                selLar = lar1.Label;
            }

            this.checkedListBoxControl1.Items.AddRange(chkItems);
            this.checkedListBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.checkedListBoxControl1.Name = "checkedListBoxControl1";
            this.popupContainerEdit1.Properties.PopupControl = this.popupContainerControl1;

        }
        private void AddCombolScale() {
            try {
                //缩放大小
                scaleBox = this.dotNetBarManager1.GetItem("ScaleBox") as DevComponents.DotNetBar.ComboBoxItem;
                if (scaleBox != null) {
                    scaleBox.Items.Clear();
                    scaleBox.Items.AddRange(mapview.ScaleRange());
                    scaleBox.ComboBoxEx.Text = "100%";
                    scaleBox.ComboBoxEx.SelectedIndexChanged += new EventHandler(ComboBoxScaleBox_SelectedIndexChanged);

                }
            } catch (Exception e1) { }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            if (tlVectorControl1.SVGDocument.RootElement == null) {
                return;
            }
            XmlElement xml1 = (XmlElement)tlVectorControl1.SVGDocument.CurrentElement;
            if (xml1 != null) {
                if (e.ClickedItem.Text == "属性") {

                    if (getlayer(SvgDocument.currentLayer, "电网规划层", tlVectorControl1.SVGDocument.getLayerList()/*"layer24288"*/)) {

                        if (xml1 == null || tlVectorControl1.SVGDocument.CurrentElement.ID == "svg") {
                            MessageBox.Show("请选择地块。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        /*  if (xml1.GetType().ToString() == "ItopVector.Core.Figure.Polygon")
                          {
                              //string IsArea = ((XmlElement)tlVectorControl1.SVGDocument.CurrentElement).GetAttribute("IsArea");
                              //if (IsArea != "")
                              //{
                              glebeProperty _gle = new glebeProperty();
                              _gle.EleID = tlVectorControl1.SVGDocument.CurrentElement.ID;
                              _gle.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;

                              IList<glebeProperty> UseProList = Services.BaseService.GetList<glebeProperty>("SelectglebePropertyByEleID", _gle);
                              if (UseProList.Count > 0)
                              {
                                  _gle = UseProList[0];

                                  frmMainProperty f = new frmMainProperty();

                                  XmlNodeList n1 = tlVectorControl1.SVGDocument.GetElementsByTagName("use");
                                  PointF[] tfArray1 = TLMath.getPolygonPoints(xml1);
                                  string str220 = "";
                                  string str110 = "";
                                  string str66 = "";

                                  GraphicsPath selectAreaPath = new GraphicsPath();
                                  selectAreaPath.AddLines(tfArray1);
                                  selectAreaPath.CloseFigure();
                                  //Matrix x=new Matrix(
                                  //Region region1 = new Region(selectAreaPath);
                                  for (int i = 0; i < n1.Count; i++)
                                  {
                                      float OffX = 0f;
                                      float OffY = 0f;
                                      bool ck = false;
                                      Use use = (Use)n1[i];
                                      if (use.GetAttribute("xlink:href").Contains("Substation"))
                                      {
                                          string strMatrix = use.GetAttribute("transform");
                                          if (strMatrix != "")
                                          {
                                              strMatrix = strMatrix.Replace("matrix(", "");
                                              strMatrix = strMatrix.Replace(")", "");
                                              string[] mat = strMatrix.Split(',');
                                              if (mat.Length > 5)
                                              {
                                                  OffX = Convert.ToSingle(mat[4]);
                                                  OffY = Convert.ToSingle(mat[5]);
                                              }
                                          }
                                          if (frmlar.getSelectedLayer().Contains(use.GetAttribute("layer")))
                                          {
                                              ck = true;
                                          }
                                          PointF TempPoint = TLMath.getUseOffset(use.GetAttribute("xlink:href"));
                                          if (selectAreaPath.IsVisible(use.X + TempPoint.X + OffX, use.Y + TempPoint.Y + OffY) && ck)
                                          {
                                              if (use.GetAttribute("xlink:href").Contains("220"))
                                              {
                                                  str220 = str220 + "'" + use.GetAttribute("id") + "',";
                                              }
                                              if (use.GetAttribute("xlink:href").Contains("110"))
                                              {
                                                  str110 = str110 + "'" + use.GetAttribute("id") + "',";
                                              }
                                              if (use.GetAttribute("xlink:href").Contains("66"))
                                              {
                                                  str66 = str66 + "'" + use.GetAttribute("id") + "',";
                                              }
                                          }
                                      }
                                  }
                                  if (str220.Length > 1)
                                  {
                                      str220 = str220.Substring(0, str220.Length - 1);
                                  }
                                  if (str110.Length > 1)
                                  {
                                      str110 = str110.Substring(0, str110.Length - 1);
                                  }
                                  if (str66.Length > 1)
                                  {
                                      str66 = str66.Substring(0, str66.Length - 1);
                                  }
                                  f.InitData(_gle, str220, str110, str66);  ////////////////////////////////////////////////////
                                  f.IsReadonly = true;
                                  f.ShowDialog();

                              }
                              //}
                          }*/

                    }
                    if (getlayer(SvgDocument.currentLayer, "城市规划层", tlVectorControl1.SVGDocument.getLayerList())/*"layer97052"*/) {
                        if (tlVectorControl1.SVGDocument.getRZBRatio() != "") {
                            rzb = tlVectorControl1.SVGDocument.getRZBRatio();
                        }
                        if (xml1.GetType().ToString() == "ItopVector.Core.Figure.Polygon") {
                            string IsArea = xml1.GetAttribute("IsArea");
                            if (IsArea != "") {
                                frmProperty f = new frmProperty();
                                f.InitData(xml1.GetAttribute("id"), tlVectorControl1.SVGDocument.SvgdataUid, SelUseArea, rzb, SvgDocument.currentLayer);
                                f.IsReadonly = true;
                                f.ShowDialog();
                            }
                        }
                    }
                    if (xml1 == null) return;
                    if (xml1.GetType().ToString() == "ItopVector.Core.Figure.Line" || xml1.GetType().ToString() == "ItopVector.Core.Figure.Polyline") {
                        string IsLead = xml1.GetAttribute("IsLead");
                        if (IsLead != "") {
                            frmLineProperty fl = new frmLineProperty();
                            fl.InitData(xml1.GetAttribute("id"), tlVectorControl1.SVGDocument.SvgdataUid, LineLen, SvgDocument.currentLayer);
                            fl.IsReadonly = true;
                            fl.ShowDialog();
                        }
                    }
                    if (xml1.GetAttribute("xlink:href").Contains("Substation")) {
                        string lab = xml1.GetAttribute("xlink:href");
                        frmSubstationProperty frmSub = new frmSubstationProperty();
                        frmSub.InitData(xml1.GetAttribute("id"), tlVectorControl1.SVGDocument.SvgdataUid, SvgDocument.currentLayer, lab);
                        frmSub.IsReadonly = true;
                        frmSub.ShowDialog();
                    }
                    if (tlVectorControl1.Operation == ToolOperation.InterEnclosure) {
                        if (tlVectorControl1.SVGDocument.CurrentElement.GetType().ToString() == "ItopVector.Core.Figure.Polygon") {
                            XmlNodeList n1 = tlVectorControl1.SVGDocument.GetElementsByTagName("use");
                            PointF[] tfArray1 = TLMath.getPolygonPoints(xml1);
                            string str220 = "";
                            string str110 = "";
                            string str66 = "";

                            GraphicsPath selectAreaPath = new GraphicsPath();
                            selectAreaPath.AddLines(tfArray1);
                            selectAreaPath.CloseFigure();
                            //Matrix x=new Matrix(
                            //Region region1 = new Region(selectAreaPath);
                            for (int i = 0; i < n1.Count; i++) {
                                float OffX = 0f;
                                float OffY = 0f;
                                bool ck = false;
                                Use use = (Use)n1[i];
                                if (use.GetAttribute("xlink:href").Contains("Substation")) {
                                    string strMatrix = use.GetAttribute("transform");
                                    if (strMatrix != "") {
                                        strMatrix = strMatrix.Replace("matrix(", "");
                                        strMatrix = strMatrix.Replace(")", "");
                                        string[] mat = strMatrix.Split(',');
                                        if (mat.Length > 5) {
                                            OffX = Convert.ToSingle(mat[4]);
                                            OffY = Convert.ToSingle(mat[5]);
                                        }
                                    }
                                    if (frmlar.getSelectedLayer().Contains(use.GetAttribute("layer"))) {
                                        ck = true;
                                    }
                                    PointF TempPoint = TLMath.getUseOffset(use.GetAttribute("xlink:href"));
                                    if (selectAreaPath.IsVisible(use.X + TempPoint.X + OffX, use.Y + TempPoint.Y + OffY) && ck) {
                                        if (use.GetAttribute("xlink:href").Contains("220")) {
                                            str220 = str220 + "'" + use.GetAttribute("id") + "',";
                                        }
                                        if (use.GetAttribute("xlink:href").Contains("110")) {
                                            str110 = str110 + "'" + use.GetAttribute("id") + "',";
                                        }
                                        if (use.GetAttribute("xlink:href").Contains("66")) {
                                            str66 = str66 + "'" + use.GetAttribute("id") + "',";
                                        }
                                    }
                                }
                            }
                            if (str220.Length > 1) {
                                str220 = str220.Substring(0, str220.Length - 1);
                            }
                            if (str110.Length > 1) {
                                str110 = str110.Substring(0, str110.Length - 1);
                            }
                            if (str66.Length > 1) {
                                str66 = str66.Substring(0, str66.Length - 1);
                            }
                            glebeProperty _gle = new glebeProperty();
                            _gle.EleID = tlVectorControl1.SVGDocument.CurrentElement.ID;
                            _gle.SvgUID = tlVectorControl1.SVGDocument.SvgdataUid;

                            IList<glebeProperty> UseProList = Services.BaseService.GetList<glebeProperty>("SelectglebePropertyByEleID", _gle);
                            if (UseProList.Count > 0) {
                                _gle = UseProList[0];
                                _gle.LayerID = SvgDocument.currentLayer;
                                frmMainProperty f = new frmMainProperty();
                                f.simpleButton4.Visible = false;
                                f.InitData(_gle, str220, str110, str66);
                                //f.InitData(_gle, str220, str110, str66);  ////////////////////////////////////////////////////
                                f.IsReadonly = true;
                                f.ShowDialog();
                                //if (f.checkBox1.Checked == false)
                                //{
                                tlVectorControl1.SVGDocument.RootElement.RemoveChild(tlVectorControl1.SVGDocument.CurrentElement);
                                //}
                                //tlVectorControl1.Refresh();
                            }
                            //}
                        }
                    }
                    tlVectorControl1.SVGDocument.CurrentElement = null;
                }

                if (e.ClickedItem.Text == "接线图") {
                    pid = tlVectorControl1.SVGDocument.SvgdataUid;
                    SVGFILE svg_temp = new SVGFILE();
                    svg_temp.SUID = xml1.GetAttribute("id");
                    IList svglist = Services.BaseService.GetList("SelectSVGFILEByKey", svg_temp);
                    if (svglist.Count > 0) {
                        svg_temp = (SVGFILE)svglist[0];
                        SvgDocument sdoc = new SvgDocument();
                        sdoc.LoadXml(svg_temp.SVGDATA);
                        tlVectorControl1.SVGDocument = sdoc;
                        tlVectorControl1.SVGDocument.SvgdataUid = svg_temp.SUID;
                        MapType = "所内接线图";
                    } else {
                        tlVectorControl1.NewFile();
                        tlVectorControl1.IsPasteGrid = false;
                        tlVectorControl1.IsShowGrid = false;
                        tlVectorControl1.IsShowRule = false;
                        tlVectorControl1.IsShowTip = false;
                        tlVectorControl1.SVGDocument.SvgdataUid = svg_temp.SUID;
                        MapType = "所内接线图";
                    }
                    if (tlVectorControl1.SVGDocument.getLayerList().Count == 0) {
                        Layer _lar = ItopVector.Core.Figure.Layer.CreateNew("接线图", tlVectorControl1.SVGDocument);
                        _lar.SetAttribute("layerType", "所内接线图");
                        _lar.Visible = true;
                        SvgDocument.currentLayer = ((Layer)tlVectorControl1.SVGDocument.getLayerList()[0]).ID;
                    }
                    CreateComboBox();

                    ButtonEnb(true);
                    LoadImage = false;
                    bk1.Visible = false;
                    selLar = "";
                    tlVectorControl1.Refresh();
                }
            }

        }

        private void checkedListBoxControl1_SelectedIndexChanged(object sender, EventArgs e) {
            //MessageBox.Show(checkedListBoxControl1.SelectedValue.ToString());
        }

        private void checkedListBoxControl1_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e) {
            selLar = checkedListBoxControl1.SelectedValue.ToString();
            Layer lar = getlayer(selLar, tlVectorControl1.SVGDocument.getLayerList());
            SvgDocument.currentLayer = lar.ID;
            if (checkedListBoxControl1.GetItemChecked(e.Index)) {
                lar.Visible = true;
                if (lar.Label == "背景层") {
                    if (img != null) {
                        ((SvgElement)img).ParseAttribute("xlink:href", Application.StartupPath + "\\Symbol\\铜陵.gif", true);
                        tlVectorControl1.Refresh();
                    }
                }
            } else {
                lar.Visible = false;
                if (lar.Label == "背景层") {
                    if (img != null) {
                        ((SvgElement)img).ParseAttribute("xlink:href", "", true);
                        tlVectorControl1.Refresh();
                    }
                }
            }

        }

        private void popupContainerEdit1_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e) {
            popupContainerEdit1.Text = selLar;
        }
        public static Layer getlayer(string LayerName, ArrayList LayerList) {
            Layer layer = null;
            for (int i = 0; i < LayerList.Count; i++) {
                if (LayerName == ((Layer)LayerList[i]).Label) {
                    layer = (Layer)LayerList[i];
                    break;
                }
            }
            return layer;
        }
        public static bool getlayer(string CurrLayerID, string LayerName, ArrayList LayerList) {
            Layer layer = null;
            string layerType = "";
            for (int i = 0; i < LayerList.Count; i++) {
                if (CurrLayerID == ((Layer)LayerList[i]).ID) {
                    layer = (Layer)LayerList[i];
                    layerType = layer.GetAttribute("layerType");
                    if (layerType == LayerName) {
                        return true;
                    }
                }
            }
            return false;
        }
        public SvgDocument svgDocument {
            get { return tlVectorControl1.SVGDocument; }

        }

        private void checkedListBoxControl1_Click(object sender, EventArgs e) {
            try {
                selLar = checkedListBoxControl1.SelectedValue.ToString();
                if (selLar != "") {
                    Layer lar = getlayer(selLar, tlVectorControl1.SVGDocument.getLayerList());
                    SvgDocument.currentLayer = lar.ID;
                }
            } catch (Exception e1) { }
        }
        public void Print() {
            if (tlVectorControl1.SVGDocument.RootElement == null) {
                MessageBox.Show("请先打开规划图。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            tlVectorControl1.Print();
        }
        public void ButtonEnb(bool enb) {
            //int a=dotNetBarManager1.Items.Count;

            //ButtonItem ghfx = dotNetBarManager1.GetItem("mBack") as ButtonItem;
            //ghfx.Enabled = enb;

        }
        public DotNetBarManager Bar() {
            return dotNetBarManager1;
        }
        public System.Drawing.Image ClipScreen() {
            return tlVectorControl1.ClipScreen(false);
        }

        private void comboSel_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (tlVectorControl1.SVGDocument.RootElement == null) {
                    return;
                }
                if (comboSel.Text == "地块列表") {
                    frmLayerList lay = new frmLayerList();
                    lay.InitData(tlVectorControl1.SVGDocument.getLayerList(), "1");
                    if (lay.ShowDialog() == DialogResult.OK) {
                        frmglebePropertyList flist1 = new frmglebePropertyList();
                        flist1.InitDataSub(tlVectorControl1.SVGDocument.SvgdataUid, lay.str_sid);
                        flist1.Show();
                    }
                }
                if (comboSel.Text == "供电区域列表") {
                    frmLayerList lay2 = new frmLayerList();
                    lay2.InitData(tlVectorControl1.SVGDocument.getLayerList(), "2");
                    if (lay2.ShowDialog() == DialogResult.OK) {
                        frmglebePropertyList flist2 = new frmglebePropertyList();
                        flist2.InitData(tlVectorControl1.SVGDocument.SvgdataUid, lay2.str_sid);
                        flist2.Show();
                    }
                }
                if (comboSel.Text == "线路列表") {
                    frmLayerList lay3 = new frmLayerList();
                    lay3.InitData(tlVectorControl1.SVGDocument.getLayerList(), "2");
                    if (lay3.ShowDialog() == DialogResult.OK) {
                        frmLinePropertyList flist3 = new frmLinePropertyList();
                        flist3.InitData(tlVectorControl1.SVGDocument.SvgdataUid, lay3.str_sid);
                        flist3.Show();
                    }
                }
                if (comboSel.Text == "电力平衡表") {
                    frmLayerList lay4 = new frmLayerList();
                    lay4.InitData(tlVectorControl1.SVGDocument.getLayerList(), "3");
                    if (lay4.ShowDialog() == DialogResult.OK) {
                        frmSubstationPropertyList fSub = new frmSubstationPropertyList();
                        fSub.InitData(tlVectorControl1.SVGDocument.SvgdataUid, lay4.str_sid);
                        fSub.Show();
                    }
                }
            } catch (Exception e1) {
                MessageBox.Show("错误：" + e1.Message);
            }

        }

        private void bk1_CheckedChanged(object sender, EventArgs e) {
            if (bk1.Checked == true) {
                LoadImage = true;
            } else {
                LoadImage = false;
            }
            tlVectorControl1.Refresh();
        }

        private void CtrlSvgView_Load(object sender, EventArgs e) {

        }
        public ItopVectorControl TLVertor {
            get {
                return tlVectorControl1;
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e) {
            if (chose == 1) {
                if (checkEdit1.Checked) {
                    mapview = new Itop.MapView.MapViewObj("MapData3d.yap");
                } else {
                    mapview = new Itop.MapView.MapViewObj();
                }
                mapview.ZeroLongLat = new LongLat(jd, wd);
                tlVectorControl1.Refresh();

            }
        }

        public void SetBk1BackColor(System.Drawing.Color color1) {
            bk1.BackColor = color1;
            checkEdit1.BackColor = color1;
        }
    }
}

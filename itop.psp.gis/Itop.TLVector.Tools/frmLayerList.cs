using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using ItopVector.Core.Figure;
using Itop.Client.Base;
using System.Xml;
namespace ItopVector.Tools
{
    public partial class frmLayerList : FormBase
    {
        private ArrayList alist;
        public string str_sid = "";
        public string str_sed = "";
        public Hashtable hs;
        public Hashtable xzhs;
        public frmLayerList()
        {
            InitializeComponent();
            hs = new Hashtable();
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void InitData(ArrayList list,string stype)
        {
            alist = list;
           
            if(stype=="1"){
                for(int i=0;i<list.Count;i++){
                    Layer layer=(Layer)list[i];
                    if (layer.GetAttribute("layerType") == "城市规划层")
                    {
                        layerList.Items.Add(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(layer.Label));
                        hs.Add(((Layer)list[i]).Label, list[i]);
                       
                    }
                }
            }
            if (stype == "2")
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Layer layer = (Layer)list[i];
                    if (layer.GetAttribute("layerType") == "电网规划层")
                    {
                        layerList.Items.Add(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(layer.Label));
                        hs.Add(((Layer)list[i]).Label, list[i]);
                    }
                }
            }
            if (stype == "3")
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Layer layer = (Layer)list[i];
                    if (layer.GetAttribute("layerType") == "城市规划层" || layer.GetAttribute("layerType") == "电网规划层")
                    {
                        layerList.Items.Add(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(layer.Label));
                        hs.Add(((Layer)list[i]).Label, list[i]);
                    }
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           
            for(int i=0;i<layerList.Items.Count;i++){
                if(layerList.GetItemChecked(i)){
                    str_sid = str_sid + "'" + ((Layer)hs[layerList.GetItemText(i)]).ID + "',";
                    //xzhs.Add(layerList.GetItemText(i), hs[layerList.GetItemText(i)]);
                    XmlNodeList nd = ((Layer)hs[layerList.GetItemText(i)]).SelectNodes("//*[@IsArea=\"1\" and @layer='" + ((Layer)hs[layerList.GetItemText(i)]).ID + "']");
                    foreach (XmlNode xn in nd)
                    {
                        str_sed = str_sed + "'" + ((XmlElement)xn).GetAttribute("id") + "',";
                    }
                }
            }
            if (str_sid.Length > 0)
            {
                str_sid = str_sid.Substring(0, str_sid.Length - 1);
            }
            else
            {
                str_sid = "'xxx'";
            }
            if (str_sed.Length > 0)
                str_sed = str_sed.Substring(0, str_sed.Length - 1);
            
        }
    }
}
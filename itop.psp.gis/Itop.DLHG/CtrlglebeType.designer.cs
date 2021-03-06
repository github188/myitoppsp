
namespace Itop.DLGH{	partial class CtrlglebeType	{		/// <summary>		/// 必需的设计器变量。		/// </summary>		private System.ComponentModel.IContainer components = null;		/// <summary>		/// 清理所有正在使用的资源。		/// </summary>		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>		protected override void Dispose(bool disposing)		{			if (disposing && (components != null))			{				components.Dispose();			}			base.Dispose(disposing);		}		#region 组件设计器生成的代码		/// <summary> 		/// 设计器支持所需的方法 - 不要		/// 使用代码编辑器修改此方法的内容。		/// </summary>		private void InitializeComponent()		{
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colUID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTypeStyle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemColorEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemColorEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridControl.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridControl.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridControl.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridControl.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridControl.Location = new System.Drawing.Point(0, 0);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemColorEdit1});
            this.gridControl.Size = new System.Drawing.Size(870, 319);
            this.gridControl.TabIndex = 0;
            this.gridControl.UseEmbeddedNavigator = true;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            this.gridView.AppearancePrint.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colUID,
            this.colTypeID,
            this.colTypeName,
            this.colTypeStyle,
            this.gridColumn3,
            this.gridColumn4,
            this.colColor,
            this.gridColumn1,
            this.gridColumn2});
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsPrint.UsePrintStyles = true;
            this.gridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
            // 
            // colUID
            // 
            this.colUID.AppearanceHeader.Options.UseTextOptions = true;
            this.colUID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUID.FieldName = "UID";
            this.colUID.Name = "colUID";
            this.colUID.OptionsColumn.ShowInCustomizationForm = false;
            // 
            // colTypeID
            // 
            this.colTypeID.AppearanceHeader.Options.UseTextOptions = true;
            this.colTypeID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTypeID.Caption = "地块编号";
            this.colTypeID.FieldName = "TypeID";
            this.colTypeID.Name = "colTypeID";
            this.colTypeID.Visible = true;
            this.colTypeID.VisibleIndex = 0;
            this.colTypeID.Width = 87;
            // 
            // colTypeName
            // 
            this.colTypeName.AppearanceHeader.Options.UseTextOptions = true;
            this.colTypeName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTypeName.Caption = "地块名称";
            this.colTypeName.FieldName = "TypeName";
            this.colTypeName.Name = "colTypeName";
            this.colTypeName.Visible = true;
            this.colTypeName.VisibleIndex = 1;
            this.colTypeName.Width = 86;
            // 
            // colTypeStyle
            // 
            this.colTypeStyle.AppearanceCell.Options.UseTextOptions = true;
            this.colTypeStyle.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTypeStyle.AppearanceHeader.Options.UseTextOptions = true;
            this.colTypeStyle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTypeStyle.Caption = "负荷密度推荐指标（MW/KM²）";
            this.colTypeStyle.DisplayFormat.FormatString = "####0.####";
            this.colTypeStyle.FieldName = "TypeStyle";
            this.colTypeStyle.Name = "colTypeStyle";
            this.colTypeStyle.Visible = true;
            this.colTypeStyle.VisibleIndex = 2;
            this.colTypeStyle.Width = 166;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "负荷密度指标范围（MW/KM²）";
            this.gridColumn3.FieldName = "ObligateField5";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 187;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "地下负荷指标（MW/KM²）";
            this.gridColumn4.DisplayFormat.FormatString = "##0.##";
            this.gridColumn4.FieldName = "ObligateField6";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 4;
            this.gridColumn4.Width = 97;
            // 
            // colColor
            // 
            this.colColor.AppearanceHeader.Options.UseTextOptions = true;
            this.colColor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colColor.Caption = "颜色";
            this.colColor.ColumnEdit = this.repositoryItemColorEdit1;
            this.colColor.FieldName = "ObjColor";
            this.colColor.Name = "colColor";
            this.colColor.Visible = true;
            this.colColor.VisibleIndex = 5;
            this.colColor.Width = 66;
            // 
            // repositoryItemColorEdit1
            // 
            this.repositoryItemColorEdit1.AutoHeight = false;
            this.repositoryItemColorEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemColorEdit1.Name = "repositoryItemColorEdit1";
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "需用系数";
            this.gridColumn1.DisplayFormat.FormatString = "##0.##";
            this.gridColumn1.FieldName = "ObligateField2";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 6;
            this.gridColumn1.Width = 66;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "容积率";
            this.gridColumn2.FieldName = "ObligateField3";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 7;
            this.gridColumn2.Width = 94;
            // 
            // CtrlglebeType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl);
            this.Name = "CtrlglebeType";
            this.Size = new System.Drawing.Size(870, 319);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemColorEdit1)).EndInit();
            this.ResumeLayout(false);

		}		#endregion		private DevExpress.XtraGrid.GridControl gridControl;		private DevExpress.XtraGrid.Views.Grid.GridView gridView;		private DevExpress.XtraGrid.Columns.GridColumn colUID;		private DevExpress.XtraGrid.Columns.GridColumn colTypeID;		private DevExpress.XtraGrid.Columns.GridColumn colTypeName;		private DevExpress.XtraGrid.Columns.GridColumn colTypeStyle;
        private DevExpress.XtraGrid.Columns.GridColumn colColor;
        private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit repositoryItemColorEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }}
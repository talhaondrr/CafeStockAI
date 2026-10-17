using System.ComponentModel;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
namespace CafeStockAI.Desktop;
public sealed class StockSheet : UserControl
{
 private readonly Panel scroll=new(){Dock=DockStyle.Fill,AutoScroll=true};
 private readonly List<GridView> grids=new();
 public event Action<string>? InvalidInput;
 public event Action<Product>? ProductSelected;
 internal IReadOnlyList<GridView> Grids=>grids;
 public StockSheet(){BackColor=Color.FromArgb(243,237,227);Controls.Add(scroll);}
 public void CancelEdit(){foreach(var view in grids)view.HideEditor();}
 public bool EndEdit(){foreach(var view in grids){if(!view.PostEditor())return false;view.CloseEditor();if(!view.UpdateCurrentRow())return false;}return true;}
 private static int Lane(CountDraft row)=>row.Product?.SourceCell.TrimStart().FirstOrDefault() switch{'A'=>0,'D'=>1,'G'=>2,_=>3};
 private static int RowOrder(CountDraft row)=>int.TryParse(new string((row.Product?.SourceCell??"").Where(char.IsDigit).ToArray()),out int number)?number:10000;
 public void SetRows(IEnumerable<CountDraft> source,bool editable,bool productsOnly=false)
 {
  if(!EndEdit())return;
  var groups=source.GroupBy(r=>string.IsNullOrWhiteSpace(r.Category)?"Kategorisiz":r.Category,StringComparer.CurrentCultureIgnoreCase).OrderBy(g=>g.Min(Lane)).ThenBy(g=>g.Min(RowOrder)).ThenBy(g=>g.Key).ToList();
  int columns=Math.Max(1,Math.Min(4,groups.Count));
  scroll.SuspendLayout();try
  {
   foreach(Control control in scroll.Controls.Cast<Control>().ToArray())control.Dispose();grids.Clear();
   var table=new TableLayoutPanel{Dock=DockStyle.Top,AutoSize=true,AutoSizeMode=AutoSizeMode.GrowAndShrink,ColumnCount=columns,RowCount=1,MinimumSize=new Size(Math.Max(350,columns*320),0),Padding=new Padding(0,0,8,16)};
   for(int i=0;i<columns;i++)table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F/columns));table.RowStyles.Add(new RowStyle(SizeType.AutoSize));scroll.Controls.Add(table);
   var lanes=Enumerable.Range(0,columns).Select(i=>new TableLayoutPanel{Dock=DockStyle.Top,AutoSize=true,AutoSizeMode=AutoSizeMode.GrowAndShrink,ColumnCount=1,RowCount=0,Margin=new Padding(4)}).ToArray();
   for(int i=0;i<columns;i++){lanes[i].ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));table.Controls.Add(lanes[i],i,0);}
   var heights=new int[columns];
   foreach(var group in groups)
   {
    var rows=group.OrderBy(RowOrder).ThenBy(r=>r.Name).ToList();int lane=Array.IndexOf(heights,heights.Min());int height=66+rows.Count*32;heights[lane]+=height+12;
    var card=new TableLayoutPanel{ColumnCount=1,RowCount=2,Dock=DockStyle.Fill,MinimumSize=new Size(300,height),Height=height,Margin=new Padding(0,0,0,12),BackColor=Color.White};
    var title=new Label{Text=$"{group.Key}  ·  {rows.Count}",Margin=Padding.Empty,Dock=DockStyle.Fill,TextAlign=ContentAlignment.MiddleLeft,Padding=new Padding(8,0,0,0),BackColor=Color.FromArgb(98,71,50),ForeColor=Color.White,Font=new Font("Segoe UI",9,FontStyle.Bold)};
    var grid=new GridControl{Name="sheet-"+group.Key,Margin=Padding.Empty,Dock=DockStyle.Fill};
    var view=new GridView(grid);grid.MainView=view;grid.ViewCollection.Add(view);
    view.OptionsBehavior.AutoPopulateColumns=false;view.OptionsBehavior.Editable=editable;
    view.OptionsView.ShowGroupPanel=false;view.OptionsView.ShowIndicator=false;view.OptionsView.ShowVerticalLines=DefaultBoolean.False;view.OptionsView.EnableAppearanceEvenRow=true;
    view.OptionsCustomization.AllowSort=false;view.OptionsCustomization.AllowFilter=false;view.OptionsCustomization.AllowColumnMoving=false;view.OptionsCustomization.AllowColumnResizing=false;
    view.HorzScrollVisibility=DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;view.VertScrollVisibility=DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
    view.RowHeight=28;view.ColumnPanelRowHeight=28;
    view.Appearance.Row.Font=new Font("Segoe UI",8.5F);view.Appearance.EvenRow.BackColor=Color.FromArgb(250,247,242);
    view.Appearance.HeaderPanel.BackColor=Color.FromArgb(235,225,211);view.Appearance.HeaderPanel.ForeColor=Color.FromArgb(77,54,36);view.Appearance.HeaderPanel.Font=new Font("Segoe UI",8,FontStyle.Bold);
    view.Appearance.FocusedCell.BackColor=Color.FromArgb(233,215,191);
    var picture=Add(view,"Picture"," ",26);picture.UnboundDataType=typeof(Image);picture.OptionsColumn.FixedWidth=true;
    var pictureEdit=new RepositoryItemPictureEdit{SizeMode=DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom};grid.RepositoryItems.Add(pictureEdit);picture.ColumnEdit=pictureEdit;
    view.CustomUnboundColumnData+=(_,e)=>{if(e.IsGetData&&e.Column==picture&&rows.ElementAtOrDefault(e.ListSourceRowIndex) is CountDraft r&&r.Product is not null)e.Value=ProductImages.Get(r.Product);};
    Add(view,"Name","Ürün",160);Add(view,"Unit","Birim",50);
    if(productsOnly)AddNumber(view,"ExpectedDepot","Min. stok",false);
    else{AddNumber(view,editable?"DepotCounted":"ExpectedDepot","Depo",editable);AddNumber(view,editable?"BarCounted":"ExpectedBar","Bar",editable);AddNumber(view,"Total","Toplam",false);}
    var input=new RepositoryItemTextEdit();grid.RepositoryItems.Add(input);input.Appearance.TextOptions.HAlignment=HorzAlignment.Center;
    input.ParseEditValue+=(_,e)=>{if(string.IsNullOrWhiteSpace(Convert.ToString(e.Value))){e.Value=null;e.Handled=true;}else if(QuantityText.TryParse(Convert.ToString(e.Value),out var value)){e.Value=value;e.Handled=true;}};
    foreach(GridColumn col in view.Columns)if(col.OptionsColumn.AllowEdit)col.ColumnEdit=input;
    view.ValidatingEditor+=(_,e)=>{if(e.Value is not null&&(!QuantityText.TryParse(Convert.ToString(e.Value),out var value)||value<0)){e.Valid=false;e.ErrorText="Sıfır veya pozitif bir miktar giriniz.";InvalidInput?.Invoke(e.ErrorText);}};
    view.InvalidValueException+=(_,e)=>e.ExceptionMode=DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
    view.CustomColumnDisplayText+=(_,e)=>{if(e.Value is decimal value)e.DisplayText=QuantityText.Format(value);};
    view.RowCellClick+=(_,e)=>{if(productsOnly&&view.GetRow(e.RowHandle) is CountDraft r&&r.Product is not null)ProductSelected?.Invoke(r.Product);};
    grid.DataSource=new BindingList<CountDraft>(rows);grids.Add(view);
    card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));card.RowStyles.Add(new RowStyle(SizeType.Absolute,30));card.RowStyles.Add(new RowStyle(SizeType.Percent,100));card.Controls.Add(title,0,0);card.Controls.Add(grid,0,1);
    var target=lanes[lane];target.RowStyles.Add(new RowStyle(SizeType.AutoSize));target.Controls.Add(card,0,target.RowCount++);
   }
  }finally{scroll.ResumeLayout(true);}
 }
 private static GridColumn Add(GridView view,string field,string caption,int width){var col=view.Columns.AddVisible(field,caption);col.Width=width;col.MinWidth=width==55?50:Math.Min(width,42);col.OptionsColumn.AllowEdit=false;col.OptionsColumn.ReadOnly=true;return col;}
 private static void AddNumber(GridView view,string field,string caption,bool editable){var col=Add(view,field,caption,55);col.OptionsColumn.AllowEdit=editable;col.OptionsColumn.ReadOnly=!editable;col.AppearanceCell.TextOptions.HAlignment=HorzAlignment.Center;col.AppearanceCell.Options.UseTextOptions=true;if(editable){col.AppearanceCell.BackColor=Color.FromArgb(255,249,226);col.AppearanceCell.Options.UseBackColor=true;}col.DisplayFormat.FormatType=FormatType.Numeric;col.DisplayFormat.FormatString=QuantityText.Pattern;}
}

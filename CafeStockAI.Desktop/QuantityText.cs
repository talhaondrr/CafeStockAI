using System.Globalization;
namespace CafeStockAI.Desktop;
internal static class QuantityText
{
 public const string Pattern="0.############################";
 public static string Format(decimal value)=>value.ToString(Pattern,CultureInfo.InvariantCulture);
 public static bool TryParse(string? text,out decimal value)=>decimal.TryParse(text?.Trim().Replace(',','.'),NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out value);
 public static void Setup(Control root)
 {
  if(root is DataGridView grid)
  {
   grid.CellFormatting+=(_,e)=>{if(e.Value is decimal number){e.Value=Format(number);e.FormattingApplied=true;}};
   grid.CellParsing+=(_,e)=>{var type=grid.Columns[e.ColumnIndex].ValueType;if((type==typeof(decimal)||type==typeof(decimal?))&&TryParse(Convert.ToString(e.Value),out var number)){e.Value=number;e.ParsingApplied=true;}};
  }
  foreach(Control child in root.Controls)Setup(child);
 }
}
public class QuantityInput : NumericUpDown
{
 protected override void UpdateEditText(){ChangingText=true;Text=QuantityText.Format(Value);}
 protected override void ValidateEditText()
 {
  string input=Text;UserEdit=false;
  if(QuantityText.TryParse(input,out var number))Value=Math.Clamp(number,Minimum,Maximum);
  UpdateEditText();
 }
}

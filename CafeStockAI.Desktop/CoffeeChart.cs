using System.ComponentModel;
using System.Drawing.Drawing2D;
namespace CafeStockAI.Desktop;

public sealed class CoffeeChart : Control
{
    private int[] values = new int[7];
    public CoffeeChart() { DoubleBuffered = true; BackColor = Color.FromArgb(255,253,249); }
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int[] Values { get => values; set { values = value; Invalidate(); } }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e); e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        int max = Math.Max(1, values.DefaultIfEmpty().Max());
        int width = Math.Max(1, (Width - 32) / 7); int baseline = Height - 32; int available = Math.Max(1, baseline - 30);
        using var bar = new SolidBrush(Color.FromArgb(141,95,62));
        using var text = new SolidBrush(Color.FromArgb(115,102,88));
        using var axis = new Pen(Color.FromArgb(232,223,211));
        using var font = new Font("Segoe UI", 9F);
        for (int n = 0; n < 4; n++) { int y = baseline - available * n / 3; e.Graphics.DrawLine(axis, 15, y, Width - 15, y); }
        for (int i = 0; i < 7; i++)
        {
            int value = i < values.Length ? values[i] : 0; int x = 22 + i * width; int height = (int)(available * (double)value / max);
            if (height > 0) e.Graphics.FillRectangle(bar, x, baseline - height, Math.Max(8, width - 22), height);
            e.Graphics.DrawString(value.ToString(), font, text, x, baseline - height - 22);
            e.Graphics.DrawString(DateTime.Today.AddDays(i - 6).ToString("ddd"), font, text, x, baseline + 8);
        }
    }
}

public sealed class CoffeeCard : Panel
{
    public CoffeeCard() { DoubleBuffered = true; BackColor = Color.FromArgb(255,253,249); }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var background = new SolidBrush(Parent?.BackColor ?? Color.FromArgb(243,237,227));
        e.Graphics.FillRectangle(background, ClientRectangle);
        if (Width < 18 || Height < 18) return;
        using var path = new GraphicsPath();
        const int diameter = 18;
        path.AddArc(0,0,diameter,diameter,180,90);
        path.AddArc(Width-diameter-1,0,diameter,diameter,270,90);
        path.AddArc(Width-diameter-1,Height-diameter-1,diameter,diameter,0,90);
        path.AddArc(0,Height-diameter-1,diameter,diameter,90,90); path.CloseFigure();
        using var fill = new SolidBrush(BackColor); using var border = new Pen(Color.FromArgb(233,224,212));
        e.Graphics.FillPath(fill,path); e.Graphics.DrawPath(border,path);
    }
}

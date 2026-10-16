using System.Drawing.Drawing2D;
namespace CafeStockAI.Desktop;
public sealed class CoffeeArt : Control
{
    public CoffeeArt() { DoubleBuffered = true; BackColor = Color.FromArgb(35,33,30); }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
        float scale = Math.Min(Width / 360F, Height / 290F);
        if (scale <= 0) return;
        g.TranslateTransform(Width / 2F, Height / 2F); g.ScaleTransform(scale,scale);
        using var halo = new SolidBrush(Color.FromArgb(60,48,38));
        using var warm = new SolidBrush(Color.FromArgb(212,172,120));
        using var cream = new SolidBrush(Color.FromArgb(244,230,207));
        using var coffee = new SolidBrush(Color.FromArgb(72,43,26));
        using var line = new Pen(Color.FromArgb(221,188,145),5F);
        using var steam = new Pen(Color.FromArgb(157,124,89),3F);
        g.FillEllipse(halo,-132,-125,264,264);
        g.DrawArc(steam,-44,-118,28,60,270,150); g.DrawArc(steam,-8,-132,30,70,90,150); g.DrawArc(steam,29,-115,26,54,270,150);
        g.FillEllipse(warm,-110,62,220,31); g.FillEllipse(cream,-92,58,184,25);
        g.DrawArc(line,58,-28,61,72,270,180);
        using var cup = new GraphicsPath(); cup.AddLine(-77,-29,77,-29); cup.AddBezier(77,-29,72,89,-72,89,-77,-29); cup.CloseFigure();
        g.FillPath(cream,cup); g.FillEllipse(warm,-77,-49,154,40); g.FillEllipse(coffee,-65,-43,130,27);
        using var latte = new Pen(Color.FromArgb(230,202,158),2F);
        g.DrawArc(latte,-32,-36,64,16,0,310); g.DrawArc(latte,-21,-33,42,10,0,300);
        g.ResetTransform();
    }
}

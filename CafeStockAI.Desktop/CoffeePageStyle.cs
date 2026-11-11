namespace CafeStockAI.Desktop;

internal static class CoffeePageStyle
{
    internal static readonly Color Canvas = Color.FromArgb(247, 241, 232);
    internal static readonly Color Espresso = Color.FromArgb(74, 41, 27);
    internal static readonly Color Accent = Color.FromArgb(111, 55, 33);
    internal static readonly Color Sand = Color.FromArgb(242, 233, 224);

    internal static void Apply(Form page, Label? title, params Button[] primaryActions)
    {
        page.BackColor = Canvas;
        if (title is not null)
        {
            title.Font = new Font("Georgia", 21, FontStyle.Bold);
            title.ForeColor = Espresso;
        }

        foreach (var button in primaryActions)
        {
            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Accent;
            button.ForeColor = Color.White;
        }

        foreach (var grid in Descendants(page).OfType<DataGridView>())
        {
            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Color.FromArgb(255, 253, 249);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.GridColor = Color.FromArgb(230, 220, 209);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Sand;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Espresso;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Sand;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Espresso;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(253, 250, 246);
        }
    }

    internal static void Frame(TableLayoutPanel layout, int row, Padding padding)
    {
        if (layout.GetControlFromPosition(0, row) is not { } content)
            return;

        layout.SuspendLayout();
        var card = new CoffeeCard { Dock = DockStyle.Fill, Padding = padding, Margin = content.Margin };
        content.Margin = Padding.Empty;
        content.Dock = DockStyle.Fill;
        card.Controls.Add(content);
        layout.Controls.Add(card, 0, row);
        layout.ResumeLayout(true);
    }

    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;
            foreach (var descendant in Descendants(child))
                yield return descendant;
        }
    }
}

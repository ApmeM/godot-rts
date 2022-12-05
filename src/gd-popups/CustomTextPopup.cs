using Godot;

using GodotAnalysers;

[SceneReference("CustomTextPopup.tscn")]
[Tool]
public partial class CustomTextPopup
{
    [Export(PropertyHint.MultilineText)] 
    public string Text { get; set; }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        this.popupLabel.Text = this.Text;
    }
}

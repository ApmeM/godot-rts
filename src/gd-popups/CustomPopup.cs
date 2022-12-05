using Godot;

using GodotAnalysers;
using GodotRts.Presentation.Utils;

[SceneReference("CustomPopup.tscn")]
[Tool]
public partial class CustomPopup
{
    [Signal]
    public delegate void PopupClosed();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.popupBackButton.Connect(CommonSignals.Pressed, this, nameof(BackButtonPressed));
    }

    private void BackButtonPressed()
    {
        this.Hide();
        this.EmitSignal(nameof(PopupClosed));
    }
}

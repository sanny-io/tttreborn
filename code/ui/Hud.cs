using Sandbox;
using Sandbox.UI;

public partial class Hud : HudEntity<RootPanel>
{
	public Hud()
	{
		if ( !IsClient )
			return;

		// s&box defaults
		RootPanel.AddChild<NameTags>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<KillFeed>();
		RootPanel.AddChild<VoiceList>();
		
		RootPanel.AddChild<Crosshair>().SetupCrosshair(new Crosshair.Properties());
	}

	[ClientRpc]
	public void OnPlayerDied( string victim, string attacker = null )
	{
		Host.AssertClient();
	}

	[ClientRpc]
	public void ShowDeathScreen( string attackerName )
	{
		Host.AssertClient();
	}
}

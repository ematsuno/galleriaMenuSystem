using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainMenu : SimpleMenu<MainMenu>
{
	public async void OnPlayPressed()
	{
		GameMenu.Show();
	}

    public async void OnOptionsPressed()
    {
		OptionsMenu.Show();
	}

	public async override void OnBackPressed()
	{
		Application.Quit();
	}
}

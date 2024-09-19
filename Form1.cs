using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace mpvHelper
{
	public partial class Form1 : Form
	{
		private string videoPath;
		private Dictionary<string, List<string>> audioPaths = new Dictionary<string, List<string>>();
		private Dictionary<string, List<string>> subPaths = new Dictionary<string, List<string>>();

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Directory.SetCurrentDirectory("D:\\Фильмы\\Tokidoki Bosotto Russia-go de Dereru Tonari no Alya-san [2024][WEB-DL][1080p]");

			textBox1.Text = Config.GetPlayerPath();

			string curDir = Directory.GetCurrentDirectory();
			int curDirLength = curDir.Length;

			foreach (var file in Directory.EnumerateFiles(curDir, "*.*", SearchOption.AllDirectories))
			{
				string extension = Path.GetExtension(file);
				if (extension == ".mkv")
				{
					if (videoPath == null)
						videoPath = Path.GetDirectoryName(file);
				}
				else if (extension == ".mka")
				{
					string dir = Path.GetDirectoryName(file).Substring(curDirLength).TrimStart('\\');
					if (dir == "")
						dir = "<Текущая директория>";

					if (!audioPaths.ContainsKey(dir))
					{
						audioPaths.Add(dir, new List<string>());
						listBox2.Items.Add(dir);
					}
					audioPaths[dir].Add(file);
				}
				else if (extension == ".ass")
				{
					string dir = Path.GetDirectoryName(file).Substring(curDirLength).TrimStart('\\');
					if (dir == "")
						dir = "<Текущая директория>";

					if (!subPaths.ContainsKey(dir))
					{
						subPaths.Add(dir, new List<string>());
						listBox3.Items.Add(dir);
					}
					subPaths[dir].Add(file);
				}
			}

			if (videoPath != null)
			{
				foreach (var file in Directory.EnumerateFiles(videoPath, "*.mkv", SearchOption.TopDirectoryOnly))
				{
					listBox1.Items.Add(Path.GetFileNameWithoutExtension(file));
				}
			}
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			if (listBox1.SelectedItem == null)
				return;

			var sb = new StringBuilder($"/c {textBox1.Text} \"{Path.Combine(this.videoPath, (string)listBox1.SelectedItem)}.mkv\"");

			if (listBox2.SelectedItem != null)
				sb.Append($" --audio-file=\"{audioPaths[(string)listBox2.SelectedItem].Find(x => x.Contains((string)listBox1.SelectedItem))}\"");
			if (listBox3.SelectedItem != null)
				sb.Append($" --sub-file=\"{subPaths[(string)listBox3.SelectedItem].Find(x => x.Contains((string)listBox1.SelectedItem))}\"");

			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = sb.ToString();
			process.StartInfo = startInfo;
			process.Start();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() != DialogResult.OK)
				return;

			textBox1.Text = dlg.FileName;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Config.SetPlayerPath(textBox1.Text);
		}
	}
}

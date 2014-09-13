
namespace NetFocus.Components.CommandBar
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	
	public abstract class CommandBarControl : CommandBarItem
	{
		public event EventHandler Click;

		protected CommandBarControl(string text) : base(text)
		{
		}

		protected CommandBarControl(Image image) : base(image)
		{
		}

		protected CommandBarControl(Image image, string text) : base(image, text)
		{
		}

		protected virtual void OnClick(EventArgs e)
		{
			if (this.Click != null)
			{
				this.Click(this, e);
			}
		}

		internal void PerformClick(EventArgs e)
		{
			this.OnClick(e);
		}
	}
}
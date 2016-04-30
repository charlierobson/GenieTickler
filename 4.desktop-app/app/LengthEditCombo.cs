using System;
using System.Drawing;
using System.Windows.Forms;

namespace USB_Generic_HID_reference_application
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class LengthEditCombo : ComboBox
    {
		public LengthEditCombo()
		{
            Items.AddRange(new object[]{ "0x100", "0x200", "0x400", "0x800", "0x1000", "0x2000", "0x4000", "0x8000" });
            SelectedIndex = 2;
		}

		public int Value
		{
			get
			{
                return Convert.ToInt32(Items[SelectedIndex]);
			}
		}
	}
}

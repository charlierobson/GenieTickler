using System.Drawing;
using System.Windows.Forms;

namespace USB_Generic_HID_reference_application
{
    [System.ComponentModel.DesignerCategory("Code")]
    class BitEditPanel : Panel
    {
        private int _bitCount;

        private TextBox _valueEdit;

        private CheckBox[] _valueBits;

        public BitEditPanel(string fieldName, int bitCount)
        {
            _bitCount = bitCount;

            _valueBits = new CheckBox[_bitCount];

            Value = 0;

            var x = 4;

            var label = new Label
            {
                Text = fieldName,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(x, 7),
                AutoSize = true
            };
            Controls.Add(label);

            _valueEdit = new TextBox
            {
                Text = FormattedValue,
                TextAlign = HorizontalAlignment.Left, Location = new Point(x + label.Width, 4)
            };
            _valueEdit.KeyPress += (o, args) =>
            {
                if (args.KeyChar == 13)
                {
                    try
                    {
                        var hexval = _valueEdit.Text;

                        if (hexval.StartsWith("0x", System.StringComparison.CurrentCultureIgnoreCase))
                        {
                            hexval = hexval.Substring(2);
                        }

                        int v = int.Parse(hexval, System.Globalization.NumberStyles.HexNumber);

                        Value = v;
                        EncodeBits(v);

                        Invalidate();
                        return;
                    }
                    catch
                    {
                    }
                }
            };
            Controls.Add(_valueEdit);

            for (var i = _bitCount - 1; i > -1; --i)
            {
                var check = new CheckBox { Tag = i, Location = new Point(x, 34), Size = new Size(16, 16) };
                check.CheckedChanged += (o, args) =>
                {
                    if (check.Checked)
                        Value = Value | 1 << (int)check.Tag;
                    else
                        Value = Value & ~(1 << (int)check.Tag);

                    _valueEdit.Text = FormattedValue;

                    Invalidate();
                };

                _valueBits[i] = check;
                Controls.Add(check);

                x += 16;
            }
        }

        public int Value { get; set; }

        public string FormattedValue { get { return _bitCount == 8 ? string.Format("0x{0:X2}", Value) : string.Format("0x{0:X4}", Value); } }

        private void EncodeBits(int data)
        {
            // assumes that the checkboxes in the collection are added in place postion order 0 -> N

            var bitMask = 1;

            foreach (var bit in _valueBits)
            {
                bit.Checked = (data & bitMask) != 0;
                bitMask <<= 1;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace era_tw_trainer
{
    class UpdateButton : Button
    {
        List<CustomizedTextBox> textBoxes;
        Dictionary<CustomizedTextBox, string> targetValues;
        Dictionary<CustomizedTextBox, bool> forceWriteValue;

        public UpdateButton(string name)
        {
            Name = name;
            textBoxes = new List<CustomizedTextBox>();
            targetValues = new Dictionary<CustomizedTextBox, string>();
            forceWriteValue = new Dictionary<CustomizedTextBox, bool>();
            Click += (o, e) => update();
        }

        public void addTextBox(CustomizedTextBox textbox)
        {
            textBoxes.Add(textbox);
            forceWriteValue[textbox] = false;
        }

        public void addTextBox(CustomizedTextBox textbox, string targetValue)
        {
            textBoxes.Add(textbox);
            targetValues[textbox] = targetValue;
            forceWriteValue[textbox] = false;
        }

        public void addTextBox(CustomizedTextBox textbox, string targetValue, bool forceWrite)
        {
            textBoxes.Add(textbox);
            targetValues[textbox] = targetValue;
            forceWriteValue[textbox] = forceWrite;
        }

        public void update()
        {
            textBoxes.Where(b => b.Enabled).ToList().ForEach(b => {
                var v = b.Text;
                if (targetValues.ContainsKey(b))
                {
                    var v1 = targetValues[b];
                    if (int.Parse(v1) > int.Parse(v) || forceWriteValue[b])
                    {
                        v = v1;
                    }
                }
                b.writeVal(v);
            });
        }
    }
}

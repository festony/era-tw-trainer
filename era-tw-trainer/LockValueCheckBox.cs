using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace era_tw_trainer
{
    class LockValueCheckBox : CheckBox
    {
        public List<CustomizedTextBox> textBoxes;

        public LockValueCheckBox(string name)
        {
            Name = name;
            Text = "Lock";
            textBoxes = new List<CustomizedTextBox>();
        }

        public void addTextBox(CustomizedTextBox textbox)
        {
            textBoxes.Add(textbox);
        }
    }
}

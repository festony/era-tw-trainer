using era_tw_trainer.TrainerRelated;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace era_tw_trainer
{
    class CustomizedTextBox : System.Windows.Forms.TextBox
    {
        public int value;
        public Trainer trainer;
        public GameToon toon;
        public AreaIndexes index;
        public int field;

        public CustomizedTextBox(String name, Trainer trainer, GameToon toon, AreaIndexes index, int field)
        {
            Name = name;
            Trace.WriteLine("-bubububu ---------");
            value = trainer.readToonProp(toon, index, field);
            Trace.WriteLine("-bubububu --xxxx------- " + value);
            this.trainer = trainer;
            this.toon = toon;
            this.index = index;
            this.field = field;
            Text = value.ToString();
            //Validated += new System.EventHandler(this.updateVal);
        }

        public void updateVal()
        {
            try
            {
                //Trace.WriteLine("-x--x------failed to parse input " + ex);
                var val = int.Parse(Text);
                if (!trainer.writeToonProp64(toon, index, field, val))
                {
                    Text = value.ToString();
                    return;
                }
                value = val;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("-x--x------failed to parse input " + ex);
            }
        }
    }
}

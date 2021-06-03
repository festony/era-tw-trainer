using era_tw_trainer.TrainerRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace era_tw_trainer
{
    class CustomizedButton : System.Windows.Forms.Button
    {
        public int value;
        public int minValue;
        public int maxValue;
        public Dictionary<int, String> textDict;
        public Trainer trainer;
        public GameToon toon;
        public AreaIndexes index;
        public int field;

        public CustomizedButton(String name, Dictionary<int, String> textDict, Trainer trainer, GameToon toon, AreaIndexes index, int field, int minValue, int maxValue)
        {
            Name = name;
            value = trainer.readToonProp(toon, index, field);

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.textDict = textDict;
            this.trainer = trainer;
            this.toon = toon;
            this.index = index;
            this.field = field;
            if (value < minValue || value > maxValue) {
                value = minValue;
            }
            Text = this.textDict[value];
            Click += new System.EventHandler(this.inc);
        }

        public void inc(object sender, EventArgs e)
        {
            value++;
            if (value < minValue)
            {
                value = minValue;
            }
            else if (value > maxValue)
            {
                value = minValue;
            }
            if (!trainer.writeToonProp64(toon, index, field, value))
            {
                return;
            }
            this.Text = textDict[value];
        }
    }
}

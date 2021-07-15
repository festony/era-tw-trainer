using era_tw_trainer.TrainerRelated;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace era_tw_trainer
{
    interface ICustomizedControl
    {
        void readVal();
        void writeVal();
        void writeVal(String val);
    }

    class CustomizedTextBox : TextBox, ICustomizedControl
    {
        public int value;
        public Trainer trainer;
        public GameToon toon;
        public AreaIndexes index;
        public int field;

        public CustomizedTextBox(String name, Trainer trainer, GameToon toon, AreaIndexes index, int field)
        {
            Name = name;
            value = trainer.readToonProp(toon, index, field);
            this.trainer = trainer;
            this.toon = toon;
            this.index = index;
            this.field = field;
            Text = value.ToString();
        }

        public void readVal()
        {
            value = trainer.readToonProp(toon, index, field);
            Text = value.ToString();
        }

        public void writeVal()
        {
            try
            {
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
                Trace.WriteLine("Failed to parse input. " + ex);
            }
        }

        public void writeVal(String val)
        {
            Text = val;
            writeVal();
        }
    }

    class CustomizedButton : Button, ICustomizedControl
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

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.textDict = textDict;
            this.trainer = trainer;
            this.toon = toon;
            this.index = index;
            this.field = field;

            value = trainer.readToonProp(toon, index, field);
            if (value < minValue)
            {
                value = minValue;
            }
            else if (value > maxValue)
            {
                value = maxValue;
            }
            Text = this.textDict[value];

            Click += (o, e) => inc();
        }

        public void readVal()
        {
            value = trainer.readToonProp(toon, index, field);
            if (value < minValue)
            {
                value = minValue;
            }
            else if (value > maxValue)
            {
                value = maxValue;
            }
            Text = this.textDict[value];
        }

        public void writeVal()
        {
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

        public void writeVal(String val)
        {
            value = int.Parse(val);
            writeVal();
        }

        public void inc()
        {
            writeVal((value + 1).ToString());
        }
    }
}

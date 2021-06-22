using era_tw_trainer.TrainerRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace era_tw_trainer
{
    public partial class MainForm : Form
    {
        private Trainer trainer;
        private List<Control> customizedControls = new List<Control>();

        public MainForm()
        {
            InitializeComponent();
            trainer = new Trainer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trainer.getGameProcessHandle();
            trainer.initScanMem();
            //trainer.getMarkFromBaseAddr(new IntPtr(0x2561FF54C10));
            Trace.WriteLine("------xxx1111--- ");
            Trace.WriteLine("------xxx--- " + trainer.tryConstructGameToonFromPotentialMark54ToonBlock(new IntPtr(0x65C3E4D8)));
            var baseAddr04 = trainer.searchFor04MarkFrom54Mark(new IntPtr(0x65C3E4D8));
            Trace.WriteLine("------bbbbbb " + baseAddr04.ToInt64().ToString("X"));
            //Trace.WriteLine("------ " + trainer.getNameAddrFromPotentialMark04NameBlock(new IntPtr(0x22ADD70E730)));
            //trainer.getMarkFromBaseAddr(new IntPtr(0x22ADCC61F50));
            //Trace.WriteLine("------ " + trainer.readName(new IntPtr(0x22ADCC90598)));
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            trainer.getGameProcessHandle();
            trainer.initScanMem();
            for (int i = 0; i<trainer.names.Count; i++)
            {
                listBoxToons.Items.Add(trainer.toons[i].name);
                if (trainer.player == trainer.toons[i])
                {
                    listBoxToons.Items[i] = "***" + trainer.toons[i].name;
                }
            }
        }

        private void loadToon()
        {
            int lineHeight = 28;

            foreach (var ctr in customizedControls)
            {
                this.Controls.Remove(ctr);
            }
            customizedControls = new List<Control>();

            GameToon toon = trainer.toons[listBoxToons.SelectedIndex];

            textBox54Addr.Text = toon.mark54Addr.ToInt64().ToString("X");
            textBoxStatusSubAddr.Text = toon.getStatusAreaBaseAddr().ToInt64().ToString("X");
            textBoxKeYinSubAddr.Text = toon.getKeYinAreaBaseAddr().ToInt64().ToString("X");
            textBoxHaoGanSubAddr.Text = toon.getHaoGanAreaBaseAddr().ToInt64().ToString("X");
            textBoxSkillsSubAddr.Text = toon.getSkillsAreaBaseAddr().ToInt64().ToString("X");
            textBoxFeatsSubAddr.Text = toon.getFeaturesAreaBaseAddr().ToInt64().ToString("X");
            textBoxPalamSubAddr.Text = toon.getPalamAreaBaseAddr().ToInt64().ToString("X");


            textBoxName.Text = toon.name;
            int v = trainer.readToonProp(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.体力);
            textBoxStamina.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.体力);
            textBoxMaxStamina.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.气力);
            textBoxPower.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.气力);
            textBoxMaxPower.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.酒气);
            textBoxDrunk.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.酒气);
            textBoxMaxDrunk.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.精力);
            textBoxSpirit.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.精力);
            textBoxMaxSpirit.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.TPS);
            textBoxTps.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.TPS);
            textBoxMaxTps.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.情绪);
            textBoxStatusMood.Text = v.ToString();
            v = trainer.readToonProp(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.理性);
            textBoxStatusRational.Text = v.ToString();
            textBoxTps.Enabled = toon == trainer.player;
            textBoxMaxTps.Enabled = toon == trainer.player;





            int i = 0;
            foreach(var field in Enum.GetValues(typeof(KeYinAreaFields)))
            {
                var keyinButton = new CustomizedButton("customizedButtonKeyin_" + i, new Dictionary<int, string> { { 0, field.ToString() + ": -" }, { 1, field.ToString() + ": 1" }, { 2, field.ToString() + ": 2" }, { 3, field.ToString() + ": 3" } }, trainer, toon, AreaIndexes.KE_YIN_AREA, (int)field, 0, 3);
                if (i < 3)
                {
                    keyinButton.Location = new System.Drawing.Point(labelKeYin.Location.X, labelKeYin.Location.Y + lineHeight * (i + 1));
                }
                else
                {
                    keyinButton.Location = new System.Drawing.Point(labelKeYin.Location.X + 175, labelKeYin.Location.Y + lineHeight * (i - 2));
                }
                keyinButton.Size = new System.Drawing.Size(125, 23);
                keyinButton.TabIndex = 200 + i;
                keyinButton.UseVisualStyleBackColor = true;

                this.Controls.Add(keyinButton);
                customizedControls.Add(keyinButton);

                i++;
            }



            v = trainer.readToonProp(toon, AreaIndexes.HAO_GAN_AREA, (int)HaoGanAreaFields.好感);
            textBoxHaoGan.Text = v.ToString();
            textBoxHaoGan.Enabled = toon != trainer.player;
            v = trainer.readToonProp(toon, AreaIndexes.HAO_GAN_AREA, (int)HaoGanAreaFields.依赖);
            textBoxYiLai.Text = v.ToString();
            textBoxYiLai.Enabled = toon != trainer.player;

            //var valTextBox = new CustomizedTextBox("customizedTextBox___0", trainer, toon, AreaIndexes.SKILLS_AREA, (int)SkillsAreaFeelFields.C);
            //valTextBox.Location = new System.Drawing.Point(344, 342);
            //valTextBox.Size = new System.Drawing.Size(350, 21);
            //valTextBox.TabIndex = 300;
            //customizedControls.Add(valTextBox);
            //this.Controls.Add(valTextBox);


            //var textBoxGroup1 = new List<CustomizedTextBox>();
            //textBoxGroup1.Add(valTextBox);

            //buttonUpdateSkills.Click += (o, e) => textBoxGroup1.ForEach(t => t.updateVal());

            //Trace.WriteLine("-x--xxx           " + valTextBox.Enabled);
            //Trace.WriteLine("-x--xxx   3        " + valTextBox.ReadOnly);
            //Trace.WriteLine("-x--xxx   5        " + valTextBox.Text);

            //textBox1.Text = valTextBox.value.ToString();
            //valTextBox.Text = textBox1.Text;

            var textBoxSkillsFieldsList = new List<CustomizedTextBox>();

            var bumpGeneralSkillsFieldsList = new List<CustomizedTextBox>();

            i = 0;
            foreach (var field in new SkillsAreaGeneralFields[] { SkillsAreaGeneralFields.亲密, SkillsAreaGeneralFields.从顺, SkillsAreaGeneralFields.欲望, SkillsAreaGeneralFields.技巧, SkillsAreaGeneralFields.侍奉, SkillsAreaGeneralFields.露出 })
            {
                var valTextBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                valTextBox.Location = new System.Drawing.Point(buttonBumpGeneralSkills.Location.X, buttonBumpGeneralSkills.Location.Y + lineHeight * (i + 1));
                valTextBox.Size = new System.Drawing.Size(45, 21);
                valTextBox.TabIndex = 300 + i;
                this.Controls.Add(valTextBox);
                customizedControls.Add(valTextBox);
                textBoxSkillsFieldsList.Add(valTextBox);
                bumpGeneralSkillsFieldsList.Add(valTextBox);

                i++;
            }
            buttonBumpGeneralSkills.Click += (o, e) => bumpGeneralSkillsFieldsList.ForEach(t => { t.Text = "50"; t.updateVal(); });


            var bumpLiveSkillsFieldsList = new List<CustomizedTextBox>();

            i = 0;
            foreach (var field in new SkillsAreaLiveSkillsFields[] { SkillsAreaLiveSkillsFields.教养, SkillsAreaLiveSkillsFields.话术, SkillsAreaLiveSkillsFields.战斗, SkillsAreaLiveSkillsFields.清扫, SkillsAreaLiveSkillsFields.料理, SkillsAreaLiveSkillsFields.音乐 })
            {
                var valTextBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                valTextBox.Location = new System.Drawing.Point(buttonBumpLiveSkills.Location.X, buttonBumpLiveSkills.Location.Y + lineHeight * (i + 1));
                valTextBox.Size = new System.Drawing.Size(45, 21);
                valTextBox.TabIndex = 310 + i;
                this.Controls.Add(valTextBox);
                customizedControls.Add(valTextBox);
                textBoxSkillsFieldsList.Add(valTextBox);
                bumpLiveSkillsFieldsList.Add(valTextBox);

                i++;
            }
            buttonBumpLiveSkills.Click += (o, e) => bumpLiveSkillsFieldsList.ForEach(t => { t.Text = "6"; t.updateVal(); });


            var bumpFeelSkillsFieldsList = new List<CustomizedTextBox>();

            i = 0;
            foreach (var field in new SkillsAreaFeelFields[] { SkillsAreaFeelFields.C, SkillsAreaFeelFields.V, SkillsAreaFeelFields.A, SkillsAreaFeelFields.B, SkillsAreaFeelFields.M })
            {
                var valTextBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                valTextBox.Location = new System.Drawing.Point(buttonBumpFeelSkills.Location.X, buttonBumpFeelSkills.Location.Y + lineHeight * (i + 1));
                valTextBox.Size = new System.Drawing.Size(45, 21);
                valTextBox.TabIndex = 320 + i;
                this.Controls.Add(valTextBox);
                customizedControls.Add(valTextBox);
                textBoxSkillsFieldsList.Add(valTextBox);
                bumpFeelSkillsFieldsList.Add(valTextBox);

                i++;
            }
            buttonBumpFeelSkills.Click += (o, e) => bumpFeelSkillsFieldsList.ForEach(t => { t.Text = "50"; t.updateVal(); });


            var bumpHSkillsFieldsList = new List<CustomizedTextBox>();

            i = 0;
            foreach (var field in new SkillsAreaHSkillsFields[] { SkillsAreaHSkillsFields.指, SkillsAreaHSkillsFields.腔, SkillsAreaHSkillsFields.A, SkillsAreaHSkillsFields.胸, SkillsAreaHSkillsFields.舌, SkillsAreaHSkillsFields.腰 })
            {
                var valTextBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                valTextBox.Location = new System.Drawing.Point(buttonBumpHSkills.Location.X, buttonBumpHSkills.Location.Y + lineHeight * (i + 1));
                valTextBox.Size = new System.Drawing.Size(45, 21);
                valTextBox.TabIndex = 330 + i;
                this.Controls.Add(valTextBox);
                customizedControls.Add(valTextBox);
                textBoxSkillsFieldsList.Add(valTextBox);
                bumpHSkillsFieldsList.Add(valTextBox);

                i++;
            }
            buttonBumpHSkills.Click += (o, e) => bumpHSkillsFieldsList.ForEach(t => { t.Text = "6"; t.updateVal(); });


            var bumpXPSkillsFieldsList = new List<CustomizedTextBox>();

            i = 0;
            foreach (var field in new SkillsAreaXPFields[] { SkillsAreaXPFields.M, SkillsAreaXPFields.S, SkillsAreaXPFields.百合 })
            {
                var valTextBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                valTextBox.Location = new System.Drawing.Point(buttonBumpXPSkills.Location.X, buttonBumpXPSkills.Location.Y + lineHeight * (i + 1));
                valTextBox.Size = new System.Drawing.Size(45, 21);
                valTextBox.TabIndex = 340 + i;
                this.Controls.Add(valTextBox);
                customizedControls.Add(valTextBox);
                textBoxSkillsFieldsList.Add(valTextBox);
                bumpXPSkillsFieldsList.Add(valTextBox);

                i++;
            }
            buttonBumpXPSkills.Click += (o, e) => bumpXPSkillsFieldsList.ForEach(t => { t.Text = "50"; t.updateVal(); });


            var bumpAddictSkillsFieldsList = new List<CustomizedTextBox>();

            i = 0;
            foreach (var field in new SkillsAreaAddictFields[] { SkillsAreaAddictFields.自慰, SkillsAreaAddictFields.精液, SkillsAreaAddictFields.百合, SkillsAreaAddictFields.腔射, SkillsAreaAddictFields.肛射 })
            {
                var valTextBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                valTextBox.Location = new System.Drawing.Point(buttonBumpAddictSkills.Location.X, buttonBumpAddictSkills.Location.Y + lineHeight * (i + 1));
                valTextBox.Size = new System.Drawing.Size(45, 21);
                valTextBox.TabIndex = 340 + i;
                this.Controls.Add(valTextBox);
                customizedControls.Add(valTextBox);
                textBoxSkillsFieldsList.Add(valTextBox);
                bumpAddictSkillsFieldsList.Add(valTextBox);

                i++;
            }
            buttonBumpAddictSkills.Click += (o, e) => bumpAddictSkillsFieldsList.ForEach(t => { t.Text = "50"; t.updateVal(); });

            buttonUpdateSkills.Click += (o, e) => textBoxSkillsFieldsList.ForEach(t => t.updateVal());



            var lineHeightLess = 25;
            var colWidthLess = 122;

            i = 0;


            //var fff = FeaturesAreaFields.J大小;
            //Dictionary<int, String> textDict = new Dictionary<int, string>();
            //var range = FeatureFieldsValueRanges.getValueRange(fff);
            //for (int j = range[0]; j <= range[1]; j++)
            //{
            //    textDict[j] = fff.ToString() + ":" + j;
            //}
            //var featButton = new CustomizedButton("customizedButtonFeat_" + i, textDict, trainer, toon, AreaIndexes.FEATURES_AREA, (int)fff, range[0], range[1]);
            //featButton.Location = new System.Drawing.Point(textBoxFeatsSubAddr.Location.X, textBoxFeatsSubAddr.Location.Y + lineHeightLess * (i + 1));
            ////featButton.Location = new System.Drawing.Point(350, 500);
            //Trace.WriteLine("------fdsfs --- " + textBoxFeatsSubAddr.Location.X);
            //Trace.WriteLine("------fdsfs --- " + textBoxFeatsSubAddr.Location.Y);
            //featButton.Size = new System.Drawing.Size(120, 23);
            //featButton.TabIndex = 400 + i;
            //featButton.UseVisualStyleBackColor = true;

            //this.Controls.Add(featButton);
            //customizedControls.Add(featButton);


            var H = 15;
            foreach (FeaturesAreaFields field in Enum.GetValues(typeof(FeaturesAreaFields)))
            {
                Dictionary<int, String> textDict = new Dictionary<int, string>();
                var range = FeatureFieldsValueRanges.getValueRange(field);
                for (int j = range[0]; j <= range[1]; j++)
                {
                    textDict[j] = field.ToString() + ":" + j;
                }
                var featButton = new CustomizedButton("customizedButtonFeat_" + i, textDict, trainer, toon, AreaIndexes.FEATURES_AREA, (int)field, range[0], range[1]);
                var ix = i / H;
                var iy = i % H;
                featButton.Location = new System.Drawing.Point(textBoxFeatsSubAddr.Location.X + colWidthLess * ix, textBoxFeatsSubAddr.Location.Y + lineHeightLess * (iy + 1));
                //featButton.Location = new System.Drawing.Point(350, 500);
                //Trace.WriteLine("------fdsfs --- " + textBoxFeatsSubAddr.Location.X);
                //Trace.WriteLine("------fdsfs --- " + textBoxFeatsSubAddr.Location.Y);
                featButton.Size = new System.Drawing.Size(120, 23);
                featButton.TabIndex = 400 + i;
                featButton.UseVisualStyleBackColor = true;

                this.Controls.Add(featButton);
                customizedControls.Add(featButton);

                i++;
            }
            //Trace.WriteLine("------fdsfs --- " + textBoxFeatsSubAddr.Location.Y);




            lineHeightLess = 25;
            colWidthLess = 100;

            var textBoxPalamFieldsList = new List<CustomizedTextBox>();
            var bumpPalamKFieldsList = new List<CustomizedTextBox>();

            i = 0;
            foreach (var field in Enum.GetValues(typeof(PalamAreaFields)))
            {
                var valTextBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.PALAM_AREA, (int)field);
                var x = i % 5;
                var y = i / 5;
                valTextBox.Location = new System.Drawing.Point(textBoxPalamSubAddr.Location.X + colWidthLess * x, textBoxPalamSubAddr.Location.Y + lineHeightLess * (y + 1));
                valTextBox.Size = new System.Drawing.Size(55, 21);
                valTextBox.TabIndex = 300 + i;
                this.Controls.Add(valTextBox);
                customizedControls.Add(valTextBox);
                textBoxPalamFieldsList.Add(valTextBox);
                if (new HashSet<PalamAreaFields> { PalamAreaFields.快A, PalamAreaFields.快B, PalamAreaFields.快C, PalamAreaFields.快M, PalamAreaFields.快V}.Contains((PalamAreaFields)field))
                {
                    bumpPalamKFieldsList.Add(valTextBox);
                }

                i++;
            }
            buttonMaxPalamK.Click += (o, e) => bumpPalamKFieldsList.ForEach(t => { t.Text = "5000000"; t.updateVal(); });
            buttonUpdatePalam.Click += (o, e) => textBoxPalamFieldsList.ForEach(t => t.updateVal());

            buttonMaxMood.Click += (o, e) =>
            {
                textBoxStatusMood.Text = "100000";
                textBoxStatusRational.Text = "0";
                trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.情绪, int.Parse(textBoxStatusMood.Text));
                trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.理性, int.Parse(textBoxStatusRational.Text));
            };
        }

        private void listBoxToons_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadToon();
        }

        private void buttonUpdateStatus_Click(object sender, EventArgs e)
        {
            GameToon toon = trainer.toons[listBoxToons.SelectedIndex];
            trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.体力, int.Parse(textBoxStamina.Text));
            trainer.writeToonProp64(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.体力, int.Parse(textBoxMaxStamina.Text));
            trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.气力, int.Parse(textBoxPower.Text));
            trainer.writeToonProp64(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.气力, int.Parse(textBoxMaxPower.Text));
            trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.酒气, int.Parse(textBoxDrunk.Text));
            trainer.writeToonProp64(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.酒气, int.Parse(textBoxMaxDrunk.Text));
            trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.精力, int.Parse(textBoxSpirit.Text));
            trainer.writeToonProp64(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.精力, int.Parse(textBoxMaxSpirit.Text));
            trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.情绪, int.Parse(textBoxStatusMood.Text));
            trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.理性, int.Parse(textBoxStatusRational.Text));
            if (toon == trainer.player)
            {
                trainer.writeToonProp64(toon, AreaIndexes.STATUS_AREA, (int)StatusAreaFields.TPS, int.Parse(textBoxTps.Text));
                trainer.writeToonProp64(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.TPS, int.Parse(textBoxMaxTps.Text));
            }

            //var xxx = new HashSet<Char>();

            //foreach(var t in trainer.toons)
            //{
            //    var name = t.name;
            //    foreach(var c in name.ToCharArray())
            //    {
            //        xxx.Add(c);
            //    }
            //}

            //var s = "";
            //foreach(var c in xxx)
            //{
            //    s += c;
            //}

            //Trace.WriteLine("######### " + s);
        }

        private void buttonUpdateHaoGan_Click(object sender, EventArgs e)
        {
            GameToon toon = trainer.toons[listBoxToons.SelectedIndex];
            if (toon != trainer.player)
            {
                trainer.writeToonProp64(toon, AreaIndexes.HAO_GAN_AREA, (int)HaoGanAreaFields.好感, int.Parse(textBoxHaoGan.Text));
                trainer.writeToonProp64(toon, AreaIndexes.HAO_GAN_AREA, (int)HaoGanAreaFields.依赖, int.Parse(textBoxYiLai.Text));
            }
        }

        private void textBoxSearchByNamePinyin_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchByNamePinyin.Text))
            {
                clearSearchRes();
                return;
            }
            var indexes = trainer.findNameMatch(textBoxSearchByNamePinyin.Text);
            updateSearchResult(indexes);
        }

        private void updateSearchResult(List<int> indexes)
        {
            listBoxToons.Items.Clear();
            for(int i=0; i< trainer.names.Count; i++)
            {
                if (indexes.Contains(i))
                {
                    listBoxToons.Items.Add("    ##" + trainer.toons[i].name + "####");
                }
                else
                {
                    listBoxToons.Items.Add(trainer.toons[i].name);
                }
            }
        }

        private void clearSearchRes()
        {
            listBoxToons.Items.Clear();
            for (int i = 0; i < trainer.names.Count; i++)
            {
                listBoxToons.Items.Add(trainer.toons[i].name);
            }
        }

        private void buttonMaxHaogan_Click(object sender, EventArgs e)
        {
            bool update = false;
            if (string.IsNullOrWhiteSpace(textBoxHaoGan.Text) || int.Parse(textBoxHaoGan.Text) < 60000)
            {
                update = true;
                textBoxHaoGan.Text = "60000";
            }
            if (string.IsNullOrWhiteSpace(textBoxYiLai.Text) || int.Parse(textBoxYiLai.Text) < 200)
            {
                update = true;
                textBoxYiLai.Text = "200";
            }

            if(update)
            {
                buttonUpdateHaoGan.PerformClick();
            }
        }
    }
}

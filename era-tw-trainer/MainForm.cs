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

        private void resetSomeButtons()
        {
            this.Controls.Remove(buttonBumpGeneralSkills);
            this.Controls.Remove(buttonBumpLiveSkills);
            this.Controls.Remove(buttonBumpFeelSkills);
            this.Controls.Remove(buttonBumpHSkills);
            this.Controls.Remove(buttonBumpXPSkills);
            this.Controls.Remove(buttonBumpAddictSkills);
            this.Controls.Remove(buttonUpdateSkills);
            this.Controls.Remove(buttonMaxPalamK);
            this.Controls.Remove(buttonUpdatePalam);
            this.Controls.Remove(buttonMaxMood);
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
            buttonBumpGeneralSkills.Click += (o, e) => bumpGeneralSkillsFieldsList.ForEach(t => { t.Text = "50"; t.writeVal(); });


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
            buttonBumpLiveSkills.Click += (o, e) => bumpLiveSkillsFieldsList.ForEach(t => { t.Text = "6"; t.writeVal(); });


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
            buttonBumpFeelSkills.Click += (o, e) => bumpFeelSkillsFieldsList.ForEach(t => { t.Text = "50"; t.writeVal(); });


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
            buttonBumpHSkills.Click += (o, e) => bumpHSkillsFieldsList.ForEach(t => { t.Text = "6"; t.writeVal(); });


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
            buttonBumpXPSkills.Click += (o, e) => bumpXPSkillsFieldsList.ForEach(t => { t.Text = "50"; t.writeVal(); });


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
            buttonBumpAddictSkills.Click += (o, e) => bumpAddictSkillsFieldsList.ForEach(t => { t.Text = "50"; t.writeVal(); });

            buttonUpdateSkills.Click += (o, e) => textBoxSkillsFieldsList.ForEach(t => t.writeVal());



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
            buttonMaxPalamK.Click += (o, e) => bumpPalamKFieldsList.ForEach(t => { t.Text = "5000000"; t.writeVal(); });
            buttonUpdatePalam.Click += (o, e) => textBoxPalamFieldsList.ForEach(t => t.writeVal());

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



        /*
        private List<Control> createPanelForToon(TrainerRelated.GameToon toon)
        {
            // TODO: work out a way to calc tabIndex

            List<Control> controls = new List<Control>();

            // Name
            TextBox textBoxName = new TextBox();
            textBoxName.Location = new Point(labelName.Location.X + 41, labelName.Location.Y);
            textBoxName.Name = "textBoxName";
            textBoxName.ReadOnly = true;
            textBoxName.Size = new Size(150, 21);

            textBoxName.Text = toon.name;
            controls.Add(textBoxName);

            // Main address
            TextBox textBox54Addr = new TextBox();
            textBox54Addr.Location = new Point(textBoxName.Location.X + 156, textBoxName.Location.Y);
            textBox54Addr.Name = "textBox54Addr";
            textBox54Addr.ReadOnly = true;
            textBox54Addr.Size = new Size(80, 21);
            //textBox54Addr.TabIndex = 22;

            textBox54Addr.Text = toon.mark54Addr.ToInt64().ToString("X");
            controls.Add(textBox54Addr);

            // Hao Gan Area
            // Hao Gan sub address
            TextBox textBoxHaoGanSubAddr = new TextBox();
            textBoxHaoGanSubAddr.Location = new Point(labelHaoGan.Location.X + 47, labelHaoGan.Location.Y);
            textBoxHaoGanSubAddr.Name = "textBoxHaoGanSubAddr";
            textBoxHaoGanSubAddr.ReadOnly = true;
            textBoxHaoGanSubAddr.Size = new Size(80, 21);
            //textBoxHaoGanSubAddr.TabIndex = 22;

            textBoxHaoGanSubAddr.Text = toon.getHaoGanAreaBaseAddr().ToInt64().ToString("X");
            controls.Add(textBoxHaoGanSubAddr);

            // Buttons and textboxes
            UpdateButton updateButtonHaoGan = new UpdateButton("updateButton_HaoGan");
            UpdateButton updateButtonHaoGanStage1 = new UpdateButton("updateButton_HaoGan_Stage1");
            UpdateButton updateButtonHaoGanStage2 = new UpdateButton("updateButton_HaoGan_Stage2");
            UpdateButton updateButtonHaoGanStage3 = new UpdateButton("updateButton_HaoGan_Stage3");

            Dictionary<HaoGanAreaFields, List<string>> haoganTargetValues = new Dictionary<HaoGanAreaFields, List<string>>();
            haoganTargetValues[HaoGanAreaFields.好感] = new List<string> { "60000", "65000", "70000" };
            haoganTargetValues[HaoGanAreaFields.依赖] = new List<string> { "200", "249", "699" };


            int width = 50;
            int height = 21;
            int deltaX = width + 6;
            int x = textBoxHaoGanSubAddr.Location.X + 86;
            int y = textBoxHaoGanSubAddr.Location.Y;

            foreach (var field in new HaoGanAreaFields[] { HaoGanAreaFields.好感, HaoGanAreaFields.依赖, HaoGanAreaFields.妊娠, HaoGanAreaFields.登场 })
            {
                CustomizedTextBox textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.HAO_GAN_AREA, (int)field);
                textBox.Location = new Point(x, y);
                x += deltaX;
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 22;

                textBox.readVal();
                controls.Add(textBox);
                updateButtonHaoGan.addTextBox(textBox);
                if (haoganTargetValues.ContainsKey(field))
                {
                    updateButtonHaoGanStage1.addTextBox(textBox, haoganTargetValues[field][0]);
                    updateButtonHaoGanStage2.addTextBox(textBox, haoganTargetValues[field][1]);
                    updateButtonHaoGanStage3.addTextBox(textBox, haoganTargetValues[field][2]);
                }
                if (field == HaoGanAreaFields.登场)
                {
                    textBox.Enabled = false;
                }
            }

            updateButtonHaoGan.Size = new Size(width, height);
            updateButtonHaoGan.Location = new Point(x, y);
            updateButtonHaoGan.Text = "Update";
            x += deltaX;
            controls.Add(updateButtonHaoGan);

            updateButtonHaoGanStage1.Size = new Size(width, height);
            updateButtonHaoGanStage1.Location = new Point(x, y);
            updateButtonHaoGanStage1.Text = "Stage1";
            x += deltaX;
            controls.Add(updateButtonHaoGanStage1);

            updateButtonHaoGanStage2.Size = new Size(width, height);
            updateButtonHaoGanStage2.Location = new Point(x, y);
            updateButtonHaoGanStage2.Text = "Stage2";
            x += deltaX;
            controls.Add(updateButtonHaoGanStage2);

            updateButtonHaoGanStage3.Size = new Size(width, height);
            updateButtonHaoGanStage3.Location = new Point(x, y);
            updateButtonHaoGanStage3.Text = "Stage3";
            x += deltaX;
            controls.Add(updateButtonHaoGanStage3);
            // Hao Gan Area End


            // Ke Yin Area
            // Ke Yin sub address
            TextBox textBoxKeYinSubAddr = new TextBox();
            textBoxKeYinSubAddr.Location = new Point(labelKeYin.Location.X + 47, labelKeYin.Location.Y);
            textBoxKeYinSubAddr.Name = "textBoxKeYinSubAddr";
            textBoxKeYinSubAddr.ReadOnly = true;
            textBoxKeYinSubAddr.Size = new Size(80, 21);
            //textBoxKeYinSubAddr.TabIndex = 22;

            textBoxKeYinSubAddr.Text = toon.getKeYinAreaBaseAddr().ToInt64().ToString("X");
            controls.Add(textBoxKeYinSubAddr);

            width = 60;
            height = 21;
            deltaX = width + 6;
            x = textBoxKeYinSubAddr.Location.X + 86;
            y = textBoxKeYinSubAddr.Location.Y;

            foreach (var field in Enum.GetValues(typeof(KeYinAreaFields)))
            {
                var keyinButton = new CustomizedButton("customizedButtonKeyin_" + field.ToString(), new Dictionary<int, string> { { 0, field.ToString() + ":-" }, { 1, field.ToString() + ":1" }, { 2, field.ToString() + ":2" }, { 3, field.ToString() + ":3" } }, trainer, toon, AreaIndexes.KE_YIN_AREA, (int)field, 0, 3);
                keyinButton.Location = new Point(x, y);
                x += deltaX;

                keyinButton.Size = new Size(width, height);
                //keyinButton.TabIndex = 200 + i;
                keyinButton.UseVisualStyleBackColor = true;

                controls.Add(keyinButton);
            }
            // Ke Yin Area End

            // Status Area
            // Status sub address
            TextBox textBoxStatusSubAddr = new TextBox();
            textBoxStatusSubAddr.Location = new Point(labelStatus.Location.X + 47, labelStatus.Location.Y);
            textBoxStatusSubAddr.Name = "textBoxStatusSubAddr";
            textBoxStatusSubAddr.ReadOnly = true;
            textBoxStatusSubAddr.Size = new Size(80, 21);
            //textBoxStatusSubAddr.TabIndex = 22;

            textBoxStatusSubAddr.Text = toon.getStatusAreaBaseAddr().ToInt64().ToString("X");
            controls.Add(textBoxStatusSubAddr);

            UpdateButton updateButtonStatus = new UpdateButton("updateButton_Status");
            UpdateButton updateButtonMaxMood = new UpdateButton("updateButton_MaxMood");
            UpdateButton updateButtonBumpStatus = new UpdateButton("updateButton_BumpStatus");

            LockValueCheckBox lockValueCheckBoxStatus = new LockValueCheckBox("lockValueCheckBox_Status");
            lockValueCheckBoxStatus.Text = "Lock status";
            lockValueCheckBoxStatus.Location = new Point(labelStatus.Location.X, labelStatus.Location.Y + 29);
            lockValueCheckBoxStatus.Size = new Size(100, 21);

            controls.Add(lockValueCheckBoxStatus);

            LockValueCheckBox lockValueCheckBoxShoot = new LockValueCheckBox("lockValueCheckBox_Shoot");
            lockValueCheckBoxShoot.Text = "Lock shoot";
            lockValueCheckBoxShoot.Location = new Point(lockValueCheckBoxStatus.Location.X, lockValueCheckBoxStatus.Location.Y + 29);
            lockValueCheckBoxShoot.Size = new Size(100, 21);

            controls.Add(lockValueCheckBoxShoot);

            width = 60;
            height = 21;
            deltaX = width + 6;
            x = textBoxStatusSubAddr.Location.X + 86;
            y = textBoxStatusSubAddr.Location.Y;

            HashSet<StatusAreaFields> lockStatusFields = new HashSet<StatusAreaFields> { StatusAreaFields.体力, StatusAreaFields.气力, StatusAreaFields.精力, StatusAreaFields.TPS };
            Dictionary<StatusAreaFields, string> adjustMoodValues = new Dictionary<StatusAreaFields, string> { { StatusAreaFields.情绪, "1500" }, { StatusAreaFields.理性, "0" } };

            List<CustomizedTextBox> statusFieldsForBump = new List<CustomizedTextBox>();
            List<CustomizedTextBox> maxStatusFieldsForBump = new List<CustomizedTextBox>();

            foreach (var field in new StatusAreaFields[] { StatusAreaFields.体力, StatusAreaFields.气力, StatusAreaFields.精力, StatusAreaFields.TPS, StatusAreaFields.酒气, StatusAreaFields.射精, StatusAreaFields.情绪, StatusAreaFields.理性 })
            {
                CustomizedTextBox textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.STATUS_AREA, (int)field);
                textBox.Location = new Point(x, y + 29);
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 22;

                textBox.readVal();
                controls.Add(textBox);
                updateButtonStatus.addTextBox(textBox);
                if (lockStatusFields.Contains(field))
                {
                    lockValueCheckBoxStatus.addTextBox(textBox);
                    statusFieldsForBump.Add(textBox);
                }
                if (field == StatusAreaFields.射精)
                {
                    lockValueCheckBoxShoot.addTextBox(textBox);
                    if (toon == trainer.player)
                    {
                        updateButtonBumpStatus.addTextBox(textBox, "200000");
                    }
                }
                if (adjustMoodValues.ContainsKey(field))
                {
                    updateButtonMaxMood.addTextBox(textBox, adjustMoodValues[field], true);
                }

                CustomizedTextBox textBoxMax = new CustomizedTextBox("customizedTextBox_Max_" + field.ToString(), trainer, toon, AreaIndexes.MAX_STATUS_AREA, (int)field);
                textBoxMax.Location = new Point(x, y);
                x += deltaX;
                textBoxMax.Size = new Size(width, height);
                textBoxMax.readVal();
                controls.Add(textBoxMax);
                updateButtonStatus.addTextBox(textBoxMax);

                if (lockStatusFields.Contains(field))
                {
                    maxStatusFieldsForBump.Add(textBoxMax);
                }

                if (toon != trainer.player && field == StatusAreaFields.TPS)
                {
                    textBox.Enabled = false;
                    textBoxMax.Enabled = false;
                }
            }

            if (toon != trainer.player)
            {
                lockValueCheckBoxShoot.Enabled = false;
            }

            updateButtonStatus.Size = new Size(width, height);
            updateButtonStatus.Location = new Point(x, y);
            updateButtonStatus.Text = "Update";
            x += deltaX;
            controls.Add(updateButtonStatus);

            updateButtonMaxMood.Size = new Size(width, height);
            updateButtonMaxMood.Location = new Point(updateButtonStatus.Location.X, updateButtonStatus.Location.Y + 29);
            updateButtonMaxMood.Text = "MaxMood";
            controls.Add(updateButtonMaxMood);

            updateButtonBumpStatus.Size = new Size(width, height);
            updateButtonBumpStatus.Location = new Point(textBoxStatusSubAddr.Location.X + 86, textBoxStatusSubAddr.Location.Y + 29 * 2);
            updateButtonBumpStatus.Text = "Bump";

            updateButtonBumpStatus.Click += (o, e) => {
                enableLockLoop = false;

                for (int j = 0; j < maxStatusFieldsForBump.Count; j++)
                {
                    statusFieldsForBump[j].writeVal(maxStatusFieldsForBump[j].Text);
                }

                enableLockLoop = true;
            };

            controls.Add(updateButtonBumpStatus);
            // Status Area End

            // Palam Area
            // Palam sub address
            TextBox textBoxPalamSubAddr = new TextBox();
            textBoxPalamSubAddr.Location = new Point(labelPalam.Location.X + 47, labelPalam.Location.Y);
            textBoxPalamSubAddr.Name = "textBoxPalamSubAddr";
            textBoxPalamSubAddr.ReadOnly = true;
            textBoxPalamSubAddr.Size = new Size(80, 21);
            //textBoxPalamSubAddr.TabIndex = 22;

            textBoxPalamSubAddr.Text = toon.getPalamAreaBaseAddr().ToInt64().ToString("X");
            controls.Add(textBoxPalamSubAddr);

            // Buttons and textboxes
            UpdateButton updateButtonPalam = new UpdateButton("updateButton_Palam");
            UpdateButton updateButtonPalamBumpK = new UpdateButton("updateButton_Palam_BumpK");
            UpdateButton updateButtonPalamMaxK = new UpdateButton("updateButton_Palam_MaxK");

            LockValueCheckBox lockValueCheckBoxPalamK = new LockValueCheckBox("lockValueCheckBox_PalamK");
            lockValueCheckBoxPalamK.Text = "Lock K";
            lockValueCheckBoxPalamK.Size = new Size(100, 21);

            width = 60;
            height = 21;
            deltaX = width + 6;
            var deltaY = height + 8;
            x = textBoxPalamSubAddr.Location.X + 86;
            y = textBoxPalamSubAddr.Location.Y;


            int i = 0;
            foreach (var field in Enum.GetValues(typeof(PalamAreaFields)))
            {
                var textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.PALAM_AREA, (int)field);
                var c = i % 5;
                var r = i / 5;
                textBox.Location = new Point(textBoxPalamSubAddr.Location.X + deltaX * c, textBoxPalamSubAddr.Location.Y + deltaY * (r + 1));
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 300 + i;
                textBox.readVal();
                controls.Add(textBox);
                updateButtonPalam.addTextBox(textBox);
                if (new HashSet<PalamAreaFields> { PalamAreaFields.快A, PalamAreaFields.快B, PalamAreaFields.快C, PalamAreaFields.快M, PalamAreaFields.快V }.Contains((PalamAreaFields)field))
                {
                    updateButtonPalamBumpK.addTextBox(textBox, "9999");
                    updateButtonPalamMaxK.addTextBox(textBox, "9999999");
                    lockValueCheckBoxPalamK.addTextBox(textBox);
                }

                i++;
            }

            x = textBoxPalamSubAddr.Location.X + 86;
            y = textBoxPalamSubAddr.Location.Y;

            updateButtonPalam.Size = new Size(width, height);
            updateButtonPalam.Location = new Point(x, y);
            updateButtonPalam.Text = "Update";
            x += deltaX;
            controls.Add(updateButtonPalam);

            updateButtonPalamBumpK.Size = new Size(width, height);
            updateButtonPalamBumpK.Location = new Point(x, y);
            updateButtonPalamBumpK.Text = "BumpK";
            x += deltaX;
            controls.Add(updateButtonPalamBumpK);

            updateButtonPalamMaxK.Size = new Size(width, height);
            updateButtonPalamMaxK.Location = new Point(x, y);
            updateButtonPalamMaxK.Text = "MaxK";
            x += deltaX;
            controls.Add(updateButtonPalamMaxK);

            lockValueCheckBoxPalamK.Location = new Point(x, y);
            controls.Add(lockValueCheckBoxPalamK);
            // Palam Area

            // Skills Area
            // Skills sub address
            TextBox textBoxSkillsSubAddr = new TextBox();
            textBoxSkillsSubAddr.Location = new Point(labelSkills.Location.X + 47, labelSkills.Location.Y);
            textBoxSkillsSubAddr.Name = "textBoxHaoGanSubAddr";
            textBoxSkillsSubAddr.ReadOnly = true;
            textBoxSkillsSubAddr.Size = new Size(80, 21);
            //textBoxSkillsSubAddr.TabIndex = 22;

            textBoxSkillsSubAddr.Text = toon.getSkillsAreaBaseAddr().ToInt64().ToString("X");
            controls.Add(textBoxSkillsSubAddr);

            width = 60;
            height = 21;
            deltaX = width + 6;
            deltaY = height + 8;

            // Buttons and textboxes
            UpdateButton updateButtonSkills = new UpdateButton("updateButton_Skills");
            updateButtonSkills.Size = new Size(width, height);
            updateButtonSkills.Location = new Point(textBoxSkillsSubAddr.Location.X, textBoxSkillsSubAddr.Location.Y + deltaY);
            updateButtonSkills.Text = "Update";
            controls.Add(updateButtonSkills);

            UpdateButton updateButtonBumpServe = new UpdateButton("updateButton_BumpServe");
            updateButtonBumpServe.Size = new Size(width, height);
            updateButtonBumpServe.Location = new Point(textBoxSkillsSubAddr.Location.X, textBoxSkillsSubAddr.Location.Y + deltaY * 2);
            updateButtonBumpServe.Text = "Serv";
            controls.Add(updateButtonBumpServe);

            x = textBoxSkillsSubAddr.Location.X + 86;
            y = textBoxSkillsSubAddr.Location.Y;

            UpdateButton updateButtonBumpGeneralSkills = new UpdateButton("updateButton_BumpGeneralSkills");
            updateButtonBumpGeneralSkills.Size = new Size(width, height);
            updateButtonBumpGeneralSkills.Location = new Point(x, y);
            x += deltaX;
            updateButtonBumpGeneralSkills.Text = "Bump";
            controls.Add(updateButtonBumpGeneralSkills);

            UpdateButton updateButtonBumpLiveSkills = new UpdateButton("updateButton_BumpLiveSkills");
            updateButtonBumpLiveSkills.Size = new Size(width, height);
            updateButtonBumpLiveSkills.Location = new Point(x, y);
            x += deltaX;
            updateButtonBumpLiveSkills.Text = "Bump";
            controls.Add(updateButtonBumpLiveSkills);

            UpdateButton updateButtonBumpFeelSkills = new UpdateButton("updateButton_BumpFeelSkills");
            updateButtonBumpFeelSkills.Size = new Size(width, height);
            updateButtonBumpFeelSkills.Location = new Point(x, y);
            x += deltaX;
            updateButtonBumpFeelSkills.Text = "Bump";
            controls.Add(updateButtonBumpFeelSkills);

            UpdateButton updateButtonBumpHSkills = new UpdateButton("updateButton_BumpHSkills");
            updateButtonBumpHSkills.Size = new Size(width, height);
            updateButtonBumpHSkills.Location = new Point(x, y);
            x += deltaX;
            updateButtonBumpHSkills.Text = "Bump";
            controls.Add(updateButtonBumpHSkills);

            UpdateButton updateButtonBumpXpSkills = new UpdateButton("updateButton_BumpXpSkills");
            updateButtonBumpXpSkills.Size = new Size(width, height);
            updateButtonBumpXpSkills.Location = new Point(x, y);
            x += deltaX;
            updateButtonBumpXpSkills.Text = "Bump";
            controls.Add(updateButtonBumpXpSkills);

            UpdateButton updateButtonBumpAddictSkills = new UpdateButton("updateButton_BumpAddictSkills");
            updateButtonBumpAddictSkills.Size = new Size(width, height);
            updateButtonBumpAddictSkills.Location = new Point(x, y);
            x += deltaX;
            updateButtonBumpAddictSkills.Text = "Bump";
            controls.Add(updateButtonBumpAddictSkills);


            x = updateButtonBumpGeneralSkills.Location.X;
            y = updateButtonBumpGeneralSkills.Location.Y + deltaY;

            foreach (var field in new SkillsAreaGeneralFields[] { SkillsAreaGeneralFields.亲密, SkillsAreaGeneralFields.从顺, SkillsAreaGeneralFields.欲望, SkillsAreaGeneralFields.技巧, SkillsAreaGeneralFields.侍奉, SkillsAreaGeneralFields.露出 })
            {
                CustomizedTextBox textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                textBox.Location = new Point(x, y);
                y += deltaY;
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 22;

                textBox.readVal();
                controls.Add(textBox);
                updateButtonSkills.addTextBox(textBox);
                updateButtonBumpGeneralSkills.addTextBox(textBox, "120");
                if (field == SkillsAreaGeneralFields.从顺 || field == SkillsAreaGeneralFields.侍奉)
                {
                    updateButtonBumpServe.addTextBox(textBox, "120");
                }
            }

            x = updateButtonBumpLiveSkills.Location.X;
            y = updateButtonBumpLiveSkills.Location.Y + deltaY;

            foreach (var field in new SkillsAreaLiveSkillsFields[] { SkillsAreaLiveSkillsFields.教养, SkillsAreaLiveSkillsFields.话术, SkillsAreaLiveSkillsFields.战斗, SkillsAreaLiveSkillsFields.清扫, SkillsAreaLiveSkillsFields.料理, SkillsAreaLiveSkillsFields.音乐 })
            {
                CustomizedTextBox textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                textBox.Location = new Point(x, y);
                y += deltaY;
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 22;

                textBox.readVal();
                controls.Add(textBox);
                updateButtonSkills.addTextBox(textBox);
                updateButtonBumpLiveSkills.addTextBox(textBox, "6");
            }

            x = updateButtonBumpFeelSkills.Location.X;
            y = updateButtonBumpFeelSkills.Location.Y + deltaY;

            foreach (var field in new SkillsAreaFeelFields[] { SkillsAreaFeelFields.C, SkillsAreaFeelFields.V, SkillsAreaFeelFields.A, SkillsAreaFeelFields.B, SkillsAreaFeelFields.M })
            {
                CustomizedTextBox textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                textBox.Location = new Point(x, y);
                y += deltaY;
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 22;

                textBox.readVal();
                controls.Add(textBox);
                updateButtonSkills.addTextBox(textBox);
                updateButtonBumpFeelSkills.addTextBox(textBox, "120");
            }

            x = updateButtonBumpHSkills.Location.X;
            y = updateButtonBumpHSkills.Location.Y + deltaY;

            foreach (var field in new SkillsAreaHSkillsFields[] { SkillsAreaHSkillsFields.指, SkillsAreaHSkillsFields.腔, SkillsAreaHSkillsFields.A, SkillsAreaHSkillsFields.胸, SkillsAreaHSkillsFields.舌, SkillsAreaHSkillsFields.腰 })
            {
                CustomizedTextBox textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                textBox.Location = new Point(x, y);
                y += deltaY;
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 22;

                textBox.readVal();
                controls.Add(textBox);
                updateButtonSkills.addTextBox(textBox);
                updateButtonBumpHSkills.addTextBox(textBox, "6");
            }

            x = updateButtonBumpXpSkills.Location.X;
            y = updateButtonBumpXpSkills.Location.Y + deltaY;

            foreach (var field in new SkillsAreaXPFields[] { SkillsAreaXPFields.M, SkillsAreaXPFields.S, SkillsAreaXPFields.百合, SkillsAreaXPFields.断袖 })
            {
                CustomizedTextBox textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                textBox.Location = new Point(x, y);
                y += deltaY;
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 22;

                textBox.readVal();
                controls.Add(textBox);
                updateButtonSkills.addTextBox(textBox);
                updateButtonBumpXpSkills.addTextBox(textBox, "120");
            }

            x = updateButtonBumpAddictSkills.Location.X;
            y = updateButtonBumpAddictSkills.Location.Y + deltaY;

            foreach (var field in new SkillsAreaAddictFields[] { SkillsAreaAddictFields.自慰, SkillsAreaAddictFields.精液, SkillsAreaAddictFields.百合, SkillsAreaAddictFields.断袖, SkillsAreaAddictFields.腔射, SkillsAreaAddictFields.肛射 })
            {
                CustomizedTextBox textBox = new CustomizedTextBox("customizedTextBox_" + field.ToString(), trainer, toon, AreaIndexes.SKILLS_AREA, (int)field);
                textBox.Location = new Point(x, y);
                y += deltaY;
                textBox.Size = new Size(width, height);
                //textBox.TabIndex = 22;

                textBox.readVal();
                controls.Add(textBox);
                updateButtonSkills.addTextBox(textBox);
                updateButtonBumpAddictSkills.addTextBox(textBox, "120");
            }
            // Skills Area End

            // Feats Area
            // Feats sub address
            TextBox textBoxFeaturesSubAddr = new TextBox();
            textBoxFeaturesSubAddr.Location = new Point(labelStatus.Location.X + 47, labelStatus.Location.Y);
            textBoxFeaturesSubAddr.Name = "textBoxStatusSubAddr";
            textBoxFeaturesSubAddr.ReadOnly = true;
            textBoxFeaturesSubAddr.Size = new Size(80, 21);
            //textBoxFeatsSubAddr.TabIndex = 22;

            textBoxFeaturesSubAddr.Text = toon.getFeaturesAreaBaseAddr().ToInt64().ToString("X");
            controls.Add(textBoxFeaturesSubAddr);










            controls.ForEach(c => c.Hide());
            this.Controls.AddRange(controls.ToArray());

            return controls;
        }

        */



        // TODO:
    }
}

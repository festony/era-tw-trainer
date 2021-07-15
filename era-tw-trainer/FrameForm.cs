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
    public partial class FrameForm : Form
    {
        private Trainer trainer;
        private List<string> toonNames;
        private Dictionary<int, List<LockValueCheckBox>> lockValueCheckBoxLists;
        Dictionary<int, List<Control>> toonPanels;
        private int currToon = -1;
        private bool enableLockLoop = true;

        public FrameForm()
        {
            InitializeComponent();
            trainer = new Trainer();
            toonNames = new List<string>();
            lockValueCheckBoxLists = new Dictionary<int, List<LockValueCheckBox>>();
            toonPanels = new Dictionary<int, List<Control>>();
        }

        private void updateToonNameList(bool includeUnavailableToons)
        {
            toonNames.Clear();
            for (int i = 0; i < trainer.names.Count; i++)
            {
                var text = trainer.toons[i].name;
                if (trainer.player == trainer.toons[i])
                {
                    text = "***" + text;
                }
                text += "  |" + i;

                if (!includeUnavailableToons && trainer.player != trainer.toons[i] && trainer.readToonProp(trainer.toons[i], AreaIndexes.HAO_GAN_AREA, (int)HaoGanAreaFields.登场) != 0)
                {
                    continue;
                }
                toonNames.Add(text);
            }

            clearSearchRes();
        }

        private void initTrainer()
        {
            trainer.getGameProcessHandle();
            trainer.initScanMem();
            //for (int i = 0; i < trainer.names.Count; i++)
            //{
            //    var text = trainer.toons[i].name;
            //    if (trainer.player == trainer.toons[i])
            //    {
            //        text = "***" + text;
            //    }
            //    text += "  |" + i;

            //    toonNames.Add(text);
            //}

            //clearSearchRes();
            updateToonNameList(checkBoxShowUnavailableToons.Checked);
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            initTrainer();
            //var testControls = createPanelForToon(trainer.toons[8]);

            // Remove these: now do lazy loading
            //for(int i=0; i<trainer.toons.Count; i++)
            //{
            //    var controls = createPanelForToon(trainer.toons[i]);
            //    var lockValueCheckBoxes = controls.Where(c => c is LockValueCheckBox).Select(c => (LockValueCheckBox) c).ToList();
            //    toonPanels[i] = controls;
            //    lockValueCheckBoxLists[i] = lockValueCheckBoxes;
            //}
        }

        private void loadPanelForToon(int index)
        {
            if (index < 0)
            {
                return;
            }
            var controls = createPanelForToon(trainer.toons[index]);
            var lockValueCheckBoxes = controls.Where(c => c is LockValueCheckBox).Select(c => (LockValueCheckBox)c).ToList();
            toonPanels[index] = controls;
            lockValueCheckBoxLists[index] = lockValueCheckBoxes;
        }

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
            Dictionary<StatusAreaFields, string> adjustMoodValues = new Dictionary<StatusAreaFields, string> { { StatusAreaFields.情绪, "1500"}, { StatusAreaFields.理性, "0" } };

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
            updateButtonBumpStatus.Location = new Point(textBoxStatusSubAddr.Location.X + 86, textBoxStatusSubAddr.Location.Y + 29*2);
            updateButtonBumpStatus.Text = "Bump";

            updateButtonBumpStatus.Click += (o, e) => {
                enableLockLoop = false;

                for (int j=0; j<maxStatusFieldsForBump.Count; j++)
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












            controls.ForEach(c => c.Hide());
            this.Controls.AddRange(controls.ToArray());

            return controls;
        }

        private void showToon(int index)
        {
            this.toonPanels[index].ForEach(c => c.Show());
        }

        private void hideToon(int index)
        {
            this.toonPanels[index].ForEach(c => c.Hide());
        }

        private void setPanelToon(int index)
        {
            if (currToon >= 0)
            {
                hideToon(currToon);
            }
            currToon = index;
            if (!toonPanels.ContainsKey(currToon))
            {
                loadPanelForToon(currToon);
            }
            readAllValsForToon(currToon);
            showToon(currToon);
        }

        // TODO
        private void readAllValsForToon(int index)
        {
            // TODO: Exclude locked values!
            if (index < 0)
            {
                return;
            }
            enableLockLoop = false;

            var textBoxesUnderLock = new HashSet<CustomizedTextBox>(getTextBoxesUnderLockForToon(index));
            this.toonPanels[index].ForEach(c => { 
                if (c is CustomizedButton)
                {
                    ((CustomizedButton)c).readVal();
                }
                else if (c is CustomizedTextBox && !textBoxesUnderLock.Contains(c))
                {
                    ((CustomizedTextBox)c).readVal();
                }
            });

            enableLockLoop = true;
        }


        private List<string> searchNamesByPinyin(string pinyin)
        {
            var indexes = trainer.findNameMatch(pinyin);
            return indexes.Select(i => toonNames[i]).ToList();
        }

        private void updateSearchResult(List<string> newNameList)
        {
            listBoxToons.Items.Clear();
            newNameList.ForEach(n => listBoxToons.Items.Add(n));
        }

        private void clearSearchRes()
        {
            textBoxSearchByNamePinyin.Text = "";
            updateSearchResult(toonNames);
        }

        private void textBoxSearchByNamePinyin_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchByNamePinyin.Text))
            {
                clearSearchRes();
                return;
            }
            updateSearchResult(searchNamesByPinyin(textBoxSearchByNamePinyin.Text));

        }

        private List<CustomizedTextBox> getTextBoxesUnderLock()
        {
            return this.lockValueCheckBoxLists.Values.SelectMany(l => l).Where(cb => cb.Checked).SelectMany(cb => cb.textBoxes).ToList();
        }

        private List<CustomizedTextBox> getTextBoxesUnderLockForToon(int index)
        {
            if (this.lockValueCheckBoxLists.ContainsKey(index))
            {
                return this.lockValueCheckBoxLists[index].Where(cb => cb.Checked).SelectMany(cb => cb.textBoxes).ToList();
            }
            return new List<CustomizedTextBox>();
        }

        private void lockValues()
        {
            enableLockLoop = false;
            getTextBoxesUnderLock().ForEach(tb => tb.writeVal());
            enableLockLoop = true;
        }

        private void timerForLockLoop_Tick(object sender, EventArgs e)
        {
            // only do locking when not form doesn't have focus
            if (Form.ActiveForm != this && enableLockLoop)
            {
                lockValues();
                readAllValsForToon(currToon);
            }
        }

        private void listBoxToons_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedText = listBoxToons.SelectedItem.ToString();

            if (!selectedText.Contains('|'))
            {
                return;
            }

            string[] sections = selectedText.Split('|');
            var indexStr = sections[sections.Length - 1];
            setPanelToon(int.Parse(indexStr));
        }

        private void checkBoxShowUnavailableToons_CheckedChanged(object sender, EventArgs e)
        {
            updateToonNameList(checkBoxShowUnavailableToons.Checked);
        }


        // TODO:
        // 1. can do lazy loading for each toon

    }
}

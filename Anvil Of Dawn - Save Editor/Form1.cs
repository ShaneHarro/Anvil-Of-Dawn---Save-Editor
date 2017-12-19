using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Anvil_Of_Dawn___Save_Editor {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        OpenFileDialog dialog = new OpenFileDialog();
        FileStream fs;
        bool fileSelected = false;

        //11 skills
        string[] combatSkills;
        string[] magicSkills;
        string[] floorLevels;

        private void openButton_Click(object sender, EventArgs e) {
            //Only show .DAT files
            dialog.Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*";

            dialog.InitialDirectory = "C:\\GOG Games\\Anvil of Dawn\\SAVES";
            dialog.Title = "Select a .DAT save file";

            if (dialog.ShowDialog() == DialogResult.OK) {
                fileSelected = true;
            }

            if (dialog.FileName == String.Empty) {
                fileSelected = false;
                return; //user didn't select a file to open
            }

            //Read in values to fields when file is opened
            if (fileSelected == true) {


                //***********************************************************************************************************************
                ////////////////////////////////////////////  READING VALUES ///////////////////////////////////////////////////////////*
                //***********************************************************************************************************************

                inventoryList.Items.Clear();
                nameLabel.Text = "Character Name: ";
                //Open file so we can start reading in the values
                fs = new FileStream(dialog.FileName, FileMode.Open);

                int currentByte;
                int currentHealth;
                int maxHealth;
                int currentMana;
                int maxMana;
                int currentStrength;
                int maxStrength;
                int currentStamina;
                int maxStamina;
                int currentAgility;
                int maxAgility;
                int currentPower;
                int maxPower;
                int xPosition;
                int yPosition;
                int levelPosition;
                int manaRegenRate;
                int itemID;

                //skills = new string[11];
                magicSkills = new string[7];
                combatSkills = new string[4];
                floorLevels = new string[32];



                //Read in all our stats from the offsets and lengths
                currentHealth = readLittleEndian(0x4C4B, 0x02);
                maxHealth = readLittleEndian(0x4C4D, 0x02);
                currentMana = readLittleEndian(0x4C4F, 0x02);
                maxMana = readLittleEndian(0x4C51, 0x02);
                currentStrength = readLittleEndian(0x4C57, 0x01);
                maxStrength = readLittleEndian(0x4C59, 0x01);
                currentStamina = readLittleEndian(0x4C63, 0x01);
                maxStamina = readLittleEndian(0x4C63, 0x01);
                currentAgility = readLittleEndian(0x4C6B, 0x01);
                maxAgility = readLittleEndian(0x4C6D, 0x01);
                currentPower = readLittleEndian(0x4C75, 0x01);
                maxPower = readLittleEndian(0x4C77, 0x01);
                xPosition = readLittleEndian(0x4C17, 0x01);
                yPosition = readLittleEndian(0x4C1B, 0x01);
                manaRegenRate = readLittleEndian(0x4C75, 0x02);

                //Go to Character Name offset
                fs.Position = 0x4D01;
                for (int i = 0; i < 14; i++) {
                    nameLabel.Text += ConvertHex(fs.ReadByte().ToString("x"));
                }


                currentHealthBox.Text = currentHealth.ToString();
                maxHealthBox.Text = maxHealth.ToString();
                currentManaBox.Text = currentMana.ToString();
                maxManaBox.Text = maxMana.ToString();
                currentStrengthBox.Text = currentStrength.ToString();
                maxStrengthBox.Text = maxStrength.ToString();
                currentStaminaBox.Text = currentStamina.ToString();
                maxStaminaBox.Text = maxStamina.ToString();
                currentAgilityBox.Text = currentAgility.ToString();
                maxAgilityBox.Text = maxAgility.ToString();
                currentPowerBox.Text = currentPower.ToString();
                maxPowerBox.Text = maxPower.ToString();
                xPosBox.Text = xPosition.ToString();
                yPosBox.Text = yPosition.ToString();
                manaTrackBar.Value = manaRegenRate;
                manaRegenBox.Text = manaTrackBar.Value.ToString();

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Read in FLOOR LEVEL
                //Go to level position offset and start from there
                fs.Position = 0x4C54;
                currentByte = fs.ReadByte();

                //Put the current floor name into the level box
                levelCollection.SelectedIndex = currentByte;



                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Read in MAGIC skills
                //Go to earth Skill offset and start from there
                fs.Position = 0x4D29;

                for (int i = 0; i < magicSkills.Length; i++) {

                    currentByte = fs.ReadByte();

                    if (currentByte == 1) {
                        magicSkills[i] = "Initiate";
                    }

                    if (currentByte == 2) {
                        magicSkills[i] = "Neophyte";
                    }

                    if (currentByte == 3) {
                        magicSkills[i] = "Apprentice";
                    }

                    if (currentByte == 4) {
                        magicSkills[i] = "Talent";
                    }

                    if (currentByte == 5) {
                        magicSkills[i] = "Transcendent";
                    }

                    if (currentByte == 6) {
                        magicSkills[i] = "Accomplished";
                    }

                    if (currentByte == 7) {
                        magicSkills[i] = "Maren";
                    }

                    if (currentByte == 8) {
                        magicSkills[i] = "Adept";
                    }

                    if (currentByte == 9) {
                        magicSkills[i] = "Maestro";
                    }

                    if (currentByte == 10) {
                        magicSkills[i] = "Archmage";
                    }
                }

                /////////////////////////////////////////////////////////////////////////////////////////////////
                //read in COMBAT skills
                //Go to hacking Skill offset and start from there
                fs.Position = 0x4D30;

                for (int i = 0; i < combatSkills.Length; i++) {

                    currentByte = fs.ReadByte();

                    if (currentByte == 1) {
                        combatSkills[i] = "Novice";
                    }

                    if (currentByte == 2) {
                        combatSkills[i] = "Competent";
                    }

                    if (currentByte == 3) {
                        combatSkills[i] = "Trained";
                    }

                    if (currentByte == 4) {
                        combatSkills[i] = "Proficient";
                    }

                    if (currentByte == 5) {
                        combatSkills[i] = "Experienced";
                    }

                    if (currentByte == 6) {
                        combatSkills[i] = "Accomplished";
                    }

                    if (currentByte == 7) {
                        combatSkills[i] = "Skilled";
                    }

                    if (currentByte == 8) {
                        combatSkills[i] = "Seasoned";
                    }

                    if (currentByte == 9) {
                        combatSkills[i] = "Expert";
                    }

                    if (currentByte == 10) {
                        combatSkills[i] = "Master";
                    }

                }

                earthCombo.SelectedIndex = earthCombo.FindStringExact(magicSkills[0]);
                windCombo.SelectedIndex = windCombo.FindStringExact(magicSkills[1]);
                fireCombo.SelectedIndex = fireCombo.FindStringExact(magicSkills[2]);
                waterCombo.SelectedIndex = waterCombo.FindStringExact(magicSkills[3]);
                lightningCombo.SelectedIndex = lightningCombo.FindStringExact(magicSkills[4]);
                fleshCombo.SelectedIndex = fleshCombo.FindStringExact(magicSkills[5]);
                voidCombo.SelectedIndex = voidCombo.FindStringExact(magicSkills[6]);
                hackingCombo.SelectedIndex = hackingCombo.FindStringExact(combatSkills[0]);
                thrustingCombo.SelectedIndex = thrustingCombo.FindStringExact(combatSkills[1]);
                slashingCombo.SelectedIndex = slashingCombo.FindStringExact(combatSkills[2]);
                rangedCombo.SelectedIndex = rangedCombo.FindStringExact(combatSkills[3]);


                //**************************
                //Reading item values - Only partially works. Commented out while not working. * 
                //**************************
                //First check the 2nd byte of item icon to see if it's in the "1st or 2nd grouping" (i.e the 2nd byte advances by 1 after the first reaches FF)
                // fs.Position = 0x4E2E;
                // if (fs.ReadByte() == 2)
                // {
                //     firstItemGroup = false;
                // }        

                //Read the byte at the itemID position
                /*
                fs.Position = 0x4E31;
                itemID = fs.ReadByte();
                //Reset the position
                fs.Position = 0x4E31;

                //(Not sure how many times to loop because not sure how many items the player can hold)
                
                for (int i = 0; i < 255; i++) {
                    switch (itemID) {
                        //All item ID's
                        case 0:
                            inventoryList.Items.Add("Obj Fill");
                            break;
                        case 1:
                            inventoryList.Items.Add("War Armor");
                            break;
                        case 2:
                            inventoryList.Items.Add("Shell Plate Armor");
                            break;
                        case 3:
                            inventoryList.Items.Add("Chain Cloak Armor");
                            break;
                        case 4:
                            inventoryList.Items.Add("Ivory Plate Armor");
                            break;
                        case 5:
                            inventoryList.Items.Add("Fiery Jazerant Armor");
                            break;
                        case 6:
                            inventoryList.Items.Add("Serpent Scale Armor");
                            break;
                        case 7:
                            inventoryList.Items.Add("Banded Ironroot Armor");
                            break;
                        case 8:
                            inventoryList.Items.Add("Battle Axe");
                            break;
                        case 9:
                            inventoryList.Items.Add("War Hammer");
                            break;
                        case 10:
                            inventoryList.Items.Add("Broad-Bladed Sword");
                            break;
                        case 11:
                            inventoryList.Items.Add("Great Sword");
                            break;
                        case 12:
                            inventoryList.Items.Add("Crossbow");
                            break;
                        case 13:
                            inventoryList.Items.Add("Throwing Blade");
                            break;
                        case 14:
                            inventoryList.Items.Add("Spear");
                            break;
                        case 15:
                            inventoryList.Items.Add("Broad Bladed Spear");
                            break;
                        case 16:
                            inventoryList.Items.Add("Staff of Lightning Wild");
                            break;
                        case 17:
                            inventoryList.Items.Add("WhirlWind");
                            break;
                        case 18:
                            inventoryList.Items.Add("SeaMaiden");
                            break;
                        case 19:
                            inventoryList.Items.Add("CrimsonRage");
                            break;
                        case 20:
                            inventoryList.Items.Add("BloodBane");
                            break;
                        case 21:
                            inventoryList.Items.Add("BackLash");
                            break;
                        case 22:
                            inventoryList.Items.Add("WarRender");
                            break;
                        case 23:
                            inventoryList.Items.Add("SteelSunder");
                            break;
                        case 24:
                            inventoryList.Items.Add("ThunarMjolnir");
                            break;
                        case 25:
                            inventoryList.Items.Add("SkullScreamer");
                            break;
                        case 26:
                            inventoryList.Items.Add("Inferno");
                            break;
                        case 27:
                            inventoryList.Items.Add("SoulWrought");
                            break;
                        case 28:
                            inventoryList.Items.Add("Crossbow Bolt");
                            break;
                        case 29:
                            inventoryList.Items.Add("Steel Circle Key");
                            break;
                        case 30:
                            inventoryList.Items.Add("Bronze Skeleton Key");
                            break;
                        case 31:
                            inventoryList.Items.Add("Copper Sun Key");
                            break;
                        case 32:
                            inventoryList.Items.Add("Stone Claw Key");
                            break;
                        case 33:
                            inventoryList.Items.Add("Iron Dragon Key");
                            break;
                        case 34:
                            inventoryList.Items.Add("Silver Gryphon Key");
                            break;
                        case 35:
                            inventoryList.Items.Add("Gold Ankh Key");
                            break;
                        case 36:
                            inventoryList.Items.Add("Pearl Serpent Key");
                            break;
                        case 37:
                            inventoryList.Items.Add("Ivory Lion Key");
                            break;
                        case 38:
                            inventoryList.Items.Add("Jade Thorn Key");
                            break;
                        case 39:
                            inventoryList.Items.Add("Onyx Raven Key");
                            break;
                        case 40:
                            inventoryList.Items.Add("Opal Moon Key");
                            break;
                        case 41:
                            inventoryList.Items.Add("Amethyst Hex Key");
                            break;
                        case 42:
                            inventoryList.Items.Add("Emerald Leaf Key");
                            break;
                        case 43:
                            inventoryList.Items.Add("Sapphire Tear Key");
                            break;
                        case 44:
                            inventoryList.Items.Add("Gold Talon");
                            break;
                        case 45:
                            inventoryList.Items.Add("Ruby Shard");
                            break;
                        case 46:
                            inventoryList.Items.Add("Diamond");
                            break;
                        case 47:
                            inventoryList.Items.Add("Stone Ankh");
                            break;
                        case 48:
                            inventoryList.Items.Add("Silver cup");
                            break;
                        case 49:
                            inventoryList.Items.Add("Pearl");
                            break;
                        case 50:
                            inventoryList.Items.Add("Onyx Rose");
                            break;
                        case 51:
                            inventoryList.Items.Add("Iron Mark");
                            break;
                        case 52:
                            inventoryList.Items.Add("Rock");
                            break;
                        case 53:
                            inventoryList.Items.Add("Boulder");
                            break;
                        case 54:
                            inventoryList.Items.Add("War Shield");
                            break;
                        case 55:
                            inventoryList.Items.Add("Aegis");
                            break;
                        case 56:
                            inventoryList.Items.Add("BloodHaven");
                            break;
                        case 57:
                            inventoryList.Items.Add("CloudBurst");
                            break;
                        case 58:
                            inventoryList.Items.Add("War armor Helm");
                            break;
                        case 59:
                            inventoryList.Items.Add("Shell Plate Helm");
                            break;
                        case 60:
                            inventoryList.Items.Add("Chain Coif");
                            break;
                        case 61:
                            inventoryList.Items.Add("Ivory Plate Helm");
                            break;
                        case 62:
                            inventoryList.Items.Add("Jazerant Helm");
                            break;
                        case 63:
                            inventoryList.Items.Add("Serpent Scale Helm");
                            break;
                        case 64:
                            inventoryList.Items.Add("Ironroot Helm");
                            break;
                        case 65:
                            inventoryList.Items.Add("Reed Helm of Decipher");
                            break;
                        case 66:
                            inventoryList.Items.Add("Obj Fill (66)");
                            break;
                        case 67:
                            inventoryList.Items.Add("Obj Fill (67)");
                            break;
                        case 68:
                            inventoryList.Items.Add("Scroll: Heavenly Mend of Unseen Artisans");
                            break;
                        case 69:
                            inventoryList.Items.Add("Scroll: Iron Fist of Chaos");
                            break;
                        case 70:
                            inventoryList.Items.Add("Scroll: Deadly Spores of Earthen Rot");
                            break;
                        case 71:
                            inventoryList.Items.Add("Scroll: Shrouded Gale of Vengeful Winds");
                            break;
                        case 72:
                            inventoryList.Items.Add("Scroll: Ghastly Howl of Mortal Anguish");
                            break;
                        case 73:
                            inventoryList.Items.Add("Scroll: Unholy Conflagration");
                            break;
                        case 74:
                            inventoryList.Items.Add("Scroll: Ash and Cinders");
                            break;
                        case 75:
                            inventoryList.Items.Add("Scroll: Shackles of Ice");
                            break;
                        case 76:
                            inventoryList.Items.Add("Scroll: Reflections of the Lake");
                            break;
                        case 77:
                            inventoryList.Items.Add("Scroll: Laughing Skull of Thunderous Might");
                            break;
                        case 78:
                            inventoryList.Items.Add("Scroll: Roaring Column of Lightning Wild");
                            break;
                        case 79:
                            inventoryList.Items.Add("Scroll: Bane's Boiling Blood");
                            break;
                        case 80:
                            inventoryList.Items.Add("Scroll: The Strength of Titans");
                            break;
                        case 81:
                            inventoryList.Items.Add("Scroll: Ritual of Unmaking");
                            break;
                        case 82:
                            inventoryList.Items.Add("Encoded Messenger Scroll");
                            break;
                        case 83:
                            inventoryList.Items.Add("Encoded Messenger Scroll");
                            break;
                        case 84:
                            inventoryList.Items.Add("Encoded Messenger Scroll");
                            break;
                        case 85:
                            inventoryList.Items.Add("Scroll of Arcane Command");
                            break;
                        case 86:
                            inventoryList.Items.Add("Blank Parchment");
                            break;
                        case 87:
                            inventoryList.Items.Add("Scroll: Words of Opening");
                            break;
                        case 88:
                            inventoryList.Items.Add("Messenger Scroll");
                            break;
                        case 89:
                            inventoryList.Items.Add("Scroll: Orders of Command");
                            break;
                        case 90:
                            inventoryList.Items.Add("Trade Journal");
                            break;
                        case 91:
                            inventoryList.Items.Add("Scroll: High Monk's Recollections");
                            break;
                        case 92:
                            inventoryList.Items.Add("War Journal");
                            break;
                        case 93:
                            inventoryList.Items.Add("93 (scroll)");
                            break;
                        case 94:
                            inventoryList.Items.Add("94 (scroll)");
                            break;
                        case 95:
                            inventoryList.Items.Add("Lord Gryphon's Dispatch");
                            break;
                        case 96:
                            inventoryList.Items.Add("Scroll of Passage");
                            break;
                        case 97:
                            inventoryList.Items.Add("Quill Parchment: What");
                            break;
                        case 98:
                            inventoryList.Items.Add("Quill Parchment: Monks");
                            break;
                        case 99:
                            inventoryList.Items.Add("Quill Parchment: Warlord");
                            break;
                        case 100:
                            inventoryList.Items.Add("Quill Parchment: Scar");
                            break;
                        case 101:
                            inventoryList.Items.Add("Quill Parchment: Key");
                            break;
                        case 102:
                            inventoryList.Items.Add("Quill Parchment: Survivors");
                            break;
                        case 103:
                            inventoryList.Items.Add("Book of Shells");
                            break;
                        case 104:
                            inventoryList.Items.Add("Captain's Log");
                            break;
                        case 105:
                            inventoryList.Items.Add("Book of Summons");
                            break;
                        case 106:
                            inventoryList.Items.Add("Book of War");
                            break;
                        case 107:
                            inventoryList.Items.Add("Obj Fill (107)");
                            break;
                        case 108:
                            inventoryList.Items.Add("Obj Fill (108)");
                            break;
                        case 109:
                            inventoryList.Items.Add("Obj Fill (109)");
                            break;
                        case 110:
                            inventoryList.Items.Add("Obj Fill (110)");
                            break;
                        case 111:
                            inventoryList.Items.Add("Obj Fill (111)");
                            break;
                        case 112:
                            inventoryList.Items.Add("Obj Fill (112)");
                            break;
                        case 113:
                            inventoryList.Items.Add("Obj Fill (113)");
                            break;
                        case 114:
                            inventoryList.Items.Add("Obj Fill (114)");
                            break;
                        case 115:
                            inventoryList.Items.Add("Brown Leather Sack");
                            break;
                        case 116:
                            inventoryList.Items.Add("Yellow Leather Sack");
                            break;
                        case 117:
                            inventoryList.Items.Add("White Leather Sack");
                            break;
                        case 118:
                            inventoryList.Items.Add("Iron Bound Chest");
                            break;
                        case 119:
                            inventoryList.Items.Add("Bronze Bound Chest");
                            break;
                        case 120:
                            inventoryList.Items.Add("Wooden Chest");
                            break;
                        case 121:
                            inventoryList.Items.Add("Obj Fill (121)");
                            break;
                        case 122:
                            inventoryList.Items.Add("Obj Fill (122)");
                            break;
                        case 123:
                            inventoryList.Items.Add("Elixir of Fire Resistance)");
                            break;
                        case 124:
                            inventoryList.Items.Add("Elixir of Heroic Rage");
                            break;
                        case 125:
                            inventoryList.Items.Add("Potion of Stamina");
                            break;
                        case 126:
                            inventoryList.Items.Add("Potion of Agility");
                            break;
                        case 127:
                            inventoryList.Items.Add("Potion of Healing");
                            break;
                        case 128:
                            inventoryList.Items.Add("Potion of Cure Poison");
                            break;
                        case 129:
                            inventoryList.Items.Add("Potion of Strength");
                            break;
                        case 130:
                            inventoryList.Items.Add("Elixir of Detect Monster");
                            break;
                        case 131:
                            inventoryList.Items.Add("Draught of Heightened Mortality");
                            break;
                        case 132:
                            inventoryList.Items.Add("Draught of Heightened Magic Power");
                            break;
                        case 133:
                            inventoryList.Items.Add("Obj Fill (133)");
                            break;
                        case 134:
                            inventoryList.Items.Add("Obj Fill (134)");
                            break;
                        case 135:
                            inventoryList.Items.Add("Wyvern's Blood");
                            break;
                        case 136:
                            inventoryList.Items.Add("Magia Plant");
                            break;
                        case 137:
                            inventoryList.Items.Add("Bottle of Wine");
                            break;
                        case 138:
                            inventoryList.Items.Add("Obj Fill (138)");
                            break;
                        case 139:
                            inventoryList.Items.Add("Block Figurine");
                            break;
                        case 140:
                            inventoryList.Items.Add("Iron Fist Figurine");
                            break;
                        case 141:
                            inventoryList.Items.Add("Laughting Skull Figurine");
                            break;
                        case 142:
                            inventoryList.Items.Add("Dark Cloak of Shadow Figurine");
                            break;
                        case 143:
                            inventoryList.Items.Add("Roaring Column Figurine");
                            break;
                        case 144:
                            inventoryList.Items.Add("Cube of Magic Immersion");
                            break;
                        case 145:
                            inventoryList.Items.Add("Soul Link Figurine");
                            break;
                        case 146:
                            inventoryList.Items.Add("Obj Fill (146)");
                            break;
                        case 147:
                            inventoryList.Items.Add("Obj Fill (147)");
                            break;
                        case 148:
                            inventoryList.Items.Add("Obj Fill (148)");
                            break;
                        case 149:
                            inventoryList.Items.Add("Obj Fill (149)");
                            break;
                        case 150:
                            inventoryList.Items.Add("Obj Fill (150)");
                            break;
                        case 151:
                            inventoryList.Items.Add("Amulet of Strength (+1)");
                            break;
                        case 152:
                            inventoryList.Items.Add("Amulet of Strength (+2)");
                            break;
                        case 153:
                            inventoryList.Items.Add("Amulet of Stamina (+1)");
                            break;
                        case 154:
                            inventoryList.Items.Add("Amulet of Stamina (+2)");
                            break;
                        case 155:
                            inventoryList.Items.Add("Amulet of Agility (+1)");
                            break;
                        case 156:
                            inventoryList.Items.Add("Amulet of Agility (+2)");
                            break;
                        case 157:
                            inventoryList.Items.Add("Amulet of Power (+1)");
                            break;
                        case 158:
                            inventoryList.Items.Add("Amulet of Power (+2)");
                            break;
                        case 159:
                            inventoryList.Items.Add("Amulet of Protection");
                            break;
                        case 160:
                            inventoryList.Items.Add("Amulet of Power Regeneration");
                            break;
                        case 161:
                            inventoryList.Items.Add("Amulet of Strength and Hale");
                            break;
                        case 162:
                            inventoryList.Items.Add("Obj Fill (162)");
                            break;
                        case 163:
                            inventoryList.Items.Add("Obj Fill (163)");
                            break;
                        case 164:
                            inventoryList.Items.Add("Obj Fill (164)");
                            break;
                        case 165:
                            inventoryList.Items.Add("Obj Fill (165)");
                            break;
                        case 166:
                            inventoryList.Items.Add("Obj Fill (166)");
                            break;
                        case 167:
                            inventoryList.Items.Add("Obj Fill (167)");
                            break;
                        case 168:
                            inventoryList.Items.Add("Obj Fill (168)");
                            break;
                        case 169:
                            inventoryList.Items.Add("Ivory Void Half");
                            break;
                        case 170:
                            inventoryList.Items.Add("Jet Void Half");
                            break;
                        case 171:
                            inventoryList.Items.Add("Symbol of Flesh - Red");
                            break;
                        case 172:
                            inventoryList.Items.Add("Symbol of Flesh - Gold");
                            break;
                        case 173:
                            inventoryList.Items.Add("Dragon Amber");
                            break;
                        case 174:
                            inventoryList.Items.Add("Stasis Jar");
                            break;
                        case 175:
                            inventoryList.Items.Add("Stasis Jar Containing Dragon Amber");
                            break;
                        case 176:
                            inventoryList.Items.Add("Bead of Immortal Clay");
                            break;
                        case 177:
                            inventoryList.Items.Add("Wood from the Wicked Tree");
                            break;
                        case 178:
                            inventoryList.Items.Add("Soul House");
                            break;
                        case 179:
                            inventoryList.Items.Add("Soul House Containing the Spirit of the Ivory Prince");
                            break;
                        case 180:
                            inventoryList.Items.Add("Sipher Containing Blessed Tears of the Weeping Moon");
                            break;
                        case 181:
                            inventoryList.Items.Add("Iron Shackles");
                            break;
                        case 182:
                            inventoryList.Items.Add("Heart Stone");
                            break;
                        case 183:
                            inventoryList.Items.Add("Coffer");
                            break;
                        case 184:
                            inventoryList.Items.Add("Sacred Sipher");
                            break;
                        case 185:
                            inventoryList.Items.Add("Dragon Amber");
                            break;
                        case 186:
                            inventoryList.Items.Add("Dragon Amber");
                            break;
                        case 187:
                            inventoryList.Items.Add("Coffer Containing the Dark Slag");
                            break;
                        case 188:
                            inventoryList.Items.Add("Eye of Insight");
                            break;
                        case 189:
                            inventoryList.Items.Add("Horn of Passage");
                            break;
                        case 190:
                            inventoryList.Items.Add("Hourglass of Temporal Freeze");
                            break;
                        case 191:
                            inventoryList.Items.Add("191 (teleport wand/rod)");
                            break;
                        case 192:
                            inventoryList.Items.Add("Staff of Unmaking");
                            break;
                        case 193:
                            inventoryList.Items.Add("Rod of Magic Immersion");
                            break;
                        case 194:
                            inventoryList.Items.Add("SteelRipper - Spear");
                            break;
                        case 195:
                            inventoryList.Items.Add("Obj Fill (195)");
                            break;
                        case 196:
                            inventoryList.Items.Add("Obj Fill (196)");
                            break;
                        case 197:
                            inventoryList.Items.Add("Obj Fill (197)");
                            break;
                        case 198:
                            inventoryList.Items.Add("Obj Fill (198)");
                            break;
                        case 199:
                            inventoryList.Items.Add("Obj Fill (199)");
                            break;
                        case 200:
                            inventoryList.Items.Add("Skarac");
                            break;
                        case 201:
                            inventoryList.Items.Add("Skarac");
                            break;
                        case 202:
                            inventoryList.Items.Add("Sigil of Fire");
                            break;
                        case 203:
                            inventoryList.Items.Add("Bloated Heart");
                            break;
                        case 204:
                            inventoryList.Items.Add("Large Crystal Orb");
                            break;
                        case 205:
                            inventoryList.Items.Add("Sol Disc");
                            break;
                        case 206:
                            inventoryList.Items.Add("Moon Disc");
                            break;
                        case 207:
                            inventoryList.Items.Add("Spring Equinox Disc");
                            break;
                        case 208:
                            inventoryList.Items.Add("Fall Equinox Disc");
                            break;
                        case 209:
                            inventoryList.Items.Add("Enchanted Sea Conch");
                            break;
                        case 210:
                            inventoryList.Items.Add("Glass Lamp of Life Essence");
                            break;
                        case 211:
                            inventoryList.Items.Add("Horn of Summons");
                            break;
                        case 212:
                            inventoryList.Items.Add("Lure of the Heart");
                            break;
                        case 213:
                            inventoryList.Items.Add("Withered Hand Key");
                            break;
                        case 214:
                            inventoryList.Items.Add("Whistle of Fiery Beckons");
                            break;
                        case 215:
                            inventoryList.Items.Add("Glowing Ember");
                            break;
                        case 216:
                            inventoryList.Items.Add("Hallowed Staff of Elder Wood");
                            break;
                        case 217:
                            inventoryList.Items.Add("Chimes of Comprehension");
                            break;
                        case 218:
                            inventoryList.Items.Add("Soul House Containing the Evil Spirit");
                            break;
                        case 219:
                            inventoryList.Items.Add("Hallowed Wreath of Elder Leaves");
                            break;
                        case 220:
                            inventoryList.Items.Add("Crest of Tempest");
                            break;
                        case 221:
                            inventoryList.Items.Add("Key Stone of Safe Crossing");
                            break;
                        case 222:
                            inventoryList.Items.Add("Soul House Containing Spirit of Female Ghost");
                            break;
                        case 223:
                            inventoryList.Items.Add("Wooden Crank Shaft");
                            break;
                        case 224:
                            inventoryList.Items.Add("Dragon Sigil of the Void");
                            break;
                        case 225:
                            inventoryList.Items.Add("Dragon Sigil of Fire");
                            break;
                        case 226:
                            inventoryList.Items.Add("Dragon Sigil of Lightning");
                            break;

                        //Second group starts here

                        case 227:
                            inventoryList.Items.Add("Dragon Sigil of the Wind");
                            break;
                        case 228:
                            inventoryList.Items.Add("Dragon Sigil of Water");
                            break;
                        case 229:
                            inventoryList.Items.Add("Dragon Sigil of the Earth");
                            break;
                        case 230:
                            inventoryList.Items.Add("Trumpet of Earthen Quake");
                            break;
                        case 231:
                            inventoryList.Items.Add("Lure of the Heart");
                            break;
                        case 232:
                            inventoryList.Items.Add("Dark Slag");
                            break;
                        case 233:
                            inventoryList.Items.Add("Obj Fill (233)");
                            break;
                        case 234:
                            inventoryList.Items.Add("Obj Fill (234)");
                            break;
                        case 235:
                            inventoryList.Items.Add("Obj Fill (235)");
                            break;
                        case 236:
                            inventoryList.Items.Add("Obj Fill (236)");
                            break;
                        case 237:
                            inventoryList.Items.Add("Obj Fill (237)");
                            break;
                        case 238:
                            inventoryList.Items.Add("Obj Fill (238)");
                            break;
                        case 239:
                            inventoryList.Items.Add("Obj Fill (239)");
                            break;
                        case 240:
                            inventoryList.Items.Add("Obj Fill (240)");
                            break;
                        case 241:
                            inventoryList.Items.Add("Obj Fill (241)");
                            break;
                        case 242:
                            inventoryList.Items.Add("Obj Fill (242)");
                            break;
                        case 243:
                            inventoryList.Items.Add("Obj Fill (243)");
                            break;
                        case 244:
                            inventoryList.Items.Add("Obj Fill (244)");
                            break;
                        case 245:
                            inventoryList.Items.Add("Obj Fill (245)");
                            break;
                        case 246:
                            inventoryList.Items.Add("Obj Fill (246)");
                            break;
                        case 247:
                            inventoryList.Items.Add("Obj Fill (247)");
                            break;
                        case 248:
                            inventoryList.Items.Add("Obj Fill (248)");
                            break;
                        case 249:
                            inventoryList.Items.Add("Obj Fill (249)");
                            break;
                        case 250:
                            inventoryList.Items.Add("Obj Fill (250)");
                            break;




                        default:
                            inventoryList.Items.Add("Unreadable Item - 99.9% chance glitch with this program");
                            break;
                    }

                    fs.Position += 0x17;
                    itemID = fs.ReadByte();
                    fs.Position -= 0x01;

                
                }
                */

                //Close the file stream
                fs.Close();

            }
        }


        //***********************************************************************************************************************
        ////////////////////////////////////////////  WRITING VALUES ///////////////////////////////////////////////////////////*
        //***********************************************************************************************************************
        private void saveChangesButton_Click(object sender, EventArgs e) {

            bool validValues = true;

            if (fileSelected == true) {

                //All stats besides mana and health must be 255 or lower (excluding X and Y coords)
                if (int.Parse(currentStrengthBox.Text) > 255 || int.Parse(maxStrengthBox.Text) > 255
                     || int.Parse(currentStaminaBox.Text) > 255 || int.Parse(maxStaminaBox.Text) > 255 || int.Parse(currentAgilityBox.Text) > 255 || int.Parse(maxAgilityBox.Text) > 255
                     || int.Parse(currentPowerBox.Text) > 255 || int.Parse(maxPowerBox.Text) > 255) {
                    MessageBox.Show("All stats besides current and max for mana & health must be 255 or lower");
                    validValues = false;
                }

                else {
                    validValues = true;
                }

                if (validValues == true) {
                    fs = new FileStream(dialog.FileName, FileMode.Open);

                    //Both used for dealing with little endian
                    //Reminder that FIRST and SECOND are in the order they appear in the file, not how the data is read!
                    string firstByte;
                    string secondByte;
                    string currentByte;

                    //Grab all newly entered stat values
                    int currentHealth = Int32.Parse(currentHealthBox.Text);
                    int maxHealth = Int32.Parse(maxHealthBox.Text);
                    int currentMana = Int32.Parse(currentManaBox.Text);
                    int maxMana = Int32.Parse(maxManaBox.Text);
                    int currentStrength = Int32.Parse(currentStrengthBox.Text);
                    int maxStrength = Int32.Parse(maxStrengthBox.Text);
                    int currentStamina = Int32.Parse(currentStaminaBox.Text);
                    int maxStamina = Int32.Parse(maxStaminaBox.Text);
                    int currentAgility = Int32.Parse(currentAgilityBox.Text);
                    int maxAgility = Int32.Parse(maxAgilityBox.Text);
                    int currentPower = Int32.Parse(currentPowerBox.Text);
                    int maxPower = Int32.Parse(maxPowerBox.Text);
                    int manaRegenTick = Int32.Parse(manaRegenBox.Text);

                    //Grab all newly entered position values
                    int xPosition = Int32.Parse(xPosBox.Text);
                    int yPosition = Int32.Parse(yPosBox.Text);

                    ////Grab all newly entered skill values
                    magicSkills[0] = earthCombo.SelectedItem.ToString();
                    magicSkills[1] = windCombo.SelectedItem.ToString();
                    magicSkills[2] = fireCombo.SelectedItem.ToString();
                    magicSkills[3] = waterCombo.SelectedItem.ToString();
                    magicSkills[4] = lightningCombo.SelectedItem.ToString();
                    magicSkills[5] = fleshCombo.SelectedItem.ToString();
                    magicSkills[6] = voidCombo.SelectedItem.ToString();
                    combatSkills[0] = hackingCombo.SelectedItem.ToString();
                    combatSkills[1] = thrustingCombo.SelectedItem.ToString();
                    combatSkills[2] = slashingCombo.SelectedItem.ToString();
                    combatSkills[3] = rangedCombo.SelectedItem.ToString();

                    //Go to offset for current Health and write (2 bytes - Don't forget to write bytes in reverse order because little endian)
                    //the "x4" makes sure a 0 is on the start of the hex string 
                    currentByte = currentHealth.ToString("x4");
                    secondByte = currentByte.Substring(0, 2);
                    firstByte = currentByte.Substring(2, 2);
                    //Write the second byte in the first position of the 2byte little endian data
                    fs.Position = 0x4C4C;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(secondByte, 16).ToString()));
                    //Write the first byte in the second position of the 2byte little endian data
                    fs.Position = 0x4C4B;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(firstByte, 16).ToString()));


                    //Go to offset for max Health and write (2 bytes - Don't forget to write bytes in reverse order because little endian)
                    //the "x4" makes sure a 0 is on the start of the hex string 
                    currentByte = maxHealth.ToString("x4");
                    secondByte = currentByte.Substring(0, 2);
                    firstByte = currentByte.Substring(2, 2);
                    //Write the second byte in the first position of the 2byte little endian data
                    fs.Position = 0x4C4E;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(secondByte, 16).ToString()));
                    //Write the first byte in the second position of the 2byte little endian data
                    fs.Position = 0x4C4D;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(firstByte, 16).ToString()));


                    //Go to offset for current Mana and write (2 bytes - Don't forget to write bytes in reverse order because little endian)
                    //the "x4" makes sure a 0 is on the start of the hex string 
                    currentByte = currentMana.ToString("x4");
                    secondByte = currentByte.Substring(0, 2);
                    firstByte = currentByte.Substring(2, 2);
                    //Write the second byte in the first position of the 2byte little endian datata
                    fs.Position = 0x4C50;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(secondByte, 16).ToString()));
                    //Write the first byte in the second position of the 2byte little endian data
                    fs.Position = 0x4C4F;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(firstByte, 16).ToString()));


                    //Go to offset for max Mana and write (2 bytes - Don't forget to write bytes in reverse order because little endian)
                    //the "x4" makes sure a 0 is on the start of the hex string 
                    currentByte = maxMana.ToString("x4");
                    secondByte = currentByte.Substring(0, 2);
                    firstByte = currentByte.Substring(2, 2);
                    //Write the second byte in the first position of the 2byte little endian data
                    fs.Position = 0x4C52;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(secondByte, 16).ToString()));
                    //Write the first byte in the second position of the 2byte little endian data
                    fs.Position = 0x4C51;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(firstByte, 16).ToString()));

                    //Go to offset for MANA REGEN TICK and write (2 bytes - Don't forget to write bytes in reverse order because little endian)
                    //the "x4" makes sure a 0 is on the start of the hex string 
                    currentByte = manaRegenTick.ToString("x4");
                    MessageBox.Show(currentByte);
                    secondByte = currentByte.Substring(0, 2);
                    MessageBox.Show("Second byte: " + secondByte);
                    firstByte = currentByte.Substring(2, 2);
                    MessageBox.Show("First byte: " + firstByte);
                    //Write the second byte in the first position of the 2byte little endian data
                    fs.Position = 0x4C76;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(secondByte, 16).ToString()));
                    //Write the first byte in the second position of the 2byte little endian data
                    fs.Position = 0x4C75;
                    fs.WriteByte(byte.Parse(Convert.ToInt32(firstByte, 16).ToString()));



                    //Go to offset for current Strength and write
                    fs.Position = 0x4C57;
                    fs.WriteByte(byte.Parse(currentStrength.ToString()));

                    //Go to offset for max Strength and write
                    fs.Position = 0x4C59;
                    fs.WriteByte(byte.Parse(maxStrength.ToString()));

                    //Go to offset for current Stamina and write
                    fs.Position = 0x4C61;
                    fs.WriteByte(byte.Parse(currentStamina.ToString()));

                    //Go to offset for max Stamina and write
                    fs.Position = 0x4C63;
                    fs.WriteByte(byte.Parse(maxStamina.ToString()));

                    //Go to offset for current Agility and write
                    fs.Position = 0x4C6B;
                    fs.WriteByte(byte.Parse(currentAgility.ToString()));

                    //Go to offset for max Agility and write
                    fs.Position = 0x4C6D;
                    fs.WriteByte(byte.Parse(maxAgility.ToString()));

                    //Go to offset for current Power and write
                    fs.Position = 0x4C75;
                    fs.WriteByte(byte.Parse(currentPower.ToString()));

                    //Go to offset for max Power and write
                    fs.Position = 0x4C77;
                    fs.WriteByte(byte.Parse(maxPower.ToString()));

                    //Go to offset for X position and write
                    fs.Position = 0x4C17;
                    fs.WriteByte(byte.Parse(xPosition.ToString()));

                    //Go to offset for Y position and write
                    fs.Position = 0x4C1B;
                    fs.WriteByte(byte.Parse(yPosition.ToString()));




                    //Write to skills
                    //Go to earth Skill offset and start from there
                    fs.Position = 0x4D29;


                    //On each skill in the array, check the level and write the proper byte accordingly
                    ///////////////////////////WRITE FOR MAGIC ONLY////////////////////////////////////////////////////////////////////////
                    for (int i = 0; i < magicSkills.Length; i++) {
                        if (magicSkills[i] == "Initiate") {
                            fs.WriteByte(0x01);
                        }

                        if (magicSkills[i] == "Neophyte") {
                            fs.WriteByte(0x02);
                        }

                        if (magicSkills[i] == "Apprentice") {
                            fs.WriteByte(0x03);
                        }

                        if (magicSkills[i] == "Talent") {
                            fs.WriteByte(0x04);
                        }

                        if (magicSkills[i] == "Transcendent") {
                            fs.WriteByte(0x05);
                        }

                        if (magicSkills[i] == "Accomplished") {
                            fs.WriteByte(0x06);
                        }

                        if (magicSkills[i] == "Maren") {
                            fs.WriteByte(0x07);
                        }

                        if (magicSkills[i] == "Adept") {
                            fs.WriteByte(0x08);
                        }

                        if (magicSkills[i] == "Maestro") {
                            fs.WriteByte(0x09);
                        }

                        if (magicSkills[i] == "Archmage") {
                            fs.WriteByte(0x0A);
                        }
                    }


                    //Write to skills
                    //Go to earth Skill offset and start from there
                    fs.Position = 0x4D30;

                    //On each skill in the array, check the level and write the proper byte accordingly
                    ///////////////////////////WRITE FOR COMBAT ONLY////////////////////////////////////////////////////////////////////////
                    for (int i = 0; i < combatSkills.Length; i++) {
                        if (combatSkills[i] == "Novice") {
                            fs.WriteByte(0x01);
                        }

                        if (combatSkills[i] == "Competent") {
                            fs.WriteByte(0x02);
                        }

                        if (combatSkills[i] == "Trained") {
                            fs.WriteByte(0x03);
                        }

                        if (combatSkills[i] == "Proficient") {
                            fs.WriteByte(0x04);
                        }

                        if (combatSkills[i] == "Experienced") {
                            fs.WriteByte(0x05);
                        }

                        if (combatSkills[i] == "Accomplished") {
                            fs.WriteByte(0x06);
                        }

                        if (combatSkills[i] == "Skilled") {
                            fs.WriteByte(0x07);
                        }

                        if (combatSkills[i] == "Seasoned") {
                            fs.WriteByte(0x08);
                        }

                        if (combatSkills[i] == "Expert") {
                            fs.WriteByte(0x09);
                        }

                        if (combatSkills[i] == "Master") {
                            fs.WriteByte(0x0A);
                        }
                    }

                    //WRITE CURRENT FLOOR LEVEL
                    fs.Position = 0x4C54;
                    int levelNumber = levelCollection.SelectedIndex;
                    fs.WriteByte((byte)levelNumber);

                    //Close the file
                    fs.Close();


                    MessageBox.Show("Stats updated! Enjoy!");
                }
            }


            else {
                MessageBox.Show("No save file selected.");
            }


        }

        //Function converts hex byte to ASCII
        public static string ConvertHex(String hexString) {
            try {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2) {
                    String hs = string.Empty;
                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;
                }

                return ascii;
            }

            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return string.Empty;
        }





        //Read LITTLE ENDIAN bytes, can also be used to read in a single byte by setting length to 0x01
        //Pass in starting byte, how many bytes
        //## Usage
        //## byteOffset in the format of "0xFFFF" or any kind of hex offset
        //## length is how many bytes to read (including the starting byte!)
        int readLittleEndian(long byteOffset, byte length) {
            //Both used for appending the hex data together and conversion for return
            string appendBytesString = "";
            int appendBytesInt = 0;

            //Make byte array for length of data to store all bytes as we read "backwards"
            int[] readBytes = new int[length];

            //Read bytes backwards but store them in "big endian" order in our array
            for (int i = length; i > 0; i--) {
                //(We subtract 1 from the total offset since we don't count the byte we start on)
                fs.Position = byteOffset + i - 1;
                readBytes[length - i] = fs.ReadByte();
            }

            //Append all bytes together for conversion
            for (int i = 0; i < length; i++) {
                appendBytesString += readBytes[i].ToString("x2");
            }

            //Convert back to int
            appendBytesInt = Convert.ToInt32(appendBytesString, 16);

            return appendBytesInt;
        }


        private void toolTipButton_Click(object sender, EventArgs e) {
            MessageBox.Show("Item REPLACEMENT is only possible in this version due to my limited knowledge of the file structure. If I ever figure out how to add/remove"
               + " items then I will add the fuctionality to do so. So for now, you will have to sacrifice an item you don't want.");
        }


        private void documentationButton_Click_1(object sender, EventArgs e) {

            var txt = Properties.Resources.documentation;
            var fileName = Path.ChangeExtension(Path.GetTempFileName(), ".html");

            var fs = File.CreateText(fileName);
            fs.Write(txt);
            fs.Flush();
            fs.Close();

            Process.Start(fileName);

        }

        private void manaTrackBar_ValueChanged(object sender, EventArgs e) {
            manaRegenBox.Text = manaTrackBar.Value.ToString();
        }

        private void manaRegenBox_TextChanged(object sender, EventArgs e) {
            //If the box is empty, default mana regen rate to 0
            if (string.IsNullOrEmpty(manaRegenBox.Text)) {
                manaTrackBar.Value = 0;
                manaRegenBox.Text = "0";
            }
            //If it goes over the maximum, set it to maximum
            else if (int.Parse(manaRegenBox.Text) > 65535) {
                manaRegenBox.Text = "65535";
            }
            else {
                manaTrackBar.Value = int.Parse(manaRegenBox.Text);
            }
             

        }
    }
}

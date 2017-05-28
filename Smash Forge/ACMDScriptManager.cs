using SALT.Scripting;
using SALT.Scripting.AnimCMD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Smash_Forge
{
    // For processing ACMD files and relaying the state to the GUI
    public class ACMDScriptManager
    {
        public ACMDScript script { get; set; }
        public SortedList<int, Hitbox> Hitboxes { get; set; }
        public int currentFrame;

        // TODO: these may need to be passed down for proper subscript parsing
        // if any subscripts are used in loops anywhere.
        private int setLoop;
        private int iterations;

        public ACMDScriptManager()
        {
            Reset();
        }

        public ACMDScriptManager(ACMDScript script)
        {
            Reset();
            this.script = script;
        }

        public void Reset()
        {
            Hitboxes = new SortedList<int, Hitbox>();
            currentFrame = 0;
            script = null;

            setLoop = 0;
            iterations = 0;
        }

        public void addOrOverwriteHitbox(int id, Hitbox newHitbox)
        {
            if (Hitboxes.ContainsKey(id))
            {
                Hitboxes[id] = newHitbox;
            }
            else
            {
                Hitboxes.Add(id, newHitbox);
            }
        }

        public int processScriptCommandsAtCurrentFrame(ICommand cmd, int halt, ref int scriptCommandIndex)
        {
            Hitbox newHitbox = null;
            switch (cmd.Ident)
            {
                case 0x42ACFE7D: // Asynchronous Timer (specific frame start for next commands)
                    {
                        halt = (int)(float)cmd.Parameters[0] - 2;
                        break;
                    }
                case 0x4B7B6E51: // Synchronous Timer (relative frame start for next commands)
                    {
                        halt += (int)(float)cmd.Parameters[0];
                        break;
                    }
                case 0xB738EABD: // hitbox 
                    {
                        newHitbox = new Hitbox();
                        int id = (int)cmd.Parameters[0];
                        if (Hitboxes.ContainsKey(id))
                            Hitboxes.Remove(id);
                        newHitbox.Type = Hitbox.HITBOX;
                        newHitbox.Bone = (int)cmd.Parameters[2];
                        newHitbox.Damage = (float)cmd.Parameters[3];
                        newHitbox.Angle = (int)cmd.Parameters[4];
                        newHitbox.KnockbackGrowth = (int)cmd.Parameters[5];
                        //FKB = (float)cmd.Parameters[6]
                        newHitbox.KnockbackBase = (int)cmd.Parameters[7];
                        newHitbox.Size = (float)cmd.Parameters[8];
                        newHitbox.X = (float)cmd.Parameters[9];
                        newHitbox.Y = (float)cmd.Parameters[10];
                        newHitbox.Z = (float)cmd.Parameters[11];
                        addOrOverwriteHitbox(id, newHitbox);
                        break;
                    }
                case 0x2988D50F: // Extended hitbox
                    {
                        newHitbox = new Hitbox();
                        int id = (int)cmd.Parameters[0];
                        if (Hitboxes.ContainsKey(id))
                            Hitboxes.Remove(id);
                        newHitbox.Type = Hitbox.HITBOX;
                        newHitbox.Extended = true;
                        newHitbox.Bone = (int)cmd.Parameters[2];
                        newHitbox.Damage = (float)cmd.Parameters[3];
                        newHitbox.Angle = (int)cmd.Parameters[4];
                        newHitbox.KnockbackGrowth = (int)cmd.Parameters[5];
                        //FKB = (float)cmd.Parameters[6]
                        newHitbox.KnockbackBase = (int)cmd.Parameters[7];
                        newHitbox.Size = (float)cmd.Parameters[8];
                        newHitbox.X = (float)cmd.Parameters[9];
                        newHitbox.Y = (float)cmd.Parameters[10];
                        newHitbox.Z = (float)cmd.Parameters[11];
                        newHitbox.X2 = (float)cmd.Parameters[24];
                        newHitbox.Y2 = (float)cmd.Parameters[25];
                        newHitbox.Z2 = (float)cmd.Parameters[26];
                        addOrOverwriteHitbox(id, newHitbox);
                        break;
                    }
                case 0x14FCC7E4: // special hitbox
                    {
                        newHitbox = new Hitbox();
                        int id = (int)cmd.Parameters[0];
                        if (Hitboxes.ContainsKey(id))
                            Hitboxes.Remove(id);
                        newHitbox.Type = Hitbox.HITBOX;
                        newHitbox.Bone = (int)cmd.Parameters[2];
                        newHitbox.Damage = (float)cmd.Parameters[3];
                        newHitbox.Angle = (int)cmd.Parameters[4];
                        newHitbox.KnockbackGrowth = (int)cmd.Parameters[5];
                        //FKB = (float)cmd.Parameters[6]
                        newHitbox.KnockbackBase = (int)cmd.Parameters[7];
                        newHitbox.Size = (float)cmd.Parameters[8];
                        newHitbox.X = (float)cmd.Parameters[9];
                        newHitbox.Y = (float)cmd.Parameters[10];
                        newHitbox.Z = (float)cmd.Parameters[11];
                        if (cmd.Parameters.Count > 39)
                        {
                            if ((int)cmd.Parameters[39] == 1)
                            {
                                newHitbox.Type = Hitbox.WINDBOX;
                            }
                        }
                        addOrOverwriteHitbox(id, newHitbox);
                        break;
                    }
                case 0x7075DC5A: // Extended special hitbox
                    {
                        newHitbox = new Hitbox();
                        int id = (int)cmd.Parameters[0];
                        if (Hitboxes.ContainsKey(id))
                            Hitboxes.Remove(id);
                        newHitbox.Type = Hitbox.HITBOX;
                        newHitbox.Extended = true;
                        newHitbox.Bone = (int)cmd.Parameters[2];
                        newHitbox.Damage = (float)cmd.Parameters[3];
                        newHitbox.Angle = (int)cmd.Parameters[4];
                        newHitbox.KnockbackGrowth = (int)cmd.Parameters[5];
                        //FKB = (float)cmd.Parameters[6]
                        newHitbox.KnockbackBase = (int)cmd.Parameters[7];
                        newHitbox.Size = (float)cmd.Parameters[8];
                        newHitbox.X = (float)cmd.Parameters[9];
                        newHitbox.Y = (float)cmd.Parameters[10];
                        newHitbox.Z = (float)cmd.Parameters[11];
                        if ((int)cmd.Parameters[39] == 1)
                        {
                            newHitbox.Type = Hitbox.WINDBOX;
                        }
                        newHitbox.X2 = (float)cmd.Parameters[40];
                        newHitbox.Y2 = (float)cmd.Parameters[41];
                        newHitbox.Z2 = (float)cmd.Parameters[42];
                        addOrOverwriteHitbox(id, newHitbox);
                        break;
                    }
                case 0xCC7CC705: // collateral hitbox (ignored by character being thrown)
                    {
                        newHitbox = new Hitbox();
                        int id = (int)cmd.Parameters[0];
                        if (Hitboxes.ContainsKey(id))
                            Hitboxes.Remove(id);
                        newHitbox.Type = Hitbox.HITBOX;
                        newHitbox.Bone = (int)cmd.Parameters[2];
                        newHitbox.Damage = (float)cmd.Parameters[3];
                        newHitbox.Angle = (int)cmd.Parameters[4];
                        newHitbox.KnockbackGrowth = (int)cmd.Parameters[5];
                        //FKB = (float)cmd.Parameters[6]
                        newHitbox.KnockbackBase = (int)cmd.Parameters[7];
                        newHitbox.Size = (float)cmd.Parameters[8];
                        newHitbox.X = (float)cmd.Parameters[9];
                        newHitbox.Y = (float)cmd.Parameters[10];
                        newHitbox.Z = (float)cmd.Parameters[11];

                        newHitbox.Ignore_Throw = true;

                        addOrOverwriteHitbox(id, newHitbox);
                        break;
                    }
                case 0xED67D5DA: // Extended collateral hitbox (ignored by character being thrown)
                    {
                        newHitbox = new Hitbox();
                        int id = (int)cmd.Parameters[0];
                        if (Hitboxes.ContainsKey(id))
                            Hitboxes.Remove(id);
                        newHitbox.Type = Hitbox.HITBOX;
                        newHitbox.Extended = true;
                        newHitbox.Bone = (int)cmd.Parameters[2];
                        newHitbox.Damage = (float)cmd.Parameters[3];
                        newHitbox.Angle = (int)cmd.Parameters[4];
                        newHitbox.KnockbackGrowth = (int)cmd.Parameters[5];
                        //FKB = (float)cmd.Parameters[6]
                        newHitbox.KnockbackBase = (int)cmd.Parameters[7];
                        newHitbox.Size = (float)cmd.Parameters[8];
                        newHitbox.X = (float)cmd.Parameters[9];
                        newHitbox.Y = (float)cmd.Parameters[10];
                        newHitbox.Z = (float)cmd.Parameters[11];
                        newHitbox.X2 = (float)cmd.Parameters[24];
                        newHitbox.Y2 = (float)cmd.Parameters[25];
                        newHitbox.Z2 = (float)cmd.Parameters[26];

                        newHitbox.Ignore_Throw = true;

                        addOrOverwriteHitbox(id, newHitbox);
                        break;
                    }
                case 0x9245E1A8: // clear all hitboxes
                    Hitboxes.Clear();
                    break;
                case 0xFF379EB6: // delete hitbox
                    if (Hitboxes.ContainsKey((int)cmd.Parameters[0]))
                    {
                        Hitboxes.Remove((int)cmd.Parameters[0]);
                    }
                    break;
                case 0x7698BB42: // deactivate previous hitbox
                    Hitboxes.Remove(Hitboxes.Keys.Max());
                    break;
                case 0xEB375E3: // Set Loop
                    iterations = int.Parse(cmd.Parameters[0] + "") - 1;
                    setLoop = scriptCommandIndex;
                    break;
                case 0x38A3EC78: // goto
                    if (iterations > 0)
                    {
                        // Can fail if a subscript has a goto with no loop starter
                        scriptCommandIndex = setLoop;
                        iterations -= 1;
                    }
                    break;

                case 0x7B48FE1C: //Extended Grabbox
                case 0x1EAF840C: //Grabbox 2 (most command grabs)
                case 0x548F2D4C: //Grabbox (used in tether grabs)
                case 0xEF787D43: //Extended Grabbox 2 (Mega Man's grab)
                    {
                        newHitbox = new Hitbox();
                        int id = (int)cmd.Parameters[0];
                        newHitbox.Type = Hitbox.GRABBOX;
                        newHitbox.Bone = int.Parse(cmd.Parameters[1] + "");
                        newHitbox.Size = (float)cmd.Parameters[2];
                        newHitbox.X = (float)cmd.Parameters[3];
                        newHitbox.Y = (float)cmd.Parameters[4];
                        newHitbox.Z = (float)cmd.Parameters[5];

                        if (cmd.Parameters.Count > 10)
                        {
                            newHitbox.X2 = float.Parse(cmd.Parameters[8] + "");
                            newHitbox.Y2 = float.Parse(cmd.Parameters[9] + "");
                            newHitbox.Z2 = float.Parse(cmd.Parameters[10] + "");
                            newHitbox.Extended = true;
                        }

                        addOrOverwriteHitbox(id, newHitbox);
                        break;
                    }
                case 0xF3A464AC: // Terminate_Grab_Collisions
                    {
                        List<int> toDelete = new List<int>();
                        foreach (KeyValuePair<int, Hitbox> kvp in Hitboxes)
                        {
                            if (kvp.Value.Type == Hitbox.GRABBOX)
                                toDelete.Add(kvp.Key);
                        }
                        foreach (int index in toDelete)
                            Hitboxes.Remove(index);
                        break;
                    }
                case 0x2F08F54F: // Delete_Catch_Collision by ID
                    int idToDelete = (int)cmd.Parameters[0];
                    if (Hitboxes[idToDelete].Type == Hitbox.GRABBOX)
                        Hitboxes.Remove(idToDelete);
                    break;
                case 0x44081C21: //SEARCH
                    {
                        newHitbox = new Hitbox();
                        int id = (int)cmd.Parameters[0];
                        if (Hitboxes.ContainsKey(id))
                            Hitboxes.Remove(id);
                        newHitbox.Type = Hitbox.SEARCHBOX;
                        newHitbox.Bone = (int)cmd.Parameters[2];

                        newHitbox.Size = (float)cmd.Parameters[3];
                        newHitbox.X = (float)cmd.Parameters[4];
                        newHitbox.Y = (float)cmd.Parameters[5];
                        newHitbox.Z = (float)cmd.Parameters[6];
                        addOrOverwriteHitbox(id, newHitbox);
                        break;
                    }
                case 0xCD0C1CC9: //Bat Within (it clears WT SEARCH event)
                case 0x98203AF6: //SRH_CLEAR_ALL
                    {
                        List<int> toDelete = new List<int>();
                        foreach (KeyValuePair<int, Hitbox> kvp in Hitboxes)
                        {
                            if (kvp.Value.Type == Hitbox.SEARCHBOX)
                                toDelete.Add(kvp.Key);
                        }
                        foreach (int index in toDelete)
                            Hitboxes.Remove(index);
                        break;
                    }
                case 0xFA1BC28A: //Subroutine1: call another script
                    halt = processSubscriptCommandsAtCurrentFrame((uint)int.Parse(cmd.Parameters[0] + ""), halt, scriptCommandIndex);
                    break;
                case 0xFAA85333:
                    break;
                case 0x321297B0:
                    break;
                case 0x7640AEEB:
                    break;
                case 0xA5BD4F32: // TRUE
                    break;
                case 0x895B9275: // FALSE
                    break;
            }

            if (newHitbox != null)
            {
                newHitbox.Bone = VBN.applyBoneThunk(newHitbox.Bone);
            }
            return halt;
        }

        public SortedList<int, Hitbox> processScript()
        {
            Hitboxes.Clear();
            // The next frame the script halts at for execution. Only modified
            // by timer commands.
            int halt = 0;
            int scriptCommandIndex = 0;
            ICommand cmd = script[scriptCommandIndex];
            //ProcessANMCMD_SOUND();

            while (halt < currentFrame)
            {
                halt = processScriptCommandsAtCurrentFrame(cmd, halt, ref scriptCommandIndex);

                scriptCommandIndex++;
                if (scriptCommandIndex >= script.Count)
                    break;
                else
                    cmd = script[scriptCommandIndex];

                // If the next command is beyond our current anim frame
                if (halt > currentFrame)
                    break;
            }
            return Hitboxes;
        }

        public int processSubscriptCommandsAtCurrentFrame(uint crc, int halt, int scriptCommandIndex)
        {
            // Try and load the other script, if we can't just keep going
            ACMDScript subscript;
            try
            {
                subscript = (ACMDScript)Runtime.Moveset.Game.Scripts[crc];
            }
            catch (KeyNotFoundException)
            {
                return halt;
            }

            int subscriptCommandIndex = 0;
            ICommand cmd = subscript[subscriptCommandIndex];
            while (halt < currentFrame)
            {
                halt = processScriptCommandsAtCurrentFrame(cmd, halt, ref subscriptCommandIndex);

                subscriptCommandIndex++;
                if (subscriptCommandIndex >= subscript.Count)
                    break;
                else
                    cmd = subscript[subscriptCommandIndex];

                // If the next command is beyond our current anim frame
                if (halt > currentFrame)
                    break;
            }

            return halt;
        }
    }

    public class Weapon
    {
        // TODO: parsing weapon scripts in tandem with regular scripts
        // TODO: visualizing / GUI for weapon data
        // TODO: actually drawing weapons
        public int weaponId { get; set; }
        public string weaponName { get; set; }

        public ModelContainer model { get; set; }
        public MovesetManager moveset { get; set; }
        public Dictionary<string, MTA> materialAnimations { get; set; }
        public Dictionary<string, SkelAnimation> animations { get; set; }

        // Stuff for rendering
        public SkelAnimation targetAnim;

        public Weapon(int weaponId, string weaponName)
        {
            this.weaponId = weaponId;
            this.weaponName = weaponName;

            model = null;
            moveset = null;
            materialAnimations = new Dictionary<string, MTA>();
            animations = new Dictionary<string, SkelAnimation>();
        }

        //Runtime.Moveset.Game.Scripts[crc]

        // Returns a list mapping weapon_id for a character to the weapon name
        public static SortedList<int, string> getCharacterWeapons(string characterName)
        {
            string characterNameLower = characterName.ToLower();
            List<int> characterWeaponIndexes = new List<int>();
            for (int i = 0; i < WEAPON_CHARACTER.Length; i++)
            {
                if (WEAPON_CHARACTER[i] == characterNameLower)
                    characterWeaponIndexes.Add(i);
            }

            SortedList<int, string> characterWeapons = new SortedList<int, string>();
            // j starts at 1 because ACMD is 1 indexed on weapons
            for (int i = 0, j = 1; i < characterWeaponIndexes.Count; i++, j++)
                characterWeapons.Add(j, WEAPON[characterWeaponIndexes[i]]);
            return characterWeapons;
        }

        /// <summary>
        /// Lite version of "openAnimation" for weapons, which does not update
        /// GUI elements.
        /// </summary>
        public void openWeaponAnimation(string filename)
        {
            if (filename.EndsWith(".mta"))
            {
                MTA mta = new MTA();
                try
                {
                    mta.Read(filename);
                    this.materialAnimations.Add(filename, mta);
                    //mtaNode.Nodes.Add(filename);
                    //MainForm.Instance.viewports[0].loadMTA(mta);
                    //Runtime.TargetMTAString = filename;
                }
                catch (EndOfStreamException)
                { }
            }
            else if (filename.EndsWith(".smd"))
            {
                var anim = new SkelAnimation();
                if (Runtime.TargetVBN == null)
                    Runtime.TargetVBN = new VBN();
                SMD.read(filename, anim, Runtime.TargetVBN);
                this.animations.Add(filename, anim);
                //animNode.Nodes.Add(filename);
            }
            else if (filename.EndsWith(".pac"))
            {
                PAC p = new PAC();
                p.Read(filename);

                foreach (var pair in p.Files)
                {
                    if (pair.Key.EndsWith(".omo"))
                    {
                        var anim = OMO.read(new FileData(pair.Value));
                        string AnimName = Regex.Match(pair.Key, @"([A-Z][0-9][0-9])(.*)").Groups[0].ToString();
                        //AnimName = pair.Key;
                        //AnimName = AnimName.Remove(AnimName.Length - 4);
                        //AnimName = AnimName.Insert(3, "_");
                        if (!string.IsNullOrEmpty(AnimName))
                        {
                            if (this.animations.ContainsKey(AnimName))
                                this.animations[AnimName].children.Add(anim);
                            else
                            {
                                //animNode.Nodes.Add(AnimName);
                                this.animations.Add(AnimName, anim);
                            }
                        }
                        else
                        {
                            if (this.animations.ContainsKey(pair.Key))
                                this.animations[pair.Key].children.Add(anim);
                            else
                            {
                                //animNode.Nodes.Add(pair.Key);
                                this.animations.Add(pair.Key, anim);
                            }
                        }
                    }
                    else if (pair.Key.EndsWith(".mta"))
                    {
                        MTA mta = new MTA();
                        try
                        {
                            if (!this.materialAnimations.ContainsKey(pair.Key))
                            {
                                mta.read(new FileData(pair.Value));
                                this.materialAnimations.Add(pair.Key, mta);
                                //mtaNode.Nodes.Add(pair.Key);
                            }

                            // matching
                            string AnimName =
                                Regex.Match(pair.Key, @"([A-Z][0-9][0-9])(.*)").Groups[0].ToString()
                                    .Replace(".mta", ".omo");
                            if (this.animations.ContainsKey(AnimName))
                            {
                                this.animations[AnimName].children.Add(mta);
                            }

                        }
                        catch (EndOfStreamException)
                        { }
                    }
                }
            }

            // Not sure if melee models use weapons
            //if (filename.EndsWith(".dat"))
            //{
            //    if (!filename.EndsWith("AJ.dat"))
            //        MessageBox.Show("Not a DAT animation");
            //    else
            //    {
            //        if (Runtime.ModelContainers[0].dat_melee == null)
            //            MessageBox.Show("Load a DAT model first");
            //        else
            //            DAT_Animation.LoadAJ(filename, Runtime.ModelContainers[0].dat_melee.bones);
            //    }
            //}

            if (filename.EndsWith(".omo"))
            {
                this.animations.Add(filename, OMO.read(new FileData(filename)));
                //animNode.Nodes.Add(filename);
            }
            if (filename.EndsWith(".chr0"))
            {
                this.animations.Add(filename, CHR0.read(new FileData(filename), Runtime.TargetVBN));
                //animNode.Nodes.Add(filename);
            }
            if (filename.EndsWith(".anim"))
            {
                this.animations.Add(filename, ANIM.read(filename, Runtime.TargetVBN));
                //animNode.Nodes.Add(filename);
            }
        }

        public void openWeaponAnimations(string motionDir)
        {
            string[] files = null;
            try
            {
                files = Directory.GetFiles(System.IO.Path.GetDirectoryName(motionDir));
            }
            catch (DirectoryNotFoundException)
            {
                // Some weapons are just models or scripts with no animations
                return;
            }

            foreach (string s in files)
            {
                this.openWeaponAnimation(s);
            }
        }

        public void makeWeaponModel(int weaponId, string weaponName, string weaponModelDir)
        {
            string[] files = null;
            try
            {
                files = Directory.GetFiles(System.IO.Path.GetDirectoryName(weaponModelDir));
            }
            catch (DirectoryNotFoundException)
            {
                // May just be scripts with no model, e.g. ZSS's "laser"
                return;
            }

            ModelContainer weaponModel = new ModelContainer();
            weaponModel.name = weaponName;

            string pnud = weaponModelDir + "model.nud";
            string pnut = "";
            string pjtb = "";
            string pvbn = "";
            string pmta = "";
            string psb = "";
            string pmoi = "";
            List<string> pacs = new List<string>();

            foreach (string s in files)
            {
                if (s.EndsWith(".nut"))
                    pnut = s;
                if (s.EndsWith(".vbn"))
                    pvbn = s;
                if (s.EndsWith(".jtb"))
                    pjtb = s;
                if (s.EndsWith(".mta"))
                    pmta = s;
                if (s.EndsWith(".sb"))
                    psb = s;
                if (s.EndsWith(".moi"))
                    pmoi = s;
                if (s.EndsWith(".pac"))
                    pacs.Add(s);
            }

            if (!pvbn.Equals(""))
            {
                weaponModel.vbn = new VBN(pvbn);
                //Runtime.TargetVBN = weapon.vbn;
                if (!pjtb.Equals(""))
                    weaponModel.vbn.readJointTable(pjtb);
                if (!psb.Equals(""))
                    weaponModel.vbn.swingBones.Read(psb);
            }

            if (!pnut.Equals(""))
            {
                NUT nut = new NUT(pnut);
                Runtime.TextureContainers.Add(nut);
            }

            if (!pnud.Equals(""))
            {
                weaponModel.nud = new NUD(pnud);

                foreach (string s in pacs)
                {
                    PAC p = new PAC();
                    p.Read(s);
                    byte[] data;
                    p.Files.TryGetValue("default.mta", out data);
                    if (data != null)
                    {
                        MTA m = new MTA();
                        m.read(new FileData(data));
                        weaponModel.nud.applyMTA(m, 0);
                    }
                }
            }

            if (!pmta.Equals(""))
            {
                try
                {
                    weaponModel.mta = new MTA();
                    weaponModel.mta.Read(pmta);
                    string mtaName = Path.Combine(Path.GetFileName(Path.GetDirectoryName(pmta)), Path.GetFileName(pmta));
                    Console.WriteLine($"MTA Name - {mtaName}");
                    // TODO: next line may be buggy for submodels
                    if (!this.materialAnimations.ContainsValue(weaponModel.mta) && !this.materialAnimations.ContainsKey(mtaName))
                        this.materialAnimations.Add(mtaName, weaponModel.mta);
                }
                catch (EndOfStreamException)
                {
                    weaponModel.mta = null;
                }
            }

            if (!pmoi.Equals(""))
            {
                weaponModel.moi = new MOI(pmoi);
            }

            if (weaponModel.nud != null)
            {
                weaponModel.nud.MergePoly();
            }

            this.model = weaponModel;
        }

        public void openWeaponScript(string weaponScriptDir)
        {
            if (File.Exists(weaponScriptDir + "\\motion.mtable"))
            {
                this.moveset = new MovesetManager(weaponScriptDir + "\\motion.mtable");
            }
        }

        // Open the weapons for a given character and associate them
        public static void openWeapons(string characterBaseDir, ModelContainer parentModel)
        {
            // characterBaseDir is the base directory of the model that was being opened
            // e.g. contanis "models", "scripts", "weapons" etc. We assume it is named after the
            // character in question, e.g. "szerosuit" for ZSS.
            string[] splitDir = characterBaseDir.Split('\\');
            string characterName = splitDir[splitDir.Length - 1];
            SortedList<int, string> characterWeapons = Weapon.getCharacterWeapons(characterName);

            foreach (KeyValuePair<int, string> kvp in characterWeapons)
            {
                int weaponId = kvp.Key;
                string weaponName = kvp.Value;

                Weapon weapon = new Weapon(weaponId, weaponName);

                string weaponModelDir = characterBaseDir + "\\model\\" + weaponName + "\\c00\\";
                weapon.makeWeaponModel(weaponId, weaponName, weaponModelDir);

                string weaponAnimDir = characterBaseDir + "\\motion\\" + weaponName + "\\";
                weapon.openWeaponAnimations(weaponAnimDir);

                string weaponScriptDir = characterBaseDir + "\\script\\animcmd\\weapon\\" + weaponName + "\\";
                weapon.openWeaponScript(weaponScriptDir);

                parentModel.weapons.Add(weaponId, weapon);
            }
        }

        // These weapon related lists were dumped from cross_f.rpx. I could
        // have made these into a dictionary or class but I left them like this
        // to show how they were reversed. Performance isn't critical.

        // How to use: indexes match between all 3 lists. E.g. index 0, fireball,
        // belongs to character Mario, who is found under "fighter" (and not "enemy")
        // in the game files.
        public static string[] WEAPON =
        {
            "fireball", "mantle", "pump", "pumpwater", "hugeflame",
            "dokan", "boomerang", "bow", "bowarrow", "hammer",
            "finalcutter", "starmissile", "hat", "ultrasword", "ultraswordhat",
            "warpstar", "reserve", "simple", "miipartshead", "rosettaticomissile",
            "dengekidama", "dengeki", "kaminari", "cloud", "vortex",
            "monsterball", "blaster", "blaster_bullet", "illusion", "arwing",
            "blaster", "blaster_bullet", "illusion", "arwing", "dein",
            "dein_s", "lightingbow", "lightingbowarrow", "phantom", "jethammer",
            "starmissile", "star", "gordo", "bomb", "bombhammer",
            "shrine", "waddledee", "gun", "barreljet", "peanuts",
            "explosion", "dkbarrel", "sword", "star", "tamago",
            "paralyzer_bullet", "whip", "cockpit", "gunship", "laser",
            "pikmin", "dolfin", "win1", "win2", "win3",
            "needle", "needlehave", "fusin", "lightingbow", "lightingbowarrow",
            "cshot", "bomb", "missile", "laser", "laser2",
            "gun", "supermissile", "transportation", "bow", "bowarrow",
            "kassar", "kinopio", "paralyzer", "auraball", "qigong",
            "pkflash", "pkfire", "pkthunder", "pkstarstorm", "yoyo",
            "yoyohead", "clawshot", "clawshothead", "clawshothand", "clawshotall",
            "breath", "sword", "beast", "stone", "gbeam",
            "fireball", "dokan", "obakyumu", "rockbuster", "crashbomb",
            "rushcoil", "leafshield", "chargeshot", "hardknuckle", "blackhole",
            "finalbg", "rockmanx", "rockmandash", "rockmanexe", "shootingstarrockman",
            "hulahoop", "sunbullet", "balanceboard", "wiibo", "towel",
            "hammer", "gravel", "shot", "bullet", "leftarm",
            "rightarm", "doclouis", "sweatlittlemac", "throwsweat", "championbelt",
            "flowerpot", "umbrella", "bowlingball", "firework", "slingshot",
            "bullet", "weeds", "butterflynet", "balloon", "clayrocket",
            "seed", "sprout", "tree", "stump", "sprinkling_water",
            "helmet", "tomnook", "tommy", "timmy", "house",
            "furniture", "moneybag", "beetle", "airshooter", "tico",
            "starpiece", "ring", "pointer", "powerstar", "meteor",
            "godwing", "explosiveflame", "explosiveflame_reserve", "fireworks", "autoaimbullet",
            "autoreticle", "reflectionboard", "blackhole", "beam", "gate",
            "pkbeamgamma", "pkbeamomega", "fire", "parasol", "acidbullet",
            "beam", "needle", "laser", "hadoudan", "shot",
            "book", "window", "thunder", "elwind", "gigafire",
            "chrom", "magicshot", "gimmickjump", "supersonic", "chaosemerald",
            "drcapsule", "drmantle", "stethoscope", "hugecapsule", "capsuleblock",
            "shot", "lastshot", "gyro", "gyroholder", "beam",
            "finalbeam", "hugebeam", "narrowbeam", "widebeam", "wariobike",
            "garlic", "boomerang", "bow", "bowarrow", "hookshot",
            "hookshothead", "hookshothand", "hookshotall", "navy", "fairy",
            "takt", "pig", "bulletm", "card", "energytoss",
            "ice", "breath", "rock", "rockstone", "explosion",
            "monsterball", "cap", "monsterball", "mantle", "food",
            "rescue", "parachute", "panel", "oil", "octopus",
            "normal_weapon", "entry", "beam", "homingbullet", "bomb",
            "energytoss", "pillar", "barrelconga", "soundeffect", "dkbarrel",
            "shot", "beam", "missile", "shot", "shuriken",
            "water", "tatami", "moon", "monsterball", "bunshin",
            "rock", "bluefalcon", "finalbg", "landmaster", "landmaster_shell",
            "mask", "wing", "firebreath", "firecannon", "gunman",
            "gunmanbullet", "clay", "can", "reticle", "kurofukuhat",
            "finalduck", "finalbg", "finalgunman", "finalenemy", "finaldog",
            "finalbird", "finalgrass", "finalcan", "grass", "hat",
            "ironball", "hat", "lightshuriken", "chakram", "tornadoshot",
            "hat", "rapidshot_bullet", "gunnercharge", "miimissile", "supermissile",
            "grenadelauncher", "flamepillar", "stealthbomb", "stealthbomb_s", "bottomshoot",
            "groundbomb", "attackairf_bullet", "laser", "fullthrottle", "esa",
            "trampoline", "firehydrant", "firehydrantwater", "bigpacman", "artisticpoint",
            "hammer", "picopicohammer", "magichand", "cannonball", "kart",
            "remainclown", "shadowmario", "batten", "magic", "sleeppowder",
            "bullet", "flowermine", "shot", "landmaster", "landmaster_shell",
            "fireball", "firetornado", "sword", "coconut", "shot",
            "needle", "laser", "hadoudan", "spike", "spike",
            "slashwave", "attackairf_bullet", "crossbomb", "rapidshot_bullet", "breath",
            "fireball", "dunban", "riki", "finalbg", "three",
            "lightarrow", "chargeshot", "lightpillar", "wariobike", "auraball",
            "qigong", "gbeam", "daimonji", "lizardonm", "bow",
            "bowarrow", "homingsw", "meteorswarm", "largeshot", "firepillar",
            "dummy_fighter", "simple2r", "simple2l", "shadowball", "bindball",
            "mewtwom", "search", "psychobreak", "hadoken", "shinkuhadoken",
            "sack", "pkfreeze", "pkfire", "pkthunder", "pkstarstorm",
            "himohebi", "himohebi", "doseitable", "needle", "roysword",
            "wave", "specialn_bullet", "wickedweavearm", "wickedweaveleg", "bat",
            "finalbg", "gomorrah", "hair", "ryusensya", "dragonhand",
            "spearhand", "waterdragon", "finalbg", "waterstream"
        };

        public static string[] WEAPON_CHARACTER =
        {
            "mario", "mario", "mario", "mario", "mario",
            "mario", "link", "link", "link", "kirby",
            "kirby", "kirby", "kirby", "kirby", "kirby",
            "kirby", "kirby", "kirby", "kirby", "kirby",
            "pikachu", "pikachu", "pikachu", "pikachu", "pikachu",
            "pikachu", "fox", "fox", "fox", "fox",
            "falco", "falco", "falco", "falco", "zelda",
            "zelda", "zelda", "zelda", "zelda", "dedede",
            "dedede", "dedede", "dedede", "dedede", "dedede",
            "dedede", "dedede", "diddy", "diddy", "diddy",
            "diddy", "diddy", "ike", "yoshi", "yoshi",
            "szerosuit", "szerosuit", "szerosuit", "szerosuit", "szerosuit",
            "pikmin", "pikmin", "pikmin", "pikmin", "pikmin",
            "sheik", "sheik", "sheik", "sheik", "sheik",
            "samus", "samus", "samus", "samus", "samus",
            "samus", "samus", "samus", "pit", "pit",
            "peach", "peach", "szerosuit", "lucario", "lucario",
            "ness", "ness", "ness", "ness", "ness",
            "ness", "link", "link", "link", "link",
            "koopa", "ganon", "ganon", "kirby", "samus",
            "luigi", "luigi", "luigi", "rockman", "rockman",
            "rockman", "rockman", "rockman", "rockman", "rockman",
            "rockman", "rockman", "rockman", "rockman", "rockman",
            "wiifit", "wiifit", "wiifit", "wiifit", "wiifit",
            "hammerbros", "popperam", "monoeye", "mettaur", "rockman",
            "rockman", "littlemac", "littlemac", "littlemac", "littlemac",
            "murabito", "murabito", "murabito", "murabito", "murabito",
            "murabito", "murabito", "murabito", "murabito", "murabito",
            "murabito", "murabito", "murabito", "murabito", "murabito",
            "murabito", "murabito", "murabito", "murabito", "murabito",
            "murabito", "murabito", "murabito", "rockman", "rosetta",
            "rosetta", "rosetta", "rosetta", "rosetta", "rosetta",
            "palutena", "palutena", "palutena", "palutena", "palutena",
            "palutena", "palutena", "palutena", "palutena", "palutena",
            "starman", "starman", "keronpa", "waddledee", "kihunter",
            "waddledoo", "plasmawisp", "plasmawisp", "plasmawisp", "shotzo",
            "reflet", "reflet", "reflet", "reflet", "reflet",
            "reflet", "kamek", "sonic", "sonic", "sonic",
            "mariod", "mariod", "mariod", "mariod", "mariod",
            "deathpod", "deathpod", "robot", "robot", "robot",
            "robot", "robot", "robot", "robot", "wario",
            "wario", "toonlink", "toonlink", "toonlink", "toonlink",
            "toonlink", "toonlink", "toonlink", "link", "toonlink",
            "toonlink", "toonlink", "masterhand", "masterhand", "masterhand",
            "masterhand", "lizardon", "lizardon", "lizardon", "lizardon",
            "lizardon", "purin", "purin", "metaknight", "gamewatch",
            "gamewatch", "gamewatch", "gamewatch", "gamewatch", "gamewatch",
            "gamewatch", "gamewatch", "yellowdevil", "meganta", "crazyhand",
            "crazyhand", "crazyhand", "donkey", "donkey", "donkey",
            "bokkuncannon", "bokkuncannon", "bokkuncannon", "peahat", "gekkouga",
            "gekkouga", "gekkouga", "gekkouga", "gekkouga", "gekkouga",
            "octorock", "captain", "captain", "fox", "fox",
            "lucina", "yoshi", "yoshi", "yoshi", "duckhunt",
            "duckhunt", "duckhunt", "duckhunt", "duckhunt", "duckhunt",
            "duckhunt", "duckhunt", "duckhunt", "duckhunt", "duckhunt",
            "duckhunt", "duckhunt", "duckhunt", "duckhunt", "miifighter",
            "miifighter", "miiswordsman", "miiswordsman", "miiswordsman", "miiswordsman",
            "miigunner", "miigunner", "miigunner", "miigunner", "miigunner",
            "miigunner", "miigunner", "miigunner", "miigunner", "miigunner",
            "miigunner", "miigunner", "miigunner", "miigunner", "pacman",
            "pacman", "pacman", "pacman", "pacman", "pacman",
            "koopajr", "koopajr", "koopajr", "koopajr", "koopajr",
            "koopajr", "koopajr", "koopajr", "bokkunmage", "churine",
            "eggrobo", "dafune", "totsukotsu", "falco", "falco",
            "shandera", "shandera", "darknut", "bonkers", "pokkuri",
            "mczakoplasmawisp", "mczakoplasmawisp", "mczakoplasmawisp", "crazyhand", "masterhand",
            "mcblade", "miienemyg", "mcgiant", "miienemyg", "koopag",
            "ridley", "shulk", "shulk", "shulk", "pit",
            "pit", "pit", "pit", "warioman", "lucariom",
            "lucariom", "samus", "lizardon", "lizardon", "pitb",
            "pitb", "mcblade", "mcgiant", "pokkuri", "ridley",
            "common", "kirby", "kirby", "mewtwo", "mewtwo",
            "mewtwo", "mewtwo", "mewtwo", "ryu", "ryu",
            "ryu", "lucas", "lucas", "lucas", "lucas",
            "lucas", "lucas", "lucas", "lucas", "roy",
            "cloud", "bayonetta", "bayonetta", "bayonetta", "bayonetta",
            "bayonetta", "bayonetta", "bayonetta", "kamui", "kamui",
            "kamui", "kamui", "kamui", "kamui"
        };

        public static string[] WEAPON_FIGHTER_ENEMY =
        {
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "enemy", "enemy", "enemy", "enemy", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "enemy", "enemy", "enemy", "enemy", "enemy",
            "enemy", "enemy", "enemy", "enemy", "enemy",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "enemy", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "enemy", "enemy", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "enemy", "enemy", "enemy",
            "enemy", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "enemy", "enemy", "enemy",
            "enemy", "enemy", "fighter", "fighter", "fighter",
            "enemy", "enemy", "enemy", "enemy", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "enemy", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "enemy", "enemy",
            "enemy", "enemy", "enemy", "fighter", "fighter",
            "enemy", "enemy", "enemy", "enemy", "enemy",
            "enemy", "enemy", "enemy", "enemy", "enemy",
            "enemy", "fighter", "enemy", "fighter", "fighter",
            "enemy", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "enemy", "enemy", "enemy", "enemy",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter", "fighter",
            "fighter", "fighter", "fighter", "fighter"
        };
    }
}

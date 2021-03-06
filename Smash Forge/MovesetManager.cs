﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SALT.Scripting.AnimCMD;
using System.IO;
using System.Security.Cryptography;
using OpenTK;
using System.Drawing;

namespace Smash_Forge
{
    public class MovesetManager
    {
        public MovesetManager(string mtablePath)
        {
            var directory = Path.GetDirectoryName(mtablePath);
            string[] files = Directory.EnumerateFiles(directory, "*.bin").ToArray();

            if (files.Length == 0)
                return;

            foreach (string file in files)
            {
                var filename = Path.GetFileName(file);

                if (filename == "game.bin")
                {
                    Game = new ACMDFile(file);
                    Endian = Game.Endian;
                }
                if (filename == "effect.bin")
                {
                    Effect = new ACMDFile(file);
                    Endian = Effect.Endian;
                }
                if (filename == "sound.bin")
                {
                    Sound = new ACMDFile(file);
                    Endian = Sound.Endian;
                }
                if (filename == "expression.bin")
                {
                    Expression = new ACMDFile(file);
                    Endian = Expression.Endian;
                }

            }
            if (File.Exists(mtablePath))
                MotionTable = new MTable(mtablePath, Endian);

            ScriptsHashList = new List<uint>();
            if (MotionTable != null)
                ScriptsHashList.AddRange(MotionTable.ToList());


        }

        public Dictionary<int, Hitbox> Hitboxes { get; set; }

        public Endianness Endian { get; set; }
        public MTable MotionTable { get; set; }

        public ACMDFile Game { get; set; }
        public ACMDFile Effect { get; set; }
        public ACMDFile Sound { get; set; }
        public ACMDFile Expression { get; set; }

        public List<uint> ScriptsHashList { get; set; }

        public acmd_frame CommandsAtFrame(string animation, int frame)
        {
            var _commandsGame = new List<ACMDCommand>();
            var _commandsEffect = new List<ACMDCommand>();
            var _commandsSound = new List<ACMDCommand>();
            var _commandsExpression = new List<ACMDCommand>();
            var _hash = Crc32.Compute(animation.ToLower());

            var info = new acmd_frame();
            info.fnum = frame;

            if (Game != null)
            {
                if (Game.Scripts.ContainsKey(_hash))
                {
                    info.Game = CommandsAtFrame((ACMDScript)Game.Scripts[_hash], frame);
                }
            }
            if (Effect != null)
            {
                if (Effect.Scripts.ContainsKey(_hash))
                {
                    info.Effect = CommandsAtFrame((ACMDScript)Effect.Scripts[_hash], frame);
                }
            }
            if (Sound != null)
            {
                if (Sound.Scripts.ContainsKey(_hash))
                {
                    info.Sound = CommandsAtFrame((ACMDScript)Sound.Scripts[_hash], frame);
                }
            }
            if (Expression != null)
            {
                if (Expression.Scripts.ContainsKey(_hash))
                {
                    info.Expression = CommandsAtFrame((ACMDScript)Expression.Scripts[_hash], frame);
                }
            }
            return info;
        }
        public ACMDCommand[] CommandsAtFrame(ACMDScript script, int frame)
        {
            int curFrame = 0;
            var commands = new List<ACMDCommand>();

            // Only do game.bin scripts for now
            if (Game == null)
                return null;

            foreach (ACMDCommand cmd in script.Commands)
            {
                if (cmd.Ident == 0x42ACFE7D)
                {
                    curFrame = (int)(float)cmd.Parameters[0];
                    continue;
                }
                else if (cmd.Ident == 0x4B7B6E51)
                {
                    curFrame++;
                    continue;
                }

                else if (curFrame == frame)
                {
                    commands.Add(cmd);
                }
                else if (curFrame > frame)
                {
                    return commands.ToArray();
                }
            }
            return commands.ToArray();
        }
    }
    public class acmd_frame
    {
        public acmd_frame()
        {
            ActiveHitboxes = new Dictionary<int, Hitbox>();
        }

        public int fnum;
        public Dictionary<int, Hitbox> ActiveHitboxes { get; set; }

        public ACMDCommand[] Game;
        public ACMDCommand[] Effect;
        public ACMDCommand[] Sound;
        public ACMDCommand[] Expression;
    }

    public class Hitbox
    {
        public float Damage { get; set; }
        public float Angle { get; set; }
        public float KnockbackGrowth { get; set; }
        public float KnockbackBase { get; set; }

        public int Type { get; set; }

        public bool Extended { get; set; }
        public bool Ignore_Throw { get; set; }

        public const int HITBOX = 0;
        public const int GRABBOX = 1;
        public const int WINDBOX = 2;
        public const int SEARCHBOX = 3;

        public int Bone { get; set; }
        public float Size { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Z2 { get; set; }

        // Stuff for interpolation, set during rendering
        // These are *post transform*
        public Vector3 va, va2;

        // Just a quick way to generalise this for all cases without opponent weight being involved
        public float GetSimplifiedKnockback(float damage, float knockbackBase,
            float knockbackGrowth, float targetPercent)
        {
            return (((damage + targetPercent) / 10) + (((targetPercent + damage) * damage) / 20)) *
                (knockbackGrowth / 100) + knockbackBase;
        }

        // These values were chosen to capture the middle 70% of hitboxes. Outliers
        // are just pulled in to the max/min values.
        public static readonly float KB_UPPER_THRESHOLD = 210;
        public static readonly float KB_LOWER_THRESHOLD = 100;
        public int getKnockbackBucket(float knockback)
        {
            float bucketRange = (KB_UPPER_THRESHOLD - KB_LOWER_THRESHOLD) / distinctColors.Count;
            if (knockback < KB_LOWER_THRESHOLD) knockback = KB_LOWER_THRESHOLD;
            if (knockback > KB_UPPER_THRESHOLD) knockback = KB_UPPER_THRESHOLD - 0.001f;
            return (int)Math.Floor((knockback - KB_LOWER_THRESHOLD) / bucketRange);
        }

        // See https://stackoverflow.com/questions/470690/how-to-automatically-generate-n-distinct-colors
        // for a really good overview of how to use distinct colours.
        public static readonly List<Color> distinctColors = new List<Color>()
        {
            Color.FromArgb(0xFF, 0xCE, 0xA2, 0x62), // Grayish yellow
            //Color.FromArgb(0xFF, 0x00, 0x7D, 0x34), // Vivid green
            Color.FromArgb(0xFF, 0xFF, 0xB3, 0x0),  // Vivid yellow
            Color.FromArgb(0xFF, 0xFF, 0x68, 0x00), // Vivid orange
            Color.FromArgb(0xFF, 0xC1, 0x0, 0x20),  // Vivid red
        };

        public Color GetRegularDisplayColor()
        {
            // Ranges from Red (weakest hitbox) to Yellow (strongest hitbox)
            float kb = GetSimplifiedKnockback(Damage, KnockbackBase, KnockbackGrowth, 160);
            return distinctColors[getKnockbackBucket(kb)];
        }

        // The colour to fill a Hitbox with when displaying
        public Color GetDisplayColor()
        {
            Color color;
            switch (Type)
            {
                case Hitbox.HITBOX:
                    if (Ignore_Throw)
                        color = Color.FromArgb(85, Color.Black);
                    else
                        color = Color.FromArgb(85, GetRegularDisplayColor());
                    break;
                case Hitbox.GRABBOX:
                    color = Color.FromArgb(85, Color.Purple);
                    break;
                case Hitbox.WINDBOX:
                    color = Color.FromArgb(85, Color.Blue);
                    break;
                case Hitbox.SEARCHBOX:
                    color = Color.FromArgb(85, Color.DarkOrange);
                    break;
                default:
                    color = Color.FromArgb(85, Color.FloralWhite);
                    break;
            }
            return color;
        }
    }
}

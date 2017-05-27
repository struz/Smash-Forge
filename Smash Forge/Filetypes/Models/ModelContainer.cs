using System;
using System.Collections.Generic;

namespace Smash_Forge
{
    public class ModelContainer
    {
        public string name = "";
        public NUD nud;
        public VBN vbn;
        public MTA mta;
        public MOI moi;

        public BCH bch;

        public DAT dat_melee;

        // For rendering animations involving weapons. None of this stuff will be displayed
        // in the GUI but it will be used when necessary.
        // TODO: I think it's worth making a new ACMDWeapon class and attaching all the stuff to that
        // that way we can track weapon name, weapon id, weapon scripts, etc.
        public SortedList<int, Weapon> weapons;

        public static Dictionary<string, SkelAnimation> Animations { get; set; }
        public static MovesetManager Moveset { get; set; }

        public ModelContainer()
        {
            weapons = new SortedList<int, Weapon>();
        }

        /*
         * This method is for clearing all the GL stuff
         * Don't want wasted buffers :>
         * */
        public void Destroy()
        {
            if(nud != null)
                nud.Destroy();
        }
    }
}


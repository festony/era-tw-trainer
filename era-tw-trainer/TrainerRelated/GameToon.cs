using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace era_tw_trainer.TrainerRelated
{
    class GameToon
    {
        public String name = null;
        public IntPtr mark04Addr = IntPtr.Zero;
        public IntPtr mark54Addr = IntPtr.Zero;
        public IntPtr[] subAddrs;

        public static int STATUS_INDEX = 0;
        public static int STATUS_MAX_INDEX = 1;
        public static int SKILLS_INDEX = 2;
        public static int FEATURES_INDEX = 3;
        public static int EXPERIENCES_INDEX = 4;
        public static int KE_YIN_INDEX = 5;
        public static int TEMP_LEVELS_INDEX = 6;
        public static int HAO_GAN_INDEX = 9;


        public GameToon(IntPtr mark54Addr, IntPtr [] subAddrs)
        {
            //this.name = name;
            //this.mark04Addr = mark04Addr;
            this.mark54Addr = mark54Addr;
            this.subAddrs = subAddrs;
        }

        public IntPtr getStatusAreaBaseAddr()
        {
            return this.subAddrs[STATUS_INDEX];
        }

        public IntPtr getMaxStatusAreaBaseAddr()
        {
            return this.subAddrs[STATUS_MAX_INDEX];
        }

        public IntPtr getSkillsAreaBaseAddr()
        {
            return this.subAddrs[SKILLS_INDEX];
        }

        public IntPtr getFeaturesAreaBaseAddr()
        {
            return this.subAddrs[FEATURES_INDEX];
        }

        public IntPtr getExperiencesAreaBaseAddr()
        {
            return this.subAddrs[EXPERIENCES_INDEX];
        }

        public IntPtr getKeYinAreaBaseAddr()
        {
            return this.subAddrs[KE_YIN_INDEX];
        }

        public IntPtr getTempLevelsAreaBaseAddr()
        {
            return this.subAddrs[TEMP_LEVELS_INDEX];
        }

        public IntPtr getHaoGanAreaBaseAddr()
        {
            return this.subAddrs[HAO_GAN_INDEX];
        }

        public IntPtr getAttrAddress(AreaIndexes index, int offset)
        {
            return this.subAddrs[(int)index] + offset;
        }

        //public Int64 readThroughMark54(int positionAfterMark54, int offset)
        //{
        //    // TODO: maybe do this in trainer class?
        //    if (hGameP == IntPtr.Zero)
        //    {
        //        Trace.WriteLine("No game handler provied...");
        //        return false;
        //    }

        //    if (mark54Addr.ToInt64() < minAddr.ToInt64() || baseAddr.ToInt64() > maxAddr.ToInt64())
        //    {
        //        Trace.WriteLine("Address invalid");
        //        return false;
        //    }

        //    var test = readInt64(hGameP, baseAddr + 8);
        //    if (test != 0x54)
        //    {
        //        Trace.WriteLine("Mark is not 0x54");
        //        return false;
        //    }

        //    Int64[] expectedMarksInSeq = new Int64[] { 0x64, 0x64, 0x64, 0x3E8, 0xC8 };
        //    var currMatchingIndex = 0;

        //    for (int i = 0; i < 10; i++)
        //    {
        //        test = readInt64(hGameP, baseAddr + 8 * (2 + i));
        //        if (test < minAddr.ToInt64() || test > maxAddr.ToInt64())
        //        {
        //            Trace.WriteLine("Read address invalid");
        //            return false;
        //        }
        //        test = getMarkFromBaseAddr(new IntPtr(test));
        //        if (test == BAD_RESULT_FLAG)
        //        {
        //            Trace.WriteLine("Bad data detected");
        //            return false;
        //        }
        //        if (test == expectedMarksInSeq[currMatchingIndex])
        //        {
        //            currMatchingIndex++;
        //            if (currMatchingIndex >= expectedMarksInSeq.Length)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}
    }
}

using era_tw_trainer.TrainerRelated;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static era_tw_trainer.Win32Stuff64bit;

namespace era_tw_trainer
{
    class Trainer
    {
        public IntPtr hGameP = IntPtr.Zero;
        IntPtr minAddr = IntPtr.Zero;
        IntPtr maxAddr = IntPtr.Zero;

        //public List<IntPtr> possibleMark54Addresses = new List<IntPtr>();

        //public List<IntPtr> possibleMoneyAddresses = new List<IntPtr>();
        //public List<IntPtr> possibleStaminaAddresses = new List<IntPtr>();
        public List<String> names = new List<String>();
        public List<IntPtr> possibleMark54Addresses = new List<IntPtr>();
        public List<IntPtr> possibleMark04Addresses = new List<IntPtr>();

        public GameToon player = null;

        public List<GameToon> toons = new List<GameToon>();

        public void getGameProcessHandle()
        {
            hGameP = IntPtr.Zero;
            foreach (var p in Process.GetProcesses())
            {
                //Trace.WriteLine("----" + p.ProcessName);
                if (p.ProcessName.ToLower().Contains("emuera1824+v9+webp"))
                {
                    Trace.WriteLine("found!!" + p.Id);
                    hGameP = OpenProcess(0x1F0FFF, false, p.Id);
                    return;
                }
            }
            Trace.WriteLine("Not found...");
        }

        // TODO: can also pass in the mscorlib.ni.lib addr to compare. should be similar with the value of [baseAddr].
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddr">baseAddr means the addr of the Int64 val before mark xx 00 00 00 00 00 00 00</param>
        /// Return: 0 means missed but can continue; BAD_RESULT_FLAG means totally failed, should exit verification proc
        public Int64 getMarkFromBaseAddr(IntPtr baseAddr)
        {
            if (hGameP == IntPtr.Zero)
            {
                Trace.WriteLine("No game handler provied...");
                return BAD_RESULT_FLAG;
            }

            if (((ulong)baseAddr.ToInt64()) < ((ulong)minAddr.ToInt64()) || ((ulong)baseAddr.ToInt64()) + 8 > ((ulong)maxAddr.ToInt64()))
            {
                Trace.WriteLine("getMarkFromBaseAddr - Address invalid - " + baseAddr.ToInt64().ToString("X"));
                return BAD_RESULT_FLAG;
            }

            var test = readInt64(hGameP, baseAddr);
            if (((ulong)test) < ((ulong)minAddr.ToInt64()) || ((ulong)test) > ((ulong)maxAddr.ToInt64()))
            {
                Trace.WriteLine("getMarkFromBaseAddr - val in [" + baseAddr.ToInt64().ToString("X") + "] is not valid addr " + test.ToString("X"));
                return 0;
            }

            test = readInt64(hGameP, baseAddr + 8);
            if (test == BAD_RESULT_FLAG)
            {
                Trace.WriteLine("Failed to read from base addr + 8");
                return BAD_RESULT_FLAG;
            }

            if (test <= 0 || test > 100000)
            {
                Trace.WriteLine("Value of [base addr + 8] is not a valid mark");
                return 0;
            }

            Trace.WriteLine("Value of [base addr + 8] is " + test);
            return test;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddr">baseAddr means the addr of the Int64 val before mark 54 00 00 00 00 00 00 00</param>
        public bool verifyIsPotentialMark54ToonBlock(IntPtr baseAddr)
        {

            // TODO: change GameToon class to carry not mark54 addr but 4 sub base addrs (stamina, max, features, skills), change this function to just validate all these marks then pass marked addresses into GameToon constructor to try create obj. fail to create means fail verification.
            if (hGameP == IntPtr.Zero)
            {
                Trace.WriteLine("No game handler provied...");
                return false;
            }

            if (((ulong)baseAddr.ToInt64()) < ((ulong)minAddr.ToInt64()) || ((ulong)baseAddr.ToInt64()) > ((ulong)maxAddr.ToInt64()))
            {
                Trace.WriteLine("verifyIsPotentialMark54ToonBlock - Address invalid - " + baseAddr.ToInt64().ToString("X"));
                return false;
            }

            var test = readInt64(hGameP, baseAddr);
            if (((ulong)test) < ((ulong)minAddr.ToInt64()) || ((ulong)test) > ((ulong)maxAddr.ToInt64()))
            {
                Trace.WriteLine("verifyIsPotentialMark54ToonBlock - val in [base addr] is not valid addr");
                return false;
            }

            test = readInt64(hGameP, baseAddr + 8);
            if (test != 0x54)
            {
                Trace.WriteLine("Mark is not 0x54");
                return false;
            }

            Int64[] expectedMarksInSeq = new Int64[] { 0x64, 0x64, 0x64, 0x3E8, 0xC8 };
            var currMatchingIndex = 0;

            for (int i = 0; i < 10; i++)
            {
                test = readInt64(hGameP, baseAddr + 8 * (2 + i));
                if (((ulong)test) < ((ulong)minAddr.ToInt64()) || ((ulong)test) > ((ulong)maxAddr.ToInt64()))
                {
                    Trace.WriteLine("verifyIsPotentialMark54ToonBlock - Read address invalid - " + baseAddr.ToInt64().ToString("X"));
                    return false;
                }
                test = getMarkFromBaseAddr(new IntPtr(test));
                if (test == BAD_RESULT_FLAG)
                {
                    Trace.WriteLine("Bad data detected");
                    return false;
                }
                if (test == expectedMarksInSeq[currMatchingIndex])
                {
                    currMatchingIndex++;
                    if (currMatchingIndex >= expectedMarksInSeq.Length)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddr">baseAddr means the addr of the Int64 val before mark 54 00 00 00 00 00 00 00</param>
        public GameToon tryConstructGameToonFromPotentialMark54ToonBlock(IntPtr baseAddr)
        {

            // TODO: change GameToon class to carry not mark54 addr but 4 sub base addrs (stamina, max, features, skills), change this function to just validate all these marks then pass marked addresses into GameToon constructor to try create obj. fail to create means fail verification.
            if (hGameP == IntPtr.Zero)
            {
                Trace.WriteLine("No game handler provied...");
                return null;
            }

            if (((ulong)baseAddr.ToInt64()) < ((ulong)minAddr.ToInt64()) || ((ulong)baseAddr.ToInt64()) > ((ulong)maxAddr.ToInt64()))
            {
                Trace.WriteLine("tryConstructGameToonFromPotentialMark54ToonBlock - Address invalid - " + baseAddr.ToInt64() + " [ " + minAddr.ToInt64() + " ] <-> [ " + maxAddr.ToInt64() + "]");
                return null;
            }

            var test = readInt64(hGameP, baseAddr);
            if (((ulong)test) < ((ulong)minAddr.ToInt64()) || ((ulong)test) > ((ulong)maxAddr.ToInt64()))
            {
                Trace.WriteLine("tryConstructGameToonFromPotentialMark54ToonBlock - val in [" + baseAddr.ToInt64().ToString("X") + "] is not valid addr " + test.ToString("X"));
                return null;
            }

            test = readInt64(hGameP, baseAddr + 8);
            if (test != 0x54)
            {
                Trace.WriteLine("Mark is not 0x54");
                return null;
            }

            Int64[] expectedMarksInSeq = new Int64[] { 0x64, 0x64, 0x64, 0x3E8, 0xC8 };
            var currMatchingIndex = 0;
            List<IntPtr> subAddrList = new List<IntPtr>();

            Trace.WriteLine("---x--- debug --- 0 --- " + baseAddr.ToInt64().ToString("X"));
            for (int i = 0; i < 20; i++)
            {
                //Trace.WriteLine("---xxxxx--- debug --- 0 --- " + baseAddr.ToInt64().ToString("X") + " " + i);
                //Trace.WriteLine("---xxxxx--- debug --- 0 --- " + (baseAddr + 8 * (2 + i)));
                //Trace.WriteLine("---xxxxx--- debug --- 0 --- " + readInt64(hGameP, baseAddr + 8 * (2 + i)));
                //Trace.WriteLine("---xxxxx--- debug --- 0 --- " + readInt64(hGameP, baseAddr + 8 * (2 + i)).ToString("X"));
                IntPtr subAddr;
                try
                {
                    subAddr = new IntPtr(readInt64(hGameP, baseAddr + 8 * (2 + i)));
                }
                catch(OverflowException e)
                {
                    Trace.WriteLine("tryConstructGameToonFromPotentialMark54ToonBlock - Read too large value - " + readInt64(hGameP, baseAddr + 8 * (2 + i)).ToString("X"));
                    if (i == 0)
                    {
                        // sometimes the first one is a strange large 8 bytes value
                        continue;
                    }
                    return null;
                }

                Trace.WriteLine("---x--- debug --- 1 --- " + subAddr.ToInt64().ToString("X"));
                if (((ulong)subAddr.ToInt64()) < ((ulong)minAddr.ToInt64()) || ((ulong)subAddr.ToInt64()) > ((ulong)maxAddr.ToInt64()))
                {
                    Trace.WriteLine("tryConstructGameToonFromPotentialMark54ToonBlock - Read address invalid - " + baseAddr.ToInt64().ToString("X"));
                    return null;
                }
                test = getMarkFromBaseAddr(subAddr);
                if (test == BAD_RESULT_FLAG)
                {
                    Trace.WriteLine("Bad data detected");
                    return null;
                }
                if (currMatchingIndex < expectedMarksInSeq.Length)
                {
                    if (test == expectedMarksInSeq[currMatchingIndex])
                    {
                        currMatchingIndex++;
                    }
                    else
                    {
                        continue;
                    }
                }
                subAddrList.Add(subAddr);
            }

            if (currMatchingIndex < expectedMarksInSeq.Length)
            {
                return null;
            }
            else
            {
                return new GameToon(baseAddr, subAddrList.ToArray());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddr">baseAddr means the addr of the Int64 val before mark 04 00 00 00 00 00 00 00</param>
        public IntPtr getNameAddrFromPotentialMark04NameBlock(IntPtr baseAddr)
        {

            if (hGameP == IntPtr.Zero)
            {
                Trace.WriteLine("No game handler provied...");
                return new IntPtr(BAD_RESULT_FLAG);
            }

            if (((ulong)baseAddr.ToInt64()) < ((ulong)minAddr.ToInt64()) || ((ulong)baseAddr.ToInt64()) > ((ulong)maxAddr.ToInt64()))
            {
                Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock - Address invalid - " + baseAddr.ToInt64().ToString("X"));
                return new IntPtr(BAD_RESULT_FLAG);
            }

            var test = readInt64(hGameP, baseAddr);
            if (((ulong)test) < ((ulong)minAddr.ToInt64()) || ((ulong)test) > ((ulong)maxAddr.ToInt64()))
            {
                Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock 1 - val in [" + baseAddr.ToInt64().ToString("X") + "] is not valid addr " + test.ToString("X"));
                return new IntPtr(BAD_RESULT_FLAG);
            }

            test = readInt64(hGameP, baseAddr + 8);
            if (test != 0x04)
            {
                Trace.WriteLine("Mark is not 0x04");
                return new IntPtr(BAD_RESULT_FLAG);
            }

            //test = readInt64(hGameP, baseAddr + 16);
            //if (test < minAddr.ToInt64() || test > maxAddr.ToInt64())
            //{
            //    Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock 2 - val in [" + baseAddr.ToInt64().ToString("X") + "] is not valid addr " + test.ToString("X"));
            //    return new IntPtr(BAD_RESULT_FLAG);
            //}

            //baseAddr = new IntPtr(test);

            //test = readInt64(hGameP, baseAddr);
            //if (test < minAddr.ToInt64() || test > maxAddr.ToInt64())
            //{
            //    Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock 3 - val in [" + baseAddr.ToInt64().ToString("X") + "] is not valid addr " + test.ToString("X"));
            //    return new IntPtr(BAD_RESULT_FLAG);
            //}

            //test = readInt32(hGameP, baseAddr + 8);
            //if (test < 0 || test > 20)
            //{
            //    Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock 4 - val in [" + (baseAddr + 8).ToInt64().ToString("X") + "] is not valid length of name string " + test.ToString("X"));
            //    Trace.WriteLine("val in [name pointer + 8] is not valid length of name string");
            //    return new IntPtr(BAD_RESULT_FLAG);
            //}

            //return baseAddr + 8;

            var offset = 0;
            var found = false;
            IntPtr baseAddr1 = baseAddr;

            while (offset < 24 && !found)
            {
                found = true;
                test = readInt64(hGameP, baseAddr + 16 + offset);
                offset += 8;
                if (((ulong)test) < ((ulong)minAddr.ToInt64()) || ((ulong)test) > ((ulong)maxAddr.ToInt64()))
                {
                    Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock 2 - val in [" + (baseAddr + 16 + offset).ToInt64().ToString("X") + "] is not valid addr " + test.ToString("X"));
                    found = false;
                    continue;
                }

                try
                {
                    baseAddr1 = new IntPtr(test);
                }
                catch (OverflowException e)
                {
                    Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock 2 - val in [" + (baseAddr + 16 + offset).ToInt64().ToString("X") + "] is not valid addr " + test.ToString("X"));
                    found = false;
                    continue;
                }

                test = readInt64(hGameP, baseAddr1);
                if (((ulong)test) < ((ulong)minAddr.ToInt64()) || ((ulong)test) > ((ulong)maxAddr.ToInt64()))
                {
                    Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock 3 - val in [" + baseAddr1.ToInt64().ToString("X") + "] is not valid addr " + test.ToString("X"));
                    found = false;
                    continue;
                }

                test = readInt32(hGameP, baseAddr1 + 8);
                if (test < 0 || test > 20)
                {
                    Trace.WriteLine("getNameAddrFromPotentialMark04NameBlock 4 - val in [" + (baseAddr1 + 8).ToInt64().ToString("X") + "] is not valid length of name string " + test.ToString("X"));
                    Trace.WriteLine("val in [name pointer + 8] is not valid length of name string");
                    found = false;
                    continue;
                }
            }

            if (!found)
            {
                return new IntPtr(BAD_RESULT_FLAG);
            }

            return baseAddr1 + 8;

        }

        public String readName(IntPtr baseAddr)
        {
            var test = readInt32(hGameP, baseAddr);
            if (test < 0 || test > 20)
            {
                Trace.WriteLine("val in [name pointer + 8] is not valid length of name string");
                return "WRONG READING";
            }

            var r = "";

            for (int i=0; i<test; i++)
            {
                byte[] codes = new byte[2];
                codes[0] = readByte(hGameP, baseAddr + 4 + i * 2);
                codes[1] = readByte(hGameP, baseAddr + 5 + i * 2);
                var currChar = Encoding.Unicode.GetString(codes);
                //Trace.WriteLine("xxxx--------- " + x);
                r += currChar;
            }

            return r;
        }

        public IntPtr searchFor04MarkFrom54Mark(IntPtr baseAddr54)
        {
            for(int i=4; i < 16; i++)
            {
                IntPtr baseAddr04 = baseAddr54 - i * 8;
                var test = getNameAddrFromPotentialMark04NameBlock(baseAddr04);
                if (test.ToInt64() != BAD_RESULT_FLAG && ((ulong)test.ToInt64()) >= ((ulong)minAddr.ToInt64()) && ((ulong)test.ToInt64()) <= ((ulong)maxAddr.ToInt64()))
                {
                    return test;
                }
            }
            return new IntPtr(BAD_RESULT_FLAG);
        }

        public void initScanMem()
        {
            if (hGameP == IntPtr.Zero)
            {
                Trace.WriteLine("No game handler provied...");
                return;
            }

            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;
            minAddr = proc_min_address;
            maxAddr = proc_max_address;

            Trace.WriteLine("min addr - " + ((ulong)proc_min_address).ToString("X"));
            Trace.WriteLine("max addr - " + ((ulong)proc_max_address).ToString("X"));

            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            //Trace.WriteLine("---- debug 1");

            names = new List<string>();
            possibleMark04Addresses = new List<IntPtr>();
            possibleMark54Addresses = new List<IntPtr>();
            toons = new List<GameToon>();
            player = null;
            //Trace.WriteLine("---- debug 2");

            IntPtr addr = new IntPtr(proc_min_address.ToInt64());
            //Trace.WriteLine("---- debug 3");
            while ((ulong)addr < (ulong)proc_max_address)
            {
                //Trace.WriteLine("---- debug 4 " + addr.ToInt64().ToString("X") + " < " + proc_max_address.ToInt64().ToString("X") + " => + " + mem_basic_info.RegionSize.ToInt64().ToString("X") + "  ==== " + (((ulong)addr.ToInt64()) + ((ulong)mem_basic_info.RegionSize.ToInt64())));
                VirtualQueryEx(hGameP, addr, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                //Trace.WriteLine("---- debug 5" + "  ==== " + (((ulong)addr.ToInt64()) + ((ulong)mem_basic_info.RegionSize.ToInt64())) + "   " + mem_basic_info.RegionSize.ToInt64());
                //addr = new IntPtr(((ulong)addr.ToInt64()) + ((ulong)mem_basic_info.RegionSize.ToInt64()));
                if (((ulong)addr.ToInt64()) + ((ulong)mem_basic_info.RegionSize.ToInt64()) >= ((ulong)proc_max_address.ToInt64()) ||  mem_basic_info.RegionSize.ToInt64() < 0)
                {
                    break;
                }
                addr = new IntPtr(addr.ToInt64() + mem_basic_info.RegionSize.ToInt64());
                if (((uint)(mem_basic_info.Protect) & (uint)(AllocationProtectEnum.PAGE_READWRITE)) > 0 && (ulong)(mem_basic_info.RegionSize) >= 0xCC8)
                {
                    ////Trace.WriteLine("checking addr - " + mem_basic_info.BaseAddress.ToString("X"));
                    //ulong firstLong = (ulong)readInt64(hGameP, mem_basic_info.BaseAddress);
                    //if (firstLong < (ulong)proc_min_address.ToInt64() || firstLong > (ulong)proc_max_address.ToInt64())
                    //{
                    //    continue;
                    //}
                    //IntPtr potentialPlayerNameAddr = new IntPtr((long)firstLong);
                    //var potentialName = readUnicodeStr(hGameP, potentialPlayerNameAddr, MAX_NAME_LEN);
                    ////Trace.WriteLine("potential name - " + potentialName);
                    ////if (potentialName.ToString().Length > 0)
                    ////{
                    ////    Trace.WriteLine("potential name - " + potentialName);
                    ////}

                    //if (!potentialName.ToLower().Equals(playerName))
                    //{
                    //    continue;
                    //}

                    //Trace.WriteLine("mem_basic_info.BaseAddress - " + mem_basic_info.BaseAddress.ToInt64().ToString("X"));
                    //Trace.WriteLine("mem_basic_info.RegionSize - " + ((ulong)mem_basic_info.RegionSize).ToString("X"));
                    //possibleBaseAddrAndBlockSizes[mem_basic_info.BaseAddress] = mem_basic_info.RegionSize;

                    Trace.WriteLine("---- debug 6");
                    int bytesRead = 0;
                    ulong bufferSize = (ulong)(mem_basic_info.RegionSize);
                    byte[] buffer = new byte[bufferSize];

                    // read everything in the buffer above
                    bool r = ReadProcessMemory(hGameP, mem_basic_info.BaseAddress, buffer, (int)mem_basic_info.RegionSize, ref bytesRead);
                    Trace.WriteLine("---- debug 7 " + mem_basic_info);
                    //Trace.WriteLine("---- debug 7 1 " + mem_basic_info.BaseAddress);
                    //Trace.WriteLine("---- debug 7 2 " + mem_basic_info.AllocationBase);
                    //Trace.WriteLine("---- debug 7 3 " + mem_basic_info.AllocationProtect);
                    //Trace.WriteLine("---- debug 7 4 " + mem_basic_info.RegionSize);
                    //Trace.WriteLine("---- debug 7 5 " + mem_basic_info.State);
                    //Trace.WriteLine("---- debug 7 6 " + mem_basic_info.Protect);
                    //Trace.WriteLine("---- debug 7 7 " + mem_basic_info.Type);
                    //Trace.WriteLine("---- debug 7 7 bbbbx " + (buffer == null));
                    //Trace.WriteLine("---- debug 7 7 rrrr " + r + " " + bytesRead + " " + buffer.Length);

                    for (int i=0; i< bytesRead - 8; i+=8)
                    {
                        //Trace.WriteLine("---- debug 7 x 1 " + i);
                        //Trace.WriteLine("---- debug 7 x 1 bbbbx " + (buffer == null));
                        //Trace.WriteLine("---- debug 7 x 1 bbbb " + buffer[i]);
                        //Trace.WriteLine("---- debug 7 x 1 bbbb " + buffer[i + 1]);
                        //Trace.WriteLine("---- debug 7 x 1 bbbb " + buffer[i + 2]);
                        //Trace.WriteLine("---- debug 7 x 1 bbbb " + buffer[i + 3]);
                        //Trace.WriteLine("---- debug 7 x 1 bbbb " + buffer[i + 4]);
                        //Trace.WriteLine("---- debug 7 x 1 bbbb " + buffer[i + 5]);
                        //Trace.WriteLine("---- debug 7 x 1 bbbb " + buffer[i + 6]);
                        //Trace.WriteLine("---- debug 7 x 1 bbbb " + buffer[i + 7]);
                        if (buffer[i] == 0x54
                            && buffer[i + 1] == 0
                            && buffer[i + 2] == 0
                            && buffer[i + 3] == 0
                            && buffer[i + 4] == 0
                            && buffer[i + 5] == 0
                            && buffer[i + 6] == 0
                            && buffer[i + 7] == 0)
                        {
                            // potentially a match, need to verify
                            Trace.WriteLine("---- debug 7 x 2 ");
                            var baseAddr54 = mem_basic_info.BaseAddress + i - 8;
                            //Trace.WriteLine("---- debug 7 x 3 " + baseAddr54);
                            //if (verifyIsPotentialMark54ToonBlock(baseAddr54))
                            //{
                            //    //Trace.WriteLine("-x--x-------- " + (mem_basic_info.BaseAddress + i - 8));

                            //    var baseAddr04 = searchFor04MarkFrom54Mark(baseAddr54) - 8;
                            //    if (baseAddr04.ToInt64() != BAD_RESULT_FLAG)
                            //    {
                            //        String name = readName(baseAddr04 + 8);
                            //        if (!string.IsNullOrWhiteSpace(name))
                            //        {
                            //            Trace.WriteLine("-x--x------name   -- " + name);
                            //            names.Add(name);
                            //            possibleMark54Addresses.Add(baseAddr54);
                            //            possibleMark04Addresses.Add(baseAddr04);

                            //        }
                            //    }
                            //}


                            GameToon toon = tryConstructGameToonFromPotentialMark54ToonBlock(baseAddr54);
                            if (toon != null)
                            {
                                Trace.WriteLine("-x--x-------- " + (mem_basic_info.BaseAddress + i - 8));
                                var baseAddr04 = searchFor04MarkFrom54Mark(baseAddr54) - 8;
                                if (baseAddr04.ToInt64() != BAD_RESULT_FLAG)
                                {
                                    String name = readName(baseAddr04 + 8);
                                    if (!string.IsNullOrWhiteSpace(name))
                                    {
                                        Trace.WriteLine("-x--x------name   -- " + name);
                                        toon.mark04Addr = baseAddr04;
                                        toon.name = name;
                                        names.Add(name);
                                        possibleMark54Addresses.Add(baseAddr54);
                                        possibleMark04Addresses.Add(baseAddr04);
                                        toons.Add(toon);

                                        if (readToonProp(toon, AreaIndexes.MAX_STATUS_AREA, (int)StatusAreaFields.TPS) > 0)
                                        {
                                            player = toon;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public int readToonProp(GameToon toon, AreaIndexes index, int fieldOffset)
        {
            Trace.WriteLine("----- reading " + toon.getAttrAddress(index, fieldOffset).ToString("X"));
            return readInt32(hGameP, toon.getAttrAddress(index, fieldOffset));
        }

        public bool writeToonProp(GameToon toon, AreaIndexes index, int fieldOffset, int value)
        {
            Trace.WriteLine("----- writing " + toon.getAttrAddress(index, fieldOffset).ToString("X"));
            return writeInt32(hGameP, toon.getAttrAddress(index, fieldOffset), value);
        }
        public bool writeToonProp64(GameToon toon, AreaIndexes index, int fieldOffset, Int64 value)
        {
            Trace.WriteLine("----- writing " + toon.getAttrAddress(index, fieldOffset).ToString("X"));
            return writeInt64(hGameP, toon.getAttrAddress(index, fieldOffset), value);
        }

        //TODO: $$$
        // TODO: search?
    }
}

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
        public static int PALAM_INDEX = 6;
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

        public IntPtr getPalamAreaBaseAddr()
        {
            return this.subAddrs[PALAM_INDEX];
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


    class PinYinMap
    {
        public static Dictionary<Char, string> CHAR_2_PINYIN = new Dictionary<char, string>
        {
            {'博', "bo" },
            {'麗', "li" },
            {'霊', "ling" },
            {'夢', "meng" },
            {'留', "liu" },
            {'琴', "qin" },
            {'喀', "ka" },
            {'娜', "na" },
            {'安', "an" },
            {'貝', "bei" },
            {'拉', "la" },
            {'爾', "er" },
            {'魅', "mei" },
            {'魔', "mo" },
            {'桑', "sang" },
            {'尼', "ni" },
            {'米', "mi" },
            {'克', "ke" },
            {'露', "lu" },
            {'切', "qie" },
            {'德', "de" },
            {'斯', "si" },
            {'塔', "ta" },
            {'薩', "sa" },
            {'菲', "fei" },
            {'雅', "ya" },
            {'北', "bei" },
            {'白', "bai" },
            {'河', "he" },
            {'千', "qian" },
            {'百', "bai" },
            {'合', "he" },
            {'岡', "gang" },
            {'崎', "qi" },
            {'美', "mei" },
            {'伊', "yi" },
            {'吹', "chui" },
            {'萃', "cui" },
            {'香', "xiang" },
            {'霧', "wu" },
            {'雨', "yu" },
            {'理', "li" },
            {'沙', "sha" },
            {'亜', "ya" },
            {'大', "da" },
            {'妖', "yao" },
            {'精', "jing" },
            {'琪', "qi" },
            {'諾', "nuo" },
            {'十', "shi" },
            {'六', "liu" },
            {'夜', "ye" },
            {'咲', "sakiguan" },
            {'蕾', "lei" },
            {'莉', "li" },
            {'雷', "lei" },
            {'特', "te" },
            {'愛', "ai" },
            {'絲', "si" },
            {'瑪', "ma" },
            {'格', "ge" },
            {'羅', "luo" },
            {'怀', "huai" },
            {'布', "bu" },
            {'莱', "lai" },
            {'普', "pu" },
            {'利', "li" },
            {'茲', "zi" },
            {'姆', "mu" },
            {'巴', "ba" },
            {'梅', "mei" },
            {'蘭', "lan" },
            {'魂', "hun" },
            {'魄', "po" },
            {'橙', "cheng" },
            {'八', "ba" },
            {'雲', "yun" },
            {'藍', "lan" },
            {'紫', "zi" },
            {'奈', "nai" },
            {'蒂', "di" },
            {'蘿', "luo" },
            {'射', "she" },
            {'命', "ming" },
            {'丸', "wan" },
            {'文', "wen" },
            {'四', "si" },
            {'季', "ji" },
            {'映', "ying" },
            {'姫', "ji" },
            {'東', "dong" },
            {'風', "feng" },
            {'谷', "gu" },
            {'早', "zao" },
            {'苗', "miao" },
            {'坂', "ban" },
            {'神', "shen" },
            {'子', "zi" },
            {'洩', "xie" },
            {'矢', "shi" },
            {'諏', "qu" },
            {'訪', "fang" },
            {'比', "bi" },
            {'那', "na" },
            {'名', "ming" },
            {'居', "ju" },
            {'天', "tian" },
            {'永', "yong" },
            {'江', "jiang" },
            {'衣', "yi" },
            {'玖', "jiu" },
            {'火', "huo" },
            {'焔', "yan" },
            {'猫', "mao" },
            {'燐', "lin" },
            {'烏', "wu" },
            {'路', "lu" },
            {'空', "kong" },
            {'古', "gu" },
            {'明', "ming" },
            {'地', "di" },
            {'恋', "lian" },
            {'納', "na" },
            {'琳', "lin" },
            {'多', "duo" },
            {'良', "liang" },
            {'小', "xiao" },
            {'傘', "san" },
            {'封', "feng" },
            {'獣', "shou" },
            {'鵺', "nueyeniao" },
            {'海', "hai" },
            {'棠', "tang" },
            {'極', "ji" },
            {'茨', "ci" },
            {'木', "mu" },
            {'華', "hua" },
            {'扇', "shan" },
            {'艾', "ai" },
            {'倫', "lun" },
            {'朝', "chao" },
            {'倉', "cang" },
            {'里', "li" },
            {'易', "yi" },
            {'覚', "jue" },
            {'芙', "fu" },
            {'朶', "duo" },
            {'城', "cheng" },
            {'荷', "he" },
            {'取', "qu" },
            {'鈴', "ling" },
            {'仙', "xian" },
            {'優', "you" },
            {'曇', "tan" },
            {'院', "yuan" },
            {'因', "yin" },
            {'幡', "fan" },
            {'帝', "di" },
            {'帕', "pa" },
            {'秋', "qiu" },
            {'聖', "sheng" },
            {'蓮', "lian" },
            {'豊', "feng" },
            {'聡', "cong" },
            {'耳', "er" },
            {'秦', "qin" },
            {'心', "xin" },
            {'紅', "hong" },
            {'悪', "e" },
            {'水', "shui" },
            {'橋', "qiao" },
            {'琶', "pa" },
            {'西', "xi" },
            {'藤', "teng" },
            {'原', "yuan" },
            {'妹', "mei" },
            {'蓬', "peng" },
            {'山', "shan" },
            {'輝', "hui" },
            {'今', "jin" },
            {'泉', "quan" },
            {'影', "ying" },
            {'狼', "lang" },
            {'星', "xing" },
            {'熊', "xiong" },
            {'勇', "yong" },
            {'儀', "yi" },
            {'犬', "quan" },
            {'走', "zou" },
            {'椛', "hua" },
            {'行', "xinghang" },
            {'寺', "si" },
            {'幽', "you" },
            {'上', "shang" },
            {'沢', "chi" },
            {'慧', "hui" },
            {'音', "yin" },
            {'見', "jian" },
            {'二', "er" },
            {'岩', "yan" },
            {'猯', "tuanrui" },
            {'藏', "zangcang" },
            {'本', "ben" },
            {'少', "shao" },
            {'針', "zhen" },
            {'妙', "miao" },
            {'意', "yi" },
            {'赤', "chi" },
            {'蛮', "man" },
            {'奇', "qi" },
            {'霍', "huo" },
            {'瓦', "wa" },
            {'洛', "luo" },
            {'欣', "xin" },
            {'可', "ke" },
            {'野', "ye" },
            {'塚', "zhongzong" },
            {'町', "tingding" },
            {'静', "jing" },
            {'葉', "ye" },
            {'穣', "rang" },
            {'鍵', "jian" },
            {'雛', "chu" },
            {'稗', "baibi" },
            {'田', "tian" },
            {'阿', "a" },
            {'求', "qiu" },
            {'宇', "yu" },
            {'佐', "zuo" },
            {'赫', "he" },
            {'恩', "en" },
            {'黒', "hei" },
            {'女', "nv" },
            {'一', "yi" },
            {'輪', "lun" },
            {'村', "cun" },
            {'紗', "sha" },
            {'蜜', "mi" },
            {'寅', "yin" },
            {'響', "xiang" },
            {'宮', "gong" },
            {'芳', "fang" },
            {'青', "qing" },
            {'娥', "e" },
            {'蘇', "su" },
            {'我', "wo" },
            {'屠', "tu" },
            {'自', "zi" },
            {'物', "wu" },
            {'部', "bu" },
            {'都', "dudou" },
            {'若', "ruo" },
            {'鷺', "lu" },
            {'九', "qiu" },
            {'弁', "bianmang" },
            {'堀', "jue" },
            {'川', "chuan" },
            {'鼓', "gu" },
            {'鬼', "gui" },
            {'人', "ren" },
            {'正', "zheng" },
            {'邪', "xie" },
            {'綿', "mian" },
            {'月', "yue" },
            {'依', "yi" },
            {'號', "hao" },
            {'朱', "zhu" },
            {'綺', "qi" },
            {'雪', "xue" },
            {'舞', "wu" },
            {'菫', "jin" },
            {'清', "qing" },
            {'瑚', "hu" },
            {'来', "lai" },
            {'稀', "xi" },
            {'探', "tan" },
            {'勞', "lao" },
            {'皮', "pi" },
            {'純', "chun" },
            {'狐', "hu" },
            {'提', "ti" },
            {'亞', "ya" },
            {'碧', "bi" },
            {'祖', "zu" },
            {'胡', "hu" },
            {'桃', "tao" },
            {'幻', "huan" },
            {'妮', "ni" },
            {'緹', "ti" },
            {'歡', "huan" },
            {'乃', "nai" },
            {'高', "gao" },
            {'吽', "houhongoumo" },
            {'成', "cheng" },
            {'丁', "ding" },
            {'礼', "li" },
            {'摩', "mo" },
            {'隠', "yin" },
            {'岐', "qi" },
            {'苑', "wan" },
            {'戎', "rong" },
            {'瓔', "ying" },
            {'花', "hua" },
            {'牛', "niu" },
            {'潤', "run" },
            {'庭', "ting" },
            {'渡', "du" },
            {'久', "jiu" },
            {'侘', "chazhai" },
            {'歌', "ge" },
            {'吉', "ji" },
            {'弔', "diaofu" },
            {'杖', "zhang" },
            {'刀', "dao" },
            {'偶', "ou" },
            {'磨', "mo" },
            {'弓', "gong" },
            {'埴', "zhi" },
            {'袿', "guigua" },
            {'驪', "li" },
            {'駒', "jujv" }

        };

        public static HashSet<char> findMatch(string s)
        {
            var r = new HashSet<char>();
            foreach (var c in CHAR_2_PINYIN.Keys)
            {
                if (CHAR_2_PINYIN[c].Contains(s.ToLower()))
                {
                    r.Add(c);
                }
            }
            return r;
        }

        public static bool match(HashSet<char> charset, string name)
        {
            foreach(var c in charset)
            {
                if (name.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static era_tw_trainer.TrainerRelated.FeaturesAreaFields;


namespace era_tw_trainer.TrainerRelated
{
    class FeatureFieldsValueRanges
    {
        public static HashSet<FeaturesAreaFields> fieldsWithRange1To3 = new HashSet<FeaturesAreaFields> { 性别 };
        public static HashSet<FeaturesAreaFields> fieldsWithRangeNeg1To1 = new HashSet<FeaturesAreaFields>
        { 
            胆量, 叛逆, 傲慢, 自尊心, h心情愉快, h美型, 对性好奇, 开朗, 重视贞操, 解放, 害羞, 怕痛, 易湿,
            h学习快, 接受快感, C敏感, V敏感, A敏感, B敏感
        };
        public static HashSet<FeaturesAreaFields> fieldsWithRangeNeg1To2 = new HashSet<FeaturesAreaFields> { 年龄, M敏感, 音感 };
        public static HashSet<FeaturesAreaFields> fieldsWithRangeNeg1To3 = new HashSet<FeaturesAreaFields> { 男女性向 };
        public static HashSet<FeaturesAreaFields> fieldsWithRangeNeg2To2 = new HashSet<FeaturesAreaFields> { 污臭耐性, 胸围 };
        public static HashSet<FeaturesAreaFields> fieldsWithRangeNeg2To3 = new HashSet<FeaturesAreaFields> { 酒量 };
        public static HashSet<FeaturesAreaFields> fieldsWithRange0To3 = new HashSet<FeaturesAreaFields> { 音乐知识 };
        public static HashSet<FeaturesAreaFields> fieldsWithRange0To6 = new HashSet<FeaturesAreaFields> { J大小 };
        public static HashSet<FeaturesAreaFields> fieldsWithRange0To20 = new HashSet<FeaturesAreaFields> { h采集, h调和, h采伐, h钓鱼 };
        public static HashSet<FeaturesAreaFields> fieldsWithSpecialRangeNeg5To3 = new HashSet<FeaturesAreaFields> { 体型 };

        public static int[] getValueRange(FeaturesAreaFields field)
        {
            if (fieldsWithRange1To3.Contains(field))
            {
                return new int[] { 1, 3 };
            }

            if (fieldsWithRangeNeg1To1.Contains(field))
            {
                return new int[] { -1, 1 };
            }

            if (fieldsWithRangeNeg1To2.Contains(field))
            {
                return new int[] { -1, 2 };
            }

            if (fieldsWithRangeNeg1To3.Contains(field))
            {
                return new int[] { -1, 3 };
            }

            if (fieldsWithRangeNeg2To2.Contains(field))
            {
                return new int[] { -2, 2 };
            }

            if (fieldsWithRangeNeg2To3.Contains(field))
            {
                return new int[] { -2, 3 };
            }

            if (fieldsWithRange0To3.Contains(field))
            {
                return new int[] { 0, 3 };
            }

            if (fieldsWithRange0To6.Contains(field))
            {
                return new int[] { 0, 6 };
            }

            if (fieldsWithRange0To20.Contains(field))
            {
                return new int[] { 0, 20 };
            }

            if (fieldsWithSpecialRangeNeg5To3.Contains(field))
            {
                return new int[] { -5, 3 };
            }

            return new int[] { 0, 1 };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace era_tw_trainer.TrainerRelated
{
    enum AreaIndexes
    {
        STATUS_AREA = 0,
        MAX_STATUS_AREA = 1,
        SKILLS_AREA = 2,
        FEATURES_AREA = 3,
        EXPERIENCES_AREA = 4,
        KE_YIN_AREA = 5,
        TEMP_LEVELS_AREA = 6,
        HAO_GAN_AREA = 9
    }

    enum ValueRanges
    {
        RANGE_0_TO_1,
        RANGE_NON_NEG,
        RANGE_0_TO_100,
        RANGE_0_TO_6,
        RANGE_NEG1_TO_1,
        RANGE_NEG1_TO_2,
        RANGE_NEG1_TO_3,
        RANGE_NEG2_TO_2,
        RANGE_NEG2_TO_3,
        RANGE_1_TO_3
    }

    // Same with MaxStatus
    enum StatusAreaFields
    {
        体力 = 0x10,
        气力 = 0x18,
        精力 = 0x40,
        TPS = 0x50,
        酒气 = 0x88
    }

    enum SkillsAreaFeelFields
    {
        C = 0x10,
        V = 0x18,
        A = 0x20,
        B = 0x28,
        M = 0x30
    }

    enum SkillsAreaGeneralFields
    {
        亲密 = 0x58,
        从顺 = 0x60,
        欲望 = 0x68,
        技巧 = 0x70,
        侍奉 = 0x78,
        露出 = 0x80
    }

    enum SkillsAreaXPFields
    {
        M = 0x88,
        S = 0x90,
        百合 = 0x98
    }

    enum SkillsAreaAddictFields
    {
        自慰 = 0x100,
        精液 = 0x108,
        百合 = 0x110,
        腔射 = 0x120,
        肛射 = 0x128
    }

    enum SkillsAreaLiveSkillsFields
    {
        清扫 = 0x150,
        话术 = 0x158,
        战斗 = 0x160,
        教养 = 0x168,
        料理 = 0x170,
        音乐 = 0x178
    }

    enum SkillsAreaHSkillsFields
    {
        指 = 0x1A0,
        舌 = 0x1A8,
        胸 = 0x1B0,
        腰 = 0x1B8,
        腔 = 0x1C0,
        A = 0x1C8
    }

    enum FeaturesAreaFields
    {
        处女 = 0x10,
        处男 = 0x18,
        性别 = 0x20,
        恋慕 = 0x28,
        淫乱 = 0x30,
        服从 = 0x38,
        接吻经验 = 0x40,
        恋人 = 0x48,
        思慕 = 0x50,
        h妊娠愿望 = 0x58,
        胆量 = 0x60,
        叛逆 = 0x68,
        傲慢 = 0x70,
        自尊心 = 0x78,
        傲娇 = 0x80,
        h心情愉快 = 0x88,
        h美型 = 0x90,
        年龄 = 0x98,
        懒散 = 0xa0,
        性情不定 = 0xa8,
        自制 = 0xb0,
        冷漠 = 0xb8,
        感情缺乏 = 0xc0,
        对性好奇 = 0xc8,
        开朗 = 0xd0,
        底线 = 0xd8,
        出风头 = 0xe0,
        无知 = 0xe8,
        //= 0xf0,
        //= 0xf8,
        重视贞操 = 0x100,
        解放 = 0x108,
        抵抗 = 0x110,
        害羞 = 0x118,
        h把柄 = 0x120,
        //= 0x128,
        //= 0x130,
        //= 0x138,
        //= 0x140,
        //= 0x148,
        怕痛 = 0x150,
        易湿 = 0x158,
        //= 0x160,
        //= 0x168,
        //= 0x170,
        //= 0x178,
        h采伐 = 0x180,
        h钓鱼 = 0x188,
        h采集 = 0x190,
        h调和 = 0x198,
        h学习快 = 0x1a0,
        h灵活手指 = 0x1a8,
        h灵活舌头 = 0x1b0,
        h擅长用针 = 0x1b8,
        猫舌 = 0x1c0,
        h调和知识 = 0x1c8,
        h药毒耐性 = 0x1d0,
        漏尿 = 0x1d8,
        //= 0x1e0,
        //= 0x1e8,
        易自慰 = 0x1f0,
        污臭耐性 = 0x1f8,
        献身 = 0x200,
        //= 0x208,
        //= 0x210,
        //= 0x218,
        //= 0x220,
        //= 0x228,
        //= 0x230,
        //= 0x238,
        接受快感 = 0x240,
        容易中毒 = 0x248,
        h容易高潮 = 0x250,
        h自慰狂 = 0x258,
        h淫壶 = 0x260,
        h尻穴狂 = 0x268,
        h淫乳 = 0x270,
        h接吻魔 = 0x278,
        h子宫口性感 = 0x280,
        //= 0x288,
        倒错的 = 0x290,
        男女性向 = 0x298,
        S = 0x2a0,
        M = 0x2a8,
        h嫉妒 = 0x2b0,
        迷信 = 0x2b8,
        小恶魔 = 0x2c0,
        //= 0x2c8,
        //= 0x2d0,
        //= 0x2d8,
        狐 = 0x2e0,
        妖狐 = 0x2e8,
        h魅力 = 0x2f0,
        h魅惑 = 0x2f8,
        h谜之魅力 = 0x300,
        //= 0x308,
        //= 0x310,
        //= 0x318,
        //= 0x320,
        //= 0x328,
        体型 = 0x330,
        C敏感 = 0x338,
        V敏感 = 0x340,
        A敏感 = 0x348,
        B敏感 = 0x350,
        胸围 = 0x358,
        M敏感 = 0x360,
        //= 0x368,
        //= 0x370,
        //= 0x378,
        //= 0x380,
        //= 0x388,
        //= 0x390,
        //= 0x398,
        //= 0x3a0,
        //= 0x3a8,
        //= 0x3b0,
        //= 0x3b8,
        //= 0x3c0,
        //= 0x3c8,
        h浓厚精液 = 0x3d0,
        酒量 = 0x3d8,
        h精爱味觉 = 0x3e0,
        //= 0x3e8,
        //= 0x3f0,
        //= 0x3f8,
        //= 0x400,
        //= 0x408,
        //= 0x410,
        //= 0x418,
        h恢复快 = 0x420,
        //= 0x428,
        h吸血鬼 = 0x430,
        h日光浴 = 0x438,
        h月光浴 = 0x440,
        h治疗 = 0x448,
        h鼓舞 = 0x450,
        h大胃王 = 0x458,
        h不可思议的根 = 0x460,
        //= 0x468,
        //= 0x470,
        h狂气 = 0x478,
        风骚 = 0x480,
        动物耳 = 0x488,
        h禁断的知识 = 0x490,
        h人气 = 0x498,
        具现 = 0x4a0,
        //= 0x4a8,
        //= 0x4b0,
        h母乳体质 = 0x4b8,
        幼儿 = 0x4c0,
        幼稚 = 0x4c8,
        h精力超群 = 0x4d0,
        h妊娠中 = 0x4d8,
        h育儿中 = 0x4e0,
        h母性 = 0x4e8,
        初潮 = 0x4f0,
        h劣情查知 = 0x4f8,
        h内裤走私者 = 0x500,
        h居场所查知 = 0x508,
        h污部屋查知 = 0x510,
        h乐器知识 = 0x518,
        h音感 = 0x520,
        h水栖 = 0x528,
        h水精 = 0x530,
        //h种族？？ = 0x538,
        h半白泽 = 0x540,
        h集合 = 0x548,
        //= 0x550,
        hM钝感无视 = 0x558,
        hC钝感无视 = 0x560,
        hB钝感无视 = 0x568,
        hV钝感无视 = 0x570,
        hA钝感无视 = 0x578,
        h清扫中毒 = 0x580,
        h千客万来 = 0x588,
        h左拥右抱 = 0x590,
        //= 0x598,
        //= 0x5a0,
        //= 0x5a8,
        //= 0x5b0,
        //= 0x5b8,
        h扫除系 = 0x5c0,
        h双枪 = 0x5c8,
        h爱欲 = 0x5d0,
        h炮友 = 0x5d8,
        //= 0x5e0,
        //= 0x5e8,
        //= 0x5f0,
        h妄想的产物 = 0x5f8,
        h妖精 = 0x600,
        h蓬莱人 = 0x608,
        h非生物 = 0x610,
        //= 0x618,
        h人类 = 0x620,
        h鬼 = 0x628,
        h付丧神 = 0x630,
        h天狗 = 0x638,
        h人妻 = 0x640,
        //= 0x648,
        //= 0x650,
        J大小 = 0x7E0
    }

    enum KeYinAreaFields
    {
        苦痛 = 0x10,
        快乐 = 0x18,
        不将 = 0x20,
        反斗 = 0x28,
        履历 = 0x30,
        时奸 = 0x38
    }

    enum HaoGanAreaFields
    {
        好感 = 0x20,
        依赖 = 0x30
    }
}

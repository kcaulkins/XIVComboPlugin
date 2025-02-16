using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class BRD
    {
        public const byte ClassID = 5;
        public const byte JobID = 23;

        public const uint
            HeavyShot = 97,
            StraightShot = 98,
            VenomousBite = 100,
            RagingStrikes = 101,
            QuickNock = 106,
            Bloodletter = 110,
            Windbite = 113,
            RainOfDeath = 117,
            EmpyrealArrow = 3558,
            WanderersMinuet = 3559,
            IronJaws = 3560,
            Sidewinder = 3562,
            PitchPerfect = 7404,
            CausticBite = 7406,
            Stormbite = 7407,
            RefulgentArrow = 7409,
            BurstShot = 16495,
            ApexArrow = 16496,
            Shadowbite = 16494,
            Ladonsbite = 25783,
            RadiantFinale = 25785,
            BattleVoice = 118,
            MagesBallad = 114,
            Army = 116,
            BlastArrow = 25784;

        public static class Buffs
        {
            public const ushort
                StraightShotReady = 122,
                BlastShotReady = 2692,
                RagingStrikes = 125,
                Barrage = 128,
                ShadowbiteReady = 3002;
        }

        public static class Debuffs
        {
            public const ushort
                VenomousBite = 124,
                Windbite = 129,
                CausticBite = 1200,
                Stormbite = 1201;
        }

        public static class Levels
        {
            public const byte
                StraightShot = 2,
                VenomousBite = 6,
                Bloodletter = 12,
                Windbite = 30,
                RainOfDeath = 45,
                PitchPerfect = 52,
                EmpyrealArrow = 54,
                IronJaws = 56,
                Sidewinder = 60,
                BiteUpgrade = 64,
                RefulgentArrow = 70,
                Shadowbite = 72,
                BurstShot = 76,
                ApexArrow = 80,
                Ladonsbite = 82,
                RadiantFinale = 90,
                BlastShot = 86;
        }
    }

    internal class BardWanderersPitchPerfectFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BardWanderersPitchPerfectFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.WanderersMinuet)
            {
                var gauge = GetJobGauge<BRDGauge>();

                if (level >= BRD.Levels.PitchPerfect && gauge.Song == Song.WANDERER && gauge.Repertoire >= 1)
                    return BRD.PitchPerfect;

                // Choose song
                if (gauge.Song == Song.WANDERER && gauge.SongTimer <= 2)
                    return CalcBestAction(BRD.MagesBallad, BRD.MagesBallad, BRD.Army);

                if (gauge.Song == Song.MAGE && gauge.SongTimer <= 11)
                    return CalcBestAction(BRD.WanderersMinuet, BRD.WanderersMinuet, BRD.Army);

                if (gauge.Song == Song.ARMY && gauge.SongTimer <= 2)
                    return CalcBestAction(BRD.WanderersMinuet, BRD.WanderersMinuet, BRD.MagesBallad);

                if(gauge.SongTimer <= 11)
                    return CalcBestAction(BRD.WanderersMinuet, BRD.WanderersMinuet, BRD.MagesBallad);
            }

            return actionID;
        }
    }


    // BUTTON 1
    // BUTTON 1
    // BUTTON 1
    // BUTTON 1
    // BUTTON 1
    // BUTTON 1
    internal class BardStraightShotUpgradeFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BrdAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.HeavyShot || actionID == BRD.BurstShot)
            {
                if (level >= BRD.Levels.StraightShot && HasEffect(BRD.Buffs.Barrage))
                    // Refulgent Arrow
                    return OriginalHook(BRD.StraightShot);



                if (level < BRD.Levels.Windbite)
                    return BRD.VenomousBite;

                if (level < BRD.Levels.IronJaws)
                {
                    var venomous = FindTargetEffect(BRD.Debuffs.VenomousBite);
                    var windbite = FindTargetEffect(BRD.Debuffs.Windbite);

                    if (venomous is null)
                        return BRD.VenomousBite;

                    if (windbite is null)
                        return BRD.Windbite;
                }

                if (level < BRD.Levels.BiteUpgrade)
                {
                    var venomous = TargetHasEffect(BRD.Debuffs.VenomousBite);
                    var windbite = TargetHasEffect(BRD.Debuffs.Windbite);

                    if ((venomous && windbite) && (TargetEffectDuration(BRD.Debuffs.Windbite) < 4 || TargetEffectDuration(BRD.Debuffs.VenomousBite) < 4))
                        return BRD.IronJaws;

                    if (HasEffect(BRD.Buffs.RagingStrikes) && GetCooldown(BRD.RagingStrikes).CooldownRemaining <= 103)
                        return BRD.IronJaws;

                    if (windbite && !venomous)
                        return BRD.VenomousBite;

                    if (!windbite)
                        return BRD.Windbite;
                }

                if (level >= BRD.Levels.BiteUpgrade)
                {
                    var caustic = TargetHasEffect(BRD.Debuffs.CausticBite);
                    var stormbite = TargetHasEffect(BRD.Debuffs.Stormbite);

                    if ((caustic && stormbite) && (TargetEffectDuration(BRD.Debuffs.CausticBite) < 4 || TargetEffectDuration(BRD.Debuffs.Stormbite) < 4))
                        return BRD.IronJaws;

                    if (stormbite && !caustic)
                        return BRD.CausticBite;
                    if (!stormbite)
                        return BRD.Stormbite;
                }







                if (IsEnabled(CustomComboPreset.BardApexFeature))
                {
                    var gauge = GetJobGauge<BRDGauge>();

                    if (level >= BRD.Levels.ApexArrow && gauge.SoulVoice == 100 && GetCooldown(BRD.RagingStrikes).CooldownRemaining >= 45)
                        return BRD.ApexArrow;

                    if (level >= BRD.Levels.ApexArrow && gauge.SoulVoice >= 80 && HasEffect(BRD.Buffs.RagingStrikes) && GetCooldown(BRD.RagingStrikes).CooldownRemaining >= 116)
                        return BRD.ApexArrow;

                    if (level >= BRD.Levels.BlastShot && HasEffect(BRD.Buffs.BlastShotReady))
                        return BRD.BlastArrow;
                }

                if (IsEnabled(CustomComboPreset.BardStraightShotUpgradeFeature))
                {
                    if (level >= BRD.Levels.StraightShot && HasEffect(BRD.Buffs.StraightShotReady))
                        // Refulgent Arrow
                        return OriginalHook(BRD.StraightShot);
                }
            }

            return actionID;
        }
    }

    internal class BardIronJawsFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BardIronJawsFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.IronJaws)
            {
                if (level < BRD.Levels.Windbite)
                    return BRD.VenomousBite;

                if (level < BRD.Levels.IronJaws)
                {
                    var venomous = FindTargetEffect(BRD.Debuffs.VenomousBite);
                    var windbite = FindTargetEffect(BRD.Debuffs.Windbite);

                    if (venomous is null)
                        return BRD.VenomousBite;

                    if (windbite is null)
                        return BRD.Windbite;

                    if (venomous?.RemainingTime < windbite?.RemainingTime)
                        return BRD.VenomousBite;

                    return BRD.Windbite;
                }

                if (level < BRD.Levels.BiteUpgrade)
                {
                    var venomous = TargetHasEffect(BRD.Debuffs.VenomousBite);
                    var windbite = TargetHasEffect(BRD.Debuffs.Windbite);

                    if (venomous && windbite)
                        return BRD.IronJaws;

                    if (windbite)
                        return BRD.VenomousBite;

                    return BRD.Windbite;
                }

                var caustic = TargetHasEffect(BRD.Debuffs.CausticBite);
                var stormbite = TargetHasEffect(BRD.Debuffs.Stormbite);

                if (caustic && stormbite)
                    return BRD.IronJaws;

                if (stormbite)
                    return BRD.CausticBite;

                return BRD.Stormbite;
            }

            return actionID;
        }
    }

    internal class BardShadowbiteFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BrdAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.QuickNock || actionID == BRD.Ladonsbite)
            {
                if (IsEnabled(CustomComboPreset.BardApexFeature))
                {
                    var gauge = GetJobGauge<BRDGauge>();

                    if (level >= BRD.Levels.ApexArrow && gauge.SoulVoice == 100)
                        return BRD.ApexArrow;

                    if (level >= BRD.Levels.BlastShot && HasEffect(BRD.Buffs.BlastShotReady))
                        return BRD.BlastArrow;
                }

                if (IsEnabled(CustomComboPreset.BardShadowbiteFeature))
                {
                    if (level >= BRD.Levels.Shadowbite && HasEffect(BRD.Buffs.ShadowbiteReady))
                        return BRD.Shadowbite;
                }
            }

            return actionID;
        }
    }


    // BUTTON 2 BUTTON 2 BUTTON 2
    // BUTTON 2 BUTTON 2 BUTTON 2
    // BUTTON 2 BUTTON 2 BUTTON 2
    // BUTTON 2 BUTTON 2 BUTTON 2
    // BUTTON 2 BUTTON 2 BUTTON 2
    internal class BardBloodletterFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BardBloodletterFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.Bloodletter)
            {
                if (level >= BRD.Levels.Sidewinder && HasEffect(BRD.Buffs.RagingStrikes) && GetCooldown(BRD.RagingStrikes).CooldownRemaining >= 117)
                    return CalcBestAction(BRD.EmpyrealArrow, BRD.EmpyrealArrow, BRD.Bloodletter);

                if (level >= BRD.Levels.Sidewinder)
                    return CalcBestAction(BRD.EmpyrealArrow, BRD.EmpyrealArrow, BRD.Bloodletter, BRD.Sidewinder);

                if (level >= BRD.Levels.EmpyrealArrow)
                    return CalcBestAction(BRD.EmpyrealArrow, BRD.EmpyrealArrow, BRD.Bloodletter);

                if (level >= BRD.Levels.Bloodletter)
                    return BRD.Bloodletter;
            }

            return actionID;
        }
    }

    internal class BardRadiantFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BardRadiantFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.RadiantFinale)
            {
                if (level < BRD.Levels.RadiantFinale)
                    return BRD.BattleVoice;

                return CalcBestAction(actionID, BRD.BattleVoice, BRD.RadiantFinale);
            }

            return actionID;
        }
    }

    internal class BardRainOfDeathFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BardRainOfDeathFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.RainOfDeath)
            {
                if (level >= BRD.Levels.Sidewinder)
                    return CalcBestAction(actionID, BRD.RainOfDeath, BRD.EmpyrealArrow, BRD.Sidewinder);

                if (level >= BRD.Levels.EmpyrealArrow)
                    return CalcBestAction(actionID, BRD.RainOfDeath, BRD.EmpyrealArrow);

                if (level >= BRD.Levels.RainOfDeath)
                    return BRD.RainOfDeath;
            }

            return actionID;
        }
    }
}

namespace AnonymouslyXerath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Color = System.Drawing.Color;

    internal enum Spells
    {
        Q,

        W,

        E,

        R
    }

    internal class Xerath
    {
        #region Static Fields

        public static Orbwalking.Orbwalker Orbwalker;

        public static Dictionary<Spells, Spell> spells = new Dictionary<Spells, Spell>
                                                             {
                                                                 { Spells.Q, new Spell(SpellSlot.Q, 1600) },
                                                                 { Spells.W, new Spell(SpellSlot.W, 1000) },
                                                                 { Spells.E, new Spell(SpellSlot.E, 1150) },
                                                                 { Spells.R, new Spell(SpellSlot.R, 5600) }
                                                             };

        private static SpellSlot _ignite;

        private static int lastNotification;

        #endregion

        #region Public Properties

        public static bool CastingR
        {
            get
            {
                return ObjectManager.Player.HasBuff("XerathLocusOfPower2")
                       || (ObjectManager.Player.LastCastedSpellName() == "XerathLocusOfPower2"
                           && Environment.TickCount - ObjectManager.Player.LastCastedSpellT() < 500);
            }
        }

        #endregion

        #region Properties

        private static HitChance CustomHitChance
        {
            get
            {
                return GetHitchance();
            }
        }

        private static Obj_AI_Hero Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.CharData.BaseSkinName != "Xerath")
            {
                return;
            }

            spells[Spells.Q].SetSkillshot(0.6f, 95f, float.MaxValue, false, SkillshotType.SkillshotLine);
            spells[Spells.W].SetSkillshot(0.7f, 125f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            spells[Spells.E].SetSkillshot(0.25f, 60f, 1400f, true, SkillshotType.SkillshotLine);
            spells[Spells.R].SetSkillshot(0.7f, 130f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            spells[Spells.Q].SetCharged("XerathArcanopulseChargeUp", "XerathArcanopulseChargeUp", 750, 1550, 1.5f);
            _ignite = Player.GetSpellSlot("summonerdot");

            ElXerathMenu.Initialize();
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += Drawings.Drawing_OnDraw;
            Obj_AI_Base.OnIssueOrder += Obj_AI_Hero_OnIssueOrder;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Game.OnWndProc += Game_OnWndProc;
        }

        #endregion

        #region Methods

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (!gapcloser.Sender.IsValidTarget(spells[Spells.E].Range)
                || gapcloser.Sender.Distance(ObjectManager.Player) > spells[Spells.E].Range)
            {
                return;
            }

            if (gapcloser.Sender.IsValidTarget(spells[Spells.E].Range)
                && (AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.misc.Antigapcloser").IsActive() && spells[Spells.E].IsReady()))
            {
                spells[Spells.E].Cast(gapcloser.Sender);
            }
        }

        private static void AutoHarassMode()
        {
            var target = TargetSelector.GetTarget(spells[Spells.Q].ChargedMaxRange, TargetSelector.DamageType.Magical);
            var wTarget = TargetSelector.GetTarget(
                spells[Spells.W].Range + spells[Spells.W].Width * 0.5f,
                TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (ElXerathMenu.Menu.Item("AnonymouslyXerath.AutoHarass").GetValue<KeyBind>().Active)
            {
                var q = AnonymouslyXerathMenu.Menu.Item("Anonymouslyerath.UseQAutoHarass").IsActive();
                var w = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.UseWAutoHarass").IsActive();
                var mana = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.harass.mana").GetValue<Slider>().Value;

                if (Player.ManaPercent < mana)
                {
                    return;
                }

                if (q && spells[Spells.Q].IsReady() && target.IsValidTarget(spells[Spells.Q].ChargedMaxRange))
                {
                    if (!spells[Spells.Q].IsCharging)
                    {
                        spells[Spells.Q].StartCharging();
                        return;
                    }

                    if (spells[Spells.Q].IsCharging)
                    {
                        var pred = spells[Spells.Q].GetPrediction(target);
                        if (pred.Hitchance >= CustomHitChance)
                        {
                            spells[Spells.Q].Cast(target);
                        }
                    }
                }
                if (wTarget != null && w && spells[Spells.W].IsReady())
                {
                    var pred = spells[Spells.W].GetPrediction(wTarget);
                    if (pred.Hitchance >= CustomHitChance)
                    {
                        spells[Spells.W].Cast(wTarget);
                    }
                }
            }
        }

        private static void CastR(Obj_AI_Base target)
        {
            var useR = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.R.AutoUseR").IsActive();
            var tapkey = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.R.OnTap").GetValue<KeyBind>().Active;
            var ultRadius = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.R.Radius").GetValue<Slider>().Value;
            var drawROn = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.Draw.RON").IsActive();

            if (!useR)
            {
                return;
            }

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            var ultType = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.R.Mode").GetValue<StringList>().SelectedIndex;

            if (target.Health - spells[Spells.R].GetDamage(target) < 0)
            {
                if (Utils.TickCount - RCombo.CastSpell <= 700)
                {
                    return;
                }
            }

            if ((RCombo._index != 0 && target.Distance(RCombo._position) > 1000))
            {
                if (Utils.TickCount - RCombo.CastSpell <= Math.Min(2500, target.Distance(RCombo._position) - 1000))
                {
                    return;
                }
            }

            switch (ultType)
            {
                case 0:
                    spells[Spells.R].Cast(target);
                    break;

                case 1:
                    var d = AnonymouslyXerathMenu.Menu.Item("Delay" + (RCombo._index + 1)).GetValue<Slider>().Value;
                    if (Utils.TickCount - RCombo.CastSpell > d)
                    {
                        spells[Spells.R].Cast(target);
                    }
                    break;

                case 2:
                    //if (tapkey)
                    if (RCombo._tapKey)
                    {
                        spells[Spells.R].Cast(target);
                    }
                    break;

                case 3:
                    if (spells[Spells.R].GetPrediction(target).Hitchance >= CustomHitChance)
                    {
                        spells[Spells.R].Cast(target);
                    }

                    break;

                case 4:

                    if (Game.CursorPos.Distance(target.ServerPosition) < ultRadius
                        && ObjectManager.Player.Distance(target.ServerPosition) < spells[Spells.R].Range)
                    {
                        spells[Spells.R].Cast(target);
                    }

                    if (drawROn)
                    {
                        Render.Circle.DrawCircle(Game.CursorPos, ultRadius, Color.White);
                    }

                    break;
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(spells[Spells.Q].ChargedMaxRange, TargetSelector.DamageType.Magical);
            if (!target.IsValidTarget())
            {
                return;
            }

            var comboQ = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.Combo.Q").IsActive();
            var comboW = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.Combo.W").IsActive();
            var comboE = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.Combo.E").IsActive();

            if (comboE && spells[Spells.E].IsReady() && Player.Distance(target) < spells[Spells.E].Range)
            {
                spells[Spells.E].Cast(target);
            }

            if (comboW && spells[Spells.W].IsReady())
            {
                var prediction = spells[Spells.W].GetPrediction(target);
                if (prediction.Hitchance >= HitChance.VeryHigh)
                {
                    spells[Spells.W].Cast(prediction.CastPosition);
                }
            }

            if (comboQ && spells[Spells.Q].IsReady() && target.IsValidTarget(spells[Spells.Q].ChargedMaxRange))
            {
                if (!spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].StartCharging();
                }

                if (spells[Spells.Q].IsCharging)
                {
                    var prediction = spells[Spells.Q].GetPrediction(target);
                    if (prediction.Hitchance >= HitChance.VeryHigh)
                    {
                        spells[Spells.Q].Cast(prediction.CastPosition);
                    }
                }
            }

            if (Player.Distance(target) <= 600 && IgniteDamage(target) >= target.Health
                && ElXerathMenu.Menu.Item("AnonymouslyXerath.Ignite").IsActive())
            {
                Player.Spellbook.CastSpell(_ignite, target);
            }
        }

//
        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint)WindowsMessages.WM_KEYUP)
            {
                RCombo._tapKey = true;
            }
        }

        private static HitChance GetHitchance()
        {
            switch (AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.hitChance").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                case 3:
                    return HitChance.VeryHigh;
                default:
                    return HitChance.Medium;
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(spells[Spells.Q].ChargedMaxRange, TargetSelector.DamageType.Magical);
            var wTarget = TargetSelector.GetTarget(
                spells[Spells.W].Range + spells[Spells.W].Width * 0.5f,
                TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            var harassQ = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.Harass.Q").IsActive();
            var harassW = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.Harass.W").IsActive();

            if (wTarget != null && harassW && spells[Spells.W].IsReady())
            {
                spells[Spells.W].CastIfHitchanceEquals(wTarget, CustomHitChance);
            }

            if (harassQ && spells[Spells.Q].IsReady() && spells[Spells.Q].IsInRange(target)
                && target.IsValidTarget(spells[Spells.Q].ChargedMaxRange))
            {
                if (!spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].StartCharging();
                    return;
                }

                if (spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].CastIfHitchanceEquals(target, CustomHitChance);
                }
            }
        }

        private static float IgniteDamage(Obj_AI_Base target)
        {
            if (_ignite == SpellSlot.Unknown || Player.Spellbook.CanUseSpell(_ignite) != SpellState.Ready)
            {
                return 0f;
            }
            return (float)Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);
        }

        private static void JungleClear()
        {
            var clearQ = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.jclear.Q").IsActive();
            var clearW = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.jclear.W").IsActive();
            var clearE = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.jclear.E").IsActive();
            var minmana = AnonymouslyXerathMenu.Menu.Item("minmanaclear").GetValue<Slider>().Value;

            if (Player.ManaPercent < minmana)
            {
                return;
            }

            var minions = MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition,
                spells[Spells.W].Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

            if (minions.Count <= 0)
            {
                return;
            }

            if (spells[Spells.Q].IsCharging)
            {
                if (minions.Max(x => x.Distance(Player, true)) < spells[Spells.Q].RangeSqr)
                {
                    if (minions.Max(x => x.Distance(Player, true)) < spells[Spells.Q].RangeSqr)
                    {
                        spells[Spells.Q].Cast(spells[Spells.Q].GetLineFarmLocation(minions).Position);
                    }
                }
            }

            if (spells[Spells.Q].IsCharging)
            {
                return;
            }

            if (spells[Spells.Q].IsReady() && clearQ)
            {
                if (spells[Spells.Q].GetLineFarmLocation(minions).MinionsHit >= 1)
                {
                    spells[Spells.Q].StartCharging();
                    return;
                }
            }

            if (spells[Spells.W].IsReady() && clearW)
            {
                var farmLocation = spells[Spells.W].GetCircularFarmLocation(minions);
                spells[Spells.W].Cast(farmLocation.Position);
            }

            if (spells[Spells.E].IsReady() && clearE)
            {
                spells[Spells.E].Cast();
            }
        }

        private static void KsMode()
        {
            var useKs = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.misc.ks").IsActive();
            if (!useKs)
            {
                return;
            }

            var target =
                HeroManager.Enemies.FirstOrDefault(
                    x =>
                    !x.HasBuffOfType(BuffType.Invulnerability) && !x.HasBuffOfType(BuffType.SpellShield)
                    && spells[Spells.Q].CanCast(x) && (x.Health + (x.HPRegenRate / 2)) <= spells[Spells.Q].GetDamage(x));

            if (spells[Spells.Q].CanCast(target) && spells[Spells.Q].IsReady())
            {
                if (!spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].StartCharging();
                }
                if (spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].Cast(target);
                }
            }
        }

        private static void LaneClear()
        {
            var clearQ = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.clear.Q").IsActive();
            var clearW = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.clear.W").IsActive();
            var minmana = AnonymouslyXerathMenu.Menu.Item("minmanaclear").GetValue<Slider>().Value;

            if (Player.ManaPercent < minmana)
            {
                return;
            }

            var minions = MinionManager.GetMinions(Player.ServerPosition, spells[Spells.Q].ChargedMaxRange);
            if (minions.Count <= 0)
            {
                return;
            }

            if (clearQ && spells[Spells.Q].IsReady())
            {
                if (spells[Spells.Q].IsCharging)
                {
                    var bestFarmPos = spells[Spells.Q].GetLineFarmLocation(minions);
                    if (minions.Count == minions.Count(x => Player.Distance(x) < spells[Spells.Q].Range)
                        && bestFarmPos.Position.IsValid() && bestFarmPos.MinionsHit > 0)
                    {
                        spells[Spells.Q].Cast(bestFarmPos.Position);
                    }
                }
                else if (minions.Count > 0)
                {
                    spells[Spells.Q].StartCharging();
                }
            }

            if (spells[Spells.W].IsReady() && clearW)
            {
                var farmLocation = spells[Spells.W].GetCircularFarmLocation(minions);
                spells[Spells.W].Cast(farmLocation.Position);
            }
        }

        private static void Obj_AI_Hero_OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            var blockMovement = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.R.Block").IsActive();
            if (CastingR && blockMovement)
            {
                args.Process = false;
            }
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SData.Name == "XerathLocusOfPower2")
                {
                    RCombo.CastSpell = 0;
                    RCombo._index = 0;
                    RCombo._position = new Vector3();
                    RCombo._tapKey = false;
                }
                else if (args.SData.Name == "xerathlocuspulse")
                {
                    RCombo.CastSpell = Utils.TickCount;
                    RCombo._index++;
                    RCombo._position = args.End;
                    RCombo._tapKey = false;
                }
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            var utarget = TargetSelector.GetTarget(spells[Spells.R].Range, TargetSelector.DamageType.Magical);
            spells[Spells.R].Range = 2000 + spells[Spells.R].Level * 1200;

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    JungleClear();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
            }

            var showNotifications = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.misc.Notifications").GetValue<bool>();

            if (spells[Spells.R].IsReady() && showNotifications && Environment.TickCount - lastNotification > 5000)
            {
                foreach (var enemy in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(h => h.IsValidTarget() && (float)Player.GetSpellDamage(h, SpellSlot.R) * 3 > h.Health))
                {
                    ShowNotification(enemy.ChampionName + ": is killable", Color.White, 4000);
                    lastNotification = Environment.TickCount;
                }
            }

            AutoHarassMode();
            KsMode();

            if (CastingR)
            {
                CastR(utarget);
            }

            if (spells[Spells.E].IsReady())
            {
                var useE = AnonymouslyXerathMenu.Menu.Item("AnonymouslyXerath.Misc.E").GetValue<KeyBind>().Active;
                var eTarget = TargetSelector.GetTarget(spells[Spells.E].Range, TargetSelector.DamageType.Magical);

                if (useE)
                {
                    spells[Spells.E].Cast(eTarget);
                }
            }
        }

        private static void ShowNotification(string message, Color color, int duration = -1, bool dispose = true)
        {
            Notifications.AddNotification(new Notification(message, duration, dispose).SetTextColor(color));
        }

        #endregion

        private static class RCombo
        {
            #region Static Fields

            public static int _index;

            public static Vector3 _position;

            public static bool _tapKey;

            public static int CastSpell;

            #endregion
        }
    }
}

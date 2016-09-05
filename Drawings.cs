namespace AnonymouslyXerath
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Color = System.Drawing.Color;

    internal class Drawings
    {
        #region Public Methods and Operators

        public static void Drawing_OnDraw(EventArgs args)
        {
            var drawOff = ElXerathMenu.Menu.Item("AnonymouslyXerath.Draw.off").GetValue<bool>();
            var drawQ = ElXerathMenu.Menu.Item("AnonymouslyXerath.Draw.Q").GetValue<Circle>();
            var drawW = ElXerathMenu.Menu.Item("AnonymouslyXerath.Draw.W").GetValue<Circle>();
            var drawE = ElXerathMenu.Menu.Item("AnonymouslyXerath.Draw.E").GetValue<Circle>();
            var drawR = ElXerathMenu.Menu.Item("AnonymouslyXerath.Draw.R").GetValue<Circle>();
            var drawText = ElXerathMenu.Menu.Item("AnonymouslyXerath.Draw.Text").GetValue<bool>();
            var rBool = ElXerathMenu.Menu.Item("AnonymouslyXerath.AutoHarass").GetValue<KeyBind>().Active;

            if (drawOff)
            {
                return;
            }

            var playerPos = Drawing.WorldToScreen(ObjectManager.Player.Position);

            if (drawQ.Active)
            {
                if (Xerath.spells[Spells.Q].Level > 0)
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Xerath.spells[Spells.Q].Range, Color.White);
                }
            }

            if (drawW.Active)
            {
                if (Xerath.spells[Spells.W].Level > 0)
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Xerath.spells[Spells.W].Range, Color.White);
                }
            }

            if (drawE.Active)
            {
                if (Xerath.spells[Spells.E].Level > 0)
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Xerath.spells[Spells.E].Range, Color.White);
                }
            }

            if (drawR.Active)
            {
                if (Xerath.spells[Spells.R].Level > 0)
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Xerath.spells[Spells.R].Range, Color.White);
                }
            }

            if (drawText)
            {
                Drawing.DrawText(
                    playerPos.X - 70,
                    playerPos.Y + 40,
                    (rBool ? Color.Green : Color.Red),
                    "{0}",
                    (rBool ? "Auto harass enabled" : "Auto harass disabled"));
            }
        }

        #endregion
    }
}

namespace AnonymouslyXerath
{
    using System;

    using LeagueSharp.Common;

    public class AnonymouslyXerathMenu
    {
        #region Static Fields

        public static Menu Menu;

        #endregion

        #region Public Methods and Operators

        public static void Initialize()
        {
            Menu = new Menu("AnonymouslyXerath", "menu", true);

            //ElXerath.Orbwalker
            var orbwalkerMenu = new Menu("Orbwalker", "orbwalker");
            Xerath.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Menu.AddSubMenu(orbwalkerMenu);

            //ElXerath.TargetSelector
            var targetSelector = new Menu("Target Selector", "TargetSelector");
            TargetSelector.AddToMenu(targetSelector);
            Menu.AddSubMenu(targetSelector);

            var cMenu = new Menu("Combo", "Combo");
            cMenu.AddItem(new MenuItem("AnonymouslyXerath.Combo.Q", "Use Q").SetValue(true));
            cMenu.AddItem(new MenuItem("AnonymouslyXerath.Combo.W", "Use W").SetValue(true));
            cMenu.AddItem(new MenuItem("AnonymouslyXerath.Combo.E", "Use E").SetValue(true));
            cMenu.AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            Menu.AddSubMenu(cMenu);

            var rMenu = new Menu("Ult", "Ult");
            rMenu.AddItem(new MenuItem("AnonymouslyXerath.R.AutoUseR", "Auto use charges").SetValue(true));
            rMenu.AddItem(
                new MenuItem("AnonymouslyXerath.R.Mode", "Mode ").SetValue(
                    new StringList(new[] { "Normal", "Custom delays", "OnTap", "Custom hitchance", "Near mouse" })));
            rMenu.AddItem(
                new MenuItem("AnonymouslyXerath.R.OnTap", "Ult on tap").SetValue(
                    new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            rMenu.AddItem(new MenuItem("AnonymouslyXerath.R.Block", "Block movement").SetValue(true));

            rMenu.SubMenu("CustomDelay").AddItem(new MenuItem("AnonymouslyXerath.R.Delay", "Custom delays").SetValue(true));
            for (var i = 1; i <= 5; i++)
            {
                rMenu.SubMenu("CustomDelay")
                    .SubMenu("Custom delay")
                    .AddItem(new MenuItem("Delay" + i, "Delay" + i).SetValue(new Slider(0, 1500, 0)));
                    .AddItem(new MenuItem)("Delay" + i, "Delay"+ i).SetValue(new Slider(2000, 1500, 2000)));
                  
            }

            rMenu.AddItem(new MenuItem("AnonymouslyXerath.R.Radius", "Target radius").SetValue(new Slider(700, 1500, 300)));

            Menu.AddSubMenu(rMenu);

            var hMenu = new Menu("Harass", "Harass");
            hMenu.AddItem(new MenuItem("AnonymouslyXerath.Harass.Q", "Use Q").SetValue(true));
            hMenu.AddItem(new MenuItem("AnonymouslyXerath.Harass.W", "Use W").SetValue(true));
            hMenu.SubMenu("AutoHarass")
                .AddItem(
                    new MenuItem("AnonymouslyXerath.AutoHarass", "[Toggle] Auto harass", false).SetValue(
                        new KeyBind("U".ToCharArray()[0], KeyBindType.Toggle)));
            hMenu.SubMenu("AutoHarass").AddItem(new MenuItem("AnonymouslyXerath.UseQAutoHarass", "Use Q").SetValue(true));
            hMenu.SubMenu("AutoHarass").AddItem(new MenuItem("AnonymouslyXerath.UseWAutoHarass", "Use W").SetValue(true));
            hMenu.SubMenu("AutoHarass")
                .AddItem(new MenuItem("AnonymouslyXerath.harass.mana", "Auto harass mana"))
                .SetValue(new Slider(55));

            Menu.AddSubMenu(hMenu);

            var lMenu = new Menu("Clear", "LaneClear");
            lMenu.AddItem(new MenuItem("AnonymouslyXerath.clear.Q", "Use Q").SetValue(true));
            lMenu.AddItem(new MenuItem("AnonymouslyXerath.clear.W", "Use W").SetValue(true));
            lMenu.AddItem(new MenuItem("fasfsafsafsasfasfa", ""));
            lMenu.AddItem(new MenuItem("AnonymouslyXerath.jclear.Q", "Jungle Use Q").SetValue(true));
            lMenu.AddItem(new MenuItem("AnonymouslyXerath.jclear.W", "Jungle Use W").SetValue(true));
            lMenu.AddItem(new MenuItem("AnonymouslyXerath.jclear.E", "Jungle Use E").SetValue(true));
            lMenu.AddItem(new MenuItem("fasfsafsafsadsasasfasfa", ""));
            lMenu.AddItem(new MenuItem("minmanaclear", "Auto harass mana")).SetValue(new Slider(55));

            Menu.AddSubMenu(lMenu);

            //ElXerath.Misc
            var miscMenu = new Menu("Misc", "Misc");
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.Draw.off", "Turn drawings off").SetValue(false));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.Draw.Q", "Draw Q").SetValue(new Circle()));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.Draw.W", "Draw W").SetValue(new Circle()));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.Draw.E", "Draw E").SetValue(new Circle()));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.Draw.R", "Draw R").SetValue(new Circle()));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.Draw.Text", "Draw Text").SetValue(true));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.Draw.RON", "Draw R target radius").SetValue(true));
            miscMenu.AddItem(new MenuItem("useEFafsdsgdrmddsddsasfsasdsdsaadsd", ""));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.Ignite", "Use ignite").SetValue(true));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.misc.ks", "Killsteal mode").SetValue(false));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.misc.Antigapcloser", "Antigapcloser").SetValue(true));
            miscMenu.AddItem(new MenuItem("AnonymouslyXerath.misc.Notifications", "Use notifications").SetValue(true));
            miscMenu.AddItem(new MenuItem("useEdaadaDFafsdsgdrmddsddsasfsasdsdsaadsd", ""));
            miscMenu.AddItem(
                new MenuItem("AnonymouslyXerath.Misc.E", "Cast E key").SetValue(
                    new KeyBind("H".ToCharArray()[0], KeyBindType.Press)));
            miscMenu.AddItem(new MenuItem("useEdaadaDFafsddssdsgdrmddsddsasfsasdsdsaadsd", ""));
            miscMenu.AddItem(
                new MenuItem("AnonymouslyXerath.hitChance", "Hitchance Q").SetValue(
                    new StringList(new[] { "Low", "Medium", "High", "Very High" }, 3)));

            Menu.AddSubMenu(miscMenu);

            Menu.AddItem(new MenuItem("422442fsaafs4242f", ""));
            Menu.AddItem(new MenuItem("422442fsaafsf", "Version: 1.0.0.6"));
            Menu.AddItem(new MenuItem("fsasfafsfsafsa", "Made By Anonymously"));

            Menu.AddToMainMenu();

            Console.WriteLine("Menu Loaded");
        }

        #endregion
    }
}

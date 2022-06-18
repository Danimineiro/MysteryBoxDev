using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MysteryBoxFrameWork
{
    public class ItemReplacerDef : Def
    {
        public readonly ThingDef targetItem;
        public readonly List<ThingDef> producedItems = new List<ThingDef>();

        public ThingDef RandomItem => producedItems.RandomElement();

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string error in base.ConfigErrors())
            {
                yield return error;
            }

            if (targetItem is null) yield return $"ItemReplacerDef of name {defName}, doesn't list a targetItem!";
            if (producedItems.NullOrEmpty()) yield return $"ItemReplacerDef of name {defName}, doesn't list any producedItems!";
        }
    }
}

using RimWorld;
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
        public readonly List<ThingDefCountClass> producedItems = new List<ThingDefCountClass>();
        public readonly List<ThingDefCountClass> allowedStuffs = new List<ThingDefCountClass>();
        public readonly QualityGenerator qualityGenerator = QualityGenerator.Gift;

        public Thing GenRandomThing
        {
            get
            {
                ThingDef randomDef = producedItems.RandomElementByWeight((countClass) => countClass.count).thingDef;
                ThingDef stuff = randomDef.MadeFromStuff ? GetRandomStuffFor(randomDef) : null;

                if (randomDef.MadeFromStuff && stuff is null)
                {
                    string errorString = $"Couldn't generate random stuff for thing: {randomDef.LabelCap} from list of allowedStuffs in ItemReplacerDef of name {defName}!";
                    Log.ErrorOnce(errorString, errorString.GetHashCode());
                }

                Thing thing = ThingMaker.MakeThing(randomDef, stuff);
                
                if (thing.TryGetComp<CompQuality>() is CompQuality quality)
                {
                    quality.SetQuality(QualityUtility.GenerateQuality(qualityGenerator), ArtGenerationContext.Outsider);
                }

                return thing;
            }
        }

        private ThingDef GetRandomStuffFor(ThingDef thingDef) => allowedStuffs.Where((thingDefCountClass) => thingDefCountClass.thingDef.stuffProps.categories.Any((cat) => thingDef.stuffCategories.Contains(cat))).RandomElementByWeight((countClass) => countClass.count).thingDef;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string error in base.ConfigErrors())
            {
                yield return error;
            }

            if (targetItem is null) yield return $"ItemReplacerDef of name {defName}, doesn't list a targetItem!";
            if (producedItems.NullOrEmpty()) yield return $"ItemReplacerDef of name {defName}, doesn't list any producedItems!";
            foreach(ThingDefCountClass countClass in allowedStuffs)
            {
                if (!countClass.thingDef.IsStuff)
                {
                    yield return $"ItemReplacerDef of name {defName} has {countClass.thingDef.defName} in allowedStuffs, but {countClass.thingDef.defName} is not stuff!";
                }
            }
        }
    }
}

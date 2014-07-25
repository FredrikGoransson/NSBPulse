using System;
using NServiceBus;

namespace NSBPulseTestMessages
{
    [Serializable]
    public class SomeDomainRelatedMessage : IMessage
    {
        public string Message { get; set; }

        public SomeDomainRelatedMessage()
        {
            Message = GenerateRandomStatements.SaySomething();
        }
    }

    public static class GenerateRandomStatements
    {
        private static readonly Random Random;

        static GenerateRandomStatements()
        {
            Random = new Random(Environment.TickCount);
        }
        public static string SaySomething()
        {
            var pIndex = Random.Next(0, People.Length);
            var person = People[pIndex];

            var sIndex = Random.Next(0, StuffToSay.Length);
            var stuffToSay = StuffToSay[sIndex];

            return string.Format("{0} says '{1}'", person, stuffToSay);
        }

        private static readonly string[] People = new[]
        {
            "Arya Stark",
            "Bran Stark",
            "Brienne of Tarth",
            "Bronn",
            "Catelyn Stark",
            "Cersei Lannister",
            "Daario Naharis",
            "Daario Naharis",
            "Daenerys Targaryen",
            "Davos Seaworth",
            "Gendry",
            "Gilly",
            "Jeor Mormont",
            "Joffrey Baratheon",
            "Jon Snow",
            "Jorah Mormont",
            "Khal Drogo",
            "Margaery Tyrell",
            "Melisandre",
            "Petyr Baelish",
            "Ramsay Bolton",
            "Robert Baratheon",
            "Samwell Tarly",
            "Theon Greyjoy",
            "Tormund Giantsbane",
            "Tywin Lannister",
            "Varys",
            "Viserys Targaryen"
        };

        private static readonly string[] StuffToSay = new[]
        {
            "A Taste of Glory",
            "Above the Rest",
            "As High as Honor",
            "Awake! Awake!",
            "Behold Our Bounty",
            "Beware Our Sting",
            "Brave and Beautiful",
            "Burning Bright",
            "Come Try Me",
            "Death Before Disgrace",
            "Ever Vigilant",
            "Family, Duty, Honor",
            "Fire and Blood",
            "First in Battle",
            "Fly High, Fly Far",
            "For All Seasons",
            "From These Beginnings",
            "Growing Strong",
            "Hear Me Roar!",
            "Here We Stand",
            "Honed and Ready",
            "Honor, not Honors",
            "I Have No Rival",
            "Let It Be Written",
            "Let Me Soar",
            "Light in Darkness",
            "Never Resting",
            "No Foe May Pass",
            "No Song So Sweet",
            "None So Dutiful",
            "None so Fierce",
            "None so Wise",
            "Our Blades Are Sharp",
            "Our Roots Go Deep",
            "Our Sun Shines Bright",
            "Ours is the Fury",
            "Pride and Purpose",
            "Proud and Free",
            "Proud to Be Faithful",
            "Right Conquers Might",
            "Righteous in Wrath",
            "Rouse Me Not",
            "Set Down Our Deeds",
            "So End Our Foes",
            "Sound the Charge",
            "The Choice Is Yours",
            "The Old, the True, the Brave",
            "The Sun of Winter",
            "Though All Men Do Despise Us",
            "Touch Me Not",
            "Tread Lightly Here",
            "True to the Mark",
            "Truth Conquers",
            "Unbowed, Unbent, Unbroken",
            "Unflinching",
            "We Do Not Sow",
            "We Guard the Way",
            "We Light the Way",
            "We Remember",
            "Winter is Coming",
            "Wisdom and Strength",
            "Work Her Will"
        };
    }
}

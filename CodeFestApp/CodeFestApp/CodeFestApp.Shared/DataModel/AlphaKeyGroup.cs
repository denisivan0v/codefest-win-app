using System;
using System.Collections.Generic;
using System.Linq;

using Windows.Globalization.Collation;

namespace CodeFestApp.DataModel
{
    public class AlphaKeyGroup<T>
    {
        private const string GlobeGroupKey = "\uD83C\uDF10";

        public AlphaKeyGroup(string key)
        {
            Key = key;
            Items = new List<T>();
        }

        public string Key { get; private set; }
        public List<T> Items { get; private set; }

        public static IEnumerable<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items,
                                                                 Func<T, string> keySelector,
                                                                 bool sort)
        {
            var characterGroupings = new CharacterGroupings();
            var list = CreateDefaultGroups(characterGroupings);

            foreach (var item in items)
            {
                var label = characterGroupings.Lookup(keySelector(item));
                var index = list.FindIndex(alphakeygroup => alphakeygroup.Key.Equals(label, StringComparison.CurrentCulture));

                if (index >= 0 && index < list.Count)
                {
                    list[index].Items.Add(item);
                }
            }

            if (sort)
            {
                foreach (var group in list)
                {
                    group.Items.Sort((c0, c1) => keySelector(c0).CompareTo(keySelector(c1)));
                }
            }

            return list;
        }

        private static List<AlphaKeyGroup<T>> CreateDefaultGroups(IEnumerable<CharacterGrouping> characterGroupings)
        {
            return (from grouping in characterGroupings
                    where grouping.Label != string.Empty
                    select grouping.Label == "..."
                               ? new AlphaKeyGroup<T>(GlobeGroupKey)
                               : new AlphaKeyGroup<T>(grouping.Label))
                .ToList();
        }
    }
}

using System.Collections.Generic;

namespace System.Linq
{
    public static class IEnumerableExtensions
    {
        public static void MergeWith<TTargetItem, TSourceItem, TId>(this IEnumerable<TTargetItem> targetCollection, 
                                                         IEnumerable<TSourceItem> sourceCollection,
                                                         Func<TTargetItem, TId> targetItemIdSelector,
                                                         Func<TSourceItem, TId> sourceItemIdSelector,
                                                         Action<TSourceItem> onAdd,
                                                         Action<TTargetItem, TSourceItem> onUpdate,
                                                         Action<TTargetItem> onDelete
                                                 )
        {
            if (sourceCollection == null)
            {
                return;
            }

            Dictionary<TId, Match<TTargetItem, TSourceItem>> matches = targetCollection.ToDictionary(targetItemIdSelector, x => new Match<TTargetItem, TSourceItem>(x, default(TSourceItem), ActionType.Delete));
            List<TSourceItem> itemsToAdd = new List<TSourceItem>();

            foreach (TSourceItem sourceItem in sourceCollection)
            {
                TId sourceItemId = sourceItemIdSelector(sourceItem);
                if (matches.TryGetValue(sourceItemId, out var match))
                {                    
                    match.actionToTake = ActionType.Update;
                    match.sourceItem = sourceItem;
                }
                else
                {
                    itemsToAdd.Add(sourceItem);
                }
            }

            matches.Values.Where(x => x.actionToTake == ActionType.Delete).ForEach(x => onDelete(x.targetItem));
            itemsToAdd.ForEach(x => onAdd(x));
            matches.Values.Where(x => x.actionToTake == ActionType.Update).ForEach(x => onUpdate(x.targetItem, x.sourceItem)); 
        }

        public static void ForEach<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            foreach(TItem item in items)
            {
                action(item);
            }
        }


        private enum ActionType
        {           
            Delete,
            Update,           
        }
        private class Match<TTargetItem, TSourceItem>
        {
            public TTargetItem targetItem;
            public TSourceItem sourceItem;
            public ActionType actionToTake;

            public Match(TTargetItem targetItem, TSourceItem sourceItem, ActionType status)
            {
                this.targetItem = targetItem;
                this.sourceItem = sourceItem;
                this.actionToTake = status;
            }
        }
    }
}

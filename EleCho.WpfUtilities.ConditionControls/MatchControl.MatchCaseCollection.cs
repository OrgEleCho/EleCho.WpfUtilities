using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EleCho.WpfUtilities.ConditionControls
{

    public partial class MatchControl
    {
        public sealed class MatchCaseCollection : Collection<MatchCase>
        {
            public MatchCaseCollection(MatchControl owner)
            {
                Owner = owner;
            }

            public MatchControl Owner { get; }

            public MatchCase? MatchedCase => this.FirstOrDefault(c => Equals(Owner.Value, c.Value));

            protected override void InsertItem(int index, MatchCase item)
            {
                ConnectCase(item);
                base.InsertItem(index, item);

                OnCollectionChanged();
            }

            protected override void ClearItems()
            {
                foreach (var _case in this)
                    DisconnectCase(_case);

                base.ClearItems();

                OnCollectionChanged();
            }

            protected override void RemoveItem(int index)
            {
                DisconnectCase(this[index]);
                base.RemoveItem(index);

                OnCollectionChanged();
            }

            protected override void SetItem(int index, MatchCase item)
            {
                var origin = this[index];
                DisconnectCase(origin);
                ConnectCase(item);

                base.SetItem(index, item);

                OnCollectionChanged();
            }

            void ConnectCase(MatchCase switchCase)
            {
                switchCase.Owner = Owner;
            }

            void DisconnectCase(MatchCase switchCase)
            {
                switchCase.Owner = null;
            }

            void OnCollectionChanged()
            {
                MatchControl.UpdateMatchControl(Owner, default);
            }
        }
    }
}

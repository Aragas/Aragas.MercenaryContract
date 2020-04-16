using System;

using TaleWorlds.CampaignSystem;

namespace Aragas.CampaignSystem
{
    public class AragasMbEvent : IMbEvent
	{
        public void AddNonSerializedListener(object owner, Action action)
		{
			var eventHandlerRec = new EventHandlerRec(owner, action);
			var nonSerializedListenerList = _nonSerializedListenerList;
			_nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		internal void Invoke()
		{
			InvokeList(_nonSerializedListenerList);
		}

		private void InvokeList(EventHandlerRec list)
		{
			while (list != null)
			{
				list.Action();
				list = list.Next;
			}
		}

		public void ClearListeners(object o)
		{
			ClearListenerOfList(ref _nonSerializedListenerList, o);
		}

		private void ClearListenerOfList(ref EventHandlerRec list, object o)
		{
			var eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			var eventHandlerRec2 = list;
			if (eventHandlerRec2 == eventHandlerRec)
			{
				list = eventHandlerRec2.Next;
				return;
			}
			while (eventHandlerRec2 != null)
			{
				if (eventHandlerRec2.Next == eventHandlerRec)
				{
					eventHandlerRec2.Next = eventHandlerRec.Next;
				}
				else
				{
					eventHandlerRec2 = eventHandlerRec2.Next;
				}
			}
		}

		private EventHandlerRec _nonSerializedListenerList;

		internal class EventHandlerRec
		{
            internal Action Action { get; private set; }

			internal object Owner { get; private set; }

			public EventHandlerRec(object owner, Action action)
			{
				Action = action;
				Owner = owner;
			}

			public EventHandlerRec Next;
		}
	}

	internal class AragasMbEvent<T> : IMbEvent<T>
	{
        public void AddNonSerializedListener(object owner, Action<T> action)
		{
            var eventHandlerRec = new EventHandlerRec<T>(owner, action);
            var nonSerializedListenerList = _nonSerializedListenerList;
			_nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		internal void Invoke(T t)
		{
			InvokeList(_nonSerializedListenerList, t);
		}

		private void InvokeList(EventHandlerRec<T> list, T t)
		{
			while (list != null)
			{
				list.Action(t);
				list = list.Next;
			}
		}

		public void ClearListeners(object o)
		{
			ClearListenerOfList(ref _nonSerializedListenerList, o);
		}

		private void ClearListenerOfList(ref EventHandlerRec<T> list, object o)
		{
			var eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			var eventHandlerRec2 = list;
			if (eventHandlerRec2 == eventHandlerRec)
			{
				list = eventHandlerRec2.Next;
				return;
			}
			while (eventHandlerRec2 != null)
			{
				if (eventHandlerRec2.Next == eventHandlerRec)
				{
					eventHandlerRec2.Next = eventHandlerRec.Next;
				}
				else
				{
					eventHandlerRec2 = eventHandlerRec2.Next;
				}
			}
		}

		private EventHandlerRec<T> _nonSerializedListenerList;

		internal class EventHandlerRec<TS>
		{
            internal Action<TS> Action { get; private set; }

			internal object Owner { get; private set; }

			public EventHandlerRec(object owner, Action<TS> action)
			{
				Action = action;
				Owner = owner;
			}

			public EventHandlerRec<TS> Next;
		}
	}

    internal class AragasMbEvent<T1, T2> : IMbEvent<T1, T2>
	{
        public void AddNonSerializedListener(object owner, Action<T1, T2> action)
		{
			var eventHandlerRec = new EventHandlerRec<T1, T2>(owner, action);
			var nonSerializedListenerList = _nonSerializedListenerList;
			_nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		internal void Invoke(T1 t1, T2 t2)
		{
			InvokeList(_nonSerializedListenerList, t1, t2);
		}

		private void InvokeList(EventHandlerRec<T1, T2> list, T1 t1, T2 t2)
		{
			while (list != null)
			{
				list.Action(t1, t2);
				list = list.Next;
			}
		}

		public void ClearListeners(object o)
		{
			ClearListenerOfList(ref _nonSerializedListenerList, o);
		}

		private void ClearListenerOfList(ref EventHandlerRec<T1, T2> list, object o)
		{
			var eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			var eventHandlerRec2 = list;
			if (eventHandlerRec2 == eventHandlerRec)
			{
				list = eventHandlerRec2.Next;
				return;
			}
			while (eventHandlerRec2 != null)
			{
				if (eventHandlerRec2.Next == eventHandlerRec)
				{
					eventHandlerRec2.Next = eventHandlerRec.Next;
				}
				else
				{
					eventHandlerRec2 = eventHandlerRec2.Next;
				}
			}
		}

		private EventHandlerRec<T1, T2> _nonSerializedListenerList;

		internal class EventHandlerRec<TS, TQ>
		{
            internal Action<TS, TQ> Action { get; private set; }

			internal object Owner { get; private set; }

			public EventHandlerRec(object owner, Action<TS, TQ> action)
			{
				Action = action;
				Owner = owner;
			}

			public EventHandlerRec<TS, TQ> Next;
		}
	}

    internal class AragasMbEvent<T1, T2, T3> : IMbEvent<T1, T2, T3>
	{
        public void AddNonSerializedListener(object owner, Action<T1, T2, T3> action)
		{
			var eventHandlerRec = new EventHandlerRec<T1, T2, T3>(owner, action);
			var nonSerializedListenerList = _nonSerializedListenerList;
			_nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		internal void Invoke(T1 t1, T2 t2, T3 t3)
		{
			InvokeList(_nonSerializedListenerList, t1, t2, t3);
		}

		private void InvokeList(EventHandlerRec<T1, T2, T3> list, T1 t1, T2 t2, T3 t3)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3);
				list = list.Next;
			}
		}

		public void ClearListeners(object o)
		{
			ClearListenerOfList(ref _nonSerializedListenerList, o);
		}

		private void ClearListenerOfList(ref EventHandlerRec<T1, T2, T3> list, object o)
		{
			var eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			var eventHandlerRec2 = list;
			if (eventHandlerRec2 == eventHandlerRec)
			{
				list = eventHandlerRec2.Next;
				return;
			}
			while (eventHandlerRec2 != null)
			{
				if (eventHandlerRec2.Next == eventHandlerRec)
				{
					eventHandlerRec2.Next = eventHandlerRec.Next;
				}
				else
				{
					eventHandlerRec2 = eventHandlerRec2.Next;
				}
			}
		}

		private EventHandlerRec<T1, T2, T3> _nonSerializedListenerList;

		internal class EventHandlerRec<TS, TQ, TR>
		{
            internal Action<TS, TQ, TR> Action { get; private set; }

			internal object Owner { get; private set; }

			public EventHandlerRec(object owner, Action<TS, TQ, TR> action)
			{
				Action = action;
				Owner = owner;
			}

			public EventHandlerRec<TS, TQ, TR> Next;
		}
	}
}